namespace SmartSdk.Forms
{
    partial class FormCadastroMidia
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
            
            // Grupo Tipo de Mídia
            grpTipoMidia = new GroupBox();
            cmbTipoMidia = new ComboBox();
            lblTipoMidia = new Label();
            
            // Grupo ID da Mídia
            grpIdMidia = new GroupBox();
            numIdMidia = new NumericUpDown();
            lblIdMidia = new Label();
            lblIdInfo = new Label();
            
            // Grupo Dados da Mídia
            grpDadosMidia = new GroupBox();
            txtDadosMidia = new TextBox();
            lblDadosMidia = new Label();
            lblExemploFormato = new Label();
            lblFormatoAtual = new Label();
            
            // Painel de botões
            panelBotoes = new Panel();
            btnSalvar = new Button();
            btnCancelar = new Button();
            
            // ToolTip
            toolTip = new ToolTip(components);
            
            // SuspendLayout dos containers
            SuspendLayout();
            panelForm.SuspendLayout();
            grpTipoMidia.SuspendLayout();
            grpIdMidia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIdMidia).BeginInit();
            grpDadosMidia.SuspendLayout();
            panelBotoes.SuspendLayout();
            
            // ====== lblTitulo ======
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblTitulo.BackColor = Color.White;
            lblTitulo.Text = "Cadastro de Mídia";
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
            // GRUPO: Tipo de Mídia
            // ====================================================================
            grpTipoMidia.Text = "Tipo de Mídia";
            grpTipoMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpTipoMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpTipoMidia.Dock = DockStyle.Top;
            grpTipoMidia.Height = 90;
            grpTipoMidia.Margin = new Padding(0, 0, 0, 10);
            grpTipoMidia.Name = "grpTipoMidia";
            
            lblTipoMidia.Text = "Selecione o tipo:";
            lblTipoMidia.Font = new Font("Segoe UI", 9F);
            lblTipoMidia.ForeColor = Color.Black;
            lblTipoMidia.Location = new Point(15, 25);
            lblTipoMidia.AutoSize = true;
            lblTipoMidia.Name = "lblTipoMidia";
            
            cmbTipoMidia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoMidia.Font = new Font("Segoe UI", 10F);
            cmbTipoMidia.Location = new Point(15, 45);
            cmbTipoMidia.Size = new Size(380, 25);
            cmbTipoMidia.Name = "cmbTipoMidia";
            cmbTipoMidia.SelectedIndexChanged += cmbTipoMidia_SelectedIndexChanged;
            
            grpTipoMidia.Controls.Add(cmbTipoMidia);
            grpTipoMidia.Controls.Add(lblTipoMidia);
            
            // ====================================================================
            // GRUPO: ID da Mídia
            // ====================================================================
            grpIdMidia.Text = "ID da Mídia";
            grpIdMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpIdMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpIdMidia.Dock = DockStyle.Top;
            grpIdMidia.Height = 110;
            grpIdMidia.Margin = new Padding(0, 0, 0, 10);
            grpIdMidia.Name = "grpIdMidia";
            
            lblIdMidia.Text = "Informe o ID (0 = servidor gera automaticamente):";
            lblIdMidia.Font = new Font("Segoe UI", 9F);
            lblIdMidia.ForeColor = Color.Black;
            lblIdMidia.Location = new Point(15, 25);
            lblIdMidia.AutoSize = true;
            lblIdMidia.Name = "lblIdMidia";
            
            numIdMidia.Font = new Font("Segoe UI", 10F);
            numIdMidia.Location = new Point(15, 45);
            numIdMidia.Size = new Size(180, 25);
            numIdMidia.Minimum = 0;
            numIdMidia.Maximum = 999999999;
            numIdMidia.Name = "numIdMidia";
            toolTip.SetToolTip(numIdMidia, "Digite 0 para que o servidor gere o ID automaticamente");
            
            lblIdInfo.Text = "💡 Dica: Informe 0 (zero) para que o servidor gere o ID automaticamente";
            lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblIdInfo.ForeColor = Color.Gray;
            lblIdInfo.Location = new Point(15, 75);
            lblIdInfo.AutoSize = true;
            lblIdInfo.Name = "lblIdInfo";
            
            grpIdMidia.Controls.Add(lblIdInfo);
            grpIdMidia.Controls.Add(numIdMidia);
            grpIdMidia.Controls.Add(lblIdMidia);
            
            // ====================================================================
            // GRUPO: Dados da Mídia
            // ====================================================================
            grpDadosMidia.Text = "Dados da Mídia";
            grpDadosMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpDadosMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpDadosMidia.Dock = DockStyle.Top;
            grpDadosMidia.Height = 140;
            grpDadosMidia.Margin = new Padding(0, 0, 0, 10);
            grpDadosMidia.Name = "grpDadosMidia";
            
            lblDadosMidia.Text = "Informe os dados da mídia:";
            lblDadosMidia.Font = new Font("Segoe UI", 9F);
            lblDadosMidia.ForeColor = Color.Black;
            lblDadosMidia.Location = new Point(15, 25);
            lblDadosMidia.AutoSize = true;
            lblDadosMidia.Name = "lblDadosMidia";
            
            txtDadosMidia.Font = new Font("Segoe UI", 11F);
            txtDadosMidia.Location = new Point(15, 45);
            txtDadosMidia.Size = new Size(380, 27);
            txtDadosMidia.CharacterCasing = CharacterCasing.Upper;
            txtDadosMidia.Name = "txtDadosMidia";
            
            lblExemploFormato.Text = "Formato esperado:";
            lblExemploFormato.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblExemploFormato.ForeColor = Color.DimGray;
            lblExemploFormato.Location = new Point(15, 80);
            lblExemploFormato.AutoSize = true;
            lblExemploFormato.Name = "lblExemploFormato";
            
            lblFormatoAtual.Text = "Selecione um tipo de mídia";
            lblFormatoAtual.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblFormatoAtual.ForeColor = Color.FromArgb(0, 120, 60);
            lblFormatoAtual.Location = new Point(15, 98);
            lblFormatoAtual.Size = new Size(380, 20);
            lblFormatoAtual.Name = "lblFormatoAtual";
            
            grpDadosMidia.Controls.Add(lblFormatoAtual);
            grpDadosMidia.Controls.Add(lblExemploFormato);
            grpDadosMidia.Controls.Add(txtDadosMidia);
            grpDadosMidia.Controls.Add(lblDadosMidia);
            
            // Adiciona grupos ao panelForm (ordem inversa para dock)
            panelForm.Controls.Add(grpDadosMidia);
            panelForm.Controls.Add(grpIdMidia);
            panelForm.Controls.Add(grpTipoMidia);
            
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
            ClientSize = new Size(450, 520);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cadastro de Mídia";
            
            Controls.Add(panelForm);
            Controls.Add(panelBotoes);
            Controls.Add(lblTitulo);
            
            Name = "FormCadastroMidia";
            Load += FormCadastroMidia_Load;
            
            // ResumeLayout
            panelBotoes.ResumeLayout(false);
            grpDadosMidia.ResumeLayout(false);
            grpDadosMidia.PerformLayout();
            grpIdMidia.ResumeLayout(false);
            grpIdMidia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIdMidia).EndInit();
            grpTipoMidia.ResumeLayout(false);
            grpTipoMidia.PerformLayout();
            panelForm.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // Componentes do formulário
        private Label lblTitulo;
        private Panel panelForm;
        
        // Grupo Tipo de Mídia
        private GroupBox grpTipoMidia;
        private ComboBox cmbTipoMidia;
        private Label lblTipoMidia;
        
        // Grupo ID da Mídia
        private GroupBox grpIdMidia;
        private NumericUpDown numIdMidia;
        private Label lblIdMidia;
        private Label lblIdInfo;
        
        // Grupo Dados da Mídia
        private GroupBox grpDadosMidia;
        private TextBox txtDadosMidia;
        private Label lblDadosMidia;
        private Label lblExemploFormato;
        private Label lblFormatoAtual;
        
        // Painel de botões
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
        
        // ToolTip
        private ToolTip toolTip;
    }
}
