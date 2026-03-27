using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    /// <summary>
    /// Simple dialog for choosing the type of entity to create.
    /// </summary>
    public partial class FormSelecionarTipoEntidade : Form
    {
        public int TipoEntidadeSelecionado =>
            _rbPessoa.Checked ? (int)EntityType.Person : (int)EntityType.Vehicle;

        public FormSelecionarTipoEntidade()
        {
            InitializeComponent();

            // Event handlers for cards and labels
            _cardPessoa.Click += (s, e) => SelectType(true);
            _cardVeiculo.Click += (s, e) => SelectType(false);
            lblPessoaDesc.Click += (s, e) => SelectType(true);
            lblVeiculoDesc.Click += (s, e) => SelectType(false);

            // Event handlers for RadioButtons - synchronize selection
            _rbPessoa.Click += (s, e) => SelectType(true);
            _rbVeiculo.Click += (s, e) => SelectType(false);

            // Update initial colors
            UpdateCards();
        }

        private void SelectType(bool person)
        {
            // Set the radio button state (manual synchronization)
            // This is necessary because the radio buttons are in different containers
            _rbPessoa.Checked = person;
            _rbVeiculo.Checked = !person;

            // Update card colors
            UpdateCards();
        }

        private void UpdateCards()
        {
            var selected = Color.FromArgb(240, 248, 255);
            var normal = Color.White;
            _cardPessoa.BackColor = _rbPessoa.Checked ? selected : normal;
            _cardVeiculo.BackColor = _rbVeiculo.Checked ? selected : normal;
        }
    }
}
