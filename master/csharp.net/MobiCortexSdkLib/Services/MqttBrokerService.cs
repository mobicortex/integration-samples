using System.Text;
using MQTTnet;
using MQTTnet.Server;
using MobiCortex.Sdk.Interfaces;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// Implementação de broker MQTT embutido usando MQTTnet.Server.
    /// </summary>
    /// <remarks>
    /// ⚠️ AVISO: Esta é uma implementação de REFERÊNCIA para desenvolvimento/testes.
    /// Não foi testada para alta carga. Para produção com muitos dispositivos,
    /// use brokers profissionais como Mosquitto, EMQX, HiveMQ ou cloud (AWS IoT, Azure IoT).
    /// </remarks>
    public class MqttBrokerService : IMqttBrokerService, IDisposable
    {
        private MqttServer? _server;
        private readonly Dictionary<string, BrokerClientConnectedEventArgs> _connectedClients = new();
        private long _totalMessagesReceived = 0;
        private long _totalMessagesSent = 0;
        private DateTime _startedAt;
        private bool _disposed;

        /// <inheritdoc/>
        public bool IsRunning => _server?.IsStarted ?? false;

        /// <inheritdoc/>
        public int Port { get; private set; } = 1883;

        /// <inheritdoc/>
        public IReadOnlyList<string> ConnectedClients => _connectedClients.Keys.ToList();

        /// <inheritdoc/>
        public event EventHandler<MqttBrokerMessageEventArgs>? MessageReceived;

        /// <inheritdoc/>
        public event EventHandler<BrokerClientConnectedEventArgs>? ClientConnected;

        /// <inheritdoc/>
        public event EventHandler<BrokerClientDisconnectedEventArgs>? ClientDisconnected;

        /// <inheritdoc/>
        public async Task<bool> StartAsync(int port = 1883, bool allowAnonymous = true, string? username = null, string? password = null)
        {
            if (_server?.IsStarted == true)
            {
                await StopAsync();
            }

            try
            {
                Port = port;
                var builder = new MqttServerOptionsBuilder()
                    .WithDefaultEndpoint()
                    .WithDefaultEndpointPort(port);

                var options = builder.Build();
                _server = new MqttFactory().CreateMqttServer(options);

                // Configurar eventos
                _server.ClientConnectedAsync += OnClientConnected;
                _server.ClientDisconnectedAsync += OnClientDisconnected;
                _server.InterceptingPublishAsync += OnMessageReceived;

                await _server.StartAsync();
                _startedAt = DateTime.Now;
                _totalMessagesReceived = 0;
                _totalMessagesSent = 0;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task StopAsync()
        {
            if (_server == null) return;

            try
            {
                await _server.StopAsync();
                _connectedClients.Clear();
            }
            catch
            {
                // Ignora erros no stop
            }
        }

        /// <inheritdoc/>
        public async Task<bool> PublishAsync(string topic, string payload, int qos = 1)
        {
            if (_server?.IsStarted != true)
                return false;

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(payload))
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                    .Build();

                await _server.InjectApplicationMessage(
                    new InjectedMqttApplicationMessage(message)
                    {
                        SenderClientId = "broker-internal"
                    });

                Interlocked.Increment(ref _totalMessagesSent);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public MqttBrokerStats GetStats()
        {
            return new MqttBrokerStats
            {
                IsRunning = IsRunning,
                Port = Port,
                ConnectedClientsCount = _connectedClients.Count,
                TotalMessagesReceived = _totalMessagesReceived,
                TotalMessagesSent = _totalMessagesSent,
                StartedAt = _startedAt
            };
        }

        private Task OnClientConnected(ClientConnectedEventArgs args)
        {
            var clientInfo = new BrokerClientConnectedEventArgs
            {
                ClientId = args.ClientId,
                ConnectedAt = DateTime.Now
            };

            _connectedClients[args.ClientId] = clientInfo;
            ClientConnected?.Invoke(this, clientInfo);
            return Task.CompletedTask;
        }

        private Task OnClientDisconnected(ClientDisconnectedEventArgs args)
        {
            _connectedClients.Remove(args.ClientId);
            
            ClientDisconnected?.Invoke(this, new BrokerClientDisconnectedEventArgs
            {
                ClientId = args.ClientId,
                Reason = args.DisconnectType.ToString(),
                DisconnectedAt = DateTime.Now
            });
            return Task.CompletedTask;
        }

        private Task OnMessageReceived(InterceptingPublishEventArgs args)
        {
            Interlocked.Increment(ref _totalMessagesReceived);

            var payload = args.ApplicationMessage.ConvertPayloadToString() ?? "";
            
            MessageReceived?.Invoke(this, new MqttBrokerMessageEventArgs
            {
                ClientId = args.ClientId,
                Topic = args.ApplicationMessage.Topic,
                Payload = payload,
                QosLevel = (int)args.ApplicationMessage.QualityOfServiceLevel,
                ReceivedAt = DateTime.Now
            });

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                StopAsync().GetAwaiter().GetResult();
                _server?.Dispose();
                _disposed = true;
            }
        }
    }
}
