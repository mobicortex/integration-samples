using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Services;

namespace SmartSdk
{
    /// <summary>
    /// MQTT client: TCP 1884 on the controller, topic mbcortex/export/event.
    /// </summary>
    public partial class FormMqttCliente : Form
    {
        private IMqttClientService? _mqttClient;
        private readonly IMobiCortexClient? _api;
        private readonly List<MqttMessageReceivedEventArgs> _messages = new();

        public FormMqttCliente()
        {
            InitializeComponent();
        }

        public FormMqttCliente(IMobiCortexClient api) : this()
        {
            _api = api;
            txtWsUrl.Text = MqttExportContract.HostFromBaseUrl(api.BaseUrl);
        }

        private void FormMqttCliente_Load(object? sender, EventArgs e)
        {
            chkEvents.Checked = true;
            chkLogs.Checked = false;
            chkSensors.Checked = false;
            chkStatus.Checked = false;
            txtTopicoCustom.Text = MqttExportContract.EventTopic;
            txtPort.Text = MqttExportContract.ListenPort.ToString();
            if (string.IsNullOrWhiteSpace(txtUser.Text))
                txtUser.Text = "mqttuser";
            if (string.IsNullOrWhiteSpace(txtSessionKey.Text))
                txtSessionKey.Text = "mqttpass";
            btnCreateUser.Enabled = _api?.IsAuthenticated == true;
        }

        private async void btnConectar_Click(object? sender, EventArgs e)
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
                var host = txtWsUrl.Text.Trim();
                if (!int.TryParse(txtPort.Text.Trim(), out var port))
                    port = MqttExportContract.ListenPort;
                var user = MqttExportContract.NormalizeUsername(txtUser.Text);
                var password = txtSessionKey.Text;

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Enter host, username and password (create a credential in Settings > MQTT, or use Create test user).",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnConectar.Enabled = false;
                btnConectar.Text = "Connecting...";

                var topics = new List<string>();
                if (chkEvents.Checked)
                    topics.Add(MqttExportContract.EventTopic);
                if (!string.IsNullOrEmpty(txtTopicoCustom.Text))
                {
                    var custom = txtTopicoCustom.Text.Trim();
                    if (!topics.Contains(custom))
                        topics.Add(custom);
                }

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

                Log($"Connecting mqtt://{host}:{port} as {user}");
                var connected = await _mqttClient.ConnectTcpAsync(host, port, user, password, topics);

                if (connected)
                {
                    btnConectar.Text = "Disconnect";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.DarkGreen;
                    Log("Connected to export broker");
                    Log($"Subscribed to: {string.Join(", ", topics)}");
                    Log("ACL is read-only on mbcortex/export/# — publish to this topic will fail.");
                }
                else
                {
                    lblStatus.Text = "Connection failed";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Failed to connect. Check port 1884, username, password, and that mqtt-server was reloaded after creating the user.");
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

        private async void btnCreateUser_Click(object? sender, EventArgs e)
        {
            if (_api == null || !_api.IsAuthenticated)
            {
                MessageBox.Show("Log in on MainForm first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnCreateUser.Enabled = false;
                var username = txtUser.Text.Trim();
                if (username.StartsWith("ext_", StringComparison.OrdinalIgnoreCase))
                    username = username.Substring(4);
                if (string.IsNullOrEmpty(username))
                    username = "sdktest";

                var result = await _api.MqttExport.SaveUserAsync(1, new MqttExportUserRequest
                {
                    Name = "SDK test",
                    Username = username,
                    Password = "",
                    Active = 1
                });

                if (!result.Success || result.Data == null)
                {
                    Log($"Create user failed: {result.Message} {result.RawResponse}");
                    return;
                }

                txtUser.Text = result.Data.Username;
                if (!string.IsNullOrEmpty(result.Data.Password))
                {
                    txtSessionKey.Text = result.Data.Password;
                    Log($"Created {result.Data.Username} — password shown once: {result.Data.Password}");
                }
                else
                {
                    Log($"Updated {result.Data.Username} (password kept). Enter the existing password to connect.");
                }
            }
            catch (Exception ex)
            {
                Log($"Create user error: {ex.Message}");
            }
            finally
            {
                btnCreateUser.Enabled = true;
            }
        }

        private void OnMqttMessageReceived(object? sender, MqttMessageReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttMessageReceived(sender, e));
                return;
            }

            _messages.Add(e);

            var timestamp = e.ReceivedAt.ToString("HH:mm:ss.fff");

            Log($"[{timestamp}] {e.Topic}");
            Log($"  QoS: {e.QosLevel} | Retain: {e.Retain}");
            Log(MqttExportContract.PrettyJson(e.Payload));
            MqttExportContract.LogPayloadHints(e.Payload, Log);
            Log("");

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

        private async void btnPublicar_Click(object? sender, EventArgs e)
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
            Log(result ? $"Published to {topic}" : "Failed to publish (export ACL is read-only)");
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _messages.Clear();
            lblMensagens.Text = "Messages: 0";
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_messages.Count == 0)
            {
                MessageBox.Show("No messages to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"mqtt_messages_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

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
