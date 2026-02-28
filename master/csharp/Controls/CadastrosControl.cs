using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para gerenciamento de cadastros principais via Central Registry
    /// </summary>
    public class CadastrosControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        private bool _isConnected = false;
        
        // Controles da interface
        private ListBox _lstUsuarios = null!;
        private Button _btnBuscar = null!;
        private Button _btnAnterior = null!;
        private Button _btnProximo = null!;
        private Button _btnNovo = null!;
        private Button _btnEditar = null!;
        private Button _btnApagar = null!;
        private TextBox _txtBusca = null!;
        private Label _lblInfo = null!;
        private Label _lblEstatisticas = null!;
        private Label _lblPaginacao = null!;
        
        // Paginação
        private int _offset = 0;
        private int _count = 10;
        private int _total = 0;
        private string? _filtroNome = null;

        public CadastrosControl()
        {
            InitializeComponent();
        }

        public void SetApiService(MobiCortexApiService apiService)
        {
            _apiService = apiService;
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(10);

            // Layout principal - TableLayoutPanel (evita problemas do SplitContainer)
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));    // Estatísticas
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));     // Conteúdo
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));     // Paginação

            // ========== PAINEL SUPERIOR - Estatísticas e Busca ==========
            var panelTop = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 100,
                BackColor = Color.FromArgb(240, 248, 255),
                Padding = new Padding(10),
                Margin = new Padding(0)
            };

            // Estatísticas
            _lblEstatisticas = new Label
            {
                Text = "📊 Carregando estatísticas...",
                Location = new Point(10, 10),
                Size = new Size(600, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 100, 150)
            };
            panelTop.Controls.Add(_lblEstatisticas);

            // Campo de busca
            var lblBusca = new Label
            {
                Text = "🔍 Buscar:",
                Location = new Point(10, 45),
                Size = new Size(70, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            panelTop.Controls.Add(lblBusca);

            _txtBusca = new TextBox
            {
                Location = new Point(85, 45),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                Enabled = false
            };
            _txtBusca.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) _ = BuscarAsync(); };
            panelTop.Controls.Add(_txtBusca);

            _btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(340, 43),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnBuscar.Click += (s, e) => _ = BuscarAsync();
            panelTop.Controls.Add(_btnBuscar);

            // Botão Limpar Busca
            var btnLimpar = new Button
            {
                Text = "Limpar",
                Location = new Point(425, 43),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnLimpar.Click += (s, e) => { _txtBusca.Clear(); _filtroNome = null; _offset = 0; _ = CarregarCadastrosAsync(); };
            panelTop.Controls.Add(btnLimpar);
            _txtBusca.Tag = btnLimpar; // Referência para habilitar/desabilitar junto

            // Adicionar painel superior ao TableLayout
            tableLayout.Controls.Add(panelTop, 0, 0);
            tableLayout.SetColumnSpan(panelTop, 2);

            // ========== PAINEL ESQUERDO - Lista de Usuários ==========
            var panelLista = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpLista = new GroupBox
            {
                Text = "📋 Usuários do Cadastro",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // ListBox de usuários
            _lstUsuarios = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                DisplayMember = "DisplayText",
                ValueMember = "Id",
                Enabled = false
            };
            _lstUsuarios.SelectedIndexChanged += LstUsuarios_SelectedIndexChanged;
            grpLista.Controls.Add(_lstUsuarios);
            panelLista.Controls.Add(grpLista);
            tableLayout.Controls.Add(panelLista, 0, 1);

            // ========== PAINEL DIREITO - Detalhes e Ações ==========
            var panelDetalhes = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpDetalhes = new GroupBox
            {
                Text = "📄 Detalhes do Usuário",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Painel de informações
            var panelInfo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                Padding = new Padding(10)
            };

            var lblSelecione = new Label
            {
                Text = "Selecione um usuário na lista para ver os detalhes",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray
            };
            panelInfo.Controls.Add(lblSelecione);
            grpDetalhes.Controls.Add(panelInfo);

            // Botões de ação
            var panelBotoes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(5)
            };

            _btnNovo = new Button
            {
                Text = "➕ Novo",
                Location = new Point(5, 10),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnNovo.Click += BtnNovo_Click;
            panelBotoes.Controls.Add(_btnNovo);

            _btnEditar = new Button
            {
                Text = "✏️ Editar",
                Location = new Point(100, 10),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnEditar.Click += BtnEditar_Click;
            panelBotoes.Controls.Add(_btnEditar);

            _btnApagar = new Button
            {
                Text = "🗑️ Apagar",
                Location = new Point(195, 10),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnApagar.Click += BtnApagar_Click;
            panelBotoes.Controls.Add(_btnApagar);

            grpDetalhes.Controls.Add(panelBotoes);
            panelDetalhes.Controls.Add(grpDetalhes);
            tableLayout.Controls.Add(panelDetalhes, 1, 1);

            // ========== PAINEL INFERIOR - Paginação e Status ==========
            var panelBottom = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 50,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };

            // Botões de paginação
            _btnAnterior = new Button
            {
                Text = "◀ Anterior",
                Location = new Point(10, 10),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnAnterior.Click += (s, e) => { _offset = Math.Max(0, _offset - _count); _ = CarregarCadastrosAsync(); };
            panelBottom.Controls.Add(_btnAnterior);

            _lblPaginacao = new Label
            {
                Text = "Página 1 de 1",
                Location = new Point(110, 15),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9)
            };
            panelBottom.Controls.Add(_lblPaginacao);

            _btnProximo = new Button
            {
                Text = "Próximo ▶",
                Location = new Point(270, 10),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnProximo.Click += (s, e) => { _offset += _count; _ = CarregarCadastrosAsync(); };
            panelBottom.Controls.Add(_btnProximo);

            // Info
            _lblInfo = new Label
            {
                Text = "Conecte-se para gerenciar cadastros",
                Location = new Point(380, 15),
                Size = new Size(400, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };
            panelBottom.Controls.Add(_lblInfo);

            // Adicionar painel inferior ao TableLayout
            tableLayout.Controls.Add(panelBottom, 0, 2);
            tableLayout.SetColumnSpan(panelBottom, 2);

            // Montar layout
            this.Controls.Add(tableLayout);
        }

        #region Event Handlers

        private void LstUsuarios_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool hasSelection = _lstUsuarios.SelectedItem != null;
            _btnEditar.Enabled = hasSelection && _isConnected;
            _btnApagar.Enabled = hasSelection && _isConnected;
            
            if (hasSelection && _lstUsuarios.SelectedItem is UserListItem item)
            {
                var user = item.User;
                _lblInfo.Text = $"Selecionado: {user.Name} (ID: {user.Id})";
            }
        }

        private void BtnNovo_Click(object? sender, EventArgs e)
        {
            var form = new NovoUsuarioForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CriarUsuarioAsync(form.Usuario);
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (_lstUsuarios.SelectedItem is UserListItem item)
            {
                var user = item.User;
                var form = new EditarUsuarioForm(user);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _ = AtualizarUsuarioAsync(form.Usuario);
                }
            }
        }

        private void BtnApagar_Click(object? sender, EventArgs e)
        {
            if (_lstUsuarios.SelectedItem is UserListItem item)
            {
                var user = item.User;
                var result = MessageBox.Show(
                    $"Deseja realmente apagar o usuário '{user.Name}'?\n\n" +
                    "Esta ação não pode ser desfeita!",
                    "Confirmar Exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _ = ApagarUsuarioAsync(user.Id);
                }
            }
        }

        #endregion

        #region API Calls

        private async Task BuscarAsync()
        {
            _filtroNome = string.IsNullOrWhiteSpace(_txtBusca.Text) ? null : _txtBusca.Text.Trim();
            _offset = 0;
            await CarregarCadastrosAsync();
        }

        private async Task CarregarEstatisticasAsync()
        {
            try
            {
                _lblEstatisticas.Text = "📊 Carregando estatísticas...";
                
                var result = await _apiService.GetCentralRegistryStatsAsync();
                
                if (result.Success && result.Data != null)
                {
                    var stats = result.Data;
                    _lblEstatisticas.Text = $"📊 Capacidade: {stats.MaxCapacity:N0} | Cadastrados: {stats.CurrentTotal:N0} | Uso: {stats.UsagePercent:F1}%";
                }
                else
                {
                    _lblEstatisticas.Text = "📊 Estatísticas indisponíveis";
                }
            }
            catch (Exception ex)
            {
                _lblEstatisticas.Text = $"📊 Erro ao carregar estatísticas: {ex.Message}";
            }
        }

        private async Task CarregarCadastrosAsync()
        {
            try
            {
                _lstUsuarios.Enabled = false;
                _lblInfo.Text = "Carregando usuários...";
                
                var result = await _apiService.GetCentralRegistryAsync(_offset, _count, _filtroNome);
                
                _lstUsuarios.Items.Clear();
                
                if (result.Success && result.Data != null)
                {
                    var response = result.Data;
                    _total = response.Total;
                    
                    foreach (var user in response.Items)
                    {
                        _lstUsuarios.Items.Add(new UserListItem(user));
                    }
                    
                    // Atualizar paginação
                    int paginaAtual = (_offset / _count) + 1;
                    int totalPaginas = (int)Math.Ceiling((double)_total / _count);
                    if (totalPaginas < 1) totalPaginas = 1;
                    
                    _lblPaginacao.Text = $"Página {paginaAtual} de {totalPaginas} (Total: {_total})";
                    
                    // Habilitar/desabilitar botões de navegação
                    _btnAnterior.Enabled = _offset > 0;
                    _btnProximo.Enabled = _offset + _count < _total;
                    
                    _lblInfo.Text = $"{_total} usuário(s) encontrado(s)";
                }
                else
                {
                    _lblPaginacao.Text = "Página 1 de 1";
                    _btnAnterior.Enabled = false;
                    _btnProximo.Enabled = false;
                    _lblInfo.Text = result.Message ?? "Erro ao carregar usuários";
                }
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
            finally
            {
                _lstUsuarios.Enabled = true;
            }
        }

        private async Task CriarUsuarioAsync(CentralRegistryUser user)
        {
            try
            {
                _lblInfo.Text = "Criando usuário...";
                
                var result = await _apiService.SaveCentralRegistryUserAsync(user);
                
                if (result.Success)
                {
                    _lblInfo.Text = $"Usuário '{user.Name}' criado com sucesso!";
                    await CarregarCadastrosAsync();
                }
                else
                {
                    _lblInfo.Text = $"Erro ao criar usuário: {result.Message}";
                }
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }

        private async Task AtualizarUsuarioAsync(CentralRegistryUser user)
        {
            try
            {
                _lblInfo.Text = "Atualizando usuário...";
                
                var result = await _apiService.SaveCentralRegistryUserAsync(user);
                
                if (result.Success)
                {
                    _lblInfo.Text = $"Usuário '{user.Name}' atualizado com sucesso!";
                    await CarregarCadastrosAsync();
                }
                else
                {
                    _lblInfo.Text = $"Erro ao atualizar usuário: {result.Message}";
                }
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }

        private async Task ApagarUsuarioAsync(int id)
        {
            try
            {
                _lblInfo.Text = "Apagando usuário...";
                
                var result = await _apiService.DeleteCentralRegistryUserAsync(id);
                
                if (result.Success)
                {
                    _lblInfo.Text = "Usuário removido com sucesso!";
                    await CarregarCadastrosAsync();
                }
                else
                {
                    _lblInfo.Text = $"Erro ao apagar usuário: {result.Message}";
                }
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }

        #endregion

        #region IConnectionAware

        public void OnConnected()
        {
            _isConnected = true;
            _txtBusca.Enabled = true;
            _btnBuscar.Enabled = true;
            if (_txtBusca.Tag is Button btnLimpar) btnLimpar.Enabled = true;
            _btnNovo.Enabled = true;
            
            _ = CarregarEstatisticasAsync();
            _ = CarregarCadastrosAsync();
        }

        #endregion
    }

    #region Classes Auxiliares

    /// <summary>
    /// Wrapper para exibição de usuário no ListBox
    /// </summary>
    public class UserListItem
    {
        public CentralRegistryUser User { get; }

        public UserListItem(CentralRegistryUser user)
        {
            User = user;
        }

        public override string ToString()
        {
            var status = User.Enabled ? "✓" : "✗";
            return $"{status} ID:{User.Id} - {User.Name}";
        }
    }

    public static class CentralRegistryUserExtensions
    {
        public static string DisplayText(this CentralRegistryUser user)
        {
            var status = user.Enabled ? "✓" : "✗";
            return $"{status} ID:{user.Id} - {user.Name}";
        }
    }

    #endregion
}
