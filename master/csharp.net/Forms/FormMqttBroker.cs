using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;
using MqttClientConnectedEventArgs = MobiCortex.Sdk.Interfaces.BrokerClientConnectedEventArgs;
using MqttClientDisconnectedEventArgs = MobiCortex.Sdk.Interfaces.BrokerClientDisconnectedEventArgs;

namespace SmartSdk
{
    /// <summary>
    /// Embedded MQTT Broker test form.
    /// Allows MobiCortex controllers to connect directly to this application.
    /// </summary>
    public partial class FormMqttBroker : Form
    {
        private IMqttBrokerService? _broker;

        public FormMqttBroker()
        {
            InitializeComponent();
        }

        private void rbAnonimo_CheckedChanged(object sender, EventArgs e)
        {
            txtUsuario.Enabled = !rbAnonimo.Checked;
            txtSenha.Enabled = !rbAnonimo.Checked;
        }

        private void rbAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtUsuario.Enabled = rbAuth.Checked;
            txtSenha.Enabled = rbAuth.Checked;
        }

        private void FormMqttBroker_Load(object sender, EventArgs e)
        {
            txtPorta.Text = "1883";
            cmbQoS.SelectedIndex = 1;
            UpdateStats();

            Log("WARNING: This is a reference MQTT broker for testing.");
            Log("Not tested for high load (max ~20 devices).");
            Log("For production with many devices, use Mosquitto, EMQX or HiveMQ.");
            Log("");
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            if (_broker?.IsRunning == true)
            {
                await StopBroker();
                return;
            }

            await StartBroker();
        }

        private async Task StartBroker()
        {
            try
            {
                if (!int.TryParse(txtPorta.Text, out var port))
                {
                    MessageBox.Show("Invalid port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnIniciar.Enabled = false;
                btnIniciar.Text = "Starting...";

                var anonymous = rbAnonimo.Checked;
                var username = txtUsuario.Text.Trim();
                var password = txtSenha.Text;

                _broker = new MqttBrokerService();
                _broker.MessageReceived += OnBrokerMessageReceived;
                _broker.ClientConnected += OnClientConnected;
                _broker.ClientDisconnected += OnClientDisconnected;

                var started = await _broker.StartAsync(port, anonymous, username, password);

                if (started)
                {
                    btnIniciar.Text = "Stop";
                    btnIniciar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = $"Running on port {port}";
                    lblStatus.ForeColor = Color.DarkGreen;

                    var info = $"Broker started on port {port}";
                    info += anonymous ? " (anonymous access)" : $" (authentication: {username})";
                    Log(info);
                    Log($"Address: mqtt://{Environment.MachineName}:{port}");
                    Log("Configure the controller to send MQTT events to this address");
                }
                else
                {
                    lblStatus.Text = "Failed to start";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Failed to start broker. Check if the port is not in use.");
                    Log("Tip: Run as Administrator or use a port > 1024");
                    _broker = null;
                }

                UpdateStats();
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                lblStatus.Text = "Error";
                lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                btnIniciar.Enabled = true;
            }
        }

        private async Task StopBroker()
        {
            if (_broker != null)
            {
                await _broker.StopAsync();
                (_broker as IDisposable)?.Dispose();
                _broker = null;
            }

            btnIniciar.Text = "Start";
            btnIniciar.BackColor = SystemColors.Control;
            lblStatus.Text = "Stopped";
            lblStatus.ForeColor = Color.Gray;
            Log("Broker stopped");
            UpdateStats();
        }

        private void OnBrokerMessageReceived(object? sender, MqttBrokerMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnBrokerMessageReceived(sender, e));
                return;
            }

            var timestamp = e.ReceivedAt.ToString("HH:mm:ss.fff");
            var shortPayload = e.Payload.Length > 150
                ? e.Payload.Substring(0, 150) + "..."
                : e.Payload;

            Log($"[{timestamp}] [{e.ClientId}] {e.Topic}");
            Log($"  QoS: {e.QosLevel}");
            Log($"  Payload: {shortPayload}");
            Log("");

            UpdateStats();
        }

        private void OnClientConnected(object? sender, BrokerClientConnectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnClientConnected(sender, e));
                return;
            }

            Log($"Client connected: {e.ClientId}");
            UpdateStats();
        }

        private void OnClientDisconnected(object? sender, BrokerClientDisconnectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnClientDisconnected(sender, e));
                return;
            }

            Log($"Client disconnected: {e.ClientId} ({e.Reason})");
            UpdateStats();
        }

        private async void btnPublicar_Click(object sender, EventArgs e)
        {
            if (_broker?.IsRunning != true)
            {
                MessageBox.Show("Start the broker first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var topic = txtPubTopico.Text.Trim();
            var payload = txtPubPayload.Text.Trim();
            var qos = cmbQoS.SelectedIndex;

            if (string.IsNullOrEmpty(topic))
            {
                MessageBox.Show("Enter the topic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = await _broker.PublishAsync(topic, payload, qos);
            Log(result ? $"Published to {topic}" : "Failed to publish");
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_broker == null) return;

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dlg.FileName = $"broker_stats_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var stats = _broker.GetStats();
                    var json = System.Text.Json.JsonSerializer.Serialize(
                        stats,
                        new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(dlg.FileName, json);
                    Log($"Statistics saved to: {dlg.FileName}");
                }
            }
        }

        private void UpdateStats()
        {
            if (_broker == null)
            {
                lblClientes.Text = "Clients: 0";
                lblMensagens.Text = "Messages: 0";
                return;
            }

            var stats = _broker.GetStats();
            lblClientes.Text = $"Clients: {stats.ConnectedClientsCount}";
            lblMensagens.Text = $"Messages: {stats.TotalMessagesReceived}";
        }

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(() => Log(message));
                return;
            }

            txtLog.AppendText($"{message}{Environment.NewLine}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_broker?.IsRunning == true)
            {
                _broker.StopAsync().GetAwaiter().GetResult();
            }

            (_broker as IDisposable)?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
