namespace SmartSdk
{
    partial class FormRede
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
            panelConfig = new Panel();
            groupEthernet = new GroupBox();
            chkDhcp = new CheckBox();
            lblIp = new Label();
            txtIp = new TextBox();
            lblMascara = new Label();
            txtMascara = new TextBox();
            lblGateway = new Label();
            txtGateway = new TextBox();
            lblDns1 = new Label();
            txtDns1 = new TextBox();
            lblDns2 = new Label();
            txtDns2 = new TextBox();
            panelBotoes = new FlowLayoutPanel();
            btnSalvar = new Button();
            btnRecarregar = new Button();
            lblExplicacao = new Label();
            txtLog = new TextBox();
            panelConfig.SuspendLayout();
            groupEthernet.SuspendLayout();
            panelBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // lblExplicacao
            // 
            lblExplicacao.BackColor = Color.FromArgb(108, 117, 125);
            lblExplicacao.Dock = DockStyle.Top;
            lblExplicacao.Font = new Font("Segoe UI", 9F);
            lblExplicacao.ForeColor = Color.White;
            lblExplicacao.Location = new Point(0, 0);
            lblExplicacao.Name = "lblExplicacao";
            lblExplicacao.Padding = new Padding(8, 4, 8, 4);
            lblExplicacao.Size = new Size(500, 40);
            lblExplicacao.TabIndex = 0;
            lblExplicacao.Text = "Configuração de Rede Ethernet (cabo)\r\n" +
                "GET/POST /network-config-cable  |  ATENÇÃO: Alterar IP pode desconectar a sessão";
            // 
            // panelConfig
            // 
            panelConfig.Controls.Add(groupEthernet);
            panelConfig.Controls.Add(panelBotoes);
            panelConfig.Dock = DockStyle.Top;
            panelConfig.Location = new Point(0, 40);
            panelConfig.Name = "panelConfig";
            panelConfig.Padding = new Padding(10);
            panelConfig.Size = new Size(500, 250);
            panelConfig.TabIndex = 1;
            // 
            // groupEthernet
            // 
            groupEthernet.Controls.Add(txtDns2);
            groupEthernet.Controls.Add(lblDns2);
            groupEthernet.Controls.Add(txtDns1);
            groupEthernet.Controls.Add(lblDns1);
            groupEthernet.Controls.Add(txtGateway);
            groupEthernet.Controls.Add(lblGateway);
            groupEthernet.Controls.Add(txtMascara);
            groupEthernet.Controls.Add(lblMascara);
            groupEthernet.Controls.Add(txtIp);
            groupEthernet.Controls.Add(lblIp);
            groupEthernet.Controls.Add(chkDhcp);
            groupEthernet.Dock = DockStyle.Fill;
            groupEthernet.Location = new Point(10, 10);
            groupEthernet.Name = "groupEthernet";
            groupEthernet.Padding = new Padding(10);
            groupEthernet.Size = new Size(480, 195);
            groupEthernet.TabIndex = 0;
            groupEthernet.Text = "Ethernet (Cabo)";
            // 
            // chkDhcp
            // 
            chkDhcp.AutoSize = true;
            chkDhcp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkDhcp.Location = new Point(15, 25);
            chkDhcp.Name = "chkDhcp";
            chkDhcp.Size = new Size(192, 19);
            chkDhcp.TabIndex = 0;
            chkDhcp.Text = "DHCP (obter IP automaticamente)";
            chkDhcp.CheckedChanged += chkDhcp_CheckedChanged;
            // 
            // lblIp
            // 
            lblIp.AutoSize = true;
            lblIp.Location = new Point(15, 55);
            lblIp.Name = "lblIp";
            lblIp.Size = new Size(20, 15);
            lblIp.TabIndex = 1;
            lblIp.Text = "IP:";
            // 
            // txtIp
            // 
            txtIp.Location = new Point(90, 52);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(150, 23);
            txtIp.TabIndex = 2;
            // 
            // lblMascara
            // 
            lblMascara.AutoSize = true;
            lblMascara.Location = new Point(15, 85);
            lblMascara.Name = "lblMascara";
            lblMascara.Size = new Size(55, 15);
            lblMascara.TabIndex = 3;
            lblMascara.Text = "Máscara:";
            // 
            // txtMascara
            // 
            txtMascara.Location = new Point(90, 82);
            txtMascara.Name = "txtMascara";
            txtMascara.Size = new Size(150, 23);
            txtMascara.TabIndex = 4;
            // 
            // lblGateway
            // 
            lblGateway.AutoSize = true;
            lblGateway.Location = new Point(15, 115);
            lblGateway.Name = "lblGateway";
            lblGateway.Size = new Size(55, 15);
            lblGateway.TabIndex = 5;
            lblGateway.Text = "Gateway:";
            // 
            // txtGateway
            // 
            txtGateway.Location = new Point(90, 112);
            txtGateway.Name = "txtGateway";
            txtGateway.Size = new Size(150, 23);
            txtGateway.TabIndex = 6;
            // 
            // lblDns1
            // 
            lblDns1.AutoSize = true;
            lblDns1.Location = new Point(265, 55);
            lblDns1.Name = "lblDns1";
            lblDns1.Size = new Size(41, 15);
            lblDns1.TabIndex = 7;
            lblDns1.Text = "DNS 1:";
            // 
            // txtDns1
            // 
            txtDns1.Location = new Point(315, 52);
            txtDns1.Name = "txtDns1";
            txtDns1.Size = new Size(150, 23);
            txtDns1.TabIndex = 8;
            // 
            // lblDns2
            // 
            lblDns2.AutoSize = true;
            lblDns2.Location = new Point(265, 85);
            lblDns2.Name = "lblDns2";
            lblDns2.Size = new Size(41, 15);
            lblDns2.TabIndex = 9;
            lblDns2.Text = "DNS 2:";
            // 
            // txtDns2
            // 
            txtDns2.Location = new Point(315, 82);
            txtDns2.Name = "txtDns2";
            txtDns2.Size = new Size(150, 23);
            txtDns2.TabIndex = 10;
            // 
            // panelBotoes
            // 
            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Controls.Add(btnRecarregar);
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.FlowDirection = FlowDirection.RightToLeft;
            panelBotoes.Location = new Point(10, 205);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Size = new Size(480, 35);
            panelBotoes.TabIndex = 1;
            // 
            // btnSalvar
            // 
            btnSalvar.BackColor = Color.FromArgb(0, 123, 255);
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(395, 3);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(82, 28);
            btnSalvar.TabIndex = 0;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = false;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // btnRecarregar
            // 
            btnRecarregar.Location = new Point(307, 3);
            btnRecarregar.Name = "btnRecarregar";
            btnRecarregar.Size = new Size(82, 28);
            btnRecarregar.TabIndex = 1;
            btnRecarregar.Text = "Recarregar";
            btnRecarregar.Click += btnRecarregar_Click;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 8.25F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(0, 290);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(500, 110);
            txtLog.TabIndex = 2;
            // 
            // FormRede
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 400);
            Controls.Add(txtLog);
            Controls.Add(panelConfig);
            Controls.Add(lblExplicacao);
            MinimumSize = new Size(450, 380);
            Name = "FormRede";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Configuração de Rede - GET/POST /network-config-cable";
            Load += FormRede_Load;
            panelConfig.ResumeLayout(false);
            groupEthernet.ResumeLayout(false);
            groupEthernet.PerformLayout();
            panelBotoes.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplicacao;
        private Panel panelConfig;
        private GroupBox groupEthernet;
        private CheckBox chkDhcp;
        private Label lblIp;
        private TextBox txtIp;
        private Label lblMascara;
        private TextBox txtMascara;
        private Label lblGateway;
        private TextBox txtGateway;
        private Label lblDns1;
        private TextBox txtDns1;
        private Label lblDns2;
        private TextBox txtDns2;
        private FlowLayoutPanel panelBotoes;
        private Button btnSalvar;
        private Button btnRecarregar;
        private TextBox txtLog;
    }
}
