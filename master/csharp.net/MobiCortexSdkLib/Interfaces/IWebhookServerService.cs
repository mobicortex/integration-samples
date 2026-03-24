namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço de servidor HTTP para receber webhooks de controladores MobiCortex.
    /// </summary>
    /// <remarks>
    /// ⚠️ <b>AVISO IMPORTANTE:</b> Esta implementação é fornecida como <b>referência/exemplo</b>.
    /// <para>
    /// Este servidor HTTP embutido utiliza HttpListener e foi projetado para demonstração,
    /// desenvolvimento e testes com poucos dispositivos (até 10-20 requisições/segundo).
    /// </para>
    /// <para>
    /// <b>NÃO FOI TESTADO</b> para cenários de alta carga com milhares de dispositivos.
    /// Se você precisa suportar muitos controladores simultaneamente (produção em escala),
    /// considere utilizar soluções profissionais como:
    /// <list type="bullet">
    ///   <item>ASP.NET Core com Kestrel (IIS/NGINX)</item>
    ///   <item>AWS API Gateway + Lambda</item>
    ///   <item>Azure Functions</item>
    ///   <item>Google Cloud Functions</item>
    ///   <item>Servidores dedicados com load balancing</item>
    /// </list>
    /// </para>
    /// <para>
    /// Esta implementação é útil para:
    /// - Prototipagem rápida
    /// - Testes de integração
    /// - Demonstrações
    /// - Sistemas pequenos (até ~50 requisições/minuto)
    /// </para>
    /// </remarks>
    public interface IWebhookServerService
    {
        /// <summary>
        /// Indica se o servidor está em execução.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Porta HTTP em que o servidor está ouvindo.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// URL base do servidor (ex: http://localhost:8080).
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Evento disparado quando um webhook é recebido.
        /// </summary>
        event EventHandler<WebhookReceivedEventArgs>? WebhookReceived;

        /// <summary>
        /// Evento disparado quando um log é gerado.
        /// </summary>
        event EventHandler<WebhookLogEventArgs>? LogReceived;

        /// <summary>
        /// Inicia o servidor HTTP.
        /// </summary>
        /// <param name="port">Porta HTTP (padrão: 8080)</param>
        /// <param name="authToken">Token de autenticação opcional (Bearer)</param>
        /// <returns>True se iniciado com sucesso</returns>
        Task<bool> StartAsync(int port = 8080, string? authToken = null);

        /// <summary>
        /// Para o servidor HTTP.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Retorna todos os webhooks recebidos (histórico).
        /// </summary>
        IReadOnlyList<WebhookReceivedEventArgs> GetHistory();

        /// <summary>
        /// Limpa o histórico de webhooks.
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// Retorna estatísticas do servidor.
        /// </summary>
        WebhookServerStats GetStats();
    }

    /// <summary>
    /// Argumentos do evento de webhook recebido.
    /// </summary>
    public class WebhookReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// ID único do webhook.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Timestamp de recebimento.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// IP do remetente.
        /// </summary>
        public string RemoteIp { get; set; } = string.Empty;

        /// <summary>
        /// Método HTTP (POST, PUT, etc).
        /// </summary>
        public string Method { get; set; } = "POST";

        /// <summary>
        /// Path da URL (ex: /webhook/events).
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Headers HTTP.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new();

        /// <summary>
        /// Conteúdo do body (JSON).
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de conteúdo.
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// Status da resposta HTTP enviada.
        /// </summary>
        public int ResponseStatusCode { get; set; } = 200;
    }

    /// <summary>
    /// Argumentos do evento de log.
    /// </summary>
    public class WebhookLogEventArgs : EventArgs
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Info;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Níveis de log.
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Estatísticas do servidor de webhook.
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
