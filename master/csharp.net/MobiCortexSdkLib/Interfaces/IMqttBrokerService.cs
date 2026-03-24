namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço de broker MQTT embutido.
    /// Permite que controladores MobiCortex se conectem diretamente ao servidor da aplicação.
    /// </summary>
    /// <remarks>
    /// ⚠️ <b>AVISO IMPORTANTE:</b> Esta implementação é fornecida como <b>referência/exemplo</b>.
    /// <para>
    /// Este broker MQTT embutido utiliza MQTTnet.Server e foi projetado para demonstração,
    /// desenvolvimento e testes com poucos dispositivos (até 10-20 conexões simultâneas).
    /// </para>
    /// <para>
    /// <b>NÃO FOI TESTADO</b> para cenários de alta carga com milhares de dispositivos.
    /// Se você precisa suportar muitos controladores simultaneamente (produção em escala),
    /// considere utilizar brokers MQTT profissionais como:
    /// <list type="bullet">
    ///   <item>Eclipse Mosquitto</item>
    ///   <item>EMQX</item>
    ///   <item>HiveMQ</item>
    ///   <item>AWS IoT Core</item>
    ///   <item>Azure IoT Hub</item>
    /// </list>
    /// </para>
    /// <para>
    /// Esta implementação é útil para:
    /// - Prototipagem rápida
    /// - Testes de integração
    /// - Demonstrações
    /// - Sistemas pequenos (até ~20 dispositivos)
    /// </para>
    /// </remarks>
    public interface IMqttBrokerService
    {
        /// <summary>
        /// Indica se o broker está em execução.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Porta TCP em que o broker está ouvindo.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Lista de clientes conectados (IDs dos clientes).
        /// </summary>
        IReadOnlyList<string> ConnectedClients { get; }

        /// <summary>
        /// Evento disparado quando uma mensagem é publicada no broker.
        /// </summary>
        event EventHandler<MqttBrokerMessageEventArgs>? MessageReceived;

        /// <summary>
        /// Evento disparado quando um cliente se conecta.
        /// </summary>
        event EventHandler<BrokerClientConnectedEventArgs>? ClientConnected;

        /// <summary>
        /// Evento disparado quando um cliente se desconecta.
        /// </summary>
        event EventHandler<BrokerClientDisconnectedEventArgs>? ClientDisconnected;

        /// <summary>
        /// Inicia o broker MQTT.
        /// </summary>
        /// <param name="port">Porta TCP (padrão: 1883)</param>
        /// <param name="allowAnonymous">Permite conexões sem autenticação</param>
        /// <param name="username">Usuário para autenticação (se allowAnonymous = false)</param>
        /// <param name="password">Senha para autenticação (se allowAnonymous = false)</param>
        /// <returns>True se iniciado com sucesso</returns>
        Task<bool> StartAsync(int port = 1883, bool allowAnonymous = true, string? username = null, string? password = null);

        /// <summary>
        /// Para o broker MQTT.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Publica uma mensagem no broker.
        /// </summary>
        /// <param name="topic">Tópico</param>
        /// <param name="payload">Conteúdo JSON</param>
        /// <param name="qos">QoS (0, 1 ou 2)</param>
        /// <returns>True se publicado com sucesso</returns>
        Task<bool> PublishAsync(string topic, string payload, int qos = 1);

        /// <summary>
        /// Retorna estatísticas do broker.
        /// </summary>
        MqttBrokerStats GetStats();
    }

    /// <summary>
    /// Argumentos do evento de mensagem no broker.
    /// </summary>
    public class MqttBrokerMessageEventArgs : EventArgs
    {
        /// <summary>
        /// ID do cliente que publicou.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Tópico da mensagem.
        /// </summary>
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo da mensagem.
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        /// <summary>
        /// QoS da mensagem.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Argumentos do evento de cliente conectado.
    /// </summary>
    public class BrokerClientConnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; } = string.Empty;
        public string? Username { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Argumentos do evento de cliente desconectado.
    /// </summary>
    public class BrokerClientDisconnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public DateTime DisconnectedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Estatísticas do broker MQTT.
    /// </summary>
    public class MqttBrokerStats
    {
        public bool IsRunning { get; set; }
        public int Port { get; set; }
        public int ConnectedClientsCount { get; set; }
        public long TotalMessagesReceived { get; set; }
        public long TotalMessagesSent { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
