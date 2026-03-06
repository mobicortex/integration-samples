using System.Net;
using System.Text;
using System.Text.Json;
using MobiCortex.Sdk.Interfaces;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// Implementação de servidor HTTP para receber webhooks usando HttpListener.
    /// </summary>
    /// <remarks>
    /// ⚠️ AVISO: Esta é uma implementação de REFERÊNCIA para desenvolvimento/testes.
    /// Não foi testada para alta carga. Para produção com muitos dispositivos,
    /// use soluções profissionais como ASP.NET Core, AWS API Gateway, Azure Functions, etc.
    /// </remarks>
    public class WebhookServerService : IWebhookServerService, IDisposable
    {
        private HttpListener? _listener;
        private CancellationTokenSource? _cts;
        private Task? _processingTask;
        private readonly List<WebhookReceivedEventArgs> _history = new();
        private readonly object _historyLock = new object();
        private long _totalRequests = 0;
        private long _successRequests = 0;
        private long _errorRequests = 0;
        private DateTime _startedAt;
        private bool _disposed;
        private string? _authToken;

        /// <inheritdoc/>
        public bool IsRunning => _listener?.IsListening ?? false;

        /// <inheritdoc/>
        public int Port { get; private set; } = 8080;

        /// <inheritdoc/>
        public string BaseUrl => $"http://localhost:{Port}";

        /// <inheritdoc/>
        public event EventHandler<WebhookReceivedEventArgs>? WebhookReceived;

        /// <inheritdoc/>
        public event EventHandler<WebhookLogEventArgs>? LogReceived;

        /// <inheritdoc/>
        public async Task<bool> StartAsync(int port = 8080, string? authToken = null)
        {
            if (_listener?.IsListening == true)
            {
                await StopAsync();
            }

            try
            {
                Port = port;
                _authToken = authToken;
                _cts = new CancellationTokenSource();
                _listener = new HttpListener();
                
                // Adicionar prefixos para ouvir em todas as interfaces
                _listener.Prefixes.Add($"http://+:{port}/");
                _listener.Prefixes.Add($"http://localhost:{port}/");
                _listener.Prefixes.Add($"http://*:{port}/");
                
                _listener.Start();
                _startedAt = DateTime.Now;
                _totalRequests = 0;
                _successRequests = 0;
                _errorRequests = 0;

                // Iniciar processamento em background
                _processingTask = Task.Run(() => ProcessRequestsAsync(_cts.Token));

                Log(LogLevel.Info, $"Servidor webhook iniciado em {BaseUrl}");
                return true;
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 5)
            {
                // Acesso negado - precisa de permissões de administrador
                Log(LogLevel.Error, "Erro: Permissão negada. Execute como Administrador ou use: netsh http add urlacl url=http://+:{port}/ user=SEU_USUARIO");
                return false;
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"Erro ao iniciar servidor: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync()
        {
            if (_listener == null) return;

            try
            {
                _cts?.Cancel();
                _listener.Stop();
                _listener.Close();
                
                if (_processingTask != null)
                {
                    try { await _processingTask.WaitAsync(TimeSpan.FromSeconds(5)); }
                    catch { /* ignora */ }
                }
                
                Log(LogLevel.Info, "Servidor webhook parado");
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"Erro ao parar servidor: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<WebhookReceivedEventArgs> GetHistory()
        {
            lock (_historyLock)
            {
                return _history.OrderByDescending(h => h.ReceivedAt).ToList();
            }
        }

        /// <inheritdoc/>
        public void ClearHistory()
        {
            lock (_historyLock)
            {
                _history.Clear();
            }
            Log(LogLevel.Info, "Histórico limpo");
        }

        /// <inheritdoc/>
        public WebhookServerStats GetStats()
        {
            return new WebhookServerStats
            {
                IsRunning = IsRunning,
                Port = Port,
                BaseUrl = BaseUrl,
                TotalRequestsReceived = _totalRequests,
                TotalRequestsSuccess = _successRequests,
                TotalRequestsError = _errorRequests,
                StartedAt = _startedAt
            };
        }

        private async Task ProcessRequestsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && _listener?.IsListening == true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequestAsync(context), ct);
                }
                catch (HttpListenerException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, $"Erro ao receber requisição: {ex.Message}");
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            Interlocked.Increment(ref _totalRequests);
            var request = context.Request;
            var response = context.Response;
            WebhookReceivedEventArgs? webhookArgs = null;

            try
            {
                // Validar autenticação se configurada
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var authHeader = request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ") || 
                        authHeader.Substring(7) != _authToken)
                    {
                        response.StatusCode = 401;
                        await WriteResponseAsync(response, "{\"error\":\"Unauthorized\"}");
                        Interlocked.Increment(ref _errorRequests);
                        return;
                    }
                }

                // Ler body
                string body;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    body = await reader.ReadToEndAsync();
                }

                // Coletar headers
                var headers = new Dictionary<string, string>();
                foreach (string key in request.Headers.AllKeys)
                {
                    if (key != null)
                        headers[key] = request.Headers[key] ?? "";
                }

                // Criar objeto do evento
                webhookArgs = new WebhookReceivedEventArgs
                {
                    RemoteIp = request.RemoteEndPoint?.Address?.ToString() ?? "unknown",
                    Method = request.HttpMethod,
                    Path = request.Url?.PathAndQuery ?? "/",
                    Headers = headers,
                    Body = body,
                    ContentType = request.ContentType ?? "application/json",
                    ResponseStatusCode = 200
                };

                // Responder com sucesso
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await WriteResponseAsync(response, "{\"status\":\"ok\",\"received\":true}");

                Interlocked.Increment(ref _successRequests);

                // Salvar no histórico
                lock (_historyLock)
                {
                    _history.Add(webhookArgs);
                    // Limitar histórico a 1000 itens
                    if (_history.Count > 1000)
                        _history.RemoveAt(0);
                }

                // Disparar evento
                WebhookReceived?.Invoke(this, webhookArgs);

                Log(LogLevel.Info, $"Webhook recebido: {request.HttpMethod} {request.Url?.PathAndQuery} de {webhookArgs.RemoteIp}");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _errorRequests);
                try
                {
                    response.StatusCode = 500;
                    await WriteResponseAsync(response, $"{{\"error\":\"{ex.Message}\"}}");
                }
                catch { /* ignora */ }

                Log(LogLevel.Error, $"Erro ao processar webhook: {ex.Message}");
            }
        }

        private async Task WriteResponseAsync(HttpListenerResponse response, string content)
        {
            var buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer);
            response.OutputStream.Close();
        }

        private void Log(LogLevel level, string message)
        {
            LogReceived?.Invoke(this, new WebhookLogEventArgs
            {
                Level = level,
                Message = message,
                Timestamp = DateTime.Now
            });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                StopAsync().GetAwaiter().GetResult();
                _listener?.Close();
                _cts?.Dispose();
                _disposed = true;
            }
        }
    }
}
