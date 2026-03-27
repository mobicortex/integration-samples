using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for editing a Person (Entity type 1).
    /// </summary>
    public partial class FormCadastroPessoaEdit : Form
    {
        // Person data (filled on save)
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool EntidadeEnabled { get; private set; } = true;

        // Data for editing
        private readonly Entity _entidade;

        /// <summary>
        /// Constructor for editing an existing person
        /// </summary>
        public FormCadastroPessoaEdit(Entity entidade)
        {
            _entidade = entidade;
            InitializeComponent();
        }

        private void FormCadastroPessoaEdit_Load(object? sender, EventArgs e)
        {
            // Fill fields with entity data
            lblEntityId.Text = _entidade.EntityId.ToString();
            txtNome.Text = _entidade.Name;
            txtDocumento.Text = _entidade.Doc;

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Loading entity ID={_entidade.EntityId}, Enabled={_entidade.Enabled}");
            chkHabilitado.Checked = _entidade.Enabled;

            // Show dates
            lblCreatedAt.Text = _entidade.CreatedAtFormatted;
            lblUpdatedAt.Text = _entidade.UpdatedAtFormatted;
        }

        /// <summary>
        /// Validates and saves the person data
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Validate name
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Enter the name of the person.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // Store the data
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim();

            EntidadeEnabled = chkHabilitado.Checked;
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Saving entity - Name={Nome}, Enabled={EntidadeEnabled}, Checked={chkHabilitado.Checked}");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
