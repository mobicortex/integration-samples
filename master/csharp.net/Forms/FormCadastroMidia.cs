using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Form for creating/editing access media.
    ///
    /// MEDIA TYPES AND HOW TO USE:
    ///
    /// 1. RFID (Wiegand 26/34 bits):
    ///    - Accepted formats: "123,45678" (facility,code) or "HEX: FF FF FF"
    ///    - The backend automatically detects and converts to binary data
    ///    - Does not require additional fields beyond type and description
    ///
    /// 2. LPR PLATE (type 17):
    ///    - Format: vehicle plate (e.g.: "ABC1D23" - Mercosul format)
    ///    - IMPORTANT: When creating via API, send ns32_0=0 and ns32_1=0 to prevent
    ///      the backend from trying to validate the plate as RFID format
    ///    - The recommended way is to use lpr_enabled=true on the vehicle entity
    ///
    /// 3. FACIAL (type 20), BIOMETRY (type 5/15/18), etc:
    ///    - Generally require integration with specific hardware
    ///    - Send the identifier in the description field
    ///    - To avoid RFID validation, send ns32_0=0 and ns32_1=0
    ///
    /// Backend reference: ws_media6.cpp (validation in media_try_apply_rfid_from_text)
    /// </summary>
    public partial class FormCadastroMidia : Form
    {
        // Media data (filled when saving)
        public int TipoMidiaSelecionado { get; private set; }
        public uint IdMidia { get; private set; }
        public string DadosMidia { get; private set; } = string.Empty;
        public string TipoMidiaNome { get; private set; } = string.Empty;

        // Edit mode
        public bool ModoEdicao { get; private set; }

        // Data for editing (optional)
        private readonly AccessMedia? _midiaExistente;
        private readonly uint? _entityIdPadrao;
        private readonly string? _placaPadraoLpr;

        /// <summary>
        /// Constructor for creating new media
        /// </summary>
        public FormCadastroMidia()
        {
            InitializeComponent();
            ModoEdicao = false;
        }

        /// <summary>
        /// Constructor for editing existing media
        /// </summary>
        public FormCadastroMidia(AccessMedia midia)
        {
            InitializeComponent();
            _midiaExistente = midia;
            ModoEdicao = true;
        }

        /// <summary>
        /// Constructor with pre-selected default entity_id
        /// </summary>
        public FormCadastroMidia(uint entityIdPadrao, string? placaPadraoLpr = null)
        {
            InitializeComponent();
            _entityIdPadrao = entityIdPadrao;
            _placaPadraoLpr = placaPadraoLpr;
            ModoEdicao = false;
        }

        private void FormCadastroMidia_Load(object sender, EventArgs e)
        {
            LoadMediaTypes();

            if (ModoEdicao && _midiaExistente != null)
            {
                ConfigureEditMode();
            }
            else
            {
                // Creation mode - ID 0 by default (server generates automatically)
                numIdMidia.Value = 0;
                cmbTipoMidia.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Configures the form for edit mode
        /// </summary>
        private void ConfigureEditMode()
        {
            if (_midiaExistente == null) return;

            lblTitulo.Text = "Edit Media";
            Text = "Edit Media";

            // Select the current type
            for (int i = 0; i < cmbTipoMidia.Items.Count; i++)
            {
                if (cmbTipoMidia.Items[i] is TipoMidiaItem item && item.Valor == _midiaExistente.TypeAlias)
                {
                    cmbTipoMidia.SelectedIndex = i;
                    break;
                }
            }

            // Fill ID (cannot be changed in edit mode)
            numIdMidia.Value = _midiaExistente.MediaId;
            numIdMidia.Enabled = false;
            lblIdInfo.Text = "Media ID cannot be changed in edit mode.";

            // Fill data
            txtDadosMidia.Text = _midiaExistente.DescriptionAlias;
        }

        /// <summary>
        /// Loads the available media types into the ComboBox
        /// </summary>
        private void LoadMediaTypes()
        {
            cmbTipoMidia.Items.Clear();

            // RFID Wiegand 26 bits - accepts: WIEGAND format, HEX, CODE Smart
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "RFID Wiegand 26",
                Valor = MediaType.Wiegand26,
                Exemplo = "123,45678",
                DescricaoFormato = "Accepted formats:\n" +
                                   "- Wiegand: 123,45678 (facility,code)\n" +
                                   "- HEX: FF FF FF or FFFFFF\n" +
                                   "- CODE Smart: 12345,123,12345"
            });

            // RFID Wiegand 34 bits
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "RFID Wiegand 34",
                Valor = MediaType.Wiegand34,
                Exemplo = "1234,567890",
                DescricaoFormato = "Accepted formats:\n" +
                                   "- Wiegand: 1234,567890 (facility,code)\n" +
                                   "- HEX: FF FF FF FF\n" +
                                   "- CODE Smart: 12345,123,12345"
            });

            // LPR Plate
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Plate (LPR)",
                Valor = MediaType.Lpr,
                Exemplo = "ABC1234",
                DescricaoFormato = "Format: Vehicle plate\n" +
                                   "- Old format: ABC1234\n" +
                                   "- Mercosul: ABC1D23"
            });

            // Facial Recognition
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Facial",
                Valor = MediaType.Facial,
                Exemplo = "FACE001",
                DescricaoFormato = "Format: Facial identifier\n" +
                                   "- Example: FACE001, ID12345"
            });

            // Biometry
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Biometry",
                Valor = MediaType.Bio,
                Exemplo = "BIO001",
                DescricaoFormato = "Format: Biometry ID\n" +
                                   "- Example: BIO001, FP12345"
            });

            // Biometry Hikvision
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Biometry Hikvision",
                Valor = MediaType.BioHikvision,
                Exemplo = "HIK001",
                DescricaoFormato = "Format: Hikvision ID\n" +
                                   "- Example: HIK001, HK12345"
            });

            // Password/Keypad
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Password/Keypad",
                Valor = MediaType.Keyboard,
                Exemplo = "123456",
                DescricaoFormato = "Format: Numeric code\n" +
                                   "- 4 to 10 numeric digits"
            });

            // Remote Control
            cmbTipoMidia.Items.Add(new TipoMidiaItem
            {
                Nome = "Remote Control",
                Valor = MediaType.RemoteControl,
                Exemplo = "CTRL001",
                DescricaoFormato = "Format: Control ID\n" +
                                   "- Example: CTRL001, HT001"
            });

            cmbTipoMidia.DisplayMember = "Nome";
            cmbTipoMidia.ValueMember = "Valor";
        }

        /// <summary>
        /// Updates the format description when the type is changed
        /// </summary>
        private void cmbTipoMidia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoMidia.SelectedItem is TipoMidiaItem tipo)
            {
                lblFormatoAtual.Text = tipo.DescricaoFormato;
                toolTip.SetToolTip(txtDadosMidia, tipo.DescricaoFormato);

                // For LPR, if we already have the entity plate, reuse it without asking again.
                if (tipo.Valor == MediaType.Lpr && !string.IsNullOrWhiteSpace(_placaPadraoLpr))
                {
                    txtDadosMidia.Text = _placaPadraoLpr!.Trim().ToUpper().Replace("-", "");
                    txtDadosMidia.ReadOnly = true;
                    lblExemploFormato.Text = "Plate automatically filled from the vehicle.";
                }
                else
                {
                    txtDadosMidia.ReadOnly = false;
                    lblExemploFormato.Text = "Format example:";
                }
            }
        }

        /// <summary>
        /// Validates and saves the media data
        /// </summary>
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validate selected type
            if (cmbTipoMidia.SelectedItem == null)
            {
                MessageBox.Show("Select a media type.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoMidia.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            // Validate media data
            if (string.IsNullOrWhiteSpace(txtDadosMidia.Text))
            {
                MessageBox.Show("Enter the media data.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDadosMidia.Focus();
                DialogResult = DialogResult.None;
                return;
            }

            var tipo = (TipoMidiaItem)cmbTipoMidia.SelectedItem;
            TipoMidiaSelecionado = tipo.Valor;
            TipoMidiaNome = tipo.Nome;

            // Store the data (uppercase for RFID/Plates)
            IdMidia = (uint)numIdMidia.Value;
            DadosMidia = txtDadosMidia.Text.Trim().ToUpper();

            // In creation mode with ID 0, confirm automatic generation
            if (!ModoEdicao && IdMidia == 0)
            {
                var result = MessageBox.Show(
                    "The ID is set to 0 (zero).\n\n" +
                    "The server will generate the ID automatically.\n\n" +
                    "Do you want to continue?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numIdMidia.Focus();
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Helper class to represent a media type in the ComboBox
        /// </summary>
        private class TipoMidiaItem
        {
            public string Nome { get; set; } = string.Empty;
            public int Valor { get; set; }
            public string Exemplo { get; set; } = string.Empty;
            public string DescricaoFormato { get; set; } = string.Empty;

            public override string ToString()
            {
                return Nome;
            }
        }
    }
}
