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
            this.lblExplicacao = new Label();
            this.txtLog = new TextBox();
            this.panelTopo.SuspendLayout();
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
            this.lblExplicacao.Text = "MQTT over WebSocket: Receive real-time events from the controller.\r\n" +
                "URL: wss://<host>/mbcortex/master/api/v1/mqtt  |  MQTT Password = session_key from HTTP login\r\n" +
                "Topics: mbcortex/master/events/#  |  mbcortex/master/logs/#  |  #  (all)";
            //
            // panelTopo - Controles de conexão
            //
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
            this.panelTopo.Size = new Size(800, 45);
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
            this.txtTopico.Text = "#";
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
            this.txtLog.Dock = DockStyle.Fill;
            this.txtLog.Font = new Font("Consolas", 9F);
            this.txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            this.txtLog.Location = new Point(0, 100);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Both;
            this.txtLog.Size = new Size(800, 350);
            this.txtLog.TabIndex = 2;
            this.txtLog.WordWrap = false;
            //
            // FormMonitoramento
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panelTopo);
            this.Controls.Add(this.lblExplicacao);
            this.MinimumSize = new Size(600, 350);
            this.Name = "FormMonitoramento";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Monitoring - MQTT over WebSocket (Real-Time Events)";
            this.panelTopo.ResumeLayout(false);
            this.panelTopo.PerformLayout();
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
    }
}
