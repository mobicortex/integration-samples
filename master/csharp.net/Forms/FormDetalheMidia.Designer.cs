namespace SmartSdk
{
    partial class FormDetalheMidia
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
            panelHeader = new Panel();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            grpInformacoes = new GroupBox();
            lblDataEdicaoValor = new Label();
            lblDataEdicao = new Label();
            lblDataCadastroValor = new Label();
            lblDataCadastro = new Label();
            lblDescricaoValor = new Label();
            lblDescricao = new Label();
            lblTipoValor = new Label();
            lblTipo = new Label();
            lblIdValor = new Label();
            lblId = new Label();
            grpBloqueio = new GroupBox();
            btnLimparData = new Button();
            dtpDataBloqueio = new DateTimePicker();
            chkBloqueioPorData = new CheckBox();
            chkBloqueada = new CheckBox();
            panelBotoes = new Panel();
            btnCancelar = new Button();
            btnSalvar = new Button();
            panelHeader.SuspendLayout();
            grpInformacoes.SuspendLayout();
            grpBloqueio.SuspendLayout();
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
            lblSubtitulo.Text = "Visualize e edite as informacoes da midia de acesso.";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            lblTitulo.Location = new Point(20, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(420, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Detalhes da Midia";
            // 
            // grpInformacoes
            // 
            grpInformacoes.Controls.Add(lblDataEdicaoValor);
            grpInformacoes.Controls.Add(lblDataEdicao);
            grpInformacoes.Controls.Add(lblDataCadastroValor);
            grpInformacoes.Controls.Add(lblDataCadastro);
            grpInformacoes.Controls.Add(lblDescricaoValor);
            grpInformacoes.Controls.Add(lblDescricao);
            grpInformacoes.Controls.Add(lblTipoValor);
            grpInformacoes.Controls.Add(lblTipo);
            grpInformacoes.Controls.Add(lblIdValor);
            grpInformacoes.Controls.Add(lblId);
            grpInformacoes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpInformacoes.ForeColor = Color.FromArgb(25, 91, 168);
            grpInformacoes.Location = new Point(20, 76);
            grpInformacoes.Name = "grpInformacoes";
            grpInformacoes.Size = new Size(444, 180);
            grpInformacoes.TabIndex = 1;
            grpInformacoes.TabStop = false;
            grpInformacoes.Text = "Informacoes";
            // 
            // lblDataEdicaoValor
            // 
            lblDataEdicaoValor.Font = new Font("Segoe UI", 9F);
            lblDataEdicaoValor.ForeColor = Color.Black;
            lblDataEdicaoValor.Location = new Point(120, 145);
            lblDataEdicaoValor.Name = "lblDataEdicaoValor";
            lblDataEdicaoValor.Size = new Size(300, 15);
            lblDataEdicaoValor.TabIndex = 9;
            lblDataEdicaoValor.Text = "-";
            // 
            // lblDataEdicao
            // 
            lblDataEdicao.AutoSize = true;
            lblDataEdicao.Font = new Font("Segoe UI", 9F);
            lblDataEdicao.ForeColor = Color.Black;
            lblDataEdicao.Location = new Point(20, 145);
            lblDataEdicao.Name = "lblDataEdicao";
            lblDataEdicao.Size = new Size(71, 15);
            lblDataEdicao.TabIndex = 8;
            lblDataEdicao.Text = "Data edicao:";
            // 
            // lblDataCadastroValor
            // 
            lblDataCadastroValor.Font = new Font("Segoe UI", 9F);
            lblDataCadastroValor.ForeColor = Color.Black;
            lblDataCadastroValor.Location = new Point(120, 118);
            lblDataCadastroValor.Name = "lblDataCadastroValor";
            lblDataCadastroValor.Size = new Size(300, 15);
            lblDataCadastroValor.TabIndex = 7;
            lblDataCadastroValor.Text = "-";
            // 
            // lblDataCadastro
            // 
            lblDataCadastro.AutoSize = true;
            lblDataCadastro.Font = new Font("Segoe UI", 9F);
            lblDataCadastro.ForeColor = Color.Black;
            lblDataCadastro.Location = new Point(20, 118);
            lblDataCadastro.Name = "lblDataCadastro";
            lblDataCadastro.Size = new Size(89, 15);
            lblDataCadastro.TabIndex = 6;
            lblDataCadastro.Text = "Data cadastro:";
            // 
            // lblDescricaoValor
            // 
            lblDescricaoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDescricaoValor.ForeColor = Color.Black;
            lblDescricaoValor.Location = new Point(120, 91);
            lblDescricaoValor.Name = "lblDescricaoValor";
            lblDescricaoValor.Size = new Size(300, 15);
            lblDescricaoValor.TabIndex = 5;
            lblDescricaoValor.Text = "-";
            // 
            // lblDescricao
            // 
            lblDescricao.AutoSize = true;
            lblDescricao.Font = new Font("Segoe UI", 9F);
            lblDescricao.ForeColor = Color.Black;
            lblDescricao.Location = new Point(20, 91);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(61, 15);
            lblDescricao.TabIndex = 4;
            lblDescricao.Text = "Descricao:";
            // 
            // lblTipoValor
            // 
            lblTipoValor.Font = new Font("Segoe UI", 9F);
            lblTipoValor.ForeColor = Color.Black;
            lblTipoValor.Location = new Point(120, 64);
            lblTipoValor.Name = "lblTipoValor";
            lblTipoValor.Size = new Size(300, 15);
            lblTipoValor.TabIndex = 3;
            lblTipoValor.Text = "-";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Font = new Font("Segoe UI", 9F);
            lblTipo.ForeColor = Color.Black;
            lblTipo.Location = new Point(20, 64);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(33, 15);
            lblTipo.TabIndex = 2;
            lblTipo.Text = "Tipo:";
            // 
            // lblIdValor
            // 
            lblIdValor.Font = new Font("Segoe UI", 9F);
            lblIdValor.ForeColor = Color.Black;
            lblIdValor.Location = new Point(120, 37);
            lblIdValor.Name = "lblIdValor";
            lblIdValor.Size = new Size(300, 15);
            lblIdValor.TabIndex = 1;
            lblIdValor.Text = "-";
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F);
            lblId.ForeColor = Color.Black;
            lblId.Location = new Point(20, 37);
            lblId.Name = "lblId";
            lblId.Size = new Size(21, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            // 
            // grpBloqueio
            // 
            grpBloqueio.Controls.Add(btnLimparData);
            grpBloqueio.Controls.Add(dtpDataBloqueio);
            grpBloqueio.Controls.Add(chkBloqueioPorData);
            grpBloqueio.Controls.Add(chkBloqueada);
            grpBloqueio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpBloqueio.ForeColor = Color.FromArgb(220, 53, 69);
            grpBloqueio.Location = new Point(20, 268);
            grpBloqueio.Name = "grpBloqueio";
            grpBloqueio.Size = new Size(444, 120);
            grpBloqueio.TabIndex = 2;
            grpBloqueio.TabStop = false;
            grpBloqueio.Text = "Bloqueio";
            // 
            // btnLimparData
            // 
            btnLimparData.Enabled = false;
            btnLimparData.Location = new Point(320, 80);
            btnLimparData.Name = "btnLimparData";
            btnLimparData.Size = new Size(100, 23);
            btnLimparData.TabIndex = 3;
            btnLimparData.Text = "Remover Data";
            btnLimparData.UseVisualStyleBackColor = true;
            btnLimparData.Click += btnLimparData_Click;
            // 
            // dtpDataBloqueio
            // 
            dtpDataBloqueio.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDataBloqueio.Enabled = false;
            dtpDataBloqueio.Format = DateTimePickerFormat.Custom;
            dtpDataBloqueio.Location = new Point(150, 80);
            dtpDataBloqueio.Name = "dtpDataBloqueio";
            dtpDataBloqueio.ShowUpDown = true;
            dtpDataBloqueio.Size = new Size(160, 23);
            dtpDataBloqueio.TabIndex = 2;
            // 
            // chkBloqueioPorData
            // 
            chkBloqueioPorData.AutoSize = true;
            chkBloqueioPorData.Font = new Font("Segoe UI", 9F);
            chkBloqueioPorData.ForeColor = Color.Black;
            chkBloqueioPorData.Location = new Point(20, 82);
            chkBloqueioPorData.Name = "chkBloqueioPorData";
            chkBloqueioPorData.Size = new Size(95, 19);
            chkBloqueioPorData.TabIndex = 1;
            chkBloqueioPorData.Text = "Permitido ate:";
            chkBloqueioPorData.UseVisualStyleBackColor = true;
            chkBloqueioPorData.CheckedChanged += chkBloqueioPorData_CheckedChanged;
            // 
            // chkBloqueada
            // 
            chkBloqueada.AutoSize = true;
            chkBloqueada.Font = new Font("Segoe UI", 9F);
            chkBloqueada.ForeColor = Color.Black;
            chkBloqueada.Location = new Point(20, 40);
            chkBloqueada.Name = "chkBloqueada";
            chkBloqueada.Size = new Size(250, 19);
            chkBloqueada.TabIndex = 0;
            chkBloqueada.Text = "Bloquear midia (acesso totalmente negado)";
            chkBloqueada.UseVisualStyleBackColor = true;
            // 
            // panelBotoes
            // 
            panelBotoes.BackColor = Color.FromArgb(244, 246, 248);
            panelBotoes.Controls.Add(btnCancelar);
            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Dock = DockStyle.Bottom;
            panelBotoes.Location = new Point(0, 402);
            panelBotoes.Name = "panelBotoes";
            panelBotoes.Size = new Size(484, 60);
            panelBotoes.TabIndex = 3;
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
            // FormDetalheMidia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(484, 462);
            Controls.Add(panelBotoes);
            Controls.Add(grpBloqueio);
            Controls.Add(grpInformacoes);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDetalheMidia";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalhes da Midia";
            panelHeader.ResumeLayout(false);
            grpInformacoes.ResumeLayout(false);
            grpInformacoes.PerformLayout();
            grpBloqueio.ResumeLayout(false);
            grpBloqueio.PerformLayout();
            panelBotoes.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private GroupBox grpInformacoes;
        private Label lblId;
        private Label lblIdValor;
        private Label lblTipo;
        private Label lblTipoValor;
        private Label lblDescricao;
        private Label lblDescricaoValor;
        private Label lblDataCadastro;
        private Label lblDataCadastroValor;
        private Label lblDataEdicao;
        private Label lblDataEdicaoValor;
        private GroupBox grpBloqueio;
        private CheckBox chkBloqueada;
        private CheckBox chkBloqueioPorData;
        private DateTimePicker dtpDataBloqueio;
        private Button btnLimparData;
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
    }
}
