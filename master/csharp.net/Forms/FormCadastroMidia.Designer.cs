namespace SmartSdk
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
            lblTitulo = new Label();
            panelForm = new Panel();
            grpDadosMidia = new GroupBox();
            lblFormatoAtual = new Label();
            lblExemploFormato = new Label();
            txtDadosMidia = new TextBox();
            lblDadosMidia = new Label();
            grpIdMidia = new GroupBox();
            lblIdInfo = new Label();
            numIdMidia = new NumericUpDown();
            lblIdMidia = new Label();
            grpTipoMidia = new GroupBox();
            cmbTipoMidia = new ComboBox();
            lblTipoMidia = new Label();
            panelBotoes = new Panel();
            btnSalvar = new Button();
            btnCancelar = new Button();
            toolTip = new ToolTip(components);
            panelForm.SuspendLayout();
            grpDadosMidia.SuspendLayout();
            grpIdMidia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numIdMidia).BeginInit();
            grpTipoMidia.SuspendLayout();
            panelBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.White;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(15, 10, 10, 10);
            lblTitulo.Size = new Size(450, 50);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Cadastro de Mídia";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelForm
            // 
            panelForm.AutoScroll = true;
            panelForm.BackColor = Color.White;
            panelForm.Controls.Add(grpDadosMidia);
            panelForm.Controls.Add(grpIdMidia);
            panelForm.Controls.Add(grpTipoMidia);
            panelForm.Dock = DockStyle.Fill;
            panelForm.Location = new Point(0, 50);
            panelForm.Name = "panelForm";
            panelForm.Padding = new Padding(15);
            panelForm.Size = new Size(450, 437);
            panelForm.TabIndex = 0;
            // 
            // grpDadosMidia
            // 
            grpDadosMidia.Controls.Add(lblFormatoAtual);
            grpDadosMidia.Controls.Add(lblExemploFormato);
            grpDadosMidia.Controls.Add(txtDadosMidia);
            grpDadosMidia.Controls.Add(lblDadosMidia);
            grpDadosMidia.Dock = DockStyle.Top;
            grpDadosMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpDadosMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpDadosMidia.Location = new Point(15, 215);
            grpDadosMidia.Margin = new Padding(0, 0, 0, 10);
            grpDadosMidia.Name = "grpDadosMidia";
            grpDadosMidia.Size = new Size(420, 209);
            grpDadosMidia.TabIndex = 0;
            grpDadosMidia.TabStop = false;
            grpDadosMidia.Text = "Dados da Mídia";
            // 
            // lblFormatoAtual
            // 
            lblFormatoAtual.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblFormatoAtual.ForeColor = Color.FromArgb(0, 120, 60);
            lblFormatoAtual.Location = new Point(15, 98);
            lblFormatoAtual.Name = "lblFormatoAtual";
            lblFormatoAtual.Size = new Size(380, 20);
            lblFormatoAtual.TabIndex = 0;
            lblFormatoAtual.Text = "Selecione um tipo de mídia";
            // 
            // lblExemploFormato
            // 
            lblExemploFormato.AutoSize = true;
            lblExemploFormato.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblExemploFormato.ForeColor = Color.DimGray;
            lblExemploFormato.Location = new Point(15, 80);
            lblExemploFormato.Name = "lblExemploFormato";
            lblExemploFormato.Size = new Size(105, 13);
            lblExemploFormato.TabIndex = 1;
            lblExemploFormato.Text = "Formato esperado:";
            // 
            // txtDadosMidia
            // 
            txtDadosMidia.CharacterCasing = CharacterCasing.Upper;
            txtDadosMidia.Font = new Font("Segoe UI", 11F);
            txtDadosMidia.Location = new Point(15, 45);
            txtDadosMidia.Name = "txtDadosMidia";
            txtDadosMidia.Size = new Size(380, 27);
            txtDadosMidia.TabIndex = 2;
            // 
            // lblDadosMidia
            // 
            lblDadosMidia.AutoSize = true;
            lblDadosMidia.Font = new Font("Segoe UI", 9F);
            lblDadosMidia.ForeColor = Color.Black;
            lblDadosMidia.Location = new Point(15, 25);
            lblDadosMidia.Name = "lblDadosMidia";
            lblDadosMidia.Size = new Size(151, 15);
            lblDadosMidia.TabIndex = 3;
            lblDadosMidia.Text = "Informe os dados da mídia:";
            // 
            // grpIdMidia
            // 
            grpIdMidia.Controls.Add(lblIdInfo);
            grpIdMidia.Controls.Add(numIdMidia);
            grpIdMidia.Controls.Add(lblIdMidia);
            grpIdMidia.Dock = DockStyle.Top;
            grpIdMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpIdMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpIdMidia.Location = new Point(15, 105);
            grpIdMidia.Margin = new Padding(0, 0, 0, 10);
            grpIdMidia.Name = "grpIdMidia";
            grpIdMidia.Size = new Size(420, 110);
            grpIdMidia.TabIndex = 1;
            grpIdMidia.TabStop = false;
            grpIdMidia.Text = "ID da Mídia";
            // 
            // lblIdInfo
            // 
            lblIdInfo.AutoSize = true;
            lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblIdInfo.ForeColor = Color.Gray;
            lblIdInfo.Location = new Point(15, 75);
            lblIdInfo.Name = "lblIdInfo";
            lblIdInfo.Size = new Size(347, 13);
            lblIdInfo.TabIndex = 0;
            lblIdInfo.Text = "💡 Dica: Informe 0 (zero) para que o servidor gere o ID automaticamente";
            // 
            // numIdMidia
            // 
            numIdMidia.Font = new Font("Segoe UI", 10F);
            numIdMidia.Location = new Point(15, 45);
            numIdMidia.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numIdMidia.Name = "numIdMidia";
            numIdMidia.Size = new Size(180, 25);
            numIdMidia.TabIndex = 1;
            toolTip.SetToolTip(numIdMidia, "Digite 0 para que o servidor gere o ID automaticamente");
            // 
            // lblIdMidia
            // 
            lblIdMidia.AutoSize = true;
            lblIdMidia.Font = new Font("Segoe UI", 9F);
            lblIdMidia.ForeColor = Color.Black;
            lblIdMidia.Location = new Point(15, 25);
            lblIdMidia.Name = "lblIdMidia";
            lblIdMidia.Size = new Size(272, 15);
            lblIdMidia.TabIndex = 2;
            lblIdMidia.Text = "Informe o ID (0 = servidor gera automaticamente):";
            // 
            // grpTipoMidia
            // 
            grpTipoMidia.Controls.Add(cmbTipoMidia);
            grpTipoMidia.Controls.Add(lblTipoMidia);
            grpTipoMidia.Dock = DockStyle.Top;
            grpTipoMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpTipoMidia.ForeColor = Color.FromArgb(0, 120, 60);
            grpTipoMidia.Location = new Point(15, 15);
            grpTipoMidia.Margin = new Padding(0, 0, 0, 10);
            grpTipoMidia.Name = "grpTipoMidia";
            grpTipoMidia.Size = new Size(420, 90);
            grpTipoMidia.TabIndex = 2;
            grpTipoMidia.TabStop = false;
            grpTipoMidia.Text = "Tipo de Mídia";
            // 
            // cmbTipoMidia
            // 
            cmbTipoMidia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoMidia.Font = new Font("Segoe UI", 10F);
            cmbTipoMidia.Location = new Point(15, 45);
            cmbTipoMidia.Name = "cmbTipoMidia";
            cmbTipoMidia.Size = new Size(380, 25);
            cmbTipoMidia.TabIndex = 0;
            cmbTipoMidia.SelectedIndexChanged += cmbTipoMidia_SelectedIndexChanged;
            // 
            // lblTipoMidia
            // 
            lblTipoMidia.AutoSize = true;
            lblTipoMidia.Font = new Font("Segoe UI", 9F);
            lblTipoMidia.ForeColor = Color.Black;
            lblTipoMidia.Location = new Point(15, 25);
            lblTipoMidia.Name = "lblTipoMidia";
            lblTipoMidia.Size = new Size(94, 15);
            lblTipoMidia.TabIndex = 1;
            lblTipoMidia.Text = "Selecione o tipo:";
            // 
            // panelBotoes
            // 
            panelBotoes.BackColor = Color.FromArgb(240, 240, 240);
            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Controls.Add(btnCancelar);
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.Location = new Point(0, 487);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Padding = new Padding(10);
            panelBotoes.Size = new Size(450, 60);
            panelBotoes.TabIndex = 1;
            // 
            // btnSalvar
            // 
            btnSalvar.BackColor = Color.FromArgb(0, 120, 60);
            btnSalvar.Cursor = Cursors.Hand;
            btnSalvar.DialogResult = DialogResult.OK;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(200, 12);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(100, 35);
            btnSalvar.TabIndex = 0;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = false;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9F);
            btnCancelar.Location = new Point(310, 12);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            // 
            // FormCadastroMidia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 547);
            Controls.Add(panelForm);
            Controls.Add(panelBotoes);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCadastroMidia";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cadastro de Mídia";
            Load += FormCadastroMidia_Load;
            panelForm.ResumeLayout(false);
            grpDadosMidia.ResumeLayout(false);
            grpDadosMidia.PerformLayout();
            grpIdMidia.ResumeLayout(false);
            grpIdMidia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numIdMidia).EndInit();
            grpTipoMidia.ResumeLayout(false);
            grpTipoMidia.PerformLayout();
            panelBotoes.ResumeLayout(false);
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
