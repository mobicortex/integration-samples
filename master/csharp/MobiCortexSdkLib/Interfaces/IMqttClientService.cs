namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço de cliente MQTT para conexão com o broker da controladora MobiCortex.
    /// </summary>
    public interface IMqttClientService
    {
        /// <summary>
        /// Indica se o cliente MQTT está conectado.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Evento disparado quando uma mensagem MQTT é recebida.
        /// </summary>
        event EventHandler<MqttMessageReceivedEventArgs>? MessageReceived;

        /// <summary>
        /// Evento disparado quando a conexão é perdida.
        /// </summary>
        event EventHandler? Disconnected;

        /// <summary>
        /// Conecta ao broker MQTT da controladora via WebSocket.
        /// </summary>
        /// <param name="wsUrl">URL WebSocket (ex: wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt)</param>
        /// <param name="sessionKey">Session key obtida no login</param>
        /// <param name="topics">Tópicos para subscrever (ex: "mbcortex/master/events/#")</param>
        /// <returns>True se conectado com sucesso</returns>
        Task<bool> ConnectAsync(string wsUrl, string sessionKey, IEnumerable<string> topics);

        /// <summary>
        /// Desconecta do broker MQTT.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Publica uma mensagem em um tópico.
        /// </summary>
        /// <param name="topic">Tópico</param>
        /// <param name="payload">Conteúdo da mensagem</param>
        /// <param name="qos">QoS (0, 1 ou 2)</param>
        /// <returns>True se publicado com sucesso</returns>
        Task<bool> PublishAsync(string topic, string payload, int qos = 0);

        /// <summary>
        /// Subscreve em um tópico.
        /// </summary>
        /// <param name="topic">Tópico (pode usar wildcards # e +)</param>
        /// <returns>True se subscrito com sucesso</returns>
        Task<bool> SubscribeAsync(string topic);

        /// <summary>
        /// Cancela subscrição de um tópico.
        /// </summary>
        Task<bool> UnsubscribeAsync(string topic);
    }

    /// <summary>
    /// Argumentos do evento de mensagem MQTT recebida.
    /// </summary>
    public class MqttMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Tópico da mensagem.
        /// </summary>
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo da mensagem (payload).
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        /// <summary>
        /// QoS da mensagem.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Indica se é uma mensagem retain.
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// Timestamp de recebimento.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
    }
}
