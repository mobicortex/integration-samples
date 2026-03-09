using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Formulário de edição de Pessoa (Entidade tipo 1).
    /// </summary>
    public partial class FormCadastroPessoaEdit : Form
    {
        // Dados da pessoa (preenchidos ao salvar)
        public string Nome { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool EntidadeEnabled { get; private set; } = true;
        
        // Dados para edição
        private readonly Entidade _entidade;
        
        /// <summary>
        /// Construtor para editar pessoa existente
        /// </summary>
        public FormCadastroPessoaEdit(Entidade entidade)
        {
            _entidade = entidade;
            InitializeComponent();
        }
        
        private void FormCadastroPessoaEdit_Load(object? sender, EventArgs e)
        {
            // Preenche campos com dados da entidade
            lblEntityId.Text = _entidade.EntityId.ToString();
            txtNome.Text = _entidade.Name;
            txtDocumento.Text = _entidade.Doc;
            
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Carregando entidade ID={_entidade.EntityId}, Enabled={_entidade.Enabled}");
            chkHabilitado.Checked = _entidade.Enabled;
            
            // Mostra datas
            lblCreatedAt.Text = _entidade.CriadoEm;
            lblUpdatedAt.Text = _entidade.AtualizadoEm;
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
            Nome = txtNome.Text.Trim();
            Documento = txtDocumento.Text.Trim();
            
            EntidadeEnabled = chkHabilitado.Checked;
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Salvando entidade - Nome={Nome}, Enabled={EntidadeEnabled}, Checked={chkHabilitado.Checked}");
            
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
