using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

namespace SmartSdk
{
    /// <summary>
    /// Webhook Server test form.
    /// Receives HTTP POST events from MobiCortex controllers.
    /// </summary>
    public partial class FormWebhookServer : Form
    {
        private IWebhookServerService? _server;
        private readonly List<WebhookReceivedEventArgs> _webhooks = new();

        public FormWebhookServer()
        {
            InitializeComponent();
        }

        public FormWebhookServer(IMobiCortexClient api) : this()
        {
            // Fill information if available
        }

        private void FormWebhookServer_Load(object? sender, EventArgs e)
        {
            txtPorta.Text = "8080";
            chkAuth.Checked = false;
            UpdateUrl();

            // Important notice
            Log("WARNING: This is a REFERENCE webhook server for testing.");
            Log("Not tested for high load (max ~20 req/sec).");
            Log("For production with many devices, use ASP.NET Core, AWS API Gateway, etc.");
            Log("");
        }

        private void btnFirewall_Click(object? sender, EventArgs e) => OpenFirewallForCurrentPort();

        private void OpenFirewallForCurrentPort()
        {
            if (!int.TryParse(txtPorta.Text.Trim(), out var port))
            {
                MessageBox.Show("Invalid port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (WebhookServerService.FirewallRuleExists(port))
            {
                Log($"Firewall inbound rule already present for TCP {port}.");
                MessageBox.Show($"TCP {port} is already allowed in Windows Firewall.", "Firewall",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Log($"Adding Windows Firewall inbound allow for TCP {port} (UAC)...");
            if (WebhookServerService.TryAddFirewallRuleElevated(port) &&
                WebhookServerService.FirewallRuleExists(port))
            {
                Log($"Firewall inbound rule added for TCP {port}.");
                MessageBox.Show($"Windows Firewall now allows TCP {port} from the LAN.", "Firewall",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log("Could not add firewall rule (UAC cancelled or failed). Run as Administrator:");
                Log("  " + WebhookServerService.FirewallNetshCommand(port));
                MessageBox.Show(
                    "Could not add the firewall rule.\nAccept the UAC prompt, or run as Administrator:\n\n" +
                    WebhookServerService.FirewallNetshCommand(port),
                    "Firewall",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void UpdateUrl()
        {
            if (!int.TryParse(txtPorta.Text.Trim(), out var porta))
                porta = 8080;
            var urls = WebhookServerService.GetLanWebhookUrls(porta);
            lblUrl.Text = urls.Count > 0
                ? string.Join("  |  ", urls)
                : $"http://0.0.0.0:{porta}/webhook";
        }

        private async void btnIniciar_Click(object? sender, EventArgs e)
        {
            if (_server?.IsRunning == true)
            {
                await StopServer();
                return;
            }

            await StartServer();
        }

        private async Task StartServer()
        {
            try
            {
                if (!int.TryParse(txtPorta.Text, out var porta))
                {
                    MessageBox.Show("Invalid port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnIniciar.Enabled = false;
                btnIniciar.Text = "Starting...";

                var authToken = chkAuth.Checked ? txtToken.Text.Trim() : null;

                _server = new WebhookServerService();
                _server.WebhookReceived += OnWebhookReceived;
                _server.LogReceived += OnLogReceived;

                var started = await _server.StartAsync(porta, authToken);

                if (started)
                {
                    btnIniciar.Text = "Stop";
                    btnIniciar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = $"Listening on 0.0.0.0:{porta} (LAN)";
                    lblStatus.ForeColor = Color.DarkGreen;
                    UpdateUrl();

                    Log($"Listening on all interfaces (0.0.0.0:{porta}) — reachable from the LAN.");
                    Log("URL to save on the controller (Settings > Webhooks, id 1..4):");
                    var urls = WebhookServerService.GetLanWebhookUrls(porta);
                    if (urls.Count == 0)
                        Log($"  http://<THIS_PC_LAN_IP>:{porta}/webhook");
                    foreach (var url in urls)
                        Log($"  {url}");
                    Log("Do not use localhost/127.0.0.1 — the controller is another device on the network.");
                    Log("Enable registered + unregistered so access and LPR events are posted.");
                    if (!WebhookServerService.FirewallRuleExists(porta))
                        Log("If the controller gets Connection timed out, click Allow Windows Firewall.");
                    Log("");
                    if (!string.IsNullOrEmpty(authToken))
                    {
                        Log($"Bearer authentication enabled");
                        Log($"Token: {authToken}");
                    }
                    else
                    {
                        Log("Access without authentication (caution!)");
                    }
                }
                else
                {
                    (_server as IDisposable)?.Dispose();
                    _server = null;
                    btnIniciar.Text = "Start";
                    btnIniciar.BackColor = SystemColors.Control;
                    lblStatus.Text = "Failed to start";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Failed to start server.");
                    Log("Tips:");
                    Log("  - Another process may already be using this port (filesync-win64 often uses 8080)");
                    Log("  - Try another port (e.g. 9099) and save that URL on the controller");
                    Log("  - Windows Firewall (Private) drops LAN TCP if there is no inbound rule");
                }

                UpdateStats();
            }
            catch (Exception ex)
            {
                (_server as IDisposable)?.Dispose();
                _server = null;
                btnIniciar.Text = "Start";
                btnIniciar.BackColor = SystemColors.Control;
                Log($"Error: {ex.Message}");
                lblStatus.Text = "Error";
                lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                btnIniciar.Enabled = true;
            }
        }

        private async Task StopServer()
        {
            if (_server != null)
            {
                await _server.StopAsync();
                (_server as IDisposable)?.Dispose();
                _server = null;
            }

            btnIniciar.Text = "Start";
            btnIniciar.BackColor = SystemColors.Control;
            lblStatus.Text = "Stopped";
            lblStatus.ForeColor = Color.Gray;
            Log("Server stopped");
            UpdateStats();
        }

        private void OnWebhookReceived(object? sender, WebhookReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnWebhookReceived(sender, e));
                return;
            }

            _webhooks.Add(e);

            var timestamp = e.ReceivedAt.ToString("HH:mm:ss.fff");
            var shortBody = e.Body.Length > 200
                ? e.Body.Substring(0, 200) + "..."
                : e.Body;

            Log($"[{timestamp}] {e.Method} {e.Path}");
            Log($"  From: {e.RemoteIp}");
            Log($"  Content-Type: {e.ContentType}");
            Log($"  Body: {shortBody}");

            MobiCortex.Sdk.Models.MqttExportContract.LogPayloadHints(e.Body, Log);

            Log("");

            // Update grid
            UpdateGrid();
            UpdateStats();
        }

        private void OnLogReceived(object? sender, WebhookLogEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnLogReceived(sender, e));
                return;
            }

            var prefix = e.Level switch
            {
                LogLevel.Error => "[ERROR] ",
                LogLevel.Warning => "[WARNING] ",
                LogLevel.Debug => "[DEBUG] ",
                _ => "[INFO] "
            };

            Log(prefix + e.Message);
        }

        private void UpdateGrid()
        {
            gridWebhooks.Rows.Clear();
            foreach (var w in _webhooks.OrderByDescending(w => w.ReceivedAt).Take(100))
            {
                var i = gridWebhooks.Rows.Add(
                    w.ReceivedAt.ToString("HH:mm:ss"),
                    w.Method,
                    w.Path,
                    w.RemoteIp,
                    w.Body.Length > 50 ? w.Body.Substring(0, 50) + "..." : w.Body
                );
                gridWebhooks.Rows[i].Tag = w;
            }
        }

        private void UpdateStats()
        {
            if (_server == null)
            {
                lblTotal.Text = "Total: 0";
                lblSucesso.Text = "Success: 0";
                lblErros.Text = "Errors: 0";
                return;
            }

            var stats = _server.GetStats();
            lblTotal.Text = $"Total: {stats.TotalRequestsReceived}";
            lblSucesso.Text = $"Success: {stats.TotalRequestsSuccess}";
            lblErros.Text = $"Errors: {stats.TotalRequestsError}";
        }

        private void chkAuth_CheckedChanged(object? sender, EventArgs e)
        {
            txtToken.Enabled = chkAuth.Checked;
        }

        private void txtPorta_TextChanged(object? sender, EventArgs e)
        {
            UpdateUrl();
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _webhooks.Clear();
            _server?.ClearHistory();
            UpdateGrid();
            UpdateStats();
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_webhooks.Count == 0)
            {
                MessageBox.Show("No webhooks to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"webhooks_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(_webhooks, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(dlg.FileName, json);
                Log($"Webhooks saved to: {dlg.FileName}");
            }
        }

        private void btnVerDetalhes_Click(object? sender, EventArgs e) => ShowSelectedWebhook();

        private void gridWebhooks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                ShowSelectedWebhook();
        }

        private void ShowSelectedWebhook()
        {
            if (gridWebhooks.SelectedRows.Count == 0) return;
            if (gridWebhooks.SelectedRows[0].Tag is not WebhookReceivedEventArgs webhook)
                return;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Timestamp: {webhook.ReceivedAt:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"Method: {webhook.Method}");
            sb.AppendLine($"Path: {webhook.Path}");
            sb.AppendLine($"Remote IP: {webhook.RemoteIp}");
            sb.AppendLine($"Content-Type: {webhook.ContentType}");
            sb.AppendLine();
            sb.AppendLine("Headers:");
            foreach (var h in webhook.Headers)
                sb.AppendLine($"  {h.Key}: {h.Value}");
            sb.AppendLine();
            sb.AppendLine("Body:");
            sb.Append(MobiCortex.Sdk.Models.MqttExportContract.PrettyJson(webhook.Body));

            using var dlg = new Form
            {
                Text = $"Webhook {webhook.ReceivedAt:HH:mm:ss}  {webhook.RemoteIp}",
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

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(message)); return; }
            txtLog.AppendText($"{message}{Environment.NewLine}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_server?.IsRunning == true)
            {
                _server.StopAsync().GetAwaiter().GetResult();
            }
            (_server as IDisposable)?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
