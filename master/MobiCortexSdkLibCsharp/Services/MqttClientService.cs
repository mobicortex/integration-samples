using System.Text;
using MQTTnet;
#if NETFRAMEWORK
using MQTTnet.Client;
#endif
using MQTTnet.Formatter;
using MobiCortex.Sdk.Interfaces;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// MQTT client implementation for connecting to MobiCortex controllers.
    /// </summary>
    public class MqttClientService : IMqttClientService, IDisposable
    {
        private IMqttClient? _client;
        private readonly List<string> _subscribedTopics = new();
        private bool _disposed;

        /// <inheritdoc/>
        public bool IsConnected => _client?.IsConnected ?? false;

        /// <inheritdoc/>
        public event EventHandler<MqttMessageReceivedEventArgs>? MessageReceived;

        /// <inheritdoc/>
        public event EventHandler? Disconnected;

        /// <summary>
        /// Creates a new instance of the MQTT Client service.
        /// </summary>
        public MqttClientService()
        {
        }

        /// <inheritdoc/>
        public async Task<bool> ConnectAsync(string wsUrl, string sessionKey, IEnumerable<string> topics)
        {
            if (_client != null && _client.IsConnected)
            {
                await DisconnectAsync();
            }

            try
            {
#if NET8_0_OR_GREATER
                var factory = new MqttClientFactory();
#else
                var factory = new MqttFactory();
#endif
                _client = factory.CreateMqttClient();

                _client.ApplicationMessageReceivedAsync += OnMessageReceived;
                _client.DisconnectedAsync += OnDisconnected;

                var options = new MqttClientOptionsBuilder()
                    .WithWebSocketServer(o => o.WithUri(wsUrl))
                    .WithCredentials("sdk", sessionKey)
                    .WithTlsOptions(o =>
                    {
                        o.WithCertificateValidationHandler(_ => true);
                    })
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithCleanSession()
                    .Build();

                var result = await _client.ConnectAsync(options, CancellationToken.None);

                if (result.ResultCode != MqttClientConnectResultCode.Success)
                {
                    return false;
                }

                foreach (var topic in topics)
                {
                    await SubscribeAsync(topic);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task DisconnectAsync()
        {
            if (_client == null) return;

            try
            {
                if (_client.IsConnected)
                {
                    await _client.DisconnectAsync();
                }
            }
            catch
            {
            }
            finally
            {
                _subscribedTopics.Clear();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PublishAsync(string topic, string payload, int qos = 0)
        {
            if (_client == null || !_client.IsConnected)
                return false;

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(payload))
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                    .Build();

                var result = await _client.PublishAsync(message, CancellationToken.None);
                return result.IsSuccess;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SubscribeAsync(string topic)
        {
            if (_client == null || !_client.IsConnected)
                return false;

            try
            {
                var result = await _client.SubscribeAsync(topic);

                if (result.Items.All(i => i.ResultCode <= MqttClientSubscribeResultCode.GrantedQoS2))
                {
                    if (!_subscribedTopics.Contains(topic))
                        _subscribedTopics.Add(topic);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UnsubscribeAsync(string topic)
        {
            if (_client == null || !_client.IsConnected)
                return false;

            try
            {
                await _client.UnsubscribeAsync(topic);
                _subscribedTopics.Remove(topic);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs args)
        {
#if NET8_0_OR_GREATER
            var payload = args.ApplicationMessage.ConvertPayloadToString() ?? "";
#else
            var segment = args.ApplicationMessage.PayloadSegment;
            var payload = segment.Count > 0
                ? Encoding.UTF8.GetString(segment.Array!, segment.Offset, segment.Count)
                : "";
#endif

            MessageReceived?.Invoke(this, new MqttMessageReceivedEventArgs
            {
                Topic = args.ApplicationMessage.Topic,
                Payload = payload,
                QosLevel = (int)args.ApplicationMessage.QualityOfServiceLevel,
                Retain = args.ApplicationMessage.Retain,
                ReceivedAt = DateTime.Now
            });

            return Task.CompletedTask;
        }

        private Task OnDisconnected(MqttClientDisconnectedEventArgs args)
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                DisconnectAsync().GetAwaiter().GetResult();
                _client?.Dispose();
                _disposed = true;
            }
        }
    }
}
