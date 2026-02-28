using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para gerenciamento de eventos
    /// Testa endpoints: POST /api/events/new
    /// </summary>
    public partial class EventsControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        private List<Event> _events = new();
        private DataGridView _dgvEvents = null!;
        private ComboBox _cmbTipo = null!;
        private TextBox _txtValor = null!;
        private TextBox _txtNome = null!;
        private Button _btnRegistrar = null!;
        private Button _btnSimularTag = null!;
        private Button _btnSimularPlaca = null!;
        private Button _btnSimularFacial = null!;
        private SplitContainer _split = null!;

        public EventsControl()
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

            _split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                Panel1MinSize = 0,
                Panel2MinSize = 0
            };
            this.Load += EventsControl_Load;
            _split.SizeChanged += Split_SizeChanged;

            // Painel esquerdo - Formulário
            var panelForm = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpForm = new GroupBox
            {
                Text = "📡 Registrar Evento",
                Dock = DockStyle.Top,
                Height = 320,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            int y = 30;
            int lblWidth = 80;
            int txtWidth = 220;

            // Tipo
            var lblTipo = new Label { Text = "Tipo:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _cmbTipo = new ComboBox
            {
                Location = new Point(lblWidth + 15, y),
                Size = new Size(txtWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "TAG", "PLACA", "FACIAL" });
            _cmbTipo.SelectedIndex = 0;
            y += 35;

            // Valor
            var lblValor = new Label { Text = "Valor:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtValor = new TextBox { Location = new Point(lblWidth + 15, y), Size = new Size(txtWidth, 25), Font = new Font("Segoe UI", 10) };
            y += 35;

            // Nome
            var lblNome = new Label { Text = "Nome:", Location = new Point(10, y), Size = new Size(lblWidth, 25), TextAlign = ContentAlignment.MiddleRight };
            _txtNome = new TextBox { Location = new Point(lblWidth + 15, y), Size = new Size(txtWidth, 25), Font = new Font("Segoe UI", 10) };
            y += 45;

            // Botão Registrar
            _btnRegistrar = new Button
            {
                Text = "✅ Registrar Evento",
                Location = new Point(lblWidth + 15, y),
                Size = new Size(txtWidth, 40),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnRegistrar.Click += async (s, e) => await RegisterEventAsync();
            y += 55;

            // Botões de Simulação
            var grpSimular = new GroupBox
            {
                Text = "⚡ Simular Eventos",
                Location = new Point(10, y),
                Size = new Size(320, 90),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            _btnSimularTag = new Button
            {
                Text = "🏷️ TAG",
                Location = new Point(10, 20),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSimularTag.Click += async (s, e) => await SimulateEventAsync("TAG", "A12B34C5", "João Silva");

            _btnSimularPlaca = new Button
            {
                Text = "🚗 Placa",
                Location = new Point(110, 20),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSimularPlaca.Click += async (s, e) => await SimulateEventAsync("PLACA", "ABC1234", "Maria Oliveira");

            _btnSimularFacial = new Button
            {
                Text = "👤 Facial",
                Location = new Point(210, 20),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSimularFacial.Click += async (s, e) => await SimulateEventAsync("FACIAL", "ID12345", "Carlos Santos");

            grpSimular.Controls.AddRange(new Control[] { _btnSimularTag, _btnSimularPlaca, _btnSimularFacial });

            grpForm.Controls.AddRange(new Control[] { lblTipo, _cmbTipo, lblValor, _txtValor, lblNome, _txtNome, _btnRegistrar, grpSimular });
            panelForm.Controls.Add(grpForm);
            _split.Panel1.Controls.Add(panelForm);

            // Painel direito - Lista de eventos recentes
            var panelList = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpList = new GroupBox
            {
                Text = "📋 Eventos Registrados (Sessão)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // DataGridView
            _dgvEvents = new DataGridView
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
            _dgvEvents.Columns.Add("Id", "ID");
            _dgvEvents.Columns.Add("Tipo", "Tipo");
            _dgvEvents.Columns.Add("Valor", "Valor");
            _dgvEvents.Columns.Add("Nome", "Nome");
            _dgvEvents.Columns.Add("Timestamp", "Data/Hora");

            grpList.Controls.Add(_dgvEvents);
            panelList.Controls.Add(grpList);
            _split.Panel2.Controls.Add(panelList);

            this.Controls.Add(_split);
        }

        private void EventsControl_Load(object? sender, EventArgs e)
        {
            if (_split.Width > 0)
                _split.SplitterDistance = Math.Clamp(350, 0, _split.Width);
        }

        private void Split_SizeChanged(object? sender, EventArgs e)
        {
            if (_split.Width > 0)
                _split.SplitterDistance = Math.Clamp(350, 0, _split.Width);
        }

        private async Task RegisterEventAsync()
        {
            if (string.IsNullOrWhiteSpace(_txtValor.Text))
            {
                MessageBox.Show("Valor é obrigatório!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tipo = _cmbTipo.SelectedItem?.ToString() ?? "TAG";
            await RegisterEventInternalAsync(tipo, _txtValor.Text, _txtNome.Text);
        }

        private async Task SimulateEventAsync(string tipo, string valor, string nome)
        {
            _cmbTipo.SelectedItem = tipo;
            _txtValor.Text = valor;
            _txtNome.Text = nome;
            await RegisterEventInternalAsync(tipo, valor, nome);
        }

        private async Task RegisterEventInternalAsync(string tipo, string valor, string? nome)
        {
            var result = await _apiService.CreateEventAsync(tipo, valor, nome);
            if (result.Success && result.Data != null)
            {
                _events.Insert(0, result.Data);
                RefreshGrid();
                MessageBox.Show($"Evento registrado! ID: {result.Data.Id}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Erro: {result.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid()
        {
            _dgvEvents.Rows.Clear();
            foreach (var e in _events.Take(100))
            {
                _dgvEvents.Rows.Add(e.Id, e.Tipo, e.Valor, e.Nome, e.Timestamp);
            }
        }

        public void OnConnected()
        {
            // Não precisa carregar dados, apenas limpa
            _events.Clear();
            RefreshGrid();
        }
    }
}
