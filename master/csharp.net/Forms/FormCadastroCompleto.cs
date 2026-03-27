using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    // =============================================================================
    //  COMPLETE REGISTRATION - MobiCortex Model (3 Levels)
    //
    //  This form demonstrates the complete hierarchical model:
    //
    //  1. CENTRAL REGISTRY (central-registry)
    //     Represents an "apartment", "company", "unit", etc.
    //     It is the root node that groups entities.
    //
    //  2. ENTITY (entities)
    //     Represents a person, vehicle, or animal linked to the registry.
    //     Each registry can have multiple entities.
    //
    //  3. ACCESS MEDIA (media)
    //     Represents an access credential (RFID card, biometry, plate, etc).
    //     Each entity can have multiple media.
    //
    //  FLOW:
    //  1. Create a Central Registry (e.g.: "Apt 101")
    //  2. Add Entities to the registry (e.g.: "John Smith", "Car ABC-1234")
    //  3. Add Media to the entities (e.g.: RFID card, LPR plate)
    //
    //  ENDPOINTS USED:
    //  - GET/POST/DELETE /central-registry
    //  - GET/POST/PUT/DELETE /entities
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroCompleto : Form
    {
        private IMobiCortexClient _api = new MobiCortexClient();

        // Currently selected items (for hierarchical navigation)
        private CentralRegistry? _cadastroSelecionado;
        private Entity? _entidadeSelecionada;

        // Registry pagination state
        private int _currentOffset = 0;
        private const int PageSize = 20;
        private uint _totalCadastros = 0;

        /// <summary>
        /// API service. Can be set via property for designer usage.
        /// </summary>
        public IMobiCortexClient ApiService
        {
            get { return _api; }
            set { _api = value; }
        }

        /// <summary>
        /// Default constructor for the Visual Studio Designer.
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
        //  CENTRAL REGISTRIES (Level 1)
        //  Endpoint: GET /central-registry?offset=0&count=20&name=filter
        // =====================================================================

        private async void FormCadastroCompleto_Load(object sender, EventArgs e)
        {
            // In VS design mode, _api may be null - don't load data
            if (_api == null) return;
            await LoadRegistries();
        }

        /// <summary>
        /// Loads the list of central registries with pagination.
        /// If the search field contains a number, searches by ID.
        /// If it contains text, filters by name.
        /// </summary>
        private async Task LoadRegistries()
        {
            var filter = txtFiltroCadastro.Text.Trim();

            // If a number was entered, search directly by ID
            if (uint.TryParse(filter, out uint searchId))
            {
                await SearchRegistryById(searchId);
                return;
            }

            // Paginated search (with optional name filter)
            var result = await _api.Registries.ListAsync(
                _currentOffset, PageSize,
                string.IsNullOrEmpty(filter) ? null : filter);

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
                    item.SubItems.Add(c.Enabled ? "Yes" : "No");
                    item.SubItems.Add($"{c.PeopleCount}P / {c.VehicleCount}V");
                    item.Tag = c;
                    listCadastros.Items.Add(item);
                }
                UpdatePagination();
            }
            else
            {
                _totalCadastros = 0;
                UpdatePagination();
                lblStatusCadastros.Text = $"Error: {result.Message}";
            }
        }

        /// <summary>
        /// Searches for a specific registry by ID and displays it in the list.
        /// GET /central-registry?id=X
        /// </summary>
        private async Task SearchRegistryById(uint id)
        {
            listCadastros.Items.Clear();
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _cadastroSelecionado = null;
            _entidadeSelecionada = null;

            var result = await _api.Registries.GetAsync(id);

            if (result.Success && result.Data != null)
            {
                var c = result.Data;
                var item = new ListViewItem(c.Id.ToString());
                item.SubItems.Add(c.Name);
                item.SubItems.Add(c.Enabled ? "Yes" : "No");
                item.SubItems.Add($"{c.PeopleCount}P / {c.VehicleCount}V");
                item.Tag = c;
                listCadastros.Items.Add(item);

                _totalCadastros = 1;
                lblStatusCadastros.Text = $"Search by ID {id} - 1 result";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                _totalCadastros = 0;
                lblStatusCadastros.Text = $"ID {id} not found";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Updates the status label and pagination buttons</summary>
        private void UpdatePagination()
        {
            int totalPages = (int)((_totalCadastros + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalCadastros;
            lblStatusCadastros.Text = $"{_totalCadastros} registry(ies)";
        }

        /// <summary>When selecting a registry, loads its entities (level 2).</summary>
        private async void listCadastros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listCadastros.SelectedItems.Count == 0) return;

            _cadastroSelecionado = listCadastros.SelectedItems[0].Tag as CentralRegistry;
            if (_cadastroSelecionado == null) return;

            lblEntidadesTitulo.Text = $"Entities of: {_cadastroSelecionado.Name}";
            await LoadEntities(_cadastroSelecionado.Id);
        }

        /// <summary>
        /// Creates a new central registry using the complete form.
        /// POST /central-registry with body: { "id": auto, "name": "...", "enabled": true }
        /// </summary>
        private async void btnNovoCadastro_Click(object sender, EventArgs e)
        {
            using var form = new FormCadastroCentral();
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var cadastro = new CentralRegistry
            {
                Id = form.IdCadastro,
                Name = form.Nome,
                Enabled = form.CadastroEnabled,
                Field1 = form.Field1,
                Field2 = form.Field2,
                Field3 = form.Field3,
                Field4 = form.Field4
            };

            var result = await _api.Registries.CreateAsync(cadastro);

            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Registry created: {form.Nome}");
                await LoadRegistries();
            }
            else if (result.IsConflict)
            {
                Log($"Registry already exists: {form.Nome} (HTTP 409)");
                ShowWarning($"The registry '{form.Nome}' already exists on the controller.");
            }
            else
            {
                var msg = $"Error creating registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        /// <summary>
        /// Edits the selected registry on double-click.
        /// PUT /central-registry?id=X
        /// </summary>
        private async void listCadastros_DoubleClick(object sender, EventArgs e)
        {
            // Gets the clicked item directly (works even if not selected)
            if (listCadastros.SelectedItems.Count == 0) return;

            var cadastro = listCadastros.SelectedItems[0].Tag as CentralRegistry;
            if (cadastro == null) return;

            // Updates the selected registry
            _cadastroSelecionado = cadastro;

            using var form = new FormCadastroCentral(cadastro);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var cadastroAtualizado = new CentralRegistry
            {
                Id = form.IdCadastro,
                Name = form.Nome,
                Enabled = form.CadastroEnabled,
                Field1 = form.Field1,
                Field2 = form.Field2,
                Field3 = form.Field3,
                Field4 = form.Field4
            };

            // Debug log
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(cadastroAtualizado);
            Log($"DEBUG: Updating registry ID={cadastroAtualizado.Id}, JSON={jsonDebug}");

            var result = await _api.Registries.UpdateAsync(cadastroAtualizado);

            if (result.Success)
            {
                Log($"Registry updated: {form.Nome}");
                await LoadRegistries();
            }
            else
            {
                var msg = $"Error updating registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected registry and all its entities/media.
        /// DELETE /central-registry?id=X
        /// </summary>
        private async void btnExcluirCadastro_Click(object sender, EventArgs e)
        {
            if (_cadastroSelecionado == null) { ShowWarning("Select a registry"); return; }

            var confirm = MessageBox.Show(
                $"Delete '{_cadastroSelecionado.Name}' and all its entities/media?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Registries.DeleteAsync(_cadastroSelecionado.Id);
            if (result.Success)
            {
                Log($"Registry deleted: {_cadastroSelecionado.Name}");
                await LoadRegistries();
            }
            else
            {
                var msg = $"Error deleting registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        private async void btnBuscarCadastro_Click(object sender, EventArgs e)
        {
            _currentOffset = 0; // New search always starts from the beginning
            await LoadRegistries();
        }

        private async void btnRefreshCadastros_Click(object sender, EventArgs e)
        {
            await LoadRegistries();
        }

        private async void btnAnterior_Click(object sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            await LoadRegistries();
        }

        private async void btnProxima_Click(object sender, EventArgs e)
        {
            _currentOffset += PageSize;
            await LoadRegistries();
        }

        /// <summary>Enter key in the search field triggers the search</summary>
        private async void txtFiltroCadastro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _currentOffset = 0;
                await LoadRegistries();
            }
        }

        // =====================================================================
        //  ENTITIES (Level 2)
        //  Endpoint: GET /entities?central_registry_id=X
        // =====================================================================

        /// <summary>
        /// Lists the entities (people/vehicles) linked to the selected registry.
        /// </summary>
        private async Task LoadEntities(uint cadastroId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _entidadeSelecionada = null;

            var result = await _api.Entities.ListByRegistryAsync(cadastroId);

            if (result.Success && result.Data != null)
            {
                lblStatusEntidades.Text = $"{result.Data.Count} entity(ies)";
                foreach (var ent in result.Data.Items)
                {
                    var item = new ListViewItem(ent.EntityId.ToString());
                    item.SubItems.Add(ent.TypeName);
                    item.SubItems.Add(ent.DisplayName);
                    item.SubItems.Add(ent.Doc);
                    item.SubItems.Add(ent.Enabled ? "Y" : "N");
                    item.SubItems.Add(ent.LprActive ? "Yes" : "");
                    item.Tag = ent;
                    listEntidades.Items.Add(item);
                }
            }
            else
            {
                lblStatusEntidades.Text = $"Error: {result.Message}";
            }
        }

        /// <summary>When selecting an entity, loads its media (level 3).</summary>
        private async void listEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _entidadeSelecionada = listEntidades.SelectedItems[0].Tag as Entity;
            if (_entidadeSelecionada == null) return;

            lblMidiasTitulo.Text = $"Media of: {_entidadeSelecionada.DisplayName}";
            await LoadMedia(_entidadeSelecionada.EntityId);
        }

        /// <summary>
        /// Edits the selected entity on double-click.
        /// </summary>
        private async void listEntidades_DoubleClick(object sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            var entidade = listEntidades.SelectedItems[0].Tag as Entity;
            if (entidade == null) return;

            _entidadeSelecionada = entidade;

            // Check the entity type
            if (entidade.TypeAlias == (int)EntityType.Vehicle)
            {
                // Open vehicle edit form
                using var formVeiculo = new FormCadastroVeiculo(entidade, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                // Create the update request (PUT /entities?id=X)
                var entidadeAtualizada = new UpdateEntityRequest
                {
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprActive = formVeiculo.LprAtivo
                };

                // DEBUG: Log the JSON being sent
                var jsonDebug = System.Text.Json.JsonSerializer.Serialize(entidadeAtualizada);
                Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebug}");

                var result = await _api.Entities.UpdateAsync(entidade.EntityId, entidadeAtualizada);
                if (result.Success)
                {
                    Log($"Vehicle updated: {entidade.DisplayName}");
                    if (_cadastroSelecionado != null)
                        await LoadEntities(_cadastroSelecionado.Id);
                }
                else
                {
                    var msg = $"Error updating vehicle:\n{result.Message}\nResponse: {result.RawResponse}";
                    Log(msg);
                    ShowWarning(msg);
                }
                return;
            }

            // Open person edit form
            using var form = new FormCadastroPessoaEdit(entidade);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            // Create the update request (PUT /entities?id=X)
            // Uses UpdateEntityRequest - only sends the fields to change
            var docLimpo = string.IsNullOrWhiteSpace(form.Documento) ? null : form.Documento.Trim();
            var entidadeAtualizadaPessoa = new UpdateEntityRequest
            {
                Name = form.Nome,
                Doc = docLimpo, // null if empty (won't be sent in JSON)
                Enabled = form.EntidadeEnabled,
                // People (type 1) don't use LPR - don't send the field (null)
                LprActive = entidade.TypeAlias == 1 ? null : entidade.LprActive
            };

            // DEBUG: Log the JSON being sent
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Form.EntidadeEnabled = {form.EntidadeEnabled}");
            var jsonDebugPessoa = System.Text.Json.JsonSerializer.Serialize(entidadeAtualizadaPessoa);
            Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebugPessoa}");

            var resultPessoa = await _api.Entities.UpdateAsync(entidade.EntityId, entidadeAtualizadaPessoa);
            if (resultPessoa.Success)
            {
                Log($"Entity updated: {form.Nome}");
                if (_cadastroSelecionado != null)
                    await LoadEntities(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Error updating entity:\n{resultPessoa.Message}\nResponse: {resultPessoa.RawResponse}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        /// <summary>
        /// Creates a new entity linked to the selected registry.
        /// POST /entities with body: { "central_registry_id": X, "type": 1, "name": "...", "doc": "..." }
        ///
        /// NOTE: In this model (complete), we provide the central_registry_id of the existing registry.
        /// If the entity ID is 0, the client sends createid=true so the controller generates the entity_id.
        /// </summary>
        private async void btnNovaEntidade_Click(object sender, EventArgs e)
        {
            if (_cadastroSelecionado == null) { ShowWarning("Select a registry first"); return; }

            // Select the type in a dedicated form (clearer than Yes/No).
            using var formTipo = new FormSelecionarTipoEntidade();
            if (formTipo.ShowDialog(this) != DialogResult.OK) return;
            int tipo = formTipo.TipoEntidadeSelecionado;

            CreateEntityRequest request;
            string nomeLog;

            if (tipo == (int)EntityType.Vehicle)
            {
                using var formVeiculo = new FormCadastroVeiculo(_cadastroSelecionado.Id, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                request = new CreateEntityRequest
                {
                    Id = formVeiculo.IdVeiculo,
                    CreateId = formVeiculo.IdVeiculo == 0 ? true : null,
                    RegistryId = _cadastroSelecionado.Id,
                    TypeAlias = (int)EntityType.Vehicle,
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprActive = formVeiculo.LprAtivo
                };

                nomeLog = $"{formVeiculo.Placa}";
            }
            else
            {
                using var formPessoa = new FormCadastroPessoa(_cadastroSelecionado.Id);
                if (formPessoa.ShowDialog(this) != DialogResult.OK) return;

                request = new CreateEntityRequest
                {
                    Id = formPessoa.Id,
                    RegistryId = _cadastroSelecionado.Id,
                    TypeAlias = tipo,
                    Name = formPessoa.Nome,
                    Doc = formPessoa.Documento,
                    LprActive = formPessoa.LprAtivo,
                    Enabled = formPessoa.EntidadeEnabled
                };
                nomeLog = formPessoa.Nome;
            }

            // Log JSON for debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(request);
            Log($"DEBUG JSON Entity: {jsonDebug}");

            var result = await _api.Entities.CreateAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entity created: {nomeLog} (ID: {result.Data.EntityId})");
                await LoadEntities(_cadastroSelecionado.Id);
            }
            else if (result.IsConflict)
            {
                // HTTP 409 = entity already exists on the controller.
                Log($"Entity already exists: {nomeLog} (HTTP 409 Conflict)");

                var resposta = MessageBox.Show(
                    $"The entity '{nomeLog}' already exists on the controller.\n\nDo you want to overwrite the existing data?",
                    "Entity already registered",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resposta == DialogResult.Yes)
                {
                    request.Overwrite = true;
                    var retryResult = await _api.Entities.CreateAsync(request);
                    if (retryResult.Success && retryResult.Data?.Ret == 0)
                    {
                        Log($"Entity overwritten: {nomeLog} (entity_id={retryResult.Data.EntityId})");
                        await LoadEntities(_cadastroSelecionado.Id);
                    }
                    else
                    {
                        var msg = $"Error overwriting entity (HTTP {retryResult.StatusCode}):\n{retryResult.Message}";
                        Log(msg);
                        ShowWarning(msg);
                    }
                }
            }
            else
            {
                var msg = $"Error creating entity (HTTP {result.StatusCode}):\n{result.Message}\nJSON: {jsonDebug}";
                if (!string.IsNullOrEmpty(result.RawResponse))
                    msg += $"\nResponse: {result.RawResponse}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected entity and all its media.
        /// DELETE /entities?id=X (cascade: removes linked media)
        /// </summary>
        private async void btnExcluirEntidade_Click(object sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { ShowWarning("Select an entity"); return; }

            var confirm = MessageBox.Show(
                $"Delete '{_entidadeSelecionada.DisplayName}' and all its media?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Entities.DeleteAsync(_entidadeSelecionada.EntityId);
            if (result.Success)
            {
                Log($"Entity deleted: {_entidadeSelecionada.DisplayName}");
                if (_cadastroSelecionado != null)
                    await LoadEntities(_cadastroSelecionado.Id);
            }
            else
            {
                var msg = $"Error deleting entity (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        private async void btnRefreshEntidades_Click(object sender, EventArgs e)
        {
            if (_cadastroSelecionado != null)
                await LoadEntities(_cadastroSelecionado.Id);
        }

        // =====================================================================
        //  ACCESS MEDIA (Level 3)
        //  Endpoint: GET /media?entity_id=X
        // =====================================================================

        /// <summary>Lists the access media of the selected entity.</summary>
        private async Task LoadMedia(uint entityId)
        {
            listMidias.Items.Clear();

            var result = await _api.Media.ListByEntityAsync(entityId);

            if (result.Success && result.Data != null)
            {
                lblStatusMidias.Text = $"{result.Data.Count} media item(s)";
                foreach (var m in result.Data.Items)
                {
                    var item = new ListViewItem(m.MediaId.ToString());
                    item.SubItems.Add(m.TypeName);
                    item.SubItems.Add(m.DescriptionAlias);
                    item.SubItems.Add(m.Enabled ? "Yes" : "No");
                    item.Tag = m;
                    listMidias.Items.Add(item);
                }
            }
            else
            {
                lblStatusMidias.Text = $"Error: {result.Message}";
            }
        }

        /// <summary>
        /// Creates a new media linked to the selected entity.
        /// POST /media with body: { "entity_id": X, "central_registry_id": Y, "type": 21, "description": "..." }
        ///
        /// Common types:
        /// - 21 = RFID Wiegand 26 (proximity card)
        /// - 22 = RFID Wiegand 34
        /// - 17 = LPR (vehicle plate)
        /// - 20 = Facial
        /// </summary>
        private async void btnNovaMidia_Click(object sender, EventArgs e)
        {
            if (_entidadeSelecionada == null) { ShowWarning("Select an entity"); return; }

            // Select the media type
            var tipos = new[] { "RFID Wiegand 26", "RFID Wiegand 34", "Plate (LPR)", "Facial" };
            var tipoIdx = SelectOption("Media Type", "Select the type:", tipos);
            if (tipoIdx < 0) return;

            int tipoMidia;
            switch (tipoIdx)
            {
                case 0:
                    tipoMidia = MediaType.Wiegand26;
                    break;
                case 1:
                    tipoMidia = MediaType.Wiegand34;
                    break;
                case 2:
                    tipoMidia = MediaType.Lpr;
                    break;
                case 3:
                    tipoMidia = MediaType.Facial;
                    break;
                default:
                    tipoMidia = MediaType.Wiegand26;
                    break;
            }

            // LPR should only be registered for vehicle-type entities.
            if (tipoMidia == MediaType.Lpr && _entidadeSelecionada.TypeAlias != (int)EntityType.Vehicle)
            {
                ShowWarning("LPR (plate) media can only be registered for Vehicle-type entities.");
                return;
            }

            string? descricao;
            if (tipoMidia == MediaType.Lpr)
            {
                // For vehicles, reuse the entity's plate without asking again.
                descricao = _entidadeSelecionada.Doc;
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    ShowWarning("This vehicle does not have a plate filled in the document field.");
                    return;
                }
            }
            else
            {
                descricao = InputBox("New Media", "Media code/description:");
            }
            if (string.IsNullOrEmpty(descricao)) return;

            var request = new CreateMediaRequest
            {
                EntityId = _entidadeSelecionada.EntityId,
                RegistryId = _entidadeSelecionada.RegistryId,
                TypeAlias = tipoMidia,
                DescriptionAlias = descricao!
            };

            // EXPLANATION: The backend validates the media format based on the content
            // of the "description" field. For RFID, it accepts Wiegand/CODE/HEX formats.
            // For LPR (plate), if we only send the description, the backend tries
            // to validate it as RFID and returns "invalid RFID format" error.
            //
            // SOLUTION: Sending ns32_0 and ns32_1 tells the backend that the binary data
            // has already been processed, so it doesn't apply RFID validation.
            // For manual LPR, we send 0 in both (the backend ignores them for LPR).
            //
            // NOTE: The RECOMMENDED way to create LPR is using lpr_active=true on the
            // vehicle entity registration, not via manual POST /media.
            if (tipoMidia == MediaType.Lpr)
            {
                request.Ns32_0 = 0;
                request.Ns32_1 = 0;
            }

            // Log JSON for debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(request);
            Log($"DEBUG JSON Media: {jsonDebug}");

            var result = await _api.Media.CreateAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Media created: {descricao} (ID: {result.Data.MediaId})");
                await LoadMedia(_entidadeSelecionada.EntityId);
            }
            else
            {
                var msg = $"Error creating media (HTTP {result.StatusCode}):\n{result.Message}\nJSON: {jsonDebug}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected media.
        /// DELETE /media?id=X
        /// </summary>
        private async void btnExcluirMidia_Click(object sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) { ShowWarning("Select a media item"); return; }

            var midia = listMidias.SelectedItems[0].Tag as AccessMedia;
            if (midia == null) return;

            // Confirmation before deleting
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the media '{midia.DescriptionAlias}'?\n\nThis action cannot be undone.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Media.DeleteAsync(midia.MediaId);
            if (result.Success)
            {
                Log($"Media deleted: {midia.DescriptionAlias}");
                if (_entidadeSelecionada != null)
                    await LoadMedia(_entidadeSelecionada.EntityId);
            }
            else
            {
                var msg = $"Error deleting media (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                ShowWarning(msg);
            }
        }

        private async void btnRefreshMidias_Click(object sender, EventArgs e)
        {
            if (_entidadeSelecionada != null)
                await LoadMedia(_entidadeSelecionada.EntityId);
        }

        // =====================================================================
        //  MEDIA DETAILS (Double-click)
        // =====================================================================

        /// <summary>
        /// Opens the media details form on double-click.
        /// </summary>
        private async void listMidias_DoubleClick(object sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) return;

            var midia = listMidias.SelectedItems[0].Tag as AccessMedia;
            if (midia == null) return;

            using var form = new FormDetalheMidia(midia);

            if (form.ShowDialog(this) == DialogResult.OK && form.FoiModificada)
            {
                bool success = true;
                string message = "";

                // Update the enabled state if changed
                if (midia.Enabled != form.NovoEstadoEnabled)
                {
                    var result = await _api.Media.ChangeStatusAsync(midia.MediaId, form.NovoEstadoEnabled);
                    if (!result.Success)
                    {
                        success = false;
                        message = result.Message ?? "Error changing status";
                    }
                    else
                    {
                        var status = form.NovoEstadoEnabled ? "enabled" : "blocked";
                        Log($"Media {midia.DescriptionAlias} {status} successfully!");
                    }
                }

                // Update the permission date if changed
                if (success && form.DataPermissaoAlterada)
                {
                    var result = await _api.Media.ChangeExpirationAsync(midia.MediaId, form.NovaDataPermissao);
                    if (!result.Success)
                    {
                        success = false;
                        message = result.Message ?? "Error changing permission date";
                    }
                    else
                    {
                        if (form.NovaDataPermissao > 0)
                            Log($"Media {midia.DescriptionAlias} permitted until {DateTimeOffset.FromUnixTimeSeconds(form.NovaDataPermissao).LocalDateTime:dd/MM/yyyy HH:mm}");
                        else
                            Log($"Expiration date removed from media {midia.DescriptionAlias}");
                    }
                }

                // If there was an error, show message
                if (!success)
                {
                    var msg = $"Error updating media:\n{message}";
                    Log(msg);
                    ShowWarning(msg);
                }

                // Reload the list in any case
                if (_entidadeSelecionada != null)
                    await LoadMedia(_entidadeSelecionada.EntityId);
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

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Displays a simple InputBox (text dialog)</summary>
        private string? InputBox(string title, string prompt)
        {
            var form = new Form { Text = title, Width = 400, Height = 150, StartPosition = FormStartPosition.CenterParent };
            var lbl = new Label { Text = prompt, Left = 10, Top = 10, Width = 360 };
            var txt = new TextBox { Left = 10, Top = 35, Width = 360 };
            var btnOk = new Button { Text = "OK", Left = 220, Top = 70, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 300, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        /// <summary>Displays an option selection dialog</summary>
        private int SelectOption(string title, string prompt, string[] options)
        {
            var form = new Form { Text = title, Width = 350, Height = 200, StartPosition = FormStartPosition.CenterParent };
            var lbl = new Label { Text = prompt, Left = 10, Top = 10, Width = 320 };
            var combo = new ComboBox { Left = 10, Top = 35, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(options);
            combo.SelectedIndex = 0;
            var btnOk = new Button { Text = "OK", Left = 170, Top = 70, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 250, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { lbl, combo, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form.ShowDialog() == DialogResult.OK ? combo.SelectedIndex : -1;
        }

        /// <summary>Asks for an optional ID (0 = automatic) and validates numeric input.</summary>
        private uint RequestOptionalId(string title, string prompt)
        {
            var valor = InputBox(title, prompt);
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            if (uint.TryParse(valor!.Trim(), out uint id)) return id;
            ShowWarning("Invalid ID. Using 0 (automatic).");
            return 0;
        }

    }
}
