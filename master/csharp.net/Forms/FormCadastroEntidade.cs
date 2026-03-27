using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for creating/editing an Entity (Person or Vehicle).
    ///
    /// Hierarchy:
    /// Central Registry -> Entity -> Media
    /// </summary>
    public partial class FormCadastroEntidade : Form
    {
        // Entity data (filled when saving)
        public uint CadastroId { get; private set; }
        public uint EntityId { get; private set; }
        public int TipoEntidade { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool LprAtivo { get; private set; }

        // Edit mode
        public bool ModoEdicao { get; private set; }
        public bool IsPessoa
        {
            get { return TipoEntidade == (int)MobiCortex.Sdk.Models.EntityType.Person; }
        }

        public bool IsVeiculo
        {
            get { return TipoEntidade == (int)MobiCortex.Sdk.Models.EntityType.Vehicle; }
        }

        // Data for editing
        private readonly Entity? _entidadeExistente;
        private uint? _cadastroIdPadrao;

        /// <summary>
        /// Default constructor for the Visual Studio Designer.
        /// </summary>
        public FormCadastroEntidade()
        {
            InitializeComponent();
            _cadastroIdPadrao = 0;
            ModoEdicao = false;
        }

        /// <summary>
        /// Constructor for creating a new entity
        /// </summary>
        /// <param name="cadastroId">ID of the linked central registry</param>
        public FormCadastroEntidade(uint cadastroId) : this()
        {
            _cadastroIdPadrao = cadastroId;
            ModoEdicao = false;
        }

        /// <summary>
        /// Constructor for editing an existing entity
        /// </summary>
        public FormCadastroEntidade(Entity entidade) : this()
        {
            _entidadeExistente = entidade;
            _cadastroIdPadrao = entidade.RegistryId;
            ModoEdicao = true;
        }

        private void FormCadastroEntidade_Load(object sender, EventArgs e)
        {
            LoadEntityTypes();

            // Display the central registry ID
            uint cadId = _cadastroIdPadrao ?? 0;
            CadastroId = cadId;
            lblCadastroIdValor.Text = cadId.ToString();

            if (ModoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Creation mode - default values
                numIdEntidade.Value = 0; // Server generates automatically
                cmbTipoEntidade.SelectedIndex = 0; // Person
                chkLprAtivo.Checked = false;
            }
        }

        /// <summary>
        /// Loads the entity types into the ComboBox
        /// </summary>
        private void LoadEntityTypes()
        {
            cmbTipoEntidade.Items.Clear();
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem
            {
                Nome = "Person",
                Valor = (int)MobiCortex.Sdk.Models.EntityType.Person
            });
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem
            {
                Nome = "Vehicle",
                Valor = (int)MobiCortex.Sdk.Models.EntityType.Vehicle
            });
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem
            {
                Nome = "Animal",
                Valor = (int)MobiCortex.Sdk.Models.EntityType.Animal
            });

            cmbTipoEntidade.DisplayMember = "Nome";
            cmbTipoEntidade.ValueMember = "Valor";
        }

        /// <summary>
        /// Configures the form for edit mode
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_entidadeExistente == null) return;

            lblTitulo.Text = "Edit Entity";
            Text = "Edit Entity";

            // Select the current type
            for (int i = 0; i < cmbTipoEntidade.Items.Count; i++)
            {
                if (cmbTipoEntidade.Items[i] is TipoEntidadeItem item &&
                    item.Valor == _entidadeExistente.TypeAlias)
                {
                    cmbTipoEntidade.SelectedIndex = i;
                    break;
                }
            }
            cmbTipoEntidade.Enabled = false; // Type cannot be changed

            // Fill ID (cannot be changed in edit mode)
            numIdEntidade.Value = _entidadeExistente.EntityId;
            numIdEntidade.Enabled = false;
            lblIdInfo.Text = "Entity ID cannot be changed in edit mode.";

            // Fill fields
            txtNome.Text = _entidadeExistente.Name;
            txtDocumento.Text = _entidadeExistente.Doc;
            chkLprAtivo.Checked = _entidadeExistente.LprActive;

            UpdateLabelsByType();
        }

        /// <summary>
        /// Updates labels and visibility when the type changes
        /// </summary>
        private void cmbTipoEntidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoEntidade.SelectedItem is TipoEntidadeItem tipo)
            {
                TipoEntidade = tipo.Valor;
                UpdateLabelsByType();
            }
        }

        /// <summary>
        /// Updates labels according to the selected entity type
        /// </summary>
        private void UpdateLabelsByType()
        {
            if (IsPessoa)
            {
                grpDocumento.Text = "Document (CPF)";
                lblDocumento.Text = "CPF:";
                lblDocInfo.Text = "Optional - for reference only";
                chkLprAtivo.Enabled = false;
                chkLprAtivo.Checked = false;
                grpLpr.Enabled = false;
            }
            else if (IsVeiculo)
            {
                grpDocumento.Text = "Vehicle Plate";
                lblDocumento.Text = "Plate:";
                lblDocInfo.Text = "E.g.: ABC1234 or ABC1D23 (Mercosul)";
                chkLprAtivo.Enabled = true;
                grpLpr.Enabled = true;
            }
            else // Animal
            {
                grpDocumento.Text = "Identification";
                lblDocumento.Text = "ID/Chip:";
                lblDocInfo.Text = "Animal identification";
                chkLprAtivo.Enabled = false;
                chkLprAtivo.Checked = false;
                grpLpr.Enabled = false;
            }
        }

        /// <summary>
        /// Validates and saves the entity data
        /// </summary>
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validate selected type
            if (cmbTipoEntidade.SelectedItem == null)
            {
                MessageBox.Show("Select the entity type.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoEntidade.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // Validate name
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Enter the entity name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // For vehicles, validate plate if provided
            if (IsVeiculo && !string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                var placa = txtDocumento.Text.Trim().ToUpper().Replace("-", "");
                if (!System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$") &&
                    !System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}[0-9]{4}$"))
                {
                    MessageBox.Show("Invalid plate.\nAccepted formats: ABC1234 or ABC1D23",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDocumento.Focus();
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            // Store the data
            TipoEntidade = ((TipoEntidadeItem)cmbTipoEntidade.SelectedItem).Valor;
            EntityId = (uint)numIdEntidade.Value;
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim().ToUpper();
            LprAtivo = chkLprAtivo.Checked;

            // In creation mode with ID 0, confirm automatic generation
            if (!ModoEdicao && EntityId == 0)
            {
                var result = MessageBox.Show(
                    "The entity ID was not provided (value 0).\n\n" +
                    "The server will generate the ID automatically.\n\n" +
                    "Do you want to continue?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numIdEntidade.Focus();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Helper class to represent an entity type in the ComboBox
        /// </summary>
        private class TipoEntidadeItem
        {
            public string Nome { get; set; } = string.Empty;
            public int Valor { get; set; }

            public override string ToString()
            {
                return Nome;
            }
        }
    }
}
