namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// MQTT client service for connecting to the MobiCortex controller broker.
    /// </summary>
    public interface IMqttClientService
    {
        /// <summary>
        /// Indicates whether the MQTT client is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Event fired when an MQTT message is received.
        /// </summary>
        event EventHandler<MqttMessageReceivedEventArgs>? MessageReceived;

        /// <summary>
        /// Event fired when the connection is lost.
        /// </summary>
        event EventHandler? Disconnected;

        /// <summary>
        /// Connects to the controller's MQTT broker via WebSocket.
        /// </summary>
        /// <param name="wsUrl">WebSocket URL (e.g.: wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt)</param>
        /// <param name="sessionKey">Session key obtained at login</param>
        /// <param name="topics">Topics to subscribe to (e.g.: "mbcortex/master/events/#")</param>
        /// <returns>True if connected successfully</returns>
        Task<bool> ConnectAsync(string wsUrl, string sessionKey, IEnumerable<string> topics);

        /// <summary>
        /// Disconnects from the MQTT broker.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Publishes a message to a topic.
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <param name="payload">Message content</param>
        /// <param name="qos">QoS (0, 1 or 2)</param>
        /// <returns>True if published successfully</returns>
        Task<bool> PublishAsync(string topic, string payload, int qos = 0);

        /// <summary>
        /// Subscribes to a topic.
        /// </summary>
        /// <param name="topic">Topic (can use wildcards # and +)</param>
        /// <returns>True if subscribed successfully</returns>
        Task<bool> SubscribeAsync(string topic);

        /// <summary>
        /// Unsubscribes from a topic.
        /// </summary>
        Task<bool> UnsubscribeAsync(string topic);
    }

    /// <summary>
    /// Received MQTT message event arguments.
    /// </summary>
    public class MqttMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Message topic.
        /// </summary>
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Message content (payload).
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        /// <summary>
        /// Message QoS.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Indicates whether this is a retain message.
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// Received timestamp.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
    }
}
