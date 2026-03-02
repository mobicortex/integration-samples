using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Forms
{
    // =============================================================================
    //  CADASTRO SIMPLIFICADO (2 Níveis)
    //
    //  Este formulário demonstra o modelo simplificado de cadastro.
    //
    //  Neste modelo, o integrador NÃO precisa gerenciar Cadastros Centrais.
    //  Basta criar a entidade com createid=true e o controlador gera
    //  automaticamente o cadastro_id e entity_id internamente.
    //
    //  FLUXO:
    //  1. Criar Entidade com createid=true (o controlador cria tudo)
    //  2. Adicionar Mídias à entidade
    //
    //  DIFERENÇA DO MODELO COMPLETO:
    //  - Completo: Cadastro → Entidade → Mídia (3 níveis, o integrador gerencia tudo)
    //  - Simplificado: Entidade → Mídia (2 níveis, o controlador gera IDs)
    //
    //  ENDPOINTS USADOS:
    //  - POST /entities  (com createid=true)
    //  - GET  /entities?cadastro_id=X  (lista entidades)
    //  - GET  /entities?id=X           (busca por ID)
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroSimples : Form
    {
        private readonly MobiCortexApiService _api;
        private Entidade? _entidadeSelecionada;

        // Cache de todas as entidades carregadas da API
        private List<Entidade> _todasEntidades = new();
        // Lista filtrada (pode ser == _todasEntidades se sem filtro)
        private List<Entidade> _entidadesFiltradas = new();

        // Estado de paginação (client-side sobre _entidadesFiltradas)
        private int _currentOffset = 0;
        private const int PageSize = 20;

        public FormCadastroSimples(MobiCortexApiService api)
        {
            _api = api;
            InitializeComponent();
        }

        // =====================================================================
        //  ENTIDADES (Nível 1 no modelo simplificado)
        //  Diferença: usamos createid=true para criar
        // =====================================================================

        private async void FormCadastroSimples_Load(object? sender, EventArgs e)
        {
            await CarregarTodasEntidades();
        }

        /// <summary>
        /// Carrega TODAS as entidades do controlador (iterando por todos os cadastros).
        /// Armazena em _todasEntidades e depois renderiza a página atual.
        ///
        /// A API não tem endpoint para listar todas as entidades diretamente.
        /// Precisamos iterar: /central-registry (paginado) → /entities?cadastro_id=X
        /// </summary>
        private async Task CarregarTodasEntidades()
        {
            lblStatusEntidades.Text = "Carregando...";
            _todasEntidades.Clear();
            _entidadesFiltradas.Clear();
            _currentOffset = 0;
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            // 1. Busca a primeira página de cadastros para saber o total
            var primeiraPagina = await _api.ListarCadastrosAsync(0, PageSize);
            if (!primeiraPagina.Success || primeiraPagina.Data == null)
            {
                lblStatusEntidades.Text = $"Erro: {primeiraPagina.Message}";
                AtualizarPaginacao();
                return;
            }

            uint totalCadastros = primeiraPagina.Data.Total;
            var todosCadastros = new List<CadastroCentral>(primeiraPagina.Data.Items);

            // 2. Busca as páginas restantes de cadastros
            int offset = PageSize;
            while (offset < totalCadastros)
            {
                var pagina = await _api.ListarCadastrosAsync(offset, PageSize);
                if (pagina.Success && pagina.Data != null)
                    todosCadastros.AddRange(pagina.Data.Items);
                offset += PageSize;
            }

            // 3. Para cada cadastro, busca suas entidades
            foreach (var cad in todosCadastros)
            {
                lblStatusEntidades.Text = $"Carregando entidades... ({_todasEntidades.Count} encontradas)";
                var ents = await _api.ListarEntidadesAsync(cad.Id);
                if (ents.Success && ents.Data != null)
                    _todasEntidades.AddRange(ents.Data.Items);
            }

            Log($"{_todasEntidades.Count} entidade(s) carregada(s) de {todosCadastros.Count} cadastro(s)");
            AplicarFiltroERenderizar();
        }

        /// <summary>
        /// Aplica o filtro de texto sobre _todasEntidades,
        /// atualiza _entidadesFiltradas e renderiza a página atual.
        /// NÃO faz chamadas à API.
        /// </summary>
        private void AplicarFiltroERenderizar()
        {
            var filtro = txtFiltroEntidade.Text.Trim();

            if (!string.IsNullOrEmpty(filtro) && !uint.TryParse(filtro, out _))
            {
                _entidadesFiltradas = _todasEntidades
                    .Where(e => e.Name.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                             || e.Doc.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                _entidadesFiltradas = _todasEntidades;
            }

            RenderizarPagina();
        }

        /// <summary>
        /// Renderiza a página atual de _entidadesFiltradas na ListView.
        /// NÃO faz chamadas à API.
        /// </summary>
        private void RenderizarPagina()
        {
            listEntidades.Items.Clear();

            var pagina = _entidadesFiltradas
                .Skip(_currentOffset)
                .Take(PageSize);

            foreach (var ent in pagina)
            {
                var item = new ListViewItem(ent.EntityId.ToString());
                item.SubItems.Add(ent.TipoNome);
                item.SubItems.Add(ent.Name);
                item.SubItems.Add(ent.Doc);
                item.SubItems.Add(ent.CadastroId.ToString());
                item.Tag = ent;
                listEntidades.Items.Add(item);
            }

            AtualizarPaginacao();
        }

        /// <summary>
        /// Busca uma entidade específica pelo entity_id.
        /// GET /entities?id=X
        /// </summary>
        private async Task BuscarEntidadePorId(uint entityId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            var result = await _api.ObterEntidadeAsync(entityId);

            if (result.Success && result.Data != null && result.Data.Ret == 0)
            {
                var ent = result.Data;
                var item = new ListViewItem(ent.EntityId.ToString());
                item.SubItems.Add(ent.TipoNome);
                item.SubItems.Add(ent.Name);
                item.SubItems.Add(ent.Doc);
                item.SubItems.Add(ent.CadastroId.ToString());
                item.Tag = ent;
                listEntidades.Items.Add(item);

                lblStatusEntidades.Text = $"Busca por ID {entityId} — 1 resultado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                lblStatusEntidades.Text = $"ID {entityId} não encontrado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Atualiza label de status e botões de paginação</summary>
        private void AtualizarPaginacao()
        {
            uint total = (uint)_entidadesFiltradas.Count;
            int totalPages = (int)((total + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < total;
            lblStatusEntidades.Text = $"{total} entidade(s) encontrada(s)";
        }

        /// <summary>Ao selecionar uma entidade, carrega suas mídias.</summary>
        private async void listEntidades_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _entidadeSelecionada = listEntidades.SelectedItems[0].Tag as Entidade;
            if (_entidadeSelecionada == null) return;

            lblMidiasTitulo.Text = $"Mídias de: {_entidadeSelecionada.Name}";
            await CarregarMidias(_entidadeSelecionada.EntityId);
        }

        /// <summary>
        /// Cria uma entidade usando o modelo simplificado (createid=true).
        ///
        /// IMPORTANTE: A diferença principal deste modelo é o campo createid.
        /// Quando createid=true, o controlador:
        /// 1. Gera um entity_id automaticamente
        /// 2. Cria um cadastro central automaticamente (ou reutiliza um existente)
        /// 3. Retorna entity_id e cadastro_id na resposta
        ///
        /// O integrador NÃO precisa criar o cadastro central antes.
        /// </summary>
        private async void btnNovaEntidade_Click(object? sender, EventArgs e)
        {
            // Pergunta o tipo (pessoa ou veículo)
            var tipoDlg = MessageBox.Show("Pessoa? (Sim = Pessoa, Não = Veículo)",
                "Tipo de Entidade", MessageBoxButtons.YesNoCancel);
            if (tipoDlg == DialogResult.Cancel) return;

            int tipo = tipoDlg == DialogResult.Yes ? (int)TipoEntidade.Pessoa : (int)TipoEntidade.Veiculo;

            var nome = InputBox("Nova Entidade",
                tipo == 1 ? "Nome da pessoa:" : "Descrição do veículo:");
            if (string.IsNullOrEmpty(nome)) return;

            var doc = InputBox("Documento",
                tipo == 1 ? "CPF (opcional):" : "Placa (ex: ABC1D23):");

            // *** CREATEID = TRUE ***  ← Esta é a diferença principal!
            var request = new CriarEntidadeRequest
            {
                // NÃO informamos cadastro_id (será gerado automaticamente)
                CreateId = true,     // ← Gera IDs automaticamente
                Tipo = tipo,
                Name = nome,
                Doc = doc ?? "",
                LprAtivo = (tipo == (int)TipoEntidade.Veiculo && !string.IsNullOrEmpty(doc)) ? 1 : 0
            };

            var result = await _api.CriarEntidadeAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entidade criada: {nome}");
                Log($"  → entity_id={result.Data.EntityId}, cadastro_id={result.Data.CadastroId}");
                if (result.Data.CreatedCentral == 1)
                    Log($"  → Cadastro central criado automaticamente");
                await CarregarTodasEntidades();
            }
            else
                Log($"Erro ao criar entidade: {result.Message}");
        }

        /// <summary>
        /// Exclui a entidade selecionada.
        /// DELETE /entities?id=X
        /// </summary>
        private async void btnExcluirEntidade_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            var confirm = MessageBox.Show(
                $"Excluir '{_entidadeSelecionada.Name}' e todas suas mídias?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.ExcluirEntidadeAsync(_entidadeSelecionada.EntityId);
            if (result.Success)
            {
                Log($"Entidade excluída: {_entidadeSelecionada.Name}");
                await CarregarTodasEntidades();
            }
            else
                Log($"Erro: {result.Message}");
        }

        private async void btnBuscarEntidade_Click(object? sender, EventArgs e)
        {
            var filtro = txtFiltroEntidade.Text.Trim();
            _currentOffset = 0;

            // Se digitou um número, busca direto pela entity_id (API)
            if (uint.TryParse(filtro, out uint idBusca))
            {
                await BuscarEntidadePorId(idBusca);
                return;
            }

            // Se cache vazio (ainda não carregou), carrega da API
            if (_todasEntidades.Count == 0 && string.IsNullOrEmpty(filtro))
            {
                await CarregarTodasEntidades();
                return;
            }

            // Filtro por nome ou limpar filtro — usa cache, sem API
            AplicarFiltroERenderizar();
        }

        private void btnAnterior_Click(object? sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            RenderizarPagina();
        }

        private void btnProxima_Click(object? sender, EventArgs e)
        {
            _currentOffset += PageSize;
            RenderizarPagina();
        }

        /// <summary>Enter no campo de busca dispara a pesquisa</summary>
        private async void txtFiltroEntidade_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _currentOffset = 0;

                var filtro = txtFiltroEntidade.Text.Trim();
                if (uint.TryParse(filtro, out uint idBusca))
                {
                    await BuscarEntidadePorId(idBusca);
                    return;
                }

                if (_todasEntidades.Count == 0 && string.IsNullOrEmpty(filtro))
                {
                    await CarregarTodasEntidades();
                    return;
                }

                AplicarFiltroERenderizar();
            }
        }

        // =====================================================================
        //  MÍDIAS (Nível 2 no modelo simplificado)
        //  Funciona exatamente igual ao modelo completo
        // =====================================================================

        private async Task CarregarMidias(uint entityId)
        {
            listMidias.Items.Clear();

            var result = await _api.ListarMidiasAsync(entityId);
            if (result.Success && result.Data != null)
            {
                lblStatusMidias.Text = $"{result.Data.Count} mídia(s)";
                foreach (var m in result.Data.Items)
                {
                    var item = new ListViewItem(m.MediaId.ToString());
                    item.SubItems.Add(m.TipoNome);
                    item.SubItems.Add(m.Descricao);
                    item.SubItems.Add(m.Habilitado == 1 ? "Sim" : "Não");
                    item.Tag = m;
                    listMidias.Items.Add(item);
                }
            }
            else
            {
                lblStatusMidias.Text = $"Erro: {result.Message}";
            }
        }

        /// <summary>
        /// Cria uma nova mídia vinculada à entidade selecionada.
        /// Funciona igual ao modelo completo.
        /// </summary>
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            var tipos = new[] { "RFID Wiegand 26", "RFID Wiegand 34", "Placa (LPR)", "Facial" };
            var tipoIdx = SelecionarOpcao("Tipo de Mídia", "Selecione o tipo:", tipos);
            if (tipoIdx < 0) return;

            int tipoMidia = tipoIdx switch
            {
                0 => TipoMidia.Wiegand26,
                1 => TipoMidia.Wiegand34,
                2 => TipoMidia.Lpr,
                3 => TipoMidia.Facial,
                _ => TipoMidia.Wiegand26
            };

            var descricao = InputBox("Nova Mídia",
                tipoMidia == TipoMidia.Lpr ? "Placa (ex: ABC1D23):" : "Código/Descrição:");
            if (string.IsNullOrEmpty(descricao)) return;

            var request = new CriarMidiaRequest
            {
                EntityId = _entidadeSelecionada.EntityId,
                CadastroId = _entidadeSelecionada.CadastroId,
                Tipo = tipoMidia,
                Descricao = descricao
            };

            var result = await _api.CriarMidiaAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Mídia criada: {descricao} (ID: {result.Data.MediaId})");
                await CarregarMidias(_entidadeSelecionada.EntityId);
            }
            else
                Log($"Erro ao criar mídia: {result.Message}");
        }

        private async void btnExcluirMidia_Click(object? sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) { Aviso("Selecione uma mídia"); return; }

            var midia = listMidias.SelectedItems[0].Tag as MidiaAcesso;
            if (midia == null) return;

            var result = await _api.ExcluirMidiaAsync(midia.MediaId);
            if (result.Success)
            {
                Log($"Mídia excluída: {midia.Descricao}");
                if (_entidadeSelecionada != null)
                    await CarregarMidias(_entidadeSelecionada.EntityId);
            }
            else
                Log($"Erro: {result.Message}");
        }

        // =====================================================================
        //  HELPERS
        // =====================================================================

        private void Log(string msg)
        {
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(msg)); return; }
            var ts = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{ts}] {msg}{Environment.NewLine}");
        }

        private void Aviso(string msg) =>
            MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private string? InputBox(string titulo, string prompt)
        {
            var form = new Form { Text = titulo, Width = 400, Height = 150, StartPosition = FormStartPosition.CenterParent };
            var lbl = new Label { Text = prompt, Left = 10, Top = 10, Width = 360 };
            var txt = new TextBox { Left = 10, Top = 35, Width = 360 };
            var btnOk = new Button { Text = "OK", Left = 220, Top = 70, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Left = 300, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        private int SelecionarOpcao(string titulo, string prompt, string[] opcoes)
        {
            var form = new Form { Text = titulo, Width = 350, Height = 200, StartPosition = FormStartPosition.CenterParent };
            var lbl = new Label { Text = prompt, Left = 10, Top = 10, Width = 320 };
            var combo = new ComboBox { Left = 10, Top = 35, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(opcoes);
            combo.SelectedIndex = 0;
            var btnOk = new Button { Text = "OK", Left = 170, Top = 70, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancelar", Left = 250, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { lbl, combo, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form.ShowDialog() == DialogResult.OK ? combo.SelectedIndex : -1;
        }
    }
}
