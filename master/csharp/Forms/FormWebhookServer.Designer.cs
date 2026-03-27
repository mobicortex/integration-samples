namespace SmartSdk
{
    partial class FormWebhookServer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            this.groupBox1 = new GroupBox();
            this.lblUrl = new Label();
            this.label3 = new Label();
            this.txtToken = new TextBox();
            this.chkAuth = new CheckBox();
            this.txtPorta = new TextBox();
            this.label1 = new Label();
            this.btnIniciar = new Button();
            this.lblStatus = new Label();
            this.txtLog = new TextBox();
            this.btnLimpar = new Button();
            this.btnSalvar = new Button();
            this.groupBox2 = new GroupBox();
            this.gridWebhooks = new DataGridView();
            this.ColTimestamp = new DataGridViewTextBoxColumn();
            this.ColMethod = new DataGridViewTextBoxColumn();
            this.ColPath = new DataGridViewTextBoxColumn();
            this.ColIP = new DataGridViewTextBoxColumn();
            this.ColBody = new DataGridViewTextBoxColumn();
            this.btnVerDetalhes = new Button();
            this.lblTotal = new Label();
            this.lblSucesso = new Label();
            this.lblErros = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.gridWebhooks).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblUrl);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtToken);
            this.groupBox1.Controls.Add(this.chkAuth);
            this.groupBox1.Controls.Add(this.txtPorta);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(760, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Webhook Server Configuration";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblUrl.ForeColor = Color.DarkBlue;
            this.lblUrl.Location = new Point(400, 28);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new Size(127, 15);
            this.lblUrl.TabIndex = 5;
            this.lblUrl.Text = "http://localhost:8080/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(320, 28);
            this.label3.Name = "label3";
            this.label3.Size = new Size(74, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "URL Base:";
            // 
            // txtToken
            // 
            this.txtToken.Enabled = false;
            this.txtToken.Location = new Point(150, 60);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new Size(300, 23);
            this.txtToken.TabIndex = 3;
            this.txtToken.Text = "your-token-here";
            // 
            // chkAuth
            // 
            this.chkAuth.AutoSize = true;
            this.chkAuth.Location = new Point(15, 62);
            this.chkAuth.Name = "chkAuth";
            this.chkAuth.Size = new Size(129, 19);
            this.chkAuth.TabIndex = 2;
            this.chkAuth.Text = "Bearer Authentication";
            this.chkAuth.UseVisualStyleBackColor = true;
            this.chkAuth.CheckedChanged += this.chkAuth_CheckedChanged;
            // 
            // txtPorta
            // 
            this.txtPorta.Location = new Point(60, 25);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Size = new Size(80, 23);
            this.txtPorta.TabIndex = 1;
            this.txtPorta.Text = "8080";
            this.txtPorta.TextChanged += this.txtPorta_TextChanged;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new Point(12, 125);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new Size(110, 40);
            this.btnIniciar.TabIndex = 1;
            this.btnIniciar.Text = "Start";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += this.btnIniciar_Click;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatus.Location = new Point(140, 137);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(51, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Stopped";
            this.lblStatus.ForeColor = Color.Gray;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.txtLog.Font = new Font("Consolas", 9F);
            this.txtLog.Location = new Point(12, 180);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(350, 280);
            this.txtLog.TabIndex = 3;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnLimpar.Location = new Point(12, 470);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new Size(75, 25);
            this.btnLimpar.TabIndex = 4;
            this.btnLimpar.Text = "Clear";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += this.btnLimpar_Click;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSalvar.Location = new Point(100, 470);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(100, 25);
            this.btnSalvar.TabIndex = 5;
            this.btnSalvar.Text = "Save JSON";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += this.btnSalvar_Click;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.groupBox2.Controls.Add(this.gridWebhooks);
            this.groupBox2.Controls.Add(this.btnVerDetalhes);
            this.groupBox2.Location = new Point(380, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(392, 335);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Received Webhooks";
            // 
            // gridWebhooks
            // 
            this.gridWebhooks.AllowUserToAddRows = false;
            this.gridWebhooks.AllowUserToDeleteRows = false;
            this.gridWebhooks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.gridWebhooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridWebhooks.BackgroundColor = SystemColors.Window;
            this.gridWebhooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridWebhooks.Columns.AddRange(new DataGridViewColumn[] { this.ColTimestamp, this.ColMethod, this.ColPath, this.ColIP, this.ColBody });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.gridWebhooks.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridWebhooks.Location = new Point(10, 25);
            this.gridWebhooks.MultiSelect = false;
            this.gridWebhooks.Name = "gridWebhooks";
            this.gridWebhooks.ReadOnly = true;
            this.gridWebhooks.RowHeadersVisible = false;
            this.gridWebhooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridWebhooks.Size = new Size(370, 270);
            this.gridWebhooks.TabIndex = 0;
            // 
            // ColTimestamp
            // 
            this.ColTimestamp.HeaderText = "Hora";
            this.ColTimestamp.Name = "ColTimestamp";
            this.ColTimestamp.ReadOnly = true;
            this.ColTimestamp.Width = 60;
            // 
            // ColMethod
            // 
            this.ColMethod.HeaderText = "Método";
            this.ColMethod.Name = "ColMethod";
            this.ColMethod.ReadOnly = true;
            this.ColMethod.Width = 50;
            // 
            // ColPath
            // 
            this.ColPath.HeaderText = "Path";
            this.ColPath.Name = "ColPath";
            this.ColPath.ReadOnly = true;
            this.ColPath.Width = 70;
            // 
            // ColIP
            // 
            this.ColIP.HeaderText = "IP";
            this.ColIP.Name = "ColIP";
            this.ColIP.ReadOnly = true;
            this.ColIP.Width = 80;
            // 
            // ColBody
            // 
            this.ColBody.HeaderText = "Body";
            this.ColBody.Name = "ColBody";
            this.ColBody.ReadOnly = true;
            // 
            // btnVerDetalhes
            // 
            this.btnVerDetalhes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnVerDetalhes.Location = new Point(280, 300);
            this.btnVerDetalhes.Name = "btnVerDetalhes";
            this.btnVerDetalhes.Size = new Size(100, 25);
            this.btnVerDetalhes.TabIndex = 1;
            this.btnVerDetalhes.Text = "View Details";
            this.btnVerDetalhes.UseVisualStyleBackColor = true;
            this.btnVerDetalhes.Click += this.btnVerDetalhes_Click;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new Point(220, 475);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new Size(40, 15);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "Total: 0";
            // 
            // lblSucesso
            // 
            this.lblSucesso.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblSucesso.AutoSize = true;
            this.lblSucesso.ForeColor = Color.DarkGreen;
            this.lblSucesso.Location = new Point(290, 475);
            this.lblSucesso.Name = "lblSucesso";
            this.lblSucesso.Size = new Size(56, 15);
            this.lblSucesso.TabIndex = 8;
            this.lblSucesso.Text = "Success: 0";
            // 
            // lblErros
            // 
            this.lblErros.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblErros.AutoSize = true;
            this.lblErros.ForeColor = Color.DarkRed;
            this.lblErros.Location = new Point(370, 475);
            this.lblErros.Name = "lblErros";
            this.lblErros.Size = new Size(45, 15);
            this.lblErros.TabIndex = 9;
            this.lblErros.Text = "Errors: 0";
            // 
            // FormWebhookServer
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 501);
            this.Controls.Add(this.lblErros);
            this.Controls.Add(this.lblSucesso);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new Size(800, 540);
            this.Name = "FormWebhookServer";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Webhook Server Test";
            this.Load += this.FormWebhookServer_Load;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.gridWebhooks).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private GroupBox groupBox1;
        private Label label1;
        private TextBox txtPorta;
        private CheckBox chkAuth;
        private TextBox txtToken;
        private Label label3;
        private Label lblUrl;
        private Button btnIniciar;
        private Label lblStatus;
        private TextBox txtLog;
        private Button btnLimpar;
        private Button btnSalvar;
        private GroupBox groupBox2;
        private DataGridView gridWebhooks;
        private Button btnVerDetalhes;
        private Label lblTotal;
        private Label lblSucesso;
        private Label lblErros;
        private DataGridViewTextBoxColumn ColTimestamp;
        private DataGridViewTextBoxColumn ColMethod;
        private DataGridViewTextBoxColumn ColPath;
        private DataGridViewTextBoxColumn ColIP;
        private DataGridViewTextBoxColumn ColBody;
    }
}
