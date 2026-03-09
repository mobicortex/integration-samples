using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de cadastro/edição de Entidade (Pessoa ou Veículo).
    /// 
    /// Hierarquia:
    /// Cadastro Central → Entidade → Mídias
    /// </summary>
    public partial class FormCadastroEntidade : Form
    {
        // Dados da entidade (preenchidos ao salvar)
        public uint CadastroId { get; private set; }
        public uint EntityId { get; private set; }
        public int TipoEntidade { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool LprAtivo { get; private set; }
        
        // Modo edição
        public bool ModoEdicao { get; private set; }
        public bool IsPessoa => TipoEntidade == (int)MobiCortex.Sdk.Models.TipoEntidade.Pessoa;
        public bool IsVeiculo => TipoEntidade == (int)MobiCortex.Sdk.Models.TipoEntidade.Veiculo;
        
        // Dados para edição
        private readonly Entidade? _entidadeExistente;
        private uint? _cadastroIdPadrao;
        
        /// <summary>
        /// Construtor padrão para o Designer do Visual Studio.
        /// </summary>
        public FormCadastroEntidade()
        {
            InitializeComponent();
            _cadastroIdPadrao = 0;
            ModoEdicao = false;
        }
        
        /// <summary>
        /// Construtor para criar nova entidade
        /// </summary>
        /// <param name="cadastroId">ID do cadastro central vinculado</param>
        public FormCadastroEntidade(uint cadastroId) : this()
        {
            _cadastroIdPadrao = cadastroId;
            ModoEdicao = false;
        }
        
        /// <summary>
        /// Construtor para editar entidade existente
        /// </summary>
        public FormCadastroEntidade(Entidade entidade) : this()
        {
            _entidadeExistente = entidade;
            _cadastroIdPadrao = entidade.CadastroId;
            ModoEdicao = true;
        }
        
        private void FormCadastroEntidade_Load(object? sender, EventArgs e)
        {
            CarregarTiposEntidade();
            
            // Exibe o ID do cadastro central
            uint cadId = _cadastroIdPadrao ?? 0;
            CadastroId = cadId;
            lblCadastroIdValor.Text = cadId.ToString();
            
            if (ModoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Modo criação - valores padrão
                numIdEntidade.Value = 0; // Servidor gera automaticamente
                cmbTipoEntidade.SelectedIndex = 0; // Pessoa
                chkLprAtivo.Checked = false;
            }
        }
        
        /// <summary>
        /// Carrega os tipos de entidade no ComboBox
        /// </summary>
        private void CarregarTiposEntidade()
        {
            cmbTipoEntidade.Items.Clear();
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem 
            { 
                Nome = "Pessoa", 
                Valor = (int)MobiCortex.Sdk.Models.TipoEntidade.Pessoa 
            });
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem 
            { 
                Nome = "Veículo", 
                Valor = (int)MobiCortex.Sdk.Models.TipoEntidade.Veiculo 
            });
            cmbTipoEntidade.Items.Add(new TipoEntidadeItem 
            { 
                Nome = "Animal", 
                Valor = (int)MobiCortex.Sdk.Models.TipoEntidade.Animal 
            });
            
            cmbTipoEntidade.DisplayMember = "Nome";
            cmbTipoEntidade.ValueMember = "Valor";
        }
        
        /// <summary>
        /// Configura o formulário para modo de edição
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_entidadeExistente == null) return;
            
            lblTitulo.Text = "Editar Entidade";
            Text = "Editar Entidade";
            
            // Seleciona o tipo atual
            for (int i = 0; i < cmbTipoEntidade.Items.Count; i++)
            {
                if (cmbTipoEntidade.Items[i] is TipoEntidadeItem item && 
                    item.Valor == _entidadeExistente.Tipo)
                {
                    cmbTipoEntidade.SelectedIndex = i;
                    break;
                }
            }
            cmbTipoEntidade.Enabled = false; // Tipo não pode ser alterado
            
            // Preenche ID (não pode ser alterado em edição)
            numIdEntidade.Value = _entidadeExistente.EntityId;
            numIdEntidade.Enabled = false;
            lblIdInfo.Text = "💡 ID da entidade não pode ser alterado em modo de edição.";
            
            // Preenche campos
            txtNome.Text = _entidadeExistente.Name;
            txtDocumento.Text = _entidadeExistente.Doc;
            chkLprAtivo.Checked = _entidadeExistente.LprAtivo;
            
            AtualizarLabelsPorTipo();
        }
        
        /// <summary>
        /// Atualiza labels e visibilidade quando o tipo muda
        /// </summary>
        private void cmbTipoEntidade_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbTipoEntidade.SelectedItem is TipoEntidadeItem tipo)
            {
                TipoEntidade = tipo.Valor;
                AtualizarLabelsPorTipo();
            }
        }
        
        /// <summary>
        /// Atualiza labels de acordo com o tipo de entidade selecionado
        /// </summary>
        private void AtualizarLabelsPorTipo()
        {
            if (IsPessoa)
            {
                grpDocumento.Text = "Documento (CPF)";
                lblDocumento.Text = "CPF:";
                lblDocInfo.Text = "Opcional - apenas para referência";
                chkLprAtivo.Enabled = false;
                chkLprAtivo.Checked = false;
                grpLpr.Enabled = false;
            }
            else if (IsVeiculo)
            {
                grpDocumento.Text = "Placa do Veículo";
                lblDocumento.Text = "Placa:";
                lblDocInfo.Text = "Ex: ABC1234 ou ABC1D23 (Mercosul)";
                chkLprAtivo.Enabled = true;
                grpLpr.Enabled = true;
            }
            else // Animal
            {
                grpDocumento.Text = "Identificação";
                lblDocumento.Text = "ID/Chip:";
                lblDocInfo.Text = "Identificação do animal";
                chkLprAtivo.Enabled = false;
                chkLprAtivo.Checked = false;
                grpLpr.Enabled = false;
            }
        }
        
        /// <summary>
        /// Valida e salva os dados da entidade
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Valida tipo selecionado
            if (cmbTipoEntidade.SelectedItem == null)
            {
                MessageBox.Show("Selecione o tipo de entidade.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoEntidade.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Valida nome
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome da entidade.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Para veículos, valida placa se informada
            if (IsVeiculo && !string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                var placa = txtDocumento.Text.Trim().ToUpper().Replace("-", "");
                if (!System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$") &&
                    !System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}[0-9]{4}$"))
                {
                    MessageBox.Show("Placa inválida.\nFormatos aceitos: ABC1234 ou ABC1D23", 
                        "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDocumento.Focus();
                    DialogResult = DialogResult.None;
                    return;
                }
            }
            
            // Armazena os dados
            TipoEntidade = ((TipoEntidadeItem)cmbTipoEntidade.SelectedItem).Valor;
            EntityId = (uint)numIdEntidade.Value;
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim().ToUpper();
            LprAtivo = chkLprAtivo.Checked;
            
            // Em modo criação com ID 0, confirma geração automática
            if (!ModoEdicao && EntityId == 0)
            {
                var result = MessageBox.Show(
                    "O ID da entidade não foi informado (valor 0).\n\n" +
                    "O servidor gerará o ID automaticamente.\n\n" +
                    "Deseja continuar?",
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
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
        /// Classe auxiliar para representar um tipo de entidade no ComboBox
        /// </summary>
        private class TipoEntidadeItem
        {
            public string Nome { get; set; } = string.Empty;
            public int Valor { get; set; }
            
            public override string ToString() => Nome;
        }
    }
}
