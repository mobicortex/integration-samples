using MQTTnet;
using MQTTnet.Client;
using SmartSdk.Services;

namespace SmartSdk.Forms
{
    // =============================================================================
    //  MONITORAMENTO - MQTT sobre WebSocket
    //
    //  Este formulário demonstra como receber eventos em tempo real do controlador
    //  usando o protocolo MQTT sobre WebSocket (WSS).
    //
    //  O controlador expõe um broker MQTT acessível via WebSocket:
    //  wss://<host>/mbcortex/master/api/v1/mqtt
    //
    //  AUTENTICAÇÃO MQTT:
    //  - Username: qualquer valor (não é verificado)
    //  - Password: session_key obtido no login HTTP
    //
    //  TÓPICOS DISPONÍVEIS:
    //  - mbcortex/master/events/#     → Eventos de acesso (registrado/não registrado)
    //  - mbcortex/master/logs/#       → Logs do sistema
    //  - mbcortex/master/sensors/#    → Eventos de sensores (portas, botões)
    //  - mbcortex/master/status/#     → Status do controlador
    //  - #                            → Todos os tópicos
    //
    //  BIBLIOTECA: MQTTnet 4.x (NuGet)
    //
    //  FLUXO:
    //  1. Fazer login HTTP normal (obter session_key)
    //  2. Conectar via WebSocket usando o session_key como senha MQTT
    //  3. Subscrever nos tópicos desejados
    //  4. Receber mensagens em tempo real
    // =============================================================================

    public partial class FormMonitoramento : Form
    {
        private readonly MobiCortexApiService _api;
        private IMqttClient? _mqttClient;

        public FormMonitoramento(MobiCortexApiService api)
        {
            _api = api;
            InitializeComponent();
        }

        // =====================================================================
        //  CONEXÃO MQTT
        // =====================================================================

        /// <summary>
        /// Conecta ao broker MQTT do controlador via WebSocket.
        ///
        /// A URL do WebSocket é:
        ///   wss://{host}/mbcortex/master/api/v1/mqtt
        ///
        /// A autenticação usa o session_key do login HTTP como senha MQTT.
        /// </summary>
        private async void btnConectar_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected == true)
            {
                await Desconectar();
                return;
            }

            if (!_api.IsAuthenticated || string.IsNullOrEmpty(_api.SessionKey))
            {
                Aviso("Faça login no MainForm antes de conectar ao MQTT");
                return;
            }

            try
            {
                btnConectar.Enabled = false;
                Log("Conectando ao MQTT via WebSocket...");

                // Monta a URL do WebSocket MQTT
                // Exemplo: wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt
                var wsUrl = _api.BaseUrl
                    .Replace("https://", "wss://")
                    .Replace("http://", "ws://")
                    + "/mbcortex/master/api/v1/mqtt";

                Log($"URL: {wsUrl}");

                // Cria o cliente MQTT usando MQTTnet
                var factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient();

                // Configura para receber mensagens
                _mqttClient.ApplicationMessageReceivedAsync += OnMensagemRecebida;
                _mqttClient.DisconnectedAsync += OnDesconectado;

                // Opções de conexão via WebSocket
                var options = new MqttClientOptionsBuilder()
                    .WithWebSocketServer(o => o.WithUri(wsUrl))
                    .WithCredentials("sdk", _api.SessionKey) // Senha = session_key
                    .WithTlsOptions(o =>
                    {
                        // Aceita certificado auto-assinado do controlador
                        o.WithCertificateValidationHandler(_ => true);
                    })
                    .WithClientId($"SmartSdk-{Environment.MachineName}")
                    .WithCleanSession(true)
                    .Build();

                await _mqttClient.ConnectAsync(options);
                Log("Conectado ao MQTT!");

                // Subscreve no tópico padrão
                await SubscreverTopico();

                btnConectar.Text = "Desconectar";
                btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                lblStatus.Text = "Conectado";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                Log($"Erro ao conectar: {ex.Message}");
            }
            finally
            {
                btnConectar.Enabled = true;
            }
        }

        /// <summary>
        /// Subscreve no tópico configurado no campo txtTopico.
        /// Use "#" para receber todas as mensagens.
        /// </summary>
        private async Task SubscreverTopico()
        {
            if (_mqttClient?.IsConnected != true) return;

            var topico = txtTopico.Text.Trim();
            if (string.IsNullOrEmpty(topico)) topico = "#";

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topico)
                .Build();

            await _mqttClient.SubscribeAsync(subscribeOptions);
            Log($"Inscrito no tópico: {topico}");
        }

        /// <summary>
        /// Altera a assinatura para um novo tópico.
        /// </summary>
        private async void btnSubscrever_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                Aviso("Conecte primeiro ao MQTT");
                return;
            }

            try
            {
                // Remove assinatura anterior (unsubscribe de tudo)
                await _mqttClient.UnsubscribeAsync(new MqttClientUnsubscribeOptionsBuilder()
                    .WithTopicFilter("#").Build());

                await SubscreverTopico();
            }
            catch (Exception ex)
            {
                Log($"Erro ao subscrever: {ex.Message}");
            }
        }

        /// <summary>
        /// Callback: mensagem MQTT recebida.
        /// Cada mensagem contém o tópico e o payload (JSON geralmente).
        /// </summary>
        private Task OnMensagemRecebida(MqttApplicationMessageReceivedEventArgs e)
        {
            var topico = e.ApplicationMessage.Topic;
            var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

            // Atualiza o log na thread da UI
            Invoke(() =>
            {
                Log($"[{topico}] {payload}");

                // Incrementa contador de mensagens
                _msgCount++;
                lblContador.Text = $"Mensagens: {_msgCount}";
            });

            return Task.CompletedTask;
        }

        private int _msgCount;

        private Task OnDesconectado(MqttClientDisconnectedEventArgs e)
        {
            Invoke(() =>
            {
                Log($"Desconectado: {e.Reason}");
                btnConectar.Text = "Conectar MQTT";
                btnConectar.BackColor = Color.FromArgb(0, 123, 255);
                lblStatus.Text = "Desconectado";
                lblStatus.ForeColor = Color.Red;
            });
            return Task.CompletedTask;
        }

        private async Task Desconectar()
        {
            if (_mqttClient?.IsConnected == true)
            {
                await _mqttClient.DisconnectAsync();
                Log("Desconectado do MQTT");
            }
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _msgCount = 0;
            lblContador.Text = "Mensagens: 0";
        }

        // =====================================================================
        //  CLEANUP
        // =====================================================================

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            await Desconectar();
            _mqttClient?.Dispose();
            base.OnFormClosing(e);
        }

        // =====================================================================
        //  HELPERS
        // =====================================================================

        private void Log(string msg)
        {
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(msg)); return; }
            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            txtLog.AppendText($"[{ts}] {msg}{Environment.NewLine}");
        }

        private void Aviso(string msg) =>
            MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
