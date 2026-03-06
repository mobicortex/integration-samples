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
            lblExplicacao.Text = "MQTT sobre WebSocket: Receba eventos em tempo real do controlador.\r\n" +
                "URL: wss://<host>/mbcortex/master/api/v1/mqtt  |  Senha MQTT = session_key do login HTTP\r\n" +
                "Tópicos: mbcortex/master/events/#  |  mbcortex/master/logs/#  |  #  (todos)";
            // 
            // panelTopo - Controles de conexão
            // 
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
            panelTopo.Size = new Size(800, 45);
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
            btnConectar.Text = "Conectar MQTT";
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
            lblStatus.Text = "Desconectado";
            // 
            // lblContador
            // 
            lblContador.AutoSize = true;
            lblContador.ForeColor = Color.Gray;
            lblContador.Location = new Point(230, 14);
            lblContador.Name = "lblContador";
            lblContador.Size = new Size(82, 15);
            lblContador.TabIndex = 2;
            lblContador.Text = "Mensagens: 0";
            // 
            // lblTopico
            // 
            lblTopico.AutoSize = true;
            lblTopico.Location = new Point(340, 14);
            lblTopico.Name = "lblTopico";
            lblTopico.Size = new Size(46, 15);
            lblTopico.TabIndex = 3;
            lblTopico.Text = "Tópico:";
            // 
            // txtTopico
            // 
            txtTopico.Location = new Point(390, 10);
            txtTopico.Name = "txtTopico";
            txtTopico.Size = new Size(220, 23);
            txtTopico.TabIndex = 4;
            txtTopico.Text = "#";
            // 
            // btnSubscrever
            // 
            btnSubscrever.Location = new Point(615, 8);
            btnSubscrever.Name = "btnSubscrever";
            btnSubscrever.Size = new Size(80, 28);
            btnSubscrever.TabIndex = 5;
            btnSubscrever.Text = "Subscrever";
            btnSubscrever.Click += btnSubscrever_Click;
            // 
            // btnLimpar
            // 
            btnLimpar.Location = new Point(700, 8);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(60, 28);
            btnLimpar.TabIndex = 6;
            btnLimpar.Text = "Limpar";
            btnLimpar.Click += btnLimpar_Click;
            // 
            // txtLog - Log de mensagens recebidas
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(0, 100);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(800, 350);
            txtLog.TabIndex = 2;
            txtLog.WordWrap = false;
            // 
            // FormMonitoramento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtLog);
            Controls.Add(panelTopo);
            Controls.Add(lblExplicacao);
            MinimumSize = new Size(600, 350);
            Name = "FormMonitoramento";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Monitoramento - MQTT sobre WebSocket (Eventos em Tempo Real)";
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
    }
}
