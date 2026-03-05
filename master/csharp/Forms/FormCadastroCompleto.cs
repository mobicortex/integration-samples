using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Forms
{
    // =============================================================================
    //  CADASTRO COMPLETO - Modelo MobiCortex (3 Níveis)
    //
    //  Este formulário demonstra o modelo hierárquico completo:
    //
    //  1. CADASTRO CENTRAL (central-registry)
    //     Representa um "apartamento", "empresa", "unidade", etc.
    //     É o nó raiz que agrupa entidades.
    //
    //  2. ENTIDADE (entities)
    //     Representa uma pessoa, veículo ou animal vinculado ao cadastro.
    //     Cada cadastro pode ter várias entidades.
    //
    //  3. MÍDIA DE ACESSO (media)
    //     Representa uma credencial de acesso (cartão RFID, biometria, placa, etc).
    //     Cada entidade pode ter várias mídias.
    //
    //  FLUXO:
    //  1. Criar um Cadastro Central (ex: "Apt 101")
    //  2. Adicionar Entidades ao cadastro (ex: "João Silva", "Carro ABC-1234")
    //  3. Adicionar Mídias às entidades (ex: cartão RFID, placa LPR)
    //
    //  ENDPOINTS USADOS:
    //  - GET/POST/DELETE /central-registry
    //  - GET/POST/PUT/DELETE /entities
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroCompleto : Form
    {
        private readonly MobiCortexApiService _api;

        // Itens selecionados atualmente (para navegação hierárquica)
        private CadastroCentral? _cadastroSelecionado;
        private Entidade? _entidadeSelecionada;

        // Estado de paginação dos cadastros
        private int _currentOffset = 0;
        private const int PageSize = 20;
        private uint _totalCadastros = 0;

        public FormCadastroCompleto(MobiCortexApiService api)
        {
            _api = api;
            InitializeComponent();
        }

        // =====================================================================
        //  CADASTROS CENTRAIS (Nível 1)
        //  Endpoint: GET /central-registry?offset=0&count=20&name=filtro
        // =====================================================================

        private async void FormCadastroCompleto_Load(object? sender, EventArgs e)
        {
            await CarregarCadastros();
        }

        /// <summary>
        /// Carrega a lista de cadastros centrais com paginação.
        /// Se o campo de busca contém um número, busca pelo ID.
        /// Se contém texto, filtra pelo nome.
        /// </summary>
        private async Task CarregarCadastros()
        {
            var filtro = txtFiltroCadastro.Text.Trim();

            // Se digitou um número, busca direto pelo ID
            if (uint.TryParse(filtro, out uint idBusca))
            {
                await BuscarCadastroPorId(idBusca);
                return;
            }

            // Busca paginada (com filtro por nome opcional)
            var result = await _api.ListarCadastrosAsync(
                _currentOffset, PageSize,
                string.IsNullOrEmpty(filtro) ? null : filtro);

            listCadastros.Items.Clear();
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _cadastroSelecionado = null;
            _entidadeSelecionada = null;

            if (result.Success && result.Data != null)
            {
                _totalCadastros = result.Data.Total;
                foreach (var c in result.Data.Items)
                {
                    var item = new ListViewItem(c.Id.ToString());
                    item.SubItems.Add(c.Name);
                    item.SubItems.Add(c.Enabled ? "Sim" : "Não");
                    item.SubItems.Add($"{c.PeopleCount}P / {c.VehicleCount}V");
                    item.Tag = c;
                    listCadastros.Items.Add(item);
                }
                AtualizarPaginacao();
            }
            else
            {
                _totalCadastros = 0;
                AtualizarPaginacao();
                lblStatusCadastros.Text = $"Erro: {result.Message}";
            }
        }

        /// <summary>
        /// Busca um cadastro específico pelo ID e exibe na lista.
        /// GET /central-registry?id=X
        /// </summary>
        private async Task BuscarCadastroPorId(uint id)
        {
            listCadastros.Items.Clear();
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _cadastroSelecionado = null;
            _entidadeSelecionada = null;

            var result = await _api.ObterCadastroAsync(id);

            if (result.Success && result.Data != null)
            {
                var c = result.Data;
                var item = new ListViewItem(c.Id.ToString());
                item.SubItems.Add(c.Name);
                item.SubItems.Add(c.Enabled ? "Sim" : "Não");
                item.SubItems.Add($"{c.PeopleCount}P / {c.VehicleCount}V");
                item.Tag = c;
                listCadastros.Items.Add(item);

                _totalCadastros = 1;
                lblStatusCadastros.Text = $"Busca por ID {id} — 1 resultado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                _totalCadastros = 0;
                lblStatusCadastros.Text = $"ID {id} não encontrado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Atualiza label de status e botões de paginação</summary>
        private void AtualizarPaginacao()
        {
            int totalPages = (int)((_totalCadastros + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalCadastros;
            lblStatusCadastros.Text = $"{_totalCadastros} cadastro(s)";
        }

        /// <summary>Ao selecionar um cadastro, carrega suas entidades (nível 2).</summary>
        private async void listCadastros_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listCadastros.SelectedItems.Count == 0) return;

            _cadastroSelecionado = listCadastros.SelectedItems[0].Tag as CadastroCentral;
            if (_cadastroSelecionado == null) return;

            lblEntidadesTitulo.Text = $"Entidades de: {_cadastroSelecionado.Name}";
            await CarregarEntidades(_cadastroSelecionado.Id);
        }

        /// <summary>
        /// Cria um novo cadastro central.
        /// POST /central-registry com body: { "id": auto, "name": "...", "enabled": true }
        /// </summary>
        private async void btnNovoCadastro_Click(object? sender, EventArgs e)
        {
            var nome = InputBox("Novo Cadastro", "Nome do cadastro (ex: Apt 101):");
            if (string.IsNullOrEmpty(nome)) return;

            var cadastro = new CadastroCentral { Name = nome, Enabled = true };
            var result = await _api.SalvarCadastroAsync(cadastro);

            if (result.Success)
            {
                Log($"Cadastro criado: {nome}");
                await CarregarCadastros();
            }
            else
            {
                var msg = $"Erro ao criar cadastro:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui o cadastro selecionado e todas suas entidades/mídias.
        /// DELETE /central-registry?id=X
        /// </summary>
        private async void btnExcluirCadastro_Click(object? sender, EventArgs e)
        {
            if (_cadastroSelecionado == null) { Aviso("Selecione um cadastro"); return; }

            var confirm = MessageBox.Show(
                $"Excluir '{_cadastroSelecionado.Name}' e todas suas entidades/mídias?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.ExcluirCadastroAsync(_cadastroSelecionado.Id);
            if (result.Success)
            {
                Log($"Cadastro excluído: {_cadastroSelecionado.Name}");
                await CarregarCadastros();
            }
            else
            {
                var msg = $"Erro ao excluir cadastro:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        private async void btnBuscarCadastro_Click(object? sender, EventArgs e)
        {
            _currentOffset = 0; // Nova busca sempre começa do início
            await CarregarCadastros();
        }

        private async void btnAnterior_Click(object? sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            await CarregarCadastros();
        }

        private async void btnProxima_Click(object? sender, EventArgs e)
        {
            _currentOffset += PageSize;
            await CarregarCadastros();
        }

        /// <summary>Enter no campo de busca dispara a pesquisa</summary>
        private async void txtFiltroCadastro_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _currentOffset = 0;
                await CarregarCadastros();
            }
        }

        // =====================================================================
        //  ENTIDADES (Nível 2)
        //  Endpoint: GET /entities?cadastro_id=X
        // =====================================================================

        /// <summary>
        /// Lista as entidades (pessoas/veículos) vinculadas ao cadastro selecionado.
        /// </summary>
        private async Task CarregarEntidades(uint cadastroId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            var result = await _api.ListarEntidadesAsync(cadastroId);

            if (result.Success && result.Data != null)
            {
                lblStatusEntidades.Text = $"{result.Data.Count} entidade(s)";
                foreach (var ent in result.Data.Items)
                {
                    var item = new ListViewItem(ent.EntityId.ToString());
                    item.SubItems.Add(ent.TipoNome);
                    item.SubItems.Add(ent.Name);
                    item.SubItems.Add(ent.Doc);
                    item.SubItems.Add(ent.LprAtivo == 1 ? "Sim" : "");
                    item.Tag = ent;
                    listEntidades.Items.Add(item);
                }
            }
            else
            {
                lblStatusEntidades.Text = $"Erro: {result.Message}";
            }
        }

        /// <summary>Ao selecionar uma entidade, carrega suas mídias (nível 3).</summary>
        private async void listEntidades_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _entidadeSelecionada = listEntidades.SelectedItems[0].Tag as Entidade;
            if (_entidadeSelecionada == null) return;

            lblMidiasTitulo.Text = $"Mídias de: {_entidadeSelecionada.Name}";
            await CarregarMidias(_entidadeSelecionada.EntityId);
        }

        /// <summary>
        /// Cria uma nova entidade vinculada ao cadastro selecionado.
        /// POST /entities com body: { "cadastro_id": X, "tipo": 1, "name": "...", "doc": "..." }
        ///
        /// NOTA: Neste modelo (completo), informamos o cadastro_id do cadastro existente.
        /// O campo createid NÃO é usado aqui (é usado no modelo simples).
        /// </summary>
        private async void btnNovaEntidade_Click(object? sender, EventArgs e)
        {
            if (_cadastroSelecionado == null) { Aviso("Selecione um cadastro primeiro"); return; }

            // Seleciona o tipo em um formulario dedicado (mais claro do que Yes/No).
            using var formTipo = new FormSelecionarTipoEntidade();
            if (formTipo.ShowDialog(this) != DialogResult.OK) return;
            int tipo = formTipo.TipoEntidadeSelecionado;

            CriarEntidadeRequest request;
            string nomeLog;

            if (tipo == (int)TipoEntidade.Veiculo)
            {
                using var formVeiculo = new FormCadastroVeiculo(_cadastroSelecionado.Id);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                request = new CriarEntidadeRequest
                {
                    Id = formVeiculo.IdVeiculo,
                    CadastroId = _cadastroSelecionado.Id,
                    Tipo = (int)TipoEntidade.Veiculo,
                    Name = formVeiculo.NomeEntidadeGerado,
                    Doc = formVeiculo.Placa,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprAtivo = formVeiculo.LprAtivo
                };

                nomeLog = $"{formVeiculo.Placa}";
            }
            else
            {
                uint entityId = SolicitarIdOpcional("ID da Entidade", "Informe o ID da entidade (0 = automático):");
                var nome = InputBox("Nova Entidade", "Nome da pessoa:");
                if (string.IsNullOrEmpty(nome)) return;

                var doc = InputBox("Documento", "CPF (opcional):");

                request = new CriarEntidadeRequest
                {
                    Id = entityId,
                    CadastroId = _cadastroSelecionado.Id,
                    Tipo = tipo,
                    Name = nome,
                    Doc = doc ?? "",
                    LprAtivo = 0
                };
                nomeLog = nome;
            }

            var result = await _api.CriarEntidadeAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entidade criada: {nomeLog} (ID: {result.Data.EntityId})");
                await CarregarEntidades(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Erro ao criar entidade:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui a entidade selecionada e todas as suas mídias.
        /// DELETE /entities?id=X (cascade: remove mídias vinculadas)
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
                if (_cadastroSelecionado != null)
                    await CarregarEntidades(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Erro ao excluir entidade:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        // =====================================================================
        //  MÍDIAS DE ACESSO (Nível 3)
        //  Endpoint: GET /media?entity_id=X
        // =====================================================================

        /// <summary>Lista as mídias de acesso da entidade selecionada.</summary>
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
        /// POST /media com body: { "entity_id": X, "cadastro_id": Y, "tipo": 21, "descricao": "..." }
        ///
        /// Tipos comuns:
        /// - 21 = RFID Wiegand 26 (cartão de proximidade)
        /// - 22 = RFID Wiegand 34
        /// - 17 = LPR (placa de veículo)
        /// - 20 = Facial
        /// </summary>
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            // Seleciona o tipo de mídia
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

            // LPR só deve ser cadastrado para entidades do tipo veículo.
            if (tipoMidia == TipoMidia.Lpr && _entidadeSelecionada.Tipo != (int)TipoEntidade.Veiculo)
            {
                Aviso("A mídia LPR (placa) só pode ser cadastrada para entidades do tipo Veículo.");
                return;
            }

            string? descricao;
            if (tipoMidia == TipoMidia.Lpr)
            {
                // Para veículo, reaproveita a própria placa da entidade sem perguntar novamente.
                descricao = _entidadeSelecionada.Doc;
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    Aviso("Este veículo não possui placa preenchida no campo documento.");
                    return;
                }
            }
            else
            {
                descricao = InputBox("Nova Mídia", "Código/Descrição da mídia:");
            }
            if (string.IsNullOrEmpty(descricao)) return;

            var request = new CriarMidiaRequest
            {
                EntityId = _entidadeSelecionada.EntityId,
                CadastroId = _entidadeSelecionada.CadastroId,
                Tipo = tipoMidia,
                Descricao = descricao
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
            if (tipoMidia == TipoMidia.Lpr)
            {
                request.Ns32_0 = 0;
                request.Ns32_1 = 0;
            }

            var result = await _api.CriarMidiaAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Mídia criada: {descricao} (ID: {result.Data.MediaId})");
                await CarregarMidias(_entidadeSelecionada.EntityId);
            }
            else
            {
                var msg = $"Erro ao criar mídia:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui a mídia selecionada.
        /// DELETE /media?id=X
        /// </summary>
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

        /// <summary>Exibe um InputBox simples (diálogo de texto)</summary>
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

        /// <summary>Exibe um diálogo de seleção de opções</summary>
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

        /// <summary>Pede um ID opcional (0 = automático) e valida entrada numérica.</summary>
        private uint SolicitarIdOpcional(string titulo, string prompt)
        {
            var valor = InputBox(titulo, prompt);
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            if (uint.TryParse(valor.Trim(), out uint id)) return id;
            Aviso("ID inválido. Será usado 0 (automático).");
            return 0;
        }

    }
}
