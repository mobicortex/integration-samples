using System.ComponentModel;
using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    public class MediasControl : UserControl, IConnectionAware
    {
        private readonly ApiService _apiService;
        private DataGridView _dataGridView = null!;
        private Button _btnAddPlate = null!;
        private Button _btnAddRfid = null!;
        private Button _btnRefresh = null!;
        private Button _btnDelete = null!;
        private ComboBox _cboUser = null!;
        private Label _lblInfo = null!;
        private BindingList<MediaDisplay> _medias = new();
        private List<User> _users = new();
        private bool _isConnected = false;

        public MediasControl(ApiService apiService)
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

            // Label Usuário
            var lblUser = new Label
            {
                Text = "Entidade:",
                Location = new Point(10, 15),
                Size = new Size(55, 20)
            };
            actionPanel.Controls.Add(lblUser);

            // ComboBox Usuário
            _cboUser = new ComboBox
            {
                Location = new Point(70, 12),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            _cboUser.SelectedIndexChanged += async (s, e) => await LoadMediasForSelectedUserAsync();
            actionPanel.Controls.Add(_cboUser);

            // Botão Atualizar
            _btnRefresh = new Button
            {
                Text = "🔄 Atualizar",
                Location = new Point(280, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnRefresh.Click += async (s, e) => await LoadMediasForSelectedUserAsync();
            actionPanel.Controls.Add(_btnRefresh);

            // Botão Adicionar Placa
            _btnAddPlate = new Button
            {
                Text = "🚗 Nova Placa",
                Location = new Point(390, 10),
                Size = new Size(110, 30),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnAddPlate.Click += BtnAddPlate_Click;
            actionPanel.Controls.Add(_btnAddPlate);

            // Botão Adicionar RFID
            _btnAddRfid = new Button
            {
                Text = "📡 Novo RFID",
                Location = new Point(510, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnAddRfid.Click += BtnAddRfid_Click;
            actionPanel.Controls.Add(_btnAddRfid);

            // Botão Excluir
            _btnDelete = new Button
            {
                Text = "🗑️ Excluir",
                Location = new Point(620, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnDelete.Click += async (s, e) => await DeleteSelectedMediaAsync();
            actionPanel.Controls.Add(_btnDelete);

            this.Controls.Add(actionPanel);

            // Label de informações
            _lblInfo = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(240, 240, 240),
                Text = "Conecte ao servidor e selecione uma entidade para visualizar as mídias.",
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
                Width = 60
            });

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoDisplay",
                HeaderText = "Tipo",
                DataPropertyName = "TipoDisplay",
                Width = 80
            });

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Valor",
                HeaderText = "Valor (Placa/RFID)",
                DataPropertyName = "Valor",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserId",
                HeaderText = "User ID",
                DataPropertyName = "UserId",
                Width = 80
            });

            _dataGridView.DataSource = _medias;
            _dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            this.Controls.Add(_dataGridView);
        }

        public void OnConnected()
        {
            _isConnected = true;
            _btnRefresh.Enabled = true;
            _btnAddPlate.Enabled = true;
            _btnAddRfid.Enabled = true;
            _cboUser.Enabled = true;
            _lblInfo.Text = "Carregando entidades...";
            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var result = await _apiService.GetUsersAsync();
                
                _users.Clear();
                _cboUser.Items.Clear();
                
                if (result.Success && result.Data != null && result.Data.Count > 0)
                {
                    _users = result.Data;
                    foreach (var user in result.Data)
                    {
                        _cboUser.Items.Add($"{user.Id} - {user.Name}");
                    }
                    _cboUser.SelectedIndex = 0;
                }
                else
                {
                    _lblInfo.Text = "Nenhuma entidade encontrada. Cadastre uma entidade primeiro.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar entidades: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao carregar entidades.";
            }
        }

        private async Task LoadMediasForSelectedUserAsync()
        {
            if (_cboUser.SelectedIndex < 0 || _cboUser.SelectedIndex >= _users.Count) return;

            var userId = _users[_cboUser.SelectedIndex].Id;

            try
            {
                _lblInfo.Text = "Carregando mídias...";
                _btnRefresh.Enabled = false;

                var result = await _apiService.GetMediasByUserAsync(userId);

                _medias.Clear();
                if (result.Success && result.Data != null)
                {
                    foreach (var media in result.Data)
                    {
                        _medias.Add(new MediaDisplay(media));
                    }
                    _lblInfo.Text = $"Total de mídias para a entidade: {_medias.Count}";
                }
                else
                {
                    _lblInfo.Text = $"Erro: {result.Message ?? "Nenhuma mídia encontrada"}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar mídias: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao carregar mídias.";
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

        private void BtnAddPlate_Click(object? sender, EventArgs e)
        {
            if (_cboUser.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma entidade primeiro.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var userId = _users[_cboUser.SelectedIndex].Id;
            var form = new AddPlateForm();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CreatePlateAsync(form.Plate, userId);
            }
        }

        private void BtnAddRfid_Click(object? sender, EventArgs e)
        {
            if (_cboUser.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma entidade primeiro.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var userId = _users[_cboUser.SelectedIndex].Id;
            var form = new AddRfidForm();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CreateRfidAsync(form.RfidNumber, userId);
            }
        }

        private async Task CreatePlateAsync(string plate, int userId)
        {
            try
            {
                _lblInfo.Text = "Cadastrando placa...";
                
                var result = await _apiService.CreatePlateAsync(plate, userId);
                
                if (result.Success && result.Data != null)
                {
                    _medias.Add(new MediaDisplay(result.Data));
                    _lblInfo.Text = $"Placa '{plate}' cadastrada com sucesso.";
                    MessageBox.Show($"Placa cadastrada com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Erro ao cadastrar placa: {result.Message}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblInfo.Text = "Erro ao cadastrar placa.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar placa: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao cadastrar placa.";
            }
        }

        private async Task CreateRfidAsync(string numero, int userId)
        {
            try
            {
                _lblInfo.Text = "Cadastrando RFID...";
                
                var result = await _apiService.CreateRfidAsync(numero, userId);
                
                if (result.Success && result.Data != null)
                {
                    _medias.Add(new MediaDisplay(result.Data));
                    _lblInfo.Text = $"RFID '{numero}' cadastrado com sucesso.";
                    MessageBox.Show($"RFID cadastrado com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Erro ao cadastrar RFID: {result.Message}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblInfo.Text = "Erro ao cadastrar RFID.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar RFID: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblInfo.Text = "Erro ao cadastrar RFID.";
            }
        }

        private async Task DeleteSelectedMediaAsync()
        {
            if (_dataGridView.SelectedRows.Count == 0) return;

            var media = (MediaDisplay)_dataGridView.SelectedRows[0].DataBoundItem;
            
            var result = MessageBox.Show(
                $"Deseja realmente excluir esta mídia?\n\nTipo: {media.TipoDisplay}\nValor: {media.Valor}",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _lblInfo.Text = "Excluindo mídia...";
                    
                    var apiResult = await _apiService.DeleteMediaAsync(media.Id);
                    
                    if (apiResult.Success)
                    {
                        _medias.Remove(media);
                        _lblInfo.Text = "Mídia excluída com sucesso.";
                    }
                    else
                    {
                        MessageBox.Show($"Erro ao excluir mídia: {apiResult.Message}", "Erro", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _lblInfo.Text = "Erro ao excluir mídia.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir mídia: {ex.Message}", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _lblInfo.Text = "Erro ao excluir mídia.";
                }
            }
        }
    }

    /// <summary>
    /// Classe para exibição de mídia no DataGridView
    /// </summary>
    public class MediaDisplay
    {
        public int Id { get; set; }
        public string TipoDisplay { get; set; } = "";
        public string Valor { get; set; } = "";
        public int UserId { get; set; }

        public MediaDisplay(Media media)
        {
            Id = media.Id;
            UserId = media.UserId;

            if (!string.IsNullOrEmpty(media.Plate))
            {
                TipoDisplay = "Placa";
                Valor = media.Plate;
            }
            else if (media.Ns320.HasValue)
            {
                TipoDisplay = "RFID";
                if (media.Ns321.HasValue && media.Ns321 > 0)
                {
                    Valor = $"{media.Ns320},{media.Ns321}";
                }
                else
                {
                    Valor = media.Ns320.ToString() ?? "";
                }
            }
            else
            {
                TipoDisplay = "Outro";
                Valor = media.Id.ToString();
            }
        }
    }

    /// <summary>
    /// Formulário para adicionar nova placa
    /// </summary>
    public class AddPlateForm : Form
    {
        private TextBox _txtPlate = null!;
        public string Plate => _txtPlate.Text.Trim().ToUpper();

        public AddPlateForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Nova Placa";
            this.Size = new Size(400, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label
            var lblPlate = new Label
            {
                Text = "Placa do Veículo:",
                Location = new Point(20, 20),
                Size = new Size(340, 20)
            };
            this.Controls.Add(lblPlate);

            // TextBox
            _txtPlate = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(340, 25),
                CharacterCasing = CharacterCasing.Upper,
                MaxLength = 7
            };
            this.Controls.Add(_txtPlate);

            // Label exemplo
            var lblExample = new Label
            {
                Text = "Exemplo: ABC1234 ou ABC1D23",
                Location = new Point(20, 75),
                Size = new Size(340, 20),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblExample);

            // Botão OK
            var btnOk = new Button
            {
                Text = "Salvar",
                Location = new Point(200, 100),
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
                Location = new Point(290, 100),
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
            _txtPlate.Focus();
        }
    }

    /// <summary>
    /// Formulário para adicionar novo RFID
    /// </summary>
    public class AddRfidForm : Form
    {
        private TextBox _txtRfid = null!;
        private RadioButton _rbWiegand = null!;
        private RadioButton _rbXcode = null!;
        private RadioButton _rb10Digitos = null!;
        private TextBox _txtFacility = null!;
        private TextBox _txtCard = null!;
        private TextBox _txtEmpresa = null!;

        public string RfidNumber => GetRfidNumber();

        public AddRfidForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Novo RFID";
            this.Size = new Size(450, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Radio Buttons para tipo de RFID
            var lblTipo = new Label
            {
                Text = "Tipo de RFID:",
                Location = new Point(20, 15),
                Size = new Size(400, 20)
            };
            this.Controls.Add(lblTipo);

            _rbWiegand = new RadioButton
            {
                Text = "Wiegand (Facility,Card)",
                Location = new Point(20, 40),
                Size = new Size(180, 20),
                Checked = true
            };
            _rbWiegand.CheckedChanged += RbTipo_CheckedChanged;
            this.Controls.Add(_rbWiegand);

            _rbXcode = new RadioButton
            {
                Text = "XCODE (Empresa,Facility,Card)",
                Location = new Point(220, 40),
                Size = new Size(200, 20)
            };
            _rbXcode.CheckedChanged += RbTipo_CheckedChanged;
            this.Controls.Add(_rbXcode);

            _rb10Digitos = new RadioButton
            {
                Text = "10 Dígitos",
                Location = new Point(20, 65),
                Size = new Size(180, 20)
            };
            _rb10Digitos.CheckedChanged += RbTipo_CheckedChanged;
            this.Controls.Add(_rb10Digitos);

            // Campos Wiegand/XCODE
            var lblEmpresa = new Label
            {
                Text = "Empresa:",
                Location = new Point(20, 100),
                Size = new Size(80, 20)
            };
            this.Controls.Add(lblEmpresa);

            _txtEmpresa = new TextBox
            {
                Location = new Point(110, 97),
                Size = new Size(100, 25),
                Enabled = false
            };
            this.Controls.Add(_txtEmpresa);

            var lblFacility = new Label
            {
                Text = "Facility Code:",
                Location = new Point(20, 130),
                Size = new Size(80, 20)
            };
            this.Controls.Add(lblFacility);

            _txtFacility = new TextBox
            {
                Location = new Point(110, 127),
                Size = new Size(100, 25)
            };
            this.Controls.Add(_txtFacility);

            var lblCard = new Label
            {
                Text = "Card Number:",
                Location = new Point(20, 160),
                Size = new Size(80, 20)
            };
            this.Controls.Add(lblCard);

            _txtCard = new TextBox
            {
                Location = new Point(110, 157),
                Size = new Size(100, 25)
            };
            this.Controls.Add(_txtCard);

            // Campo 10 dígitos
            var lbl10Digitos = new Label
            {
                Text = "Número (10 dígitos):",
                Location = new Point(20, 195),
                Size = new Size(150, 20)
            };
            this.Controls.Add(lbl10Digitos);

            _txtRfid = new TextBox
            {
                Location = new Point(170, 192),
                Size = new Size(150, 25),
                MaxLength = 10,
                Enabled = false
            };
            this.Controls.Add(_txtRfid);

            // Exemplo
            var lblExample = new Label
            {
                Text = "Exemplo Wiegand: 123,45678",
                Location = new Point(230, 130),
                Size = new Size(180, 20),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblExample);

            var lblExample2 = new Label
            {
                Text = "Exemplo XCODE: 12345,123,45678",
                Location = new Point(230, 160),
                Size = new Size(180, 20),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblExample2);

            // Botões
            var btnOk = new Button
            {
                Text = "Salvar",
                Location = new Point(250, 230),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnOk);

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(340, 230),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            UpdateFields();
        }

        private void RbTipo_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateFields();
        }

        private void UpdateFields()
        {
            if (_rb10Digitos.Checked)
            {
                _txtEmpresa.Enabled = false;
                _txtFacility.Enabled = false;
                _txtCard.Enabled = false;
                _txtRfid.Enabled = true;
            }
            else if (_rbXcode.Checked)
            {
                _txtEmpresa.Enabled = true;
                _txtFacility.Enabled = true;
                _txtCard.Enabled = true;
                _txtRfid.Enabled = false;
            }
            else // Wiegand
            {
                _txtEmpresa.Enabled = false;
                _txtFacility.Enabled = true;
                _txtCard.Enabled = true;
                _txtRfid.Enabled = false;
            }
        }

        private string GetRfidNumber()
        {
            if (_rb10Digitos.Checked)
            {
                return _txtRfid.Text.Trim();
            }
            else if (_rbXcode.Checked)
            {
                return $"{_txtEmpresa.Text.Trim()},{_txtFacility.Text.Trim()},{_txtCard.Text.Trim()}";
            }
            else // Wiegand
            {
                return $"{_txtFacility.Text.Trim()},{_txtCard.Text.Trim()}";
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_rb10Digitos.Checked)
                _txtRfid.Focus();
            else
                _txtFacility.Focus();
        }
    }
}