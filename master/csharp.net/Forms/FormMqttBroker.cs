using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;
using MqttClientConnectedEventArgs = MobiCortex.Sdk.Interfaces.BrokerClientConnectedEventArgs;
using MqttClientDisconnectedEventArgs = MobiCortex.Sdk.Interfaces.BrokerClientDisconnectedEventArgs;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de teste do Broker MQTT embutido.
    /// Permite que controladores MobiCortex se conectem diretamente nesta aplicação.
    /// </summary>
    public partial class FormMqttBroker : Form
    {
        private IMqttBrokerService? _broker;

        public FormMqttBroker()
        {
            InitializeComponent();
        }

        private void FormMqttBroker_Load(object? sender, EventArgs e)
        {
            txtPorta.Text = "1883";
            cmbQoS.SelectedIndex = 1; // QoS 1
            AtualizarStats();
            
            // Aviso importante
            Log("⚠️ AVISO: Este é um broker MQTT de REFERÊNCIA para testes.");
            Log("Não foi testado para alta carga (máx ~20 dispositivos).");
            Log("Para produção com muitos dispositivos, use Mosquitto, EMQX ou HiveMQ.");
            Log("");
        }

        private async void btnIniciar_Click(object? sender, EventArgs e)
        {
            if (_broker?.IsRunning == true)
            {
                await PararBroker();
                return;
            }

            await IniciarBroker();
        }

        private async Task IniciarBroker()
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

                var anonimo = rbAnonimo.Checked;
                var usuario = txtUsuario.Text.Trim();
                var senha = txtSenha.Text;

                _broker = new MqttBrokerService();
                _broker.MessageReceived += OnBrokerMessageReceived;
                _broker.ClientConnected += OnClientConnected;
                _broker.ClientDisconnected += OnClientDisconnected;

                var started = await _broker.StartAsync(porta, anonimo, usuario, senha);

                if (started)
                {
                    btnIniciar.Text = "Parar";
                    btnIniciar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = $"Rodando na porta {porta}";
                    lblStatus.ForeColor = Color.DarkGreen;

                    var info = $"Broker iniciado na porta {porta}";
                    info += anonimo ? " (acesso anônimo)" : $" (autenticação: {usuario})";
                    Log(info);
                    Log($"Endereço: mqtt://{Environment.MachineName}:{porta}");
                    Log("Configure a controladora para enviar eventos MQTT para este endereço");
                }
                else
                {
                    lblStatus.Text = "Falha ao iniciar";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Falha ao iniciar broker. Verifique se a porta não está em uso.");
                    Log("Dica: Execute como Administrador ou use porta > 1024");
                    _broker = null;
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

        private async Task PararBroker()
        {
            if (_broker != null)
            {
                await _broker.StopAsync();
                (_broker as IDisposable)?.Dispose();
                _broker = null;
            }

            btnIniciar.Text = "Iniciar";
            btnIniciar.BackColor = SystemColors.Control;
            lblStatus.Text = "Parado";
            lblStatus.ForeColor = Color.Gray;
            Log("Broker parado");
            AtualizarStats();
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

            AtualizarStats();
        }

        private void OnClientConnected(object? sender, BrokerClientConnectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnClientConnected(sender, e));
                return;
            }

            Log($"Cliente conectado: {e.ClientId}");
            AtualizarStats();
        }

        private void OnClientDisconnected(object? sender, BrokerClientDisconnectedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnClientDisconnected(sender, e));
                return;
            }

            Log($"Cliente desconectado: {e.ClientId} ({e.Reason})");
            AtualizarStats();
        }

        private async void btnPublicar_Click(object? sender, EventArgs e)
        {
            if (_broker?.IsRunning != true)
            {
                MessageBox.Show("Inicie o broker primeiro", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var topico = txtPubTopico.Text.Trim();
            var payload = txtPubPayload.Text.Trim();
            var qos = cmbQoS.SelectedIndex;

            if (string.IsNullOrEmpty(topico))
            {
                MessageBox.Show("Informe o tópico", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = await _broker.PublishAsync(topico, payload, qos);
            Log(result ? $"Publicado em {topico}" : "Falha ao publicar");
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_broker == null) return;

            using var dlg = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"broker_stats_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var stats = _broker.GetStats();
                var json = System.Text.Json.JsonSerializer.Serialize(stats, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(dlg.FileName, json);
                Log($"Estatísticas salvas em: {dlg.FileName}");
            }
        }

        private void AtualizarStats()
        {
            if (_broker == null)
            {
                lblClientes.Text = "Clientes: 0";
                lblMensagens.Text = "Mensagens: 0";
                return;
            }

            var stats = _broker.GetStats();
            lblClientes.Text = $"Clientes: {stats.ConnectedClientsCount}";
            lblMensagens.Text = $"Mensagens: {stats.TotalMessagesReceived}";
        }

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(message)); return; }
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
