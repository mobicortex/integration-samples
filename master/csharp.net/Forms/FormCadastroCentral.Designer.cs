namespace SmartSdk
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
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new Panel();
            this.lblSubtitulo = new Label();
            this.lblTitulo = new Label();
            this.grpId = new GroupBox();
            this.lblIdInfo = new Label();
            this.numId = new NumericUpDown();
            this.lblId = new Label();
            this.grpNome = new GroupBox();
            this.txtNome = new TextBox();
            this.lblNome = new Label();
            this.grpCamposOpcionais = new GroupBox();
            this.txtField4 = new TextBox();
            this.lblField4 = new Label();
            this.txtField3 = new TextBox();
            this.lblField3 = new Label();
            this.txtField2 = new TextBox();
            this.lblField2 = new Label();
            this.txtField1 = new TextBox();
            this.lblField1 = new Label();
            this.grpStatus = new GroupBox();
            this.chkBloqueado = new CheckBox();
            this.panelBotoes = new Panel();
            this.btnCancelar = new Button();
            this.btnSalvar = new Button();
            this.panelHeader.SuspendLayout();
            this.grpId.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.numId).BeginInit();
            this.grpNome.SuspendLayout();
            this.grpCamposOpcionais.SuspendLayout();
            this.grpStatus.SuspendLayout();
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
            this.lblSubtitulo.Text = "Central Registry (Unit/Apartment/Company)";
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            this.lblTitulo.Location = new Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(420, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Central Registry";
            //
            // grpId
            //
            this.grpId.Controls.Add(this.lblIdInfo);
            this.grpId.Controls.Add(this.numId);
            this.grpId.Controls.Add(this.lblId);
            this.grpId.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpId.ForeColor = Color.FromArgb(25, 91, 168);
            this.grpId.Location = new Point(20, 76);
            this.grpId.Name = "grpId";
            this.grpId.Size = new Size(444, 90);
            this.grpId.TabIndex = 1;
            this.grpId.TabStop = false;
            this.grpId.Text = "Identification";
            //
            // lblIdInfo
            //
            this.lblIdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            this.lblIdInfo.ForeColor = Color.Gray;
            this.lblIdInfo.Location = new Point(20, 60);
            this.lblIdInfo.Name = "lblIdInfo";
            this.lblIdInfo.Size = new Size(400, 15);
            this.lblIdInfo.TabIndex = 2;
            this.lblIdInfo.Text = "0 = automatic generation by the server";
            //
            // numId
            //
            this.numId.Location = new Point(80, 30);
            this.numId.Maximum = new decimal(new int[] { -1, 0, 0, 0 }); // uint.MaxValue = 4294967295
            this.numId.Name = "numId";
            this.numId.Size = new Size(140, 23);
            this.numId.TabIndex = 1;
            //
            // lblId
            //
            this.lblId.AutoSize = true;
            this.lblId.Font = new Font("Segoe UI", 9F);
            this.lblId.ForeColor = Color.Black;
            this.lblId.Location = new Point(20, 32);
            this.lblId.Name = "lblId";
            this.lblId.Size = new Size(21, 15);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "ID:";
            //
            // grpNome
            //
            this.grpNome.Controls.Add(this.txtNome);
            this.grpNome.Controls.Add(this.lblNome);
            this.grpNome.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpNome.ForeColor = Color.FromArgb(25, 91, 168);
            this.grpNome.Location = new Point(20, 176);
            this.grpNome.Name = "grpNome";
            this.grpNome.Size = new Size(444, 70);
            this.grpNome.TabIndex = 2;
            this.grpNome.TabStop = false;
            this.grpNome.Text = "Name";
            //
            // txtNome
            //
            this.txtNome.Font = new Font("Segoe UI", 10F);
            this.txtNome.Location = new Point(80, 28);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new Size(340, 25);
            this.txtNome.TabIndex = 1;
            //
            // lblNome
            //
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new Font("Segoe UI", 9F);
            this.lblNome.ForeColor = Color.Black;
            this.lblNome.Location = new Point(20, 32);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new Size(43, 15);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Name:";
            //
            // grpCamposOpcionais
            //
            this.grpCamposOpcionais.Controls.Add(this.txtField4);
            this.grpCamposOpcionais.Controls.Add(this.lblField4);
            this.grpCamposOpcionais.Controls.Add(this.txtField3);
            this.grpCamposOpcionais.Controls.Add(this.lblField3);
            this.grpCamposOpcionais.Controls.Add(this.txtField2);
            this.grpCamposOpcionais.Controls.Add(this.lblField2);
            this.grpCamposOpcionais.Controls.Add(this.txtField1);
            this.grpCamposOpcionais.Controls.Add(this.lblField1);
            this.grpCamposOpcionais.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpCamposOpcionais.ForeColor = Color.FromArgb(25, 91, 168);
            this.grpCamposOpcionais.Location = new Point(20, 256);
            this.grpCamposOpcionais.Name = "grpCamposOpcionais";
            this.grpCamposOpcionais.Size = new Size(444, 140);
            this.grpCamposOpcionais.TabIndex = 3;
            this.grpCamposOpcionais.TabStop = false;
            this.grpCamposOpcionais.Text = "Optional Fields";
            //
            // txtField4
            //
            this.txtField4.Font = new Font("Segoe UI", 9F);
            this.txtField4.Location = new Point(280, 90);
            this.txtField4.Name = "txtField4";
            this.txtField4.Size = new Size(140, 23);
            this.txtField4.TabIndex = 7;
            //
            // lblField4
            //
            this.lblField4.AutoSize = true;
            this.lblField4.Font = new Font("Segoe UI", 9F);
            this.lblField4.ForeColor = Color.Black;
            this.lblField4.Location = new Point(230, 93);
            this.lblField4.Name = "lblField4";
            this.lblField4.Size = new Size(44, 15);
            this.lblField4.TabIndex = 6;
            this.lblField4.Text = "Field";
            //
            // txtField3
            //
            this.txtField3.Font = new Font("Segoe UI", 9F);
            this.txtField3.Location = new Point(80, 90);
            this.txtField3.Name = "txtField3";
            this.txtField3.Size = new Size(140, 23);
            this.txtField3.TabIndex = 5;
            //
            // lblField3
            //
            this.lblField3.AutoSize = true;
            this.lblField3.Font = new Font("Segoe UI", 9F);
            this.lblField3.ForeColor = Color.Black;
            this.lblField3.Location = new Point(20, 93);
            this.lblField3.Name = "lblField3";
            this.lblField3.Size = new Size(44, 15);
            this.lblField3.TabIndex = 4;
            this.lblField3.Text = "Field";
            //
            // txtField2
            //
            this.txtField2.Font = new Font("Segoe UI", 9F);
            this.txtField2.Location = new Point(280, 40);
            this.txtField2.Name = "txtField2";
            this.txtField2.Size = new Size(140, 23);
            this.txtField2.TabIndex = 3;
            //
            // lblField2
            //
            this.lblField2.AutoSize = true;
            this.lblField2.Font = new Font("Segoe UI", 9F);
            this.lblField2.ForeColor = Color.Black;
            this.lblField2.Location = new Point(230, 43);
            this.lblField2.Name = "lblField2";
            this.lblField2.Size = new Size(44, 15);
            this.lblField2.TabIndex = 2;
            this.lblField2.Text = "Field";
            //
            // txtField1
            //
            this.txtField1.Font = new Font("Segoe UI", 9F);
            this.txtField1.Location = new Point(80, 40);
            this.txtField1.Name = "txtField1";
            this.txtField1.Size = new Size(140, 23);
            this.txtField1.TabIndex = 1;
            //
            // lblField1
            //
            this.lblField1.AutoSize = true;
            this.lblField1.Font = new Font("Segoe UI", 9F);
            this.lblField1.ForeColor = Color.Black;
            this.lblField1.Location = new Point(20, 43);
            this.lblField1.Name = "lblField1";
            this.lblField1.Size = new Size(44, 15);
            this.lblField1.TabIndex = 0;
            this.lblField1.Text = "Field";
            //
            // grpStatus
            //
            this.grpStatus.Controls.Add(this.chkBloqueado);
            this.grpStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.grpStatus.ForeColor = Color.FromArgb(220, 53, 69);
            this.grpStatus.Location = new Point(20, 406);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new Size(444, 60);
            this.grpStatus.TabIndex = 4;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "Status";
            //
            // chkBloqueado
            //
            this.chkBloqueado.AutoSize = true;
            this.chkBloqueado.Font = new Font("Segoe UI", 9F);
            this.chkBloqueado.ForeColor = Color.Black;
            this.chkBloqueado.Location = new Point(20, 28);
            this.chkBloqueado.Name = "chkBloqueado";
            this.chkBloqueado.Size = new Size(190, 19);
            this.chkBloqueado.TabIndex = 0;
            this.chkBloqueado.Text = "Enable registry";
            this.chkBloqueado.UseVisualStyleBackColor = true;
            //
            // panelBotoes
            //
            this.panelBotoes.BackColor = Color.FromArgb(244, 246, 248);
            this.panelBotoes.Controls.Add(this.btnCancelar);
            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Dock = DockStyle.Bottom;
            this.panelBotoes.Location = new Point(0, 476);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new Size(484, 60);
            this.panelBotoes.TabIndex = 5;
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
            // FormCadastroCentral
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new Size(484, 536);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.grpCamposOpcionais);
            this.Controls.Add(this.grpNome);
            this.Controls.Add(this.grpId);
            this.Controls.Add(this.panelHeader);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += new System.EventHandler(this.FormCadastroCentral_Load);
            this.Name = "FormCadastroCentral";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Central Registry";
            this.panelHeader.ResumeLayout(false);
            this.grpId.ResumeLayout(false);
            this.grpId.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.numId).EndInit();
            this.grpNome.ResumeLayout(false);
            this.grpNome.PerformLayout();
            this.grpCamposOpcionais.ResumeLayout(false);
            this.grpCamposOpcionais.PerformLayout();
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private GroupBox grpId;
        private Label lblId;
        private NumericUpDown numId;
        private Label lblIdInfo;
        private GroupBox grpNome;
        private Label lblNome;
        private TextBox txtNome;
        private GroupBox grpCamposOpcionais;
        private Label lblField1;
        private TextBox txtField1;
        private Label lblField2;
        private TextBox txtField2;
        private Label lblField3;
        private TextBox txtField3;
        private Label lblField4;
        private TextBox txtField4;
        private GroupBox grpStatus;
        private CheckBox chkBloqueado;
        private Panel panelBotoes;
        private Button btnSalvar;
        private Button btnCancelar;
    }
}
