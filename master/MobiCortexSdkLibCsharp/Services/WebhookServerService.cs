using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using MobiCortex.Sdk.Interfaces;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// HTTP server implementation for receiving webhooks using TcpListener (sockets).
    /// Does not use HttpListener/HTTP.sys, so it does not require Administrator or URL ACL.
    /// </summary>
    /// <remarks>
    /// WARNING: This is a REFERENCE implementation for development/testing.
    /// It has not been tested for high load. For production with many devices,
    /// use professional solutions such as ASP.NET Core, AWS API Gateway, Azure Functions, etc.
    /// </remarks>
    public class WebhookServerService : IWebhookServerService, IDisposable
    {
        private const int MaxHeaderBytes = 64 * 1024;
        private const int MaxBodyBytes = 10 * 1024 * 1024;

        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
        private Task? _processingTask;
        private readonly List<WebhookReceivedEventArgs> _history = new();
        private readonly object _historyLock = new object();
        private long _totalRequests = 0;
        private long _successRequests = 0;
        private long _errorRequests = 0;
        private DateTime _startedAt;
        private bool _disposed;
        private volatile bool _running;
        private string? _authToken;

        /// <inheritdoc/>
        public bool IsRunning => _running && _listener != null;

        /// <inheritdoc/>
        public int Port { get; private set; } = 8080;

        /// <inheritdoc/>
        public string BaseUrl
        {
            get
            {
                var lan = GetLanIpv4Addresses();
                var host = lan.Count > 0 ? lan[0] : "0.0.0.0";
                return $"http://{host}:{Port}";
            }
        }

        /// <summary>
        /// Webhook URLs on up LAN IPv4 addresses (what the controller should call).
        /// </summary>
        public static IReadOnlyList<string> GetLanWebhookUrls(int port)
        {
            return GetLanIpv4Addresses()
                .Select(ip => $"http://{ip}:{port}/webhook")
                .ToList();
        }

        /// <inheritdoc/>
        public event EventHandler<WebhookReceivedEventArgs>? WebhookReceived;

        /// <inheritdoc/>
        public event EventHandler<WebhookLogEventArgs>? LogReceived;

        /// <inheritdoc/>
        public async Task<bool> StartAsync(int port = 8080, string? authToken = null)
        {
            if (_running)
            {
                await StopAsync();
            }

            try
            {
                Port = port;
                _authToken = authToken;
                _cts = new CancellationTokenSource();

                // 0.0.0.0 = all IPv4 interfaces (LAN). Sockets, not HTTP.sys: no admin / urlacl.
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _running = true;
                _startedAt = DateTime.Now;
                _totalRequests = 0;
                _successRequests = 0;
                _errorRequests = 0;

                _processingTask = Task.Run(() => AcceptLoopAsync(_cts.Token));

                Log(LogLevel.Info, $"Webhook server listening on 0.0.0.0:{port} (all interfaces, reachable from the LAN)");
                var lanUrls = GetLanWebhookUrls(port);
                if (lanUrls.Count == 0)
                    Log(LogLevel.Warning, "No LAN IPv4 found. Check the network adapter.");
                foreach (var url in lanUrls)
                    Log(LogLevel.Info, $"Controller URL: {url}");
                return true;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                CleanupFailedStart();
                Log(LogLevel.Error, $"Error: port {port} is already in use.");
                return false;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AccessDenied)
            {
                CleanupFailedStart();
                Log(LogLevel.Error, $"Error: the OS refused port {port} (excluded port range). Try another port.");
                return false;
            }
            catch (Exception ex)
            {
                CleanupFailedStart();
                Log(LogLevel.Error, $"Error starting server: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync()
        {
            if (_listener == null && !_running) return;

            try
            {
                _running = false;
                _cts?.Cancel();
                try { _listener?.Stop(); } catch { /* ignore */ }

                if (_processingTask != null)
                {
#if NET8_0_OR_GREATER
                    try { await _processingTask.WaitAsync(TimeSpan.FromSeconds(5)); }
#else
                    try { await Task.WhenAny(_processingTask, Task.Delay(TimeSpan.FromSeconds(5))); }
#endif
                    catch { /* ignore */ }
                }

                _listener = null;
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

        private async Task AcceptLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && _listener != null)
            {
                try
                {
#if NET8_0_OR_GREATER
                    var client = await _listener.AcceptTcpClientAsync(ct);
#else
                    var client = await _listener.AcceptTcpClientAsync();
#endif
                    _ = Task.Run(() => HandleClientAsync(client, ct), ct);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (!ct.IsCancellationRequested)
                        Log(LogLevel.Error, $"Error accepting connection: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
        {
            using (client)
            {
                try
                {
                    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(30));
                    var stream = client.GetStream();
                    var remoteIp = (client.Client.RemoteEndPoint as IPEndPoint)?.Address?.ToString() ?? "unknown";
                    var request = await ReadHttpRequestAsync(stream, remoteIp, timeoutCts.Token);
                    if (request == null)
                    {
                        await WriteHttpResponseAsync(stream, 400, "{\"error\":\"Bad Request\"}", timeoutCts.Token);
                        Interlocked.Increment(ref _errorRequests);
                        return;
                    }

                    await ProcessRequestAsync(stream, request, timeoutCts.Token);
                }
                catch (OperationCanceledException) when (!ct.IsCancellationRequested)
                {
                    Interlocked.Increment(ref _errorRequests);
                }
                catch (Exception ex)
                {
                    Interlocked.Increment(ref _errorRequests);
                    Log(LogLevel.Error, $"Error processing webhook: {ex.Message}");
                }
            }
        }

        private async Task ProcessRequestAsync(NetworkStream stream, IncomingHttpRequest request, CancellationToken ct)
        {
            Interlocked.Increment(ref _totalRequests);

            try
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    request.Headers.TryGetValue("Authorization", out var authHeader);
                    if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ") ||
                        authHeader.Substring(7) != _authToken)
                    {
                        await WriteHttpResponseAsync(stream, 401, "{\"error\":\"Unauthorized\"}", ct);
                        Interlocked.Increment(ref _errorRequests);
                        return;
                    }
                }

                var webhookArgs = new WebhookReceivedEventArgs
                {
                    RemoteIp = request.RemoteIp,
                    Method = request.Method,
                    Path = request.Path,
                    Headers = request.Headers,
                    Body = request.Body,
                    ContentType = string.IsNullOrEmpty(request.ContentType) ? "application/json" : request.ContentType,
                    ResponseStatusCode = 200
                };

                await WriteHttpResponseAsync(stream, 200, "{\"status\":\"ok\",\"received\":true}", ct);
                Interlocked.Increment(ref _successRequests);

                lock (_historyLock)
                {
                    _history.Add(webhookArgs);
                    if (_history.Count > 1000)
                        _history.RemoveAt(0);
                }

                WebhookReceived?.Invoke(this, webhookArgs);
                Log(LogLevel.Info, $"Webhook received: {request.Method} {request.Path} from {webhookArgs.RemoteIp}");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _errorRequests);
                try
                {
                    await WriteHttpResponseAsync(stream, 500, $"{{\"error\":\"{ex.Message}\"}}", ct);
                }
                catch { /* ignore */ }

                Log(LogLevel.Error, $"Error processing webhook: {ex.Message}");
            }
        }

        private static async Task<IncomingHttpRequest?> ReadHttpRequestAsync(NetworkStream stream, string remoteIp, CancellationToken ct)
        {
            var raw = new MemoryStream();
            var buf = new byte[4096];
            var headerEnd = -1;

            while (raw.Length < MaxHeaderBytes)
            {
                var n = await ReadAsync(stream, buf, 0, buf.Length, ct);
                if (n <= 0)
                    break;
                raw.Write(buf, 0, n);
                headerEnd = IndexOfHeaderEnd(raw);
                if (headerEnd >= 0)
                    break;
            }

            if (headerEnd < 0)
                return null;

            var all = raw.ToArray();
            var headerText = Encoding.ASCII.GetString(all, 0, headerEnd);
            var leftoverCount = all.Length - headerEnd;

            var lines = headerText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines.Length == 0)
                return null;

            var parts = lines[0].Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return null;

            var req = new IncomingHttpRequest
            {
                Method = parts[0],
                Path = parts[1],
                RemoteIp = remoteIp
            };

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrEmpty(line))
                    continue;
                var colon = line.IndexOf(':');
                if (colon <= 0)
                    continue;
                req.Headers[line.Substring(0, colon).Trim()] = line.Substring(colon + 1).Trim();
            }

            if (req.Headers.TryGetValue("Content-Type", out var contentType))
                req.ContentType = contentType;

            var contentLength = 0;
            if (req.Headers.TryGetValue("Content-Length", out var clText) &&
                int.TryParse(clText, out var parsedLength))
            {
                contentLength = parsedLength;
            }

            if (contentLength < 0 || contentLength > MaxBodyBytes)
                return null;

            if (req.Headers.TryGetValue("Expect", out var expect) &&
                expect.IndexOf("100-continue", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                await WriteAllAsync(stream, Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n"), ct);
            }

            var body = new byte[contentLength];
            var copied = Math.Min(leftoverCount, contentLength);
            if (copied > 0)
                Buffer.BlockCopy(all, headerEnd, body, 0, copied);

            var offset = copied;
            while (offset < contentLength)
            {
                var n = await ReadAsync(stream, body, offset, contentLength - offset, ct);
                if (n <= 0)
                    break;
                offset += n;
            }

            req.Body = Encoding.UTF8.GetString(body, 0, offset);
            return req;
        }

        private static int IndexOfHeaderEnd(MemoryStream raw)
        {
            var data = raw.GetBuffer();
            var len = (int)raw.Length;
            for (var i = 0; i <= len - 4; i++)
            {
                if (data[i] == (byte)'\r' && data[i + 1] == (byte)'\n' &&
                    data[i + 2] == (byte)'\r' && data[i + 3] == (byte)'\n')
                    return i + 4;
            }

            for (var i = 0; i <= len - 2; i++)
            {
                if (data[i] == (byte)'\n' && data[i + 1] == (byte)'\n')
                    return i + 2;
            }

            return -1;
        }

        private static async Task WriteHttpResponseAsync(NetworkStream stream, int status, string json, CancellationToken ct)
        {
            var reason = status switch
            {
                200 => "OK",
                400 => "Bad Request",
                401 => "Unauthorized",
                500 => "Internal Server Error",
                _ => "OK"
            };
            var body = Encoding.UTF8.GetBytes(json);
            var header = Encoding.ASCII.GetBytes(
                $"HTTP/1.1 {status} {reason}\r\n" +
                "Content-Type: application/json; charset=utf-8\r\n" +
                $"Content-Length: {body.Length}\r\n" +
                "Connection: close\r\n" +
                "\r\n");
            await WriteAllAsync(stream, header, ct);
            await WriteAllAsync(stream, body, ct);
        }

        private static async Task WriteAllAsync(NetworkStream stream, byte[] data, CancellationToken ct)
        {
#if NET8_0_OR_GREATER
            await stream.WriteAsync(data.AsMemory(0, data.Length), ct);
#else
            await stream.WriteAsync(data, 0, data.Length);
#endif
        }

        private static async Task<int> ReadAsync(NetworkStream stream, byte[] buffer, int offset, int count, CancellationToken ct)
        {
#if NET8_0_OR_GREATER
            return await stream.ReadAsync(buffer.AsMemory(offset, count), ct);
#else
            return await stream.ReadAsync(buffer, offset, count);
#endif
        }

        public static IReadOnlyList<string> GetLanIpv4Addresses()
        {
            var preferred = new List<string>();
            var others = new List<string>();
            try
            {
                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up)
                        continue;
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        continue;
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;
                    foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ua.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        var ip = ua.Address.ToString();
                        if (ip.StartsWith("169.254.", StringComparison.Ordinal))
                            continue;
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                            ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                            preferred.Add(ip);
                        else
                            others.Add(ip);
                    }
                }
            }
            catch
            {
                /* ignore */
            }

            preferred.AddRange(others);
            return preferred;
        }

        private void CleanupFailedStart()
        {
            _running = false;
            try { _listener?.Stop(); } catch { /* ignore */ }
            _listener = null;
            _cts?.Dispose();
            _cts = null;
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
                _cts?.Dispose();
                _disposed = true;
            }
        }

        private sealed class IncomingHttpRequest
        {
            public string Method { get; set; } = "GET";
            public string Path { get; set; } = "/";
            public string RemoteIp { get; set; } = "unknown";
            public string ContentType { get; set; } = "";
            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public string Body { get; set; } = "";
        }
    }
}
