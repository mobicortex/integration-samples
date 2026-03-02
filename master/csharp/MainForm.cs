using SmartSdk.Services;
using SmartSdk.Forms;

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
        // Serviço da API - compartilhado entre todos os formulários
        private readonly MobiCortexApiService _api;

        public MainForm()
        {
            _api = new MobiCortexApiService();
            _api.OnLog += Log;
            InitializeComponent();
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
                lblStatus.Text = "Conectando...";
                lblStatus.ForeColor = Color.DarkBlue;

                // Faz login na API (POST /login com a senha)
                var result = await _api.LoginAsync(txtSenha.Text);

                if (result.Success && result.Data?.Ret == 0)
                {
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
    }
}
