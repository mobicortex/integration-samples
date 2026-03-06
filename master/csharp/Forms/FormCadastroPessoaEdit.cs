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
        public int LprAtivo { get; private set; }
        public int Habilitado { get; private set; } = 1;
        
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
            chkLprAtivo.Checked = _entidade.LprAtivo == 1;
            chkHabilitado.Checked = _entidade.Habilitado == 1;
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
            LprAtivo = chkLprAtivo.Checked ? 1 : 0;
            Habilitado = chkHabilitado.Checked ? 1 : 0;
            
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
