using System.Text.Json;
using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    // =============================================================================
    //  FORMULÁRIO PRINCIPAL - Launcher
    //
    //  Este é o ponto de entrada do aplicativo.
    //  Contém apenas a configuração de conexão (IP, usuário, senha)
    //  e botões que abrem cada formulário de demonstração.
    //
    //  A instância do MobiCortexApiService é compartilhada entre todos os forms.
    //  Cada form demonstra uma funcionalidade específica da API.
    // =============================================================================

    public partial class MainForm : Form
    {
        // Cliente da API - compartilhado entre todos os formulários
        private readonly MobiCortexClient _api;

        // Caminho do arquivo de configurações do usuário
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SmartSdk", "settings.json");

        public MainForm()
        {
            _api = new MobiCortexClient();
            InitializeComponent();
            CarregarConfiguracoes();
        }

        // =====================================================================
        //  PERSISTÊNCIA DE CONFIGURAÇÕES
        // =====================================================================

        private void CarregarConfiguracoes()
        {
            try
            {
                if (!File.Exists(SettingsPath)) return;
                var json = File.ReadAllText(SettingsPath);
                var cfg = JsonSerializer.Deserialize<AppSettings>(json);
                if (cfg == null) return;
                if (!string.IsNullOrEmpty(cfg.Ip)) txtIP.Text = cfg.Ip;
            }
            catch { /* ignora erros de leitura */ }
        }

        private void SalvarConfiguracoes()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
                var cfg = new AppSettings { Ip = txtIP.Text.Trim() };
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(cfg));
            }
            catch { /* ignora erros de escrita */ }
        }

        private record AppSettings
        {
            public string Ip { get; init; } = "";
        }

        // =====================================================================
        //  CONEXÃO
        // =====================================================================

        private async void btnConectar_Click(object? sender, EventArgs e)
        {
            // Monta a URL base a partir do IP informado
            var ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip)) { Erro("Informe o IP do controlador"); return; }

            // Adiciona porta padrão se não informada
            if (!ip.Contains(':')) ip += ":4449";
            if (!ip.StartsWith("https://")) ip = "https://" + ip;

            // Configura a URL base no serviço
            _api.ConfigureBaseUrl(ip);

            try
            {
                btnConectar.Enabled = false;
                btnConectar.Text = "Conectando...";
                lblStatus.Text = "Fazendo login...";
                lblStatus.ForeColor = Color.DarkBlue;

                // Faz login na API (POST /login com a senha)
                Log($"DEBUG: Tentando login em {txtIP.Text.Trim()}");
                var result = await _api.LoginAsync(txtSenha.Text);
                Log($"DEBUG: Login result - Success={result.Success}, HTTP={(result.RawResponse?.Substring(0, Math.Min(100, result.RawResponse?.Length ?? 0)) ?? "N/A")}");

                if (result.Success && result.Data?.Ret == 0)
                {
                    SalvarConfiguracoes();

                    // Login OK - habilita os botões de demo
                    lblStatus.Text = $"Conectado - Session: {result.Data.SessionKey?[..16]}...";
                    lblStatus.ForeColor = Color.DarkGreen;
                    btnConectar.Text = "Conectado";
                    btnConectar.BackColor = Color.FromArgb(40, 167, 69);
                    HabilitarBotoes(true);
                    Log($"Login OK! Expira em {result.Data.ExpiresIn}s");
                }
                else
                {
                    lblStatus.Text = $"Falha: {result.Message ?? "erro desconhecido"}";
                    lblStatus.ForeColor = Color.DarkRed;
                    btnConectar.Text = "Conectar";
                    btnConectar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro: {ex.Message}";
                lblStatus.ForeColor = Color.DarkRed;
                btnConectar.Text = "Conectar";
                btnConectar.Enabled = true;
            }
        }

        private void HabilitarBotoes(bool enabled)
        {
            btnCadastroCompleto.Enabled = enabled;
            btnCadastroSimples.Enabled = enabled;
            btnMonitoramento.Enabled = enabled;
            btnRede.Enabled = enabled;
            btnDashboard.Enabled = enabled;
            btnMqttCliente.Enabled = enabled; // Precisa de conexão prévia para obter session key
            // btnMqttBroker e btnWebhookServer não precisam de conexão - funcionam standalone
        }

        // =====================================================================
        //  BOTÕES - Abrir formulários de demonstração
        //  Cada botão abre um Form independente passando o serviço da API.
        // =====================================================================

        private void btnCadastroCompleto_Click(object? sender, EventArgs e)
        {
            // Abre o formulário do modelo MobiCortex (3 níveis: Cadastro → Entidade → Mídia)
            new FormCadastroCompleto(_api).Show();
        }

        private void btnCadastroSimples_Click(object? sender, EventArgs e)
        {
            // Abre o formulário do modelo simples (2 níveis: Entidade → Mídia)
            new FormCadastroSimples(_api).Show();
        }

        private void btnMonitoramento_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de monitoramento MQTT em tempo real
            new FormMonitoramento(_api).Show();
        }

        private void btnRede_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de configuração de rede
            new FormRede(_api).Show();
        }

        private void btnDashboard_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de dashboard (informações do dispositivo)
            new FormDashboard(_api).Show();
        }

        private void btnMqttCliente_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de teste MQTT Cliente (conecta ao broker da controladora)
            new FormMqttCliente(_api).Show();
        }

        private void btnMqttBroker_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de teste MQTT Broker (broker embutido)
            new FormMqttBroker().Show();
        }

        private void btnWebhookServer_Click(object? sender, EventArgs e)
        {
            // Abre o formulário de teste Webhook Server (recebe eventos HTTP)
            new FormWebhookServer(_api).Show();
        }

        private void btnAbrirInterfaceWeb_Click(object? sender, EventArgs e)
        {
            // Abre a interface web da controladora no navegador padrão
            var ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                Erro("Informe o IP do controlador primeiro");
                return;
            }

            // Monta a URL (usa https por padrão)
            var url = ip.StartsWith("http") ? ip : $"https://{ip}";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                Log($"Abrindo interface web: {url}");
            }
            catch (Exception ex)
            {
                Erro($"Erro ao abrir navegador: {ex.Message}");
            }
        }

        // =====================================================================
        //  LOG
        // =====================================================================

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(message)); return; }

            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            txtLog.AppendText($"[{ts}] {message}{Environment.NewLine}");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnLimparLog_Click(object? sender, EventArgs e) => txtLog.Clear();

        private void Erro(string msg) =>
            MessageBox.Show(msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}

