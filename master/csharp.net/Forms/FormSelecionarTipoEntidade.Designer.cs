namespace SmartSdk
{
    partial class FormSelecionarTipoEntidade
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
            this._cardPessoa = new Panel();
            this.lblPessoaDesc = new Label();
            this._rbPessoa = new RadioButton();
            this._cardVeiculo = new Panel();
            this.lblVeiculoDesc = new Label();
            this._rbVeiculo = new RadioButton();
            this._btnOk = new Button();
            this._btnCancelar = new Button();
            this.panelHeader.SuspendLayout();
            this._cardPessoa.SuspendLayout();
            this._cardVeiculo.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.BackColor = Color.FromArgb(244, 246, 248);
            this.panelHeader.Controls.Add(this.lblSubtitulo);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(504, 62);
            this.panelHeader.TabIndex = 0;
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblSubtitulo.Location = new Point(20, 33);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new Size(440, 15);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Select an option to continue the registration.";
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            this.lblTitulo.Location = new Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(420, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Choose the entity type";
            //
            // _cardPessoa
            //
            this._cardPessoa.BackColor = Color.FromArgb(240, 248, 255);
            this._cardPessoa.BorderStyle = BorderStyle.FixedSingle;
            this._cardPessoa.Controls.Add(this.lblPessoaDesc);
            this._cardPessoa.Controls.Add(this._rbPessoa);
            this._cardPessoa.Cursor = Cursors.Hand;
            this._cardPessoa.Location = new Point(20, 76);
            this._cardPessoa.Name = "_cardPessoa";
            this._cardPessoa.Size = new Size(470, 74);
            this._cardPessoa.TabIndex = 1;
            //
            // lblPessoaDesc
            //
            this.lblPessoaDesc.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblPessoaDesc.Location = new Point(34, 36);
            this.lblPessoaDesc.Name = "lblPessoaDesc";
            this.lblPessoaDesc.Size = new Size(390, 15);
            this.lblPessoaDesc.TabIndex = 1;
            this.lblPessoaDesc.Text = "Resident, employee, visitor";
            //
            // _rbPessoa
            //
            this._rbPessoa.AutoSize = true;
            this._rbPessoa.Checked = true;
            this._rbPessoa.Location = new Point(14, 12);
            this._rbPessoa.Name = "_rbPessoa";
            this._rbPessoa.Size = new Size(65, 19);
            this._rbPessoa.TabIndex = 0;
            this._rbPessoa.TabStop = true;
            this._rbPessoa.Text = "Person";
            this._rbPessoa.UseVisualStyleBackColor = true;
            //
            // _cardVeiculo
            //
            this._cardVeiculo.BackColor = Color.White;
            this._cardVeiculo.BorderStyle = BorderStyle.FixedSingle;
            this._cardVeiculo.Controls.Add(this.lblVeiculoDesc);
            this._cardVeiculo.Controls.Add(this._rbVeiculo);
            this._cardVeiculo.Cursor = Cursors.Hand;
            this._cardVeiculo.Location = new Point(20, 160);
            this._cardVeiculo.Name = "_cardVeiculo";
            this._cardVeiculo.Size = new Size(470, 74);
            this._cardVeiculo.TabIndex = 2;
            //
            // lblVeiculoDesc
            //
            this.lblVeiculoDesc.ForeColor = Color.FromArgb(95, 102, 109);
            this.lblVeiculoDesc.Location = new Point(34, 36);
            this.lblVeiculoDesc.Name = "lblVeiculoDesc";
            this.lblVeiculoDesc.Size = new Size(390, 15);
            this.lblVeiculoDesc.TabIndex = 1;
            this.lblVeiculoDesc.Text = "Car, motorcycle, truck";
            //
            // _rbVeiculo
            //
            this._rbVeiculo.AutoSize = true;
            this._rbVeiculo.Location = new Point(14, 12);
            this._rbVeiculo.Name = "_rbVeiculo";
            this._rbVeiculo.Size = new Size(66, 19);
            this._rbVeiculo.TabIndex = 0;
            this._rbVeiculo.Text = "Vehicle";
            this._rbVeiculo.UseVisualStyleBackColor = true;
            //
            // _btnOk
            //
            this._btnOk.BackColor = Color.FromArgb(25, 135, 84);
            this._btnOk.DialogResult = DialogResult.OK;
            this._btnOk.FlatAppearance.BorderSize = 0;
            this._btnOk.FlatStyle = FlatStyle.Flat;
            this._btnOk.ForeColor = Color.White;
            this._btnOk.Location = new Point(304, 248);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new Size(96, 30);
            this._btnOk.TabIndex = 3;
            this._btnOk.Text = "Continue";
            this._btnOk.UseVisualStyleBackColor = false;
            //
            // _btnCancelar
            //
            this._btnCancelar.DialogResult = DialogResult.Cancel;
            this._btnCancelar.Location = new Point(406, 248);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new Size(86, 30);
            this._btnCancelar.TabIndex = 4;
            this._btnCancelar.Text = "Cancel";
            this._btnCancelar.UseVisualStyleBackColor = true;
            //
            // FormSelecionarTipoEntidade
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.CancelButton = this._btnCancelar;
            this.ClientSize = new Size(504, 291);
            this.Controls.Add(this._btnCancelar);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._cardVeiculo);
            this.Controls.Add(this._cardPessoa);
            this.Controls.Add(this.panelHeader);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelecionarTipoEntidade";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Entity Type";
            this.panelHeader.ResumeLayout(false);
            this._cardPessoa.ResumeLayout(false);
            this._cardPessoa.PerformLayout();
            this._cardVeiculo.ResumeLayout(false);
            this._cardVeiculo.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Panel _cardPessoa;
        private RadioButton _rbPessoa;
        private Label lblPessoaDesc;
        private Panel _cardVeiculo;
        private RadioButton _rbVeiculo;
        private Label lblVeiculoDesc;
        private Button _btnOk;
        private Button _btnCancelar;
    }
}
