using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;
using MQTTnet.Protocol;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de teste do Cliente MQTT.
    /// Conecta ao broker MQTT da controladora MobiCortex.
    /// </summary>
    public partial class FormMqttCliente : Form
    {
        private IMqttClientService? _mqttClient;
        private readonly List<MqttMessageReceivedEventArgs> _messages = new();

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

        private void FormMqttCliente_Load(object? sender, EventArgs e)
        {
            // Tópicos padrão pré-selecionados
            chkEvents.Checked = true;
            chkLogs.Checked = false;
            chkSensors.Checked = false;
        }

        private async void btnConectar_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected == true)
            {
                await Desconectar();
                return;
            }

            await Conectar();
        }

        private async Task Conectar()
        {
            try
            {
                var wsUrl = txtWsUrl.Text.Trim();
                var sessionKey = txtSessionKey.Text.Trim();

                if (string.IsNullOrEmpty(wsUrl) || string.IsNullOrEmpty(sessionKey))
                {
                    MessageBox.Show("Informe a URL WebSocket e a Session Key", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnConectar.Enabled = false;
                btnConectar.Text = "Conectando...";

                // Coletar tópicos
                var topics = new List<string>();
                if (chkEvents.Checked) topics.Add("mbcortex/master/events/#");
                if (chkLogs.Checked) topics.Add("mbcortex/master/logs/#");
                if (chkSensors.Checked) topics.Add("mbcortex/master/sensors/#");
                if (chkStatus.Checked) topics.Add("mbcortex/master/status/#");
                if (!string.IsNullOrEmpty(txtTopicoCustom.Text))
                    topics.Add(txtTopicoCustom.Text.Trim());

                if (!topics.Any())
                {
                    MessageBox.Show("Selecione pelo menos um tópico", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnConectar.Enabled = true;
                    btnConectar.Text = "Conectar";
                    return;
                }

                _mqttClient = new MqttClientService();
                _mqttClient.MessageReceived += OnMqttMessageReceived;
                _mqttClient.Disconnected += OnMqttDisconnected;

                var connected = await _mqttClient.ConnectAsync(wsUrl, sessionKey, topics);

                if (connected)
                {
                    btnConectar.Text = "Desconectar";
                    btnConectar.BackColor = Color.FromArgb(220, 53, 69);
                    lblStatus.Text = "Conectado";
                    lblStatus.ForeColor = Color.DarkGreen;
                    Log("Conectado ao broker MQTT");
                    Log($"Subscrito em: {string.Join(", ", topics)}");
                }
                else
                {
                    lblStatus.Text = "Falha na conexão";
                    lblStatus.ForeColor = Color.DarkRed;
                    Log("Falha ao conectar ao broker MQTT");
                    _mqttClient = null;
                }
            }
            catch (Exception ex)
            {
                Log($"Erro: {ex.Message}");
                lblStatus.Text = "Erro";
                lblStatus.ForeColor = Color.DarkRed;
            }
            finally
            {
                btnConectar.Enabled = true;
            }
        }

        private async Task Desconectar()
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                (_mqttClient as IDisposable)?.Dispose();
                _mqttClient = null;
            }

            btnConectar.Text = "Conectar";
            btnConectar.BackColor = SystemColors.Control;
            lblStatus.Text = "Desconectado";
            lblStatus.ForeColor = Color.Gray;
            Log("Desconectado do broker MQTT");
        }

        private void OnMqttMessageReceived(object? sender, MqttMessageReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttMessageReceived(sender, e));
                return;
            }

            _messages.Add(e);

            // Formatar para exibição
            var timestamp = e.ReceivedAt.ToString("HH:mm:ss.fff");
            var shortPayload = e.Payload.Length > 100 
                ? e.Payload.Substring(0, 100) + "..." 
                : e.Payload;

            Log($"[{timestamp}] {e.Topic}");
            Log($"  QoS: {e.QosLevel} | Retain: {e.Retain}");
            Log($"  Payload: {shortPayload}");
            Log("");

            // Atualizar contador
            lblMensagens.Text = $"Mensagens: {_messages.Count}";
        }

        private void OnMqttDisconnected(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttDisconnected(sender, e));
                return;
            }

            Log("Conexão MQTT perdida!");
            lblStatus.Text = "Desconectado";
            lblStatus.ForeColor = Color.DarkRed;
            btnConectar.Text = "Conectar";
            btnConectar.BackColor = SystemColors.Control;
        }

        private async void btnPublicar_Click(object? sender, EventArgs e)
        {
            if (_mqttClient?.IsConnected != true)
            {
                MessageBox.Show("Conecte primeiro ao broker", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var topic = txtPubTopico.Text.Trim();
            var payload = txtPubPayload.Text.Trim();
            var qos = cmbPubQoS.SelectedIndex;

            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(payload))
            {
                MessageBox.Show("Informe tópico e payload", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = await _mqttClient.PublishAsync(topic, payload, qos);
            Log(result ? $"Publicado em {topic}" : "Falha ao publicar");
        }

        private void btnLimpar_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
            _messages.Clear();
            lblMensagens.Text = "Mensagens: 0";
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            if (_messages.Count == 0)
            {
                MessageBox.Show("Nenhuma mensagem para salvar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Log($"Mensagens salvas em: {dlg.FileName}");
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
