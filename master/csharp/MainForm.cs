using SmartSdk.Services;
using SmartSdk.Controls;

namespace SmartSdk
{
    public partial class MainForm : Form
    {
        private readonly MobiCortexApiService _apiService;
        private bool _isConnected = false;

        public MainForm()
        {
            _apiService = new MobiCortexApiService();
            _apiService.OnLog += Log;
            InitializeComponent();
            InitializeUserControls();
        }

        private void InitializeUserControls()
        {
            // Injetar o serviço API nos controles criados pelo Designer
            cadastrosControl.SetApiService(_apiService);
            vehiclesControl.SetApiService(_apiService);
            eventsControl.SetApiService(_apiService);
            logsControl.SetApiService(_apiService);
            networkControl.SetApiService(_apiService);
            wsControl.SetApiService(_apiService);
            mqttControl.SetApiService(_apiService);
        }

        private void OnLoginClick(object? sender, EventArgs e)
        {
            _ = BtnLogin_Click();
        }

        private async Task BtnLogin_Click()
        {
            if (_isConnected)
            {
                Log("Já está conectado. Usando sessão existente.");
                return;
            }

            try
            {
                var serverUrl = _txtServerUrl.Text.Trim();
                var user = _txtUser.Text.Trim();
                var password = _txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Preencha o IP da Master e a senha", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Adiciona porta padrão se não especificada
                if (!serverUrl.Contains(":"))
                {
                    serverUrl += ":4449";
                }

                if (!serverUrl.StartsWith("https://"))
                {
                    serverUrl = "https://" + serverUrl;
                }

                _btnLogin.Enabled = false;
                _btnLogin.Text = "Conectando...";
                _lblStatus.Text = "⏳ Conectando...";
                _lblStatus.ForeColor = Color.DarkBlue;
                Application.DoEvents();

                // Tentar login
                var loginResult = await _apiService.LoginAsync(password);

                if (loginResult.Success && loginResult.Data?.Ret == 0)
                {
                    _isConnected = true;
                    _lblStatus.Text = $"✓ Conectado: {user} | Session: {loginResult.Data.SessionKey?[..16]}...";
                    _lblStatus.ForeColor = Color.DarkGreen;
                    _btnLogin.Text = "Conectado ✓";
                    _btnLogin.BackColor = Color.FromArgb(40, 167, 69);
                    Log($"✓ Login bem-sucedido - Usuário: {user}");
                    Log($"  Session Key: {loginResult.Data.SessionKey?[..32]}...");
                    Log($"  Expira em: {loginResult.Data.ExpiresIn}s");

                    // Notificar controles da conexão
                    OnConnected();
                    
                    // Passar URL base para o controle MQTT (para WSS)
                    mqttControl.SetBaseUrl(serverUrl);
                }
                else
                {
                    var errorMsg = loginResult.Message ?? $"Erro de autenticação (ret: {loginResult.Data?.Ret})";
                    _lblStatus.Text = $"✗ Falha: {errorMsg}";
                    _lblStatus.ForeColor = Color.DarkRed;
                    _btnLogin.Text = "Conectar";
                    _btnLogin.Enabled = true;
                    Log($"✗ Login falhou: {errorMsg}");

                    if (!string.IsNullOrEmpty(loginResult.RawResponse))
                    {
                        Log($"  Resposta: {loginResult.RawResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"✗ Erro: {ex.Message}";
                _lblStatus.ForeColor = Color.DarkRed;
                _btnLogin.Text = "Conectar";
                _btnLogin.Enabled = true;
                Log($"✗ Erro durante login: {ex.Message}");
            }
        }

        private void OnClearLogClick(object? sender, EventArgs e)
        {
            _txtLog.Clear();
            Log("🗑️ Log limpo");
        }

        private void Log(string message)
        {
            if (_txtLog == null || _txtLog.IsDisposed) return;

            if (_txtLog.InvokeRequired)
            {
                _txtLog.Invoke(() => Log(message));
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _txtLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            _txtLog.SelectionStart = _txtLog.Text.Length;
            _txtLog.ScrollToCaret();
        }

        private void OnConnected()
        {
            cadastrosControl?.OnConnected();
            vehiclesControl?.OnConnected();
            eventsControl?.OnConnected();
            logsControl?.OnConnected();
            networkControl?.OnConnected();
            wsControl?.OnConnected();
            mqttControl?.OnConnected();
        }

        private void cadastrosControl_Load(object sender, EventArgs e)
        {

        }
    }
}
