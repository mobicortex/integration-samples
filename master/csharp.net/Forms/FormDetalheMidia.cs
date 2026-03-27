using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for displaying and editing access media details.
    /// </summary>
    public partial class FormDetalheMidia : Form
    {
        private readonly AccessMedia? _midia;
        private uint _expirationOriginal;

        /// <summary>
        /// Returns true if the media was modified.
        /// </summary>
        public bool FoiModificada { get; private set; }

        /// <summary>
        /// Returns the new enabled state (true=enabled, false=blocked).
        /// </summary>
        public bool NovoEstadoEnabled
        {
            get { return !chkBloqueada.Checked; }
        }

        /// <summary>
        /// Returns the permission expiration date (0 = no limit).
        /// </summary>
        public uint NovaDataPermissao
        {
            get
            {
                if (chkBloqueioPorData.Checked && dtpDataBloqueio.Enabled)
                {
                    return (uint)new DateTimeOffset(dtpDataBloqueio.Value).ToUnixTimeSeconds();
                }

                return 0;
            }
        }

        /// <summary>
        /// Returns true if the permission date was changed.
        /// </summary>
        public bool DataPermissaoAlterada
        {
            get { return NovaDataPermissao != _expirationOriginal; }
        }

        public FormDetalheMidia()
        {
            InitializeComponent();
        }

        public FormDetalheMidia(AccessMedia midia)
        {
            InitializeComponent();
            _midia = midia;
            _expirationOriginal = midia.Expiration;
            LoadData();
        }

        private void LoadData()
        {
            if (_midia == null) return;

            lblIdValor.Text = _midia.MediaId.ToString();
            lblTipoValor.Text = _midia.TypeName;
            lblDescricaoValor.Text = _midia.DescriptionAlias;
            lblDataCadastroValor.Text = _midia.CreatedAtFormatted;
            lblDataEdicaoValor.Text = _midia.UpdatedAtFormatted;

            chkBloqueada.Checked = !_midia.Enabled;

            if (_midia.Expiration > 0)
            {
                chkBloqueioPorData.Checked = true;
                dtpDataBloqueio.Enabled = true;
                btnLimparData.Enabled = true;
                dtpDataBloqueio.Value = DateTimeOffset.FromUnixTimeSeconds(_midia.Expiration).LocalDateTime;
            }
            else
            {
                chkBloqueioPorData.Checked = false;
                dtpDataBloqueio.Enabled = false;
                btnLimparData.Enabled = false;
                dtpDataBloqueio.Value = DateTime.Now.AddMonths(1);
            }

            Text = $"Media Details - {_midia.DescriptionAlias}";
        }

        private void chkBloqueioPorData_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = chkBloqueioPorData.Checked;
            dtpDataBloqueio.Enabled = enabled;
            btnLimparData.Enabled = enabled;

            if (enabled && dtpDataBloqueio.Value <= DateTime.Now)
            {
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
            if (_midia == null)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            bool previousState = _midia.Enabled;
            bool newState = !chkBloqueada.Checked;

            bool stateChanged = previousState != newState;
            bool permissionDateChanged = DataPermissaoAlterada;

            FoiModificada = stateChanged || permissionDateChanged;

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
