namespace SmartSdk
{
    partial class FormCadastroPessoa
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
            this.lblTitulo = new Label();
            this.lblSubtitulo = new Label();
            this.grpIdentificacao = new GroupBox();
            this.lblIdInfo = new Label();
            this.numId = new NumericUpDown();
            this.lblId = new Label();
            this.grpNome = new GroupBox();
            this.txtNome = new TextBox();
            this.lblNome = new Label();
            this.grpDocumento = new GroupBox();
            this.txtDocumento = new TextBox();
            this.lblDocumento = new Label();
            this.grpOpcoes = new GroupBox();
            this.chkLprAtivo = new CheckBox();
            this.chkHabilitado = new CheckBox();
            this.btnSalvar = new Button();
            this.btnCancelar = new Button();
            this.grpIdentificacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numId).BeginInit();
            this.grpNome.SuspendLayout();
            this.grpDocumento.SuspendLayout();
            this.grpOpcoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(180, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Cadastrar Pessoa";
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.ForeColor = Color.Gray;
            this.lblSubtitulo.Location = new Point(14, 34);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new Size(220, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Entidade do tipo Pessoa (tipo = 1)";
            // 
            // grpIdentificacao
            // 
            this.grpIdentificacao.Controls.Add(this.lblIdInfo);
            this.grpIdentificacao.Controls.Add(this.numId);
            this.grpIdentificacao.Controls.Add(this.lblId);
            this.grpIdentificacao.Location = new Point(12, 60);
            this.grpIdentificacao.Name = "grpIdentificacao";
            this.grpIdentificacao.Size = new Size(360, 90);
            this.grpIdentificacao.TabIndex = 2;
            this.grpIdentificacao.TabStop = false;
            this.grpIdentificacao.Text = "Identificação";
            // 
            // lblIdInfo
            // 
            this.lblIdInfo.AutoSize = true;
            this.lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblIdInfo.ForeColor = Color.Gray;
            this.lblIdInfo.Location = new Point(80, 55);
            this.lblIdInfo.Name = "lblIdInfo";
            this.lblIdInfo.Size = new Size(180, 13);
            this.lblIdInfo.TabIndex = 2;
            this.lblIdInfo.Text = "0 = geração automática pelo servidor";
            // 
            // numId
            // 
            this.numId.Location = new Point(80, 25);
            this.numId.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.numId.Name = "numId";
            this.numId.Size = new Size(120, 23);
            this.numId.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new Point(15, 28);
            this.lblId.Name = "lblId";
            this.lblId.Size = new Size(21, 15);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "ID:";
            // 
            // grpNome
            // 
            this.grpNome.Controls.Add(this.txtNome);
            this.grpNome.Controls.Add(this.lblNome);
            this.grpNome.Location = new Point(12, 160);
            this.grpNome.Name = "grpNome";
            this.grpNome.Size = new Size(360, 80);
            this.grpNome.TabIndex = 3;
            this.grpNome.TabStop = false;
            this.grpNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Location = new Point(15, 45);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new Size(330, 23);
            this.txtNome.TabIndex = 1;
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new Point(15, 25);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new Size(95, 15);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Nome completo:";
            // 
            // grpDocumento
            // 
            this.grpDocumento.Controls.Add(this.txtDocumento);
            this.grpDocumento.Controls.Add(this.lblDocumento);
            this.grpDocumento.Location = new Point(12, 250);
            this.grpDocumento.Name = "grpDocumento";
            this.grpDocumento.Size = new Size(360, 80);
            this.grpDocumento.TabIndex = 4;
            this.grpDocumento.TabStop = false;
            this.grpDocumento.Text = "Documento";
            // 
            // txtDocumento
            // 
            this.txtDocumento.Location = new Point(15, 45);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.PlaceholderText = "CPF (opcional)";
            this.txtDocumento.Size = new Size(200, 23);
            this.txtDocumento.TabIndex = 1;
            // 
            // lblDocumento
            // 
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Location = new Point(15, 25);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new Size(73, 15);
            this.lblDocumento.TabIndex = 0;
            this.lblDocumento.Text = "CPF/Doc:";
            // 
            // grpOpcoes
            // 
            this.grpOpcoes.Controls.Add(this.chkLprAtivo);
            this.grpOpcoes.Controls.Add(this.chkHabilitado);
            this.grpOpcoes.Location = new Point(12, 340);
            this.grpOpcoes.Name = "grpOpcoes";
            this.grpOpcoes.Size = new Size(360, 80);
            this.grpOpcoes.TabIndex = 5;
            this.grpOpcoes.TabStop = false;
            this.grpOpcoes.Text = "Opções";
            // 
            // chkLprAtivo
            // 
            this.chkLprAtivo.AutoSize = true;
            this.chkLprAtivo.Location = new Point(15, 50);
            this.chkLprAtivo.Name = "chkLprAtivo";
            this.chkLprAtivo.Size = new Size(194, 19);
            this.chkLprAtivo.TabIndex = 1;
            this.chkLprAtivo.Text = "LPR Ativo (reconhecimento facial)";
            this.chkLprAtivo.UseVisualStyleBackColor = true;
            // 
            // chkHabilitado
            // 
            this.chkHabilitado.AutoSize = true;
            this.chkHabilitado.Checked = true;
            this.chkHabilitado.CheckState = CheckState.Checked;
            this.chkHabilitado.Location = new Point(15, 25);
            this.chkHabilitado.Name = "chkHabilitado";
            this.chkHabilitado.Size = new Size(113, 19);
            this.chkHabilitado.TabIndex = 0;
            this.chkHabilitado.Text = "Entidade ativa";
            this.chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = Color.FromArgb(40, 167, 69);
            this.btnSalvar.FlatStyle = FlatStyle.Flat;
            this.btnSalvar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSalvar.ForeColor = Color.White;
            this.btnSalvar.Location = new Point(200, 430);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(80, 35);
            this.btnSalvar.TabIndex = 6;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += this.btnSalvar_Click;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = Color.FromArgb(108, 117, 125);
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancelar.ForeColor = Color.White;
            this.btnCancelar.Location = new Point(292, 430);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(80, 35);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += this.btnCancelar_Click;
            // 
            // FormCadastroPessoa
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(384, 476);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.grpOpcoes);
            this.Controls.Add(this.grpDocumento);
            this.Controls.Add(this.grpNome);
            this.Controls.Add(this.grpIdentificacao);
            this.Controls.Add(this.lblSubtitulo);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCadastroPessoa";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Cadastrar Pessoa";
            this.Load += this.FormCadastroPessoa_Load;
            this.grpIdentificacao.ResumeLayout(false);
            this.grpIdentificacao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numId).EndInit();
            this.grpNome.ResumeLayout(false);
            this.grpNome.PerformLayout();
            this.grpDocumento.ResumeLayout(false);
            this.grpDocumento.PerformLayout();
            this.grpOpcoes.ResumeLayout(false);
            this.grpOpcoes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitulo;
        private Label lblSubtitulo;
        private GroupBox grpIdentificacao;
        private Label lblIdInfo;
        private NumericUpDown numId;
        private Label lblId;
        private GroupBox grpNome;
        private TextBox txtNome;
        private Label lblNome;
        private GroupBox grpDocumento;
        private TextBox txtDocumento;
        private Label lblDocumento;
        private GroupBox grpOpcoes;
        private CheckBox chkLprAtivo;
        private CheckBox chkHabilitado;
        private Button btnSalvar;
        private Button btnCancelar;
    }
}
