namespace SmartSdk
{
    partial class FormSaidas
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
            panelTop = new Panel();
            btnAtualizar = new Button();
            lblTime = new Label();
            txtTimeMs = new TextBox();
            btnPulse = new Button();
            btnOn = new Button();
            btnOff = new Button();
            btnToggle = new Button();
            splitMain = new SplitContainer();
            gridDevices = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNome = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colModelo = new DataGridViewTextBoxColumn();
            colOnline = new DataGridViewTextBoxColumn();
            colMaster = new DataGridViewTextBoxColumn();
            colSaidas = new DataGridViewTextBoxColumn();
            gridOutputs = new DataGridView();
            colKind = new DataGridViewTextBoxColumn();
            colNum = new DataGridViewTextBoxColumn();
            colLabel = new DataGridViewTextBoxColumn();
            colCmds = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            panelLog = new Panel();
            txtLog = new TextBox();
            btnLimparLog = new Button();
            lblLog = new Label();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridDevices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridOutputs).BeginInit();
            panelLog.SuspendLayout();
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
            lblExplicacao.Size = new Size(960, 40);
            lblExplicacao.TabIndex = 0;
            lblExplicacao.Text = "Outputs: GET /devices lists SMART / RS485 / ctrl / external. POST /devices/relay pulses or latches relay/DOUT. Pick a device, then an output.";
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnAtualizar);
            panelTop.Controls.Add(lblTime);
            panelTop.Controls.Add(txtTimeMs);
            panelTop.Controls.Add(btnPulse);
            panelTop.Controls.Add(btnOn);
            panelTop.Controls.Add(btnOff);
            panelTop.Controls.Add(btnToggle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 40);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(8);
            panelTop.Size = new Size(960, 48);
            panelTop.TabIndex = 1;
            // 
            // btnAtualizar
            // 
            btnAtualizar.BackColor = Color.FromArgb(0, 123, 255);
            btnAtualizar.FlatStyle = FlatStyle.Flat;
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(8, 10);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(110, 28);
            btnAtualizar.TabIndex = 0;
            btnAtualizar.Text = "Refresh";
            btnAtualizar.UseVisualStyleBackColor = false;
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new Point(140, 15);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(72, 15);
            lblTime.TabIndex = 1;
            lblTime.Text = "Pulse (ms):";
            // 
            // txtTimeMs
            // 
            txtTimeMs.Location = new Point(218, 12);
            txtTimeMs.Name = "txtTimeMs";
            txtTimeMs.Size = new Size(70, 23);
            txtTimeMs.TabIndex = 2;
            txtTimeMs.Text = "1000";
            // 
            // btnPulse
            // 
            btnPulse.BackColor = Color.FromArgb(40, 167, 69);
            btnPulse.Enabled = false;
            btnPulse.FlatStyle = FlatStyle.Flat;
            btnPulse.ForeColor = Color.White;
            btnPulse.Location = new Point(310, 10);
            btnPulse.Name = "btnPulse";
            btnPulse.Size = new Size(90, 28);
            btnPulse.TabIndex = 3;
            btnPulse.Text = "Pulse";
            btnPulse.UseVisualStyleBackColor = false;
            btnPulse.Click += btnPulse_Click;
            // 
            // btnOn
            // 
            btnOn.BackColor = Color.FromArgb(23, 162, 184);
            btnOn.Enabled = false;
            btnOn.FlatStyle = FlatStyle.Flat;
            btnOn.ForeColor = Color.White;
            btnOn.Location = new Point(410, 10);
            btnOn.Name = "btnOn";
            btnOn.Size = new Size(70, 28);
            btnOn.TabIndex = 4;
            btnOn.Text = "On";
            btnOn.UseVisualStyleBackColor = false;
            btnOn.Click += btnOn_Click;
            // 
            // btnOff
            // 
            btnOff.BackColor = Color.FromArgb(108, 117, 125);
            btnOff.Enabled = false;
            btnOff.FlatStyle = FlatStyle.Flat;
            btnOff.ForeColor = Color.White;
            btnOff.Location = new Point(490, 10);
            btnOff.Name = "btnOff";
            btnOff.Size = new Size(70, 28);
            btnOff.TabIndex = 5;
            btnOff.Text = "Off";
            btnOff.UseVisualStyleBackColor = false;
            btnOff.Click += btnOff_Click;
            // 
            // btnToggle
            // 
            btnToggle.BackColor = Color.FromArgb(111, 66, 193);
            btnToggle.Enabled = false;
            btnToggle.FlatStyle = FlatStyle.Flat;
            btnToggle.ForeColor = Color.White;
            btnToggle.Location = new Point(570, 10);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new Size(80, 28);
            btnToggle.TabIndex = 6;
            btnToggle.Text = "Toggle";
            btnToggle.UseVisualStyleBackColor = false;
            btnToggle.Click += btnToggle_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 88);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(gridDevices);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(gridOutputs);
            splitMain.Size = new Size(960, 372);
            splitMain.SplitterDistance = 220;
            splitMain.TabIndex = 2;
            // 
            // gridDevices
            // 
            gridDevices.AllowUserToAddRows = false;
            gridDevices.AllowUserToDeleteRows = false;
            gridDevices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridDevices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridDevices.Columns.AddRange(new DataGridViewColumn[] { colId, colNome, colTipo, colModelo, colOnline, colMaster, colSaidas });
            gridDevices.Dock = DockStyle.Fill;
            gridDevices.MultiSelect = false;
            gridDevices.Name = "gridDevices";
            gridDevices.ReadOnly = true;
            gridDevices.RowHeadersVisible = false;
            gridDevices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridDevices.SelectionChanged += gridDevices_SelectionChanged;
            // 
            // colId
            // 
            colId.HeaderText = "Id";
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colNome
            // 
            colNome.HeaderText = "Name";
            colNome.Name = "colNome";
            colNome.ReadOnly = true;
            // 
            // colTipo
            // 
            colTipo.HeaderText = "Type";
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            // 
            // colModelo
            // 
            colModelo.HeaderText = "Model";
            colModelo.Name = "colModelo";
            colModelo.ReadOnly = true;
            // 
            // colOnline
            // 
            colOnline.HeaderText = "Online";
            colOnline.Name = "colOnline";
            colOnline.ReadOnly = true;
            // 
            // colMaster
            // 
            colMaster.HeaderText = "Master";
            colMaster.Name = "colMaster";
            colMaster.ReadOnly = true;
            // 
            // colSaidas
            // 
            colSaidas.FillWeight = 40F;
            colSaidas.HeaderText = "Outputs";
            colSaidas.Name = "colSaidas";
            colSaidas.ReadOnly = true;
            // 
            // gridOutputs
            // 
            gridOutputs.AllowUserToAddRows = false;
            gridOutputs.AllowUserToDeleteRows = false;
            gridOutputs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridOutputs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridOutputs.Columns.AddRange(new DataGridViewColumn[] { colKind, colNum, colLabel, colCmds, colEstado });
            gridOutputs.Dock = DockStyle.Fill;
            gridOutputs.MultiSelect = false;
            gridOutputs.Name = "gridOutputs";
            gridOutputs.ReadOnly = true;
            gridOutputs.RowHeadersVisible = false;
            gridOutputs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridOutputs.SelectionChanged += gridOutputs_SelectionChanged;
            // 
            // colKind
            // 
            colKind.FillWeight = 50F;
            colKind.HeaderText = "Kind";
            colKind.Name = "colKind";
            colKind.ReadOnly = true;
            // 
            // colNum
            // 
            colNum.FillWeight = 40F;
            colNum.HeaderText = "N";
            colNum.Name = "colNum";
            colNum.ReadOnly = true;
            // 
            // colLabel
            // 
            colLabel.HeaderText = "Output";
            colLabel.Name = "colLabel";
            colLabel.ReadOnly = true;
            // 
            // colCmds
            // 
            colCmds.HeaderText = "Commands";
            colCmds.Name = "colCmds";
            colCmds.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.FillWeight = 50F;
            colEstado.HeaderText = "State";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // panelLog
            // 
            panelLog.Controls.Add(txtLog);
            panelLog.Controls.Add(btnLimparLog);
            panelLog.Controls.Add(lblLog);
            panelLog.Dock = DockStyle.Bottom;
            panelLog.Location = new Point(0, 460);
            panelLog.Name = "panelLog";
            panelLog.Padding = new Padding(5);
            panelLog.Size = new Size(960, 140);
            panelLog.TabIndex = 3;
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
            txtLog.Size = new Size(950, 84);
            txtLog.TabIndex = 1;
            // 
            // btnLimparLog
            // 
            btnLimparLog.Dock = DockStyle.Bottom;
            btnLimparLog.Location = new Point(5, 107);
            btnLimparLog.Name = "btnLimparLog";
            btnLimparLog.Size = new Size(950, 28);
            btnLimparLog.TabIndex = 2;
            btnLimparLog.Text = "Clear Log";
            btnLimparLog.Click += btnLimparLog_Click;
            // 
            // lblLog
            // 
            lblLog.Dock = DockStyle.Top;
            lblLog.Location = new Point(5, 5);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(950, 18);
            lblLog.TabIndex = 0;
            lblLog.Text = "Operations Log:";
            // 
            // FormSaidas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 600);
            Controls.Add(splitMain);
            Controls.Add(panelLog);
            Controls.Add(panelTop);
            Controls.Add(lblExplicacao);
            MinimumSize = new Size(800, 500);
            Name = "FormSaidas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Outputs (DOUT / Relays) — GET /devices + POST /devices/relay";
            Load += FormSaidas_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridDevices).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridOutputs).EndInit();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblExplicacao;
        private Panel panelTop;
        private Button btnAtualizar;
        private Label lblTime;
        private TextBox txtTimeMs;
        private Button btnPulse;
        private Button btnOn;
        private Button btnOff;
        private Button btnToggle;
        private SplitContainer splitMain;
        private DataGridView gridDevices;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNome;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colModelo;
        private DataGridViewTextBoxColumn colOnline;
        private DataGridViewTextBoxColumn colMaster;
        private DataGridViewTextBoxColumn colSaidas;
        private DataGridView gridOutputs;
        private DataGridViewTextBoxColumn colKind;
        private DataGridViewTextBoxColumn colNum;
        private DataGridViewTextBoxColumn colLabel;
        private DataGridViewTextBoxColumn colCmds;
        private DataGridViewTextBoxColumn colEstado;
        private Panel panelLog;
        private TextBox txtLog;
        private Button btnLimparLog;
        private Label lblLog;
    }
}
