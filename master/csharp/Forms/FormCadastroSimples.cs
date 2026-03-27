using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    // =============================================================================
    //  SIMPLIFIED REGISTRATION (2 Levels)
    //
    //  This form demonstrates the simplified registration model.
    //
    //  In this model, the integrator does NOT need to manage Central Registries.
    //  Just create the entity with createid=true and the controller automatically
    //  generates the registry_id and entity_id internally.
    //
    //  FLOW:
    //  1. Create Entity with createid=true (the controller creates everything)
    //  2. Add Media to the entity
    //
    //  DIFFERENCE FROM THE COMPLETE MODEL:
    //  - Complete: Registry -> Entity -> Media (3 levels, integrator manages everything)
    //  - Simplified: Entity -> Media (2 levels, controller generates IDs)
    //
    //  ENDPOINTS USED:
    //  - POST /entities  (with createid=true)
    //  - GET  /entities?offset=X&count=Y[&name=filter]  (paginated global list)
    //  - GET  /entities?id=X                            (search by ID)
    //  - GET/POST/DELETE /media
    // =============================================================================

    public partial class FormCadastroSimples : Form
    {
        private IMobiCortexClient _api = null!;
        private Entity? _selectedEntity;

        // Server-side pagination state
        private int _currentOffset = 0;
        private uint _totalEntities = 0;
        private const int PageSize = 10;

        // Current text filter (sent to server)
        private string _nameFilter = "";

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
        public FormCadastroSimples()
        {
            InitializeComponent();
        }

        public FormCadastroSimples(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        // =====================================================================
        //  ENTITIES (Level 1 in the simplified model)
        //  Difference: we use createid=true to create
        // =====================================================================

        private async void FormCadastroSimples_Load(object? sender, EventArgs e)
        {
            // In VS design mode, _api may be null - do not load data
            if (_api == null) return;
            await LoadEntities();
        }

        /// <summary>
        /// Loads the current page of entities from the server.
        /// GET /entities?offset=X&amp;count=Y[&amp;name=filter]
        /// A single HTTP call - pagination and filtering are done on the server.
        /// </summary>
        private async Task LoadEntities()
        {
            lblStatusEntidades.Text = "Loading...";
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _selectedEntity = null;

            var result = await _api.Entities.ListAllAsync(
                _currentOffset, PageSize,
                name: string.IsNullOrEmpty(_nameFilter) ? null : _nameFilter);

            if (!result.Success || result.Data == null)
            {
                lblStatusEntidades.Text = $"Error: {result.Message}";
                UpdatePagination();
                return;
            }

            _totalEntities = result.Data.Total;

            foreach (var ent in result.Data.Items)
            {
                var item = new ListViewItem(ent.EntityId.ToString());
                item.SubItems.Add(ent.TypeName);
                item.SubItems.Add(ent.DisplayName);
                item.SubItems.Add(ent.Doc);
                item.SubItems.Add(ent.Enabled ? "Y" : "N");
                item.SubItems.Add(ent.RegistryId.ToString());
                item.Tag = ent;
                listEntidades.Items.Add(item);
            }

            UpdatePagination();
        }

        /// <summary>
        /// Searches for a specific entity by entity_id.
        /// GET /entities?id=X
        /// </summary>
        private async Task SearchEntityById(uint entityId)
        {
            listEntidades.Items.Clear();
            listMidias.Items.Clear();
            _selectedEntity = null;

            var result = await _api.Entities.GetAsync(entityId);

            if (result.Success && result.Data != null && result.Data.Ret == 0)
            {
                var ent = result.Data;
                var item = new ListViewItem(ent.EntityId.ToString());
                item.SubItems.Add(ent.TypeName);
                item.SubItems.Add(ent.DisplayName);
                item.SubItems.Add(ent.Doc);
                item.SubItems.Add(ent.Enabled ? "Y" : "N");
                item.SubItems.Add(ent.RegistryId.ToString());
                item.Tag = ent;
                listEntidades.Items.Add(item);

                lblStatusEntidades.Text = $"Search by ID {entityId} - 1 result";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "ID";
            }
            else
            {
                lblStatusEntidades.Text = $"ID {entityId} not found";
                btnAnterior.Enabled = false;
                btnProxima.Enabled = false;
                lblPagina.Text = "0/0";
            }
        }

        /// <summary>Updates status label and pagination buttons</summary>
        private void UpdatePagination()
        {
            int totalPages = (int)((_totalEntities + PageSize - 1) / PageSize);
            int currentPage = totalPages > 0 ? (_currentOffset / PageSize) + 1 : 0;

            lblPagina.Text = $"{currentPage}/{totalPages}";
            btnAnterior.Enabled = _currentOffset > 0;
            btnProxima.Enabled = (_currentOffset + PageSize) < _totalEntities;
            lblStatusEntidades.Text = $"{_totalEntities} entity(ies) found";
        }

        /// <summary>When selecting an entity, loads its media.</summary>
        private async void listEntidades_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listEntidades.SelectedItems.Count == 0) return;

            _selectedEntity = listEntidades.SelectedItems[0].Tag as Entity;
            if (_selectedEntity == null) return;

            lblMidiasTitulo.Text = $"Media of: {_selectedEntity.DisplayName}";
            await LoadMedia(_selectedEntity.EntityId);
        }

        /// <summary>
        /// Creates an entity using the simplified model (createid=true).
        ///
        /// IMPORTANT: The main difference in this model is the createid field.
        /// When createid=true, the controller:
        /// 1. Generates an entity_id automatically
        /// 2. Creates a central registry automatically (or reuses an existing one)
        /// 3. Returns entity_id and central_registry_id in the response
        ///
        /// The integrator does not need to create the central registry beforehand.
        /// </summary>
        private async void btnNovaEntidade_Click(object? sender, EventArgs e)
        {
            // Select the type in a dedicated form (clearer than Yes/No).
            using var formTipo = new FormSelecionarTipoEntidade();
            if (formTipo.ShowDialog(this) != DialogResult.OK) return;
            int tipo = formTipo.TipoEntidadeSelecionado;

            string? nome;
            string doc;
            string? brand = null;
            string? model = null;
            string? color = null;
            bool lprActive;
            bool enabled;

            if (tipo == (int)EntityType.Person)
            {
                using var formPessoa = new FormCadastroPessoa(0);
                if (formPessoa.ShowDialog(this) != DialogResult.OK) return;

                nome = formPessoa.Nome;
                doc = formPessoa.Documento;
                lprActive = formPessoa.LprAtivo;
                enabled = formPessoa.EntidadeEnabled;
            }
            else if (tipo == (int)EntityType.Vehicle)
            {
                using var formVeiculo = new FormCadastroVeiculo(0, _api);
                if (formVeiculo.ShowDialog(this) != DialogResult.OK) return;

                nome = string.Empty;
                doc = formVeiculo.Placa;
                brand = string.IsNullOrWhiteSpace(formVeiculo.Marca) ? null : formVeiculo.Marca;
                model = string.IsNullOrWhiteSpace(formVeiculo.Modelo) ? null : formVeiculo.Modelo;
                color = string.IsNullOrWhiteSpace(formVeiculo.Cor) ? null : formVeiculo.Cor;
                lprActive = formVeiculo.LprAtivo;
                enabled = formVeiculo.EntidadeEnabled;
            }
            else
            {
                Warning("Entity type not supported in simplified registration.");
                return;
            }

            // *** CREATEID = TRUE ***  <- This is the main difference!
            var request = new CreateEntityRequest
            {
                // We do not provide central_registry_id.
                // The controller generates entity_id and central_registry_id automatically.
                Id = 0,
                CreateId = true,
                TypeAlias = tipo,
                Enabled = enabled,
                Name = nome,
                Doc = doc,
                Brand = brand,
                Model = model,
                Color = color,
                LprActive = lprActive
            };

            var result = await _api.Entities.CreateAsync(request);
            if (result.Success && result.Data?.Ret == 0)
            {
                var nameLog = !string.IsNullOrWhiteSpace(nome) ? nome : doc;
                Log($"Entity created: {nameLog}");
                Log($"  -> entity_id={result.Data.EntityId}, central_registry_id={result.Data.RegistryId}");
                if (result.Data.CreatedCentral == 1)
                    Log($"  -> Central registry created automatically");
                _currentOffset = 0;
                await LoadEntities();
            }
            else if (result.IsConflict)
            {
                // HTTP 409 = entity already exists on the controller.
                var nameLog = !string.IsNullOrWhiteSpace(nome) ? nome : doc;
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
                        _currentOffset = 0;
                        await LoadEntities();
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
                var msg = $"Error creating entity (HTTP {result.StatusCode}):\n{result.Message}";
                if (!string.IsNullOrEmpty(result.RawResponse))
                    msg += $"\nResponse: {result.RawResponse}";
                Log(msg);
                Warning(msg);
            }
        }

        /// <summary>
        /// Deletes the selected entity.
        /// DELETE /entities?id=X
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
                _currentOffset = 0;
                await LoadEntities();
            }
            else
            {
                var msg = $"Error deleting entity (HTTP {result.StatusCode}):\n{result.Message}";
                Log(msg);
                Warning(msg);
            }
        }

        private async void btnBuscarEntidade_Click(object? sender, EventArgs e)
        {
            var filter = txtFiltroEntidade.Text.Trim();
            _currentOffset = 0;

            // If a number was entered, search directly by entity_id (API)
            if (uint.TryParse(filter, out uint searchId))
            {
                await SearchEntityById(searchId);
                return;
            }

            // Text filter - sent to server
            _nameFilter = filter;
            await LoadEntities();
        }

        private async void btnAnterior_Click(object? sender, EventArgs e)
        {
            _currentOffset = Math.Max(0, _currentOffset - PageSize);
            await LoadEntities();
        }

        private async void btnProxima_Click(object? sender, EventArgs e)
        {
            _currentOffset += PageSize;
            await LoadEntities();
        }

        /// <summary>Enter key in the search field triggers the search</summary>
        private async void txtFiltroEntidade_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _currentOffset = 0;

                var filter = txtFiltroEntidade.Text.Trim();
                if (uint.TryParse(filter, out uint searchId))
                {
                    await SearchEntityById(searchId);
                    return;
                }

                _nameFilter = filter;
                await LoadEntities();
            }
        }

        // =====================================================================
        //  MEDIA (Level 2 in the simplified model)
        //  Works exactly the same as the complete model
        // =====================================================================

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
        /// Opens the media registration form to create a new media
        /// linked to the selected entity.
        /// </summary>
        private async void btnNovaMidia_Click(object? sender, EventArgs e)
        {
            if (_selectedEntity == null) { Warning("Select an entity"); return; }

            using var form = new FormCadastroMidia(_selectedEntity.EntityId, _selectedEntity.Doc);

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // LPR should only be registered for vehicle-type entities.
                if (form.TipoMidiaSelecionado == MediaType.Lpr &&
                    _selectedEntity.TypeAlias != (int)EntityType.Vehicle)
                {
                    Warning("LPR (plate) media can only be registered for Vehicle-type entities.");
                    return;
                }

                var request = new CreateMediaRequest
                {
                    EntityId = _selectedEntity.EntityId,
                    RegistryId = _selectedEntity.RegistryId,
                    TypeAlias = form.TipoMidiaSelecionado,
                    DescriptionAlias = form.TipoMidiaSelecionado == MediaType.Lpr &&
                                !string.IsNullOrWhiteSpace(_selectedEntity.Doc)
                        ? _selectedEntity.Doc
                        : form.DadosMidia
                };

                // EXPLANATION: The backend validates the media format based on the content
                // of the "description" field. For RFID, it accepts Wiegand/CODE/HEX formats.
                // For LPR (plate), if we send only the description, the backend tries
                // to validate it as RFID and returns "invalid RFID format" error.
                //
                // SOLUTION: Sending ns32_0 and ns32_1 tells the backend that the binary data
                // has already been processed, so it does not apply RFID validation.
                // For manual LPR, we send 0 in both (the backend ignores it for LPR).
                // NOTE: The recommended approach is to use lpr_enabled=true on the vehicle entity,
                // not via manual POST /media.
                if (form.TipoMidiaSelecionado == MediaType.Lpr)
                {
                    request.Ns32_0 = 0;
                    request.Ns32_1 = 0;
                }

                var result = await _api.Media.CreateAsync(request);
                if (result.Success && result.Data?.Ret == 0)
                {
                    Log($"Media created: {form.TipoMidiaNome} - {form.DadosMidia} (ID: {result.Data.MediaId})");
                    await LoadMedia(_selectedEntity.EntityId);
                }
                else
                {
                    var msg = $"Error creating media (HTTP {result.StatusCode}):\n{result.Message}";
                    Log(msg);
                    Warning(msg);
                }
            }
        }

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

        /// <summary>Displays a Yes/No dialog and returns true for yes.</summary>
        private bool ConfirmYesNo(string question, string title)
        {
            var answer = MessageBox.Show(
                question,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            return answer == DialogResult.Yes;
        }
    }
}
