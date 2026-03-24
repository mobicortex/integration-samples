using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Diálogo simples para escolher o tipo da entidade a ser criada.
    /// </summary>
    public partial class FormSelecionarTipoEntidade : Form
    {
        public int TipoEntidadeSelecionado =>
            _rbPessoa.Checked ? (int)TipoEntidade.Pessoa : (int)TipoEntidade.Veiculo;

        public FormSelecionarTipoEntidade()
        {
            InitializeComponent();
            
            // Event handlers dos cards e labels
            _cardPessoa.Click += (s, e) => SelecionarTipo(true);
            _cardVeiculo.Click += (s, e) => SelecionarTipo(false);
            lblPessoaDesc.Click += (s, e) => SelecionarTipo(true);
            lblVeiculoDesc.Click += (s, e) => SelecionarTipo(false);
            
            // Event handlers dos RadioButtons - sincroniza a seleção
            _rbPessoa.Click += (s, e) => SelecionarTipo(true);
            _rbVeiculo.Click += (s, e) => SelecionarTipo(false);
            
            // Atualiza cores iniciais
            AtualizarCards();
        }

        private void SelecionarTipo(bool pessoa)
        {
            // Define o estado dos radio buttons (sincronização manual)
            // Isso é necessário porque os radio buttons estão em containers diferentes
            _rbPessoa.Checked = pessoa;
            _rbVeiculo.Checked = !pessoa;
            
            // Atualiza as cores dos cards
            AtualizarCards();
        }

        private void AtualizarCards()
        {
            var selected = Color.FromArgb(240, 248, 255);
            var normal = Color.White;
            _cardPessoa.BackColor = _rbPessoa.Checked ? selected : normal;
            _cardVeiculo.BackColor = _rbVeiculo.Checked ? selected : normal;
        }
    }
}
