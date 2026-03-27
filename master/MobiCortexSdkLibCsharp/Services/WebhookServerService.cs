using System.Net;
using System.Text;
using System.Text.Json;
using MobiCortex.Sdk.Interfaces;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// HTTP server implementation for receiving webhooks using HttpListener.
    /// </summary>
    /// <remarks>
    /// WARNING: This is a REFERENCE implementation for development/testing.
    /// It has not been tested for high load. For production with many devices,
    /// use professional solutions such as ASP.NET Core, AWS API Gateway, Azure Functions, etc.
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
                
                // Add prefixes to listen on all interfaces
                _listener.Prefixes.Add($"http://+:{port}/");
                _listener.Prefixes.Add($"http://localhost:{port}/");
                _listener.Prefixes.Add($"http://*:{port}/");
                
                _listener.Start();
                _startedAt = DateTime.Now;
                _totalRequests = 0;
                _successRequests = 0;
                _errorRequests = 0;

                // Start background processing
                _processingTask = Task.Run(() => ProcessRequestsAsync(_cts.Token));

                Log(LogLevel.Info, $"Webhook server started at {BaseUrl}");
                return true;
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 5)
            {
                // Access denied - requires administrator permissions
                Log(LogLevel.Error, $"Error: Permission denied. Run as Administrator or use: netsh http add urlacl url=http://+:{port}/ user=YOUR_USER");
                return false;
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"Error starting server: {ex.Message}");
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
#if NET8_0_OR_GREATER
                    try { await _processingTask.WaitAsync(TimeSpan.FromSeconds(5)); }
#else
                    try { await Task.WhenAny(_processingTask, Task.Delay(TimeSpan.FromSeconds(5))); }
#endif
                    catch { /* ignore */ }
                }

                Log(LogLevel.Info, "Webhook server stopped");
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"Error stopping server: {ex.Message}");
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
            Log(LogLevel.Info, "History cleared");
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
                    Log(LogLevel.Error, $"Error receiving request: {ex.Message}");
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
                // Validate authentication if configured
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

                // Read body
                string body;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    body = await reader.ReadToEndAsync();
                }

                // Collect headers
                var headers = new Dictionary<string, string>();
                foreach (string key in request.Headers.AllKeys)
                {
                    if (key != null)
                        headers[key] = request.Headers[key] ?? "";
                }

                // Create event object
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

                // Respond with success
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await WriteResponseAsync(response, "{\"status\":\"ok\",\"received\":true}");

                Interlocked.Increment(ref _successRequests);

                // Save to history
                lock (_historyLock)
                {
                    _history.Add(webhookArgs);
                    // Limit history to 1000 items
                    if (_history.Count > 1000)
                        _history.RemoveAt(0);
                }

                // Fire event
                WebhookReceived?.Invoke(this, webhookArgs);

                Log(LogLevel.Info, $"Webhook received: {request.HttpMethod} {request.Url?.PathAndQuery} from {webhookArgs.RemoteIp}");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _errorRequests);
                try
                {
                    response.StatusCode = 500;
                    await WriteResponseAsync(response, $"{{\"error\":\"{ex.Message}\"}}");
                }
                catch { /* ignore */ }

                Log(LogLevel.Error, $"Error processing webhook: {ex.Message}");
            }
        }

        private async Task WriteResponseAsync(HttpListenerResponse response, string content)
        {
            var buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
#if NET8_0_OR_GREATER
            await response.OutputStream.WriteAsync(buffer);
#else
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
#endif
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
