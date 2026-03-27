using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for creating/editing a Central Registry (Unit).
    /// </summary>
    public partial class FormCadastroCentral : Form
    {
        // Registry data (filled on save)
        public uint IdCadastro { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string? Field1 { get; private set; }
        public string? Field2 { get; private set; }
        public string? Field3 { get; private set; }
        public string? Field4 { get; private set; }
        public bool CadastroEnabled { get; private set; } = true;

        // Edit mode
        public bool ModoEdicao { get; private set; }

        // Data for editing
        private readonly CentralRegistry? _existingRegistry;

        /// <summary>
        /// Constructor for creating a new registry
        /// </summary>
        public FormCadastroCentral()
        {
            InitializeComponent();
            ModoEdicao = false;
            // The checkbox in the UI means "registry active".
            chkBloqueado.Checked = true;
        }

        /// <summary>
        /// Constructor for editing an existing registry
        /// </summary>
        public FormCadastroCentral(CentralRegistry cadastro)
        {
            InitializeComponent();
            _existingRegistry = cadastro;
            ModoEdicao = true;
        }

        private void FormCadastroCentral_Load(object? sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"FormCadastroCentral_Load: ModoEdicao={ModoEdicao}, _existingRegistry={_existingRegistry}");

            if (ModoEdicao && _existingRegistry != null)
            {
                System.Diagnostics.Debug.WriteLine($"Setting up edit mode: ID={_existingRegistry.Id}, Name={_existingRegistry.Name}, Enabled={_existingRegistry.Enabled}");
                ConfigureEditMode();
            }
            else
            {
                // Creation mode - default values
                numId.Value = 0;
                chkBloqueado.Checked = true;
                lblIdInfo.Text = "The server will generate the ID automatically";
            }
        }

        /// <summary>
        /// Configures the form for edit mode
        /// </summary>
        private void ConfigureEditMode()
        {
            if (_existingRegistry == null) return;

            lblTitulo.Text = "Edit Central Registry";
            Text = "Edit Central Registry";

            // Fill fields
            numId.Value = _existingRegistry.Id;
            numId.Enabled = false; // ID cannot be changed
            lblIdInfo.Text = "Registry ID cannot be changed in edit mode.";

            txtNome.Text = _existingRegistry.Name;
            txtField1.Text = _existingRegistry.Field1 ?? "";
            txtField2.Text = _existingRegistry.Field2 ?? "";
            txtField3.Text = _existingRegistry.Field3 ?? "";
            txtField4.Text = _existingRegistry.Field4 ?? "";

            chkBloqueado.Checked = _existingRegistry.Enabled;
        }

        /// <summary>
        /// Validates and saves the registry data
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Validate name
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Enter the name of the registry.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // Store the data
            IdCadastro = (uint)numId.Value;
            Nome = txtNome.Text.Trim();
            Field1 = string.IsNullOrWhiteSpace(txtField1.Text) ? null : txtField1.Text.Trim();
            Field2 = string.IsNullOrWhiteSpace(txtField2.Text) ? null : txtField2.Text.Trim();
            Field3 = string.IsNullOrWhiteSpace(txtField3.Text) ? null : txtField3.Text.Trim();
            Field4 = string.IsNullOrWhiteSpace(txtField4.Text) ? null : txtField4.Text.Trim();
            CadastroEnabled = chkBloqueado.Checked;

            // In creation mode with ID 0, confirm automatic generation
            if (!ModoEdicao && IdCadastro == 0)
            {
                var result = MessageBox.Show(
                    "The ID was not provided (value 0).\n\n" +
                    "The server will generate the ID automatically.\n\n" +
                    "Do you want to continue?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numId.Focus();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
