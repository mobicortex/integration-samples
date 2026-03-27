using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;
using MQTTnet.Protocol;

namespace SmartSdk
{
    /// <summary>
    /// MQTT Client test form.
    /// Connects to the MobiCortex controller's MQTT broker.
    /// </summary>
    public partial class FormMqttCliente : Form
    {
        private IMqttClientService? _mqttClient;
        private readonly List<MqttMessageReceivedEventArgs> _messages = new List<MqttMessageReceivedEventArgs>();

        public FormMqttCliente()
        {
            InitializeComponent();
        }

        public FormMqttCliente(IMobiCortexClient api) : this()
        {
            txtWsUrl.Text = api.BaseUrl
                .Replace("https://", "wss://")
                .Replace("http://", "ws://") + "/mbcortex/master/api/v1/mqtt";
            txtSessionKey.Text = api.SessionKey ?? "";
        }

        private void FormMqttCliente_Load(object sender, EventArgs e)
        {
            // Default topics pre-selected
            chkEvents.Checked = true;
            chkLogs.Checked = false;
            chkSensors.Checked = false;
        }

        private async void btnConectar_Click(object sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected == true)
            {
                await Disconnect();
                return;
            }

            await Connect();
        }

        private async Task Connect()
        {
            try
            {
                var wsUrl = txtWsUrl.Text.Trim();
                var sessionKey = txtSessionKey.Text.Trim();

                if (string.IsNullOrEmpty(wsUrl) || string.IsNullOrEmpty(sessionKey))
                {
                    MessageBox.Show("Enter the WebSocket URL and Session Key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnConectar.Enabled = false;
                btnConectar.Text = "Connecting...";

                // Collect topics
                var topics = new List<string>();
                if (chkEvents.Checked) topics.Add("mbcortex/master/events/#");
                if (chkLogs.Checked) topics.Add("mbcortex/master/logs/#");
                if (chkSensors.Checked) topics.Add("mbcortex/master/sensors/#");
                if (chkStatus.Checked) topics.Add("mbcortex/master/status/#");
                if (!string.IsNullOrEmpty(txtTopicoCustom.Text))
                    topics.Add(txtTopicoCustom.Text.Trim());

                if (!topics.Any())
                {
                    MessageBox.Show("Select at least one topic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnConectar.Enabled = true;
                    btnConectar.Text = "Connect";
                    return;
                }

                _mqttClient = new MqttClientService();
                _mqttClient.MessageReceived += OnMqttMessageReceived;
                _mqttClient.Disconnected += OnMqttDisconnected;

                var connected = await _mqttClient.ConnectAsync(wsUrl, sessionKey, topics);

                if (connected)
                {
                    btnConectar.Text = "Disconnect";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.DarkGreen;
                    Log("Connected to MQTT broker");
                    Log($"Subscribed to: {string.Join(", ", topics)}");
                }
                else
                {
                    lblStatus.Text = "Connection failed";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Failed to connect to MQTT broker");
                    _mqttClient = null;
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                lblStatus.Text = "Error";
                lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                btnConectar.Enabled = true;
            }
        }

        private async Task Disconnect()
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                (_mqttClient as IDisposable)?.Dispose();
                _mqttClient = null;
            }

            btnConectar.Text = "Connect";
            btnConectar.BackColor = SystemColors.Control;
            lblStatus.Text = "Disconnected";
            lblStatus.ForeColor = Color.Gray;
            Log("Disconnected from MQTT broker");
        }

        private void OnMqttMessageReceived(object? sender, MqttMessageReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttMessageReceived(sender, e));
                return;
            }

            _messages.Add(e);

            // Format for display
            var timestamp = e.ReceivedAt.ToString("HH:mm:ss.fff");
            var shortPayload = e.Payload.Length > 100
                ? e.Payload.Substring(0, 100) + "..."
                : e.Payload;

            Log($"[{timestamp}] {e.Topic}");
            Log($"  QoS: {e.QosLevel} | Retain: {e.Retain}");
            Log($"  Payload: {shortPayload}");
            Log("");

            // Update counter
            lblMensagens.Text = $"Messages: {_messages.Count}";
        }

        private void OnMqttDisconnected(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttDisconnected(sender, e));
                return;
            }

            Log("MQTT connection lost!");
            lblStatus.Text = "Disconnected";
            lblStatus.ForeColor = Color.DarkRed;
            btnConectar.Text = "Connect";
            btnConectar.BackColor = SystemColors.Control;
        }

        private async void btnPublicar_Click(object sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                MessageBox.Show("Connect to the broker first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var topic = txtPubTopico.Text.Trim();
            var payload = txtPubPayload.Text.Trim();
            var qos = cmbPubQoS.SelectedIndex;

            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(payload))
            {
                MessageBox.Show("Enter topic and payload", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = await _mqttClient.PublishAsync(topic, payload, qos);
            Log(result ? $"Published to {topic}" : "Failed to publish");
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            _messages.Clear();
            lblMensagens.Text = "Messages: 0";
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_messages.Count == 0)
            {
                MessageBox.Show("No messages to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dlg.FileName = $"mqtt_messages_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(_messages, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    File.WriteAllText(dlg.FileName, json);
                    Log($"Messages saved to: {dlg.FileName}");
                }
            }
        }

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(message)); return; }
            txtLog.AppendText($"{message}{Environment.NewLine}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_mqttClient?.IsConnected == true)
            {
                _mqttClient.DisconnectAsync().GetAwaiter().GetResult();
            }
            (_mqttClient as IDisposable)?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
