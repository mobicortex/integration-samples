using System.Text.Json;
using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    // =============================================================================
    //  MAIN FORM - Launcher
    //
    //  This is the application entry point.
    //  Contains only the connection configuration (IP, user, password)
    //  and buttons that open each demonstration form.
    //
    //  The MobiCortexApiService instance is shared among all forms.
    //  Each form demonstrates a specific API feature.
    // =============================================================================

    public partial class MainForm : Form
    {
        // API client - shared among all forms
        private readonly MobiCortexClient _api;

        // User settings file path
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SmartSdk", "settings.json");

        public MainForm()
        {
            _api = new MobiCortexClient();
            InitializeComponent();
            LoadSettings();
        }

        // =====================================================================
        //  SETTINGS PERSISTENCE
        // =====================================================================

        private void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsPath)) return;
                var json = File.ReadAllText(SettingsPath);
                var cfg = JsonSerializer.Deserialize<AppSettings>(json);
                if (cfg == null) return;
                if (!string.IsNullOrEmpty(cfg.Ip)) txtIP.Text = cfg.Ip;
            }
            catch { /* ignore read errors */ }
        }

        private void SaveSettings()
        {
            try
            {
                var settingsDirectory = Path.GetDirectoryName(SettingsPath);
                if (!string.IsNullOrEmpty(settingsDirectory))
                {
                    Directory.CreateDirectory(settingsDirectory);
                }

                var cfg = new AppSettings();
                cfg.Ip = txtIP.Text.Trim();
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(cfg));
            }
            catch { /* ignore write errors */ }
        }

        private class AppSettings
        {
            public string Ip { get; set; }

            public AppSettings()
            {
                Ip = string.Empty;
            }
        }

        // =====================================================================
        //  CONNECTION
        // =====================================================================

        private async void btnConectar_Click(object sender, EventArgs e)
        {
            // Build the base URL from the provided IP
            var ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip)) { ShowError("Enter the controller IP"); return; }

            // Add default port if not provided
            if (!ip.Contains(':')) ip += ":4449";
            if (!ip.StartsWith("https://")) ip = "https://" + ip;

            // Configure the base URL in the service
            _api.ConfigureBaseUrl(ip);

            try
            {
                btnConectar.Enabled = false;
                btnConectar.Text = "Connecting...";
                lblStatus.Text = "Logging in...";
                lblStatus.ForeColor = Color.DarkBlue;

                // Log in to the API (POST /login with the password)
                Log($"DEBUG: Attempting login at {txtIP.Text.Trim()}");
                var result = await _api.LoginAsync(txtSenha.Text);
                Log($"DEBUG: Login result - Success={result.Success}, HTTP={(result.RawResponse?.Substring(0, Math.Min(100, result.RawResponse?.Length ?? 0)) ?? "N/A")}");

                if (result.Success && result.Data?.Ret == 0)
                {
                    SaveSettings();

                    // Login OK - enable demo buttons
                    lblStatus.Text = $"Connected - Session: {result.Data.SessionKey?.Substring(0, Math.Min(16, result.Data.SessionKey?.Length ?? 0))}...";
                    lblStatus.ForeColor = Color.DarkGreen;
                    btnConectar.Text = "Connected";
                    btnConectar.BackColor = Color.FromArgb(40, 167, 69);
                    EnableButtons(true);
                    Log($"Login OK! Expires in {result.Data.ExpiresIn}s");
                }
                else
                {
                    lblStatus.Text = $"Failure: {result.Message ?? "unknown error"}";
                    lblStatus.ForeColor = Color.DarkRed;
                    btnConectar.Text = "Connect";
                    btnConectar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = Color.DarkRed;
                btnConectar.Text = "Connect";
                btnConectar.Enabled = true;
            }
        }

        private void EnableButtons(bool enabled)
        {
            btnCadastroCompleto.Enabled = enabled;
            btnCadastroSimples.Enabled = enabled;
            btnMonitoramento.Enabled = enabled;
            btnDashboard.Enabled = enabled;
            btnMqttCliente.Enabled = enabled; // Requires prior connection to obtain session key
            // btnMqttBroker and btnWebhookServer do not require a connection - they work standalone
        }

        // =====================================================================
        //  BUTTONS - Open demonstration forms
        //  Each button opens an independent Form passing the API service.
        // =====================================================================

        private void btnCadastroCompleto_Click(object sender, EventArgs e)
        {
            // Opens the MobiCortex model form (3 levels: Registry -> Entity -> Media)
            new FormCadastroCompleto(_api).Show();
        }

        private void btnCadastroSimples_Click(object sender, EventArgs e)
        {
            // Opens the simple model form (2 levels: Entity -> Media)
            new FormCadastroSimples(_api).Show();
        }

        private void btnMonitoramento_Click(object sender, EventArgs e)
        {
            // Opens the real-time MQTT monitoring form
            new FormMonitoramento(_api).Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // Opens the dashboard form (device information)
            new FormDashboard(_api).Show();
        }

        private void btnMqttCliente_Click(object sender, EventArgs e)
        {
            // Opens the MQTT Client test form (connects to the controller broker)
            new FormMqttCliente(_api).Show();
        }

        private void btnMqttBroker_Click(object sender, EventArgs e)
        {
            // Opens the MQTT Broker test form (embedded broker)
            new FormMqttBroker().Show();
        }

        private void btnWebhookServer_Click(object sender, EventArgs e)
        {
            // Opens the Webhook Server test form (receives HTTP events)
            new FormWebhookServer(_api).Show();
        }

        private void btnAbrirInterfaceWeb_Click(object sender, EventArgs e)
        {
            // Opens the controller web interface in the default browser
            var ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                ShowError("Enter the controller IP first");
                return;
            }

            // Build the URL (uses https by default)
            var url = ip.StartsWith("http") ? ip : $"https://{ip}";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                Log($"Opening web interface: {url}");
            }
            catch (Exception ex)
            {
                ShowError($"Error opening browser: {ex.Message}");
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

        private void btnLimparLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        
        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}

