using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de cadastro/edição de Pessoa (Entidade tipo 1).
    /// </summary>
    public partial class FormCadastroPessoa : Form
    {
        // Dados da pessoa (preenchidos ao salvar)
        public uint Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public int LprAtivo { get; private set; }
        public int EntidadeHabilitado { get; private set; } = 1;
        
        // Modo edição
        public bool ModoEdicao { get; private set; }
        
        // Dados para edição
        private readonly Entidade? _entidadeExistente;
        private readonly uint _cadastroIdPadrao;
        
        /// <summary>
        /// Construtor para criar nova pessoa
        /// </summary>
        public FormCadastroPessoa(uint cadastroId)
        {
            _cadastroIdPadrao = cadastroId;
            InitializeComponent();
            ModoEdicao = false;
        }
        
        /// <summary>
        /// Construtor para editar pessoa existente
        /// </summary>
        public FormCadastroPessoa(Entidade entidade)
        {
            _entidadeExistente = entidade;
            _cadastroIdPadrao = entidade.CadastroId;
            InitializeComponent();
            ModoEdicao = true;
        }
        
        private void FormCadastroPessoa_Load(object? sender, EventArgs e)
        {
            if (ModoEdicao && _entidadeExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Modo criação - valores padrão
                numId.Value = 0;
                chkHabilitado.Checked = true;
                chkLprAtivo.Checked = false;
                lblIdInfo.Text = "0 = geração automática pelo servidor";
            }
        }
        
        /// <summary>
        /// Configura o formulário para modo de edição
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_entidadeExistente == null) return;
            
            lblTitulo.Text = "Editar Pessoa";
            Text = "Editar Pessoa";
            
            // Preenche campos
            numId.Value = _entidadeExistente.EntityId;
            numId.Enabled = false; // ID não pode ser alterado
            lblIdInfo.Text = "ID da entidade não pode ser alterado em modo de edição.";
            
            txtNome.Text = _entidadeExistente.Name;
            txtDocumento.Text = _entidadeExistente.Doc;
            chkLprAtivo.Checked = _entidadeExistente.LprAtivo == 1;
            chkHabilitado.Checked = _entidadeExistente.Habilitado == 1; // Propriedade Habilitado da entidade (int)
        }
        
        /// <summary>
        /// Valida e salva os dados da pessoa
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Valida nome
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome da pessoa.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Armazena os dados
            Id = (uint)numId.Value;
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim();
            LprAtivo = chkLprAtivo.Checked ? 1 : 0;
            EntidadeHabilitado = chkHabilitado.Checked ? 1 : 0;
            
            // Em modo criação com ID 0, confirma geração automática
            if (!ModoEdicao && Id == 0)
            {
                lblIdInfo.Text = "Será gerado automaticamente pelo servidor";
            }
            
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
