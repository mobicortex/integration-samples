namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Embedded MQTT broker service.
    /// Allows MobiCortex controllers to connect directly to the application server.
    /// </summary>
    /// <remarks>
    /// WARNING: This implementation is provided as a reference/example.
    /// <para>
    /// This embedded MQTT broker uses MQTTnet.Server and was designed for demonstration,
    /// development and testing with few devices (up to 10-20 simultaneous connections).
    /// </para>
    /// <para>
    /// It has NOT BEEN TESTED for high-load scenarios with thousands of devices.
    /// If you need to support many controllers simultaneously (production at scale),
    /// consider using professional MQTT brokers such as:
    /// <list type="bullet">
    ///   <item>Eclipse Mosquitto</item>
    ///   <item>EMQX</item>
    ///   <item>HiveMQ</item>
    ///   <item>AWS IoT Core</item>
    ///   <item>Azure IoT Hub</item>
    /// </list>
    /// </para>
    /// <para>
    /// This implementation is useful for:
    /// - Rapid prototyping
    /// - Integration testing
    /// - Demonstrations
    /// - Small systems (up to ~20 devices)
    /// </para>
    /// </remarks>
    public interface IMqttBrokerService
    {
        /// <summary>
        /// Indicates whether the broker is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// TCP port the broker is listening on.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// List of connected clients (client IDs).
        /// </summary>
        IReadOnlyList<string> ConnectedClients { get; }

        /// <summary>
        /// Event fired when a message is published to the broker.
        /// </summary>
        event EventHandler<MqttBrokerMessageEventArgs>? MessageReceived;

        /// <summary>
        /// Event fired when a client connects.
        /// </summary>
        event EventHandler<BrokerClientConnectedEventArgs>? ClientConnected;

        /// <summary>
        /// Event fired when a client disconnects.
        /// </summary>
        event EventHandler<BrokerClientDisconnectedEventArgs>? ClientDisconnected;

        /// <summary>
        /// Starts the MQTT broker.
        /// </summary>
        /// <param name="port">TCP port (default: 1883)</param>
        /// <param name="allowAnonymous">Allow unauthenticated connections</param>
        /// <param name="username">Username for authentication (if allowAnonymous = false)</param>
        /// <param name="password">Password for authentication (if allowAnonymous = false)</param>
        /// <returns>True if started successfully</returns>
        Task<bool> StartAsync(int port = 1883, bool allowAnonymous = true, string? username = null, string? password = null);

        /// <summary>
        /// Stops the MQTT broker.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Publishes a message to the broker.
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <param name="payload">JSON content</param>
        /// <param name="qos">QoS (0, 1 or 2)</param>
        /// <returns>True if published successfully</returns>
        Task<bool> PublishAsync(string topic, string payload, int qos = 1);

        /// <summary>
        /// Returns broker statistics.
        /// </summary>
        MqttBrokerStats GetStats();
    }

    /// <summary>
    /// Broker message event arguments.
    /// </summary>
    public class MqttBrokerMessageEventArgs : EventArgs
    {
        /// <summary>
        /// ID of the client that published.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Message topic.
        /// </summary>
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Message content.
        /// </summary>
        public string Payload { get; set; } = string.Empty;

        /// <summary>
        /// Message QoS.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Client connected event arguments.
    /// </summary>
    public class BrokerClientConnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; } = string.Empty;
        public string? Username { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Client disconnected event arguments.
    /// </summary>
    public class BrokerClientDisconnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public DateTime DisconnectedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// MQTT broker statistics.
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
