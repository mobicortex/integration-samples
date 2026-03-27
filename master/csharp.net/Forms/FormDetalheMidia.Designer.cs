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
            this.panelHeader = new Panel();
            this.lblSubtitulo = new Label();
            this.lblTitulo = new Label();
            this.grpInformacoes = new GroupBox();
            this.lblDataEdicaoValor = new Label();
            this.lblDataEdicao = new Label();
            this.lblDataCadastroValor = new Label();
            this.lblDataCadastro = new Label();
            this.lblDescricaoValor = new Label();
            this.lblDescricao = new Label();
            this.lblTipoValor = new Label();
            this.lblTipo = new Label();
            this.lblIdValor = new Label();
            this.lblId = new Label();
            this.grpBloqueio = new GroupBox();
            this.btnLimparData = new Button();
            this.dtpDataBloqueio = new DateTimePicker();
            this.chkBloqueioPorData = new CheckBox();
            this.chkBloqueada = new CheckBox();
            this.panelBotoes = new Panel();
            this.btnCancelar = new Button();
            this.btnSalvar = new Button();
            this.panelHeader.SuspendLayout();
            this.grpInformacoes.SuspendLayout();
            this.grpBloqueio.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.BackColor = Color.FromArgb(244, 246, 248);
            this.panelHeader.Controls.Add(this.lblSubtitulo);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(484, 62);
            this.panelHeader.TabIndex = 0;
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblSubtitulo.Location = new Point(20, 33);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new Size(440, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "View and edit access media information.";
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            this.lblTitulo.Location = new Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(420, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Media Details";
            //
            // grpInformacoes
            //
            this.grpInformacoes.Controls.Add(this.lblDataEdicaoValor);
            this.grpInformacoes.Controls.Add(this.lblDataEdicao);
            this.grpInformacoes.Controls.Add(this.lblDataCadastroValor);
            this.grpInformacoes.Controls.Add(this.lblDataCadastro);
            this.grpInformacoes.Controls.Add(this.lblDescricaoValor);
            this.grpInformacoes.Controls.Add(this.lblDescricao);
            this.grpInformacoes.Controls.Add(this.lblTipoValor);
            this.grpInformacoes.Controls.Add(this.lblTipo);
            this.grpInformacoes.Controls.Add(this.lblIdValor);
            this.grpInformacoes.Controls.Add(this.lblId);
            this.grpInformacoes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpInformacoes.ForeColor = Color.FromArgb(25, 91, 168);
            this.grpInformacoes.Location = new Point(20, 76);
            this.grpInformacoes.Name = "grpInformacoes";
            this.grpInformacoes.Size = new Size(444, 180);
            this.grpInformacoes.TabIndex = 1;
            this.grpInformacoes.TabStop = false;
            this.grpInformacoes.Text = "Information";
            //
            // lblDataEdicaoValor
            //
            this.lblDataEdicaoValor.Font = new Font("Segoe UI", 9F);
            this.lblDataEdicaoValor.ForeColor = Color.Black;
            this.lblDataEdicaoValor.Location = new Point(120, 145);
            this.lblDataEdicaoValor.Name = "lblDataEdicaoValor";
            this.lblDataEdicaoValor.Size = new Size(300, 15);
            this.lblDataEdicaoValor.TabIndex = 9;
            this.lblDataEdicaoValor.Text = "-";
            //
            // lblDataEdicao
            //
            this.lblDataEdicao.AutoSize = true;
            this.lblDataEdicao.Font = new Font("Segoe UI", 9F);
            this.lblDataEdicao.ForeColor = Color.Black;
            this.lblDataEdicao.Location = new Point(20, 145);
            this.lblDataEdicao.Name = "lblDataEdicao";
            this.lblDataEdicao.Size = new Size(71, 15);
            this.lblDataEdicao.TabIndex = 8;
            this.lblDataEdicao.Text = "Edit date:";
            //
            // lblDataCadastroValor
            //
            this.lblDataCadastroValor.Font = new Font("Segoe UI", 9F);
            this.lblDataCadastroValor.ForeColor = Color.Black;
            this.lblDataCadastroValor.Location = new Point(120, 118);
            this.lblDataCadastroValor.Name = "lblDataCadastroValor";
            this.lblDataCadastroValor.Size = new Size(300, 15);
            this.lblDataCadastroValor.TabIndex = 7;
            this.lblDataCadastroValor.Text = "-";
            //
            // lblDataCadastro
            //
            this.lblDataCadastro.AutoSize = true;
            this.lblDataCadastro.Font = new Font("Segoe UI", 9F);
            this.lblDataCadastro.ForeColor = Color.Black;
            this.lblDataCadastro.Location = new Point(20, 118);
            this.lblDataCadastro.Name = "lblDataCadastro";
            this.lblDataCadastro.Size = new Size(89, 15);
            this.lblDataCadastro.TabIndex = 6;
            this.lblDataCadastro.Text = "Registry date:";
            //
            // lblDescricaoValor
            //
            this.lblDescricaoValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblDescricaoValor.ForeColor = Color.Black;
            this.lblDescricaoValor.Location = new Point(120, 91);
            this.lblDescricaoValor.Name = "lblDescricaoValor";
            this.lblDescricaoValor.Size = new Size(300, 15);
            this.lblDescricaoValor.TabIndex = 5;
            this.lblDescricaoValor.Text = "-";
            //
            // lblDescricao
            //
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new Font("Segoe UI", 9F);
            this.lblDescricao.ForeColor = Color.Black;
            this.lblDescricao.Location = new Point(20, 91);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new Size(61, 15);
            this.lblDescricao.TabIndex = 4;
            this.lblDescricao.Text = "Description:";
            //
            // lblTipoValor
            //
            this.lblTipoValor.Font = new Font("Segoe UI", 9F);
            this.lblTipoValor.ForeColor = Color.Black;
            this.lblTipoValor.Location = new Point(120, 64);
            this.lblTipoValor.Name = "lblTipoValor";
            this.lblTipoValor.Size = new Size(300, 15);
            this.lblTipoValor.TabIndex = 3;
            this.lblTipoValor.Text = "-";
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new Font("Segoe UI", 9F);
            this.lblTipo.ForeColor = Color.Black;
            this.lblTipo.Location = new Point(20, 64);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new Size(33, 15);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Type:";
            //
            // lblIdValor
            //
            this.lblIdValor.Font = new Font("Segoe UI", 9F);
            this.lblIdValor.ForeColor = Color.Black;
            this.lblIdValor.Location = new Point(120, 37);
            this.lblIdValor.Name = "lblIdValor";
            this.lblIdValor.Size = new Size(300, 15);
            this.lblIdValor.TabIndex = 1;
            this.lblIdValor.Text = "-";
            //
            // lblId
            //
            this.lblId.AutoSize = true;
            this.lblId.Font = new Font("Segoe UI", 9F);
            this.lblId.ForeColor = Color.Black;
            this.lblId.Location = new Point(20, 37);
            this.lblId.Name = "lblId";
            this.lblId.Size = new Size(21, 15);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "ID:";
            //
            // grpBloqueio
            //
            this.grpBloqueio.Controls.Add(this.btnLimparData);
            this.grpBloqueio.Controls.Add(this.dtpDataBloqueio);
            this.grpBloqueio.Controls.Add(this.chkBloqueioPorData);
            this.grpBloqueio.Controls.Add(this.chkBloqueada);
            this.grpBloqueio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpBloqueio.ForeColor = Color.FromArgb(220, 53, 69);
            this.grpBloqueio.Location = new Point(20, 268);
            this.grpBloqueio.Name = "grpBloqueio";
            this.grpBloqueio.Size = new Size(444, 120);
            this.grpBloqueio.TabIndex = 2;
            this.grpBloqueio.TabStop = false;
            this.grpBloqueio.Text = "Blocking";
            //
            // btnLimparData
            //
            this.btnLimparData.Enabled = false;
            this.btnLimparData.Location = new Point(320, 80);
            this.btnLimparData.Name = "btnLimparData";
            this.btnLimparData.Size = new Size(100, 23);
            this.btnLimparData.TabIndex = 3;
            this.btnLimparData.Text = "Remove Date";
            this.btnLimparData.UseVisualStyleBackColor = true;
            this.btnLimparData.Click += new System.EventHandler(this.btnLimparData_Click);
            //
            // dtpDataBloqueio
            //
            this.dtpDataBloqueio.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpDataBloqueio.Enabled = false;
            this.dtpDataBloqueio.Format = DateTimePickerFormat.Custom;
            this.dtpDataBloqueio.Location = new Point(150, 80);
            this.dtpDataBloqueio.Name = "dtpDataBloqueio";
            this.dtpDataBloqueio.ShowUpDown = true;
            this.dtpDataBloqueio.Size = new Size(160, 23);
            this.dtpDataBloqueio.TabIndex = 2;
            //
            // chkBloqueioPorData
            //
            this.chkBloqueioPorData.AutoSize = true;
            this.chkBloqueioPorData.Font = new Font("Segoe UI", 9F);
            this.chkBloqueioPorData.ForeColor = Color.Black;
            this.chkBloqueioPorData.Location = new Point(20, 82);
            this.chkBloqueioPorData.Name = "chkBloqueioPorData";
            this.chkBloqueioPorData.Size = new Size(95, 19);
            this.chkBloqueioPorData.TabIndex = 1;
            this.chkBloqueioPorData.Text = "Allowed until:";
            this.chkBloqueioPorData.UseVisualStyleBackColor = true;
            this.chkBloqueioPorData.CheckedChanged += new System.EventHandler(this.chkBloqueioPorData_CheckedChanged);
            //
            // chkBloqueada
            //
            this.chkBloqueada.AutoSize = true;
            this.chkBloqueada.Font = new Font("Segoe UI", 9F);
            this.chkBloqueada.ForeColor = Color.Black;
            this.chkBloqueada.Location = new Point(20, 40);
            this.chkBloqueada.Name = "chkBloqueada";
            this.chkBloqueada.Size = new Size(250, 19);
            this.chkBloqueada.TabIndex = 0;
            this.chkBloqueada.Text = "Block media (access fully denied)";
            this.chkBloqueada.UseVisualStyleBackColor = true;
            //
            // panelBotoes
            //
            this.panelBotoes.BackColor = Color.FromArgb(244, 246, 248);
            this.panelBotoes.Controls.Add(this.btnCancelar);
            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Dock = DockStyle.Bottom;
            this.panelBotoes.Location = new Point(0, 402);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new Size(484, 60);
            this.panelBotoes.TabIndex = 3;
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.Location = new Point(380, 15);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(84, 30);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // btnSalvar
            //
            this.btnSalvar.BackColor = Color.FromArgb(25, 135, 84);
            this.btnSalvar.DialogResult = DialogResult.OK;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.FlatStyle = FlatStyle.Flat;
            this.btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSalvar.ForeColor = Color.White;
            this.btnSalvar.Location = new Point(280, 15);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(90, 30);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Save";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            //
            // FormDetalheMidia
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new Size(484, 462);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.grpBloqueio);
            this.Controls.Add(this.grpInformacoes);
            this.Controls.Add(this.panelHeader);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDetalheMidia";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Media Details";
            this.panelHeader.ResumeLayout(false);
            this.grpInformacoes.ResumeLayout(false);
            this.grpInformacoes.PerformLayout();
            this.grpBloqueio.ResumeLayout(false);
            this.grpBloqueio.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
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
