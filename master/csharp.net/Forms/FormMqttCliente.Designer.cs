namespace SmartSdk
{
    partial class FormMqttCliente
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.txtSessionKey = new TextBox();
            this.label2 = new Label();
            this.txtWsUrl = new TextBox();
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.txtTopicoCustom = new TextBox();
            this.label3 = new Label();
            this.chkStatus = new CheckBox();
            this.chkSensors = new CheckBox();
            this.chkLogs = new CheckBox();
            this.chkEvents = new CheckBox();
            this.btnConectar = new Button();
            this.lblStatus = new Label();
            this.groupBox3 = new GroupBox();
            this.cmbPubQoS = new ComboBox();
            this.label5 = new Label();
            this.btnPublicar = new Button();
            this.txtPubPayload = new TextBox();
            this.label6 = new Label();
            this.txtPubTopico = new TextBox();
            this.label4 = new Label();
            this.txtLog = new TextBox();
            this.btnLimpar = new Button();
            this.btnSalvar = new Button();
            this.lblMensagens = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSessionKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtWsUrl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(560, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // txtSessionKey
            // 
            this.txtSessionKey.Location = new Point(100, 60);
            this.txtSessionKey.Name = "txtSessionKey";
            this.txtSessionKey.Size = new Size(440, 23);
            this.txtSessionKey.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 63);
            this.label2.Name = "label2";
            this.label2.Size = new Size(70, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Session Key:";
            // 
            // txtWsUrl
            // 
            this.txtWsUrl.Location = new Point(100, 25);
            this.txtWsUrl.Name = "txtWsUrl";
            this.txtWsUrl.Size = new Size(440, 23);
            this.txtWsUrl.TabIndex = 1;
            this.txtWsUrl.Text = "wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new Size(74, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "WebSocket URL:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTopicoCustom);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.chkStatus);
            this.groupBox2.Controls.Add(this.chkSensors);
            this.groupBox2.Controls.Add(this.chkLogs);
            this.groupBox2.Controls.Add(this.chkEvents);
            this.groupBox2.Location = new Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(300, 150);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Topics to Subscribe";
            // 
            // txtTopicoCustom
            // 
            this.txtTopicoCustom.Location = new Point(100, 110);
            this.txtTopicoCustom.Name = "txtTopicoCustom";
            this.txtTopicoCustom.Size = new Size(185, 23);
            this.txtTopicoCustom.TabIndex = 5;
            this.txtTopicoCustom.Text = "#";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(15, 113);
            this.label3.Name = "label3";
            this.label3.Size = new Size(55, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Custom:";
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new Point(150, 55);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new Size(57, 19);
            this.chkStatus.TabIndex = 3;
            this.chkStatus.Text = "Status";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // chkSensors
            // 
            this.chkSensors.AutoSize = true;
            this.chkSensors.Location = new Point(150, 25);
            this.chkSensors.Name = "chkSensors";
            this.chkSensors.Size = new Size(66, 19);
            this.chkSensors.TabIndex = 2;
            this.chkSensors.Text = "Sensors";
            this.chkSensors.UseVisualStyleBackColor = true;
            // 
            // chkLogs
            // 
            this.chkLogs.AutoSize = true;
            this.chkLogs.Location = new Point(15, 55);
            this.chkLogs.Name = "chkLogs";
            this.chkLogs.Size = new Size(51, 19);
            this.chkLogs.TabIndex = 1;
            this.chkLogs.Text = "Logs";
            this.chkLogs.UseVisualStyleBackColor = true;
            // 
            // chkEvents
            // 
            this.chkEvents.AutoSize = true;
            this.chkEvents.Location = new Point(15, 25);
            this.chkEvents.Name = "chkEvents";
            this.chkEvents.Size = new Size(61, 19);
            this.chkEvents.TabIndex = 0;
            this.chkEvents.Text = "Events";
            this.chkEvents.UseVisualStyleBackColor = true;
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new Point(330, 130);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new Size(110, 40);
            this.btnConectar.TabIndex = 2;
            this.btnConectar.Text = "Connect";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatus.Location = new Point(330, 180);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(85, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Disconnected";
            this.lblStatus.ForeColor = Color.Gray;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbPubQoS);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnPublicar);
            this.groupBox3.Controls.Add(this.txtPubPayload);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtPubTopico);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new Point(12, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(560, 130);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Publish Message";
            // 
            // cmbPubQoS
            // 
            this.cmbPubQoS.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPubQoS.FormattingEnabled = true;
            this.cmbPubQoS.Items.AddRange(new object[] { "0 - At most once", "1 - At least once", "2 - Exactly once" });
            this.cmbPubQoS.Location = new Point(435, 25);
            this.cmbPubQoS.Name = "cmbPubQoS";
            this.cmbPubQoS.Size = new Size(110, 23);
            this.cmbPubQoS.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(400, 28);
            this.label5.Name = "label5";
            this.label5.Size = new Size(32, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "QoS:";
            // 
            // btnPublicar
            // 
            this.btnPublicar.Location = new Point(435, 90);
            this.btnPublicar.Name = "btnPublicar";
            this.btnPublicar.Size = new Size(110, 30);
            this.btnPublicar.TabIndex = 4;
            this.btnPublicar.Text = "Publish";
            this.btnPublicar.UseVisualStyleBackColor = true;
            this.btnPublicar.Click += new System.EventHandler(this.btnPublicar_Click);
            // 
            // txtPubPayload
            // 
            this.txtPubPayload.Location = new Point(70, 60);
            this.txtPubPayload.Name = "txtPubPayload";
            this.txtPubPayload.Size = new Size(350, 60);
            this.txtPubPayload.TabIndex = 3;
            this.txtPubPayload.Text = "{\"test\":true}";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new Point(15, 63);
            this.label6.Name = "label6";
            this.label6.Size = new Size(47, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Payload:";
            // 
            // txtPubTopico
            // 
            this.txtPubTopico.Location = new Point(70, 25);
            this.txtPubTopico.Name = "txtPubTopico";
            this.txtPubTopico.Size = new Size(310, 23);
            this.txtPubTopico.TabIndex = 1;
            this.txtPubTopico.Text = "test/topic";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(15, 28);
            this.label4.Name = "label4";
            this.label4.Size = new Size(43, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Topic:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtLog.Font = new Font("Consolas", 9F);
            this.txtLog.Location = new Point(12, 430);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(560, 150);
            this.txtLog.TabIndex = 5;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnLimpar.Location = new Point(12, 590);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new Size(75, 25);
            this.btnLimpar.TabIndex = 6;
            this.btnLimpar.Text = "Clear";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSalvar.Location = new Point(100, 590);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(100, 25);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "Save JSON";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // lblMensagens
            // 
            this.lblMensagens.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblMensagens.AutoSize = true;
            this.lblMensagens.Location = new Point(220, 595);
            this.lblMensagens.Name = "lblMensagens";
            this.lblMensagens.Size = new Size(80, 15);
            this.lblMensagens.TabIndex = 8;
            this.lblMensagens.Text = "Messages: 0";
            // 
            // FormMqttCliente
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 621);
            this.Controls.Add(this.lblMensagens);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new Size(600, 660);
            this.Name = "FormMqttCliente";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MQTT Client Test";
            this.Load += new System.EventHandler(this.FormMqttCliente_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtWsUrl;
        private Label label1;
        private TextBox txtSessionKey;
        private Label label2;
        private GroupBox groupBox2;
        private CheckBox chkEvents;
        private CheckBox chkLogs;
        private CheckBox chkSensors;
        private CheckBox chkStatus;
        private TextBox txtTopicoCustom;
        private Label label3;
        private Button btnConectar;
        private Label lblStatus;
        private GroupBox groupBox3;
        private Label label4;
        private TextBox txtPubTopico;
        private Label label5;
        private ComboBox cmbPubQoS;
        private Button btnPublicar;
        private TextBox txtPubPayload;
        private Label label6;
        private TextBox txtLog;
        private Button btnLimpar;
        private Button btnSalvar;
        private Label lblMensagens;
    }
}
