namespace SmartSdk.Forms
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
            
            // Componentes principais
            lblTitulo = new Label();
            panelForm = new Panel();
            
            // Grupo ID do Cadastro
            grpIdCadastro = new GroupBox();
            numIdCadastro = new NumericUpDown();
            lblIdCadastro = new Label();
            lblIdInfo = new Label();
            
            // Grupo Nome
            grpNome = new GroupBox();
            txtNome = new TextBox();
            lblNome = new Label();
            
            // Grupo Tipo
            grpTipo = new GroupBox();
            numTipo = new NumericUpDown();
            lblTipo = new Label();
            lblTipoInfo = new Label();
            
            // Grupo Vagas
            grpVagas = new GroupBox();
            numVagas = new NumericUpDown();
            lblVagas = new Label();
            
            // Painel de botões
            panelBotoes = new Panel();
            btnSalvar = new Button();
            btnCancelar = new Button();
            
            // ToolTip
            toolTip = new ToolTip(components);
            
            // SuspendLayout
            SuspendLayout();
            panelForm.SuspendLayout();
            grpIdCadastro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIdCadastro).BeginInit();
            grpNome.SuspendLayout();
            grpTipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTipo).BeginInit();
            grpVagas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numVagas).BeginInit();
            panelBotoes.SuspendLayout();
            
            // ====== lblTitulo ======
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblTitulo.BackColor = Color.White;
            lblTitulo.Text = "Cadastro Central";
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
            // GRUPO: ID do Cadastro
            // ====================================================================
            grpIdCadastro.Text = "ID do Cadastro";
            grpIdCadastro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpIdCadastro.ForeColor = Color.FromArgb(0, 120, 60);
            grpIdCadastro.Dock = DockStyle.Top;
            grpIdCadastro.Height = 110;
            grpIdCadastro.Margin = new Padding(0, 0, 0, 10);
            grpIdCadastro.Name = "grpIdCadastro";
            
            lblIdCadastro.Text = "Informe o ID (0 = servidor gera automaticamente):";
            lblIdCadastro.Font = new Font("Segoe UI", 9F);
            lblIdCadastro.ForeColor = Color.Black;
            lblIdCadastro.Location = new Point(15, 25);
            lblIdCadastro.AutoSize = true;
            lblIdCadastro.Name = "lblIdCadastro";
            
            numIdCadastro.Font = new Font("Segoe UI", 10F);
            numIdCadastro.Location = new Point(15, 45);
            numIdCadastro.Size = new Size(180, 25);
            numIdCadastro.Minimum = 0;
            numIdCadastro.Maximum = 999999999;
            numIdCadastro.Name = "numIdCadastro";
            toolTip.SetToolTip(numIdCadastro, "Digite 0 para que o servidor gere o ID automaticamente");
            
            lblIdInfo.Text = "💡 Dica: Informe 0 (zero) para que o servidor gere o ID automaticamente";
            lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblIdInfo.ForeColor = Color.Gray;
            lblIdInfo.Location = new Point(15, 75);
            lblIdInfo.AutoSize = true;
            lblIdInfo.Name = "lblIdInfo";
            
            grpIdCadastro.Controls.Add(lblIdInfo);
            grpIdCadastro.Controls.Add(numIdCadastro);
            grpIdCadastro.Controls.Add(lblIdCadastro);
            
            // ====================================================================
            // GRUPO: Nome
            // ====================================================================
            grpNome.Text = "Nome";
            grpNome.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpNome.ForeColor = Color.FromArgb(0, 120, 60);
            grpNome.Dock = DockStyle.Top;
            grpNome.Height = 90;
            grpNome.Margin = new Padding(0, 0, 0, 10);
            grpNome.Name = "grpNome";
            
            lblNome.Text = "Nome do cadastro:";
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
            // GRUPO: Tipo
            // ====================================================================
            grpTipo.Text = "Tipo";
            grpTipo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpTipo.ForeColor = Color.FromArgb(0, 120, 60);
            grpTipo.Dock = DockStyle.Top;
            grpTipo.Height = 110;
            grpTipo.Margin = new Padding(0, 0, 0, 10);
            grpTipo.Name = "grpTipo";
            
            lblTipo.Text = "Tipo do cadastro (valor numérico):";
            lblTipo.Font = new Font("Segoe UI", 9F);
            lblTipo.ForeColor = Color.Black;
            lblTipo.Location = new Point(15, 25);
            lblTipo.AutoSize = true;
            lblTipo.Name = "lblTipo";
            
            numTipo.Font = new Font("Segoe UI", 10F);
            numTipo.Location = new Point(15, 45);
            numTipo.Size = new Size(120, 25);
            numTipo.Minimum = 0;
            numTipo.Maximum = 999;
            numTipo.Name = "numTipo";
            
            lblTipoInfo.Text = "Uso livre pelo integrador (ex: 1=Apartamento, 2=Empresa)";
            lblTipoInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblTipoInfo.ForeColor = Color.Gray;
            lblTipoInfo.Location = new Point(15, 75);
            lblTipoInfo.AutoSize = true;
            lblTipoInfo.Name = "lblTipoInfo";
            
            grpTipo.Controls.Add(lblTipoInfo);
            grpTipo.Controls.Add(numTipo);
            grpTipo.Controls.Add(lblTipo);
            
            // ====================================================================
            // GRUPO: Vagas
            // ====================================================================
            grpVagas.Text = "Vagas";
            grpVagas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpVagas.ForeColor = Color.FromArgb(0, 120, 60);
            grpVagas.Dock = DockStyle.Top;
            grpVagas.Height = 90;
            grpVagas.Margin = new Padding(0, 0, 0, 10);
            grpVagas.Name = "grpVagas";
            
            lblVagas.Text = "Quantidade de vagas (para estacionamento):";
            lblVagas.Font = new Font("Segoe UI", 9F);
            lblVagas.ForeColor = Color.Black;
            lblVagas.Location = new Point(15, 25);
            lblVagas.AutoSize = true;
            lblVagas.Name = "lblVagas";
            
            numVagas.Font = new Font("Segoe UI", 10F);
            numVagas.Location = new Point(15, 45);
            numVagas.Size = new Size(120, 25);
            numVagas.Minimum = 0;
            numVagas.Maximum = 999;
            numVagas.Name = "numVagas";
            
            grpVagas.Controls.Add(numVagas);
            grpVagas.Controls.Add(lblVagas);
            
            // Adiciona grupos ao panelForm (ordem inversa para dock)
            panelForm.Controls.Add(grpVagas);
            panelForm.Controls.Add(grpTipo);
            panelForm.Controls.Add(grpNome);
            panelForm.Controls.Add(grpIdCadastro);
            
            // ====================================================================
            // PAINEL DE BOTÕES
            // ====================================================================
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.Height = 60;
            panelBotoes.BackColor = Color.FromArgb(240, 240, 240);
            panelBotoes.Padding = new Padding(10);
            panelBotoes.Name = "panelBotoes";
            
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 9F);
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.Location = new Point(310, 12);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Name = "btnCancelar";
            
            btnSalvar.Text = "Salvar";
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
            ClientSize = new Size(450, 540);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cadastro Central";
            
            Controls.Add(panelForm);
            Controls.Add(panelBotoes);
            Controls.Add(lblTitulo);
            
            Name = "FormCadastroCentral";
            Load += FormCadastroCentral_Load;
            
            // ResumeLayout
            panelBotoes.ResumeLayout(false);
            grpVagas.ResumeLayout(false);
            grpVagas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numVagas).EndInit();
            grpTipo.ResumeLayout(false);
            grpTipo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTipo).EndInit();
            grpNome.ResumeLayout(false);
            grpNome.PerformLayout();
            grpIdCadastro.ResumeLayout(false);
            grpIdCadastro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIdCadastro).EndInit();
            panelForm.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // Componentes
        private Label lblTitulo;
        private Panel panelForm;
        
        private GroupBox grpIdCadastro;
        private NumericUpDown numIdCadastro;
        private Label lblIdCadastro;
        private Label lblIdInfo;
        
        private GroupBox grpNome;
        private TextBox txtNome;
        private Label lblNome;
        
        private GroupBox grpTipo;
        private NumericUpDown numTipo;
        private Label lblTipo;
        private Label lblTipoInfo;
        
        private GroupBox grpVagas;
        private NumericUpDown numVagas;
        private Label lblVagas;
        
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
        
        private ToolTip toolTip;
    }
}
