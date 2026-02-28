using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    #region Novo Cadastro Form

    public class NovoCadastroForm : Form
    {
        private TextBox _txtNome = null!;
        public string NomeCadastro => _txtNome.Text.Trim();

        public NovoCadastroForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "➕ Novo Cadastro";
            this.Size = new Size(450, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Label
            var lblNome = new Label
            {
                Text = "Nome do Cadastro:",
                Location = new Point(20, 20),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblNome);

            // TextBox
            _txtNome = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtNome);

            // Label exemplo
            var lblExemplo = new Label
            {
                Text = "Exemplo: Moradores, Visitantes, Funcionários",
                Location = new Point(20, 75),
                Size = new Size(400, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(lblExemplo);

            // Botão Salvar
            var btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(240, 110),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnSalvar);

            // Botão Cancelar
            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(340, 110),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtNome.Focus();
        }
    }

    #endregion

    #region Abrir Cadastro Form

    public class AbrirCadastroForm : Form
    {
        private readonly int _cadastroId;
        private readonly string _cadastroNome;

        public AbrirCadastroForm(int cadastroId, string cadastroNome)
        {
            _cadastroId = cadastroId;
            _cadastroNome = cadastroNome;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"📂 Cadastro: {_cadastroNome}";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // Layout principal
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Header
            var lblHeader = new Label
            {
                Text = $"Gerenciando Cadastro #{_cadastroId}",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };
            panel.Controls.Add(lblHeader);

            // Informações
            var lblInfo = new Label
            {
                Text = $"Nome: {_cadastroNome}\n" +
                       $"ID: {_cadastroId}\n\n" +
                       "Funcionalidades disponíveis neste cadastro:\n" +
                       "• Adicionar entidades (pessoas, veículos)\n" +
                       "• Gerenciar mídias (RFID, placas, etc)\n" +
                       "• Editar informações\n" +
                       "• Exportar dados",
                Location = new Point(20, 60),
                Size = new Size(640, 200),
                Font = new Font("Segoe UI", 10)
            };
            panel.Controls.Add(lblInfo);

            // Botão Fechar
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(580, 400),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnFechar.Click += (s, e) => this.Close();
            panel.Controls.Add(btnFechar);

            this.Controls.Add(panel);
        }
    }

    #endregion

    #region Nova Entidade Form

    public class NovaEntidadeForm : Form
    {
        private readonly int _cadastroId;
        private TextBox _txtNome = null!;
        private TextBox _txtDoc = null!;
        private ComboBox _cmbTipo = null!;
        private CheckBox _chkHabilitado = null!;

        public EntityData EntityData => new()
        {
            Name = _txtNome.Text.Trim(),
            Doc = _txtDoc.Text.Trim(),
            Tipo = _cmbTipo.SelectedIndex + 1,
            Habilitado = _chkHabilitado.Checked ? 1 : 0
        };

        public NovaEntidadeForm(int cadastroId)
        {
            _cadastroId = cadastroId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "➕ Nova Entidade";
            this.Size = new Size(450, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;

            // Nome
            var lblNome = new Label
            {
                Text = "Nome:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblNome);

            _txtNome = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtNome);
            y += 40;

            // Documento
            var lblDoc = new Label
            {
                Text = "Documento:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDoc);

            _txtDoc = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtDoc);
            y += 40;

            // Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblTipo);

            _cmbTipo = new ComboBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "Pessoa (1)", "Veículo (2)", "Animal (3)", "Outro (4)" });
            _cmbTipo.SelectedIndex = 0;
            this.Controls.Add(_cmbTipo);
            y += 40;

            // Habilitado
            _chkHabilitado = new CheckBox
            {
                Text = "Habilitado",
                Location = new Point(130, y),
                Size = new Size(150, 25),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_chkHabilitado);
            y += 50;

            // Botões
            var btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(240, y),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnSalvar);

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(340, y),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtNome.Focus();
        }
    }

    #endregion

    #region Editar Entidade Form

    public class EditarEntidadeForm : Form
    {
        private TextBox _txtNome = null!;
        private TextBox _txtDoc = null!;
        private ComboBox _cmbTipo = null!;
        private CheckBox _chkHabilitado = null!;

        public EntityData EntityData => new()
        {
            Name = _txtNome.Text.Trim(),
            Doc = _txtDoc.Text.Trim(),
            Tipo = _cmbTipo.SelectedIndex + 1,
            Habilitado = _chkHabilitado.Checked ? 1 : 0
        };

        public EditarEntidadeForm(EntidadeItem entidade)
        {
            InitializeComponent();
            _txtNome.Text = entidade.Name;
        }

        private void InitializeComponent()
        {
            this.Text = "✏️ Editar Entidade";
            this.Size = new Size(450, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;

            // Nome
            var lblNome = new Label
            {
                Text = "Nome:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblNome);

            _txtNome = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtNome);
            y += 40;

            // Documento
            var lblDoc = new Label
            {
                Text = "Documento:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDoc);

            _txtDoc = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtDoc);
            y += 40;

            // Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblTipo);

            _cmbTipo = new ComboBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "Pessoa (1)", "Veículo (2)", "Animal (3)", "Outro (4)" });
            _cmbTipo.SelectedIndex = 0;
            this.Controls.Add(_cmbTipo);
            y += 40;

            // Habilitado
            _chkHabilitado = new CheckBox
            {
                Text = "Habilitado",
                Location = new Point(130, y),
                Size = new Size(150, 25),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_chkHabilitado);
            y += 50;

            // Botões
            var btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(240, y),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnSalvar);

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(340, y),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtNome.Focus();
            _txtNome.SelectAll();
        }
    }

    #endregion

    #region Gerenciar Mídias Form

    public class GerenciarMidiasForm : Form
    {
        private readonly MobiCortexApiService _apiService;
        private readonly int _entidadeId;
        private readonly string _entidadeNome;
        
        private ListBox _lstMidias = null!;
        private Button _btnAdicionar = null!;
        private Button _btnRemover = null!;
        private Label _lblInfo = null!;

        public GerenciarMidiasForm(MobiCortexApiService apiService, int entidadeId, string entidadeNome)
        {
            _apiService = apiService;
            _entidadeId = entidadeId;
            _entidadeNome = entidadeNome;
            InitializeComponent();
            _ = CarregarMidiasAsync();
        }

        private void InitializeComponent()
        {
            this.Text = $"📎 Mídias - {_entidadeNome}";
            this.Size = new Size(550, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // Header
            var lblHeader = new Label
            {
                Text = $"Gerenciando Mídias da Entidade #{_entidadeId}",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                Padding = new Padding(10)
            };
            this.Controls.Add(lblHeader);

            // Panel principal
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Location = new Point(0, 40),
                Height = this.Height - 40 - 80
            };

            // Lista de mídias
            var lblMidias = new Label
            {
                Text = "Mídias cadastradas:",
                Location = new Point(10, 10),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panel.Controls.Add(lblMidias);

            _lstMidias = new ListBox
            {
                Location = new Point(10, 35),
                Size = new Size(400, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            panel.Controls.Add(_lstMidias);

            // Botões
            _btnAdicionar = new Button
            {
                Text = "➕ Adicionar",
                Location = new Point(420, 35),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAdicionar.Click += BtnAdicionar_Click;
            panel.Controls.Add(_btnAdicionar);

            _btnRemover = new Button
            {
                Text = "🗑️ Remover",
                Location = new Point(420, 75),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            _btnRemover.Click += BtnRemover_Click;
            panel.Controls.Add(_btnRemover);

            _lstMidias.SelectedIndexChanged += (s, e) => _btnRemover.Enabled = _lstMidias.SelectedIndex >= 0;

            // Info
            _lblInfo = new Label
            {
                Text = "Carregando mídias...",
                Location = new Point(10, 295),
                Size = new Size(510, 25),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };
            panel.Controls.Add(_lblInfo);

            // Label de tipos de mídia
            var lblTipos = new Label
            {
                Text = "Tipos de mídia suportados:\n" +
                       "• RFID (tags de proximidade)\n" +
                       "• Placa (reconhecimento LPR)\n" +
                       "• Facial (biometria facial)\n" +
                       "• Senha (teclado numérico)\n" +
                       "• Biometria (impressão digital)",
                Location = new Point(10, 325),
                Size = new Size(510, 100),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            panel.Controls.Add(lblTipos);

            this.Controls.Add(panel);

            // Botão Fechar (na parte inferior)
            var btnFechar = new Button
            {
                Text = "Fechar",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFechar.Click += (s, e) => this.Close();
            this.Controls.Add(btnFechar);
        }

        private void BtnAdicionar_Click(object? sender, EventArgs e)
        {
            var form = new NovaMidiaForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = AdicionarMidiaAsync(form.TipoMidia, form.ValorMidia);
            }
        }

        private void BtnRemover_Click(object? sender, EventArgs e)
        {
            if (_lstMidias.SelectedItem is MidiaItem midia)
            {
                var result = MessageBox.Show(
                    $"Deseja remover a mídia '{midia.Nome}'?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _ = RemoverMidiaAsync(midia.Id);
                }
            }
        }

        private async Task CarregarMidiasAsync()
        {
            try
            {
                _lblInfo.Text = "Carregando mídias...";
                await Task.Delay(300);
                
                // Simulação - em produção, chamar endpoint /master/api/v1/media?entity_id={id}
                _lstMidias.Items.Clear();
                _lstMidias.Items.Add(new MidiaItem { Id = 1, Nome = "RFID: 12345678", Tipo = "RFID" });
                _lstMidias.Items.Add(new MidiaItem { Id = 2, Nome = "Placa: ABC1234", Tipo = "Placa" });
                
                _lblInfo.Text = $"{ _lstMidias.Items.Count} mídia(s) carregada(s)";
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }

        private async Task AdicionarMidiaAsync(string tipo, string valor)
        {
            try
            {
                _lblInfo.Text = "Adicionando mídia...";
                await Task.Delay(300);
                
                var novoId = _lstMidias.Items.Count + 1;
                _lstMidias.Items.Add(new MidiaItem 
                { 
                    Id = novoId, 
                    Nome = $"{tipo}: {valor}",
                    Tipo = tipo
                });
                
                _lblInfo.Text = "Mídia adicionada com sucesso!";
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }

        private async Task RemoverMidiaAsync(int midiaId)
        {
            try
            {
                _lblInfo.Text = "Removendo mídia...";
                await Task.Delay(300);
                
                _lstMidias.Items.RemoveAt(_lstMidias.SelectedIndex);
                
                _lblInfo.Text = "Mídia removida com sucesso!";
            }
            catch (Exception ex)
            {
                _lblInfo.Text = $"Erro: {ex.Message}";
            }
        }
    }

    #endregion

    #region Nova Midia Form

    public class NovaMidiaForm : Form
    {
        private ComboBox _cmbTipo = null!;
        private TextBox _txtValor = null!;

        public string TipoMidia => _cmbTipo.SelectedItem?.ToString()?.Split(' ')[0] ?? "RFID";
        public string ValorMidia => _txtValor.Text.Trim();

        public NovaMidiaForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "➕ Nova Mídia";
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;

            // Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(20, y),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblTipo);

            _cmbTipo = new ComboBox
            {
                Location = new Point(110, y),
                Size = new Size(260, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { 
                "RFID (TAG de proximidade)",
                "Placa (LPR)",
                "Facial (Biometria)",
                "Senha (Teclado)",
                "Biometria (Digital)"
            });
            _cmbTipo.SelectedIndex = 0;
            _cmbTipo.SelectedIndexChanged += CmbTipo_SelectedIndexChanged;
            this.Controls.Add(_cmbTipo);
            y += 40;

            // Valor
            var lblValor = new Label
            {
                Text = "Valor:",
                Location = new Point(20, y),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblValor);

            _txtValor = new TextBox
            {
                Location = new Point(110, y),
                Size = new Size(260, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtValor);
            y += 50;

            // Botões
            var btnSalvar = new Button
            {
                Text = "💾 Adicionar",
                Location = new Point(190, y),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnSalvar);

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(290, y),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;

            AtualizarPlaceholder();
        }

        private void CmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AtualizarPlaceholder();
        }

        private void AtualizarPlaceholder()
        {
            // Em um controle real, definiríamos o placeholder aqui
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtValor.Focus();
        }
    }

    #endregion

    #region Novo Usuario Form (Central Registry)

    public class NovoUsuarioForm : Form
    {
        private TextBox _txtNome = null!;
        private CheckBox _chkEnabled = null!;
        private NumericUpDown _numSlots1 = null!;
        private NumericUpDown _numSlots2 = null!;
        private ComboBox _cmbTipo = null!;

        public CentralRegistryUser Usuario => new()
        {
            Id = 0, // 0 = auto-assign
            Name = _txtNome.Text.Trim(),
            Enabled = _chkEnabled.Checked,
            Slots1 = (int)_numSlots1.Value,
            Slots2 = (int)_numSlots2.Value,
            Type = _cmbTipo.SelectedIndex + 1
        };

        public NovoUsuarioForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "➕ Novo Usuário";
            this.Size = new Size(450, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;

            // Nome
            var lblNome = new Label
            {
                Text = "Nome:*",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblNome);

            _txtNome = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtNome);
            y += 40;

            // Habilitado
            _chkEnabled = new CheckBox
            {
                Text = "Habilitado",
                Location = new Point(130, y),
                Size = new Size(150, 25),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_chkEnabled);
            y += 35;

            // Slots1
            var lblSlots1 = new Label
            {
                Text = "Vagas G1:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblSlots1);

            _numSlots1 = new NumericUpDown
            {
                Location = new Point(130, y),
                Size = new Size(80, 25),
                Minimum = 0,
                Maximum = 15,
                Value = 0,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_numSlots1);

            // Slots2
            var lblSlots2 = new Label
            {
                Text = "Vagas G2:",
                Location = new Point(220, y),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblSlots2);

            _numSlots2 = new NumericUpDown
            {
                Location = new Point(310, y),
                Size = new Size(80, 25),
                Minimum = 0,
                Maximum = 15,
                Value = 0,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_numSlots2);
            y += 40;

            // Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblTipo);

            _cmbTipo = new ComboBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "Tipo 1", "Tipo 2", "Tipo 3", "Tipo 4", "Tipo 5" });
            _cmbTipo.SelectedIndex = 0;
            this.Controls.Add(_cmbTipo);
            y += 50;

            // Botões
            var btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(240, y),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSalvar.Click += (s, e) => 
            {
                if (string.IsNullOrWhiteSpace(_txtNome.Text))
                {
                    MessageBox.Show("Nome é obrigatório!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            };
            this.Controls.Add(btnSalvar);

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(340, y),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtNome.Focus();
        }
    }

    #endregion

    #region Editar Usuario Form (Central Registry)

    public class EditarUsuarioForm : Form
    {
        private TextBox _txtNome = null!;
        private CheckBox _chkEnabled = null!;
        private NumericUpDown _numSlots1 = null!;
        private NumericUpDown _numSlots2 = null!;
        private ComboBox _cmbTipo = null!;
        private Label _lblId = null!;

        public CentralRegistryUser Usuario { get; private set; }

        public EditarUsuarioForm(CentralRegistryUser user)
        {
            Usuario = user;
            InitializeComponent();
            PreencherDados();
        }

        private void InitializeComponent()
        {
            this.Text = "✏️ Editar Usuário";
            this.Size = new Size(450, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;

            // ID (somente leitura)
            var lblIdTitulo = new Label
            {
                Text = "ID:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblIdTitulo);

            _lblId = new Label
            {
                Location = new Point(130, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255)
            };
            this.Controls.Add(_lblId);
            y += 35;

            // Nome
            var lblNome = new Label
            {
                Text = "Nome:*",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblNome);

            _txtNome = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_txtNome);
            y += 40;

            // Habilitado
            _chkEnabled = new CheckBox
            {
                Text = "Habilitado",
                Location = new Point(130, y),
                Size = new Size(150, 25),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_chkEnabled);
            y += 35;

            // Slots1
            var lblSlots1 = new Label
            {
                Text = "Vagas G1:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblSlots1);

            _numSlots1 = new NumericUpDown
            {
                Location = new Point(130, y),
                Size = new Size(80, 25),
                Minimum = 0,
                Maximum = 15,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_numSlots1);

            // Slots2
            var lblSlots2 = new Label
            {
                Text = "Vagas G2:",
                Location = new Point(220, y),
                Size = new Size(80, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblSlots2);

            _numSlots2 = new NumericUpDown
            {
                Location = new Point(310, y),
                Size = new Size(80, 25),
                Minimum = 0,
                Maximum = 15,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_numSlots2);
            y += 40;

            // Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(20, y),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblTipo);

            _cmbTipo = new ComboBox
            {
                Location = new Point(130, y),
                Size = new Size(290, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cmbTipo.Items.AddRange(new object[] { "Tipo 1", "Tipo 2", "Tipo 3", "Tipo 4", "Tipo 5" });
            this.Controls.Add(_cmbTipo);
            y += 50;

            // Botões
            var btnSalvar = new Button
            {
                Text = "💾 Salvar",
                Location = new Point(240, y),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSalvar.Click += (s, e) => 
            {
                if (string.IsNullOrWhiteSpace(_txtNome.Text))
                {
                    MessageBox.Show("Nome é obrigatório!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                
                Usuario.Name = _txtNome.Text.Trim();
                Usuario.Enabled = _chkEnabled.Checked;
                Usuario.Slots1 = (int)_numSlots1.Value;
                Usuario.Slots2 = (int)_numSlots2.Value;
                Usuario.Type = _cmbTipo.SelectedIndex + 1;
            };
            this.Controls.Add(btnSalvar);

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar",
                Location = new Point(340, y),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnSalvar;
            this.CancelButton = btnCancelar;
        }

        private void PreencherDados()
        {
            _lblId.Text = Usuario.Id.ToString();
            _txtNome.Text = Usuario.Name;
            _chkEnabled.Checked = Usuario.Enabled;
            _numSlots1.Value = Math.Min(15, Math.Max(0, Usuario.Slots1));
            _numSlots2.Value = Math.Min(15, Math.Max(0, Usuario.Slots2));
            _cmbTipo.SelectedIndex = Math.Min(4, Math.Max(0, Usuario.Type - 1));
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _txtNome.Focus();
            _txtNome.SelectAll();
        }
    }

    #endregion

    #region Classes de Dados Auxiliares

    public class MidiaItem
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Tipo { get; set; } = "";
        public override string ToString() => Nome;
    }

    public class EntityData
    {
        public string Name { get; set; } = "";
        public string Doc { get; set; } = "";
        public int Tipo { get; set; } = 1;
        public int Habilitado { get; set; } = 1;
    }

    public class EntidadeItem
    {
        public int Id { get; set; }
        public int CadastroId { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() => Name;
    }

    #endregion
}
