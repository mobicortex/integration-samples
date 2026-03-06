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
