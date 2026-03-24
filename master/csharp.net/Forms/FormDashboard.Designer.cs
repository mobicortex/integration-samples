namespace SmartSdk
{
    partial class FormDashboard
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
            lblExplicacao = new Label();
            panelCards = new TableLayoutPanel();
            groupDevice = new GroupBox();
            tableDevice = new TableLayoutPanel();
            label1 = new Label();
            lblModelo = new Label();
            label3 = new Label();
            lblFirmware = new Label();
            label5 = new Label();
            lblGid = new Label();
            label7 = new Label();
            lblUptime = new Label();
            label9 = new Label();
            lblCpu = new Label();
            label11 = new Label();
            lblMemoria = new Label();
            groupStats = new GroupBox();
            tableStats = new TableLayoutPanel();
            label13 = new Label();
            lblCadastros = new Label();
            label15 = new Label();
            lblPessoas = new Label();
            label17 = new Label();
            lblVeiculos = new Label();
            label19 = new Label();
            lblMidias = new Label();
            label21 = new Label();
            lblFacial = new Label();
            label23 = new Label();
            lblRfid = new Label();
            label25 = new Label();
            lblLpr = new Label();
            label27 = new Label();
            lblControle = new Label();
            groupCapacidade = new GroupBox();
            lblCapacidade = new Label();
            progressCapacidade = new ProgressBar();
            panelBotoes = new Panel();
            btnAtualizar = new Button();
            txtLog = new TextBox();
            panelCards.SuspendLayout();
            groupDevice.SuspendLayout();
            tableDevice.SuspendLayout();
            groupStats.SuspendLayout();
            tableStats.SuspendLayout();
            groupCapacidade.SuspendLayout();
            panelBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // lblExplicacao
            // 
            lblExplicacao.BackColor = Color.FromArgb(0, 128, 128);
            lblExplicacao.Dock = DockStyle.Top;
            lblExplicacao.Font = new Font("Segoe UI", 9F);
            lblExplicacao.ForeColor = Color.White;
            lblExplicacao.Location = new Point(0, 0);
            lblExplicacao.Name = "lblExplicacao";
            lblExplicacao.Padding = new Padding(8, 4, 8, 4);
            lblExplicacao.Size = new Size(600, 35);
            lblExplicacao.TabIndex = 0;
            lblExplicacao.Text = "Dashboard: GET /device-info + GET /dashboard + GET /central-registry/stats";
            // 
            // panelCards
            // 
            panelCards.ColumnCount = 2;
            panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panelCards.Controls.Add(groupDevice, 0, 0);
            panelCards.Controls.Add(groupStats, 1, 0);
            panelCards.Controls.Add(groupCapacidade, 0, 1);
            panelCards.Controls.Add(panelBotoes, 1, 1);
            panelCards.Dock = DockStyle.Top;
            panelCards.Location = new Point(0, 35);
            panelCards.Name = "panelCards";
            panelCards.Padding = new Padding(8);
            panelCards.RowCount = 2;
            panelCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            panelCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            panelCards.Size = new Size(600, 300);
            panelCards.TabIndex = 1;
            // 
            // groupDevice - Informações do Hardware
            // 
            groupDevice.Controls.Add(tableDevice);
            groupDevice.Dock = DockStyle.Fill;
            groupDevice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupDevice.Location = new Point(11, 11);
            groupDevice.Name = "groupDevice";
            groupDevice.Size = new Size(286, 194);
            groupDevice.TabIndex = 0;
            groupDevice.Text = "Dispositivo (device-info)";
            // 
            // tableDevice
            // 
            tableDevice.ColumnCount = 2;
            tableDevice.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableDevice.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableDevice.Controls.Add(label1, 0, 0);
            tableDevice.Controls.Add(lblModelo, 1, 0);
            tableDevice.Controls.Add(label3, 0, 1);
            tableDevice.Controls.Add(lblFirmware, 1, 1);
            tableDevice.Controls.Add(label5, 0, 2);
            tableDevice.Controls.Add(lblGid, 1, 2);
            tableDevice.Controls.Add(label7, 0, 3);
            tableDevice.Controls.Add(lblUptime, 1, 3);
            tableDevice.Controls.Add(label9, 0, 4);
            tableDevice.Controls.Add(lblCpu, 1, 4);
            tableDevice.Controls.Add(label11, 0, 5);
            tableDevice.Controls.Add(lblMemoria, 1, 5);
            tableDevice.Dock = DockStyle.Fill;
            tableDevice.Location = new Point(3, 19);
            tableDevice.Name = "tableDevice";
            tableDevice.RowCount = 6;
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableDevice.Size = new Size(280, 172);
            tableDevice.TabIndex = 0;
            // 
            // Labels do device
            // 
            label1.Text = "Modelo:"; label1.Font = new Font("Segoe UI", 9F); label1.Dock = DockStyle.Fill; label1.TextAlign = ContentAlignment.MiddleLeft;
            lblModelo.Text = "-"; lblModelo.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblModelo.Dock = DockStyle.Fill; lblModelo.TextAlign = ContentAlignment.MiddleLeft;
            label3.Text = "Firmware:"; label3.Font = new Font("Segoe UI", 9F); label3.Dock = DockStyle.Fill; label3.TextAlign = ContentAlignment.MiddleLeft;
            lblFirmware.Text = "-"; lblFirmware.Font = new Font("Segoe UI", 9F); lblFirmware.Dock = DockStyle.Fill; lblFirmware.TextAlign = ContentAlignment.MiddleLeft;
            label5.Text = "GID:"; label5.Font = new Font("Segoe UI", 9F); label5.Dock = DockStyle.Fill; label5.TextAlign = ContentAlignment.MiddleLeft;
            lblGid.Text = "-"; lblGid.Font = new Font("Segoe UI", 9F); lblGid.Dock = DockStyle.Fill; lblGid.TextAlign = ContentAlignment.MiddleLeft;
            label7.Text = "Uptime:"; label7.Font = new Font("Segoe UI", 9F); label7.Dock = DockStyle.Fill; label7.TextAlign = ContentAlignment.MiddleLeft;
            lblUptime.Text = "-"; lblUptime.Font = new Font("Segoe UI", 9F); lblUptime.Dock = DockStyle.Fill; lblUptime.TextAlign = ContentAlignment.MiddleLeft;
            label9.Text = "CPU:"; label9.Font = new Font("Segoe UI", 9F); label9.Dock = DockStyle.Fill; label9.TextAlign = ContentAlignment.MiddleLeft;
            lblCpu.Text = "-"; lblCpu.Font = new Font("Segoe UI", 9F); lblCpu.Dock = DockStyle.Fill; lblCpu.TextAlign = ContentAlignment.MiddleLeft;
            label11.Text = "Memória:"; label11.Font = new Font("Segoe UI", 9F); label11.Dock = DockStyle.Fill; label11.TextAlign = ContentAlignment.MiddleLeft;
            lblMemoria.Text = "-"; lblMemoria.Font = new Font("Segoe UI", 9F); lblMemoria.Dock = DockStyle.Fill; lblMemoria.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupStats - Estatísticas
            // 
            groupStats.Controls.Add(tableStats);
            groupStats.Dock = DockStyle.Fill;
            groupStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupStats.Location = new Point(303, 11);
            groupStats.Name = "groupStats";
            groupStats.Size = new Size(286, 194);
            groupStats.TabIndex = 1;
            groupStats.Text = "Estatísticas (dashboard)";
            // 
            // tableStats
            // 
            tableStats.ColumnCount = 2;
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableStats.Controls.Add(label13, 0, 0);
            tableStats.Controls.Add(lblCadastros, 1, 0);
            tableStats.Controls.Add(label15, 0, 1);
            tableStats.Controls.Add(lblPessoas, 1, 1);
            tableStats.Controls.Add(label17, 0, 2);
            tableStats.Controls.Add(lblVeiculos, 1, 2);
            tableStats.Controls.Add(label19, 0, 3);
            tableStats.Controls.Add(lblMidias, 1, 3);
            tableStats.Controls.Add(label21, 0, 4);
            tableStats.Controls.Add(lblFacial, 1, 4);
            tableStats.Controls.Add(label23, 0, 5);
            tableStats.Controls.Add(lblRfid, 1, 5);
            tableStats.Controls.Add(label25, 0, 6);
            tableStats.Controls.Add(lblLpr, 1, 6);
            tableStats.Controls.Add(label27, 0, 7);
            tableStats.Controls.Add(lblControle, 1, 7);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(3, 19);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 8;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableStats.Size = new Size(280, 172);
            tableStats.TabIndex = 0;
            // 
            // Labels de stats
            // 
            label13.Text = "Cadastros:"; label13.Font = new Font("Segoe UI", 9F); label13.Dock = DockStyle.Fill; label13.TextAlign = ContentAlignment.MiddleLeft;
            lblCadastros.Text = "-"; lblCadastros.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblCadastros.Dock = DockStyle.Fill; lblCadastros.TextAlign = ContentAlignment.MiddleLeft;
            label15.Text = "Pessoas:"; label15.Font = new Font("Segoe UI", 9F); label15.Dock = DockStyle.Fill; label15.TextAlign = ContentAlignment.MiddleLeft;
            lblPessoas.Text = "-"; lblPessoas.Font = new Font("Segoe UI", 9F); lblPessoas.Dock = DockStyle.Fill; lblPessoas.TextAlign = ContentAlignment.MiddleLeft;
            label17.Text = "Veículos:"; label17.Font = new Font("Segoe UI", 9F); label17.Dock = DockStyle.Fill; label17.TextAlign = ContentAlignment.MiddleLeft;
            lblVeiculos.Text = "-"; lblVeiculos.Font = new Font("Segoe UI", 9F); lblVeiculos.Dock = DockStyle.Fill; lblVeiculos.TextAlign = ContentAlignment.MiddleLeft;
            label19.Text = "Total Mídias:"; label19.Font = new Font("Segoe UI", 9F); label19.Dock = DockStyle.Fill; label19.TextAlign = ContentAlignment.MiddleLeft;
            lblMidias.Text = "-"; lblMidias.Font = new Font("Segoe UI", 9F); lblMidias.Dock = DockStyle.Fill; lblMidias.TextAlign = ContentAlignment.MiddleLeft;
            label21.Text = "Facial:"; label21.Font = new Font("Segoe UI", 9F); label21.Dock = DockStyle.Fill; label21.TextAlign = ContentAlignment.MiddleLeft;
            lblFacial.Text = "-"; lblFacial.Font = new Font("Segoe UI", 9F); lblFacial.Dock = DockStyle.Fill; lblFacial.TextAlign = ContentAlignment.MiddleLeft;
            label23.Text = "RFID:"; label23.Font = new Font("Segoe UI", 9F); label23.Dock = DockStyle.Fill; label23.TextAlign = ContentAlignment.MiddleLeft;
            lblRfid.Text = "-"; lblRfid.Font = new Font("Segoe UI", 9F); lblRfid.Dock = DockStyle.Fill; lblRfid.TextAlign = ContentAlignment.MiddleLeft;
            label25.Text = "LPR (placas):"; label25.Font = new Font("Segoe UI", 9F); label25.Dock = DockStyle.Fill; label25.TextAlign = ContentAlignment.MiddleLeft;
            lblLpr.Text = "-"; lblLpr.Font = new Font("Segoe UI", 9F); lblLpr.Dock = DockStyle.Fill; lblLpr.TextAlign = ContentAlignment.MiddleLeft;
            label27.Text = "Controle:"; label27.Font = new Font("Segoe UI", 9F); label27.Dock = DockStyle.Fill; label27.TextAlign = ContentAlignment.MiddleLeft;
            lblControle.Text = "-"; lblControle.Font = new Font("Segoe UI", 9F); lblControle.Dock = DockStyle.Fill; lblControle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupCapacidade
            // 
            groupCapacidade.Controls.Add(progressCapacidade);
            groupCapacidade.Controls.Add(lblCapacidade);
            groupCapacidade.Dock = DockStyle.Fill;
            groupCapacidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupCapacidade.Location = new Point(11, 211);
            groupCapacidade.Name = "groupCapacidade";
            groupCapacidade.Size = new Size(286, 78);
            groupCapacidade.TabIndex = 2;
            groupCapacidade.Text = "Capacidade (central-registry/stats)";
            // 
            // lblCapacidade
            // 
            lblCapacidade.Dock = DockStyle.Top;
            lblCapacidade.Font = new Font("Segoe UI", 9F);
            lblCapacidade.Location = new Point(3, 19);
            lblCapacidade.Name = "lblCapacidade";
            lblCapacidade.Padding = new Padding(5, 3, 0, 0);
            lblCapacidade.Size = new Size(280, 22);
            lblCapacidade.TabIndex = 0;
            lblCapacidade.Text = "- / -";
            // 
            // progressCapacidade
            // 
            progressCapacidade.Dock = DockStyle.Bottom;
            progressCapacidade.Location = new Point(3, 52);
            progressCapacidade.Name = "progressCapacidade";
            progressCapacidade.Size = new Size(280, 23);
            progressCapacidade.TabIndex = 1;
            // 
            // panelBotoes
            // 
            panelBotoes.Controls.Add(btnAtualizar);
            panelBotoes.Dock = DockStyle.Fill;
            panelBotoes.Location = new Point(303, 211);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Padding = new Padding(5);
            panelBotoes.Size = new Size(286, 78);
            panelBotoes.TabIndex = 3;
            // 
            // btnAtualizar
            // 
            btnAtualizar.BackColor = Color.FromArgb(0, 123, 255);
            btnAtualizar.FlatStyle = FlatStyle.Flat;
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(8, 25);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(120, 30);
            btnAtualizar.TabIndex = 0;
            btnAtualizar.Text = "Atualizar";
            btnAtualizar.UseVisualStyleBackColor = false;
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 8.25F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(0, 335);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(600, 115);
            txtLog.TabIndex = 2;
            // 
            // FormDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 450);
            Controls.Add(txtLog);
            Controls.Add(panelCards);
            Controls.Add(lblExplicacao);
            MinimumSize = new Size(550, 400);
            Name = "FormDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard - Informações do Controlador";
            Load += FormDashboard_Load;
            panelCards.ResumeLayout(false);
            groupDevice.ResumeLayout(false);
            tableDevice.ResumeLayout(false);
            groupStats.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            groupCapacidade.ResumeLayout(false);
            panelBotoes.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplicacao;
        private TableLayoutPanel panelCards;
        private GroupBox groupDevice;
        private TableLayoutPanel tableDevice;
        private Label label1;
        private Label lblModelo;
        private Label label3;
        private Label lblFirmware;
        private Label label5;
        private Label lblGid;
        private Label label7;
        private Label lblUptime;
        private Label label9;
        private Label lblCpu;
        private Label label11;
        private Label lblMemoria;
        private GroupBox groupStats;
        private TableLayoutPanel tableStats;
        private Label label13;
        private Label lblCadastros;
        private Label label15;
        private Label lblPessoas;
        private Label label17;
        private Label lblVeiculos;
        private Label label19;
        private Label lblMidias;
        private Label label21;
        private Label lblFacial;
        private Label label23;
        private Label lblRfid;
        private Label label25;
        private Label lblLpr;
        private Label label27;
        private Label lblControle;
        private GroupBox groupCapacidade;
        private Label lblCapacidade;
        private ProgressBar progressCapacidade;
        private Panel panelBotoes;
        private Button btnAtualizar;
        private TextBox txtLog;
    }
}
