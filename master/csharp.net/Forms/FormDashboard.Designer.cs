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
            this.lblExplicacao = new Label();
            this.panelCards = new TableLayoutPanel();
            this.groupDevice = new GroupBox();
            this.tableDevice = new TableLayoutPanel();
            this.label1 = new Label();
            this.lblModelo = new Label();
            this.label3 = new Label();
            this.lblFirmware = new Label();
            this.label5 = new Label();
            this.lblGid = new Label();
            this.label7 = new Label();
            this.lblUptime = new Label();
            this.label9 = new Label();
            this.lblCpu = new Label();
            this.label11 = new Label();
            this.lblMemoria = new Label();
            this.groupStats = new GroupBox();
            this.tableStats = new TableLayoutPanel();
            this.label13 = new Label();
            this.lblCadastros = new Label();
            this.label15 = new Label();
            this.lblPessoas = new Label();
            this.label17 = new Label();
            this.lblVeiculos = new Label();
            this.label19 = new Label();
            this.lblMidias = new Label();
            this.label21 = new Label();
            this.lblFacial = new Label();
            this.label23 = new Label();
            this.lblRfid = new Label();
            this.label25 = new Label();
            this.lblLpr = new Label();
            this.label27 = new Label();
            this.lblControle = new Label();
            this.groupCapacidade = new GroupBox();
            this.lblCapacidade = new Label();
            this.progressCapacidade = new ProgressBar();
            this.panelBotoes = new Panel();
            this.btnAtualizar = new Button();
            this.txtLog = new TextBox();
            this.panelCards.SuspendLayout();
            this.groupDevice.SuspendLayout();
            this.tableDevice.SuspendLayout();
            this.groupStats.SuspendLayout();
            this.tableStats.SuspendLayout();
            this.groupCapacidade.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            //
            // lblExplicacao
            //
            this.lblExplicacao.BackColor = Color.FromArgb(0, 128, 128);
            this.lblExplicacao.Dock = DockStyle.Top;
            this.lblExplicacao.Font = new Font("Segoe UI", 9F);
            this.lblExplicacao.ForeColor = Color.White;
            this.lblExplicacao.Location = new Point(0, 0);
            this.lblExplicacao.Name = "lblExplicacao";
            this.lblExplicacao.Padding = new Padding(8, 4, 8, 4);
            this.lblExplicacao.Size = new Size(600, 35);
            this.lblExplicacao.TabIndex = 0;
            this.lblExplicacao.Text = "Dashboard: GET /device-info + GET /dashboard + GET /central-registry/stats";
            //
            // panelCards
            //
            this.panelCards.ColumnCount = 2;
            this.panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.panelCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.panelCards.Controls.Add(this.groupDevice, 0, 0);
            this.panelCards.Controls.Add(this.groupStats, 1, 0);
            this.panelCards.Controls.Add(this.groupCapacidade, 0, 1);
            this.panelCards.Controls.Add(this.panelBotoes, 1, 1);
            this.panelCards.Dock = DockStyle.Top;
            this.panelCards.Location = new Point(0, 35);
            this.panelCards.Name = "panelCards";
            this.panelCards.Padding = new Padding(8);
            this.panelCards.RowCount = 2;
            this.panelCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            this.panelCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            this.panelCards.Size = new Size(600, 300);
            this.panelCards.TabIndex = 1;
            //
            // groupDevice - Informações do Hardware
            //
            this.groupDevice.Controls.Add(this.tableDevice);
            this.groupDevice.Dock = DockStyle.Fill;
            this.groupDevice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.groupDevice.Location = new Point(11, 11);
            this.groupDevice.Name = "groupDevice";
            this.groupDevice.Size = new Size(286, 194);
            this.groupDevice.TabIndex = 0;
            this.groupDevice.Text = "Device (device-info)";
            //
            // tableDevice
            //
            this.tableDevice.ColumnCount = 2;
            this.tableDevice.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            this.tableDevice.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableDevice.Controls.Add(this.label1, 0, 0);
            this.tableDevice.Controls.Add(this.lblModelo, 1, 0);
            this.tableDevice.Controls.Add(this.label3, 0, 1);
            this.tableDevice.Controls.Add(this.lblFirmware, 1, 1);
            this.tableDevice.Controls.Add(this.label5, 0, 2);
            this.tableDevice.Controls.Add(this.lblGid, 1, 2);
            this.tableDevice.Controls.Add(this.label7, 0, 3);
            this.tableDevice.Controls.Add(this.lblUptime, 1, 3);
            this.tableDevice.Controls.Add(this.label9, 0, 4);
            this.tableDevice.Controls.Add(this.lblCpu, 1, 4);
            this.tableDevice.Controls.Add(this.label11, 0, 5);
            this.tableDevice.Controls.Add(this.lblMemoria, 1, 5);
            this.tableDevice.Dock = DockStyle.Fill;
            this.tableDevice.Location = new Point(3, 19);
            this.tableDevice.Name = "tableDevice";
            this.tableDevice.RowCount = 6;
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tableDevice.Size = new Size(280, 172);
            this.tableDevice.TabIndex = 0;
            //
            // Labels do device
            //
            this.label1.Text = "Model:"; this.label1.Font = new Font("Segoe UI", 9F); this.label1.Dock = DockStyle.Fill; this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.lblModelo.Text = "-"; this.lblModelo.Font = new Font("Segoe UI", 9F, FontStyle.Bold); this.lblModelo.Dock = DockStyle.Fill; this.lblModelo.TextAlign = ContentAlignment.MiddleLeft;
            this.label3.Text = "Firmware:"; this.label3.Font = new Font("Segoe UI", 9F); this.label3.Dock = DockStyle.Fill; this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.lblFirmware.Text = "-"; this.lblFirmware.Font = new Font("Segoe UI", 9F); this.lblFirmware.Dock = DockStyle.Fill; this.lblFirmware.TextAlign = ContentAlignment.MiddleLeft;
            this.label5.Text = "GID:"; this.label5.Font = new Font("Segoe UI", 9F); this.label5.Dock = DockStyle.Fill; this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.lblGid.Text = "-"; this.lblGid.Font = new Font("Segoe UI", 9F); this.lblGid.Dock = DockStyle.Fill; this.lblGid.TextAlign = ContentAlignment.MiddleLeft;
            this.label7.Text = "Uptime:"; this.label7.Font = new Font("Segoe UI", 9F); this.label7.Dock = DockStyle.Fill; this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.lblUptime.Text = "-"; this.lblUptime.Font = new Font("Segoe UI", 9F); this.lblUptime.Dock = DockStyle.Fill; this.lblUptime.TextAlign = ContentAlignment.MiddleLeft;
            this.label9.Text = "CPU:"; this.label9.Font = new Font("Segoe UI", 9F); this.label9.Dock = DockStyle.Fill; this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCpu.Text = "-"; this.lblCpu.Font = new Font("Segoe UI", 9F); this.lblCpu.Dock = DockStyle.Fill; this.lblCpu.TextAlign = ContentAlignment.MiddleLeft;
            this.label11.Text = "Memory:"; this.label11.Font = new Font("Segoe UI", 9F); this.label11.Dock = DockStyle.Fill; this.label11.TextAlign = ContentAlignment.MiddleLeft;
            this.lblMemoria.Text = "-"; this.lblMemoria.Font = new Font("Segoe UI", 9F); this.lblMemoria.Dock = DockStyle.Fill; this.lblMemoria.TextAlign = ContentAlignment.MiddleLeft;
            //
            // groupStats - Estatísticas
            //
            this.groupStats.Controls.Add(this.tableStats);
            this.groupStats.Dock = DockStyle.Fill;
            this.groupStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.groupStats.Location = new Point(303, 11);
            this.groupStats.Name = "groupStats";
            this.groupStats.Size = new Size(286, 194);
            this.groupStats.TabIndex = 1;
            this.groupStats.Text = "Statistics (dashboard)";
            //
            // tableStats
            //
            this.tableStats.ColumnCount = 2;
            this.tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            this.tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableStats.Controls.Add(this.label13, 0, 0);
            this.tableStats.Controls.Add(this.lblCadastros, 1, 0);
            this.tableStats.Controls.Add(this.label15, 0, 1);
            this.tableStats.Controls.Add(this.lblPessoas, 1, 1);
            this.tableStats.Controls.Add(this.label17, 0, 2);
            this.tableStats.Controls.Add(this.lblVeiculos, 1, 2);
            this.tableStats.Controls.Add(this.label19, 0, 3);
            this.tableStats.Controls.Add(this.lblMidias, 1, 3);
            this.tableStats.Controls.Add(this.label21, 0, 4);
            this.tableStats.Controls.Add(this.lblFacial, 1, 4);
            this.tableStats.Controls.Add(this.label23, 0, 5);
            this.tableStats.Controls.Add(this.lblRfid, 1, 5);
            this.tableStats.Controls.Add(this.label25, 0, 6);
            this.tableStats.Controls.Add(this.lblLpr, 1, 6);
            this.tableStats.Controls.Add(this.label27, 0, 7);
            this.tableStats.Controls.Add(this.lblControle, 1, 7);
            this.tableStats.Dock = DockStyle.Fill;
            this.tableStats.Location = new Point(3, 19);
            this.tableStats.Name = "tableStats";
            this.tableStats.RowCount = 8;
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            this.tableStats.Size = new Size(280, 172);
            this.tableStats.TabIndex = 0;
            //
            // Labels de stats
            //
            this.label13.Text = "Registries:"; this.label13.Font = new Font("Segoe UI", 9F); this.label13.Dock = DockStyle.Fill; this.label13.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCadastros.Text = "-"; this.lblCadastros.Font = new Font("Segoe UI", 9F, FontStyle.Bold); this.lblCadastros.Dock = DockStyle.Fill; this.lblCadastros.TextAlign = ContentAlignment.MiddleLeft;
            this.label15.Text = "Persons:"; this.label15.Font = new Font("Segoe UI", 9F); this.label15.Dock = DockStyle.Fill; this.label15.TextAlign = ContentAlignment.MiddleLeft;
            this.lblPessoas.Text = "-"; this.lblPessoas.Font = new Font("Segoe UI", 9F); this.lblPessoas.Dock = DockStyle.Fill; this.lblPessoas.TextAlign = ContentAlignment.MiddleLeft;
            this.label17.Text = "Vehicles:"; this.label17.Font = new Font("Segoe UI", 9F); this.label17.Dock = DockStyle.Fill; this.label17.TextAlign = ContentAlignment.MiddleLeft;
            this.lblVeiculos.Text = "-"; this.lblVeiculos.Font = new Font("Segoe UI", 9F); this.lblVeiculos.Dock = DockStyle.Fill; this.lblVeiculos.TextAlign = ContentAlignment.MiddleLeft;
            this.label19.Text = "Total Media:"; this.label19.Font = new Font("Segoe UI", 9F); this.label19.Dock = DockStyle.Fill; this.label19.TextAlign = ContentAlignment.MiddleLeft;
            this.lblMidias.Text = "-"; this.lblMidias.Font = new Font("Segoe UI", 9F); this.lblMidias.Dock = DockStyle.Fill; this.lblMidias.TextAlign = ContentAlignment.MiddleLeft;
            this.label21.Text = "Facial:"; this.label21.Font = new Font("Segoe UI", 9F); this.label21.Dock = DockStyle.Fill; this.label21.TextAlign = ContentAlignment.MiddleLeft;
            this.lblFacial.Text = "-"; this.lblFacial.Font = new Font("Segoe UI", 9F); this.lblFacial.Dock = DockStyle.Fill; this.lblFacial.TextAlign = ContentAlignment.MiddleLeft;
            this.label23.Text = "RFID:"; this.label23.Font = new Font("Segoe UI", 9F); this.label23.Dock = DockStyle.Fill; this.label23.TextAlign = ContentAlignment.MiddleLeft;
            this.lblRfid.Text = "-"; this.lblRfid.Font = new Font("Segoe UI", 9F); this.lblRfid.Dock = DockStyle.Fill; this.lblRfid.TextAlign = ContentAlignment.MiddleLeft;
            this.label25.Text = "LPR (plates):"; this.label25.Font = new Font("Segoe UI", 9F); this.label25.Dock = DockStyle.Fill; this.label25.TextAlign = ContentAlignment.MiddleLeft;
            this.lblLpr.Text = "-"; this.lblLpr.Font = new Font("Segoe UI", 9F); this.lblLpr.Dock = DockStyle.Fill; this.lblLpr.TextAlign = ContentAlignment.MiddleLeft;
            this.label27.Text = "Control:"; this.label27.Font = new Font("Segoe UI", 9F); this.label27.Dock = DockStyle.Fill; this.label27.TextAlign = ContentAlignment.MiddleLeft;
            this.lblControle.Text = "-"; this.lblControle.Font = new Font("Segoe UI", 9F); this.lblControle.Dock = DockStyle.Fill; this.lblControle.TextAlign = ContentAlignment.MiddleLeft;
            //
            // groupCapacidade
            //
            this.groupCapacidade.Controls.Add(this.progressCapacidade);
            this.groupCapacidade.Controls.Add(this.lblCapacidade);
            this.groupCapacidade.Dock = DockStyle.Fill;
            this.groupCapacidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.groupCapacidade.Location = new Point(11, 211);
            this.groupCapacidade.Name = "groupCapacidade";
            this.groupCapacidade.Size = new Size(286, 78);
            this.groupCapacidade.TabIndex = 2;
            this.groupCapacidade.Text = "Capacity (central-registry/stats)";
            //
            // lblCapacidade
            //
            this.lblCapacidade.Dock = DockStyle.Top;
            this.lblCapacidade.Font = new Font("Segoe UI", 9F);
            this.lblCapacidade.Location = new Point(3, 19);
            this.lblCapacidade.Name = "lblCapacidade";
            this.lblCapacidade.Padding = new Padding(5, 3, 0, 0);
            this.lblCapacidade.Size = new Size(280, 22);
            this.lblCapacidade.TabIndex = 0;
            this.lblCapacidade.Text = "- / -";
            //
            // progressCapacidade
            //
            this.progressCapacidade.Dock = DockStyle.Bottom;
            this.progressCapacidade.Location = new Point(3, 52);
            this.progressCapacidade.Name = "progressCapacidade";
            this.progressCapacidade.Size = new Size(280, 23);
            this.progressCapacidade.TabIndex = 1;
            //
            // panelBotoes
            //
            this.panelBotoes.Controls.Add(this.btnAtualizar);
            this.panelBotoes.Dock = DockStyle.Fill;
            this.panelBotoes.Location = new Point(303, 211);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Padding = new Padding(5);
            this.panelBotoes.Size = new Size(286, 78);
            this.panelBotoes.TabIndex = 3;
            //
            // btnAtualizar
            //
            this.btnAtualizar.BackColor = Color.FromArgb(0, 123, 255);
            this.btnAtualizar.FlatStyle = FlatStyle.Flat;
            this.btnAtualizar.ForeColor = Color.White;
            this.btnAtualizar.Location = new Point(8, 25);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new Size(120, 30);
            this.btnAtualizar.TabIndex = 0;
            this.btnAtualizar.Text = "Refresh";
            this.btnAtualizar.UseVisualStyleBackColor = false;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            //
            // txtLog
            //
            this.txtLog.BackColor = Color.FromArgb(30, 30, 30);
            this.txtLog.Dock = DockStyle.Fill;
            this.txtLog.Font = new Font("Consolas", 8.25F);
            this.txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            this.txtLog.Location = new Point(0, 335);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(600, 115);
            this.txtLog.TabIndex = 2;
            //
            // FormDashboard
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panelCards);
            this.Controls.Add(this.lblExplicacao);
            this.MinimumSize = new Size(550, 400);
            this.Name = "FormDashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dashboard - Controller Information";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            this.panelCards.ResumeLayout(false);
            this.groupDevice.ResumeLayout(false);
            this.tableDevice.ResumeLayout(false);
            this.groupStats.ResumeLayout(false);
            this.tableStats.ResumeLayout(false);
            this.groupCapacidade.ResumeLayout(false);
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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
