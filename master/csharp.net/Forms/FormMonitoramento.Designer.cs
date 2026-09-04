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
            this.panelTopo = new Panel();
            this.lblStatus = new Label();
            this.lblContador = new Label();
            this.btnConectar = new Button();
            this.btnLimpar = new Button();
            this.lblTopico = new Label();
            this.txtTopico = new TextBox();
            this.btnSubscrever = new Button();
            this.lblHost = new Label();
            this.txtHost = new TextBox();
            this.lblPort = new Label();
            this.txtPort = new TextBox();
            this.lblUser = new Label();
            this.txtUser = new TextBox();
            this.lblPass = new Label();
            this.txtPass = new TextBox();
            this.lblExplicacao = new Label();
            this.txtLog = new TextBox();
            this.gridEventos = new DataGridView();
            this.ColHora = new DataGridViewTextBoxColumn();
            this.ColEvento = new DataGridViewTextBoxColumn();
            this.ColPlaca = new DataGridViewTextBoxColumn();
            this.ColRegistered = new DataGridViewTextBoxColumn();
            this.ColTopic = new DataGridViewTextBoxColumn();
            this.ColPayload = new DataGridViewTextBoxColumn();
            this.panelTopo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.gridEventos).BeginInit();
            this.SuspendLayout();
            //
            // lblExplicacao - Banner explicativo
            //
            this.lblExplicacao.BackColor = Color.FromArgb(255, 193, 7);
            this.lblExplicacao.Dock = DockStyle.Top;
            this.lblExplicacao.Font = new Font("Segoe UI", 9F);
            this.lblExplicacao.ForeColor = Color.Black;
            this.lblExplicacao.Location = new Point(0, 0);
            this.lblExplicacao.Name = "lblExplicacao";
            this.lblExplicacao.Padding = new Padding(8, 4, 8, 4);
            this.lblExplicacao.Size = new Size(800, 55);
            this.lblExplicacao.TabIndex = 0;
            this.lblExplicacao.Text = "MQTT TCP on the controller export listener (port 1884).\r\n" +
                "User/password = credential from Settings > MQTT (same username and password).\r\n" +
                "Topic: mbcortex/export/event   |   1883 on the controller is loopback IPC only.";
            //
            // panelTopo - Controles de conexão
            //
            this.panelTopo.Controls.Add(this.txtPass);
            this.panelTopo.Controls.Add(this.lblPass);
            this.panelTopo.Controls.Add(this.txtUser);
            this.panelTopo.Controls.Add(this.lblUser);
            this.panelTopo.Controls.Add(this.txtPort);
            this.panelTopo.Controls.Add(this.lblPort);
            this.panelTopo.Controls.Add(this.txtHost);
            this.panelTopo.Controls.Add(this.lblHost);
            this.panelTopo.Controls.Add(this.btnSubscrever);
            this.panelTopo.Controls.Add(this.txtTopico);
            this.panelTopo.Controls.Add(this.lblTopico);
            this.panelTopo.Controls.Add(this.btnLimpar);
            this.panelTopo.Controls.Add(this.btnConectar);
            this.panelTopo.Controls.Add(this.lblContador);
            this.panelTopo.Controls.Add(this.lblStatus);
            this.panelTopo.Dock = DockStyle.Top;
            this.panelTopo.Location = new Point(0, 55);
            this.panelTopo.Name = "panelTopo";
            this.panelTopo.Padding = new Padding(8);
            this.panelTopo.Size = new Size(800, 80);
            this.panelTopo.TabIndex = 1;
            //
            // btnConectar
            //
            this.btnConectar.BackColor = Color.FromArgb(0, 123, 255);
            this.btnConectar.FlatStyle = FlatStyle.Flat;
            this.btnConectar.ForeColor = Color.White;
            this.btnConectar.Location = new Point(8, 8);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new Size(120, 28);
            this.btnConectar.TabIndex = 0;
            this.btnConectar.Text = "Connect MQTT";
            this.btnConectar.UseVisualStyleBackColor = false;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatus.ForeColor = Color.Red;
            this.lblStatus.Location = new Point(135, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(85, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Disconnected";
            //
            // lblContador
            //
            this.lblContador.AutoSize = true;
            this.lblContador.ForeColor = Color.Gray;
            this.lblContador.Location = new Point(230, 14);
            this.lblContador.Name = "lblContador";
            this.lblContador.Size = new Size(82, 15);
            this.lblContador.TabIndex = 2;
            this.lblContador.Text = "Messages: 0";
            //
            // lblTopico
            //
            this.lblTopico.AutoSize = true;
            this.lblTopico.Location = new Point(340, 14);
            this.lblTopico.Name = "lblTopico";
            this.lblTopico.Size = new Size(46, 15);
            this.lblTopico.TabIndex = 3;
            this.lblTopico.Text = "Topic:";
            //
            // txtTopico
            //
            this.txtTopico.Location = new Point(390, 10);
            this.txtTopico.Name = "txtTopico";
            this.txtTopico.Size = new Size(220, 23);
            this.txtTopico.TabIndex = 4;
            this.txtTopico.Text = "mbcortex/export/event";
            //
            // lblHost
            //
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new Point(8, 50);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new Size(35, 15);
            this.lblHost.TabIndex = 7;
            this.lblHost.Text = "Host:";
            //
            // txtHost
            //
            this.txtHost.Location = new Point(48, 47);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new Size(140, 23);
            this.txtHost.TabIndex = 8;
            this.txtHost.Text = "192.168.0.180";
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new Point(194, 50);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new Size(32, 15);
            this.lblPort.TabIndex = 9;
            this.lblPort.Text = "Port:";
            //
            // txtPort
            //
            this.txtPort.Location = new Point(228, 47);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new Size(50, 23);
            this.txtPort.TabIndex = 10;
            this.txtPort.Text = "1884";
            //
            // lblUser
            //
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new Point(286, 50);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new Size(33, 15);
            this.lblUser.TabIndex = 11;
            this.lblUser.Text = "User:";
            //
            // txtUser
            //
            this.txtUser.Location = new Point(322, 47);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new Size(120, 23);
            this.txtUser.TabIndex = 12;
            this.txtUser.Text = "mqttuser";
            //
            // lblPass
            //
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new Point(450, 50);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new Size(60, 15);
            this.lblPass.TabIndex = 13;
            this.lblPass.Text = "Password:";
            //
            // txtPass
            //
            this.txtPass.Location = new Point(512, 47);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new Size(140, 23);
            this.txtPass.TabIndex = 14;
            this.txtPass.Text = "mqttpass";
            this.txtPass.UseSystemPasswordChar = true;
            //
            // btnSubscrever
            //
            this.btnSubscrever.Location = new Point(615, 8);
            this.btnSubscrever.Name = "btnSubscrever";
            this.btnSubscrever.Size = new Size(80, 28);
            this.btnSubscrever.TabIndex = 5;
            this.btnSubscrever.Text = "Subscribe";
            this.btnSubscrever.Click += new System.EventHandler(this.btnSubscrever_Click);
            //
            // btnLimpar
            //
            this.btnLimpar.Location = new Point(700, 8);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new Size(60, 28);
            this.btnLimpar.TabIndex = 6;
            this.btnLimpar.Text = "Clear";
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            //
            // txtLog - Log de mensagens recebidas
            //
            this.txtLog.BackColor = Color.FromArgb(30, 30, 30);
            this.txtLog.Dock = DockStyle.Bottom;
            this.txtLog.Font = new Font("Consolas", 9F);
            this.txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            this.txtLog.Location = new Point(0, 360);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(800, 90);
            this.txtLog.TabIndex = 2;
            this.txtLog.WordWrap = true;
            //
            // gridEventos
            //
            this.gridEventos.AllowUserToAddRows = false;
            this.gridEventos.AllowUserToDeleteRows = false;
            this.gridEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridEventos.BackgroundColor = SystemColors.Window;
            this.gridEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEventos.Columns.AddRange(new DataGridViewColumn[] { this.ColHora, this.ColEvento, this.ColPlaca, this.ColRegistered, this.ColTopic, this.ColPayload });
            this.gridEventos.Dock = DockStyle.Fill;
            this.gridEventos.Location = new Point(0, 135);
            this.gridEventos.MultiSelect = false;
            this.gridEventos.Name = "gridEventos";
            this.gridEventos.ReadOnly = true;
            this.gridEventos.RowHeadersVisible = false;
            this.gridEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridEventos.TabIndex = 3;
            this.gridEventos.CellDoubleClick += new DataGridViewCellEventHandler(this.gridEventos_CellDoubleClick);
            //
            // ColHora
            //
            this.ColHora.FillWeight = 70F;
            this.ColHora.HeaderText = "Time";
            this.ColHora.Name = "ColHora";
            this.ColHora.ReadOnly = true;
            //
            // ColEvento
            //
            this.ColEvento.FillWeight = 70F;
            this.ColEvento.HeaderText = "Event";
            this.ColEvento.Name = "ColEvento";
            this.ColEvento.ReadOnly = true;
            //
            // ColPlaca
            //
            this.ColPlaca.FillWeight = 80F;
            this.ColPlaca.HeaderText = "Plate";
            this.ColPlaca.Name = "ColPlaca";
            this.ColPlaca.ReadOnly = true;
            //
            // ColRegistered
            //
            this.ColRegistered.FillWeight = 80F;
            this.ColRegistered.HeaderText = "Registered";
            this.ColRegistered.Name = "ColRegistered";
            this.ColRegistered.ReadOnly = true;
            //
            // ColTopic
            //
            this.ColTopic.FillWeight = 140F;
            this.ColTopic.HeaderText = "Topic";
            this.ColTopic.Name = "ColTopic";
            this.ColTopic.ReadOnly = true;
            //
            // ColPayload
            //
            this.ColPayload.FillWeight = 220F;
            this.ColPayload.HeaderText = "Payload";
            this.ColPayload.Name = "ColPayload";
            this.ColPayload.ReadOnly = true;
            //
            // FormMonitoramento
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.gridEventos);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panelTopo);
            this.Controls.Add(this.lblExplicacao);
            this.MinimumSize = new Size(820, 350);
            this.Name = "FormMonitoramento";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Monitoring — MQTT TCP 1884 (mbcortex/export/event)";
            this.panelTopo.ResumeLayout(false);
            this.panelTopo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.gridEventos).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
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
        private DataGridView gridEventos;
        private DataGridViewTextBoxColumn ColHora;
        private DataGridViewTextBoxColumn ColEvento;
        private DataGridViewTextBoxColumn ColPlaca;
        private DataGridViewTextBoxColumn ColRegistered;
        private DataGridViewTextBoxColumn ColTopic;
        private DataGridViewTextBoxColumn ColPayload;
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
