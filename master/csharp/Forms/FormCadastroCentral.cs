using SmartSdk.Models;

namespace SmartSdk.Forms
{
    /// <summary>
    /// Formulário de cadastro/edição de Cadastro Central.
    /// 
    /// O Cadastro Central é o nó raiz da hierarquia:
    /// Cadastro Central → Entidades → Mídias
    /// </summary>
    public partial class FormCadastroCentral : Form
    {
        // Dados do cadastro (preenchidos ao salvar)
        public uint IdCadastro { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public int Tipo { get; private set; }
        public int Vagas { get; private set; }
        
        // Modo edição
        public bool ModoEdicao { get; private set; }
        
        // Dados para edição
        private readonly CadastroCentral? _cadastroExistente;
        
        /// <summary>
        /// Construtor para criar novo cadastro
        /// </summary>
        public FormCadastroCentral()
        {
            InitializeComponent();
            ModoEdicao = false;
        }
        
        /// <summary>
        /// Construtor para editar cadastro existente
        /// </summary>
        public FormCadastroCentral(CadastroCentral cadastro)
        {
            InitializeComponent();
            _cadastroExistente = cadastro;
            ModoEdicao = true;
        }
        
        private void FormCadastroCentral_Load(object? sender, EventArgs e)
        {
            if (ModoEdicao && _cadastroExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Modo criação - ID 0 por padrão (servidor gera automaticamente)
                numIdCadastro.Value = 0;
                numTipo.Value = 0;
                numVagas.Value = 0;
            }
        }
        
        /// <summary>
        /// Configura o formulário para modo de edição
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_cadastroExistente == null) return;
            
            lblTitulo.Text = "Editar Cadastro Central";
            Text = "Editar Cadastro Central";
            
            // Preenche campos
            numIdCadastro.Value = _cadastroExistente.Id;
            numIdCadastro.Enabled = false; // ID não pode ser alterado
            lblIdInfo.Text = "💡 ID do cadastro não pode ser alterado em modo de edição.";
            
            txtNome.Text = _cadastroExistente.Name;
            numTipo.Value = _cadastroExistente.Type;
            numVagas.Value = _cadastroExistente.Vagas;
        }
        
        /// <summary>
        /// Valida e salva os dados do cadastro
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Valida nome
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do cadastro.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Armazena os dados
            IdCadastro = (uint)numIdCadastro.Value;
            Nome = txtNome.Text.Trim();
            Tipo = (int)numTipo.Value;
            Vagas = (int)numVagas.Value;
            
            // Em modo criação com ID 0, confirma geração automática
            if (!ModoEdicao && IdCadastro == 0)
            {
                var result = MessageBox.Show(
                    "O ID não foi informado (valor 0).\n\n" +
                    "O código será gerado automaticamente pela controladora.\n\n" +
                    "Deseja continuar?",
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numIdCadastro.Focus();
                    return;
                }
            }
            
            DialogResult = DialogResult.OK;
        }
    }
}
