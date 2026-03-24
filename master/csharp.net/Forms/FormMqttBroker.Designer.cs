namespace SmartSdk
{
    partial class FormMqttBroker
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
            this.groupBox1 = new GroupBox();
            this.txtSenha = new TextBox();
            this.lblSenha = new Label();
            this.txtUsuario = new TextBox();
            this.lblUsuario = new Label();
            this.rbAuth = new RadioButton();
            this.rbAnonimo = new RadioButton();
            this.txtPorta = new TextBox();
            this.label1 = new Label();
            this.btnIniciar = new Button();
            this.lblStatus = new Label();
            this.groupBox2 = new GroupBox();
            this.cmbQoS = new ComboBox();
            this.label4 = new Label();
            this.btnPublicar = new Button();
            this.txtPubPayload = new TextBox();
            this.label3 = new Label();
            this.txtPubTopico = new TextBox();
            this.label2 = new Label();
            this.txtLog = new TextBox();
            this.btnLimpar = new Button();
            this.btnSalvar = new Button();
            this.lblClientes = new Label();
            this.lblMensagens = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSenha);
            this.groupBox1.Controls.Add(this.lblSenha);
            this.groupBox1.Controls.Add(this.txtUsuario);
            this.groupBox1.Controls.Add(this.lblUsuario);
            this.groupBox1.Controls.Add(this.rbAuth);
            this.groupBox1.Controls.Add(this.rbAnonimo);
            this.groupBox1.Controls.Add(this.txtPorta);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(560, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuração do Broker";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new Point(350, 90);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '●';
            this.txtSenha.Size = new Size(120, 23);
            this.txtSenha.TabIndex = 7;
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Location = new Point(300, 93);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new Size(42, 15);
            this.lblSenha.TabIndex = 6;
            this.lblSenha.Text = "Senha:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new Point(150, 90);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new Size(120, 23);
            this.txtUsuario.TabIndex = 5;
            this.txtUsuario.Text = "mobicortex";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new Point(90, 93);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new Size(50, 15);
            this.lblUsuario.TabIndex = 4;
            this.lblUsuario.Text = "Usuário:";
            // 
            // rbAuth
            // 
            this.rbAuth.AutoSize = true;
            this.rbAuth.Location = new Point(15, 65);
            this.rbAuth.Name = "rbAuth";
            this.rbAuth.Size = new Size(95, 19);
            this.rbAuth.TabIndex = 3;
            this.rbAuth.Text = "Autenticação";
            this.rbAuth.UseVisualStyleBackColor = true;
            this.rbAuth.CheckedChanged += this.rbAuth_CheckedChanged;
            // 
            // rbAnonimo
            // 
            this.rbAnonimo.AutoSize = true;
            this.rbAnonimo.Checked = true;
            this.rbAnonimo.Location = new Point(15, 40);
            this.rbAnonimo.Name = "rbAnonimo";
            this.rbAnonimo.Size = new Size(118, 19);
            this.rbAnonimo.TabIndex = 2;
            this.rbAnonimo.TabStop = true;
            this.rbAnonimo.Text = "Acesso Anônimo";
            this.rbAnonimo.UseVisualStyleBackColor = true;
            this.rbAnonimo.CheckedChanged += this.rbAnonimo_CheckedChanged;
            // 
            // txtPorta
            // 
            this.txtPorta.Location = new Point(440, 25);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Size = new Size(100, 23);
            this.txtPorta.TabIndex = 1;
            this.txtPorta.Text = "1883";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(395, 28);
            this.label1.Name = "label1";
            this.label1.Size = new Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Porta:";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new Point(12, 155);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new Size(110, 40);
            this.btnIniciar.TabIndex = 1;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += this.btnIniciar_Click;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblStatus.Location = new Point(140, 167);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(51, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Parado";
            this.lblStatus.ForeColor = Color.Gray;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbQoS);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnPublicar);
            this.groupBox2.Controls.Add(this.txtPubPayload);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtPubTopico);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new Point(12, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(560, 140);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Publicar Mensagem no Broker";
            // 
            // cmbQoS
            // 
            this.cmbQoS.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbQoS.FormattingEnabled = true;
            this.cmbQoS.Items.AddRange(new object[] { "0 - At most once", "1 - At least once", "2 - Exactly once" });
            this.cmbQoS.Location = new Point(435, 25);
            this.cmbQoS.Name = "cmbQoS";
            this.cmbQoS.Size = new Size(110, 23);
            this.cmbQoS.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(400, 28);
            this.label4.Name = "label4";
            this.label4.Size = new Size(32, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "QoS:";
            // 
            // btnPublicar
            // 
            this.btnPublicar.Location = new Point(435, 100);
            this.btnPublicar.Name = "btnPublicar";
            this.btnPublicar.Size = new Size(110, 30);
            this.btnPublicar.TabIndex = 4;
            this.btnPublicar.Text = "Publicar";
            this.btnPublicar.UseVisualStyleBackColor = true;
            this.btnPublicar.Click += this.btnPublicar_Click;
            // 
            // txtPubPayload
            // 
            this.txtPubPayload.Location = new Point(70, 60);
            this.txtPubPayload.Multiline = true;
            this.txtPubPayload.Name = "txtPubPayload";
            this.txtPubPayload.Size = new Size(350, 70);
            this.txtPubPayload.TabIndex = 3;
            this.txtPubPayload.Text = "{\"event\":\"test\",\"timestamp\":\"2024-01-01T00:00:00Z\"}";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(15, 63);
            this.label3.Name = "label3";
            this.label3.Size = new Size(47, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Payload:";
            // 
            // txtPubTopico
            // 
            this.txtPubTopico.Location = new Point(70, 25);
            this.txtPubTopico.Name = "txtPubTopico";
            this.txtPubTopico.Size = new Size(310, 23);
            this.txtPubTopico.TabIndex = 1;
            this.txtPubTopico.Text = "mbcortex/events/access";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 28);
            this.label2.Name = "label2";
            this.label2.Size = new Size(43, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tópico:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtLog.Font = new Font("Consolas", 9F);
            this.txtLog.Location = new Point(12, 360);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(560, 150);
            this.txtLog.TabIndex = 4;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnLimpar.Location = new Point(12, 520);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new Size(75, 25);
            this.btnLimpar.TabIndex = 5;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += this.btnLimpar_Click;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSalvar.Location = new Point(100, 520);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(100, 25);
            this.btnSalvar.TabIndex = 6;
            this.btnSalvar.Text = "Salvar Stats";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += this.btnSalvar_Click;
            // 
            // lblClientes
            // 
            this.lblClientes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblClientes.AutoSize = true;
            this.lblClientes.Location = new Point(220, 525);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new Size(57, 15);
            this.lblClientes.TabIndex = 7;
            this.lblClientes.Text = "Clientes: 0";
            // 
            // lblMensagens
            // 
            this.lblMensagens.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblMensagens.AutoSize = true;
            this.lblMensagens.Location = new Point(320, 525);
            this.lblMensagens.Name = "lblMensagens";
            this.lblMensagens.Size = new Size(75, 15);
            this.lblMensagens.TabIndex = 8;
            this.lblMensagens.Text = "Mensagens: 0";
            // 
            // FormMqttBroker
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(584, 551);
            this.Controls.Add(this.lblMensagens);
            this.Controls.Add(this.lblClientes);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new Size(600, 590);
            this.Name = "FormMqttBroker";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Teste MQTT Broker (Embutido)";
            this.Load += this.FormMqttBroker_Load;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void rbAnonimo_CheckedChanged(object? sender, EventArgs e)
        {
            txtUsuario.Enabled = !rbAnonimo.Checked;
            txtSenha.Enabled = !rbAnonimo.Checked;
        }

        private void rbAuth_CheckedChanged(object? sender, EventArgs e)
        {
            txtUsuario.Enabled = rbAuth.Checked;
            txtSenha.Enabled = rbAuth.Checked;
        }

        private GroupBox groupBox1;
        private Label label1;
        private TextBox txtPorta;
        private RadioButton rbAuth;
        private RadioButton rbAnonimo;
        private TextBox txtSenha;
        private Label lblSenha;
        private TextBox txtUsuario;
        private Label lblUsuario;
        private Button btnIniciar;
        private Label lblStatus;
        private GroupBox groupBox2;
        private Label label2;
        private TextBox txtPubTopico;
        private Label label3;
        private TextBox txtPubPayload;
        private Button btnPublicar;
        private ComboBox cmbQoS;
        private Label label4;
        private TextBox txtLog;
        private Button btnLimpar;
        private Button btnSalvar;
        private Label lblClientes;
        private Label lblMensagens;
    }
}
