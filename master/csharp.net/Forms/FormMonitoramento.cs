using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de Monitoramento MQTT - conecta ao broker da controladora.
    /// </summary>
    public partial class FormMonitoramento : Form
    {
        private IMobiCortexClient _api = null!;
        private IMqttClientService? _mqttClient;
        private int _msgCount;

        public FormMonitoramento()
        {
            InitializeComponent();
        }

        public FormMonitoramento(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

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

                var wsUrl = _api.BaseUrl
                    .Replace("https://", "wss://")
                    .Replace("http://", "ws://")
                    + "/mbcortex/master/api/v1/mqtt";

                Log($"URL: {wsUrl}");

                _mqttClient = new MqttClientService();
                _mqttClient.MessageReceived += OnMensagemRecebida;
                _mqttClient.Disconnected += OnDesconectado;

                var topico = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topico)) topico = "#";

                var connected = await _mqttClient.ConnectAsync(wsUrl, _api.SessionKey, new[] { topico });

                if (connected)
                {
                    Log("Conectado ao MQTT!");
                    Log($"Inscrito no tópico: {topico}");

                    btnConectar.Text = "Desconectar";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Conectado";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    Log("Falha ao conectar ao MQTT");
                }
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

        private async void btnSubscrever_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                Aviso("Conecte primeiro ao MQTT");
                return;
            }

            try
            {
                var topico = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topico)) topico = "#";

                await _mqttClient.SubscribeAsync(topico);
                Log($"Inscrito no tópico: {topico}");
            }
            catch (Exception ex)
            {
                Log($"Erro ao subscrever: {ex.Message}");
            }
        }

        private void OnMensagemRecebida(object? sender, MqttMessageReceivedEventArgs e)
        {
            Invoke(() =>
            {
                Log($"[{e.Topic}] {e.Payload}");
                _msgCount++;
                lblContador.Text = $"Mensagens: {_msgCount}";
            });
        }

        private void OnDesconectado(object? sender, EventArgs e)
        {
            Invoke(() =>
            {
                Log("Desconectado do MQTT");
                btnConectar.Text = "Conectar MQTT";
                btnConectar.BackColor = Color.FromArgb(0, 123, 255);
                lblStatus.Text = "Desconectado";
                lblStatus.ForeColor = Color.Red;
            });
        }

        private async Task Desconectar()
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                (_mqttClient as IDisposable)?.Dispose();
                _mqttClient = null;
                Log("Desconectado do MQTT");
            }
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _msgCount = 0;
            lblContador.Text = "Mensagens: 0";
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            await Desconectar();
            base.OnFormClosing(e);
        }

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
