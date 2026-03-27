namespace SmartSdk
{
    partial class FormCadastroEntidade
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
            this.components = new System.ComponentModel.Container();

            // Componentes principais
            this.lblTitulo = new Label();
            this.panelForm = new Panel();

            // Grupo Cadastro Central
            this.grpCadastroCentral = new GroupBox();
            this.lblCadastroIdValor = new Label();
            this.lblCadastroId = new Label();

            // Grupo Tipo de Entidade
            this.grpTipoEntidade = new GroupBox();
            this.cmbTipoEntidade = new ComboBox();
            this.lblTipoEntidade = new Label();

            // Grupo ID da Entidade
            this.grpIdEntidade = new GroupBox();
            this.numIdEntidade = new NumericUpDown();
            this.lblIdEntidade = new Label();
            this.lblIdInfo = new Label();

            // Grupo Nome
            this.grpNome = new GroupBox();
            this.txtNome = new TextBox();
            this.lblNome = new Label();

            // Grupo Documento
            this.grpDocumento = new GroupBox();
            this.txtDocumento = new TextBox();
            this.lblDocumento = new Label();
            this.lblDocInfo = new Label();

            // Grupo LPR
            this.grpLpr = new GroupBox();
            this.chkLprAtivo = new CheckBox();
            this.lblLprInfo = new Label();

            // Painel de botões
            this.panelBotoes = new Panel();
            this.btnSalvar = new Button();
            this.btnCancelar = new Button();

            // ToolTip
            this.toolTip = new ToolTip(this.components);

            // SuspendLayout
            this.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.grpCadastroCentral.SuspendLayout();
            this.grpTipoEntidade.SuspendLayout();
            this.grpIdEntidade.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numIdEntidade).BeginInit();
            this.grpNome.SuspendLayout();
            this.grpDocumento.SuspendLayout();
            this.grpLpr.SuspendLayout();
            this.panelBotoes.SuspendLayout();

            // ====== lblTitulo ======
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblTitulo.BackColor = Color.White;
            this.lblTitulo.Text = "Entity Registration";
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTitulo.Padding = new Padding(15, 10, 10, 10);
            this.lblTitulo.Height = 50;
            this.lblTitulo.Name = "lblTitulo";

            // ====== panelForm ======
            this.panelForm.Dock = DockStyle.Fill;
            this.panelForm.BackColor = Color.White;
            this.panelForm.Padding = new Padding(15);
            this.panelForm.AutoScroll = true;
            this.panelForm.Name = "panelForm";

            // ====================================================================
            // GRUPO: Cadastro Central
            // ====================================================================
            this.grpCadastroCentral.Text = "Central Registry";
            this.grpCadastroCentral.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpCadastroCentral.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpCadastroCentral.Dock = DockStyle.Top;
            this.grpCadastroCentral.Height = 75;
            this.grpCadastroCentral.Margin = new Padding(0, 0, 0, 10);
            this.grpCadastroCentral.Name = "grpCadastroCentral";

            this.lblCadastroId.Text = "Registry ID:";
            this.lblCadastroId.Font = new Font("Segoe UI", 9F);
            this.lblCadastroId.ForeColor = Color.Black;
            this.lblCadastroId.Location = new Point(15, 25);
            this.lblCadastroId.AutoSize = true;
            this.lblCadastroId.Name = "lblCadastroId";

            this.lblCadastroIdValor.Text = "-";
            this.lblCadastroIdValor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCadastroIdValor.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblCadastroIdValor.Location = new Point(120, 23);
            this.lblCadastroIdValor.Size = new Size(200, 20);
            this.lblCadastroIdValor.Name = "lblCadastroIdValor";

            this.grpCadastroCentral.Controls.Add(this.lblCadastroIdValor);
            this.grpCadastroCentral.Controls.Add(this.lblCadastroId);

            // ====================================================================
            // GRUPO: Tipo de Entidade
            // ====================================================================
            this.grpTipoEntidade.Text = "Entity Type";
            this.grpTipoEntidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpTipoEntidade.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpTipoEntidade.Dock = DockStyle.Top;
            this.grpTipoEntidade.Height = 90;
            this.grpTipoEntidade.Margin = new Padding(0, 0, 0, 10);
            this.grpTipoEntidade.Name = "grpTipoEntidade";

            this.lblTipoEntidade.Text = "Select the type:";
            this.lblTipoEntidade.Font = new Font("Segoe UI", 9F);
            this.lblTipoEntidade.ForeColor = Color.Black;
            this.lblTipoEntidade.Location = new Point(15, 25);
            this.lblTipoEntidade.AutoSize = true;
            this.lblTipoEntidade.Name = "lblTipoEntidade";

            this.cmbTipoEntidade.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoEntidade.Font = new Font("Segoe UI", 10F);
            this.cmbTipoEntidade.Location = new Point(15, 45);
            this.cmbTipoEntidade.Size = new Size(380, 25);
            this.cmbTipoEntidade.Name = "cmbTipoEntidade";
            this.cmbTipoEntidade.SelectedIndexChanged += new System.EventHandler(this.cmbTipoEntidade_SelectedIndexChanged);

            this.grpTipoEntidade.Controls.Add(this.cmbTipoEntidade);
            this.grpTipoEntidade.Controls.Add(this.lblTipoEntidade);

            // ====================================================================
            // GRUPO: ID da Entidade
            // ====================================================================
            this.grpIdEntidade.Text = "Entity ID";
            this.grpIdEntidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpIdEntidade.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpIdEntidade.Dock = DockStyle.Top;
            this.grpIdEntidade.Height = 110;
            this.grpIdEntidade.Margin = new Padding(0, 0, 0, 10);
            this.grpIdEntidade.Name = "grpIdEntidade";

            this.lblIdEntidade.Text = "Enter the ID (0 = server generates automatically):";
            this.lblIdEntidade.Font = new Font("Segoe UI", 9F);
            this.lblIdEntidade.ForeColor = Color.Black;
            this.lblIdEntidade.Location = new Point(15, 25);
            this.lblIdEntidade.AutoSize = true;
            this.lblIdEntidade.Name = "lblIdEntidade";

            this.numIdEntidade.Font = new Font("Segoe UI", 10F);
            this.numIdEntidade.Location = new Point(15, 45);
            this.numIdEntidade.Size = new Size(180, 25);
            this.numIdEntidade.Minimum = 0;
            this.numIdEntidade.Maximum = 4294967295; // uint.MaxValue
            this.numIdEntidade.Name = "numIdEntidade";
            this.toolTip.SetToolTip(this.numIdEntidade, "Digite 0 para que o servidor gere o ID automaticamente");

            this.lblIdInfo.Text = "💡 Tip: Enter 0 (zero) for the server to generate the ID automatically";
            this.lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblIdInfo.ForeColor = Color.Gray;
            this.lblIdInfo.Location = new Point(15, 75);
            this.lblIdInfo.AutoSize = true;
            this.lblIdInfo.Name = "lblIdInfo";

            this.grpIdEntidade.Controls.Add(this.lblIdInfo);
            this.grpIdEntidade.Controls.Add(this.numIdEntidade);
            this.grpIdEntidade.Controls.Add(this.lblIdEntidade);

            // ====================================================================
            // GRUPO: Nome
            // ====================================================================
            this.grpNome.Text = "Name";
            this.grpNome.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpNome.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpNome.Dock = DockStyle.Top;
            this.grpNome.Height = 90;
            this.grpNome.Margin = new Padding(0, 0, 0, 10);
            this.grpNome.Name = "grpNome";

            this.lblNome.Text = "Entity name:";
            this.lblNome.Font = new Font("Segoe UI", 9F);
            this.lblNome.ForeColor = Color.Black;
            this.lblNome.Location = new Point(15, 25);
            this.lblNome.AutoSize = true;
            this.lblNome.Name = "lblNome";

            this.txtNome.Font = new Font("Segoe UI", 10F);
            this.txtNome.Location = new Point(15, 45);
            this.txtNome.Size = new Size(380, 25);
            this.txtNome.Name = "txtNome";

            this.grpNome.Controls.Add(this.txtNome);
            this.grpNome.Controls.Add(this.lblNome);

            // ====================================================================
            // GRUPO: Documento
            // ====================================================================
            this.grpDocumento.Text = "Document";
            this.grpDocumento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpDocumento.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpDocumento.Dock = DockStyle.Top;
            this.grpDocumento.Height = 110;
            this.grpDocumento.Margin = new Padding(0, 0, 0, 10);
            this.grpDocumento.Name = "grpDocumento";

            this.lblDocumento.Text = "CPF or Plate:";
            this.lblDocumento.Font = new Font("Segoe UI", 9F);
            this.lblDocumento.ForeColor = Color.Black;
            this.lblDocumento.Location = new Point(15, 25);
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Name = "lblDocumento";

            this.txtDocumento.Font = new Font("Segoe UI", 10F);
            this.txtDocumento.Location = new Point(15, 45);
            this.txtDocumento.Size = new Size(380, 25);
            this.txtDocumento.CharacterCasing = CharacterCasing.Upper;
            this.txtDocumento.Name = "txtDocumento";

            this.lblDocInfo.Text = "For vehicles: enter the plate to enable LPR recognition";
            this.lblDocInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblDocInfo.ForeColor = Color.Gray;
            this.lblDocInfo.Location = new Point(15, 75);
            this.lblDocInfo.AutoSize = true;
            this.lblDocInfo.Name = "lblDocInfo";

            this.grpDocumento.Controls.Add(this.lblDocInfo);
            this.grpDocumento.Controls.Add(this.txtDocumento);
            this.grpDocumento.Controls.Add(this.lblDocumento);

            // ====================================================================
            // GRUPO: LPR
            // ====================================================================
            this.grpLpr.Text = "License Plate Recognition (LPR)";
            this.grpLpr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpLpr.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpLpr.Dock = DockStyle.Top;
            this.grpLpr.Height = 80;
            this.grpLpr.Margin = new Padding(0, 0, 0, 10);
            this.grpLpr.Name = "grpLpr";

            this.chkLprAtivo.Text = "Enable automatic plate recognition (LPR)";
            this.chkLprAtivo.Font = new Font("Segoe UI", 9F);
            this.chkLprAtivo.Location = new Point(15, 25);
            this.chkLprAtivo.AutoSize = true;
            this.chkLprAtivo.Name = "chkLprAtivo";

            this.lblLprInfo.Text = "Automatically creates an LPR media using the document/plate";
            this.lblLprInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblLprInfo.ForeColor = Color.Gray;
            this.lblLprInfo.Location = new Point(33, 45);
            this.lblLprInfo.AutoSize = true;
            this.lblLprInfo.Name = "lblLprInfo";

            this.grpLpr.Controls.Add(this.lblLprInfo);
            this.grpLpr.Controls.Add(this.chkLprAtivo);

            // Adiciona grupos ao panelForm (ordem inversa para dock)
            this.panelForm.Controls.Add(this.grpLpr);
            this.panelForm.Controls.Add(this.grpDocumento);
            this.panelForm.Controls.Add(this.grpNome);
            this.panelForm.Controls.Add(this.grpIdEntidade);
            this.panelForm.Controls.Add(this.grpTipoEntidade);
            this.panelForm.Controls.Add(this.grpCadastroCentral);

            // ====================================================================
            // PAINEL DE BOTÕES
            // ====================================================================
            this.panelBotoes.Dock = DockStyle.Bottom;
            this.panelBotoes.Height = 60;
            this.panelBotoes.BackColor = Color.FromArgb(240, 240, 240);
            this.panelBotoes.Padding = new Padding(10);
            this.panelBotoes.Name = "panelBotoes";

            this.btnCancelar.Text = "Cancel";
            this.btnCancelar.Font = new Font("Segoe UI", 9F);
            this.btnCancelar.Size = new Size(100, 35);
            this.btnCancelar.Location = new Point(310, 12);
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.Name = "btnCancelar";

            this.btnSalvar.Text = "Save";
            this.btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSalvar.Size = new Size(100, 35);
            this.btnSalvar.Location = new Point(200, 12);
            this.btnSalvar.BackColor = Color.FromArgb(0, 120, 60);
            this.btnSalvar.ForeColor = Color.White;
            this.btnSalvar.FlatStyle = FlatStyle.Flat;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.Cursor = Cursors.Hand;
            this.btnSalvar.DialogResult = DialogResult.OK;
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);

            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Controls.Add(this.btnCancelar);

            // ====================================================================
            // FORM
            // ====================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 700);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Entity Registration";

            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.lblTitulo);

            this.Name = "FormCadastroEntidade";
            this.Load += new System.EventHandler(this.FormCadastroEntidade_Load);

            // ResumeLayout
            this.panelBotoes.ResumeLayout(false);
            this.grpLpr.ResumeLayout(false);
            this.grpLpr.PerformLayout();
            this.grpDocumento.ResumeLayout(false);
            this.grpDocumento.PerformLayout();
            this.grpNome.ResumeLayout(false);
            this.grpNome.PerformLayout();
            this.grpIdEntidade.ResumeLayout(false);
            this.grpIdEntidade.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numIdEntidade).EndInit();
            this.grpTipoEntidade.ResumeLayout(false);
            this.grpTipoEntidade.PerformLayout();
            this.grpCadastroCentral.ResumeLayout(false);
            this.grpCadastroCentral.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Componentes
        private Label lblTitulo;
        private Panel panelForm;

        private GroupBox grpCadastroCentral;
        private Label lblCadastroId;
        private Label lblCadastroIdValor;

        private GroupBox grpTipoEntidade;
        private ComboBox cmbTipoEntidade;
        private Label lblTipoEntidade;

        private GroupBox grpIdEntidade;
        private NumericUpDown numIdEntidade;
        private Label lblIdEntidade;
        private Label lblIdInfo;

        private GroupBox grpNome;
        private TextBox txtNome;
        private Label lblNome;

        private GroupBox grpDocumento;
        private TextBox txtDocumento;
        private Label lblDocumento;
        private Label lblDocInfo;

        private GroupBox grpLpr;
        private CheckBox chkLprAtivo;
        private Label lblLprInfo;

        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;

        private ToolTip toolTip;
    }
}
