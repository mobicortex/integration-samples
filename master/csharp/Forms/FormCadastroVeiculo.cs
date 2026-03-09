using MobiCortex.Sdk.Interfaces;
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
        public string Marca { get; private set; } = string.Empty;
        public string Modelo { get; private set; } = string.Empty;
        public string Cor { get; private set; } = string.Empty;
        public string Placa { get; private set; } = string.Empty;
        public bool LprAtivo { get; private set; }
        public bool EntidadeEnabled { get; private set; } = true;

        // Modo edição
        private readonly bool _modoEdicao = false;
        private readonly Entidade? _entidadeExistente;
        private readonly IMobiCortexClient? _api;

        private static readonly string[] FallbackVehicleColors =
        {
            "Amarela", "Azul", "Bege", "Branca", "Cinza", "Dourada", "Grena",
            "Laranja", "Marrom", "Prata", "Preta", "Rosa", "Roxa", "Verde",
            "Vermelha", "Fantasia"
        };

        private static readonly string[] FallbackVehicleBrands =
        {
            "Fiat", "Volkswagen", "Chevrolet", "Toyota", "Hyundai", "Honda", "Jeep",
            "Renault", "Nissan", "Ford", "BYD", "Peugeot", "Citroen", "CAOA Chery",
            "Mitsubishi", "Kia", "Mercedes-Benz", "BMW", "Audi", "Volvo", "Ram"
        };

        /// <summary>
        /// Construtor padrão para o Designer do Visual Studio.
        /// </summary>
        public FormCadastroVeiculo()
        {
            InitializeComponent();
            CadastroId = 0;
            ConfigurarFallbackCatalogos();
        }

        public FormCadastroVeiculo(uint cadastroId, IMobiCortexClient? api = null)
        {
            CadastroId = cadastroId;
            _api = api;
            InitializeComponent();
            ConfigurarFallbackCatalogos();
            lblCadastro.Text = $"Cadastro selecionado: {CadastroId}";
        }

        /// <summary>
        /// Construtor para edição de veículo existente
        /// </summary>
        public FormCadastroVeiculo(Entidade entidade, IMobiCortexClient? api = null) : this(entidade.CadastroId, api)
        {
            _modoEdicao = true;
            _entidadeExistente = entidade;
        }

        private async void FormCadastroVeiculo_Load(object? sender, EventArgs e)
        {
            await CarregarCatalogosVeiculoAsync();

            if (_modoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
                return;
            }

            _chkHabilitado.Checked = true;
            EntidadeEnabled = true;
        }

        private void ConfigurarFallbackCatalogos()
        {
            PreencherCores(FallbackVehicleColors);
            PreencherMarcas(FallbackVehicleBrands);
        }

        private async Task CarregarCatalogosVeiculoAsync()
        {
            if (_api == null) return;

            var result = await _api.Sistema.ObterCatalogosVeiculoAsync();
            if (!result.Success || result.Data == null || result.Data.Ret != 0)
            {
                return;
            }

            var colors = result.Data.Colors
                .Select(color => color.Label?.Trim())
                .Where(label => !string.IsNullOrWhiteSpace(label))
                .Cast<string>()
                .ToArray();

            var brands = result.Data.Brands
                .Select(brand => brand.Name?.Trim())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Cast<string>()
                .ToArray();

            if (colors.Length > 0)
            {
                PreencherCores(colors);
            }

            if (brands.Length > 0)
            {
                PreencherMarcas(brands);
            }
        }

        private void PreencherCores(IEnumerable<string> colors)
        {
            var selected = _cmbCor.SelectedItem?.ToString() ?? _cmbCor.Text;
            _cmbCor.BeginUpdate();
            _cmbCor.Items.Clear();
            _cmbCor.Items.Add("Selecionar...");

            foreach (var color in colors.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                _cmbCor.Items.Add(color);
            }

            if (!string.IsNullOrWhiteSpace(selected))
            {
                var index = _cmbCor.FindStringExact(selected);
                if (index >= 0)
                {
                    _cmbCor.SelectedIndex = index;
                }
                else if (!string.Equals(selected, "Selecionar...", StringComparison.OrdinalIgnoreCase))
                {
                    _cmbCor.Items.Add(selected);
                    _cmbCor.SelectedIndex = _cmbCor.Items.Count - 1;
                }
                else
                {
                    _cmbCor.SelectedIndex = 0;
                }
            }
            else
            {
                _cmbCor.SelectedIndex = 0;
            }

            _cmbCor.EndUpdate();
        }

        private void PreencherMarcas(IEnumerable<string> brands)
        {
            var selected = _txtMarca.Text;
            var orderedBrands = brands
                .Where(brand => !string.IsNullOrWhiteSpace(brand))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            _txtMarca.BeginUpdate();
            _txtMarca.Items.Clear();
            _txtMarca.Items.AddRange(orderedBrands);
            _txtMarca.DropDownStyle = ComboBoxStyle.DropDown;

            if (!string.IsNullOrWhiteSpace(selected))
            {
                var index = _txtMarca.FindStringExact(selected);
                if (index >= 0)
                {
                    _txtMarca.SelectedIndex = index;
                }
                else
                {
                    _txtMarca.Text = selected;
                }
            }
            else
            {
                _txtMarca.SelectedIndex = -1;
                _txtMarca.Text = string.Empty;
            }

            _txtMarca.EndUpdate();
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

            // Preenche placa
            _txtPlaca.Text = _entidadeExistente.Doc;

            // Preenche marca/modelo/cor se disponíveis
            if (!string.IsNullOrEmpty(_entidadeExistente.Brand))
                _txtMarca.Text = _entidadeExistente.Brand;
            if (!string.IsNullOrEmpty(_entidadeExistente.Model))
                _txtModelo.Text = _entidadeExistente.Model;
            if (!string.IsNullOrEmpty(_entidadeExistente.Color))
            {
                var colorIndex = _cmbCor.FindStringExact(_entidadeExistente.Color);
                if (colorIndex >= 0)
                    _cmbCor.SelectedIndex = colorIndex;
                else
                {
                    _cmbCor.Items.Add(_entidadeExistente.Color);
                    _cmbCor.SelectedIndex = _cmbCor.Items.Count - 1;
                }
            }

            // LPR
            _chkLpr.Checked = _entidadeExistente.LprAtivo;

            // Enabled
            EntidadeEnabled = _entidadeExistente.Enabled;
            _chkHabilitado.Checked = _entidadeExistente.Enabled;
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
            Marca = _txtMarca.Text.Trim();
            Modelo = _txtModelo.Text.Trim();
            Cor = _cmbCor.SelectedIndex <= 0 ? string.Empty : _cmbCor.SelectedItem?.ToString() ?? string.Empty;
            Placa = placaNormalizada;
            LprAtivo = _chkLpr.Checked;
            EntidadeEnabled = _chkHabilitado.Checked;

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
