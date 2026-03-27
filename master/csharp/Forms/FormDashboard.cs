using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    // =============================================================================
    //  DASHBOARD - Controller Information
    //
    //  This form demonstrates how to obtain device information
    //  and general statistics from the controller.
    //
    //  ENDPOINTS:
    //  GET /device-info -> Hardware (model, firmware, CPU, memory, uptime)
    //  GET /dashboard   -> Statistics (registries, people, vehicles, media)
    //  GET /central-registry/stats -> Storage capacity
    //
    //  All calls are simple GETs, with no parameters.
    // =============================================================================

    public partial class FormDashboard : Form
    {
        private IMobiCortexClient _api = null!;

        /// <summary>
        /// API service. Can be set via property for designer use.
        /// </summary>
        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

        /// <summary>
        /// Default constructor for Visual Studio Designer.
        /// </summary>
        public FormDashboard()
        {
            InitializeComponent();
        }

        public FormDashboard(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        // =====================================================================
        //  LOAD DATA
        // =====================================================================

        private async void FormDashboard_Load(object? sender, EventArgs e)
        {
            // In VS design mode, _api may be null - do not load data
            if (_api == null) return;
            await LoadAll();
        }

        /// <summary>
        /// Loads all controller information.
        /// </summary>
        private async Task LoadAll()
        {
            Log("Loading controller information...");

            // Execute all 3 calls in parallel for efficiency
            var taskDevice = _api.SystemInfo.GetDeviceInfoAsync();
            var taskDashboard = _api.SystemInfo.GetDashboardAsync();
            var taskStats = _api.Registries.GetStatisticsAsync();

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
                Log($"Error device-info: {deviceResult.Message}");
            }

            // ---- Dashboard ----
            var dashResult = taskDashboard.Result;
            if (dashResult.Success && dashResult.Data != null)
            {
                var s = dashResult.Data;
                var c = s.Counts;
                var m = s.Media;
                lblCadastros.Text = c.Registries.ToString("N0");
                lblPessoas.Text = c.People.ToString("N0");
                lblVeiculos.Text = c.Vehicles.ToString("N0");
                lblMidias.Text = c.MemoryRecords.ToString("N0");
                lblFacial.Text = m.Facial.ToString("N0");
                lblRfid.Text = m.Rfid.ToString("N0");
                lblLpr.Text = m.Lpr.ToString("N0");
                lblControle.Text = m.RemoteControl.ToString("N0");
                Log($"Dashboard: {c.Registries} registries, {c.People} people, {c.Vehicles} vehicles");
            }
            else
            {
                Log($"Error dashboard: {dashResult.Message}");
            }

            // ---- Stats (capacity) ----
            var statsResult = taskStats.Result;
            if (statsResult.Success && statsResult.Data != null)
            {
                var st = statsResult.Data;
                lblCapacidade.Text = $"{st.CurrentTotal:N0} / {st.MaxCapacity:N0} ({st.UsagePercent:F1}%)";
                progressCapacidade.Value = Math.Min(100, (int)st.UsagePercent);
                Log($"Capacity: {st.UsagePercent:F1}% used");
            }
            else
            {
                Log($"Error stats: {statsResult.Message}");
            }

            Log("Information loaded successfully.");
        }

        private async void btnAtualizar_Click(object? sender, EventArgs e)
        {
            await LoadAll();
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
