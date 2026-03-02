using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Forms
{
    // =============================================================================
    //  DASHBOARD - Informações do Controlador
    //
    //  Este formulário demonstra como obter informações do dispositivo
    //  e estatísticas gerais do controlador.
    //
    //  ENDPOINTS:
    //  GET /device-info → Hardware (modelo, firmware, CPU, memória, uptime)
    //  GET /dashboard   → Estatísticas (cadastros, pessoas, veículos, mídias)
    //  GET /central-registry/stats → Capacidade de armazenamento
    //
    //  Todas chamadas são GET simples, sem parâmetros.
    // =============================================================================

    public partial class FormDashboard : Form
    {
        private readonly MobiCortexApiService _api;

        public FormDashboard(MobiCortexApiService api)
        {
            _api = api;
            InitializeComponent();
        }

        // =====================================================================
        //  CARREGAR DADOS
        // =====================================================================

        private async void FormDashboard_Load(object? sender, EventArgs e)
        {
            await CarregarTudo();
        }

        /// <summary>
        /// Carrega todas as informações do controlador.
        /// </summary>
        private async Task CarregarTudo()
        {
            Log("Carregando informações do controlador...");

            // Executa as 3 chamadas em paralelo para eficiência
            var taskDevice = _api.ObterDeviceInfoAsync();
            var taskDashboard = _api.ObterDashboardAsync();
            var taskStats = _api.ObterEstatisticasAsync();

            await Task.WhenAll(taskDevice, taskDashboard, taskStats);

            // ---- Device Info ----
            var deviceResult = taskDevice.Result;
            if (deviceResult.Success && deviceResult.Data != null)
            {
                var d = deviceResult.Data;
                lblModelo.Text = d.HwModel;
                lblFirmware.Text = d.FwVersion;
                lblGid.Text = d.GidStr;
                lblUptime.Text = d.UptimeStr;
                lblCpu.Text = $"{d.CpuLoad1:F1}% | {d.CpuTempC:F0}°C";
                lblMemoria.Text = $"{d.MemUsedPct}%";
                Log($"Device: {d.HwModel} | FW: {d.FwVersion} | Uptime: {d.UptimeStr}");
            }
            else
            {
                Log($"Erro device-info: {deviceResult.Message}");
            }

            // ---- Dashboard ----
            var dashResult = taskDashboard.Result;
            if (dashResult.Success && dashResult.Data != null)
            {
                var s = dashResult.Data;
                lblCadastros.Text = s.Cadastros.ToString("N0");
                lblPessoas.Text = s.Pessoas.ToString("N0");
                lblVeiculos.Text = s.Veiculos.ToString("N0");
                lblMidias.Text = s.TotalMidias.ToString("N0");
                lblFacial.Text = s.Facial.ToString("N0");
                lblRfid.Text = s.Rfid.ToString("N0");
                lblLpr.Text = s.Lpr.ToString("N0");
                lblControle.Text = s.ControleRemoto.ToString("N0");
                Log($"Dashboard: {s.Cadastros} cadastros, {s.Pessoas} pessoas, {s.Veiculos} veículos");
            }
            else
            {
                Log($"Erro dashboard: {dashResult.Message}");
            }

            // ---- Stats (capacidade) ----
            var statsResult = taskStats.Result;
            if (statsResult.Success && statsResult.Data != null)
            {
                var st = statsResult.Data;
                lblCapacidade.Text = $"{st.CurrentTotal:N0} / {st.MaxCapacity:N0} ({st.UsagePercent:F1}%)";
                progressCapacidade.Value = Math.Min(100, (int)st.UsagePercent);
                Log($"Capacidade: {st.UsagePercent:F1}% utilizado");
            }
            else
            {
                Log($"Erro stats: {statsResult.Message}");
            }

            Log("Informações carregadas com sucesso.");
        }

        private async void btnAtualizar_Click(object? sender, EventArgs e)
        {
            await CarregarTudo();
        }

        // =====================================================================
        //  HELPERS
        // =====================================================================

        private void Log(string msg)
        {
            var ts = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{ts}] {msg}{Environment.NewLine}");
        }
    }
}
