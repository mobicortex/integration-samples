namespace SmartSdk
{
    partial class MainForm
    {
        // Controles do formulário - expostos para o Visual Studio Designer
        private System.Windows.Forms.TextBox _txtLog;
        private System.Windows.Forms.TextBox _txtServerUrl;
        private System.Windows.Forms.TextBox _txtUser;
        private System.Windows.Forms.TextBox _txtPassword;
        private System.Windows.Forms.Button _btnLogin;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage tabCadastros;
        private System.Windows.Forms.TabPage tabVehicles;
        private System.Windows.Forms.TabPage tabEvents;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.TabPage tabNetwork;
        private System.Windows.Forms.TabPage tabWebSocket;
        private System.Windows.Forms.TabPage tabMqtt;
        private System.Windows.Forms.TabPage tabTests;
        private SmartSdk.Controls.CadastrosControl cadastrosControl;
        private SmartSdk.Controls.VehiclesControl vehiclesControl;
        private SmartSdk.Controls.EventsControl eventsControl;
        private SmartSdk.Controls.LogsControl logsControl;
        private SmartSdk.Controls.NetworkControl networkControl;
        private SmartSdk.Controls.WebSocketControl wsControl;
        private SmartSdk.Controls.MqttControl mqttControl;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel logPanel;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnClearLog;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _txtLog = new TextBox();
            _txtServerUrl = new TextBox();
            _txtUser = new TextBox();
            _txtPassword = new TextBox();
            _btnLogin = new Button();
            _lblStatus = new Label();
            _tabControl = new TabControl();
            tabCadastros = new TabPage();
            cadastrosControl = new SmartSdk.Controls.CadastrosControl();
            tabVehicles = new TabPage();
            vehiclesControl = new SmartSdk.Controls.VehiclesControl();
            tabEvents = new TabPage();
            eventsControl = new SmartSdk.Controls.EventsControl();
            tabLogs = new TabPage();
            logsControl = new SmartSdk.Controls.LogsControl();
            tabNetwork = new TabPage();
            networkControl = new SmartSdk.Controls.NetworkControl();
            tabWebSocket = new TabPage();
            wsControl = new SmartSdk.Controls.WebSocketControl();
            tabMqtt = new TabPage();
            mqttControl = new SmartSdk.Controls.MqttControl();
            tabTests = new TabPage();
            topPanel = new Panel();
            lblInfo = new Label();
            lblPassword = new Label();
            lblUser = new Label();
            lblServer = new Label();
            splitContainer = new SplitContainer();
            logPanel = new Panel();
            btnClearLog = new Button();
            lblLog = new Label();
            _tabControl.SuspendLayout();
            tabCadastros.SuspendLayout();
            tabVehicles.SuspendLayout();
            tabEvents.SuspendLayout();
            tabLogs.SuspendLayout();
            tabNetwork.SuspendLayout();
            tabWebSocket.SuspendLayout();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            logPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _txtLog
            // 
            _txtLog.BackColor = Color.FromArgb(30, 30, 30);
            _txtLog.Dock = DockStyle.Fill;
            _txtLog.Font = new Font("Consolas", 9F);
            _txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            _txtLog.Location = new Point(5, 25);
            _txtLog.Margin = new Padding(3, 3, 3, 5);
            _txtLog.Multiline = true;
            _txtLog.Name = "_txtLog";
            _txtLog.ReadOnly = true;
            _txtLog.ScrollBars = ScrollBars.Vertical;
            _txtLog.Size = new Size(1166, 61);
            _txtLog.TabIndex = 1;
            // 
            // _txtServerUrl
            // 
            _txtServerUrl.Location = new Point(95, 8);
            _txtServerUrl.Name = "_txtServerUrl";
            _txtServerUrl.Size = new Size(200, 23);
            _txtServerUrl.TabIndex = 1;
            _txtServerUrl.Text = "https://192.168.120.45";
            // 
            // _txtUser
            // 
            _txtUser.Location = new Point(365, 8);
            _txtUser.Name = "_txtUser";
            _txtUser.Size = new Size(100, 23);
            _txtUser.TabIndex = 3;
            _txtUser.Text = "master";
            // 
            // _txtPassword
            // 
            _txtPassword.Location = new Point(525, 8);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Size = new Size(100, 23);
            _txtPassword.TabIndex = 5;
            _txtPassword.Text = "admin";
            _txtPassword.UseSystemPasswordChar = true;
            // 
            // _btnLogin
            // 
            _btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            _btnLogin.FlatStyle = FlatStyle.Flat;
            _btnLogin.ForeColor = Color.White;
            _btnLogin.Location = new Point(635, 6);
            _btnLogin.Name = "_btnLogin";
            _btnLogin.Size = new Size(100, 28);
            _btnLogin.TabIndex = 6;
            _btnLogin.Text = "Conectar";
            _btnLogin.UseVisualStyleBackColor = false;
            _btnLogin.Click += OnLoginClick;
            // 
            // _lblStatus
            // 
            _lblStatus.ForeColor = Color.FromArgb(180, 0, 0);
            _lblStatus.Location = new Point(10, 40);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(700, 20);
            _lblStatus.TabIndex = 7;
            _lblStatus.Text = "⚠ Não conectado";
            // 
            // _tabControl
            // 
            _tabControl.Controls.Add(tabCadastros);
            _tabControl.Controls.Add(tabVehicles);
            _tabControl.Controls.Add(tabEvents);
            _tabControl.Controls.Add(tabLogs);
            _tabControl.Controls.Add(tabNetwork);
            _tabControl.Controls.Add(tabWebSocket);
            _tabControl.Controls.Add(tabMqtt);
            _tabControl.Controls.Add(tabTests);
            _tabControl.Dock = DockStyle.Fill;
            _tabControl.Font = new Font("Segoe UI", 10F);
            _tabControl.Location = new Point(0, 0);
            _tabControl.Name = "_tabControl";
            _tabControl.SelectedIndex = 0;
            _tabControl.Size = new Size(1176, 538);
            _tabControl.TabIndex = 0;
            // 
            // tabCadastros
            // 
            tabCadastros.Controls.Add(cadastrosControl);
            tabCadastros.Location = new Point(4, 26);
            tabCadastros.Name = "tabCadastros";
            tabCadastros.Size = new Size(1168, 508);
            tabCadastros.TabIndex = 0;
            tabCadastros.Text = "📁 Cadastros";
            tabCadastros.UseVisualStyleBackColor = true;
            // 
            // cadastrosControl
            // 
            cadastrosControl.Dock = DockStyle.Fill;
            cadastrosControl.Location = new Point(0, 0);
            cadastrosControl.Name = "cadastrosControl";
            cadastrosControl.Padding = new Padding(10);
            cadastrosControl.Size = new Size(1168, 508);
            cadastrosControl.TabIndex = 0;
            cadastrosControl.Load += cadastrosControl_Load;
            // 
            // tabVehicles
            // 
            tabVehicles.Controls.Add(vehiclesControl);
            tabVehicles.Location = new Point(4, 26);
            tabVehicles.Name = "tabVehicles";
            tabVehicles.Size = new Size(1168, 508);
            tabVehicles.TabIndex = 1;
            tabVehicles.Text = "🚗 Veículos";
            tabVehicles.UseVisualStyleBackColor = true;
            // 
            // vehiclesControl
            // 
            vehiclesControl.Dock = DockStyle.Fill;
            vehiclesControl.Location = new Point(0, 0);
            vehiclesControl.Name = "vehiclesControl";
            vehiclesControl.Size = new Size(1168, 508);
            vehiclesControl.TabIndex = 0;
            // 
            // tabEvents
            // 
            tabEvents.Controls.Add(eventsControl);
            tabEvents.Location = new Point(4, 26);
            tabEvents.Name = "tabEvents";
            tabEvents.Size = new Size(1168, 508);
            tabEvents.TabIndex = 2;
            tabEvents.Text = "📡 Eventos";
            tabEvents.UseVisualStyleBackColor = true;
            // 
            // eventsControl
            // 
            eventsControl.Dock = DockStyle.Fill;
            eventsControl.Location = new Point(0, 0);
            eventsControl.Name = "eventsControl";
            eventsControl.Size = new Size(1168, 508);
            eventsControl.TabIndex = 0;
            // 
            // tabLogs
            // 
            tabLogs.Controls.Add(logsControl);
            tabLogs.Location = new Point(4, 26);
            tabLogs.Name = "tabLogs";
            tabLogs.Size = new Size(1168, 508);
            tabLogs.TabIndex = 3;
            tabLogs.Text = "📋 Logs";
            tabLogs.UseVisualStyleBackColor = true;
            // 
            // logsControl
            // 
            logsControl.Dock = DockStyle.Fill;
            logsControl.Location = new Point(0, 0);
            logsControl.Name = "logsControl";
            logsControl.Size = new Size(1168, 508);
            logsControl.TabIndex = 0;
            // 
            // tabNetwork
            // 
            tabNetwork.Controls.Add(networkControl);
            tabNetwork.Location = new Point(4, 26);
            tabNetwork.Name = "tabNetwork";
            tabNetwork.Size = new Size(1168, 508);
            tabNetwork.TabIndex = 4;
            tabNetwork.Text = "🌐 Rede";
            tabNetwork.UseVisualStyleBackColor = true;
            // 
            // networkControl
            // 
            networkControl.Dock = DockStyle.Fill;
            networkControl.Location = new Point(0, 0);
            networkControl.Name = "networkControl";
            networkControl.Size = new Size(1168, 508);
            networkControl.TabIndex = 0;
            // 
            // tabWebSocket
            // 
            tabWebSocket.Controls.Add(wsControl);
            tabWebSocket.Location = new Point(4, 26);
            tabWebSocket.Name = "tabWebSocket";
            tabWebSocket.Size = new Size(1168, 508);
            tabWebSocket.TabIndex = 5;
            tabWebSocket.Text = "🔌 WebSockets";
            tabWebSocket.UseVisualStyleBackColor = true;
            // 
            // wsControl
            // 
            wsControl.Dock = DockStyle.Fill;
            wsControl.Location = new Point(0, 0);
            wsControl.Name = "wsControl";
            wsControl.Size = new Size(1168, 508);
            wsControl.TabIndex = 0;
            // 
            // tabMqtt
            // 
            tabMqtt.Controls.Add(mqttControl);
            tabMqtt.Location = new Point(4, 26);
            tabMqtt.Name = "tabMqtt";
            tabMqtt.Size = new Size(1168, 508);
            tabMqtt.TabIndex = 6;
            tabMqtt.Text = "📡 MQTT";
            tabMqtt.UseVisualStyleBackColor = true;
            // 
            // mqttControl
            // 
            mqttControl.Dock = DockStyle.Fill;
            mqttControl.Location = new Point(0, 0);
            mqttControl.Name = "mqttControl";
            mqttControl.Size = new Size(1168, 508);
            mqttControl.TabIndex = 0;
            // 
            // tabTests
            // 
            tabTests.Location = new Point(4, 26);
            tabTests.Name = "tabTests";
            tabTests.Size = new Size(1168, 508);
            tabTests.TabIndex = 7;
            tabTests.Text = "\U0001f9ea Testes Rápidos";
            tabTests.UseVisualStyleBackColor = true;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(240, 240, 240);
            topPanel.Controls.Add(lblInfo);
            topPanel.Controls.Add(_lblStatus);
            topPanel.Controls.Add(_btnLogin);
            topPanel.Controls.Add(lblPassword);
            topPanel.Controls.Add(_txtPassword);
            topPanel.Controls.Add(lblUser);
            topPanel.Controls.Add(_txtUser);
            topPanel.Controls.Add(lblServer);
            topPanel.Controls.Add(_txtServerUrl);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(10);
            topPanel.Size = new Size(1176, 100);
            topPanel.TabIndex = 0;
            // 
            // lblInfo
            // 
            lblInfo.ForeColor = Color.Gray;
            lblInfo.Location = new Point(10, 65);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(700, 20);
            lblInfo.TabIndex = 8;
            lblInfo.Text = "Smart SDK v2.0 - Testador de API MobiCortex Master";
            // 
            // lblPassword
            // 
            lblPassword.Location = new Point(475, 10);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(40, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Senha:";
            // 
            // lblUser
            // 
            lblUser.Location = new Point(305, 10);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(50, 20);
            lblUser.TabIndex = 2;
            lblUser.Text = "Usuário:";
            // 
            // lblServer
            // 
            lblServer.Location = new Point(10, 10);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(80, 20);
            lblServer.TabIndex = 0;
            lblServer.Text = "IP da Master:";
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 100);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(_tabControl);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(logPanel);
            splitContainer.Size = new Size(1176, 661);
            splitContainer.SplitterDistance = 538;
            splitContainer.TabIndex = 1;
            // 
            // logPanel
            // 
            logPanel.Controls.Add(_txtLog);
            logPanel.Controls.Add(btnClearLog);
            logPanel.Controls.Add(lblLog);
            logPanel.Dock = DockStyle.Fill;
            logPanel.Location = new Point(0, 0);
            logPanel.Name = "logPanel";
            logPanel.Padding = new Padding(5);
            logPanel.Size = new Size(1176, 119);
            logPanel.TabIndex = 0;
            // 
            // btnClearLog
            // 
            btnClearLog.BackColor = Color.FromArgb(108, 117, 125);
            btnClearLog.Dock = DockStyle.Bottom;
            btnClearLog.FlatStyle = FlatStyle.Flat;
            btnClearLog.ForeColor = Color.White;
            btnClearLog.Location = new Point(5, 86);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(1166, 28);
            btnClearLog.TabIndex = 2;
            btnClearLog.Text = "🗑️ Limpar Log";
            btnClearLog.UseVisualStyleBackColor = false;
            btnClearLog.Click += OnClearLogClick;
            // 
            // lblLog
            // 
            lblLog.Dock = DockStyle.Top;
            lblLog.Location = new Point(5, 5);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(1166, 20);
            lblLog.TabIndex = 0;
            lblLog.Text = "📋 Log de Operações:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1176, 761);
            Controls.Add(splitContainer);
            Controls.Add(topPanel);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Smart SDK - MobiCortex Master API Tester";
            _tabControl.ResumeLayout(false);
            tabCadastros.ResumeLayout(false);
            tabVehicles.ResumeLayout(false);
            tabEvents.ResumeLayout(false);
            tabLogs.ResumeLayout(false);
            tabNetwork.ResumeLayout(false);
            tabWebSocket.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            logPanel.ResumeLayout(false);
            logPanel.PerformLayout();
            ResumeLayout(false);

        }
    }
}
