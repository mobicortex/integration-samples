using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Services;

namespace SmartSdk
{
    /// <summary>
    /// MQTT monitoring: TCP 1884, topic mbcortex/export/event.
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
            txtHost.Text = MqttExportContract.HostFromBaseUrl(api.BaseUrl);
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
                await Disconnect();
                return;
            }

            await ConnectAndSubscribe();
        }

        private async void btnSubscrever_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                await ConnectAndSubscribe();
                return;
            }

            try
            {
                var topic = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topic))
                    topic = MqttExportContract.EventTopic;

                await _mqttClient.SubscribeAsync(topic);
                Log($"Subscribed to topic: {topic}");
            }
            catch (Exception ex)
            {
                Log($"Error subscribing: {ex.Message}");
            }
        }

        private async Task ConnectAndSubscribe()
        {
            var host = txtHost.Text.Trim();
            var user = MqttExportContract.NormalizeUsername(txtUser.Text);
            var password = txtPass.Text;
            if (!int.TryParse(txtPort.Text.Trim(), out var port))
                port = MqttExportContract.ListenPort;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                Warning("Enter host, user and password (export credential, not the HTTP session).");
                return;
            }

            try
            {
                btnConectar.Enabled = false;
                btnSubscrever.Enabled = false;
                Log($"Connecting mqtt://{host}:{port} as {user}");

                _mqttClient = new MqttClientService();
                _mqttClient.MessageReceived += OnMessageReceived;
                _mqttClient.Disconnected += OnDisconnected;

                var topic = txtTopico.Text.Trim();
                if (string.IsNullOrEmpty(topic))
                    topic = MqttExportContract.EventTopic;

                var connected = await _mqttClient.ConnectTcpAsync(host, port, user, password, new[] { topic });

                if (connected)
                {
                    Log("Connected to export broker");
                    Log($"Subscribed to topic: {topic}");

                    btnConectar.Text = "Disconnect";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    Log("Failed to connect. Check port 1884, username, password, and that mqtt-server was reloaded after creating the user.");
                }
            }
            catch (Exception ex)
            {
                Log($"Error connecting: {ex.Message}");
            }
            finally
            {
                btnConectar.Enabled = true;
                btnSubscrever.Enabled = true;
            }
        }

        private void OnMessageReceived(object? sender, MqttMessageReceivedEventArgs e)
        {
            Invoke(() =>
            {
                Log($"[{e.Topic}]\r\n{MqttExportContract.PrettyJson(e.Payload)}");
                MqttExportContract.LogPayloadHints(e.Payload, Log);
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

        private void btnLimpar_Click(object? sender, EventArgs e)
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

        private void Warning(string msg) =>
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
