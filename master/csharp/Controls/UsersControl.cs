using System.ComponentModel;
using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    public class UsersControl : UserControl, IConnectionAware
    {
        private readonly ApiService _apiService;
        private DataGridView _dataGridView = null!;
        private Button _btnAdd = null!;
        private Button _btnRefresh = null!;
        private Button _btnDelete = null!;
        private TextBox _txtSearch = null!;
        private Label _lblInfo = null!;
        private BindingList<User> _users = new();
        private bool _isConnected = false;

        public UsersControl(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Padding = new Padding(10);

            // Painel de ações
            var actionPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            // Botão Atualizar
            _btnRefresh = new Button
            {
                Text = "🔄 Atualizar",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnRefresh.Click += async (s, e) => await LoadUsersAsync();
            actionPanel.Controls.Add(_btnRefresh);

            // Botão Adicionar
            _btnAdd = new Button
            {
                Text = "➕ Nova Entidade",
                Location = new Point(120, 10),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnAdd.Click += BtnAdd_Click;
            actionPanel.Controls.Add(_btnAdd);

            // Botão Excluir
            _btnDelete = new Button
            {
                Text = "🗑️ Excluir",
                Location = new Point(250, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnDelete.Click += async (s, e) => await DeleteSelectedUserAsync();
            actionPanel.Controls.Add(_btnDelete);

            // Label de busca
            var lblSearch = new Label
            {
                Text = "Buscar:",
                Location = new Point(370, 15),
                Size = new Size(50, 20)
            };
            actionPanel.Controls.Add(lblSearch);

            // TextBox de busca
            _txtSearch = new TextBox
            {
                Location = new Point(420, 12),
                Size = new Size(200, 25)
            };
            _txtSearch.TextChanged += TxtSearch_TextChanged;
            actionPanel.Controls.Add(_txtSearch);

            this.Controls.Add(actionPanel);

            // Label de informações
            _lblInfo = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(240, 240, 240),
                Text = "Conecte ao servidor para visualizar as entidades.",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            this.Controls.Add(_lblInfo);

            // DataGridView
            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 245, 245)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(70, 130, 180),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10),
                    SelectionBackColor = Color.FromArgb(100, 149, 237),
                    SelectionForeColor = Color.White
                }
            };

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 80
            });

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Nome da Entidade",
                DataPropertyName = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            _dataGridView.DataSource = _users;
            _dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            this.Controls.Add(_dataGridView);
        }

        public void OnConnected()
        {
            _isConnected = true;
            _btnRefresh.Enabled = true;
            _btnAdd.Enabled = true;
            _lblInfo.Text = "Carregando entidades...";
            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            if (!_isConnected) return;

            try
            {
                _lblInfo.Text = "Carregando entidades...";
                _btnRefresh.Enabled = false;

                var result = await _apiService.GetUsersAsync();

                _users.Clear();
                if (result.Success && result.Data != null)
                {
                    foreach (var user in result.Data)
                    {
                        _users.Add(user);
                    }
                    _lblInfo.Text = $"Total de entidades: {_users.Count}";
                }
                else
                {
                    _lblInfo.Text = $"Erro: {result.Message ?? "Nenhuma entidade encontrada"}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar entidades: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao carregar entidades.";
            }
            finally
            {
                _btnRefresh.Enabled = true;
            }
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            _btnDelete.Enabled = _dataGridView.SelectedRows.Count > 0 && _isConnected;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var searchText = _txtSearch.Text.ToLower();
            
            if (string.IsNullOrEmpty(searchText))
            {
                _dataGridView.DataSource = _users;
            }
            else
            {
                var filtered = _users.Where(u => 
                    u.Name.ToLower().Contains(searchText) || 
                    u.Id.ToString().Contains(searchText)).ToList();
                _dataGridView.DataSource = new BindingList<User>(filtered);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var form = new AddUserForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CreateUserAsync(form.UserName);
            }
        }

        private async Task CreateUserAsync(string name)
        {
            try
            {
                _lblInfo.Text = "Criando entidade...";
                
                var result = await _apiService.CreateUserAsync(name);
                
                if (result.Success && result.Data != null)
                {
                    _users.Add(result.Data);
                    _lblInfo.Text = $"Entidade '{name}' criada com sucesso. ID: {result.Data.Id}";
                    MessageBox.Show($"Entidade criada com sucesso!\nID: {result.Data.Id}", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Erro ao criar entidade: {result.Message}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblInfo.Text = "Erro ao criar entidade.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar entidade: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao criar entidade.";
            }
        }

        private async Task DeleteSelectedUserAsync()
        {
            if (_dataGridView.SelectedRows.Count == 0) return;

            var user = (User)_dataGridView.SelectedRows[0].DataBoundItem;
            
            var result = MessageBox.Show(
                $"Deseja realmente excluir a entidade '{user.Name}'?\n\nID: {user.Id}",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _lblInfo.Text = "Excluindo entidade...";
                    
                    var apiResult = await _apiService.DeleteUserAsync(user.Id);
                    
                    if (apiResult.Success)
                    {
                        _users.Remove(user);
                        _lblInfo.Text = $"Entidade '{user.Name}' excluída com sucesso.";
                    }
                    else
                    {
                        MessageBox.Show($"Erro ao excluir entidade: {apiResult.Message}", "Erro", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _lblInfo.Text = "Erro ao excluir entidade.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir entidade: {ex.Message}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblInfo.Text = "Erro ao excluir entidade.";
                }
            }
        }
    }

    /// <summary>
    /// Formulário para adicionar nova entidade
    /// </summary>
    public class AddUserForm : Form
    {
        private TextBox _txtName = null!;
        public string UserName => _txtName.Text.Trim();

        public AddUserForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Nova Entidade";
            this.Size = new Size(400, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label
            var lblName = new Label
            {
                Text = "Nome da Entidade:",
                Location = new Point(20, 20),
                Size = new Size(340, 20)
            };
            this.Controls.Add(lblName);

            // TextBox
            _txtName = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(340, 25)
            };
            this.Controls.Add(_txtName);

            // Botão OK
            var btnOk = new Button
            {
                Text = "Salvar",
                Location = new Point(200, 90),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnOk);

            // Botão Cancelar
            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(290, 90),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtName.Focus();
        }
    }
}