using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de teste do Servidor Webhook.
    /// Recebe eventos HTTP POST de controladores MobiCortex.
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
            // Preenche informações se disponíveis
        }

        private void FormWebhookServer_Load(object? sender, EventArgs e)
        {
            txtPorta.Text = "8080";
            chkAuth.Checked = false;
            AtualizarUrl();
            
            // Aviso importante
            Log("⚠️ AVISO: Este é um servidor webhook de REFERÊNCIA para testes.");
            Log("Não foi testado para alta carga (máx ~20 req/seg).");
            Log("Para produção com muitos dispositivos, use ASP.NET Core, AWS API Gateway, etc.");
            Log("");
        }

        private void AtualizarUrl()
        {
            var porta = txtPorta.Text;
            lblUrl.Text = $"http://localhost:{porta}/";
        }

        private async void btnIniciar_Click(object? sender, EventArgs e)
        {
            if (_server?.IsRunning == true)
            {
                await PararServidor();
                return;
            }

            await IniciarServidor();
        }

        private async Task IniciarServidor()
        {
            try
            {
                if (!int.TryParse(txtPorta.Text, out var porta))
                {
                    MessageBox.Show("Porta inválida", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnIniciar.Enabled = false;
                btnIniciar.Text = "Iniciando...";

                var authToken = chkAuth.Checked ? txtToken.Text.Trim() : null;

                _server = new WebhookServerService();
                _server.WebhookReceived += OnWebhookReceived;
                _server.LogReceived += OnLogReceived;

                var started = await _server.StartAsync(porta, authToken);

                if (started)
                {
                    btnIniciar.Text = "Parar";
                    btnIniciar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = $"Rodando na porta {porta}";
                    lblStatus.ForeColor = Color.DarkGreen;

                    Log($"Servidor webhook iniciado em http://localhost:{porta}/");
                    Log($"URL para configuração na controladora:");
                    Log($"  http://SEU_IP:{porta}/webhook");
                    Log("");
                    if (!string.IsNullOrEmpty(authToken))
                    {
                        Log($"Autenticação Bearer habilitada");
                        Log($"Token: {authToken}");
                    }
                    else
                    {
                        Log("Acesso sem autenticação (atenção!)");
                    }
                }
                else
                {
                    lblStatus.Text = "Falha ao iniciar";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Falha ao iniciar servidor.");
                    Log("Dicas:");
                    Log("  - Execute como Administrador");
                    Log("  - Ou use: netsh http add urlacl url=http://+:{porta}/ user=SEU_USUARIO");
                    Log("  - Ou use porta > 1024 sem necessidade de admin");
                    _server = null;
                }

                AtualizarStats();
            }
            catch (Exception ex)
            {
                Log($"Erro: {ex.Message}");
                lblStatus.Text = "Erro";
                lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                btnIniciar.Enabled = true;
            }
        }

        private async Task PararServidor()
        {
            if (_server != null)
            {
                await _server.StopAsync();
                (_server as IDisposable)?.Dispose();
                _server = null;
            }

            btnIniciar.Text = "Iniciar";
            btnIniciar.BackColor = SystemColors.Control;
            lblStatus.Text = "Parado";
            lblStatus.ForeColor = Color.Gray;
            Log("Servidor parado");
            AtualizarStats();
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
            Log($"  De: {e.RemoteIp}");
            Log($"  Content-Type: {e.ContentType}");
            Log($"  Body: {shortBody}");
            
            // Tentar extrair informações relevantes do JSON
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(e.Body);
                if (doc.RootElement.TryGetProperty("event_type", out var eventType))
                {
                    Log($"  Evento: {eventType.GetString()}");
                }
                if (doc.RootElement.TryGetProperty("device_id", out var deviceId))
                {
                    Log($"  Device: {deviceId.GetString()}");
                }
            }
            catch { /* ignora erro de parse */ }

            Log("");

            // Atualizar grid
            AtualizarGrid();
            AtualizarStats();
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
                LogLevel.Error => "[ERRO] ",
                LogLevel.Warning => "[AVISO] ",
                LogLevel.Debug => "[DEBUG] ",
                _ => "[INFO] "
            };

            Log(prefix + e.Message);
        }

        private void AtualizarGrid()
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

        private void AtualizarStats()
        {
            if (_server == null)
            {
                lblTotal.Text = "Total: 0";
                lblSucesso.Text = "Sucesso: 0";
                lblErros.Text = "Erros: 0";
                return;
            }

            var stats = _server.GetStats();
            lblTotal.Text = $"Total: {stats.TotalRequestsReceived}";
            lblSucesso.Text = $"Sucesso: {stats.TotalRequestsSuccess}";
            lblErros.Text = $"Erros: {stats.TotalRequestsError}";
        }

        private void chkAuth_CheckedChanged(object? sender, EventArgs e)
        {
            txtToken.Enabled = chkAuth.Checked;
        }

        private void txtPorta_TextChanged(object? sender, EventArgs e)
        {
            AtualizarUrl();
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _webhooks.Clear();
            _server?.ClearHistory();
            AtualizarGrid();
            AtualizarStats();
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_webhooks.Count == 0)
            {
                MessageBox.Show("Nenhum webhook para salvar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Log($"Webhooks salvos em: {dlg.FileName}");
            }
        }

        private void btnVerDetalhes_Click(object? sender, EventArgs e)
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
                using var doc = System.Text.Json.JsonDocument.Parse(webhook.Body);
                sb.Append(System.Text.Json.JsonSerializer.Serialize(doc, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            }
            catch
            {
                sb.Append(webhook.Body);
            }

            MessageBox.Show(sb.ToString(), "Detalhes do Webhook", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
