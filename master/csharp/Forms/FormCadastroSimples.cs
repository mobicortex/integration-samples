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
    //  - GET  /entities?offset=X&count=Y[&name=filtro]  (lista paginada global)
    //  - GET  /entities?id=X                            (busca por ID)
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroSimples : Form
    {
        private readonly MobiCortexApiService _api;
        private Entidade? _entidadeSelecionada;

        // Estado de paginação server-side
        private int _currentOffset = 0;
        private uint _totalEntidades = 0;
        private const int PageSize = 10;

        // Filtro de texto atual (enviado ao servidor)
        private string _filtroNome = "";

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
            await CarregarEntidades();
        }

        /// <summary>
        /// Carrega a página atual de entidades do servidor.
        /// GET /entities?offset=X&amp;count=Y[&amp;name=filtro]
        /// Uma única chamada HTTP — paginação e filtro são feitos no servidor.
        /// </summary>
        private async Task CarregarEntidades()
        {
            lblStatusEntidades.Text = "Carregando...";
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            var result = await _api.ListarEntidadesGlobalAsync(
                _currentOffset, PageSize,
                nome: string.IsNullOrEmpty(_filtroNome) ? null : _filtroNome);

            if (!result.Success || result.Data == null)
            {
                lblStatusEntidades.Text = $"Erro: {result.Message}";
                AtualizarPaginacao();
                return;
            }

            _totalEntidades = result.Data.Total;

            foreach (var ent in result.Data.Items)
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
            int totalPages = (int)((_totalEntidades + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalEntidades;
            lblStatusEntidades.Text = $"{_totalEntidades} entidade(s) encontrada(s)";
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
            // Seleciona o tipo em um formulario dedicado (mais claro do que Yes/No).
            using var formTipo = new FormSelecionarTipoEntidade();
            if (formTipo.ShowDialog(this) != DialogResult.OK) return;
            int tipo = formTipo.TipoEntidadeSelecionado;

            var nome = InputBox("Nova Entidade",
                tipo == 1 ? "Nome da pessoa:" : "Nome do proprietário:");
            if (string.IsNullOrEmpty(nome)) return;

            var doc = InputBox("Documento",
                tipo == 1 ? "CPF (opcional):" : "Placa (ex: ABC1D23):");

            string? brand = null;
            string? model = null;
            string? color = null;
            string? obs = null;
            int lprAtivo = 0;

            if (tipo == (int)TipoEntidade.Veiculo)
            {
                var placaNormalizada = (doc ?? string.Empty).Trim().ToUpper().Replace("-", "");
                if (string.IsNullOrWhiteSpace(placaNormalizada))
                {
                    Aviso("Placa é obrigatória para veículo.");
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(placaNormalizada, @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$") &&
                    !System.Text.RegularExpressions.Regex.IsMatch(placaNormalizada, @"^[A-Z]{3}[0-9]{4}$"))
                {
                    Aviso("Placa inválida. Formatos aceitos: ABC1234 ou ABC1D23.");
                    return;
                }

                doc = placaNormalizada;
                brand = InputBox("Opcional - Marca", "Marca do veículo (opcional):")?.Trim();
                model = InputBox("Opcional - Modelo", "Modelo do veículo (opcional):")?.Trim();
                color = InputBox("Opcional - Cor", "Cor do veículo (opcional):")?.Trim();
                obs = InputBox("Opcional - Observações", "Observações (opcional):")?.Trim();
                lprAtivo = ConfirmarSimNao("Ativar LPR automático para esta placa?", "LPR");
            }

            // *** CREATEID = TRUE ***  ← Esta é a diferença principal!
            var request = new CriarEntidadeRequest
            {
                // NÃO informamos cadastro_id (será gerado automaticamente)
                Id = 0,
                CreateId = true,     // ← Gera IDs automaticamente
                Tipo = tipo,
                Name = nome,
                Doc = doc ?? "",
                Brand = string.IsNullOrWhiteSpace(brand) ? null : brand,
                Model = string.IsNullOrWhiteSpace(model) ? null : model,
                Color = string.IsNullOrWhiteSpace(color) ? null : color,
                Obs = string.IsNullOrWhiteSpace(obs) ? null : obs,
                LprAtivo = lprAtivo
            };

            var result = await _api.CriarEntidadeAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entidade criada: {nome}");
                Log($"  → entity_id={result.Data.EntityId}, cadastro_id={result.Data.CadastroId}");
                if (result.Data.CreatedCentral == 1)
                    Log($"  → Cadastro central criado automaticamente");
                _currentOffset = 0;
                await CarregarEntidades();
            }
            else
            {
                var msg = $"Erro ao criar entidade:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
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
                _currentOffset = 0;
                await CarregarEntidades();
            }
            else
            {
                var msg = $"Erro ao excluir entidade:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
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

            // Filtro por texto — enviado ao servidor
            _filtroNome = filtro;
            await CarregarEntidades();
        }

        private async void btnAnterior_Click(object? sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            await CarregarEntidades();
        }

        private async void btnProxima_Click(object? sender, EventArgs e)
        {
            _currentOffset += PageSize;
            await CarregarEntidades();
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

                _filtroNome = filtro;
                await CarregarEntidades();
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
        /// Abre o formulário de cadastro de mídia para criar uma nova mídia
        /// vinculada à entidade selecionada.
        /// </summary>
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            using var form = new FormCadastroMidia(_entidadeSelecionada.EntityId, _entidadeSelecionada.Doc);
            
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // LPR só deve ser cadastrado para entidades do tipo veículo.
                if (form.TipoMidiaSelecionado == TipoMidia.Lpr &&
                    _entidadeSelecionada.Tipo != (int)TipoEntidade.Veiculo)
                {
                    Aviso("A mídia LPR (placa) só pode ser cadastrada para entidades do tipo Veículo.");
                    return;
                }

                var request = new CriarMidiaRequest
                {
                    EntityId = _entidadeSelecionada.EntityId,
                    CadastroId = _entidadeSelecionada.CadastroId,
                    Tipo = form.TipoMidiaSelecionado,
                    Descricao = form.TipoMidiaSelecionado == TipoMidia.Lpr &&
                                !string.IsNullOrWhiteSpace(_entidadeSelecionada.Doc)
                        ? _entidadeSelecionada.Doc
                        : form.DadosMidia
                };

                // EXPLICAÇÃO: O backend valida o formato da mídia baseado no conteúdo
                // do campo "descricao". Para RFID, ele aceita formatos Wiegand/CODE/HEX.
                // Para LPR (placa), se enviarmos apenas a descricao, o backend tenta
                // validar como RFID e retorna erro "formato RFID invalido".
                // 
                // SOLUÇÃO: Enviar ns32_0 e ns32_1 indica ao backend que os dados binários
                // já foram processados, então ele não aplica a validação RFID.
                // Para LPR manual, enviamos 0 em ambos (o backend ignora para LPR).
                // 
                // NOTA: A forma RECOMENDADA de criar LPR é usando lpr_ativo=1 no cadastro
                // da entidade (veículo), não via POST /media manual.
                if (form.TipoMidiaSelecionado == TipoMidia.Lpr)
                {
                    request.Ns32_0 = 0;
                    request.Ns32_1 = 0;
                }

                var result = await _api.CriarMidiaAsync(request);
                if (result.Success && result.Data?.Ret == 0)
                {
                    Log($"Mídia criada: {form.TipoMidiaNome} - {form.DadosMidia} (ID: {result.Data.MediaId})");
                    await CarregarMidias(_entidadeSelecionada.EntityId);
                }
                else
                {
                    var msg = $"Erro ao criar mídia:\n{result.Message}";
                    Log(msg);
                    Aviso(msg);
                }
            }
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
            {
                var msg = $"Erro ao excluir mídia:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
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

        /// <summary>Exibe diálogo Sim/Não e retorna 1 (sim) ou 0 (não).</summary>
        private int ConfirmarSimNao(string pergunta, string titulo)
        {
            var resposta = MessageBox.Show(
                pergunta,
                titulo,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            return resposta == DialogResult.Yes ? 1 : 0;
        }
    }
}
