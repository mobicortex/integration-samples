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
    //     Represents a person, vehicle or animal linked to the registry.
    //     Each registry can have multiple entities.
    //
    //  3. ACCESS MEDIA (media)
    //     Represents an access credential (RFID card, biometrics, plate, etc).
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
        private IMobiCortexClient _api = null!;

        // Currently selected items (for hierarchical navigation)
        private CentralRegistry? _selectedRegistry;
        private Entity? _selectedEntity;

        // Registry pagination state
        private int _currentOffset = 0;
        private const int PageSize = 20;
        private uint _totalRegistries = 0;

        /// <summary>
        /// API service. Can be set via property for designer use.
        /// </summary>
        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

        /// <summary>
        /// Default constructor for Visual Studio Designer.
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

        private async void FormCadastroCompleto_Load(object? sender, EventArgs e)
        {
            // In VS design mode, _api may be null - do not load data
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
            _selectedRegistry = null;
            _selectedEntity = null;

            if (result.Success && result.Data != null)
            {
                _totalRegistries = result.Data.Total;
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
                _totalRegistries = 0;
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
            _selectedRegistry = null;
            _selectedEntity = null;

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

                _totalRegistries = 1;
                lblStatusCadastros.Text = $"Search by ID {id} - 1 result";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                _totalRegistries = 0;
                lblStatusCadastros.Text = $"ID {id} not found";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Updates status label and pagination buttons</summary>
        private void UpdatePagination()
        {
            int totalPages = (int)((_totalRegistries + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalRegistries;
            lblStatusCadastros.Text = $"{_totalRegistries} registry(ies)";
        }

        /// <summary>When selecting a registry, loads its entities (level 2).</summary>
        private async void listCadastros_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listCadastros.SelectedItems.Count == 0) return;

            _selectedRegistry = listCadastros.SelectedItems[0].Tag as CentralRegistry;
            if (_selectedRegistry == null) return;

            lblEntidadesTitulo.Text = $"Entities of: {_selectedRegistry.Name}";
            await LoadEntities(_selectedRegistry.Id);
        }

        /// <summary>
        /// Creates a new central registry using the complete form.
        /// POST /central-registry with body: { "id": auto, "name": "...", "enabled": true }
        /// </summary>
        private async void btnNovoCadastro_Click(object? sender, EventArgs e)
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
                Warning($"The registry '{form.Nome}' already exists on the controller.");
            }
            else
            {
                var msg = $"Error creating registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Edits the selected registry on double click.
        /// PUT /central-registry?id=X
        /// </summary>
        private async void listCadastros_DoubleClick(object? sender, EventArgs e)
        {
            // Get the clicked item directly (works even if not selected)
            if (listCadastros.SelectedItems.Count == 0) return;

            var cadastro = listCadastros.SelectedItems[0].Tag as CentralRegistry;
            if (cadastro == null) return;

            // Update the selected registry
            _selectedRegistry = cadastro;

            using var form = new FormCadastroCentral(cadastro);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var updatedRegistry = new CentralRegistry
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
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(updatedRegistry);
            Log($"DEBUG: Updating registry ID={updatedRegistry.Id}, JSON={jsonDebug}");

            var result = await _api.Registries.UpdateAsync(updatedRegistry);

            if (result.Success)
            {
                Log($"Registry updated: {form.Nome}");
                await LoadRegistries();
            }
            else
            {
                var msg = $"Error updating registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected registry and all its entities/media.
        /// DELETE /central-registry?id=X
        /// </summary>
        private async void btnExcluirCadastro_Click(object? sender, EventArgs e)
        {
            if (_selectedRegistry == null) { Warning("Select a registry"); return; }

            var confirm = MessageBox.Show(
                $"Delete '{_selectedRegistry.Name}' and all its entities/media?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Registries.DeleteAsync(_selectedRegistry.Id);
            if (result.Success)
            {
                Log($"Registry deleted: {_selectedRegistry.Name}");
                await LoadRegistries();
            }
            else
            {
                var msg = $"Error deleting registry (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        private async void btnBuscarCadastro_Click(object? sender, EventArgs e)
        {
            _currentOffset = 0; // New search always starts from the beginning
            await LoadRegistries();
        }

        private async void btnRefreshCadastros_Click(object? sender, EventArgs e)
        {
            await LoadRegistries();
        }

        private async void btnAnterior_Click(object? sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            await LoadRegistries();
        }

        private async void btnProxima_Click(object? sender, EventArgs e)
        {
            _currentOffset += PageSize;
            await LoadRegistries();
        }

        /// <summary>Enter key in the search field triggers the search</summary>
        private async void txtFiltroCadastro_KeyDown(object? sender, KeyEventArgs e)
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
        private async Task LoadEntities(uint registryId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _selectedEntity = null;

            var result = await _api.Entities.ListByRegistryAsync(registryId);

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
        private async void listEntidades_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _selectedEntity = listEntidades.SelectedItems[0].Tag as Entity;
            if (_selectedEntity == null) return;

            lblMidiasTitulo.Text = $"Media of: {_selectedEntity.DisplayName}";
            await LoadMedia(_selectedEntity.EntityId);
        }

        /// <summary>
        /// Edits the selected entity on double click.
        /// </summary>
        private async void listEntidades_DoubleClick(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            var entidade = listEntidades.SelectedItems[0].Tag as Entity;
            if (entidade == null) return;

            _selectedEntity = entidade;

            // Check entity type
            if (entidade.TypeAlias == (int)EntityType.Vehicle)
            {
                // Open vehicle edit form
                using var formVeiculo = new FormCadastroVeiculo(entidade, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                // Create the update request (PUT /entities?id=X)
                var updatedEntity = new UpdateEntityRequest
                {
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprActive = formVeiculo.LprAtivo
                };

                // DEBUG: Log the JSON being sent
                var jsonDebug = System.Text.Json.JsonSerializer.Serialize(updatedEntity);
                Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebug}");

                var result = await _api.Entities.UpdateAsync(entidade.EntityId, updatedEntity);
                if (result.Success)
                {
                    Log($"Vehicle updated: {entidade.DisplayName}");
                    if (_selectedRegistry != null)
                        await LoadEntities(_selectedRegistry.Id);
                }
                else
                {
                    var msg = $"Error updating vehicle:\n{result.Message}\nResponse: {result.RawResponse}";
                    Log(msg);
                    Warning(msg);
                }
                return;
            }

            // Open person edit form
            using var form = new FormCadastroPessoaEdit(entidade);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            // Create the update request (PUT /entities?id=X)
            // Uses UpdateEntityRequest - only sends the fields to be changed
            var cleanDoc = string.IsNullOrWhiteSpace(form.Documento) ? null : form.Documento.Trim();
            var updatedPerson = new UpdateEntityRequest
            {
                Name = form.Nome,
                Doc = cleanDoc, // null if empty (will not be sent in JSON)
                Enabled = form.EntidadeEnabled,
                // People (type 1) do not use LPR - do not send the field (null)
                LprActive = entidade.TypeAlias == 1 ? null : entidade.LprActive
            };

            // DEBUG: Log the JSON being sent
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Form.EntidadeEnabled = {form.EntidadeEnabled}");
            var jsonDebugPessoa = System.Text.Json.JsonSerializer.Serialize(updatedPerson);
            Log($"DEBUG PUT /entities?id={entidade.EntityId}: {jsonDebugPessoa}");

            var resultPessoa = await _api.Entities.UpdateAsync(entidade.EntityId, updatedPerson);
            if (resultPessoa.Success)
            {
                Log($"Entity updated: {form.Nome}");
                if (_selectedRegistry != null)
                    await LoadEntities(_selectedRegistry.Id);
            }
            else
            {
                var msg = $"Error updating entity:\n{resultPessoa.Message}\nResponse: {resultPessoa.RawResponse}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Creates a new entity linked to the selected registry.
        /// POST /entities with body: { "central_registry_id": X, "type": 1, "name": "...", "doc": "..." }
        ///
        /// NOTE: In this model (complete), we provide the central_registry_id of the existing registry.
        /// If the entity ID is 0, the client sends createid=true for the controller to generate the entity_id.
        /// </summary>
        private async void btnNovaEntidade_Click(object? sender, EventArgs e)
        {
            if (_selectedRegistry == null) { Warning("Select a registry first"); return; }

            // Select the type in a dedicated form (clearer than Yes/No).
            using var formTipo = new FormSelecionarTipoEntidade();
            if (formTipo.ShowDialog(this) != DialogResult.OK) return;
            int tipo = formTipo.TipoEntidadeSelecionado;

            CreateEntityRequest request;
            string nameLog;

            if (tipo == (int)EntityType.Vehicle)
            {
                using var formVeiculo = new FormCadastroVeiculo(_selectedRegistry.Id, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                request = new CreateEntityRequest
                {
                    Id = formVeiculo.IdVeiculo,
                    CreateId = formVeiculo.IdVeiculo == 0 ? true : null,
                    RegistryId = _selectedRegistry.Id,
                    TypeAlias = (int)EntityType.Vehicle,
                    Doc = formVeiculo.Placa,
                    Enabled = formVeiculo.EntidadeEnabled,
                    Brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca,
                    Model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo,
                    Color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor,
                    LprActive = formVeiculo.LprAtivo
                };

                nameLog = $"{formVeiculo.Placa}";
            }
            else
            {
                using var formPessoa = new FormCadastroPessoa(_selectedRegistry.Id);
                if (formPessoa.ShowDialog(this) != DialogResult.OK) return;

                request = new CreateEntityRequest
                {
                    Id = formPessoa.Id,
                    RegistryId = _selectedRegistry.Id,
                    TypeAlias = tipo,
                    Name = formPessoa.Nome,
                    Doc = formPessoa.Documento,
                    LprActive = formPessoa.LprAtivo,
                    Enabled = formPessoa.EntidadeEnabled
                };
                nameLog = formPessoa.Nome;
            }

            // Log JSON for debug
            var jsonDebug = System.Text.Json.JsonSerializer.Serialize(request);
            Log($"DEBUG JSON Entity: {jsonDebug}");

            var result = await _api.Entities.CreateAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                Log($"Entity created: {nameLog} (ID: {result.Data.EntityId})");
                await LoadEntities(_selectedRegistry.Id);
            }
            else if (result.IsConflict)
            {
                // HTTP 409 = entity already exists on the controller.
                Log($"Entity already exists: {nameLog} (HTTP 409 Conflict)");

                var answer = MessageBox.Show(
                    $"The entity '{nameLog}' already exists on the controller.\n\nDo you want to overwrite the existing data?",
                    "Entity already registered",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer == DialogResult.Yes)
                {
                    request.Overwrite = true;
                    var retryResult = await _api.Entities.CreateAsync(request);
                    if (retryResult.Success && retryResult.Data?.Ret == 0)
                    {
                        Log($"Entity overwritten: {nameLog} (entity_id={retryResult.Data.EntityId})");
                        await LoadEntities(_selectedRegistry.Id);
                    }
                    else
                    {
                        var msg = $"Error overwriting entity (HTTP {retryResult.StatusCode}):\n{retryResult.Message}";
                        Log(msg);
                        Warning(msg);
                    }
                }
            }
            else
            {
                var msg = $"Error creating entity (HTTP {result.StatusCode}):\n{result.Message}\nJSON: {jsonDebug}";
                if (!string.IsNullOrEmpty(result.RawResponse))
                    msg += $"\nResponse: {result.RawResponse}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected entity and all its media.
        /// DELETE /entities?id=X (cascade: removes linked media)
        /// </summary>
        private async void btnExcluirEntidade_Click(object? sender, EventArgs e)
        {
            if (_selectedEntity == null) { Warning("Select an entity"); return; }

            var confirm = MessageBox.Show(
                $"Delete '{_selectedEntity.DisplayName}' and all its media?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Entities.DeleteAsync(_selectedEntity.EntityId);
            if (result.Success)
            {
                Log($"Entity deleted: {_selectedEntity.DisplayName}");
                if (_selectedRegistry != null)
                    await LoadEntities(_selectedRegistry.Id);
            }
            else
            {
                var msg = $"Error deleting entity (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        private async void btnRefreshEntidades_Click(object? sender, EventArgs e)
        {
            if (_selectedRegistry != null)
                await LoadEntities(_selectedRegistry.Id);
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
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_selectedEntity == null) { Warning("Select an entity"); return; }

            // Select media type
            var types = new[] { "RFID Wiegand 26", "RFID Wiegand 34", "Plate (LPR)", "Facial" };
            var typeIdx = SelectOption("Media Type", "Select the type:", types);
            if (typeIdx < 0) return;

            int mediaType = typeIdx switch
            {
                0 => MediaType.Wiegand26,
                1 => MediaType.Wiegand34,
                2 => MediaType.Lpr,
                3 => MediaType.Facial,
                _ => MediaType.Wiegand26
            };

            // LPR should only be registered for vehicle-type entities.
            if (mediaType == MediaType.Lpr && _selectedEntity.TypeAlias != (int)EntityType.Vehicle)
            {
                Warning("LPR (plate) media can only be registered for Vehicle-type entities.");
                return;
            }

            string? description;
            if (mediaType == MediaType.Lpr)
            {
                // For vehicles, reuse the entity plate without asking again.
                description = _selectedEntity.Doc;
                if (string.IsNullOrWhiteSpace(description))
                {
                    Warning("This vehicle does not have a plate filled in the document field.");
                    return;
                }
            }
            else
            {
                description = InputBox("New Media", "Media code/description:");
            }
            if (string.IsNullOrEmpty(description)) return;

            var request = new CreateMediaRequest
            {
                EntityId = _selectedEntity.EntityId,
                RegistryId = _selectedEntity.RegistryId,
                TypeAlias = mediaType,
                DescriptionAlias = description
            };

            // EXPLANATION: The backend validates the media format based on the content
            // of the "description" field. For RFID, it accepts Wiegand/CODE/HEX formats.
            // For LPR (plate), if we send only the description, the backend tries
            // to validate it as RFID and returns "invalid RFID format" error.
            //
            // SOLUTION: Sending ns32_0 and ns32_1 tells the backend that the binary data
            // has already been processed, so it does not apply RFID validation.
            // For manual LPR, we send 0 in both (the backend ignores it for LPR).
            //
            // NOTE: The RECOMMENDED way to create LPR is using lpr_active=true on the
            // vehicle entity registration, not via manual POST /media.
            if (mediaType == MediaType.Lpr)
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
                Log($"Media created: {description} (ID: {result.Data.MediaId})");
                await LoadMedia(_selectedEntity.EntityId);
            }
            else
            {
                var msg = $"Error creating media (HTTP {result.StatusCode}):\n{result.Message}\nJSON: {jsonDebug}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected media.
        /// DELETE /media?id=X
        /// </summary>
        private async void btnExcluirMidia_Click(object? sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) { Warning("Select a media item"); return; }

            var media = listMidias.SelectedItems[0].Tag as AccessMedia;
            if (media == null) return;

            // Confirmation before deleting
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the media '{media.DescriptionAlias}'?\n\nThis action cannot be undone.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = await _api.Media.DeleteAsync(media.MediaId);
            if (result.Success)
            {
                Log($"Media deleted: {media.DescriptionAlias}");
                if (_selectedEntity != null)
                    await LoadMedia(_selectedEntity.EntityId);
            }
            else
            {
                var msg = $"Error deleting media (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        private async void btnRefreshMidias_Click(object? sender, EventArgs e)
        {
            if (_selectedEntity != null)
                await LoadMedia(_selectedEntity.EntityId);
        }

        // =====================================================================
        //  MEDIA DETAILS (Double click)
        // =====================================================================

        /// <summary>
        /// Opens the media details form on double click.
        /// </summary>
        private async void listMidias_DoubleClick(object? sender, EventArgs e)
        {
            if (listMidias.SelectedItems.Count == 0) return;

            var media = listMidias.SelectedItems[0].Tag as AccessMedia;
            if (media == null) return;

            using var form = new FormDetalheMidia(media);

            if (form.ShowDialog(this) == DialogResult.OK && form.FoiModificada)
            {
                bool success = true;
                string message = "";

                // Update the enabled state if changed
                if (media.Enabled != form.NovoEstadoEnabled)
                {
                    var result = await _api.Media.ChangeStatusAsync(media.MediaId, form.NovoEstadoEnabled);
                    if (!result.Success)
                    {
                        success = false;
                        message = result.Message ?? "Error changing status";
                    }
                    else
                    {
                        var status = form.NovoEstadoEnabled ? "enabled" : "blocked";
                        Log($"Media {media.DescriptionAlias} {status} successfully!");
                    }
                }

                // Update the permission date if changed
                if (success && form.DataPermissaoAlterada)
                {
                    var result = await _api.Media.ChangeExpirationAsync(media.MediaId, form.NovaDataPermissao);
                    if (!result.Success)
                    {
                        success = false;
                        message = result.Message ?? "Error changing permission date";
                    }
                    else
                    {
                        if (form.NovaDataPermissao > 0)
                            Log($"Media {media.DescriptionAlias} permitted until {DateTimeOffset.FromUnixTimeSeconds(form.NovaDataPermissao).LocalDateTime:dd/MM/yyyy HH:mm}");
                        else
                            Log($"Expiration date removed from media {media.DescriptionAlias}");
                    }
                }

                // If there was an error, show the message
                if (!success)
                {
                    var msg = $"Error updating media:\n{message}";
                    Log(msg);
                    Warning(msg);
                }

                // Reload the list in any case
                if (_selectedEntity != null)
                    await LoadMedia(_selectedEntity.EntityId);
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

        private void Warning(string msg) =>
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        /// <summary>Requests an optional ID (0 = automatic) and validates numeric input.</summary>
        private uint RequestOptionalId(string title, string prompt)
        {
            var value = InputBox(title, prompt);
            if (string.IsNullOrWhiteSpace(value)) return 0;
            if (uint.TryParse(value.Trim(), out uint id)) return id;
            Warning("Invalid ID. Using 0 (automatic).");
            return 0;
        }

    }
}
