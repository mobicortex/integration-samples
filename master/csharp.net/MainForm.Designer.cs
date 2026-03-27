namespace SmartSdk
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnAbrirInterfaceWeb = new System.Windows.Forms.Button();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.lblTituloDemos = new System.Windows.Forms.Label();
            this.btnCadastroCompleto = new System.Windows.Forms.Button();
            this.lblDescCompleto = new System.Windows.Forms.Label();
            this.btnCadastroSimples = new System.Windows.Forms.Button();
            this.lblDescSimples = new System.Windows.Forms.Label();
            this.btnMonitoramento = new System.Windows.Forms.Button();
            this.lblDescMonitoramento = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.lblDescDashboard = new System.Windows.Forms.Label();
            this.btnMqttCliente = new System.Windows.Forms.Button();
            this.lblDescMqttCliente = new System.Windows.Forms.Label();
            this.btnMqttBroker = new System.Windows.Forms.Button();
            this.lblDescMqttBroker = new System.Windows.Forms.Label();
            this.btnWebhookServer = new System.Windows.Forms.Button();
            this.lblDescWebhookServer = new System.Windows.Forms.Label();
            this.panelLog = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnLimparLog = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Controls.Add(this.btnConectar);
            this.panelTop.Controls.Add(this.lblSenha);
            this.panelTop.Controls.Add(this.txtSenha);
            this.panelTop.Controls.Add(this.lblUsuario);
            this.panelTop.Controls.Add(this.txtUsuario);
            this.panelTop.Controls.Add(this.lblIP);
            this.panelTop.Controls.Add(this.txtIP);
            this.panelTop.Controls.Add(this.btnAbrirInterfaceWeb);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(13, 9, 13, 9);
            this.panelTop.Size = new System.Drawing.Size(672, 61);
            this.panelTop.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(385, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(81, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Not connected";
            // 
            // btnConectar
            // 
            this.btnConectar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnConectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar.ForeColor = System.Drawing.Color.White;
            this.btnConectar.Location = new System.Drawing.Point(385, 10);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(94, 24);
            this.btnConectar.TabIndex = 6;
            this.btnConectar.Text = "Connect";
            this.btnConectar.UseVisualStyleBackColor = false;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Location = new System.Drawing.Point(287, 13);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(41, 13);
            this.lblSenha.TabIndex = 4;
            this.lblSenha.Text = "Password:";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(287, 30);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(86, 20);
            this.txtSenha.TabIndex = 5;
            this.txtSenha.Text = "admin";
            this.txtSenha.UseSystemPasswordChar = true;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(193, 13);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "User:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Location = new System.Drawing.Point(193, 30);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(86, 20);
            this.txtUsuario.TabIndex = 3;
            this.txtUsuario.Text = "master";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(13, 13);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(92, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "Controller IP:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(13, 30);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(172, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.120.45";
            // 
            // btnAbrirInterfaceWeb
            // 
            this.btnAbrirInterfaceWeb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(201)))), ((int)(((byte)(151)))));
            this.btnAbrirInterfaceWeb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirInterfaceWeb.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAbrirInterfaceWeb.ForeColor = System.Drawing.Color.White;
            this.btnAbrirInterfaceWeb.Location = new System.Drawing.Point(493, 10);
            this.btnAbrirInterfaceWeb.Name = "btnAbrirInterfaceWeb";
            this.btnAbrirInterfaceWeb.Size = new System.Drawing.Size(111, 24);
            this.btnAbrirInterfaceWeb.TabIndex = 8;
            this.btnAbrirInterfaceWeb.Text = "🌐 Interface Web";
            this.btnAbrirInterfaceWeb.UseVisualStyleBackColor = false;
            this.btnAbrirInterfaceWeb.Click += new System.EventHandler(this.btnAbrirInterfaceWeb_Click);
            // 
            // panelBotoes
            // 
            this.panelBotoes.AutoScroll = true;
            this.panelBotoes.Controls.Add(this.lblTituloDemos);
            this.panelBotoes.Controls.Add(this.btnCadastroCompleto);
            this.panelBotoes.Controls.Add(this.lblDescCompleto);
            this.panelBotoes.Controls.Add(this.btnCadastroSimples);
            this.panelBotoes.Controls.Add(this.lblDescSimples);
            this.panelBotoes.Controls.Add(this.btnMonitoramento);
            this.panelBotoes.Controls.Add(this.lblDescMonitoramento);
            this.panelBotoes.Controls.Add(this.btnDashboard);
            this.panelBotoes.Controls.Add(this.lblDescDashboard);
            this.panelBotoes.Controls.Add(this.btnMqttCliente);
            this.panelBotoes.Controls.Add(this.lblDescMqttCliente);
            this.panelBotoes.Controls.Add(this.btnMqttBroker);
            this.panelBotoes.Controls.Add(this.lblDescMqttBroker);
            this.panelBotoes.Controls.Add(this.btnWebhookServer);
            this.panelBotoes.Controls.Add(this.lblDescWebhookServer);
            this.panelBotoes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBotoes.Location = new System.Drawing.Point(0, 61);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.panelBotoes.Size = new System.Drawing.Size(672, 451);
            this.panelBotoes.TabIndex = 1;
            // 
            // lblTituloDemos
            // 
            this.lblTituloDemos.AutoSize = true;
            this.lblTituloDemos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloDemos.Location = new System.Drawing.Point(13, 13);
            this.lblTituloDemos.Name = "lblTituloDemos";
            this.lblTituloDemos.Size = new System.Drawing.Size(273, 21);
            this.lblTituloDemos.TabIndex = 0;
            this.lblTituloDemos.Text = "API Integration Examples";
            // 
            // btnCadastroCompleto
            // 
            this.btnCadastroCompleto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnCadastroCompleto.Enabled = false;
            this.btnCadastroCompleto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroCompleto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCadastroCompleto.ForeColor = System.Drawing.Color.White;
            this.btnCadastroCompleto.Location = new System.Drawing.Point(13, 48);
            this.btnCadastroCompleto.Name = "btnCadastroCompleto";
            this.btnCadastroCompleto.Size = new System.Drawing.Size(214, 35);
            this.btnCadastroCompleto.TabIndex = 1;
            this.btnCadastroCompleto.Text = "Full Registry (MobiCortex)";
            this.btnCadastroCompleto.UseVisualStyleBackColor = false;
            this.btnCadastroCompleto.Click += new System.EventHandler(this.btnCadastroCompleto_Click);
            // 
            // lblDescCompleto
            // 
            this.lblDescCompleto.ForeColor = System.Drawing.Color.Gray;
            this.lblDescCompleto.Location = new System.Drawing.Point(236, 48);
            this.lblDescCompleto.Name = "lblDescCompleto";
            this.lblDescCompleto.Size = new System.Drawing.Size(420, 35);
            this.lblDescCompleto.TabIndex = 2;
            this.lblDescCompleto.Text = "3-level hierarchical model: Central Registry → Entity (person/vehicle) → Access Me" +
    "dia. Ideal for condominiums and companies.";
            // 
            // btnCadastroSimples
            // 
            this.btnCadastroSimples.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCadastroSimples.Enabled = false;
            this.btnCadastroSimples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastroSimples.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCadastroSimples.ForeColor = System.Drawing.Color.White;
            this.btnCadastroSimples.Location = new System.Drawing.Point(13, 95);
            this.btnCadastroSimples.Name = "btnCadastroSimples";
            this.btnCadastroSimples.Size = new System.Drawing.Size(214, 35);
            this.btnCadastroSimples.TabIndex = 3;
            this.btnCadastroSimples.Text = "Simplified Registry";
            this.btnCadastroSimples.UseVisualStyleBackColor = false;
            this.btnCadastroSimples.Click += new System.EventHandler(this.btnCadastroSimples_Click);
            // 
            // lblDescSimples
            // 
            this.lblDescSimples.ForeColor = System.Drawing.Color.Gray;
            this.lblDescSimples.Location = new System.Drawing.Point(236, 95);
            this.lblDescSimples.Name = "lblDescSimples";
            this.lblDescSimples.Size = new System.Drawing.Size(420, 35);
            this.lblDescSimples.TabIndex = 4;
            this.lblDescSimples.Text = "Simplified 2-level model: Person/Vehicle → Media. IDs automatically generated" +
    " by the controller (createid=true).";
            // 
            // btnMonitoramento
            // 
            this.btnMonitoramento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnMonitoramento.Enabled = false;
            this.btnMonitoramento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMonitoramento.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMonitoramento.ForeColor = System.Drawing.Color.Black;
            this.btnMonitoramento.Location = new System.Drawing.Point(13, 143);
            this.btnMonitoramento.Name = "btnMonitoramento";
            this.btnMonitoramento.Size = new System.Drawing.Size(214, 35);
            this.btnMonitoramento.TabIndex = 5;
            this.btnMonitoramento.Text = "Monitoring (MQTT)";
            this.btnMonitoramento.UseVisualStyleBackColor = false;
            this.btnMonitoramento.Click += new System.EventHandler(this.btnMonitoramento_Click);
            // 
            // lblDescMonitoramento
            // 
            this.lblDescMonitoramento.ForeColor = System.Drawing.Color.Gray;
            this.lblDescMonitoramento.Location = new System.Drawing.Point(236, 143);
            this.lblDescMonitoramento.Name = "lblDescMonitoramento";
            this.lblDescMonitoramento.Size = new System.Drawing.Size(420, 35);
            this.lblDescMonitoramento.TabIndex = 6;
            this.lblDescMonitoramento.Text = "Receives real-time events via MQTT over WebSocket. Monitors access, sensors " +
    "and controller status.";
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnDashboard.Enabled = false;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(13, 191);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(214, 35);
            this.btnDashboard.TabIndex = 7;
            this.btnDashboard.Text = "Dashboard / Device Info";
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // lblDescDashboard
            // 
            this.lblDescDashboard.ForeColor = System.Drawing.Color.Gray;
            this.lblDescDashboard.Location = new System.Drawing.Point(236, 191);
            this.lblDescDashboard.Name = "lblDescDashboard";
            this.lblDescDashboard.Size = new System.Drawing.Size(420, 35);
            this.lblDescDashboard.TabIndex = 8;
            this.lblDescDashboard.Text = "Displays controller information: model, firmware, CPU, memory, registry and" +
    " media statistics.";
            // 
            // btnMqttCliente
            // 
            this.btnMqttCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.btnMqttCliente.Enabled = false;
            this.btnMqttCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMqttCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMqttCliente.ForeColor = System.Drawing.Color.White;
            this.btnMqttCliente.Location = new System.Drawing.Point(13, 238);
            this.btnMqttCliente.Name = "btnMqttCliente";
            this.btnMqttCliente.Size = new System.Drawing.Size(214, 35);
            this.btnMqttCliente.TabIndex = 9;
            this.btnMqttCliente.Text = "MQTT Client → Controller";
            this.btnMqttCliente.UseVisualStyleBackColor = false;
            this.btnMqttCliente.Click += new System.EventHandler(this.btnMqttCliente_Click);
            // 
            // lblDescMqttCliente
            // 
            this.lblDescMqttCliente.ForeColor = System.Drawing.Color.Gray;
            this.lblDescMqttCliente.Location = new System.Drawing.Point(236, 238);
            this.lblDescMqttCliente.Name = "lblDescMqttCliente";
            this.lblDescMqttCliente.Size = new System.Drawing.Size(420, 35);
            this.lblDescMqttCliente.TabIndex = 10;
            this.lblDescMqttCliente.Text = "Connects as MQTT client to the controller broker. Receives real-time events" +
    " via WebSocket.";
            // 
            // btnMqttBroker
            // 
            this.btnMqttBroker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnMqttBroker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMqttBroker.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMqttBroker.ForeColor = System.Drawing.Color.White;
            this.btnMqttBroker.Location = new System.Drawing.Point(13, 286);
            this.btnMqttBroker.Name = "btnMqttBroker";
            this.btnMqttBroker.Size = new System.Drawing.Size(214, 35);
            this.btnMqttBroker.TabIndex = 11;
            this.btnMqttBroker.Text = "MQTT Broker (Embedded)";
            this.btnMqttBroker.UseVisualStyleBackColor = false;
            this.btnMqttBroker.Click += new System.EventHandler(this.btnMqttBroker_Click);
            // 
            // lblDescMqttBroker
            // 
            this.lblDescMqttBroker.ForeColor = System.Drawing.Color.Gray;
            this.lblDescMqttBroker.Location = new System.Drawing.Point(236, 286);
            this.lblDescMqttBroker.Name = "lblDescMqttBroker";
            this.lblDescMqttBroker.Size = new System.Drawing.Size(420, 35);
            this.lblDescMqttBroker.TabIndex = 12;
            this.lblDescMqttBroker.Text = "Starts an embedded MQTT broker. The controller can connect directly to this" +
    " server.";
            // 
            // btnWebhookServer
            // 
            this.btnWebhookServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(126)))), ((int)(((byte)(20)))));
            this.btnWebhookServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWebhookServer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnWebhookServer.ForeColor = System.Drawing.Color.White;
            this.btnWebhookServer.Location = new System.Drawing.Point(13, 334);
            this.btnWebhookServer.Name = "btnWebhookServer";
            this.btnWebhookServer.Size = new System.Drawing.Size(214, 35);
            this.btnWebhookServer.TabIndex = 13;
            this.btnWebhookServer.Text = "Webhook Server (HTTP)";
            this.btnWebhookServer.UseVisualStyleBackColor = false;
            this.btnWebhookServer.Click += new System.EventHandler(this.btnWebhookServer_Click);
            // 
            // lblDescWebhookServer
            // 
            this.lblDescWebhookServer.ForeColor = System.Drawing.Color.Gray;
            this.lblDescWebhookServer.Location = new System.Drawing.Point(236, 334);
            this.lblDescWebhookServer.Name = "lblDescWebhookServer";
            this.lblDescWebhookServer.Size = new System.Drawing.Size(420, 35);
            this.lblDescWebhookServer.TabIndex = 14;
            this.lblDescWebhookServer.Text = "Starts an HTTP server to receive events via webhook. The controller sends PO" +
    "ST with JSON events.";
            // 
            // panelLog
            // 
            this.panelLog.Controls.Add(this.txtLog);
            this.panelLog.Controls.Add(this.btnLimparLog);
            this.panelLog.Controls.Add(this.lblLog);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLog.Location = new System.Drawing.Point(0, 512);
            this.panelLog.Name = "panelLog";
            this.panelLog.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelLog.Size = new System.Drawing.Size(672, 95);
            this.panelLog.TabIndex = 2;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtLog.Location = new System.Drawing.Point(4, 20);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(664, 47);
            this.txtLog.TabIndex = 1;
            // 
            // btnLimparLog
            // 
            this.btnLimparLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLimparLog.Location = new System.Drawing.Point(4, 67);
            this.btnLimparLog.Name = "btnLimparLog";
            this.btnLimparLog.Size = new System.Drawing.Size(664, 24);
            this.btnLimparLog.TabIndex = 2;
            this.btnLimparLog.Text = "Clear Log";
            this.btnLimparLog.UseVisualStyleBackColor = true;
            this.btnLimparLog.Click += new System.EventHandler(this.btnLimparLog_Click);
            // 
            // lblLog
            // 
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLog.Location = new System.Drawing.Point(4, 4);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(664, 16);
            this.lblLog.TabIndex = 0;
            this.lblLog.Text = "Operations Log:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 607);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(688, 439);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartSDK - MobiCortex Integration Examples";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.panelBotoes.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.panelLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelTop;
        private Label lblIP;
        private TextBox txtIP;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblSenha;
        private TextBox txtSenha;
        private Button btnConectar;
        private Button btnAbrirInterfaceWeb;
        private Label lblStatus;
        private Panel panelBotoes;
        private Label lblTituloDemos;
        private Button btnCadastroCompleto;
        private Label lblDescCompleto;
        private Button btnCadastroSimples;
        private Label lblDescSimples;
        private Button btnMonitoramento;
        private Label lblDescMonitoramento;
        private Button btnDashboard;
        private Label lblDescDashboard;
        private Button btnMqttCliente;
        private Label lblDescMqttCliente;
        private Button btnMqttBroker;
        private Label lblDescMqttBroker;
        private Button btnWebhookServer;
        private Label lblDescWebhookServer;
        private Panel panelLog;
        private Label lblLog;
        private TextBox txtLog;
        private Button btnLimparLog;
    }
}
