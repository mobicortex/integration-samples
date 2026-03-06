namespace SmartSdk
{
    partial class FormCadastroCentral
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelHeader = new Panel();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            grpId = new GroupBox();
            lblIdInfo = new Label();
            numId = new NumericUpDown();
            lblId = new Label();
            grpNome = new GroupBox();
            txtNome = new TextBox();
            lblNome = new Label();
            grpCamposOpcionais = new GroupBox();
            txtField4 = new TextBox();
            lblField4 = new Label();
            txtField3 = new TextBox();
            lblField3 = new Label();
            txtField2 = new TextBox();
            lblField2 = new Label();
            txtField1 = new TextBox();
            lblField1 = new Label();
            grpStatus = new GroupBox();
            chkBloqueado = new CheckBox();
            panelBotoes = new Panel();
            btnCancelar = new Button();
            btnSalvar = new Button();
            panelHeader.SuspendLayout();
            grpId.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numId).BeginInit();
            grpNome.SuspendLayout();
            grpCamposOpcionais.SuspendLayout();
            grpStatus.SuspendLayout();
            panelBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(244, 246, 248);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(484, 62);
            panelHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.ForeColor = Color.FromArgb(95, 102, 109);
            lblSubtitulo.Location = new Point(20, 33);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(440, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Cadastro Central (Unidade/Apartamento/Empresa)";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            lblTitulo.Location = new Point(20, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(420, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cadastro Central";
            // 
            // grpId
            // 
            grpId.Controls.Add(lblIdInfo);
            grpId.Controls.Add(numId);
            grpId.Controls.Add(lblId);
            grpId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpId.ForeColor = Color.FromArgb(25, 91, 168);
            grpId.Location = new Point(20, 76);
            grpId.Name = "grpId";
            grpId.Size = new Size(444, 90);
            grpId.TabIndex = 1;
            grpId.TabStop = false;
            grpId.Text = "Identificacao";
            // 
            // lblIdInfo
            // 
            lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblIdInfo.ForeColor = Color.Gray;
            lblIdInfo.Location = new Point(20, 60);
            lblIdInfo.Name = "lblIdInfo";
            lblIdInfo.Size = new Size(400, 15);
            lblIdInfo.TabIndex = 2;
            lblIdInfo.Text = "0 = geracao automatica pelo servidor";
            // 
            // numId
            // 
            numId.Location = new Point(80, 30);
            numId.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numId.Name = "numId";
            numId.Size = new Size(140, 23);
            numId.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F);
            lblId.ForeColor = Color.Black;
            lblId.Location = new Point(20, 32);
            lblId.Name = "lblId";
            lblId.Size = new Size(21, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            // 
            // grpNome
            // 
            grpNome.Controls.Add(txtNome);
            grpNome.Controls.Add(lblNome);
            grpNome.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpNome.ForeColor = Color.FromArgb(25, 91, 168);
            grpNome.Location = new Point(20, 176);
            grpNome.Name = "grpNome";
            grpNome.Size = new Size(444, 70);
            grpNome.TabIndex = 2;
            grpNome.TabStop = false;
            grpNome.Text = "Nome";
            // 
            // txtNome
            // 
            txtNome.Font = new Font("Segoe UI", 10F);
            txtNome.Location = new Point(80, 28);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(340, 25);
            txtNome.TabIndex = 1;
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Segoe UI", 9F);
            lblNome.ForeColor = Color.Black;
            lblNome.Location = new Point(20, 32);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(43, 15);
            lblNome.TabIndex = 0;
            lblNome.Text = "Nome:";
            // 
            // grpCamposOpcionais
            // 
            grpCamposOpcionais.Controls.Add(txtField4);
            grpCamposOpcionais.Controls.Add(lblField4);
            grpCamposOpcionais.Controls.Add(txtField3);
            grpCamposOpcionais.Controls.Add(lblField3);
            grpCamposOpcionais.Controls.Add(txtField2);
            grpCamposOpcionais.Controls.Add(lblField2);
            grpCamposOpcionais.Controls.Add(txtField1);
            grpCamposOpcionais.Controls.Add(lblField1);
            grpCamposOpcionais.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCamposOpcionais.ForeColor = Color.FromArgb(25, 91, 168);
            grpCamposOpcionais.Location = new Point(20, 256);
            grpCamposOpcionais.Name = "grpCamposOpcionais";
            grpCamposOpcionais.Size = new Size(444, 140);
            grpCamposOpcionais.TabIndex = 3;
            grpCamposOpcionais.TabStop = false;
            grpCamposOpcionais.Text = "Campos Opcionais";
            // 
            // txtField4
            // 
            txtField4.Font = new Font("Segoe UI", 9F);
            txtField4.Location = new Point(280, 90);
            txtField4.Name = "txtField4";
            txtField4.Size = new Size(140, 23);
            txtField4.TabIndex = 7;
            // 
            // lblField4
            // 
            lblField4.AutoSize = true;
            lblField4.Font = new Font("Segoe UI", 9F);
            lblField4.ForeColor = Color.Black;
            lblField4.Location = new Point(230, 93);
            lblField4.Name = "lblField4";
            lblField4.Size = new Size(44, 15);
            lblField4.TabIndex = 6;
            lblField4.Text = "Campo";
            // 
            // txtField3
            // 
            txtField3.Font = new Font("Segoe UI", 9F);
            txtField3.Location = new Point(80, 90);
            txtField3.Name = "txtField3";
            txtField3.Size = new Size(140, 23);
            txtField3.TabIndex = 5;
            // 
            // lblField3
            // 
            lblField3.AutoSize = true;
            lblField3.Font = new Font("Segoe UI", 9F);
            lblField3.ForeColor = Color.Black;
            lblField3.Location = new Point(20, 93);
            lblField3.Name = "lblField3";
            lblField3.Size = new Size(44, 15);
            lblField3.TabIndex = 4;
            lblField3.Text = "Campo";
            // 
            // txtField2
            // 
            txtField2.Font = new Font("Segoe UI", 9F);
            txtField2.Location = new Point(280, 40);
            txtField2.Name = "txtField2";
            txtField2.Size = new Size(140, 23);
            txtField2.TabIndex = 3;
            // 
            // lblField2
            // 
            lblField2.AutoSize = true;
            lblField2.Font = new Font("Segoe UI", 9F);
            lblField2.ForeColor = Color.Black;
            lblField2.Location = new Point(230, 43);
            lblField2.Name = "lblField2";
            lblField2.Size = new Size(44, 15);
            lblField2.TabIndex = 2;
            lblField2.Text = "Campo";
            // 
            // txtField1
            // 
            txtField1.Font = new Font("Segoe UI", 9F);
            txtField1.Location = new Point(80, 40);
            txtField1.Name = "txtField1";
            txtField1.Size = new Size(140, 23);
            txtField1.TabIndex = 1;
            // 
            // lblField1
            // 
            lblField1.AutoSize = true;
            lblField1.Font = new Font("Segoe UI", 9F);
            lblField1.ForeColor = Color.Black;
            lblField1.Location = new Point(20, 43);
            lblField1.Name = "lblField1";
            lblField1.Size = new Size(44, 15);
            lblField1.TabIndex = 0;
            lblField1.Text = "Campo";
            // 
            // grpStatus
            // 
            grpStatus.Controls.Add(chkBloqueado);
            grpStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpStatus.ForeColor = Color.FromArgb(220, 53, 69);
            grpStatus.Location = new Point(20, 406);
            grpStatus.Name = "grpStatus";
            grpStatus.Size = new Size(444, 60);
            grpStatus.TabIndex = 4;
            grpStatus.TabStop = false;
            grpStatus.Text = "Status";
            // 
            // chkBloqueado
            // 
            chkBloqueado.AutoSize = true;
            chkBloqueado.Font = new Font("Segoe UI", 9F);
            chkBloqueado.ForeColor = Color.Black;
            chkBloqueado.Location = new Point(20, 28);
            chkBloqueado.Name = "chkBloqueado";
            chkBloqueado.Size = new Size(190, 19);
            chkBloqueado.TabIndex = 0;
            chkBloqueado.Text = "Ativar cadastro";
            chkBloqueado.UseVisualStyleBackColor = true;
            // 
            // panelBotoes
            // 
            panelBotoes.BackColor = Color.FromArgb(244, 246, 248);
            panelBotoes.Controls.Add(btnCancelar);
            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.Location = new Point(0, 476);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Size = new Size(484, 60);
            panelBotoes.TabIndex = 5;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(380, 15);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(84, 30);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            btnSalvar.BackColor = Color.FromArgb(25, 135, 84);
            btnSalvar.DialogResult = DialogResult.OK;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(280, 15);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(90, 30);
            btnSalvar.TabIndex = 0;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = false;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // FormCadastroCentral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(484, 536);
            Controls.Add(panelBotoes);
            Controls.Add(grpStatus);
            Controls.Add(grpCamposOpcionais);
            Controls.Add(grpNome);
            Controls.Add(grpId);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Load += FormCadastroCentral_Load;
            Name = "FormCadastroCentral";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cadastro Central";
            panelHeader.ResumeLayout(false);
            grpId.ResumeLayout(false);
            grpId.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numId).EndInit();
            grpNome.ResumeLayout(false);
            grpNome.PerformLayout();
            grpCamposOpcionais.ResumeLayout(false);
            grpCamposOpcionais.PerformLayout();
            grpStatus.ResumeLayout(false);
            grpStatus.PerformLayout();
            panelBotoes.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private GroupBox grpId;
        private Label lblId;
        private NumericUpDown numId;
        private Label lblIdInfo;
        private GroupBox grpNome;
        private Label lblNome;
        private TextBox txtNome;
        private GroupBox grpCamposOpcionais;
        private Label lblField1;
        private TextBox txtField1;
        private Label lblField2;
        private TextBox txtField2;
        private Label lblField3;
        private TextBox txtField3;
        private Label lblField4;
        private TextBox txtField4;
        private GroupBox grpStatus;
        private CheckBox chkBloqueado;
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
    }
}
