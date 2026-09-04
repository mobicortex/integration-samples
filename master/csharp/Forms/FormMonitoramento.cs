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
        private readonly List<MqttMessageReceivedEventArgs> _messages = new();
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
                _messages.Add(e);
                if (_messages.Count > 500)
                    _messages.RemoveAt(0);

                var summary = SummarizePayload(e.Payload);
                var preview = e.Payload.Length > 80 ? e.Payload.Substring(0, 80) + "..." : e.Payload;
                gridEventos.Rows.Insert(0,
                    e.ReceivedAt.ToString("HH:mm:ss"),
                    summary.Event,
                    summary.Plate,
                    summary.Registered,
                    e.Topic,
                    preview);
                gridEventos.Rows[0].Tag = e;
                while (gridEventos.Rows.Count > 200)
                    gridEventos.Rows.RemoveAt(gridEventos.Rows.Count - 1);

                _msgCount++;
                lblContador.Text = $"Messages: {_msgCount}";
                Log($"{summary.Event} plate={summary.Plate} registered={summary.Registered}");
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
            gridEventos.Rows.Clear();
            _messages.Clear();
            _msgCount = 0;
            lblContador.Text = "Messages: 0";
        }

        private void gridEventos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                ShowSelectedEvent();
        }

        private void ShowSelectedEvent()
        {
            if (gridEventos.SelectedRows.Count == 0) return;
            if (gridEventos.SelectedRows[0].Tag is not MqttMessageReceivedEventArgs msg)
                return;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Timestamp: {msg.ReceivedAt:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"Topic: {msg.Topic}");
            sb.AppendLine($"QoS: {msg.QosLevel}  Retain: {msg.Retain}");
            sb.AppendLine();
            sb.AppendLine("Payload:");
            sb.Append(MqttExportContract.PrettyJson(msg.Payload));

            using var dlg = new Form
            {
                Text = $"MQTT {msg.ReceivedAt:HH:mm:ss}  {msg.Topic}",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(780, 580),
                MinimumSize = new Size(480, 320)
            };
            dlg.Controls.Add(new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                WordWrap = true,
                Font = new Font("Consolas", 9F),
                Text = sb.ToString()
            });
            dlg.ShowDialog(this);
        }

        private static (string Event, string Plate, string Registered) SummarizePayload(string json)
        {
            var ev = "";
            var plate = "";
            var registered = "";
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("event", out var eventEl))
                {
                    if (eventEl.ValueKind == System.Text.Json.JsonValueKind.String)
                        ev = eventEl.GetString() ?? "";
                    else if (eventEl.ValueKind == System.Text.Json.JsonValueKind.Object &&
                             eventEl.TryGetProperty("event_type", out var nested))
                        ev = nested.GetString() ?? "";
                }
                if (root.TryGetProperty("event_type", out var eventType))
                    ev = eventType.GetString() ?? ev;
                if (root.TryGetProperty("plate", out var plateEl))
                    plate = plateEl.GetString() ?? "";
                if (root.TryGetProperty("registered", out var regEl))
                    registered = regEl.ToString();
            }
            catch
            {
                /* raw payload stays in the grid */
            }
            return (ev, plate, registered);
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
