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
            components = new System.ComponentModel.Container();
            
            // Componentes principais
            lblTitulo = new Label();
            panelForm = new Panel();
            
            // Grupo Cadastro Central
            grpCadastroCentral = new GroupBox();
            lblCadastroIdValor = new Label();
            lblCadastroId = new Label();
            
            // Grupo Tipo de Entidade
            grpTipoEntidade = new GroupBox();
            cmbTipoEntidade = new ComboBox();
            lblTipoEntidade = new Label();
            
            // Grupo ID da Entidade
            grpIdEntidade = new GroupBox();
            numIdEntidade = new NumericUpDown();
            lblIdEntidade = new Label();
            lblIdInfo = new Label();
            
            // Grupo Nome
            grpNome = new GroupBox();
            txtNome = new TextBox();
            lblNome = new Label();
            
            // Grupo Documento
            grpDocumento = new GroupBox();
            txtDocumento = new TextBox();
            lblDocumento = new Label();
            lblDocInfo = new Label();
            
            // Grupo LPR
            grpLpr = new GroupBox();
            chkLprAtivo = new CheckBox();
            lblLprInfo = new Label();
            
            // Painel de botões
            panelBotoes = new Panel();
            btnSalvar = new Button();
            btnCancelar = new Button();
            
            // ToolTip
            toolTip = new ToolTip(components);
            
            // SuspendLayout
            SuspendLayout();
            panelForm.SuspendLayout();
            grpCadastroCentral.SuspendLayout();
            grpTipoEntidade.SuspendLayout();
            grpIdEntidade.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIdEntidade).BeginInit();
            grpNome.SuspendLayout();
            grpDocumento.SuspendLayout();
            grpLpr.SuspendLayout();
            panelBotoes.SuspendLayout();
            
            // ====== lblTitulo ======
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblTitulo.BackColor = Color.White;
            lblTitulo.Text = "Entity Registration";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Padding = new Padding(15, 10, 10, 10);
            lblTitulo.Height = 50;
            lblTitulo.Name = "lblTitulo";
            
            // ====== panelForm ======
            panelForm.Dock = DockStyle.Fill;
            panelForm.BackColor = Color.White;
            panelForm.Padding = new Padding(15);
            panelForm.AutoScroll = true;
            panelForm.Name = "panelForm";
            
            // ====================================================================
            // GRUPO: Cadastro Central
            // ====================================================================
            grpCadastroCentral.Text = "Central Registry";
            grpCadastroCentral.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCadastroCentral.ForeColor = Color.FromArgb(0, 120, 60);
            grpCadastroCentral.Dock = DockStyle.Top;
            grpCadastroCentral.Height = 75;
            grpCadastroCentral.Margin = new Padding(0, 0, 0, 10);
            grpCadastroCentral.Name = "grpCadastroCentral";
            
            lblCadastroId.Text = "Registry ID:";
            lblCadastroId.Font = new Font("Segoe UI", 9F);
            lblCadastroId.ForeColor = Color.Black;
            lblCadastroId.Location = new Point(15, 25);
            lblCadastroId.AutoSize = true;
            lblCadastroId.Name = "lblCadastroId";
            
            lblCadastroIdValor.Text = "-";
            lblCadastroIdValor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCadastroIdValor.ForeColor = Color.FromArgb(0, 120, 60);
            lblCadastroIdValor.Location = new Point(120, 23);
            lblCadastroIdValor.Size = new Size(200, 20);
            lblCadastroIdValor.Name = "lblCadastroIdValor";
            
            grpCadastroCentral.Controls.Add(lblCadastroIdValor);
            grpCadastroCentral.Controls.Add(lblCadastroId);
            
            // ====================================================================
            // GRUPO: Tipo de Entidade
            // ====================================================================
            grpTipoEntidade.Text = "Entity Type";
            grpTipoEntidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpTipoEntidade.ForeColor = Color.FromArgb(0, 120, 60);
            grpTipoEntidade.Dock = DockStyle.Top;
            grpTipoEntidade.Height = 90;
            grpTipoEntidade.Margin = new Padding(0, 0, 0, 10);
            grpTipoEntidade.Name = "grpTipoEntidade";
            
            lblTipoEntidade.Text = "Select the type:";
            lblTipoEntidade.Font = new Font("Segoe UI", 9F);
            lblTipoEntidade.ForeColor = Color.Black;
            lblTipoEntidade.Location = new Point(15, 25);
            lblTipoEntidade.AutoSize = true;
            lblTipoEntidade.Name = "lblTipoEntidade";
            
            cmbTipoEntidade.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoEntidade.Font = new Font("Segoe UI", 10F);
            cmbTipoEntidade.Location = new Point(15, 45);
            cmbTipoEntidade.Size = new Size(380, 25);
            cmbTipoEntidade.Name = "cmbTipoEntidade";
            cmbTipoEntidade.SelectedIndexChanged += cmbTipoEntidade_SelectedIndexChanged;
            
            grpTipoEntidade.Controls.Add(cmbTipoEntidade);
            grpTipoEntidade.Controls.Add(lblTipoEntidade);
            
            // ====================================================================
            // GRUPO: ID da Entidade
            // ====================================================================
            grpIdEntidade.Text = "Entity ID";
            grpIdEntidade.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpIdEntidade.ForeColor = Color.FromArgb(0, 120, 60);
            grpIdEntidade.Dock = DockStyle.Top;
            grpIdEntidade.Height = 110;
            grpIdEntidade.Margin = new Padding(0, 0, 0, 10);
            grpIdEntidade.Name = "grpIdEntidade";
            
            lblIdEntidade.Text = "Enter the ID (0 = server generates automatically):";
            lblIdEntidade.Font = new Font("Segoe UI", 9F);
            lblIdEntidade.ForeColor = Color.Black;
            lblIdEntidade.Location = new Point(15, 25);
            lblIdEntidade.AutoSize = true;
            lblIdEntidade.Name = "lblIdEntidade";
            
            numIdEntidade.Font = new Font("Segoe UI", 10F);
            numIdEntidade.Location = new Point(15, 45);
            numIdEntidade.Size = new Size(180, 25);
            numIdEntidade.Minimum = 0;
            numIdEntidade.Maximum = 4294967295; // uint.MaxValue
            numIdEntidade.Name = "numIdEntidade";
            toolTip.SetToolTip(numIdEntidade, "Digite 0 para que o servidor gere o ID automaticamente");
            
            lblIdInfo.Text = "💡 Tip: Enter 0 (zero) for the server to generate the ID automatically";
            lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblIdInfo.ForeColor = Color.Gray;
            lblIdInfo.Location = new Point(15, 75);
            lblIdInfo.AutoSize = true;
            lblIdInfo.Name = "lblIdInfo";
            
            grpIdEntidade.Controls.Add(lblIdInfo);
            grpIdEntidade.Controls.Add(numIdEntidade);
            grpIdEntidade.Controls.Add(lblIdEntidade);
            
            // ====================================================================
            // GRUPO: Nome
            // ====================================================================
            grpNome.Text = "Name";
            grpNome.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpNome.ForeColor = Color.FromArgb(0, 120, 60);
            grpNome.Dock = DockStyle.Top;
            grpNome.Height = 90;
            grpNome.Margin = new Padding(0, 0, 0, 10);
            grpNome.Name = "grpNome";
            
            lblNome.Text = "Entity name:";
            lblNome.Font = new Font("Segoe UI", 9F);
            lblNome.ForeColor = Color.Black;
            lblNome.Location = new Point(15, 25);
            lblNome.AutoSize = true;
            lblNome.Name = "lblNome";
            
            txtNome.Font = new Font("Segoe UI", 10F);
            txtNome.Location = new Point(15, 45);
            txtNome.Size = new Size(380, 25);
            txtNome.Name = "txtNome";
            
            grpNome.Controls.Add(txtNome);
            grpNome.Controls.Add(lblNome);
            
            // ====================================================================
            // GRUPO: Documento
            // ====================================================================
            grpDocumento.Text = "Document";
            grpDocumento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpDocumento.ForeColor = Color.FromArgb(0, 120, 60);
            grpDocumento.Dock = DockStyle.Top;
            grpDocumento.Height = 110;
            grpDocumento.Margin = new Padding(0, 0, 0, 10);
            grpDocumento.Name = "grpDocumento";
            
            lblDocumento.Text = "CPF or Plate:";
            lblDocumento.Font = new Font("Segoe UI", 9F);
            lblDocumento.ForeColor = Color.Black;
            lblDocumento.Location = new Point(15, 25);
            lblDocumento.AutoSize = true;
            lblDocumento.Name = "lblDocumento";
            
            txtDocumento.Font = new Font("Segoe UI", 10F);
            txtDocumento.Location = new Point(15, 45);
            txtDocumento.Size = new Size(380, 25);
            txtDocumento.CharacterCasing = CharacterCasing.Upper;
            txtDocumento.Name = "txtDocumento";
            
            lblDocInfo.Text = "For vehicles: enter the plate to enable LPR recognition";
            lblDocInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblDocInfo.ForeColor = Color.Gray;
            lblDocInfo.Location = new Point(15, 75);
            lblDocInfo.AutoSize = true;
            lblDocInfo.Name = "lblDocInfo";
            
            grpDocumento.Controls.Add(lblDocInfo);
            grpDocumento.Controls.Add(txtDocumento);
            grpDocumento.Controls.Add(lblDocumento);
            
            // ====================================================================
            // GRUPO: LPR
            // ====================================================================
            grpLpr.Text = "License Plate Recognition (LPR)";
            grpLpr.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpLpr.ForeColor = Color.FromArgb(0, 120, 60);
            grpLpr.Dock = DockStyle.Top;
            grpLpr.Height = 80;
            grpLpr.Margin = new Padding(0, 0, 0, 10);
            grpLpr.Name = "grpLpr";
            
            chkLprAtivo.Text = "Enable automatic plate recognition (LPR)";
            chkLprAtivo.Font = new Font("Segoe UI", 9F);
            chkLprAtivo.Location = new Point(15, 25);
            chkLprAtivo.AutoSize = true;
            chkLprAtivo.Name = "chkLprAtivo";
            
            lblLprInfo.Text = "Automatically creates an LPR media using the document/plate";
            lblLprInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblLprInfo.ForeColor = Color.Gray;
            lblLprInfo.Location = new Point(33, 45);
            lblLprInfo.AutoSize = true;
            lblLprInfo.Name = "lblLprInfo";
            
            grpLpr.Controls.Add(lblLprInfo);
            grpLpr.Controls.Add(chkLprAtivo);
            
            // Adiciona grupos ao panelForm (ordem inversa para dock)
            panelForm.Controls.Add(grpLpr);
            panelForm.Controls.Add(grpDocumento);
            panelForm.Controls.Add(grpNome);
            panelForm.Controls.Add(grpIdEntidade);
            panelForm.Controls.Add(grpTipoEntidade);
            panelForm.Controls.Add(grpCadastroCentral);
            
            // ====================================================================
            // PAINEL DE BOTÕES
            // ====================================================================
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.Height = 60;
            panelBotoes.BackColor = Color.FromArgb(240, 240, 240);
            panelBotoes.Padding = new Padding(10);
            panelBotoes.Name = "panelBotoes";
            
            btnCancelar.Text = "Cancel";
            btnCancelar.Font = new Font("Segoe UI", 9F);
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.Location = new Point(310, 12);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Name = "btnCancelar";
            
            btnSalvar.Text = "Save";
            btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSalvar.Size = new Size(100, 35);
            btnSalvar.Location = new Point(200, 12);
            btnSalvar.BackColor = Color.FromArgb(0, 120, 60);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Cursor = Cursors.Hand;
            btnSalvar.DialogResult = DialogResult.OK;
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Click += btnSalvar_Click;
            
            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Controls.Add(btnCancelar);
            
            // ====================================================================
            // FORM
            // ====================================================================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 700);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cadastro de Entidade";
            
            Controls.Add(panelForm);
            Controls.Add(panelBotoes);
            Controls.Add(lblTitulo);
            
            Name = "FormCadastroEntidade";
            Load += FormCadastroEntidade_Load;
            
            // ResumeLayout
            panelBotoes.ResumeLayout(false);
            grpLpr.ResumeLayout(false);
            grpLpr.PerformLayout();
            grpDocumento.ResumeLayout(false);
            grpDocumento.PerformLayout();
            grpNome.ResumeLayout(false);
            grpNome.PerformLayout();
            grpIdEntidade.ResumeLayout(false);
            grpIdEntidade.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIdEntidade).EndInit();
            grpTipoEntidade.ResumeLayout(false);
            grpTipoEntidade.PerformLayout();
            grpCadastroCentral.ResumeLayout(false);
            grpCadastroCentral.PerformLayout();
            panelForm.ResumeLayout(false);
            ResumeLayout(false);
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
