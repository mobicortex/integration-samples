namespace SmartSdk
{
    partial class FormMonitoramento
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
            panelTopo = new Panel();
            lblStatus = new Label();
            lblContador = new Label();
            btnConectar = new Button();
            btnLimpar = new Button();
            lblTopico = new Label();
            txtTopico = new TextBox();
            btnSubscrever = new Button();
            lblHost = new Label();
            txtHost = new TextBox();
            lblPort = new Label();
            txtPort = new TextBox();
            lblUser = new Label();
            txtUser = new TextBox();
            lblPass = new Label();
            txtPass = new TextBox();
            lblExplicacao = new Label();
            txtLog = new TextBox();
            panelTopo.SuspendLayout();
            SuspendLayout();
            // 
            // lblExplicacao - Banner explicativo
            // 
            lblExplicacao.BackColor = Color.FromArgb(255, 193, 7);
            lblExplicacao.Dock = DockStyle.Top;
            lblExplicacao.Font = new Font("Segoe UI", 9F);
            lblExplicacao.ForeColor = Color.Black;
            lblExplicacao.Location = new Point(0, 0);
            lblExplicacao.Name = "lblExplicacao";
            lblExplicacao.Padding = new Padding(8, 4, 8, 4);
            lblExplicacao.Size = new Size(800, 55);
            lblExplicacao.TabIndex = 0;
            lblExplicacao.Text = "MQTT TCP on the controller export listener (port 1884).\r\n" +
                "User/password = credential from Settings > MQTT (same username and password).\r\n" +
                "Topic: mbcortex/export/event   |   1883 on the controller is loopback IPC only.";
            // 
            // panelTopo - Controles de conexão
            // 
            panelTopo.Controls.Add(txtPass);
            panelTopo.Controls.Add(lblPass);
            panelTopo.Controls.Add(txtUser);
            panelTopo.Controls.Add(lblUser);
            panelTopo.Controls.Add(txtPort);
            panelTopo.Controls.Add(lblPort);
            panelTopo.Controls.Add(txtHost);
            panelTopo.Controls.Add(lblHost);
            panelTopo.Controls.Add(btnSubscrever);
            panelTopo.Controls.Add(txtTopico);
            panelTopo.Controls.Add(lblTopico);
            panelTopo.Controls.Add(btnLimpar);
            panelTopo.Controls.Add(btnConectar);
            panelTopo.Controls.Add(lblContador);
            panelTopo.Controls.Add(lblStatus);
            panelTopo.Dock = DockStyle.Top;
            panelTopo.Location = new Point(0, 55);
            panelTopo.Name = "panelTopo";
            panelTopo.Padding = new Padding(8);
            panelTopo.Size = new Size(800, 80);
            panelTopo.TabIndex = 1;
            // 
            // btnConectar
            // 
            btnConectar.BackColor = Color.FromArgb(0, 123, 255);
            btnConectar.FlatStyle = FlatStyle.Flat;
            btnConectar.ForeColor = Color.White;
            btnConectar.Location = new Point(8, 8);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(120, 28);
            btnConectar.TabIndex = 0;
            btnConectar.Text = "Connect MQTT";
            btnConectar.UseVisualStyleBackColor = false;
            btnConectar.Click += btnConectar_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(135, 14);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(85, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Disconnected";
            // 
            // lblContador
            // 
            lblContador.AutoSize = true;
            lblContador.ForeColor = Color.Gray;
            lblContador.Location = new Point(230, 14);
            lblContador.Name = "lblContador";
            lblContador.Size = new Size(82, 15);
            lblContador.TabIndex = 2;
            lblContador.Text = "Messages: 0";
            // 
            // lblTopico
            // 
            lblTopico.AutoSize = true;
            lblTopico.Location = new Point(340, 14);
            lblTopico.Name = "lblTopico";
            lblTopico.Size = new Size(46, 15);
            lblTopico.TabIndex = 3;
            lblTopico.Text = "Topic:";
            // 
            // txtTopico
            // 
            txtTopico.Location = new Point(390, 10);
            txtTopico.Name = "txtTopico";
            txtTopico.Size = new Size(220, 23);
            txtTopico.TabIndex = 4;
            txtTopico.Text = "mbcortex/export/event";
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(8, 50);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(35, 15);
            lblHost.TabIndex = 7;
            lblHost.Text = "Host:";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(48, 47);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(140, 23);
            txtHost.TabIndex = 8;
            txtHost.Text = "192.168.0.180";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(194, 50);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 15);
            lblPort.TabIndex = 9;
            lblPort.Text = "Port:";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(228, 47);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(50, 23);
            txtPort.TabIndex = 10;
            txtPort.Text = "1884";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(286, 50);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(33, 15);
            lblUser.TabIndex = 11;
            lblUser.Text = "User:";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(322, 47);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(120, 23);
            txtUser.TabIndex = 12;
            txtUser.Text = "mqttuser";
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Location = new Point(450, 50);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(60, 15);
            lblPass.TabIndex = 13;
            lblPass.Text = "Password:";
            // 
            // txtPass
            // 
            txtPass.Location = new Point(512, 47);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(140, 23);
            txtPass.TabIndex = 14;
            txtPass.Text = "mqttpass";
            txtPass.UseSystemPasswordChar = true;
            // 
            // btnSubscrever
            // 
            btnSubscrever.Location = new Point(615, 8);
            btnSubscrever.Name = "btnSubscrever";
            btnSubscrever.Size = new Size(80, 28);
            btnSubscrever.TabIndex = 5;
            btnSubscrever.Text = "Subscribe";
            btnSubscrever.Click += btnSubscrever_Click;
            // 
            // btnLimpar
            // 
            btnLimpar.Location = new Point(700, 8);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(60, 28);
            btnLimpar.TabIndex = 6;
            btnLimpar.Text = "Clear";
            btnLimpar.Click += btnLimpar_Click;
            // 
            // txtLog - Log de mensagens recebidas
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(0, 135);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(800, 350);
            txtLog.TabIndex = 2;
            txtLog.WordWrap = true;
            // 
            // FormMonitoramento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtLog);
            Controls.Add(panelTopo);
            Controls.Add(lblExplicacao);
            MinimumSize = new Size(820, 350);
            Name = "FormMonitoramento";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Monitoring — MQTT TCP 1884 (mbcortex/export/event)";
            panelTopo.ResumeLayout(false);
            panelTopo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplicacao;
        private Panel panelTopo;
        private Button btnConectar;
        private Label lblStatus;
        private Label lblContador;
        private Label lblTopico;
        private TextBox txtTopico;
        private Button btnSubscrever;
        private Button btnLimpar;
        private TextBox txtLog;
        private Label lblHost;
        private TextBox txtHost;
        private Label lblPort;
        private TextBox txtPort;
        private Label lblUser;
        private TextBox txtUser;
        private Label lblPass;
        private TextBox txtPass;
    }
}
