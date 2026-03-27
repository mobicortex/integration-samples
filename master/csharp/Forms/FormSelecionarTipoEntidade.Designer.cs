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
            panelHeader = new Panel();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            _cardPessoa = new Panel();
            lblPessoaDesc = new Label();
            _rbPessoa = new RadioButton();
            _cardVeiculo = new Panel();
            lblVeiculoDesc = new Label();
            _rbVeiculo = new RadioButton();
            _btnOk = new Button();
            _btnCancelar = new Button();
            panelHeader.SuspendLayout();
            _cardPessoa.SuspendLayout();
            _cardVeiculo.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(244, 246, 248);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(504, 62);
            panelHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.ForeColor = Color.FromArgb(95, 102, 109);
            lblSubtitulo.Location = new Point(20, 33);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(440, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Select an option to continue the registration.";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(32, 36, 40);
            lblTitulo.Location = new Point(20, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(420, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Choose the entity type";
            // 
            // _cardPessoa
            // 
            _cardPessoa.BackColor = Color.FromArgb(240, 248, 255);
            _cardPessoa.BorderStyle = BorderStyle.FixedSingle;
            _cardPessoa.Controls.Add(lblPessoaDesc);
            _cardPessoa.Controls.Add(_rbPessoa);
            _cardPessoa.Cursor = Cursors.Hand;
            _cardPessoa.Location = new Point(20, 76);
            _cardPessoa.Name = "_cardPessoa";
            _cardPessoa.Size = new Size(470, 74);
            _cardPessoa.TabIndex = 1;
            // 
            // lblPessoaDesc
            // 
            lblPessoaDesc.ForeColor = Color.FromArgb(95, 102, 109);
            lblPessoaDesc.Location = new Point(34, 36);
            lblPessoaDesc.Name = "lblPessoaDesc";
            lblPessoaDesc.Size = new Size(390, 15);
            lblPessoaDesc.TabIndex = 1;
            lblPessoaDesc.Text = "Resident, employee, visitor";
            // 
            // _rbPessoa
            // 
            _rbPessoa.AutoSize = true;
            _rbPessoa.Checked = true;
            _rbPessoa.Location = new Point(14, 12);
            _rbPessoa.Name = "_rbPessoa";
            _rbPessoa.Size = new Size(65, 19);
            _rbPessoa.TabIndex = 0;
            _rbPessoa.TabStop = true;
            _rbPessoa.Text = "Person";
            _rbPessoa.UseVisualStyleBackColor = true;
            // 
            // _cardVeiculo
            // 
            _cardVeiculo.BackColor = Color.White;
            _cardVeiculo.BorderStyle = BorderStyle.FixedSingle;
            _cardVeiculo.Controls.Add(lblVeiculoDesc);
            _cardVeiculo.Controls.Add(_rbVeiculo);
            _cardVeiculo.Cursor = Cursors.Hand;
            _cardVeiculo.Location = new Point(20, 160);
            _cardVeiculo.Name = "_cardVeiculo";
            _cardVeiculo.Size = new Size(470, 74);
            _cardVeiculo.TabIndex = 2;
            // 
            // lblVeiculoDesc
            // 
            lblVeiculoDesc.ForeColor = Color.FromArgb(95, 102, 109);
            lblVeiculoDesc.Location = new Point(34, 36);
            lblVeiculoDesc.Name = "lblVeiculoDesc";
            lblVeiculoDesc.Size = new Size(390, 15);
            lblVeiculoDesc.TabIndex = 1;
            lblVeiculoDesc.Text = "Car, motorcycle, truck";
            // 
            // _rbVeiculo
            // 
            _rbVeiculo.AutoSize = true;
            _rbVeiculo.Location = new Point(14, 12);
            _rbVeiculo.Name = "_rbVeiculo";
            _rbVeiculo.Size = new Size(66, 19);
            _rbVeiculo.TabIndex = 0;
            _rbVeiculo.Text = "Vehicle";
            _rbVeiculo.UseVisualStyleBackColor = true;
            // 
            // _btnOk
            // 
            _btnOk.BackColor = Color.FromArgb(25, 135, 84);
            _btnOk.DialogResult = DialogResult.OK;
            _btnOk.FlatAppearance.BorderSize = 0;
            _btnOk.FlatStyle = FlatStyle.Flat;
            _btnOk.ForeColor = Color.White;
            _btnOk.Location = new Point(304, 248);
            _btnOk.Name = "_btnOk";
            _btnOk.Size = new Size(96, 30);
            _btnOk.TabIndex = 3;
            _btnOk.Text = "Continue";
            _btnOk.UseVisualStyleBackColor = false;
            // 
            // _btnCancelar
            // 
            _btnCancelar.DialogResult = DialogResult.Cancel;
            _btnCancelar.Location = new Point(406, 248);
            _btnCancelar.Name = "_btnCancelar";
            _btnCancelar.Size = new Size(86, 30);
            _btnCancelar.TabIndex = 4;
            _btnCancelar.Text = "Cancel";
            _btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormSelecionarTipoEntidade
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = _btnCancelar;
            ClientSize = new Size(504, 291);
            Controls.Add(_btnCancelar);
            Controls.Add(_btnOk);
            Controls.Add(_cardVeiculo);
            Controls.Add(_cardPessoa);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelecionarTipoEntidade";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tipo de Entidade";
            panelHeader.ResumeLayout(false);
            _cardPessoa.ResumeLayout(false);
            _cardPessoa.PerformLayout();
            _cardVeiculo.ResumeLayout(false);
            _cardVeiculo.PerformLayout();
            ResumeLayout(false);
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
