using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de cadastro/edição de Cadastro Central (Unidade).
    /// </summary>
    public partial class FormCadastroCentral : Form
    {
        // Dados do cadastro (preenchidos ao salvar)
        public uint IdCadastro { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string? Field1 { get; private set; }
        public string? Field2 { get; private set; }
        public string? Field3 { get; private set; }
        public string? Field4 { get; private set; }
        public bool CadastroEnabled { get; private set; } = true;
        
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
            // O checkbox na UI significa "cadastro ativo".
            chkBloqueado.Checked = true;
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
            System.Diagnostics.Debug.WriteLine($"FormCadastroCentral_Load: ModoEdicao={ModoEdicao}, _cadastroExistente={_cadastroExistente}");
            
            if (ModoEdicao && _cadastroExistente != null)
            {
                System.Diagnostics.Debug.WriteLine($"Configurando modo edição: ID={_cadastroExistente.Id}, Nome={_cadastroExistente.Name}, Enabled={_cadastroExistente.Enabled}");
                ConfigurarModoEdicao();
            }
            else
            {
                // Modo criação - valores padrão
                numId.Value = 0;
                chkBloqueado.Checked = true;
                lblIdInfo.Text = "0 = geracao automatica pelo servidor";
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
            numId.Value = _cadastroExistente.Id;
            numId.Enabled = false; // ID não pode ser alterado
            lblIdInfo.Text = "ID do cadastro nao pode ser alterado em modo de edicao.";
            
            txtNome.Text = _cadastroExistente.Name;
            txtField1.Text = _cadastroExistente.Field1 ?? "";
            txtField2.Text = _cadastroExistente.Field2 ?? "";
            txtField3.Text = _cadastroExistente.Field3 ?? "";
            txtField4.Text = _cadastroExistente.Field4 ?? "";
            
            chkBloqueado.Checked = _cadastroExistente.Enabled;
        }
        
        /// <summary>
        /// Valida e salva os dados do cadastro
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Valida nome
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do cadastro.", "Validacao", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Armazena os dados
            IdCadastro = (uint)numId.Value;
            Nome = txtNome.Text.Trim();
            Field1 = string.IsNullOrWhiteSpace(txtField1.Text) ? null : txtField1.Text.Trim();
            Field2 = string.IsNullOrWhiteSpace(txtField2.Text) ? null : txtField2.Text.Trim();
            Field3 = string.IsNullOrWhiteSpace(txtField3.Text) ? null : txtField3.Text.Trim();
            Field4 = string.IsNullOrWhiteSpace(txtField4.Text) ? null : txtField4.Text.Trim();
            CadastroEnabled = chkBloqueado.Checked;
            
            // Em modo criação com ID 0, confirma geração automática
            if (!ModoEdicao && IdCadastro == 0)
            {
                var result = MessageBox.Show(
                    "O ID nao foi informado (valor 0).\n\n" +
                    "O codigo sera gerado automaticamente pela controladora.\n\n" +
                    "Deseja continuar?",
                    "Confirmacao", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numId.Focus();
                    return;
                }
            }
            
            DialogResult = DialogResult.OK;
        }
    }
}
