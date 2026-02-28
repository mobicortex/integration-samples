using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para gerenciamento de veículos
    /// Testa endpoints: GET/POST/PUT/DELETE /api/veiculos
    /// </summary>
    public partial class VehiclesControl : UserControl, IConnectionAware
    {
        private const int DefaultSplitterDistance = 350;
        private MobiCortexApiService _apiService = null!;
        private List<Vehicle> _vehicles = new();
        private DataGridView _dgvVehicles = null!;
        private TextBox _txtPlaca = null!;
        private TextBox _txtTagRfid = null!;
        private TextBox _txtProprietario = null!;
        private ComboBox _cmbTipo = null!;
        private TextBox _txtFiltro = null!;
        private Button _btnNovo = null!;
        private Button _btnSalvar = null!;
        private Button _btnExcluir = null!;
        private Button _btnAtualizar = null!;
        private long? _editingId = null;

        public VehiclesControl()
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

            // Layout principal
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                Panel1MinSize = 0,
                Panel2MinSize = 0
            };
            this.Load += (_, _) => AdjustSplitterDistance(split);
            split.SizeChanged += (_, _) => AdjustSplitterDistance(split);

            // Painel esquerdo - Formulário
            var panelForm = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpForm = new GroupBox
            {
                Text = "📋 Cadastro de Veículo",
                Dock = DockStyle.Top,
                Height = 280,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            int y = 30;
            int lblWidth = 90;
            int txtWidth = 220;

            // Placa
            var lblPlaca = new Label { Text = "Placa:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtPlaca = new TextBox { Location = new Point(lblWidth + 15, y), Size = new Size(txtWidth, 25), CharacterCasing = CharacterCasing.Upper, Font = new Font("Segoe UI", 10) };
            y += 35;

            // TAG RFID
            var lblTag = new Label { Text = "TAG RFID:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtTagRfid = new TextBox { Location = new Point(lblWidth + 15, y), Size = new Size(txtWidth, 25), Font = new Font("Segoe UI", 10) };
            y += 35;

            // Proprietário
            var lblProp = new Label { Text = "Proprietário:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtProprietario = new TextBox { Location = new Point(lblWidth + 15, y), Size = new Size(txtWidth, 25), Font = new Font("Segoe UI", 10) };
            y += 35;

            // Tipo
            var lblTipo = new Label { Text = "Tipo:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _cmbTipo = new ComboBox
            {
                Location = new Point(lblWidth + 15, y),
                Size = new Size(txtWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "Morador", "Visitante", "Funcionário" });
            _cmbTipo.SelectedIndex = 0;
            y += 45;

            // Botões
            var btnPanel = new Panel { Location = new Point(10, y), Size = new Size(320, 40) };
            _btnNovo = new Button
            {
                Text = "🆕 Novo",
                Location = new Point(0, 0),
                Size = new Size(75, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            _btnNovo.Click += (s, e) => ClearForm();

            _btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(80, 0),
                Size = new Size(75, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            _btnSalvar.Click += async (s, e) => await SaveVehicleAsync();

            _btnExcluir = new Button
            {
                Text = "🗑️ Excluir",
                Location = new Point(160, 0),
                Size = new Size(75, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            _btnExcluir.Click += async (s, e) => await DeleteVehicleAsync();

            _btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new Point(240, 0),
                Size = new Size(75, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            _btnAtualizar.Click += async (s, e) => await LoadVehiclesAsync();

            btnPanel.Controls.AddRange(new Control[] { _btnNovo, _btnSalvar, _btnExcluir, _btnAtualizar });

            grpForm.Controls.AddRange(new Control[] { lblPlaca, _txtPlaca, lblTag, _txtTagRfid, lblProp, _txtProprietario, lblTipo, _cmbTipo, btnPanel });
            panelForm.Controls.Add(grpForm);
            split.Panel1.Controls.Add(panelForm);

            // Painel direito - Lista
            var panelList = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpList = new GroupBox
            {
                Text = "📋 Lista de Veículos",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Barra de ferramentas
            var toolPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
            var lblFiltro = new Label { Text = "🔍 Filtro:", Location = new Point(5, 8), Size = new Size(50, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtFiltro = new TextBox { Location = new Point(60, 5), Size = new Size(150, 25) };
            var btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(215, 3),
                Size = new Size(70, 28),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.Click += async (s, e) => await LoadVehiclesAsync(_txtFiltro.Text);

            var btnExportXlsx = new Button
            {
                Text = "📊 Excel",
                Location = new Point(290, 3),
                Size = new Size(70, 28),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportXlsx.Click += async (s, e) => await ExportXlsxAsync();

            var btnExportPdf = new Button
            {
                Text = "📄 PDF",
                Location = new Point(365, 3),
                Size = new Size(70, 28),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportPdf.Click += async (s, e) => await ExportPdfAsync();

            toolPanel.Controls.AddRange(new Control[] { lblFiltro, _txtFiltro, btnBuscar, btnExportXlsx, btnExportPdf });
            grpList.Controls.Add(toolPanel);

            // DataGridView
            _dgvVehicles = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 240, 240) }
            };
            _dgvVehicles.Columns.Add("Id", "ID");
            _dgvVehicles.Columns.Add("Placa", "Placa");
            _dgvVehicles.Columns.Add("TagRfid", "TAG RFID");
            _dgvVehicles.Columns.Add("Proprietario", "Proprietário");
            _dgvVehicles.Columns.Add("Tipo", "Tipo");
            _dgvVehicles.Columns.Add("DataCadastro", "Data Cadastro");

            _dgvVehicles.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    SelectVehicle(e.RowIndex);
                }
            };

            grpList.Controls.Add(_dgvVehicles);
            panelList.Controls.Add(grpList);
            split.Panel2.Controls.Add(panelList);

            this.Controls.Add(split);
        }

        private void ClearForm()
        {
            _editingId = null;
            _txtPlaca.Clear();
            _txtTagRfid.Clear();
            _txtProprietario.Clear();
            _cmbTipo.SelectedIndex = 0;
            _dgvVehicles.ClearSelection();
        }

        private void SelectVehicle(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _vehicles.Count) return;

            var vehicle = _vehicles[rowIndex];
            _editingId = vehicle.Id;
            _txtPlaca.Text = vehicle.Placa;
            _txtTagRfid.Text = vehicle.TagRfid;
            _txtProprietario.Text = vehicle.Proprietario;
            
            var tipoIndex = _cmbTipo.Items.Cast<string>().ToList().FindIndex(t => t == vehicle.Tipo);
            if (tipoIndex >= 0) _cmbTipo.SelectedIndex = tipoIndex;
        }

        private async Task LoadVehiclesAsync(string? filtro = null)
        {
            var result = await _apiService.GetVehiclesAsync(filtro);
            if (result.Success && result.Data != null)
            {
                _vehicles = result.Data;
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            _dgvVehicles.Rows.Clear();
            foreach (var v in _vehicles)
            {
                _dgvVehicles.Rows.Add(v.Id, v.Placa, v.TagRfid, v.Proprietario, v.Tipo, v.DataCadastro);
            }
        }

        private async Task SaveVehicleAsync()
        {
            if (string.IsNullOrWhiteSpace(_txtPlaca.Text))
            {
                MessageBox.Show("Placa é obrigatória!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtProprietario.Text))
            {
                MessageBox.Show("Proprietário é obrigatório!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ApiResult<Vehicle> result;
            if (_editingId.HasValue)
            {
                result = await _apiService.UpdateVehicleAsync(
                    _editingId.Value,
                    _txtPlaca.Text.ToUpper(),
                    _txtTagRfid.Text,
                    _txtProprietario.Text,
                    _cmbTipo.SelectedItem?.ToString() ?? "Morador");
            }
            else
            {
                result = await _apiService.CreateVehicleAsync(
                    _txtPlaca.Text.ToUpper(),
                    _txtTagRfid.Text,
                    _txtProprietario.Text,
                    _cmbTipo.SelectedItem?.ToString() ?? "Morador");
            }

            if (result.Success)
            {
                MessageBox.Show(_editingId.HasValue ? "Veículo atualizado!" : "Veículo cadastrado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                await LoadVehiclesAsync();
            }
            else
            {
                MessageBox.Show($"Erro: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DeleteVehicleAsync()
        {
            if (!_editingId.HasValue)
            {
                MessageBox.Show("Selecione um veículo para excluir!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Deseja excluir o veículo {_txtPlaca.Text}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var result = await _apiService.DeleteVehicleAsync(_editingId.Value);
            if (result.Success)
            {
                MessageBox.Show("Veículo excluído!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                await LoadVehiclesAsync();
            }
            else
            {
                MessageBox.Show($"Erro: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExportXlsxAsync()
        {
            var result = await _apiService.ExportVehiclesXlsxAsync();
            if (result.Success && result.Data != null)
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = "veiculos.xlsx"
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    await File.WriteAllBytesAsync(saveDialog.FileName, result.Data);
                    MessageBox.Show("Arquivo exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Erro na exportação: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExportPdfAsync()
        {
            var result = await _apiService.ExportVehiclesPdfAsync();
            if (result.Success && result.Data != null)
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = "veiculos.pdf"
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    await File.WriteAllBytesAsync(saveDialog.FileName, result.Data);
                    MessageBox.Show("Arquivo exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show($"Erro na exportação: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdjustSplitterDistance(SplitContainer split)
        {
            if (split.Width <= 0) return;

            var desired = Math.Clamp(DefaultSplitterDistance, 0, split.Width);
            if (split.SplitterDistance != desired)
                split.SplitterDistance = desired;
        }

        public void OnConnected()
        {
            _ = LoadVehiclesAsync();
        }
    }
}
