using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    // =============================================================================
    //  CADASTRO COMPLETO - Modelo MobiCortex (3 NÃ­veis)
    //
    //  Este formulÃ¡rio demonstra o modelo hierÃ¡rquico completo:
    //
    //  1. CADASTRO CENTRAL (central-registry)
    //     Representa um "apartamento", "empresa", "unidade", etc.
    //     Ã‰ o nÃ³ raiz que agrupa entidades.
    //
    //  2. ENTIDADE (entities)
    //     Representa uma pessoa, veÃ­culo ou animal vinculado ao cadastro.
    //     Cada cadastro pode ter vÃ¡rias entidades.
    //
    //  3. MÃDIA DE ACESSO (media)
    //     Representa uma credencial de acesso (cartÃ£o RFID, biometria, placa, etc).
    //     Cada entidade pode ter vÃ¡rias mÃ­dias.
    //
    //  FLUXO:
    //  1. Criar um Cadastro Central (ex: "Apt 101")
    //  2. Adicionar Entidades ao cadastro (ex: "JoÃ£o Silva", "Carro ABC-1234")
    //  3. Adicionar MÃ­dias Ã s entidades (ex: cartÃ£o RFID, placa LPR)
    //
    //  ENDPOINTS USADOS:
    //  - GET/POST/DELETE /central-registry
    //  - GET/POST/PUT/DELETE /entities
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroCompleto : Form
    {
        private IMobiCortexClient _api = null!;

        // Itens selecionados atualmente (para navegaÃ§Ã£o hierÃ¡rquica)
        private CadastroCentral? _cadastroSelecionado;
        private Entidade? _entidadeSelecionada;

        // Estado de paginaÃ§Ã£o dos cadastros
        private int _currentOffset = 0;
        private const int PageSize = 20;
        private uint _totalCadastros = 0;

        /// <summary>
        /// ServiÃ§o da API. Pode ser definido via propriedade para uso no designer.
        /// </summary>
        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

        /// <summary>
        /// Construtor padrÃ£o para o Designer do Visual Studio.
        /// </summary>
        public FormCadastroCompleto()
        {
            InitializeComponent();
        }

        public FormCadastroCompleto(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        // =====================================================================
        //  CADASTROS CENTRAIS (NÃ­vel 1)
        //  Endpoint: GET /central-registry?offset=0&count=20&name=filtro
        // =====================================================================

        private async void FormCadastroCompleto_Load(object? sender, EventArgs e)
        {
            // No modo design do VS, _api pode ser null - nÃ£o carregar dados
            if (_api == null) return;
            await CarregarCadastros();
        }

        /// <summary>
        /// Carrega a lista de cadastros centrais com paginaÃ§Ã£o.
        /// Se o campo de busca contÃ©m um nÃºmero, busca pelo ID.
        /// Se contÃ©m texto, filtra pelo nome.
        /// </summary>
        private async Task CarregarCadastros()
        {
            var filtro = txtFiltroCadastro.Text.Trim();

            // Se digitou um nÃºmero, busca direto pelo ID
            if (uint.TryParse(filtro, out uint idBusca))
            {
                await BuscarCadastroPorId(idBusca);
                return;
            }

            // Busca paginada (com filtro por nome opcional)
            var result = await _api.Cadastros.ListarAsync(
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
                    item.SubItems.Add(c.Enabled ? "Sim" : "NÃ£o");
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
        /// Busca um cadastro especÃ­fico pelo ID e exibe na lista.
        /// GET /central-registry?id=X
        /// </summary>
        private async Task BuscarCadastroPorId(uint id)
        {
            listCadastros.Items.Clear();
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _cadastroSelecionado = null;
            _entidadeSelecionada = null;

            var result = await _api.Cadastros.ObterAsync(id);

            if (result.Success && result.Data != null)
            {
                var c = result.Data;
                var item = new ListViewItem(c.Id.ToString());
                item.SubItems.Add(c.Name);
                item.SubItems.Add(c.Enabled ? "Sim" : "NÃ£o");
                item.SubItems.Add($"{c.PeopleCount}P / {c.VehicleCount}V");
                item.Tag = c;
                listCadastros.Items.Add(item);

                _totalCadastros = 1;
                lblStatusCadastros.Text = $"Busca por ID {id} â€” 1 resultado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                _totalCadastros = 0;
                lblStatusCadastros.Text = $"ID {id} nÃ£o encontrado";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Atualiza label de status e botÃµes de paginaÃ§Ã£o</summary>
        private void AtualizarPaginacao()
        {
            int totalPages = (int)((_totalCadastros + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalCadastros;
            lblStatusCadastros.Text = $"{_totalCadastros} cadastro(s)";
        }

        /// <summary>Ao selecionar um cadastro, carrega suas entidades (nÃ­vel 2).</summary>
        private async void listCadastros_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listCadastros.SelectedItems.Count == 0) return;

            _cadastroSelecionado = listCadastros.SelectedItems[0].Tag as CadastroCentral;
            if (_cadastroSelecionado == null) return;

            lblEntidadesTitulo.Text = $"Entidades de: {_cadastroSelecionado.Name}";
            await CarregarEntidades(_cadastroSelecionado.Id);
        }

        /// <summary>
        /// Cria um novo cadastro central usando o formulÃ¡rio completo.
        /// POST /central-registry com body: { "id": auto, "name": "...", "enabled": true }
        /// </summary>
        private async void btnNovoCadastro_Click(object? sender, EventArgs e)
        {
            using var form = new FormCadastroCentral();
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var cadastro = new CadastroCentral
            {
                Id = form.IdCadastro,
                Name = form.Nome,
                Enabled = form.CadastroEnabled,
                Field1 = form.Field1,
                Field2 = form.Field2,
                Field3 = form.Field3,
                Field4 = form.Field4
            };

            var result = await _api.Cadastros.CriarAsync(cadastro);

            if (result.Success)
            {
                Log($"Cadastro criado: {form.Nome}");
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
        /// Edita o cadastro selecionado ao dar duplo clique.
        /// PUT /central-registry?id=X
        /// </summary>
        private async void listCadastros_DoubleClick(object? sender, EventArgs e)
        {
            // ObtÃ©m o item clicado diretamente (funciona mesmo se nÃ£o estiver selecionado)
            if (listCadastros.SelectedItems.Count == 0) return;
            
            var cadastro = listCadastros.SelectedItems[0].Tag as CadastroCentral;
            if (cadastro == null) return;
            
            // Atualiza o cadastro selecionado
            _cadastroSelecionado = cadastro;

            using var form = new FormCadastroCentral(cadastro);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var cadastroAtualizado = new CadastroCentral
            {
                Id = form.IdCadastro,
                Name = form.Nome,
                Enabled = form.CadastroEnabled,
                Field1 = form.Field1,
                Field2 = form.Field2,
                Field3 = form.Field3,
                Field4 = form.Field4
            };

            // Log para debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(cadastroAtualizado);
            Log($"DEBUG: Atualizando cadastro ID={cadastroAtualizado.Id}, JSON={jsonDebug}");

            var result = await _api.Cadastros.AtualizarAsync(cadastroAtualizado);

            if (result.Success)
            {
                Log($"Cadastro atualizado: {form.Nome}");
                await CarregarCadastros();
            }
            else
            {
                var msg = $"Erro ao atualizar cadastro:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui o cadastro selecionado e todas suas entidades/mÃ­dias.
        /// DELETE /central-registry?id=X
        /// </summary>
        private async void btnExcluirCadastro_Click(object? sender, EventArgs e)
        {
            if (_cadastroSelecionado == null) { Aviso("Selecione um cadastro"); return; }

            var confirm = MessageBox.Show(
                $"Excluir '{_cadastroSelecionado.Name}' e todas suas entidades/mÃ­dias?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Cadastros.ExcluirAsync(_cadastroSelecionado.Id);
            if (result.Success)
            {
                Log($"Cadastro excluÃ­do: {_cadastroSelecionado.Name}");
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
            _currentOffset = 0; // Nova busca sempre comeÃ§a do inÃ­cio
            await CarregarCadastros();
        }

        private async void btnRefreshCadastros_Click(object? sender, EventArgs e)
        {
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
        //  ENTIDADES (NÃ­vel 2)
        //  Endpoint: GET /entities?central_registry_id=X
        // =====================================================================

        /// <summary>
        /// Lista as entidades (pessoas/veÃ­culos) vinculadas ao cadastro selecionado.
        /// </summary>
        private async Task CarregarEntidades(uint cadastroId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            var result = await _api.Entidades.ListarPorCadastroAsync(cadastroId);

            if (result.Success && result.Data != null)
            {
                lblStatusEntidades.Text = $"{result.Data.Count} entidade(s)";
                foreach (var ent in result.Data.Items)
                {
                    var item = new ListViewItem(ent.EntityId.ToString());
                    item.SubItems.Add(ent.TipoNome);
                    item.SubItems.Add(ent.NomeExibicao);
                    item.SubItems.Add(ent.Doc);
                    item.SubItems.Add(ent.Enabled ? "S" : "N");
                    item.SubItems.Add(ent.LprAtivo ? "Sim" : "");
                    item.Tag = ent;
                    listEntidades.Items.Add(item);
                }
            }
            else
            {
                lblStatusEntidades.Text = $"Erro: {result.Message}";
            }
        }

        /// <summary>Ao selecionar uma entidade, carrega suas mÃ­dias (nÃ­vel 3).</summary>
        private async void listEntidades_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _entidadeSelecionada = listEntidades.SelectedItems[0].Tag as Entidade;
            if (_entidadeSelecionada == null) return;

            lblMidiasTitulo.Text = $"MÃ­dias de: {_entidadeSelecionada.NomeExibicao}";
            await CarregarMidias(_entidadeSelecionada.EntityId);
        }

        /// <summary>
        /// Edita a entidade selecionada ao dar duplo clique.
        /// </summary>
        private async void listEntidades_DoubleClick(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            var entidade = listEntidades.SelectedItems[0].Tag as Entidade;
            if (entidade == null) return;

            _entidadeSelecionada = entidade;

            // Verifica o tipo de entidade
            if (entidade.Tipo == (int)TipoEntidade.Veiculo)
            {
                // Abre formulÃ¡rio de ediÃ§Ã£o de veÃ­culo
                using var formVeiculo = new FormCadastroVeiculo(entidade, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                // Cria o request de atualizaÃ§Ã£o (PUT /entities?id=X)
                var entidadeAtualizada = new AtualizarEntidadeRequest
                {
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprAtivo = formVeiculo.LprAtivo
                };

                // DEBUG: Log do JSON sendo enviado
                var jsonDebug = System.Text.Json.JsonSerializer.Serialize(entidadeAtualizada);
                Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebug}");

                var result = await _api.Entidades.AtualizarAsync(entidade.EntityId, entidadeAtualizada);
                if (result.Success)
                {
                    Log($"VeÃ­culo atualizado: {entidade.NomeExibicao}");
                    if (_cadastroSelecionado != null)
                        await CarregarEntidades(_cadastroSelecionado.Id);
                }
                else
                {
                    var msg = $"Erro ao atualizar veÃ­culo:\n{result.Message}\nResposta: {result.RawResponse}";
                    Log(msg);
                    Aviso(msg);
                }
                return;
            }

            // Abre formulÃ¡rio de ediÃ§Ã£o de pessoa
            using var form = new FormCadastroPessoaEdit(entidade);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            // Cria o request de atualizaÃ§Ã£o (PUT /entities?id=X)
            // Usa AtualizarEntidadeRequest - sÃ³ envia os campos que quer alterar
            var docLimpo = string.IsNullOrWhiteSpace(form.Documento) ? null : form.Documento.Trim();
            var entidadeAtualizadaPessoa = new AtualizarEntidadeRequest
            {
                Name = form.Nome,
                Doc = docLimpo, // null se vazio (nÃ£o serÃ¡ enviado no JSON)
                Enabled = form.EntidadeEnabled,
                // Pessoas (tipo 1) nÃ£o usam LPR - nÃ£o envia o campo (null)
                LprAtivo = entidade.Tipo == 1 ? null : entidade.LprAtivo
            };

            // DEBUG: Log do JSON sendo enviado
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Form.EntidadeEnabled = {form.EntidadeEnabled}");
            var jsonDebugPessoa = System.Text.Json.JsonSerializer.Serialize(entidadeAtualizadaPessoa);
            Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebugPessoa}");

            var resultPessoa = await _api.Entidades.AtualizarAsync(entidade.EntityId, entidadeAtualizadaPessoa);
            if (resultPessoa.Success)
            {
                Log($"Entidade atualizada: {form.Nome}");
                if (_cadastroSelecionado != null)
                    await CarregarEntidades(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Erro ao atualizar entidade:\n{resultPessoa.Message}\nResposta: {resultPessoa.RawResponse}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Cria uma nova entidade vinculada ao cadastro selecionado.
        /// POST /entities com body: { "central_registry_id": X, "type": 1, "name": "...", "doc": "..." }
        ///
        /// NOTA: Neste modelo (completo), informamos o central_registry_id do cadastro existente.
        /// Se o ID da entidade for 0, o cliente envia createid=true para a controladora gerar o entity_id.
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
                using var formVeiculo = new FormCadastroVeiculo(_cadastroSelecionado.Id, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                request = new CriarEntidadeRequest
                {
                    Id = formVeiculo.IdVeiculo,
                    CreateId = formVeiculo.IdVeiculo == 0 ? true : null,
                    CadastroId = _cadastroSelecionado.Id,
                    Tipo = (int)TipoEntidade.Veiculo,
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprAtivo = formVeiculo.LprAtivo
                };

                nomeLog = $"{formVeiculo.Placa}";
            }
            else
            {
                using var formPessoa = new FormCadastroPessoa(_cadastroSelecionado.Id);
                if (formPessoa.ShowDialog(this) != DialogResult.OK) return;

                request = new CriarEntidadeRequest
                {
                    Id = formPessoa.Id,
                    CadastroId = _cadastroSelecionado.Id,
                    Tipo = tipo,
                    Name = formPessoa.Nome,
                    Doc = formPessoa.Documento,
                    LprAtivo = formPessoa.LprAtivo,
                    Enabled = formPessoa.EntidadeEnabled
                };
                nomeLog = formPessoa.Nome;
            }

            // Log do JSON para debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(request);
            Log($"DEBUG JSON Entidade: {jsonDebug}");

            var result = await _api.Entidades.CriarAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entidade criada: {nomeLog} (ID: {result.Data.EntityId})");
                await CarregarEntidades(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Erro ao criar entidade:\n{result.Message}\nJSON: {jsonDebug}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui a entidade selecionada e todas as suas mÃ­dias.
        /// DELETE /entities?id=X (cascade: remove mÃ­dias vinculadas)
        /// </summary>
        private async void btnExcluirEntidade_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            var confirm = MessageBox.Show(
                $"Excluir '{_entidadeSelecionada.NomeExibicao}' e todas suas mÃ­dias?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Entidades.ExcluirAsync(_entidadeSelecionada.EntityId);
            if (result.Success)
            {
                Log($"Entidade excluÃ­da: {_entidadeSelecionada.NomeExibicao}");
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

        private async void btnRefreshEntidades_Click(object? sender, EventArgs e)
        {
            if (_cadastroSelecionado != null)
                await CarregarEntidades(_cadastroSelecionado.Id);
        }

        // =====================================================================
        //  MÃDIAS DE ACESSO (NÃ­vel 3)
        //  Endpoint: GET /media?entity_id=X
        // =====================================================================

        /// <summary>Lista as mÃ­dias de acesso da entidade selecionada.</summary>
        private async Task CarregarMidias(uint entityId)
        {
            listMidias.Items.Clear();

            var result = await _api.Midias.ListarPorEntidadeAsync(entityId);

            if (result.Success && result.Data != null)
            {
                lblStatusMidias.Text = $"{result.Data.Count} mÃ­dia(s)";
                foreach (var m in result.Data.Items)
                {
                    var item = new ListViewItem(m.MediaId.ToString());
                    item.SubItems.Add(m.TipoNome);
                    item.SubItems.Add(m.Descricao);
                    item.SubItems.Add(m.Enabled ? "Sim" : "NÃ£o");
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
        /// Cria uma nova mÃ­dia vinculada Ã  entidade selecionada.
        /// POST /media com body: { "entity_id": X, "central_registry_id": Y, "type": 21, "description": "..." }
        ///
        /// Tipos comuns:
        /// - 21 = RFID Wiegand 26 (cartÃ£o de proximidade)
        /// - 22 = RFID Wiegand 34
        /// - 17 = LPR (placa de veÃ­culo)
        /// - 20 = Facial
        /// </summary>
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { Aviso("Selecione uma entidade"); return; }

            // Seleciona o tipo de mÃ­dia
            var tipos = new[] { "RFID Wiegand 26", "RFID Wiegand 34", "Placa (LPR)", "Facial" };
            var tipoIdx = SelecionarOpcao("Tipo de MÃ­dia", "Selecione o tipo:", tipos);
            if (tipoIdx < 0) return;

            int tipoMidia = tipoIdx switch
            {
                0 => TipoMidia.Wiegand26,
                1 => TipoMidia.Wiegand34,
                2 => TipoMidia.Lpr,
                3 => TipoMidia.Facial,
                _ => TipoMidia.Wiegand26
            };

            // LPR sÃ³ deve ser cadastrado para entidades do tipo veÃ­culo.
            if (tipoMidia == TipoMidia.Lpr && _entidadeSelecionada.Tipo != (int)TipoEntidade.Veiculo)
            {
                Aviso("A mÃ­dia LPR (placa) sÃ³ pode ser cadastrada para entidades do tipo VeÃ­culo.");
                return;
            }

            string? descricao;
            if (tipoMidia == TipoMidia.Lpr)
            {
                // Para veÃ­culo, reaproveita a prÃ³pria placa da entidade sem perguntar novamente.
                descricao = _entidadeSelecionada.Doc;
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    Aviso("Este veÃ­culo nÃ£o possui placa preenchida no campo documento.");
                    return;
                }
            }
            else
            {
                descricao = InputBox("Nova MÃ­dia", "CÃ³digo/DescriÃ§Ã£o da mÃ­dia:");
            }
            if (string.IsNullOrEmpty(descricao)) return;

            var request = new CriarMidiaRequest
            {
                EntityId = _entidadeSelecionada.EntityId,
                CadastroId = _entidadeSelecionada.CadastroId,
                Tipo = tipoMidia,
                Descricao = descricao
            };

            // EXPLICAÃ‡ÃƒO: O backend valida o formato da mÃ­dia baseado no conteÃºdo
            // do campo "descricao". Para RFID, ele aceita formatos Wiegand/CODE/HEX.
            // Para LPR (placa), se enviarmos apenas a descricao, o backend tenta
            // validar como RFID e retorna erro "formato RFID invalido".
            // 
            // SOLUÃ‡ÃƒO: Enviar ns32_0 e ns32_1 indica ao backend que os dados binÃ¡rios
            // jÃ¡ foram processados, entÃ£o ele nÃ£o aplica a validaÃ§Ã£o RFID.
            // Para LPR manual, enviamos 0 em ambos (o backend ignora para LPR).
            // 
            // NOTA: A forma RECOMENDADA de criar LPR Ã© usando lpr_ativo=true no cadastro
            // da entidade (veÃ­culo), nÃ£o via POST /media manual.
            if (tipoMidia == TipoMidia.Lpr)
            {
                request.Ns32_0 = 0;
                request.Ns32_1 = 0;
            }

            // Log do JSON para debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(request);
            Log($"DEBUG JSON Midia: {jsonDebug}");

            var result = await _api.Midias.CriarAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"MÃ­dia criada: {descricao} (ID: {result.Data.MediaId})");
                await CarregarMidias(_entidadeSelecionada.EntityId);
            }
            else
            {
                var msg = $"Erro ao criar mÃ­dia:\n{result.Message}\nJSON: {jsonDebug}";
                Log(msg);
                Aviso(msg);
            }
        }

        /// <summary>
        /// Exclui a mÃ­dia selecionada.
        /// DELETE /media?id=X
        /// </summary>
        private async void btnExcluirMidia_Click(object? sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) { Aviso("Selecione uma mÃ­dia"); return; }

            var midia = listMidias.SelectedItems[0].Tag as MidiaAcesso;
            if (midia == null) return;

            // ConfirmaÃ§Ã£o antes de excluir
            var confirm = MessageBox.Show(
                $"Tem certeza que deseja excluir a mÃ­dia '{midia.Descricao}'?\n\nEsta aÃ§Ã£o nÃ£o pode ser desfeita.",
                "Confirmar ExclusÃ£o",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Midias.ExcluirAsync(midia.MediaId);
            if (result.Success)
            {
                Log($"MÃ­dia excluÃ­da: {midia.Descricao}");
                if (_entidadeSelecionada != null)
                    await CarregarMidias(_entidadeSelecionada.EntityId);
            }
            else
            {
                var msg = $"Erro ao excluir mÃ­dia:\n{result.Message}";
                Log(msg);
                Aviso(msg);
            }
        }

        private async void btnRefreshMidias_Click(object? sender, EventArgs e)
        {
            if (_entidadeSelecionada != null)
                await CarregarMidias(_entidadeSelecionada.EntityId);
        }

        // =====================================================================
        //  DETALHES DA MIDIA (Duplo clique)
        // =====================================================================

        /// <summary>
        /// Abre o formulÃ¡rio de detalhes da mÃ­dia ao dar duplo clique.
        /// </summary>
        private async void listMidias_DoubleClick(object? sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) return;

            var midia = listMidias.SelectedItems[0].Tag as MidiaAcesso;
            if (midia == null) return;

            using var form = new FormDetalheMidia(midia);
            
            if (form.ShowDialog(this) == DialogResult.OK && form.FoiModificada)
            {
                bool sucesso = true;
                string mensagem = "";

                // Atualiza o estado de habilitaÃ§Ã£o se alterado
                if (midia.Enabled != form.NovoEstadoEnabled)
                {
                    var result = await _api.Midias.AlterarStatusAsync(midia.MediaId, form.NovoEstadoEnabled);
                    if (!result.Success)
                    {
                        sucesso = false;
                        mensagem = result.Message ?? "Erro ao alterar status";
                    }
                    else
                    {
                        var status = form.NovoEstadoEnabled ? "liberada" : "bloqueada";
                        Log($"MÃ­dia {midia.Descricao} {status} com sucesso!");
                    }
                }

                // Atualiza a data de permissao se alterada
                if (sucesso && form.DataPermissaoAlterada)
                {
                    var result = await _api.Midias.AlterarExpiracaoAsync(midia.MediaId, form.NovaDataPermissao);
                    if (!result.Success)
                    {
                        sucesso = false;
                        mensagem = result.Message ?? "Erro ao alterar data de permissao";
                    }
                    else
                    {
                        if (form.NovaDataPermissao > 0)
                            Log($"MÃ­dia {midia.Descricao} permitida atÃ© {DateTimeOffset.FromUnixTimeSeconds(form.NovaDataPermissao).LocalDateTime:dd/MM/yyyy HH:mm}");
                        else
                            Log($"Data limite removida da mÃ­dia {midia.Descricao}");
                    }
                }

                // Se houve erro, mostra mensagem
                if (!sucesso)
                {
                    var msg = $"Erro ao atualizar mÃ­dia:\n{mensagem}";
                    Log(msg);
                    Aviso(msg);
                }

                // Recarrega a lista em qualquer caso
                if (_entidadeSelecionada != null)
                    await CarregarMidias(_entidadeSelecionada.EntityId);
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

        /// <summary>Exibe um InputBox simples (diÃ¡logo de texto)</summary>
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

        /// <summary>Exibe um diÃ¡logo de seleÃ§Ã£o de opÃ§Ãµes</summary>
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

        /// <summary>Pede um ID opcional (0 = automÃ¡tico) e valida entrada numÃ©rica.</summary>
        private uint SolicitarIdOpcional(string titulo, string prompt)
        {
            var valor = InputBox(titulo, prompt);
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            if (uint.TryParse(valor.Trim(), out uint id)) return id;
            Aviso("ID invÃ¡lido. SerÃ¡ usado 0 (automÃ¡tico).");
            return 0;
        }

    }
}





