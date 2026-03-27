using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for creating/editing a Person (Entity type 1).
    /// </summary>
    public partial class FormCadastroPessoa : Form
    {
        // Person data (filled when saving)
        public uint Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool LprAtivo { get; private set; }
        public bool EntidadeEnabled { get; private set; } = true;

        // Edit mode
        public bool ModoEdicao { get; private set; }

        // Data for editing
        private readonly Entity? _entidadeExistente;
        private readonly uint _cadastroIdPadrao;

        public FormCadastroPessoa() : this(0)
        {
        }

        /// <summary>
        /// Constructor for creating a new person
        /// </summary>
        public FormCadastroPessoa(uint cadastroId)
        {
            _cadastroIdPadrao = cadastroId;
            InitializeComponent();
            ModoEdicao = false;
        }

        /// <summary>
        /// Constructor for editing an existing person
        /// </summary>
        public FormCadastroPessoa(Entity entidade)
        {
            _entidadeExistente = entidade;
            _cadastroIdPadrao = entidade.RegistryId;
            InitializeComponent();
            ModoEdicao = true;
        }

        private void FormCadastroPessoa_Load(object sender, EventArgs e)
        {
            if (ModoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Creation mode - default values
                numId.Value = 0;
                chkHabilitado.Checked = true;
                chkLprAtivo.Checked = false;
                chkLprAtivo.Enabled = false;
                chkLprAtivo.Text = "LPR does not apply to person";
                lblIdInfo.Text = "0 = automatic generation by server";
            }
        }

        /// <summary>
        /// Configures the form for edit mode
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_entidadeExistente == null) return;

            lblTitulo.Text = "Edit Person";
            Text = "Edit Person";

            // Fill fields
            numId.Value = _entidadeExistente.EntityId;
            numId.Enabled = false; // ID cannot be changed
            lblIdInfo.Text = "Entity ID cannot be changed in edit mode.";

            txtNome.Text = _entidadeExistente.Name;
            txtDocumento.Text = _entidadeExistente.Doc;
            chkLprAtivo.Checked = false;
            chkLprAtivo.Enabled = false;
            chkLprAtivo.Text = "LPR does not apply to person";
            chkHabilitado.Checked = _entidadeExistente.Enabled;
        }

        /// <summary>
        /// Validates and saves the person data
        /// </summary>
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validate name
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Enter the person's name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // Store the data
            Id = (uint)numId.Value;
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim();
            LprAtivo = false;
            EntidadeEnabled = chkHabilitado.Checked;

            // In creation mode with ID 0, confirm automatic generation
            if (!ModoEdicao && Id == 0)
            {
                lblIdInfo.Text = "Will be generated automatically by the server";
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
