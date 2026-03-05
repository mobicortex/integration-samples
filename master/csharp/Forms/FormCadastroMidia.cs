using SmartSdk.Models;

namespace SmartSdk.Forms
{
    /// <summary>
    /// Formulário de cadastro/edição de mídias de acesso.
    /// 
    /// FORMATOS ACEITOS PELO BACKEND (ws_media6.cpp):
    /// - WIEGAND: "123,45678" (facility,code) - máx: 65535,65535
    /// - CODE SMART: "12345,123,12345" (5,3,5 dígitos)
    /// - HEX: "HEX: FF FF FF" ou "FFFFFF" (3 bytes)
    /// 
    /// O backend detecta automaticamente o formato e converte para ns32.
    /// </summary>
    public partial class FormCadastroMidia : Form
    {
        // Dados da mídia (preenchidos ao salvar)
        public int TipoMidiaSelecionado { get; private set; }
        public uint IdMidia { get; private set; }
        public string DadosMidia { get; private set; } = string.Empty;
        public string TipoMidiaNome { get; private set; } = string.Empty;
        
        // Modo edição
        public bool ModoEdicao { get; private set; }
        
        // Dados para edição (opcional)
        private readonly MidiaAcesso? _midiaExistente;
        private readonly uint? _entityIdPadrao;
        
        /// <summary>
        /// Construtor para criar nova mídia
        /// </summary>
        public FormCadastroMidia()
        {
            InitializeComponent();
            ModoEdicao = false;
        }
        
        /// <summary>
        /// Construtor para editar mídia existente
        /// </summary>
        public FormCadastroMidia(MidiaAcesso midia)
        {
            InitializeComponent();
            _midiaExistente = midia;
            ModoEdicao = true;
        }
        
        /// <summary>
        /// Construtor com entity_id padrão pré-selecionado
        /// </summary>
        public FormCadastroMidia(uint entityIdPadrao)
        {
            InitializeComponent();
            _entityIdPadrao = entityIdPadrao;
            ModoEdicao = false;
        }
        
        private void FormCadastroMidia_Load(object? sender, EventArgs e)
        {
            CarregarTiposMidia();
            
            if (ModoEdicao && _midiaExistente != null)
            {
                ConfigurarModoEdicao();
            }
            else
            {
                // Modo criação - ID 0 por padrão (servidor gera automaticamente)
                numIdMidia.Value = 0;
                cmbTipoMidia.SelectedIndex = 0;
            }
        }
        
        /// <summary>
        /// Configura o formulário para modo de edição
        /// </summary>
        private void ConfigurarModoEdicao()
        {
            if (_midiaExistente == null) return;
            
            lblTitulo.Text = "Editar Mídia";
            Text = "Editar Mídia";
            
            // Seleciona o tipo atual
            for (int i = 0; i < cmbTipoMidia.Items.Count; i++)
            {
                if (cmbTipoMidia.Items[i] is TipoMidiaItem item && item.Valor == _midiaExistente.Tipo)
                {
                    cmbTipoMidia.SelectedIndex = i;
                    break;
                }
            }
            
            // Preenche ID (não pode ser alterado em edição)
            numIdMidia.Value = _midiaExistente.MediaId;
            numIdMidia.Enabled = false;
            lblIdInfo.Text = "💡 ID da mídia não pode ser alterado em modo de edição.";
            
            // Preenche dados
            txtDadosMidia.Text = _midiaExistente.Descricao;
        }
        
        /// <summary>
        /// Carrega os tipos de mídia disponíveis no ComboBox
        /// </summary>
        private void CarregarTiposMidia()
        {
            cmbTipoMidia.Items.Clear();
            
            // RFID Wiegand 26 bits - aceita: WIEGAND formato, HEX, CODE Smart
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "RFID Wiegand 26", 
                Valor = TipoMidia.Wiegand26,
                Exemplo = "123,45678",
                DescricaoFormato = "Formatos aceitos:\n" +
                                   "• Wiegand: 123,45678 (facility,code)\n" +
                                   "• HEX: FF FF FF ou FFFFFF\n" +
                                   "• CODE Smart: 12345,123,12345"
            });
            
            // RFID Wiegand 34 bits
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "RFID Wiegand 34", 
                Valor = TipoMidia.Wiegand34,
                Exemplo = "1234,567890",
                DescricaoFormato = "Formatos aceitos:\n" +
                                   "• Wiegand: 1234,567890 (facility,code)\n" +
                                   "• HEX: FF FF FF FF\n" +
                                   "• CODE Smart: 12345,123,12345"
            });
            
            // Placa LPR
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Placa (LPR)", 
                Valor = TipoMidia.Lpr,
                Exemplo = "ABC1234",
                DescricaoFormato = "Formato: Placa do veículo\n" +
                                   "• Modelo antigo: ABC1234\n" +
                                   "• Mercosul: ABC1D23"
            });
            
            // Reconhecimento Facial
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Facial", 
                Valor = TipoMidia.Facial,
                Exemplo = "FACE001",
                DescricaoFormato = "Formato: Identificador facial\n" +
                                   "• Exemplo: FACE001, ID12345"
            });
            
            // Biometria
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Biometria", 
                Valor = TipoMidia.Bio,
                Exemplo = "BIO001",
                DescricaoFormato = "Formato: ID da biometria\n" +
                                   "• Exemplo: BIO001, FP12345"
            });
            
            // Biometria Hikvision
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Biometria Hikvision", 
                Valor = TipoMidia.BioHikvision,
                Exemplo = "HIK001",
                DescricaoFormato = "Formato: ID Hikvision\n" +
                                   "• Exemplo: HIK001, HK12345"
            });
            
            // Senha/Teclado
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Senha/Teclado", 
                Valor = TipoMidia.Teclado,
                Exemplo = "123456",
                DescricaoFormato = "Formato: Código numérico\n" +
                                   "• 4 a 10 dígitos numéricos"
            });
            
            // Controle Remoto
            cmbTipoMidia.Items.Add(new TipoMidiaItem 
            { 
                Nome = "Controle Remoto", 
                Valor = TipoMidia.ControleRemoto,
                Exemplo = "CTRL001",
                DescricaoFormato = "Formato: ID do controle\n" +
                                   "• Exemplo: CTRL001, HT001"
            });
            
            cmbTipoMidia.DisplayMember = "Nome";
            cmbTipoMidia.ValueMember = "Valor";
        }
        
        /// <summary>
        /// Atualiza a descrição do formato quando o tipo é alterado
        /// </summary>
        private void cmbTipoMidia_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbTipoMidia.SelectedItem is TipoMidiaItem tipo)
            {
                lblFormatoAtual.Text = tipo.DescricaoFormato;
                toolTip.SetToolTip(txtDadosMidia, tipo.DescricaoFormato);
                
                // Sugere um valor de exemplo no placeholder
                txtDadosMidia.PlaceholderText = $"Ex: {tipo.Exemplo}";
            }
        }
        
        /// <summary>
        /// Valida e salva os dados da mídia
        /// </summary>
        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Valida tipo selecionado
            if (cmbTipoMidia.SelectedItem == null)
            {
                MessageBox.Show("Selecione um tipo de mídia.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoMidia.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            // Valida dados da mídia
            if (string.IsNullOrWhiteSpace(txtDadosMidia.Text))
            {
                MessageBox.Show("Informe os dados da mídia.", "Validação", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDadosMidia.Focus();
                DialogResult = DialogResult.None;
                return;
            }
            
            var tipo = (TipoMidiaItem)cmbTipoMidia.SelectedItem;
            TipoMidiaSelecionado = tipo.Valor;
            TipoMidiaNome = tipo.Nome;
            
            // Armazena os dados (maiúsculo para RFID/Placas)
            IdMidia = (uint)numIdMidia.Value;
            DadosMidia = txtDadosMidia.Text.Trim().ToUpper();
            
            // Em modo criação com ID 0, confirma geração automática
            if (!ModoEdicao && IdMidia == 0)
            {
                var result = MessageBox.Show(
                    "O ID está configurado como 0 (zero).\n\n" +
                    "O servidor gerará o ID automaticamente.\n\n" +
                    "Deseja continuar?",
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                if (result != DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                    numIdMidia.Focus();
                    return;
                }
            }
            
            DialogResult = DialogResult.OK;
        }
        
        /// <summary>
        /// Classe auxiliar para representar um tipo de mídia no ComboBox
        /// </summary>
        private class TipoMidiaItem
        {
            public string Nome { get; set; } = string.Empty;
            public int Valor { get; set; }
            public string Exemplo { get; set; } = string.Empty;
            public string DescricaoFormato { get; set; } = string.Empty;
            
            public override string ToString() => Nome;
        }
    }
}
