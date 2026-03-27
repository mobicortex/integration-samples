using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    /// <summary>
    /// MQTT Monitoring Form - connects to the controller's broker.
    /// </summary>
    public partial class FormMonitoramento : Form
    {
        private IMobiCortexClient _api = new MobiCortexClient();
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
            get { return _api; }
            set { _api = value; }
        }

        private async void btnConectar_Click(object sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected == true)
            {
                await Disconnect();
                return;
            }

            if (!_api.IsAuthenticated || string.IsNullOrEmpty(_api.SessionKey))
            {
                ShowWarning("Log in on MainForm before connecting to MQTT");
                return;
            }

            try
            {
                btnConectar.Enabled = false;
                Log("Connecting to MQTT via WebSocket...");

                var wsUrl = _api.BaseUrl
                    .Replace("https://", "wss://")
                    .Replace("http://", "ws://")
                    + "/mbcortex/master/api/v1/mqtt";

                Log($"URL: {wsUrl}");

                _mqttClient = new MqttClientService();
                _mqttClient.MessageReceived += OnMessageReceived;
                _mqttClient.Disconnected += OnDisconnected;

                var topic = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topic)) topic = "#";

                var connected = await _mqttClient.ConnectAsync(wsUrl, _api.SessionKey!, new[] { topic });

                if (connected)
                {
                    Log("Connected to MQTT!");
                    Log($"Subscribed to topic: {topic}");

                    btnConectar.Text = "Disconnect";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    Log("Failed to connect to MQTT");
                }
            }
            catch (Exception ex)
            {
                Log($"Error connecting: {ex.Message}");
            }
            finally
            {
                btnConectar.Enabled = true;
            }
        }

        private async void btnSubscrever_Click(object sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                ShowWarning("Connect to MQTT first");
                return;
            }

            try
            {
                var topic = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topic)) topic = "#";

                await _mqttClient.SubscribeAsync(topic);
                Log($"Subscribed to topic: {topic}");
            }
            catch (Exception ex)
            {
                Log($"Error subscribing: {ex.Message}");
            }
        }

        private void OnMessageReceived(object? sender, MqttMessageReceivedEventArgs e)
        {
            Invoke(() =>
            {
                Log($"[{e.Topic}] {e.Payload}");
                _msgCount++;
                lblContador.Text = $"Messages: {_msgCount}";
            });
        }

        private void OnDisconnected(object? sender, EventArgs e)
        {
            Invoke(() =>
            {
                Log("Disconnected from MQTT");
                btnConectar.Text = "Connect MQTT";
                btnConectar.BackColor = Color.FromArgb(0, 123, 255);
                lblStatus.Text = "Disconnected";
                lblStatus.ForeColor = Color.Red;
            });
        }

        private async Task Disconnect()
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                (_mqttClient as IDisposable)?.Dispose();
                _mqttClient = null;
                Log("Disconnected from MQTT");
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            _msgCount = 0;
            lblContador.Text = "Messages: 0";
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            await Disconnect();
            base.OnFormClosing(e);
        }

        private void Log(string msg)
        {
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(msg)); return; }
            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            txtLog.AppendText($"[{ts}] {msg}{Environment.NewLine}");
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
