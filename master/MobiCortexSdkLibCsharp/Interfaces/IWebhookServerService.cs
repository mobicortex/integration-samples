namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// HTTP server service for receiving webhooks from MobiCortex controllers.
    /// </summary>
    /// <remarks>
    /// WARNING: This implementation is provided as a reference/example.
    /// <para>
    /// This embedded HTTP server uses HttpListener and was designed for demonstration,
    /// development and testing with few devices (up to 10-20 requests/second).
    /// </para>
    /// <para>
    /// It has NOT BEEN TESTED for high-load scenarios with thousands of devices.
    /// If you need to support many controllers simultaneously (production at scale),
    /// consider using professional solutions such as:
    /// <list type="bullet">
    ///   <item>ASP.NET Core with Kestrel (IIS/NGINX)</item>
    ///   <item>AWS API Gateway + Lambda</item>
    ///   <item>Azure Functions</item>
    ///   <item>Google Cloud Functions</item>
    ///   <item>Dedicated servers with load balancing</item>
    /// </list>
    /// </para>
    /// <para>
    /// This implementation is useful for:
    /// - Rapid prototyping
    /// - Integration testing
    /// - Demonstrations
    /// - Small systems (up to ~50 requests/minute)
    /// </para>
    /// </remarks>
    public interface IWebhookServerService
    {
        /// <summary>
        /// Indicates whether the server is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// HTTP port the server is listening on.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Server base URL (e.g.: http://localhost:8080).
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Event fired when a webhook is received.
        /// </summary>
        event EventHandler<WebhookReceivedEventArgs>? WebhookReceived;

        /// <summary>
        /// Event fired when a log entry is generated.
        /// </summary>
        event EventHandler<WebhookLogEventArgs>? LogReceived;

        /// <summary>
        /// Starts the HTTP server.
        /// </summary>
        /// <param name="port">HTTP port (default: 8080)</param>
        /// <param name="authToken">Optional authentication token (Bearer)</param>
        /// <returns>True if started successfully</returns>
        Task<bool> StartAsync(int port = 8080, string? authToken = null);

        /// <summary>
        /// Stops the HTTP server.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Returns all received webhooks (history).
        /// </summary>
        IReadOnlyList<WebhookReceivedEventArgs> GetHistory();

        /// <summary>
        /// Clears the webhook history.
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// Returns server statistics.
        /// </summary>
        WebhookServerStats GetStats();
    }

    /// <summary>
    /// Received webhook event arguments.
    /// </summary>
    public class WebhookReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Unique webhook ID.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Received timestamp.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Sender IP.
        /// </summary>
        public string RemoteIp { get; set; } = string.Empty;

        /// <summary>
        /// HTTP method (POST, PUT, etc.).
        /// </summary>
        public string Method { get; set; } = "POST";

        /// <summary>
        /// URL path (e.g.: /webhook/events).
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// HTTP headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new();

        /// <summary>
        /// Body content (JSON).
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// HTTP response status sent.
        /// </summary>
        public int ResponseStatusCode { get; set; } = 200;
    }

    /// <summary>
    /// Log event arguments.
    /// </summary>
    public class WebhookLogEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Info;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Log levels.
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Webhook server statistics.
    /// </summary>
    public class WebhookServerStats
    {
        public bool IsRunning { get; set; }
        public int Port { get; set; }
        public long TotalRequestsReceived { get; set; }
        public long TotalRequestsSuccess { get; set; }
        public long TotalRequestsError { get; set; }
        public DateTime StartedAt { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
    }
}
