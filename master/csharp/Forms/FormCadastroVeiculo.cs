using SmartSdk.Models;

namespace SmartSdk.Forms
{
    /// <summary>
    /// Formulario simplificado para cadastro de veiculo.
    /// Campos: ID, marca, modelo, cor, placa e LPR.
    /// </summary>
    public class FormCadastroVeiculo : Form
    {
        private NumericUpDown _numId = null!;
        private TextBox _txtNomeProprietario = null!;
        private TextBox _txtMarca = null!;
        private TextBox _txtModelo = null!;
        private ComboBox _cmbCor = null!;
        private TextBox _txtPlaca = null!;
        private CheckBox _chkLpr = null!;

        public uint CadastroId { get; }
        public uint IdVeiculo { get; private set; }
        public string NomeProprietario { get; private set; } = string.Empty;
        public string Marca { get; private set; } = string.Empty;
        public string Modelo { get; private set; } = string.Empty;
        public string Cor { get; private set; } = string.Empty;
        public string Placa { get; private set; } = string.Empty;
        public int LprAtivo { get; private set; }

        /// <summary>
        /// Nome tecnico da entidade enviado para API.
        /// O backend exige "name", entao geramos automaticamente com base nos campos do veiculo.
        /// </summary>
        public string NomeEntidadeGerado
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NomeProprietario)) return NomeProprietario;
                var baseNome = $"{Marca} {Modelo}".Trim();
                if (!string.IsNullOrWhiteSpace(baseNome)) return baseNome;
                return $"Veiculo {Placa}";
            }
        }

        public FormCadastroVeiculo(uint cadastroId)
        {
            CadastroId = cadastroId;
            InicializarTela();
        }

        private void InicializarTela()
        {
            SuspendLayout();

            Text = "Cadastrar Veiculo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.White;
            Width = 640;
            Height = 560;

            var panelHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = ClientSize.Width,
                Height = 56,
                BackColor = Color.FromArgb(244, 246, 248),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblTitulo = new Label
            {
                Left = 20,
                Top = 10,
                Width = 420,
                Text = "Novo Veiculo",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 36, 40)
            };

            var lblSubtitulo = new Label
            {
                Left = 20,
                Top = 31,
                Width = 520,
                Text = "Preencha os dados abaixo para cadastrar o veiculo no cadastro selecionado.",
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);

            var lblIdentificacao = new Label
            {
                Left = 20,
                Top = 70,
                Width = 180,
                Height = 18,
                Text = "Identificacao",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 91, 168)
            };

            var panelIdentificacao = new Panel
            {
                Left = 20,
                Top = 92,
                Width = 590,
                Height = 82,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var lblCadastro = new Label
            {
                Left = 16,
                Top = 28,
                Width = 250,
                Text = $"Cadastro selecionado: {CadastroId}",
                ForeColor = Color.Black
            };

            var lblId = new Label
            {
                Left = 16,
                Top = 56,
                Width = 30,
                Text = "ID:"
            };

            _numId = new NumericUpDown
            {
                Left = 52,
                Top = 53,
                Width = 140,
                Maximum = uint.MaxValue,
                Value = 0
            };

            var lblIdInfo = new Label
            {
                Left = 202,
                Top = 56,
                Width = 360,
                Text = "0 = geracao automatica pela controladora",
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            panelIdentificacao.Controls.Add(lblCadastro);
            panelIdentificacao.Controls.Add(lblId);
            panelIdentificacao.Controls.Add(_numId);
            panelIdentificacao.Controls.Add(lblIdInfo);

            var lblDados = new Label
            {
                Left = 20,
                Top = 184,
                Width = 180,
                Height = 18,
                Text = "Dados do Veiculo",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 91, 168)
            };

            var panelDados = new Panel
            {
                Left = 20,
                Top = 206,
                Width = 590,
                Height = 186,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var lblMarca = new Label { Left = 16, Top = 30, Width = 140, Text = "Marca" };
            _txtMarca = new TextBox { Left = 16, Top = 49, Width = 270, PlaceholderText = "Ex: Honda" };

            var lblNomeProprietario = new Label { Left = 302, Top = 30, Width = 180, Text = "Nome do Proprietario" };
            _txtNomeProprietario = new TextBox
            {
                Left = 302,
                Top = 49,
                Width = 270,
                PlaceholderText = "Opcional"
            };

            var lblModelo = new Label { Left = 16, Top = 86, Width = 140, Text = "Modelo" };
            _txtModelo = new TextBox { Left = 16, Top = 105, Width = 270, PlaceholderText = "Ex: Civic" };

            var lblCor = new Label { Left = 302, Top = 86, Width = 140, Text = "Cor" };
            _cmbCor = new ComboBox
            {
                Left = 302,
                Top = 105,
                Width = 270,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbCor.Items.AddRange(new object[]
            {
                "Selecionar...",
                "Branco",
                "Preto",
                "Prata",
                "Cinza",
                "Azul",
                "Vermelho",
                "Verde",
                "Amarelo",
                "Marrom",
                "Outro"
            });
            _cmbCor.SelectedIndex = 0;

            var lblPlaca = new Label { Left = 16, Top = 132, Width = 140, Text = "Placa" };
            _txtPlaca = new TextBox { Left = 16, Top = 151, Width = 270, PlaceholderText = "ABC-1234" };

            panelDados.Controls.Add(lblMarca);
            panelDados.Controls.Add(_txtMarca);
            panelDados.Controls.Add(lblNomeProprietario);
            panelDados.Controls.Add(_txtNomeProprietario);
            panelDados.Controls.Add(lblModelo);
            panelDados.Controls.Add(_txtModelo);
            panelDados.Controls.Add(lblCor);
            panelDados.Controls.Add(_cmbCor);
            panelDados.Controls.Add(lblPlaca);
            panelDados.Controls.Add(_txtPlaca);

            var panelLpr = new Panel
            {
                Left = 20,
                Top = 398,
                Width = 590,
                Height = 44,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 250, 252)
            };

            _chkLpr = new CheckBox
            {
                Left = 12,
                Top = 11,
                Width = 220,
                Text = "Leitura de Placa (LPR)",
                Checked = true
            };

            var lblLprInfo = new Label
            {
                Left = 236,
                Top = 13,
                Width = 330,
                Text = "Identificar veiculo automaticamente",
                ForeColor = Color.FromArgb(95, 102, 109)
            };

            panelLpr.Controls.Add(_chkLpr);
            panelLpr.Controls.Add(lblLprInfo);

            var panelBotoes = new Panel
            {
                Left = 0,
                Top = 448,
                Width = ClientSize.Width,
                Height = 42,
                BackColor = Color.FromArgb(244, 246, 248),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            var btnSalvar = new Button
            {
                Text = "Salvar",
                Left = 446,
                Top = 8,
                Width = 78,
                Height = 26,
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;

            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Left = 530,
                Top = 8,
                Width = 82,
                Height = 26,
                DialogResult = DialogResult.Cancel
            };

            panelBotoes.Controls.Add(btnSalvar);
            panelBotoes.Controls.Add(btnCancelar);

            Controls.Add(panelHeader);
            Controls.Add(lblIdentificacao);
            Controls.Add(panelIdentificacao);
            Controls.Add(lblDados);
            Controls.Add(panelDados);
            Controls.Add(panelLpr);
            Controls.Add(panelBotoes);

            AcceptButton = btnSalvar;
            CancelButton = btnCancelar;

            ResumeLayout(false);
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            var placaNormalizada = _txtPlaca.Text.Trim().ToUpper().Replace("-", "").Replace(" ", "");
            if (string.IsNullOrWhiteSpace(placaNormalizada))
            {
                MessageBox.Show("Placa e obrigatoria.", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                _txtPlaca.Focus();
                return;
            }

            var placaValida =
                System.Text.RegularExpressions.Regex.IsMatch(placaNormalizada, @"^[A-Z]{3}[0-9]{4}$") ||
                System.Text.RegularExpressions.Regex.IsMatch(placaNormalizada, @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");

            if (!placaValida)
            {
                MessageBox.Show("Placa invalida. Use ABC1234 ou ABC1D23.", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                _txtPlaca.Focus();
                return;
            }

            IdVeiculo = (uint)_numId.Value;
            NomeProprietario = _txtNomeProprietario.Text.Trim();
            Marca = _txtMarca.Text.Trim();
            Modelo = _txtModelo.Text.Trim();
            Cor = _cmbCor.SelectedIndex <= 0 ? string.Empty : _cmbCor.SelectedItem?.ToString() ?? string.Empty;
            Placa = placaNormalizada;
            LprAtivo = _chkLpr.Checked ? 1 : 0;

            if (IdVeiculo == 0)
            {
                var result = MessageBox.Show(
                    "ID nao informado. O codigo sera gerado automaticamente pela controladora.\n\nDeseja continuar?",
                    "Confirmacao - ID Automatico",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    return;
                }
            }
        }
    }
}
