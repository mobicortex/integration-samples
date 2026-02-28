using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para configuração de rede
    /// Testa endpoints: GET/PUT /api/network
    /// </summary>
    public partial class NetworkControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        
        // Ethernet
        private CheckBox _chkEthDhcp = null!;
        private TextBox _txtEthIp = null!;
        private TextBox _txtEthMask = null!;
        private TextBox _txtEthGateway = null!;
        
        // WiFi
        private CheckBox _chkWifiEnabled = null!;
        private TextBox _txtWifiSsid = null!;
        private TextBox _txtWifiPassword = null!;
        private CheckBox _chkWifiDhcp = null!;
        private TextBox _txtWifiIp = null!;
        private TextBox _txtWifiMask = null!;
        private TextBox _txtWifiGateway = null!;
        
        // Server
        private TextBox _txtServerName = null!;
        private TextBox _txtServerPort = null!;

        private Button _btnCarregar = null!;
        private Button _btnSalvar = null!;

        public NetworkControl()
        {
            InitializeComponent();
        }

        public void SetApiService(MobiCortexApiService apiService)
        {
            _apiService = apiService;
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoScroll = true,
                BackColor = Color.White
            };

            int currentY = 10;

            // Grupo Ethernet
            var grpEthernet = CreateGroupBox("🌐 Configuração Ethernet", 10, currentY, 400, 180);
            int y = 25;

            _chkEthDhcp = new CheckBox { Text = "Usar DHCP", Location = new Point(10, y), AutoSize = true };
            _chkEthDhcp.CheckedChanged += (s, e) => UpdateEthernetFields();
            y += 30;

            _txtEthIp = CreateTextBox("IP:", 10, y, 120, 150);
            y += 30;
            _txtEthMask = CreateTextBox("Máscara:", 10, y, 120, 150);
            y += 30;
            _txtEthGateway = CreateTextBox("Gateway:", 10, y, 120, 150);

            grpEthernet.Controls.AddRange(new Control[] { _chkEthDhcp, _txtEthIp, _txtEthMask, _txtEthGateway });
            panel.Controls.Add(grpEthernet);
            currentY += 190;

            // Grupo WiFi
            var grpWifi = CreateGroupBox("📶 Configuração WiFi", 10, currentY, 400, 240);
            y = 25;

            _chkWifiEnabled = new CheckBox { Text = "Habilitar WiFi", Location = new Point(10, y), AutoSize = true };
            _chkWifiEnabled.CheckedChanged += (s, e) => UpdateWifiFields();
            y += 30;

            _txtWifiSsid = CreateTextBox("SSID:", 10, y, 120, 200);
            y += 30;
            _txtWifiPassword = CreateTextBox("Senha:", 10, y, 120, 200);
            _txtWifiPassword.PasswordChar = '*';
            y += 30;

            _chkWifiDhcp = new CheckBox { Text = "Usar DHCP", Location = new Point(120, y), AutoSize = true };
            _chkWifiDhcp.CheckedChanged += (s, e) => UpdateWifiFields();
            y += 30;

            _txtWifiIp = CreateTextBox("IP:", 10, y, 120, 150);
            y += 30;
            _txtWifiMask = CreateTextBox("Máscara:", 10, y, 120, 150);
            y += 30;
            _txtWifiGateway = CreateTextBox("Gateway:", 10, y, 120, 150);

            grpWifi.Controls.AddRange(new Control[] { _chkWifiEnabled, _txtWifiSsid, _txtWifiPassword, _chkWifiDhcp, _txtWifiIp, _txtWifiMask, _txtWifiGateway });
            panel.Controls.Add(grpWifi);
            currentY += 250;

            // Grupo Servidor
            var grpServer = CreateGroupBox("🖥️ Configuração do Servidor", 10, currentY, 400, 120);
            y = 25;

            _txtServerName = CreateTextBox("Nome:", 10, y, 120, 200);
            y += 30;
            _txtServerPort = CreateTextBox("Porta:", 10, y, 120, 100);

            grpServer.Controls.AddRange(new Control[] { _txtServerName, _txtServerPort });
            panel.Controls.Add(grpServer);
            currentY += 130;

            // Botões
            var btnPanel = new Panel { Location = new Point(10, currentY), Size = new Size(400, 50) };
            
            _btnCarregar = new Button
            {
                Text = "🔄 Carregar Config",
                Location = new Point(0, 5),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnCarregar.Click += async (s, e) => await LoadConfigAsync();

            _btnSalvar = new Button
            {
                Text = "💾 Salvar Config",
                Location = new Point(160, 5),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnSalvar.Click += async (s, e) => await SaveConfigAsync();

            btnPanel.Controls.AddRange(new Control[] { _btnCarregar, _btnSalvar });
            panel.Controls.Add(btnPanel);

            this.Controls.Add(panel);
        }

        private GroupBox CreateGroupBox(string title, int x, int y, int width, int height)
        {
            return new GroupBox
            {
                Text = title,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
        }

        private TextBox CreateTextBox(string label, int x, int y, int lblWidth, int txtWidth)
        {
            var lbl = new Label
            {
                Text = label,
                Location = new Point(x, y + 3),
                Size = new Size(lblWidth - 5, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            var txt = new TextBox
            {
                Location = new Point(x + lblWidth, y),
                Size = new Size(txtWidth, 25),
                Font = new Font("Segoe UI", 9)
            };
            
            // Adiciona controle pai posteriormente
            var parent = new Panel();
            parent.Controls.Add(lbl);
            parent.Controls.Add(txt);
            
            return txt;
        }

        private void UpdateEthernetFields()
        {
            bool dhcp = _chkEthDhcp.Checked;
            _txtEthIp.Enabled = !dhcp;
            _txtEthMask.Enabled = !dhcp;
            _txtEthGateway.Enabled = !dhcp;
        }

        private void UpdateWifiFields()
        {
            bool enabled = _chkWifiEnabled.Checked;
            bool dhcp = _chkWifiDhcp.Checked;
            
            _txtWifiSsid.Enabled = enabled;
            _txtWifiPassword.Enabled = enabled;
            _chkWifiDhcp.Enabled = enabled;
            _txtWifiIp.Enabled = enabled && !dhcp;
            _txtWifiMask.Enabled = enabled && !dhcp;
            _txtWifiGateway.Enabled = enabled && !dhcp;
        }

        private async Task LoadConfigAsync()
        {
            var result = await _apiService.GetNetworkConfigAsync();
            if (result.Success && result.Data != null)
            {
                // Ethernet
                _chkEthDhcp.Checked = result.Data.Ethernet.Dhcp;
                _txtEthIp.Text = result.Data.Ethernet.Ip;
                _txtEthMask.Text = result.Data.Ethernet.Mask;
                _txtEthGateway.Text = result.Data.Ethernet.Gateway;
                UpdateEthernetFields();

                // WiFi
                _chkWifiEnabled.Checked = result.Data.WiFi.Enabled;
                _txtWifiSsid.Text = result.Data.WiFi.Ssid;
                _txtWifiPassword.Text = result.Data.WiFi.Password;
                _chkWifiDhcp.Checked = result.Data.WiFi.Dhcp;
                _txtWifiIp.Text = result.Data.WiFi.Ip;
                _txtWifiMask.Text = result.Data.WiFi.Mask;
                _txtWifiGateway.Text = result.Data.WiFi.Gateway;
                UpdateWifiFields();

                // Server
                _txtServerName.Text = result.Data.Server.Name;
                _txtServerPort.Text = result.Data.Server.Port;

                MessageBox.Show("Configuração carregada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Erro: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SaveConfigAsync()
        {
            var ethernet = new NetworkInterface
            {
                Dhcp = _chkEthDhcp.Checked,
                Ip = _txtEthIp.Text,
                Mask = _txtEthMask.Text,
                Gateway = _txtEthGateway.Text
            };

            var wifi = new WiFiConfig
            {
                Enabled = _chkWifiEnabled.Checked,
                Ssid = _txtWifiSsid.Text,
                Password = _txtWifiPassword.Text,
                Dhcp = _chkWifiDhcp.Checked,
                Ip = _txtWifiIp.Text,
                Mask = _txtWifiMask.Text,
                Gateway = _txtWifiGateway.Text
            };

            var server = new ServerConfig
            {
                Name = _txtServerName.Text,
                Port = _txtServerPort.Text
            };

            var result = await _apiService.UpdateNetworkConfigAsync(ethernet, wifi, server);
            if (result.Success)
            {
                MessageBox.Show("Configuração salva!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Erro: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnConnected()
        {
            _ = LoadConfigAsync();
        }
    }
}
