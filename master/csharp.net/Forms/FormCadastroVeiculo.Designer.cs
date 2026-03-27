namespace SmartSdk
{
    partial class FormCadastroVeiculo
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
            this.panelHeader = new Panel();
            this.lblSubtitulo = new Label();
            this.lblTitulo = new Label();
            this.lblIdentificacao = new Label();
            this.panelIdentificacao = new Panel();
            this.lblIdInfo = new Label();
            this._numId = new NumericUpDown();
            this.lblId = new Label();
            this.lblCadastro = new Label();
            this.lblDados = new Label();
            this.panelDados = new Panel();
            this._txtPlaca = new TextBox();
            this.lblPlaca = new Label();
            this._cmbCor = new ComboBox();
            this.lblCor = new Label();
            this._txtModelo = new TextBox();
            this.lblModelo = new Label();
            this._txtMarca = new ComboBox();
            this.lblMarca = new Label();
            this.panelLpr = new Panel();
            this.lblLprInfo = new Label();
            this._chkLpr = new CheckBox();
            this.panelStatus = new Panel();
            this.lblStatusInfo = new Label();
            this._chkHabilitado = new CheckBox();
            this.panelBotoes = new Panel();
            this.btnCancelar = new Button();
            this.btnSalvar = new Button();
            this.panelHeader.SuspendLayout();
            this.panelIdentificacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numId)).BeginInit();
            this.panelDados.SuspendLayout();
            this.panelLpr.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.panelHeader.BackColor = Color.FromArgb(244, 246, 248);
            this.panelHeader.Controls.Add(this.lblSubtitulo);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(624, 56);
            this.panelHeader.TabIndex = 0;
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblSubtitulo.Location = new Point(20, 31);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new Size(520, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Fill in the data below to register the vehicle in the selected registry.";
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            this.lblTitulo.Location = new Point(20, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(420, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "New Vehicle";
            //
            // lblIdentificacao
            //
            this.lblIdentificacao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblIdentificacao.ForeColor = Color.FromArgb(25, 91, 168);
            this.lblIdentificacao.Location = new Point(20, 70);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new Size(180, 18);
            this.lblIdentificacao.TabIndex = 1;
            this.lblIdentificacao.Text = "Identification";
            //
            // panelIdentificacao
            //
            this.panelIdentificacao.BackColor = Color.White;
            this.panelIdentificacao.BorderStyle = BorderStyle.FixedSingle;
            this.panelIdentificacao.Controls.Add(this.lblIdInfo);
            this.panelIdentificacao.Controls.Add(this._numId);
            this.panelIdentificacao.Controls.Add(this.lblId);
            this.panelIdentificacao.Controls.Add(this.lblCadastro);
            this.panelIdentificacao.Location = new Point(20, 92);
            this.panelIdentificacao.Name = "panelIdentificacao";
            this.panelIdentificacao.Size = new Size(590, 82);
            this.panelIdentificacao.TabIndex = 2;
            //
            // lblIdInfo
            //
            this.lblIdInfo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblIdInfo.Location = new Point(202, 56);
            this.lblIdInfo.Name = "lblIdInfo";
            this.lblIdInfo.Size = new Size(360, 15);
            this.lblIdInfo.TabIndex = 3;
            this.lblIdInfo.Text = "0 = automatic generation by the controller";
            //
            // _numId
            //
            this._numId.Location = new Point(52, 53);
            this._numId.Maximum = new decimal(new int[] { -1, 0, 0, 0 }); // uint.MaxValue = 4294967295
            this._numId.Name = "_numId";
            this._numId.Size = new Size(140, 23);
            this._numId.TabIndex = 2;
            //
            // lblId
            //
            this.lblId.Location = new Point(16, 56);
            this.lblId.Name = "lblId";
            this.lblId.Size = new Size(30, 15);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "ID:";
            //
            // lblCadastro
            //
            this.lblCadastro.ForeColor = Color.Black;
            this.lblCadastro.Location = new Point(16, 28);
            this.lblCadastro.Name = "lblCadastro";
            this.lblCadastro.Size = new Size(250, 15);
            this.lblCadastro.TabIndex = 0;
            this.lblCadastro.Text = "Selected registry: 0";
            //
            // lblDados
            //
            this.lblDados.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblDados.ForeColor = Color.FromArgb(25, 91, 168);
            this.lblDados.Location = new Point(20, 184);
            this.lblDados.Name = "lblDados";
            this.lblDados.Size = new Size(180, 18);
            this.lblDados.TabIndex = 3;
            this.lblDados.Text = "Vehicle Data";
            //
            // panelDados
            //
            this.panelDados.BackColor = Color.White;
            this.panelDados.BorderStyle = BorderStyle.FixedSingle;
            this.panelDados.Controls.Add(this._txtPlaca);
            this.panelDados.Controls.Add(this.lblPlaca);
            this.panelDados.Controls.Add(this._cmbCor);
            this.panelDados.Controls.Add(this.lblCor);
            this.panelDados.Controls.Add(this._txtModelo);
            this.panelDados.Controls.Add(this.lblModelo);
            this.panelDados.Controls.Add(this._txtMarca);
            this.panelDados.Controls.Add(this.lblMarca);
            this.panelDados.Location = new Point(20, 206);
            this.panelDados.Name = "panelDados";
            this.panelDados.Size = new Size(590, 186);
            this.panelDados.TabIndex = 4;
            //
            // _txtPlaca
            //
            this._txtPlaca.Location = new Point(16, 151);
            this._txtPlaca.Name = "_txtPlaca";
            this._txtPlaca.Size = new Size(270, 23);
            this._txtPlaca.TabIndex = 9;
            //
            // lblPlaca
            //
            this.lblPlaca.Location = new Point(16, 132);
            this.lblPlaca.Name = "lblPlaca";
            this.lblPlaca.Size = new Size(140, 15);
            this.lblPlaca.TabIndex = 8;
            this.lblPlaca.Text = "License Plate";
            //
            // _cmbCor
            //
            this._cmbCor.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cmbCor.FormattingEnabled = true;
            this._cmbCor.Items.AddRange(new object[] { "Select...", "White", "Black", "Silver", "Gray", "Blue", "Red", "Green", "Yellow", "Brown", "Other" });
            this._cmbCor.Location = new Point(302, 49);
            this._cmbCor.Name = "_cmbCor";
            this._cmbCor.Size = new Size(270, 23);
            this._cmbCor.TabIndex = 3;
            //
            // lblCor
            //
            this.lblCor.Location = new Point(302, 30);
            this.lblCor.Name = "lblCor";
            this.lblCor.Size = new Size(140, 15);
            this.lblCor.TabIndex = 2;
            this.lblCor.Text = "Color";
            //
            // _txtModelo
            //
            this._txtModelo.Location = new Point(16, 105);
            this._txtModelo.Name = "_txtModelo";
            this._txtModelo.Size = new Size(270, 23);
            this._txtModelo.TabIndex = 5;
            //
            // lblModelo
            //
            this.lblModelo.Location = new Point(16, 86);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new Size(140, 15);
            this.lblModelo.TabIndex = 4;
            this.lblModelo.Text = "Model";
            //
            // _txtMarca
            //
            this._txtMarca.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this._txtMarca.AutoCompleteSource = AutoCompleteSource.ListItems;
            this._txtMarca.DropDownStyle = ComboBoxStyle.DropDown;
            this._txtMarca.FormattingEnabled = true;
            this._txtMarca.Location = new Point(16, 49);
            this._txtMarca.Name = "_txtMarca";
            this._txtMarca.Size = new Size(270, 23);
            this._txtMarca.TabIndex = 1;
            //
            // lblMarca
            //
            this.lblMarca.Location = new Point(16, 30);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new Size(140, 15);
            this.lblMarca.TabIndex = 0;
            this.lblMarca.Text = "Brand";
            //
            // panelLpr
            //
            this.panelLpr.BackColor = Color.FromArgb(248, 250, 252);
            this.panelLpr.BorderStyle = BorderStyle.FixedSingle;
            this.panelLpr.Controls.Add(this.lblLprInfo);
            this.panelLpr.Controls.Add(this._chkLpr);
            this.panelLpr.Location = new Point(20, 398);
            this.panelLpr.Name = "panelLpr";
            this.panelLpr.Size = new Size(590, 44);
            this.panelLpr.TabIndex = 5;
            //
            // lblLprInfo
            //
            this.lblLprInfo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblLprInfo.Location = new Point(236, 13);
            this.lblLprInfo.Name = "lblLprInfo";
            this.lblLprInfo.Size = new Size(330, 15);
            this.lblLprInfo.TabIndex = 1;
            this.lblLprInfo.Text = "Identify vehicle automatically";
            //
            // _chkLpr
            //
            this._chkLpr.AutoSize = true;
            this._chkLpr.Checked = true;
            this._chkLpr.CheckState = CheckState.Checked;
            this._chkLpr.Location = new Point(12, 11);
            this._chkLpr.Name = "_chkLpr";
            this._chkLpr.Size = new Size(162, 19);
            this._chkLpr.TabIndex = 0;
            this._chkLpr.Text = "License Plate Reading (LPR)";
            this._chkLpr.UseVisualStyleBackColor = true;
            //
            // panelStatus
            //
            this.panelStatus.BackColor = Color.FromArgb(248, 250, 252);
            this.panelStatus.BorderStyle = BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.lblStatusInfo);
            this.panelStatus.Controls.Add(this._chkHabilitado);
            this.panelStatus.Location = new Point(20, 448);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new Size(590, 44);
            this.panelStatus.TabIndex = 6;
            //
            // lblStatusInfo
            //
            this.lblStatusInfo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblStatusInfo.Location = new Point(236, 13);
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new Size(330, 15);
            this.lblStatusInfo.TabIndex = 1;
            this.lblStatusInfo.Text = "Uncheck to disable access for this vehicle";
            //
            // _chkHabilitado
            //
            this._chkHabilitado.AutoSize = true;
            this._chkHabilitado.Checked = true;
            this._chkHabilitado.CheckState = CheckState.Checked;
            this._chkHabilitado.Location = new Point(12, 11);
            this._chkHabilitado.Name = "_chkHabilitado";
            this._chkHabilitado.Size = new Size(91, 19);
            this._chkHabilitado.TabIndex = 0;
            this._chkHabilitado.Text = "Active vehicle";
            this._chkHabilitado.UseVisualStyleBackColor = true;
            //
            // panelBotoes
            //
            this.panelBotoes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelBotoes.BackColor = Color.FromArgb(244, 246, 248);
            this.panelBotoes.Controls.Add(this.btnCancelar);
            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Location = new Point(0, 498);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new Size(624, 42);
            this.panelBotoes.TabIndex = 7;
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.Location = new Point(530, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new Size(82, 26);
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
            this.btnSalvar.ForeColor = Color.White;
            this.btnSalvar.Location = new Point(446, 8);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new Size(78, 26);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Save";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            //
            // FormCadastroVeiculo
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new Size(624, 540);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelLpr);
            this.Controls.Add(this.panelDados);
            this.Controls.Add(this.lblDados);
            this.Controls.Add(this.panelIdentificacao);
            this.Controls.Add(this.lblIdentificacao);
            this.Controls.Add(this.panelHeader);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += new System.EventHandler(this.FormCadastroVeiculo_Load);
            this.Name = "FormCadastroVeiculo";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Register Vehicle";
            this.panelHeader.ResumeLayout(false);
            this.panelIdentificacao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._numId)).EndInit();
            this.panelDados.ResumeLayout(false);
            this.panelDados.PerformLayout();
            this.panelLpr.ResumeLayout(false);
            this.panelLpr.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblSubtitulo;
        private Label lblTitulo;
        private Label lblIdentificacao;
        private Panel panelIdentificacao;
        private Label lblCadastro;
        private Label lblId;
        private NumericUpDown _numId;
        private Label lblIdInfo;
        private Label lblDados;
        private Panel panelDados;
        private Label lblMarca;
        private ComboBox _txtMarca;
        private Label lblModelo;
        private TextBox _txtModelo;
        private Label lblCor;
        private ComboBox _cmbCor;
        private Label lblPlaca;
        private TextBox _txtPlaca;
        private Panel panelLpr;
        private CheckBox _chkLpr;
        private Label lblLprInfo;
        private Panel panelStatus;
        private Label lblStatusInfo;
        private CheckBox _chkHabilitado;
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
    }
}
