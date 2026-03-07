using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulario simplificado para cadastro de veiculo.
    /// Campos: ID, marca, modelo, cor, placa e LPR.
    /// </summary>
    public partial class FormCadastroVeiculo : Form
    {
        public uint CadastroId { get; }
        public uint IdVeiculo { get; private set; }
        public string NomeProprietario { get; private set; } = string.Empty;
        public string Marca { get; private set; } = string.Empty;
        public string Modelo { get; private set; } = string.Empty;
        public string Cor { get; private set; } = string.Empty;
        public string Placa { get; private set; } = string.Empty;
        public int LprAtivo { get; private set; }
        public bool EntidadeEnabled { get; private set; } = true;

        // Modo edição
        private readonly bool _modoEdicao = false;
        private readonly Entidade? _entidadeExistente;

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

        /// <summary>
        /// Construtor padrão para o Designer do Visual Studio.
        /// </summary>
        public FormCadastroVeiculo()
        {
            InitializeComponent();
            CadastroId = 0;
        }

        public FormCadastroVeiculo(uint cadastroId)
        {
            CadastroId = cadastroId;
            InitializeComponent();
            lblCadastro.Text = $"Cadastro selecionado: {CadastroId}";
        }

        /// <summary>
        /// Construtor para edição de veículo existente
        /// </summary>
        public FormCadastroVeiculo(Entidade entidade) : this()
        {
            _modoEdicao = true;
            _entidadeExistente = entidade;
            CadastroId = entidade.CadastroId;
        }

        private void FormCadastroVeiculo_Load(object? sender, EventArgs e)
        {
            if (_modoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
            }
        }

        private void ConfigurarModoEdicao()
        {
            if (_entidadeExistente == null) return;

            Text = "Editar Veículo";
            lblTitulo.Text = "Editar Veículo";
            lblCadastro.Text = $"Cadastro ID: {CadastroId}";

            // Preenche os campos com os dados existentes
            // Aumenta o máximo para aceitar IDs altos (faixa web >= 4294000000)
            _numId.Maximum = uint.MaxValue;
            _numId.Value = _entidadeExistente.EntityId;
            _numId.Enabled = false; // ID não pode ser alterado

            // O nome pode conter: "Proprietario", "Marca Modelo" ou "Veiculo PLACA"
            // Tenta extrair o nome do proprietário se não parecer ser gerado
            var name = _entidadeExistente.Name;
            if (!name.StartsWith("Veiculo ") && !name.Contains(" ") == false)
            {
                // Pode ser "Marca Modelo" ou nome do proprietário
                // Se tiver mais de uma palavra e não começar com Veiculo, assume como proprietário
                _txtNomeProprietario.Text = name;
            }

            // Preenche placa
            _txtPlaca.Text = _entidadeExistente.Doc;

            // Preenche marca/modelo/cor se disponíveis
            if (!string.IsNullOrEmpty(_entidadeExistente.Brand))
                _txtMarca.Text = _entidadeExistente.Brand;
            if (!string.IsNullOrEmpty(_entidadeExistente.Model))
                _txtModelo.Text = _entidadeExistente.Model;
            if (!string.IsNullOrEmpty(_entidadeExistente.Color))
            {
                var colorIndex = _cmbCor.Items.IndexOf(_entidadeExistente.Color);
                if (colorIndex >= 0)
                    _cmbCor.SelectedIndex = colorIndex;
                else
                    _cmbCor.Text = _entidadeExistente.Color;
            }

            // LPR
            _chkLpr.Checked = _entidadeExistente.LprAtivo == 1;

            // Enabled
            EntidadeEnabled = _entidadeExistente.Enabled;
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
