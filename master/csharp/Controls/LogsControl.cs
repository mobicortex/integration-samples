using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para visualização de logs
    /// Testa endpoints: GET /api/logs
    /// </summary>
    public partial class LogsControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        private List<LogEntry> _logs = new();
        private DataGridView _dgvLogs = null!;
        private TextBox _txtFiltro = null!;
        private Button _btnBuscar = null!;
        private Button _btnAtualizar = null!;
        private ComboBox _cmbTipoFiltro = null!;

        public LogsControl()
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

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Barra de ferramentas
            var toolPanel = new Panel { Dock = DockStyle.Top, Height = 50 };

            var lblTipo = new Label { Text = "Tipo:", Location = new Point(5, 15), Size = new Size(35, 20), TextAlign = ContentAlignment.MiddleRight };
            _cmbTipoFiltro = new ComboBox
            {
                Location = new Point(45, 12),
                Size = new Size(90, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbTipoFiltro.Items.AddRange(new object[] { "Todos", "TAG", "PLACA", "FACIAL" });
            _cmbTipoFiltro.SelectedIndex = 0;

            var lblFiltro = new Label { Text = "Filtro:", Location = new Point(145, 15), Size = new Size(40, 20), TextAlign = ContentAlignment.MiddleRight };
            _txtFiltro = new TextBox { Location = new Point(190, 12), Size = new Size(150, 25) };

            _btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(350, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnBuscar.Click += async (s, e) => await LoadLogsAsync();

            _btnAtualizar = new Button
            {
                Text = "🔄 Atualizar",
                Location = new Point(450, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAtualizar.Click += async (s, e) => await LoadLogsAsync();

            var btnLimpar = new Button
            {
                Text = "🧹 Limpar",
                Location = new Point(550, 10),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLimpar.Click += (s, e) =>
            {
                _logs.Clear();
                RefreshGrid();
            };

            toolPanel.Controls.AddRange(new Control[] { lblTipo, _cmbTipoFiltro, lblFiltro, _txtFiltro, _btnBuscar, _btnAtualizar, btnLimpar });
            panel.Controls.Add(toolPanel);

            // DataGridView
            _dgvLogs = new DataGridView
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

            _dgvLogs.Columns.Add("Id", "ID");
            _dgvLogs.Columns.Add("Tipo", "Tipo");
            _dgvLogs.Columns.Add("Dado", "Dado");
            _dgvLogs.Columns.Add("Nome", "Nome");
            _dgvLogs.Columns.Add("Timestamp", "Data/Hora");
            _dgvLogs.Columns.Add("IpOrigem", "IP Origem");

            // Cores por tipo
            _dgvLogs.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= _logs.Count) return;
                
                var tipo = _logs[e.RowIndex].Tipo;
                switch (tipo)
                {
                    case "TAG":
                        e.CellStyle!.BackColor = Color.FromArgb(200, 230, 255);
                        break;
                    case "PLACA":
                        e.CellStyle!.BackColor = Color.FromArgb(200, 255, 200);
                        break;
                    case "FACIAL":
                        e.CellStyle!.BackColor = Color.FromArgb(255, 230, 200);
                        break;
                }
            };

            panel.Controls.Add(_dgvLogs);
            this.Controls.Add(panel);
        }

        private async Task LoadLogsAsync()
        {
            string? filtro = null;
            
            var tipoSelecionado = _cmbTipoFiltro.SelectedItem?.ToString();
            if (tipoSelecionado != "Todos")
            {
                filtro = tipoSelecionado;
            }

            if (!string.IsNullOrWhiteSpace(_txtFiltro.Text))
            {
                filtro = string.IsNullOrEmpty(filtro) ? _txtFiltro.Text : $"{filtro} {_txtFiltro.Text}";
            }

            var result = await _apiService.GetLogsAsync(filtro);
            if (result.Success && result.Data != null)
            {
                _logs = result.Data;
                RefreshGrid();
            }
            else
            {
                MessageBox.Show($"Erro ao carregar logs: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid()
        {
            _dgvLogs.Rows.Clear();
            foreach (var log in _logs)
            {
                _dgvLogs.Rows.Add(log.Id, log.Tipo, log.Dado, log.Nome, log.Timestamp, log.IpOrigem);
            }
        }

        public void OnConnected()
        {
            _ = LoadLogsAsync();
        }
    }
}
