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
            panelTop = new Panel();
            lblStatus = new Label();
            btnConectar = new Button();
            lblSenha = new Label();
            txtSenha = new TextBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblIP = new Label();
            txtIP = new TextBox();
            btnAbrirInterfaceWeb = new Button();
            panelBotoes = new Panel();
            lblTituloDemos = new Label();
            btnCadastroCompleto = new Button();
            lblDescCompleto = new Label();
            btnCadastroSimples = new Button();
            lblDescSimples = new Label();
            btnMonitoramento = new Button();
            lblDescMonitoramento = new Label();
            btnRede = new Button();
            lblDescRede = new Label();
            btnDashboard = new Button();
            lblDescDashboard = new Label();
            btnMqttCliente = new Button();
            lblDescMqttCliente = new Label();
            btnMqttBroker = new Button();
            lblDescMqttBroker = new Label();
            btnWebhookServer = new Button();
            lblDescWebhookServer = new Label();
            panelLog = new Panel();
            txtLog = new TextBox();
            btnLimparLog = new Button();
            lblLog = new Label();
            panelTop.SuspendLayout();
            panelBotoes.SuspendLayout();
            panelLog.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(240, 240, 240);
            panelTop.Controls.Add(lblStatus);
            panelTop.Controls.Add(btnConectar);
            panelTop.Controls.Add(lblSenha);
            panelTop.Controls.Add(txtSenha);
            panelTop.Controls.Add(lblUsuario);
            panelTop.Controls.Add(txtUsuario);
            panelTop.Controls.Add(lblIP);
            panelTop.Controls.Add(txtIP);
            panelTop.Controls.Add(btnAbrirInterfaceWeb);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(15, 10, 15, 10);
            panelTop.Size = new Size(784, 70);
            panelTop.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(449, 52);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(88, 15);
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Não conectado";
            lblStatus.Click += lblStatus_Click;
            // 
            // btnConectar
            // 
            btnConectar.BackColor = Color.FromArgb(0, 122, 204);
            btnConectar.FlatStyle = FlatStyle.Flat;
            btnConectar.ForeColor = Color.White;
            btnConectar.Location = new Point(449, 12);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(110, 28);
            btnConectar.TabIndex = 6;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = false;
            btnConectar.Click += btnConectar_Click;
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Location = new Point(335, 15);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(42, 15);
            lblSenha.TabIndex = 4;
            lblSenha.Text = "Senha:";
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(335, 35);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(100, 23);
            txtSenha.TabIndex = 5;
            txtSenha.Text = "admin";
            txtSenha.UseSystemPasswordChar = true;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(225, 15);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 15);
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuário:";
            // 
            // txtUsuario
            // 
            txtUsuario.Enabled = false;
            txtUsuario.Location = new Point(225, 35);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(100, 23);
            txtUsuario.TabIndex = 3;
            txtUsuario.Text = "master";
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Location = new Point(15, 15);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(104, 15);
            lblIP.TabIndex = 0;
            lblIP.Text = "IP do Controlador:";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(15, 35);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(200, 23);
            txtIP.TabIndex = 1;
            txtIP.Text = "192.168.120.45";
            // 
            // btnAbrirInterfaceWeb
            // 
            btnAbrirInterfaceWeb.BackColor = Color.FromArgb(32, 201, 151);
            btnAbrirInterfaceWeb.FlatStyle = FlatStyle.Flat;
            btnAbrirInterfaceWeb.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAbrirInterfaceWeb.ForeColor = Color.White;
            btnAbrirInterfaceWeb.Location = new Point(575, 12);
            btnAbrirInterfaceWeb.Name = "btnAbrirInterfaceWeb";
            btnAbrirInterfaceWeb.Size = new Size(130, 28);
            btnAbrirInterfaceWeb.TabIndex = 8;
            btnAbrirInterfaceWeb.Text = "🌐 Interface Web";
            btnAbrirInterfaceWeb.UseVisualStyleBackColor = false;
            btnAbrirInterfaceWeb.Click += btnAbrirInterfaceWeb_Click;
            // 
            // panelBotoes
            // 
            panelBotoes.AutoScroll = true;
            panelBotoes.Controls.Add(lblTituloDemos);
            panelBotoes.Controls.Add(btnCadastroCompleto);
            panelBotoes.Controls.Add(lblDescCompleto);
            panelBotoes.Controls.Add(btnCadastroSimples);
            panelBotoes.Controls.Add(lblDescSimples);
            panelBotoes.Controls.Add(btnMonitoramento);
            panelBotoes.Controls.Add(lblDescMonitoramento);
            panelBotoes.Controls.Add(btnRede);
            panelBotoes.Controls.Add(lblDescRede);
            panelBotoes.Controls.Add(btnDashboard);
            panelBotoes.Controls.Add(lblDescDashboard);
            panelBotoes.Controls.Add(btnMqttCliente);
            panelBotoes.Controls.Add(lblDescMqttCliente);
            panelBotoes.Controls.Add(btnMqttBroker);
            panelBotoes.Controls.Add(lblDescMqttBroker);
            panelBotoes.Controls.Add(btnWebhookServer);
            panelBotoes.Controls.Add(lblDescWebhookServer);
            panelBotoes.Dock = DockStyle.Fill;
            panelBotoes.Location = new Point(0, 70);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Padding = new Padding(15);
            panelBotoes.Size = new Size(784, 520);
            panelBotoes.TabIndex = 1;
            // 
            // lblTituloDemos
            // 
            lblTituloDemos.AutoSize = true;
            lblTituloDemos.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloDemos.Location = new Point(15, 15);
            lblTituloDemos.Name = "lblTituloDemos";
            lblTituloDemos.Size = new Size(273, 21);
            lblTituloDemos.TabIndex = 0;
            lblTituloDemos.Text = "Exemplos de Integração com a API";
            // 
            // btnCadastroCompleto
            // 
            btnCadastroCompleto.BackColor = Color.FromArgb(0, 123, 255);
            btnCadastroCompleto.Enabled = false;
            btnCadastroCompleto.FlatStyle = FlatStyle.Flat;
            btnCadastroCompleto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCadastroCompleto.ForeColor = Color.White;
            btnCadastroCompleto.Location = new Point(15, 55);
            btnCadastroCompleto.Name = "btnCadastroCompleto";
            btnCadastroCompleto.Size = new Size(250, 40);
            btnCadastroCompleto.TabIndex = 1;
            btnCadastroCompleto.Text = "Cadastro Completo (MobiCortex)";
            btnCadastroCompleto.UseVisualStyleBackColor = false;
            btnCadastroCompleto.Click += btnCadastroCompleto_Click;
            // 
            // lblDescCompleto
            // 
            lblDescCompleto.ForeColor = Color.Gray;
            lblDescCompleto.Location = new Point(275, 55);
            lblDescCompleto.Name = "lblDescCompleto";
            lblDescCompleto.Size = new Size(490, 40);
            lblDescCompleto.TabIndex = 2;
            lblDescCompleto.Text = "Modelo hierárquico de 3 níveis: Cadastro Central → Entidade (pessoa/veículo) → Mídia de Acesso. Ideal para condomínios e empresas.";
            // 
            // btnCadastroSimples
            // 
            btnCadastroSimples.BackColor = Color.FromArgb(40, 167, 69);
            btnCadastroSimples.Enabled = false;
            btnCadastroSimples.FlatStyle = FlatStyle.Flat;
            btnCadastroSimples.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCadastroSimples.ForeColor = Color.White;
            btnCadastroSimples.Location = new Point(15, 110);
            btnCadastroSimples.Name = "btnCadastroSimples";
            btnCadastroSimples.Size = new Size(250, 40);
            btnCadastroSimples.TabIndex = 3;
            btnCadastroSimples.Text = "Cadastro Simplificado";
            btnCadastroSimples.UseVisualStyleBackColor = false;
            btnCadastroSimples.Click += btnCadastroSimples_Click;
            // 
            // lblDescSimples
            // 
            lblDescSimples.ForeColor = Color.Gray;
            lblDescSimples.Location = new Point(275, 110);
            lblDescSimples.Name = "lblDescSimples";
            lblDescSimples.Size = new Size(490, 40);
            lblDescSimples.TabIndex = 4;
            lblDescSimples.Text = "Modelo simplificado de 2 níveis: Pessoa/Veículo → Mídia. IDs gerados automaticamente pelo controlador (createid=true).";
            // 
            // btnMonitoramento
            // 
            btnMonitoramento.BackColor = Color.FromArgb(255, 193, 7);
            btnMonitoramento.Enabled = false;
            btnMonitoramento.FlatStyle = FlatStyle.Flat;
            btnMonitoramento.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMonitoramento.ForeColor = Color.Black;
            btnMonitoramento.Location = new Point(15, 165);
            btnMonitoramento.Name = "btnMonitoramento";
            btnMonitoramento.Size = new Size(250, 40);
            btnMonitoramento.TabIndex = 5;
            btnMonitoramento.Text = "Monitoramento (MQTT)";
            btnMonitoramento.UseVisualStyleBackColor = false;
            btnMonitoramento.Click += btnMonitoramento_Click;
            // 
            // lblDescMonitoramento
            // 
            lblDescMonitoramento.ForeColor = Color.Gray;
            lblDescMonitoramento.Location = new Point(275, 165);
            lblDescMonitoramento.Name = "lblDescMonitoramento";
            lblDescMonitoramento.Size = new Size(490, 40);
            lblDescMonitoramento.TabIndex = 6;
            lblDescMonitoramento.Text = "Recebe eventos em tempo real via MQTT over WebSocket. Monitora acessos, sensores e status do controlador.";
            // 
            // btnRede
            // 
            btnRede.BackColor = Color.FromArgb(108, 117, 125);
            btnRede.Enabled = false;
            btnRede.FlatStyle = FlatStyle.Flat;
            btnRede.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRede.ForeColor = Color.White;
            btnRede.Location = new Point(15, 220);
            btnRede.Name = "btnRede";
            btnRede.Size = new Size(250, 40);
            btnRede.TabIndex = 7;
            btnRede.Text = "Configuração de Rede";
            btnRede.UseVisualStyleBackColor = false;
            btnRede.Click += btnRede_Click;
            // 
            // lblDescRede
            // 
            lblDescRede.ForeColor = Color.Gray;
            lblDescRede.Location = new Point(275, 220);
            lblDescRede.Name = "lblDescRede";
            lblDescRede.Size = new Size(490, 40);
            lblDescRede.TabIndex = 8;
            lblDescRede.Text = "Leitura e configuração de rede do controlador (IP fixo, DHCP, máscara, gateway).";
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(23, 162, 184);
            btnDashboard.Enabled = false;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(15, 275);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(250, 40);
            btnDashboard.TabIndex = 9;
            btnDashboard.Text = "Dashboard / Info Dispositivo";
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // lblDescDashboard
            // 
            lblDescDashboard.ForeColor = Color.Gray;
            lblDescDashboard.Location = new Point(275, 275);
            lblDescDashboard.Name = "lblDescDashboard";
            lblDescDashboard.Size = new Size(490, 40);
            lblDescDashboard.TabIndex = 10;
            lblDescDashboard.Text = "Exibe informações do controlador: modelo, firmware, CPU, memória, estatísticas de cadastros e mídias.";
            // 
            // btnMqttCliente
            // 
            btnMqttCliente.BackColor = Color.FromArgb(111, 66, 193);
            btnMqttCliente.Enabled = false;
            btnMqttCliente.FlatStyle = FlatStyle.Flat;
            btnMqttCliente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMqttCliente.ForeColor = Color.White;
            btnMqttCliente.Location = new Point(15, 330);
            btnMqttCliente.Name = "btnMqttCliente";
            btnMqttCliente.Size = new Size(250, 40);
            btnMqttCliente.TabIndex = 11;
            btnMqttCliente.Text = "MQTT Cliente → Controladora";
            btnMqttCliente.UseVisualStyleBackColor = false;
            btnMqttCliente.Click += btnMqttCliente_Click;
            // 
            // lblDescMqttCliente
            // 
            lblDescMqttCliente.ForeColor = Color.Gray;
            lblDescMqttCliente.Location = new Point(275, 330);
            lblDescMqttCliente.Name = "lblDescMqttCliente";
            lblDescMqttCliente.Size = new Size(490, 40);
            lblDescMqttCliente.TabIndex = 12;
            lblDescMqttCliente.Text = "Conecta como cliente MQTT ao broker da controladora. Recebe eventos em tempo real via WebSocket.";
            // 
            // btnMqttBroker
            // 
            btnMqttBroker.BackColor = Color.FromArgb(220, 53, 69);
            btnMqttBroker.FlatStyle = FlatStyle.Flat;
            btnMqttBroker.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMqttBroker.ForeColor = Color.White;
            btnMqttBroker.Location = new Point(15, 385);
            btnMqttBroker.Name = "btnMqttBroker";
            btnMqttBroker.Size = new Size(250, 40);
            btnMqttBroker.TabIndex = 13;
            btnMqttBroker.Text = "MQTT Broker (Embutido)";
            btnMqttBroker.UseVisualStyleBackColor = false;
            btnMqttBroker.Click += btnMqttBroker_Click;
            // 
            // lblDescMqttBroker
            // 
            lblDescMqttBroker.ForeColor = Color.Gray;
            lblDescMqttBroker.Location = new Point(275, 385);
            lblDescMqttBroker.Name = "lblDescMqttBroker";
            lblDescMqttBroker.Size = new Size(490, 40);
            lblDescMqttBroker.TabIndex = 14;
            lblDescMqttBroker.Text = "Inicia um broker MQTT embutido. A controladora pode se conectar diretamente a este servidor.";
            // 
            // btnWebhookServer
            // 
            btnWebhookServer.BackColor = Color.FromArgb(253, 126, 20);
            btnWebhookServer.FlatStyle = FlatStyle.Flat;
            btnWebhookServer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnWebhookServer.ForeColor = Color.White;
            btnWebhookServer.Location = new Point(15, 440);
            btnWebhookServer.Name = "btnWebhookServer";
            btnWebhookServer.Size = new Size(250, 40);
            btnWebhookServer.TabIndex = 15;
            btnWebhookServer.Text = "Webhook Server (HTTP)";
            btnWebhookServer.UseVisualStyleBackColor = false;
            btnWebhookServer.Click += btnWebhookServer_Click;
            // 
            // lblDescWebhookServer
            // 
            lblDescWebhookServer.ForeColor = Color.Gray;
            lblDescWebhookServer.Location = new Point(275, 440);
            lblDescWebhookServer.Name = "lblDescWebhookServer";
            lblDescWebhookServer.Size = new Size(490, 40);
            lblDescWebhookServer.TabIndex = 16;
            lblDescWebhookServer.Text = "Inicia um servidor HTTP para receber eventos via webhook. A controladora envia POST com eventos JSON.";
            // 
            // panelLog
            // 
            panelLog.Controls.Add(txtLog);
            panelLog.Controls.Add(btnLimparLog);
            panelLog.Controls.Add(lblLog);
            panelLog.Dock = DockStyle.Bottom;
            panelLog.Location = new Point(0, 590);
            panelLog.Name = "panelLog";
            panelLog.Padding = new Padding(5);
            panelLog.Size = new Size(784, 110);
            panelLog.TabIndex = 2;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 8.25F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(5, 23);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(774, 54);
            txtLog.TabIndex = 1;
            // 
            // btnLimparLog
            // 
            btnLimparLog.Dock = DockStyle.Bottom;
            btnLimparLog.Location = new Point(5, 77);
            btnLimparLog.Name = "btnLimparLog";
            btnLimparLog.Size = new Size(774, 28);
            btnLimparLog.TabIndex = 2;
            btnLimparLog.Text = "Limpar Log";
            btnLimparLog.Click += btnLimparLog_Click;
            // 
            // lblLog
            // 
            lblLog.Dock = DockStyle.Top;
            lblLog.Location = new Point(5, 5);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(774, 18);
            lblLog.TabIndex = 0;
            lblLog.Text = "Log de Operações:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 700);
            Controls.Add(panelBotoes);
            Controls.Add(panelLog);
            Controls.Add(panelTop);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SmartSDK - Exemplos de Integração MobiCortex";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelBotoes.ResumeLayout(false);
            panelBotoes.PerformLayout();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            ResumeLayout(false);
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
        private Button btnRede;
        private Label lblDescRede;
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
