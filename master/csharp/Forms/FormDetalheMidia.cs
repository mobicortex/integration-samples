using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulário para exibir e editar detalhes de uma mídia de acesso.
    /// </summary>
    public partial class FormDetalheMidia : Form
    {
        private readonly MidiaAcesso _midia;
        private uint _dtBlockOriginal;

        /// <summary>
        /// Retorna true se a mídia foi modificada.
        /// </summary>
        public bool FoiModificada { get; private set; }

        /// <summary>
        /// Retorna o novo estado de habilitação (1=habilitada, 0=bloqueada).
        /// </summary>
        public int NovoEstadoHabilitado => chkBloqueada.Checked ? 0 : 1;

        /// <summary>
        /// Retorna a data limite de permissão (0 = sem limite).
        /// </summary>
        public uint NovaDataPermissao => chkBloqueioPorData.Checked && dtpDataBloqueio.Enabled
            ? (uint)new DateTimeOffset(dtpDataBloqueio.Value).ToUnixTimeSeconds()
            : 0;

        /// <summary>
        /// Retorna true se a data de permissão foi alterada.
        /// </summary>
        public bool DataPermissaoAlterada => NovaDataPermissao != _dtBlockOriginal;

        public FormDetalheMidia(MidiaAcesso midia)
        {
            InitializeComponent();
            _midia = midia;
            _dtBlockOriginal = midia.DtBlock;
            CarregarDados();
        }

        private void CarregarDados()
        {
            // Preenche informações
            lblIdValor.Text = _midia.MediaId.ToString();
            lblTipoValor.Text = _midia.TipoNome;
            lblDescricaoValor.Text = _midia.Descricao;
            lblDataCadastroValor.Text = _midia.CriadoEm;
            lblDataEdicaoValor.Text = _midia.AtualizadoEm;

            // Configura checkbox de bloqueio total
            // Habilitado=1 significa liberada, Habilitado=0 significa bloqueada
            chkBloqueada.Checked = _midia.Habilitado == 0;

            // Configura data de permissão (dt_block = data em que a mídia será bloqueada)
            if (_midia.DtBlock > 0)
            {
                chkBloqueioPorData.Checked = true;
                dtpDataBloqueio.Enabled = true;
                btnLimparData.Enabled = true;
                dtpDataBloqueio.Value = DateTimeOffset.FromUnixTimeSeconds(_midia.DtBlock).LocalDateTime;
            }
            else
            {
                chkBloqueioPorData.Checked = false;
                dtpDataBloqueio.Enabled = false;
                btnLimparData.Enabled = false;
                dtpDataBloqueio.Value = DateTime.Now.AddMonths(1); // Default: 1 mês
            }

            // Atualiza título
            Text = $"Detalhes da Midia - {_midia.Descricao}";
        }

        private void chkBloqueioPorData_CheckedChanged(object sender, EventArgs e)
        {
            bool habilitado = chkBloqueioPorData.Checked;
            dtpDataBloqueio.Enabled = habilitado;
            btnLimparData.Enabled = habilitado;

            if (habilitado && dtpDataBloqueio.Value <= DateTime.Now)
            {
                // Se ativou e a data está no passado, ajusta para 1 mês no futuro
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
            // Verifica se houve alteração no estado
            int estadoAnterior = _midia.Habilitado;
            int estadoNovo = chkBloqueada.Checked ? 0 : 1;

            bool estadoAlterado = estadoAnterior != estadoNovo;
            bool dataPermissaoAlterada = DataPermissaoAlterada;

            FoiModificada = estadoAlterado || dataPermissaoAlterada;

            // Validação: não pode ter data de permissão no passado
            if (chkBloqueioPorData.Checked && dtpDataBloqueio.Value <= DateTime.Now)
            {
                MessageBox.Show("A data de permissao deve ser futura.", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
