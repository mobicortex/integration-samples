using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Simplified form for vehicle registration.
    /// Fields: ID, brand, model, color, plate and LPR.
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

        // Edit mode
        private readonly bool _editMode = false;
        private readonly Entity? _existingEntity;
        private readonly IMobiCortexClient? _api;

        private static readonly string[] FallbackVehicleColors =
        {
            "Yellow", "Blue", "Beige", "White", "Gray", "Gold", "Maroon",
            "Orange", "Brown", "Silver", "Black", "Pink", "Purple", "Green",
            "Red", "Custom"
        };

        private static readonly string[] FallbackVehicleBrands =
        {
            "Fiat", "Volkswagen", "Chevrolet", "Toyota", "Hyundai", "Honda", "Jeep",
            "Renault", "Nissan", "Ford", "BYD", "Peugeot", "Citroen", "CAOA Chery",
            "Mitsubishi", "Kia", "Mercedes-Benz", "BMW", "Audi", "Volvo", "Ram"
        };

        /// <summary>
        /// Default constructor for Visual Studio Designer.
        /// </summary>
        public FormCadastroVeiculo()
        {
            InitializeComponent();
            CadastroId = 0;
            ConfigureFallbackCatalogs();
        }

        public FormCadastroVeiculo(uint cadastroId, IMobiCortexClient? api = null)
        {
            CadastroId = cadastroId;
            _api = api;
            InitializeComponent();
            ConfigureFallbackCatalogs();
            lblCadastro.Text = $"Selected registry: {CadastroId}";
        }

        /// <summary>
        /// Constructor for editing an existing vehicle
        /// </summary>
        public FormCadastroVeiculo(Entity entidade, IMobiCortexClient? api = null) : this(entidade.RegistryId, api)
        {
            _editMode = true;
            _existingEntity = entidade;
        }

        private async void FormCadastroVeiculo_Load(object? sender, EventArgs e)
        {
            await LoadVehicleCatalogsAsync();

            if (_editMode && _existingEntity != null)
            {
                ConfigureEditMode();
                return;
            }

            _chkHabilitado.Checked = true;
            EntidadeEnabled = true;
        }

        private void ConfigureFallbackCatalogs()
        {
            FillColors(FallbackVehicleColors);
            FillBrands(FallbackVehicleBrands);
        }

        private async Task LoadVehicleCatalogsAsync()
        {
            if (_api == null) return;

            var result = await _api.SystemInfo.GetVehicleCatalogsAsync();
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
                FillColors(colors);
            }

            if (brands.Length > 0)
            {
                FillBrands(brands);
            }
        }

        private void FillColors(IEnumerable<string> colors)
        {
            var selected = _cmbCor.SelectedItem?.ToString() ?? _cmbCor.Text;
            _cmbCor.BeginUpdate();
            _cmbCor.Items.Clear();
            _cmbCor.Items.Add("Select...");

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
                else if (!string.Equals(selected, "Select...", StringComparison.OrdinalIgnoreCase))
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

        private void FillBrands(IEnumerable<string> brands)
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

        private void ConfigureEditMode()
        {
            if (_existingEntity == null) return;

            Text = "Edit Vehicle";
            lblTitulo.Text = "Edit Vehicle";
            lblCadastro.Text = $"Registry ID: {CadastroId}";

            // Fill fields with existing data
            // Increase maximum to accept high IDs (web range >= 4294000000)
            _numId.Maximum = uint.MaxValue;
            _numId.Value = _existingEntity.EntityId;
            _numId.Enabled = false; // ID cannot be changed

            // Fill plate
            _txtPlaca.Text = _existingEntity.Doc;

            // Fill brand/model/color if available
            if (!string.IsNullOrEmpty(_existingEntity.Brand))
                _txtMarca.Text = _existingEntity.Brand;
            if (!string.IsNullOrEmpty(_existingEntity.Model))
                _txtModelo.Text = _existingEntity.Model;
            if (!string.IsNullOrEmpty(_existingEntity.Color))
            {
                var colorIndex = _cmbCor.FindStringExact(_existingEntity.Color);
                if (colorIndex >= 0)
                    _cmbCor.SelectedIndex = colorIndex;
                else
                {
                    _cmbCor.Items.Add(_existingEntity.Color);
                    _cmbCor.SelectedIndex = _cmbCor.Items.Count - 1;
                }
            }

            // LPR
            _chkLpr.Checked = _existingEntity.LprActive;

            // Enabled
            EntidadeEnabled = _existingEntity.Enabled;
            _chkHabilitado.Checked = _existingEntity.Enabled;
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            var normalizedPlate = _txtPlaca.Text.Trim().ToUpper().Replace("-", "").Replace(" ", "");
            if (string.IsNullOrWhiteSpace(normalizedPlate))
            {
                MessageBox.Show("License plate is mandatory.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                _txtPlaca.Focus();
                return;
            }

            var validPlate =
                System.Text.RegularExpressions.Regex.IsMatch(normalizedPlate, @"^[A-Z]{3}[0-9]{4}$") ||
                System.Text.RegularExpressions.Regex.IsMatch(normalizedPlate, @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");

            if (!validPlate)
            {
                MessageBox.Show("Invalid plate. Use ABC1234 or ABC1D23.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                _txtPlaca.Focus();
                return;
            }

            IdVeiculo = (uint)_numId.Value;
            Marca = _txtMarca.Text.Trim();
            Modelo = _txtModelo.Text.Trim();
            Cor = _cmbCor.SelectedIndex <= 0 ? string.Empty : _cmbCor.SelectedItem?.ToString() ?? string.Empty;
            Placa = normalizedPlate;
            LprAtivo = _chkLpr.Checked;
            EntidadeEnabled = _chkHabilitado.Checked;

            if (IdVeiculo == 0)
            {
                var result = MessageBox.Show(
                    "ID not provided. The server will generate the ID automatically.\n\nDo you want to continue?",
                    "Confirmation - Automatic ID",
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
