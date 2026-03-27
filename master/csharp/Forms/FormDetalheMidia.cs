using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for displaying and editing access media details.
    /// </summary>
    public partial class FormDetalheMidia : Form
    {
        private readonly AccessMedia _media;
        private uint _expirationOriginal;

        /// <summary>
        /// Returns true if the media was modified.
        /// </summary>
        public bool FoiModificada { get; private set; }

        /// <summary>
        /// Returns the new enabled state (true=enabled, false=blocked).
        /// </summary>
        public bool NovoEstadoEnabled => !chkBloqueada.Checked;

        /// <summary>
        /// Returns the permission expiration date (0 = no limit).
        /// </summary>
        public uint NovaDataPermissao => chkBloqueioPorData.Checked && dtpDataBloqueio.Enabled
            ? (uint)new DateTimeOffset(dtpDataBloqueio.Value).ToUnixTimeSeconds()
            : 0;

        /// <summary>
        /// Returns true if the permission date was changed.
        /// </summary>
        public bool DataPermissaoAlterada => NovaDataPermissao != _expirationOriginal;

        public FormDetalheMidia(AccessMedia media)
        {
            InitializeComponent();
            _media = media;
            _expirationOriginal = media.Expiration;
            LoadData();
        }

        private void LoadData()
        {
            // Fill information
            lblIdValor.Text = _media.MediaId.ToString();
            lblTipoValor.Text = _media.TypeName;
            lblDescricaoValor.Text = _media.DescriptionAlias;
            lblDataCadastroValor.Text = _media.CreatedAtFormatted;
            lblDataEdicaoValor.Text = _media.UpdatedAtFormatted;

            // Configure total block checkbox
            // Enabled=true means enabled, Enabled=false means blocked
            chkBloqueada.Checked = !_media.Enabled;

            // Configure permission date (expiration = date when the media expires)
            if (_media.Expiration > 0)
            {
                chkBloqueioPorData.Checked = true;
                dtpDataBloqueio.Enabled = true;
                btnLimparData.Enabled = true;
                dtpDataBloqueio.Value = DateTimeOffset.FromUnixTimeSeconds(_media.Expiration).LocalDateTime;
            }
            else
            {
                chkBloqueioPorData.Checked = false;
                dtpDataBloqueio.Enabled = false;
                btnLimparData.Enabled = false;
                dtpDataBloqueio.Value = DateTime.Now.AddMonths(1); // Default: 1 month
            }

            // Update title
            Text = $"Media Details - {_media.DescriptionAlias}";
        }

        private void chkBloqueioPorData_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = chkBloqueioPorData.Checked;
            dtpDataBloqueio.Enabled = enabled;
            btnLimparData.Enabled = enabled;

            if (enabled && dtpDataBloqueio.Value <= DateTime.Now)
            {
                // If enabled and date is in the past, adjust to 1 month in the future
                dtpDataBloqueio.Value = DateTime.Now.AddMonths(1);
            }
        }

        private void btnLimparData_Click(object sender, EventArgs e)
        {
            chkBloqueioPorData.Checked = false;
            dtpDataBloqueio.Value = DateTime.Now.AddMonths(1);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Check if there was a change in state
            bool previousState = _media.Enabled;
            bool newState = !chkBloqueada.Checked;

            bool stateChanged = previousState != newState;
            bool permissionDateChanged = DataPermissaoAlterada;

            FoiModificada = stateChanged || permissionDateChanged;

            // Validation: permission date cannot be in the past
            if (chkBloqueioPorData.Checked && dtpDataBloqueio.Value <= DateTime.Now)
            {
                MessageBox.Show("The permission date must be in the future.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
