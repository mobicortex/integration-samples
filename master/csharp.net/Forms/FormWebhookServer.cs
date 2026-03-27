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
        private readonly List<WebhookReceivedEventArgs> _webhooks = new List<WebhookReceivedEventArgs>();

        public FormWebhookServer()
        {
            InitializeComponent();
        }

        public FormWebhookServer(IMobiCortexClient api) : this()
        {
            // Fill information if available
        }

        private void FormWebhookServer_Load(object sender, EventArgs e)
        {
            txtPorta.Text = "8080";
            chkAuth.Checked = false;
            UpdateUrl();

            // Important warning
            Log("WARNING: This is a REFERENCE webhook server for testing.");
            Log("Not tested for high load (max ~20 req/sec).");
            Log("For production with many devices, use ASP.NET Core, AWS API Gateway, etc.");
            Log("");
        }

        private void UpdateUrl()
        {
            var port = txtPorta.Text;
            lblUrl.Text = $"http://localhost:{port}/";
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
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
                if (!int.TryParse(txtPorta.Text, out var port))
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

                var started = await _server.StartAsync(port, authToken);

                if (started)
                {
                    btnIniciar.Text = "Stop";
                    btnIniciar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = $"Running on port {port}";
                    lblStatus.ForeColor = Color.DarkGreen;

                    Log($"Webhook server started at http://localhost:{port}/");
                    Log($"URL for controller configuration:");
                    Log($"  http://YOUR_IP:{port}/webhook");
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
                    lblStatus.Text = "Failed to start";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Failed to start server.");
                    Log("Tips:");
                    Log("  - Run as Administrator");
                    Log($"  - Or use: netsh http add urlacl url=http://+:{port}/ user=YOUR_USER");
                    Log("  - Or use a port > 1024 without needing admin");
                    _server = null;
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

            // Try to extract relevant information from the JSON
            try
            {
                using (var doc = System.Text.Json.JsonDocument.Parse(e.Body))
                {
                    if (doc.RootElement.TryGetProperty("event_type", out var eventType))
                    {
                        Log($"  Event: {eventType.GetString()}");
                    }
                    if (doc.RootElement.TryGetProperty("device_id", out var deviceId))
                    {
                        Log($"  Device: {deviceId.GetString()}");
                    }
                }
            }
            catch { /* ignore parse error */ }

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

            var prefix = "[INFO] ";
            switch (e.Level)
            {
                case LogLevel.Error:
                    prefix = "[ERROR] ";
                    break;
                case LogLevel.Warning:
                    prefix = "[WARNING] ";
                    break;
                case LogLevel.Debug:
                    prefix = "[DEBUG] ";
                    break;
            }

            Log(prefix + e.Message);
        }

        private void UpdateGrid()
        {
            gridWebhooks.Rows.Clear();
            foreach (var w in _webhooks.OrderByDescending(w => w.ReceivedAt).Take(100))
            {
                gridWebhooks.Rows.Add(
                    w.ReceivedAt.ToString("HH:mm:ss"),
                    w.Method,
                    w.Path,
                    w.RemoteIp,
                    w.Body.Length > 50 ? w.Body.Substring(0, 50) + "..." : w.Body
                );
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

        private void chkAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtToken.Enabled = chkAuth.Checked;
        }

        private void txtPorta_TextChanged(object sender, EventArgs e)
        {
            UpdateUrl();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            _webhooks.Clear();
            _server?.ClearHistory();
            UpdateGrid();
            UpdateStats();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_webhooks.Count == 0)
            {
                MessageBox.Show("No webhooks to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dlg.FileName = $"webhooks_{DateTime.Now:yyyyMMdd_HHmmss}.json";

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
        }

        private void btnVerDetalhes_Click(object sender, EventArgs e)
        {
            if (gridWebhooks.SelectedRows.Count == 0) return;

            var index = gridWebhooks.SelectedRows[0].Index;
            var webhook = _webhooks.OrderByDescending(w => w.ReceivedAt).Skip(index).FirstOrDefault();

            if (webhook == null) return;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Timestamp: {webhook.ReceivedAt:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"Method: {webhook.Method}");
            sb.AppendLine($"Path: {webhook.Path}");
            sb.AppendLine($"Remote IP: {webhook.RemoteIp}");
            sb.AppendLine($"Content-Type: {webhook.ContentType}");
            sb.AppendLine();
            sb.AppendLine("Headers:");
            foreach (var h in webhook.Headers)
            {
                sb.AppendLine($"  {h.Key}: {h.Value}");
            }
            sb.AppendLine();
            sb.AppendLine("Body:");

            // Format JSON if possible
            try
            {
                using (var doc = System.Text.Json.JsonDocument.Parse(webhook.Body))
                {
                    sb.Append(System.Text.Json.JsonSerializer.Serialize(doc, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
                }
            }
            catch
            {
                sb.Append(webhook.Body);
            }

            MessageBox.Show(sb.ToString(), "Webhook Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
