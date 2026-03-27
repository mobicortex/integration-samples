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
            this.components = new System.ComponentModel.Container();
            this.lblTitulo = new Label();
            this.panelForm = new Panel();
            this.grpDadosMidia = new GroupBox();
            this.lblFormatoAtual = new Label();
            this.lblExemploFormato = new Label();
            this.txtDadosMidia = new TextBox();
            this.lblDadosMidia = new Label();
            this.grpIdMidia = new GroupBox();
            this.lblIdInfo = new Label();
            this.numIdMidia = new NumericUpDown();
            this.lblIdMidia = new Label();
            this.grpTipoMidia = new GroupBox();
            this.cmbTipoMidia = new ComboBox();
            this.lblTipoMidia = new Label();
            this.panelBotoes = new Panel();
            this.btnSalvar = new Button();
            this.btnCancelar = new Button();
            this.toolTip = new ToolTip(this.components);
            this.panelForm.SuspendLayout();
            this.grpDadosMidia.SuspendLayout();
            this.grpIdMidia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numIdMidia).BeginInit();
            this.grpTipoMidia.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.BackColor = Color.White;
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblTitulo.Location = new Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new Padding(15, 10, 10, 10);
            this.lblTitulo.Size = new Size(450, 50);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "Media Registration";
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // panelForm
            //
            this.panelForm.AutoScroll = true;
            this.panelForm.BackColor = Color.White;
            this.panelForm.Controls.Add(this.grpDadosMidia);
            this.panelForm.Controls.Add(this.grpIdMidia);
            this.panelForm.Controls.Add(this.grpTipoMidia);
            this.panelForm.Dock = DockStyle.Fill;
            this.panelForm.Location = new Point(0, 50);
            this.panelForm.Name = "panelForm";
            this.panelForm.Padding = new Padding(15);
            this.panelForm.Size = new Size(450, 437);
            this.panelForm.TabIndex = 0;
            //
            // grpDadosMidia
            //
            this.grpDadosMidia.Controls.Add(this.lblFormatoAtual);
            this.grpDadosMidia.Controls.Add(this.lblExemploFormato);
            this.grpDadosMidia.Controls.Add(this.txtDadosMidia);
            this.grpDadosMidia.Controls.Add(this.lblDadosMidia);
            this.grpDadosMidia.Dock = DockStyle.Top;
            this.grpDadosMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpDadosMidia.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpDadosMidia.Location = new Point(15, 215);
            this.grpDadosMidia.Margin = new Padding(0, 0, 0, 10);
            this.grpDadosMidia.Name = "grpDadosMidia";
            this.grpDadosMidia.Size = new Size(420, 209);
            this.grpDadosMidia.TabIndex = 0;
            this.grpDadosMidia.TabStop = false;
            this.grpDadosMidia.Text = "Media Data";
            //
            // lblFormatoAtual
            //
            this.lblFormatoAtual.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.lblFormatoAtual.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblFormatoAtual.Location = new Point(15, 98);
            this.lblFormatoAtual.Name = "lblFormatoAtual";
            this.lblFormatoAtual.Size = new Size(380, 20);
            this.lblFormatoAtual.TabIndex = 0;
            this.lblFormatoAtual.Text = "Select a media type";
            //
            // lblExemploFormato
            //
            this.lblExemploFormato.AutoSize = true;
            this.lblExemploFormato.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblExemploFormato.ForeColor = Color.DimGray;
            this.lblExemploFormato.Location = new Point(15, 80);
            this.lblExemploFormato.Name = "lblExemploFormato";
            this.lblExemploFormato.Size = new Size(105, 13);
            this.lblExemploFormato.TabIndex = 1;
            this.lblExemploFormato.Text = "Expected format:";
            //
            // txtDadosMidia
            //
            this.txtDadosMidia.CharacterCasing = CharacterCasing.Upper;
            this.txtDadosMidia.Font = new Font("Segoe UI", 11F);
            this.txtDadosMidia.Location = new Point(15, 45);
            this.txtDadosMidia.Name = "txtDadosMidia";
            this.txtDadosMidia.Size = new Size(380, 27);
            this.txtDadosMidia.TabIndex = 2;
            //
            // lblDadosMidia
            //
            this.lblDadosMidia.AutoSize = true;
            this.lblDadosMidia.Font = new Font("Segoe UI", 9F);
            this.lblDadosMidia.ForeColor = Color.Black;
            this.lblDadosMidia.Location = new Point(15, 25);
            this.lblDadosMidia.Name = "lblDadosMidia";
            this.lblDadosMidia.Size = new Size(151, 15);
            this.lblDadosMidia.TabIndex = 3;
            this.lblDadosMidia.Text = "Enter media data:";
            //
            // grpIdMidia
            //
            this.grpIdMidia.Controls.Add(this.lblIdInfo);
            this.grpIdMidia.Controls.Add(this.numIdMidia);
            this.grpIdMidia.Controls.Add(this.lblIdMidia);
            this.grpIdMidia.Dock = DockStyle.Top;
            this.grpIdMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpIdMidia.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpIdMidia.Location = new Point(15, 105);
            this.grpIdMidia.Margin = new Padding(0, 0, 0, 10);
            this.grpIdMidia.Name = "grpIdMidia";
            this.grpIdMidia.Size = new Size(420, 110);
            this.grpIdMidia.TabIndex = 1;
            this.grpIdMidia.TabStop = false;
            this.grpIdMidia.Text = "Media ID";
            //
            // lblIdInfo
            //
            this.lblIdInfo.AutoSize = true;
            this.lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblIdInfo.ForeColor = Color.Gray;
            this.lblIdInfo.Location = new Point(15, 75);
            this.lblIdInfo.Name = "lblIdInfo";
            this.lblIdInfo.Size = new Size(347, 13);
            this.lblIdInfo.TabIndex = 0;
            this.lblIdInfo.Text = "💡 Tip: Enter 0 (zero) for the server to generate the ID automatically";
            //
            // numIdMidia
            //
            this.numIdMidia.Font = new Font("Segoe UI", 10F);
            this.numIdMidia.Location = new Point(15, 45);
            this.numIdMidia.Maximum = new decimal(new int[] { -1, 0, 0, 0 }); // uint.MaxValue = 4294967295
            this.numIdMidia.Name = "numIdMidia";
            this.numIdMidia.Size = new Size(180, 25);
            this.numIdMidia.TabIndex = 1;
            this.toolTip.SetToolTip(this.numIdMidia, "Enter 0 for the server to generate the ID automatically");
            //
            // lblIdMidia
            //
            this.lblIdMidia.AutoSize = true;
            this.lblIdMidia.Font = new Font("Segoe UI", 9F);
            this.lblIdMidia.ForeColor = Color.Black;
            this.lblIdMidia.Location = new Point(15, 25);
            this.lblIdMidia.Name = "lblIdMidia";
            this.lblIdMidia.Size = new Size(272, 15);
            this.lblIdMidia.TabIndex = 2;
            this.lblIdMidia.Text = "Enter the ID (0 = server generates automatically):";
            //
            // grpTipoMidia
            //
            this.grpTipoMidia.Controls.Add(this.cmbTipoMidia);
            this.grpTipoMidia.Controls.Add(this.lblTipoMidia);
            this.grpTipoMidia.Dock = DockStyle.Top;
            this.grpTipoMidia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpTipoMidia.ForeColor = Color.FromArgb(0, 120, 60);
            this.grpTipoMidia.Location = new Point(15, 15);
            this.grpTipoMidia.Margin = new Padding(0, 0, 0, 10);
            this.grpTipoMidia.Name = "grpTipoMidia";
            this.grpTipoMidia.Size = new Size(420, 90);
            this.grpTipoMidia.TabIndex = 2;
            this.grpTipoMidia.TabStop = false;
            this.grpTipoMidia.Text = "Media Type";
            //
            // cmbTipoMidia
            //
            this.cmbTipoMidia.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoMidia.Font = new Font("Segoe UI", 10F);
            this.cmbTipoMidia.Location = new Point(15, 45);
            this.cmbTipoMidia.Name = "cmbTipoMidia";
            this.cmbTipoMidia.Size = new Size(380, 25);
            this.cmbTipoMidia.TabIndex = 0;
            this.cmbTipoMidia.SelectedIndexChanged += new System.EventHandler(this.cmbTipoMidia_SelectedIndexChanged);
            //
            // lblTipoMidia
            //
            this.lblTipoMidia.AutoSize = true;
            this.lblTipoMidia.Font = new Font("Segoe UI", 9F);
            this.lblTipoMidia.ForeColor = Color.Black;
            this.lblTipoMidia.Location = new Point(15, 25);
            this.lblTipoMidia.Name = "lblTipoMidia";
            this.lblTipoMidia.Size = new Size(94, 15);
            this.lblTipoMidia.TabIndex = 1;
            this.lblTipoMidia.Text = "Select the type:";
            //
            // panelBotoes
            //
            this.panelBotoes.BackColor = Color.FromArgb(240, 240, 240);
            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Controls.Add(this.btnCancelar);
            this.panelBotoes.Dock = DockStyle.Bottom;
            this.panelBotoes.Location = new Point(0, 487);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Padding = new Padding(10);
            this.panelBotoes.Size = new Size(450, 60);
            this.panelBotoes.TabIndex = 1;
            //
            // btnSalvar
            //
            this.btnSalvar.BackColor = Color.FromArgb(0, 120, 60);
            this.btnSalvar.Cursor = Cursors.Hand;
            this.btnSalvar.DialogResult = DialogResult.OK;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.FlatStyle = FlatStyle.Flat;
            this.btnSalvar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnSalvar.ForeColor = Color.White;
            this.btnSalvar.Location = new Point(200, 12);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(100, 35);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Save";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.Font = new Font("Segoe UI", 9F);
            this.btnCancelar.Location = new Point(310, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(100, 35);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancel";
            //
            // FormCadastroMidia
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 547);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCadastroMidia";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Media Registration";
            this.Load += new System.EventHandler(this.FormCadastroMidia_Load);
            this.panelForm.ResumeLayout(false);
            this.grpDadosMidia.ResumeLayout(false);
            this.grpDadosMidia.PerformLayout();
            this.grpIdMidia.ResumeLayout(false);
            this.grpIdMidia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numIdMidia).EndInit();
            this.grpTipoMidia.ResumeLayout(false);
            this.grpTipoMidia.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
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
