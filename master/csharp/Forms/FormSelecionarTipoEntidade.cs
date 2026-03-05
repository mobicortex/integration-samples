using SmartSdk.Models;

namespace SmartSdk.Forms
{
    /// <summary>
    /// Dialogo simples para escolher o tipo da entidade a ser criada.
    /// </summary>
    public class FormSelecionarTipoEntidade : Form
    {
        private readonly RadioButton _rbPessoa;
        private readonly RadioButton _rbVeiculo;
        private readonly Button _btnOk;
        private readonly Button _btnCancelar;
        private readonly Panel _cardPessoa;
        private readonly Panel _cardVeiculo;

        public int TipoEntidadeSelecionado =>
            _rbPessoa.Checked ? (int)TipoEntidade.Pessoa : (int)TipoEntidade.Veiculo;

        public FormSelecionarTipoEntidade()
        {
            SuspendLayout();

            Text = "Tipo de Entidade";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.White;
            Width = 520;
            Height = 330;

            var panelHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = ClientSize.Width,
                Height = 62,
                BackColor = Color.FromArgb(244, 246, 248),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblTitulo = new Label
            {
                Text = "Escolha o tipo da entidade",
                Left = 20,
                Top = 12,
                Width = 420,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 36, 40)
            };

            var lblSubtitulo = new Label
            {
                Text = "Selecione uma opcao para continuar o cadastro.",
                Left = 20,
                Top = 33,
                Width = 440,
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);

            _cardPessoa = new Panel
            {
                Left = 20,
                Top = 76,
                Width = 470,
                Height = 74,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 248, 255),
                Cursor = Cursors.Hand
            };

            _rbPessoa = new RadioButton
            {
                Text = "Pessoa",
                Left = 14,
                Top = 12,
                Width = 120,
                Checked = true
            };

            var lblPessoaDesc = new Label
            {
                Text = "Morador, colaborador, visitante",
                Left = 34,
                Top = 36,
                Width = 390,
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            _cardPessoa.Controls.Add(_rbPessoa);
            _cardPessoa.Controls.Add(lblPessoaDesc);

            _cardVeiculo = new Panel
            {
                Left = 20,
                Top = 160,
                Width = 470,
                Height = 74,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            _rbVeiculo = new RadioButton
            {
                Text = "Veiculo",
                Left = 14,
                Top = 12,
                Width = 120
            };

            var lblVeiculoDesc = new Label
            {
                Text = "Carro, moto, caminhonete",
                Left = 34,
                Top = 36,
                Width = 390,
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            _cardVeiculo.Controls.Add(_rbVeiculo);
            _cardVeiculo.Controls.Add(lblVeiculoDesc);

            _cardPessoa.Click += (_, _) => SelecionarTipo(true);
            _cardVeiculo.Click += (_, _) => SelecionarTipo(false);
            lblPessoaDesc.Click += (_, _) => SelecionarTipo(true);
            lblVeiculoDesc.Click += (_, _) => SelecionarTipo(false);
            _rbPessoa.CheckedChanged += (_, _) => AtualizarCards();
            _rbVeiculo.CheckedChanged += (_, _) => AtualizarCards();

            _btnOk = new Button
            {
                Text = "Continuar",
                Width = 96,
                Height = 30,
                Left = 304,
                Top = 248,
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnOk.FlatAppearance.BorderSize = 0;

            _btnCancelar = new Button
            {
                Text = "Cancelar",
                Width = 86,
                Height = 30,
                Left = 406,
                Top = 248,
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(panelHeader);
            Controls.Add(_cardPessoa);
            Controls.Add(_cardVeiculo);
            Controls.Add(_btnOk);
            Controls.Add(_btnCancelar);

            AcceptButton = _btnOk;
            CancelButton = _btnCancelar;

            AtualizarCards();
            ResumeLayout(false);
        }

        private void SelecionarTipo(bool pessoa)
        {
            _rbPessoa.Checked = pessoa;
            _rbVeiculo.Checked = !pessoa;
        }

        private void AtualizarCards()
        {
            var selected = Color.FromArgb(240, 248, 255);
            var normal = Color.White;
            _cardPessoa.BackColor = _rbPessoa.Checked ? selected : normal;
            _cardVeiculo.BackColor = _rbVeiculo.Checked ? selected : normal;
        }
    }
}
