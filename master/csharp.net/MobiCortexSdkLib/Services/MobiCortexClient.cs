using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Exceptions;

namespace MobiCortex.Sdk.Services
{
    /// <summary>
    /// Cliente principal para integraÃ§Ã£o com controladores MobiCortex.
    /// </summary>
    public class MobiCortexClient : IMobiCortexClient, ICadastroService, IEntidadeService, IMidiaService, ISistemaService, IAccessService, IWebhookConfigService, IVideoSourceService
    {
        private const string API = "/mbcortex/master/api/v1";
        private readonly HttpClient _http;
        private string _baseUrl = "";
        private string? _sessionKey;

        private readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <inheritdoc/>
        public bool IsAuthenticated => !string.IsNullOrEmpty(_sessionKey);

        /// <inheritdoc/>
        public string? SessionKey => _sessionKey;

        /// <inheritdoc/>
        public ICadastroService Cadastros => this;

        /// <inheritdoc/>
        public IEntidadeService Entidades => this;

        /// <inheritdoc/>
        public IMidiaService Midias => this;

        /// <inheritdoc/>
        public ISistemaService Sistema => this;

        /// <inheritdoc/>
        public IAccessService Acesso => this;

        /// <inheritdoc/>
        public IWebhookConfigService Webhooks => this;

        /// <inheritdoc/>
        public IVideoSourceService VideoSources => this;

        /// <summary>
        /// Cria uma nova instÃ¢ncia do cliente MobiCortex.
        /// </summary>
        public MobiCortexClient()
        {
            _http = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            });
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <inheritdoc/>
        public void ConfigureBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        /// <inheritdoc/>
        public string BaseUrl => _baseUrl;

        /// <inheritdoc/>
        public async Task<ApiResult<LoginResponse>> LoginAsync(string password)
        {
            var request = new { pass = password };
            var result = await PostAsync<LoginResponse>("/login", request);
            
            if (result.Success && result.Data?.SessionKey != null)
            {
                _sessionKey = result.Data.SessionKey;
                _http.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sessionKey);
            }
            
            return result;
        }

        /// <inheritdoc/>
        public async Task<ApiResult<bool>> TestConnectionAsync()
        {
            try
            {
                // Verifica se a URL base foi configurada
                if (string.IsNullOrEmpty(_baseUrl))
                    return new ApiResult<bool> { Success = false, Message = "URL base nÃ£o configurada. Chame ConfigureBaseUrl() primeiro." };

                // Tenta acessar o endpoint /login com POST vazio para testar conectividade
                // O endpoint /login aceita POST e retorna 400 apenas se o body estiver incorreto
                var url = $"{_baseUrl}/mbcortex/master/api/v1/login";
                
                // Faz POST com body vazio/invÃ¡lido - esperamos 400 Bad Request se servidor responder
                // Isso Ã© suficiente para confirmar que o servidor estÃ¡ online
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);
                
                var responseBody = await response.Content.ReadAsStringAsync();
                
                // Se recebeu qualquer resposta HTTP (mesmo 400), o servidor estÃ¡ respondendo
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || 
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    response.IsSuccessStatusCode)
                {
                    return new ApiResult<bool> { Success = true, Data = true, RawResponse = $"HTTP {(int)response.StatusCode}" };
                }
                
                return new ApiResult<bool> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = responseBody };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResult<bool> { Success = false, Message = $"Falha na conexÃ£o: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResult<bool> { Success = false, Message = $"Erro: {ex.Message}" };
            }
        }

        #region ICadastroService
        async Task<ApiResult<CadastroListResponse>> ICadastroService.ListarAsync(int offset, int count, string? nameFilter, string? searchFilter)
        {
            var url = $"/central-registry?offset={offset}&count={count}";
            if (!string.IsNullOrEmpty(searchFilter))
                url += $"&search={Uri.EscapeDataString(searchFilter)}";
            else if (!string.IsNullOrEmpty(nameFilter))
                url += $"&name={Uri.EscapeDataString(nameFilter)}";
            return await GetAsync<CadastroListResponse>(url);
        }

        async Task<ApiResult<CadastroCentral>> ICadastroService.ObterAsync(uint id)
        {
            return await GetAsync<CadastroCentral>($"/central-registry?id={id}");
        }

        async Task<ApiResult<ApiRetResponse>> ICadastroService.CriarAsync(CadastroCentral cadastro)
        {
            return await PostAsync<ApiRetResponse>("/central-registry", BuildCentralRegistryPayload(cadastro));
        }

        async Task<ApiResult<ApiRetResponse>> ICadastroService.AtualizarAsync(CadastroCentral cadastro)
        {
            // API usa POST para criar/atualizar (nÃ£o PUT)
            return await PostAsync<ApiRetResponse>("/central-registry", BuildCentralRegistryPayload(cadastro));
        }

        async Task<ApiResult<ApiRetResponse>> ICadastroService.ExcluirAsync(uint id)
        {
            return await DeleteAsync<ApiRetResponse>($"/central-registry?id={id}");
        }

        async Task<ApiResult<CadastroStats>> ICadastroService.ObterEstatisticasAsync()
        {
            return await GetAsync<CadastroStats>("/central-registry/stats");
        }
        #endregion

        #region IEntidadeService
        async Task<ApiResult<EntidadeListResponse>> IEntidadeService.ListarPorCadastroAsync(uint centralRegistryId)
        {
            return await GetAsync<EntidadeListResponse>($"/entities?central_registry_id={centralRegistryId}");
        }

        async Task<ApiResult<EntidadeListResponse>> IEntidadeService.ListarTodosAsync(int offset, int count, string? nome)
        {
            var url = $"/entities?offset={offset}&count={count}";
            if (!string.IsNullOrEmpty(nome))
                url += $"&name={Uri.EscapeDataString(nome)}";
            return await GetAsync<EntidadeListResponse>(url);
        }

        async Task<ApiResult<Entidade>> IEntidadeService.ObterAsync(uint entityId)
        {
            return await GetAsync<Entidade>($"/entities?id={entityId}");
        }

        async Task<ApiResult<CriarEntidadeResponse>> IEntidadeService.CriarAsync(CriarEntidadeRequest request)
        {
            return await PostAsync<CriarEntidadeResponse>("/entities", BuildEntityCreatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IEntidadeService.AtualizarAsync(uint entityId, AtualizarEntidadeRequest request)
        {
            // API usa PUT /entities?id=X para atualizar entidade parcialmente
            return await PutAsync<ApiRetResponse>($"/entities?id={entityId}", BuildEntityUpdatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IEntidadeService.ExcluirAsync(uint entityId)
        {
            return await DeleteAsync<ApiRetResponse>($"/entities?id={entityId}");
        }

        async Task<ApiResult<VehicleDriverListResponse>> IEntidadeService.ObterCondutoresVeiculoAsync(uint vehicleId)
        {
            return await GetAsync<VehicleDriverListResponse>($"/vehicle-drivers?vehicle_id={vehicleId}");
        }

        async Task<ApiResult<VehicleDriverUpdateResponse>> IEntidadeService.AtualizarCondutoresVeiculoAsync(uint vehicleId, IEnumerable<uint> driverIds)
        {
            return await PutAsync<VehicleDriverUpdateResponse>($"/vehicle-drivers?vehicle_id={vehicleId}", new UpdateVehicleDriversRequest
            {
                DriverIds = driverIds?.Distinct().ToList() ?? new List<uint>()
            });
        }
        #endregion

        #region IMidiaService
        async Task<ApiResult<MidiaListResponse>> IMidiaService.ListarPorEntidadeAsync(uint entityId)
        {
            return await GetAsync<MidiaListResponse>($"/media?entity_id={entityId}");
        }

        async Task<ApiResult<MidiaAcesso>> IMidiaService.ObterAsync(uint mediaId)
        {
            return await GetAsync<MidiaAcesso>($"/media?id={mediaId}");
        }

        async Task<ApiResult<CriarMidiaResponse>> IMidiaService.CriarAsync(CriarMidiaRequest request)
        {
            return await PostAsync<CriarMidiaResponse>("/media", BuildMediaCreatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IMidiaService.AlterarStatusAsync(uint mediaId, bool enabled)
        {
            return await PutAsync<ApiRetResponse>($"/media?id={mediaId}", new { enabled });
        }

        async Task<ApiResult<ApiRetResponse>> IMidiaService.AlterarExpiracaoAsync(uint mediaId, uint expiration)
        {
            return await PutAsync<ApiRetResponse>($"/media?id={mediaId}", new { expiration });
        }

        async Task<ApiResult<ApiRetResponse>> IMidiaService.ExcluirAsync(uint mediaId)
        {
            return await DeleteAsync<ApiRetResponse>($"/media?id={mediaId}");
        }
        #endregion

        #region ISistemaService
        async Task<ApiResult<DeviceInfo>> ISistemaService.ObterDeviceInfoAsync()
        {
            return await GetAsync<DeviceInfo>("/device-info");
        }

        async Task<ApiResult<DashboardStats>> ISistemaService.ObterDashboardAsync()
        {
            return await GetAsync<DashboardStats>("/dashboard");
        }

        async Task<ApiResult<VehicleCatalogsResponse>> ISistemaService.ObterCatalogosVeiculoAsync()
        {
            return await GetAsync<VehicleCatalogsResponse>("/vehicle-catalogs");
        }

        #endregion

        #region IAccessService
        async Task<ApiResult<ApiRetResponse>> IAccessService.AlterarSenhaAsync(ChangePasswordRequest request)
        {
            return await PutAsync<ApiRetResponse>("/login", request);
        }

        async Task<ApiResult<ApiTokenListResponse>> IAccessService.ListarTokensAsync()
        {
            return await GetAsync<ApiTokenListResponse>("/token");
        }

        async Task<ApiResult<ApiTokenCreateResponse>> IAccessService.CriarTokenAsync(CreateApiTokenRequest request)
        {
            return await PostAsync<ApiTokenCreateResponse>("/token", request);
        }

        async Task<ApiResult<ApiRetResponse>> IAccessService.ExcluirTokenAsync(string token)
        {
            return await DeleteAsync<ApiRetResponse>("/token", new { token });
        }
        #endregion

        #region IWebhookConfigService
        async Task<ApiResult<WebhookListResponse>> IWebhookConfigService.ListarAsync()
        {
            return await GetAsync<WebhookListResponse>("/webhook");
        }

        async Task<ApiResult<WebhookConfig>> IWebhookConfigService.ObterAsync(int id)
        {
            return await GetAsync<WebhookConfig>($"/webhook?id={id}");
        }

        async Task<ApiResult<WebhookConfig>> IWebhookConfigService.SalvarAsync(int id, WebhookConfig config)
        {
            return await PostAsync<WebhookConfig>($"/webhook?id={id}", config);
        }

        async Task<ApiResult<ApiRetResponse>> IWebhookConfigService.ExcluirAsync(int id)
        {
            return await DeleteAsync<ApiRetResponse>($"/webhook?id={id}");
        }
        #endregion

        #region IVideoSourceService
        async Task<ApiResult<VideoSourceListResponse>> IVideoSourceService.ListarAsync()
        {
            return await GetAsync<VideoSourceListResponse>("/video-source");
        }

        async Task<ApiResult<VideoSourceConfig>> IVideoSourceService.ObterAsync(int id)
        {
            return await GetAsync<VideoSourceConfig>($"/video-source?id={id}");
        }

        async Task<ApiResult<VideoSourceConfig>> IVideoSourceService.SalvarAsync(int id, VideoSourceConfig config)
        {
            return await PostAsync<VideoSourceConfig>($"/video-source?id={id}", config);
        }

        async Task<ApiResult<ApiRetResponse>> IVideoSourceService.ExcluirAsync(int id)
        {
            return await DeleteAsync<ApiRetResponse>($"/video-source?id={id}");
        }
        #endregion

        #region MÃ©todos HTTP Privados
        private static object BuildCentralRegistryPayload(CadastroCentral cadastro)
        {
            var payload = new Dictionary<string, object?>
            {
                ["id"] = cadastro.Id,
                ["name"] = cadastro.Name ?? string.Empty,
                ["enabled"] = cadastro.Enabled,
                ["type"] = cadastro.Type,
                ["slots1"] = cadastro.Slots1,
                ["slots2"] = cadastro.Slots2
            };

            AddIfNotBlank(payload, "field1", cadastro.Field1);
            AddIfNotBlank(payload, "field2", cadastro.Field2);
            AddIfNotBlank(payload, "field3", cadastro.Field3);
            AddIfNotBlank(payload, "field4", cadastro.Field4);

            return payload;
        }

        private static object BuildEntityCreatePayload(CriarEntidadeRequest request)
        {
            var payload = new Dictionary<string, object?>
            {
                ["type"] = request.Type,
                ["enabled"] = request.Enabled
            };

            var centralRegistryId = request.CentralRegistryId.GetValueOrDefault();
            if (centralRegistryId > 0)
                payload["central_registry_id"] = centralRegistryId;

            if (request.Id == 0)
                payload["createid"] = true;
            else
                payload["id"] = request.Id;

            AddIfNotBlank(payload, "doc", request.Doc);

            if (request.Type == 2)
            {
                AddIfNotBlank(payload, "brand", request.Brand);
                AddIfNotBlank(payload, "model", request.Model);
                AddIfNotBlank(payload, "color", request.Color);
                payload["lpr_enabled"] = request.LprEnabled;
            }
            else
            {
                payload["name"] = request.Name ?? string.Empty;
                if (request.LprEnabled)
                    payload["lpr_enabled"] = true;
            }

            if (request.Id > 0 && request.Overwrite == true)
                payload["overwrite"] = true;

            return payload;
        }

        private static object BuildMediaCreatePayload(CriarMidiaRequest request)
        {
            var payload = new Dictionary<string, object?>
            {
                ["entity_id"] = request.EntityId,
                ["central_registry_id"] = request.CentralRegistryId,
                ["type"] = request.Type,
                ["enabled"] = true
            };

            AddIfNotBlank(payload, "description", request.Description);

            if (request.Ns32_0.HasValue)
                payload["ns32_0"] = request.Ns32_0.Value;
            if (request.Ns32_1.HasValue)
                payload["ns32_1"] = request.Ns32_1.Value;

            return payload;
        }


        private static object BuildEntityUpdatePayload(AtualizarEntidadeRequest request)
        {
            var payload = new Dictionary<string, object?>();

            if (request.Enabled.HasValue)
                payload["enabled"] = request.Enabled.Value;

            AddIfNotBlank(payload, "name", request.Name);
            AddIfNotBlank(payload, "doc", request.Doc);
            AddIfNotBlank(payload, "brand", request.Brand);
            AddIfNotBlank(payload, "model", request.Model);
            AddIfNotBlank(payload, "color", request.Color);

            if (request.LprEnabled.HasValue)
                payload["lpr_enabled"] = request.LprEnabled.Value;

            return payload;
        }
        private static void AddIfNotBlank(IDictionary<string, object?> payload, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                payload[key] = value.Trim();
        }
        private async Task<ApiResult<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var response = await _http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new ApiResult<T> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = json };

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }

        private async Task<ApiResult<T>> PostAsync<T>(string endpoint, object body)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var jsonBody = JsonSerializer.Serialize(body, _json);
                Debug.WriteLine($"[MobiCortex SDK] POST {url}");
                Debug.WriteLine($"[MobiCortex SDK] Body: {jsonBody}");
                
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);
                var json = await response.Content.ReadAsStringAsync();
                
                Debug.WriteLine($"[MobiCortex SDK] Response: HTTP {(int)response.StatusCode} - {json}");
                
                if (!response.IsSuccessStatusCode)
                    return new ApiResult<T> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = json };

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MobiCortex SDK] Error: {ex.Message}");
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }

        private async Task<ApiResult<T>> PutAsync<T>(string endpoint, object body)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var jsonBody = JsonSerializer.Serialize(body, _json);
                Debug.WriteLine($"[MobiCortex SDK] PUT {url}");
                Debug.WriteLine($"[MobiCortex SDK] Body: {jsonBody}");
                
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var response = await _http.PutAsync(url, content);
                var json = await response.Content.ReadAsStringAsync();
                
                Debug.WriteLine($"[MobiCortex SDK] Response: HTTP {(int)response.StatusCode} - {json}");
                
                if (!response.IsSuccessStatusCode)
                    return new ApiResult<T> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = json };

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MobiCortex SDK] Error: {ex.Message}");
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }

        private async Task<ApiResult<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var response = await _http.DeleteAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    return new ApiResult<T> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = json };

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }

        private async Task<ApiResult<T>> DeleteAsync<T>(string endpoint, object body)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var jsonBody = JsonSerializer.Serialize(body, _json);
                using var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };
                var response = await _http.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ApiResult<T> { Success = false, Message = $"HTTP {(int)response.StatusCode}", RawResponse = json };

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }
        #endregion
    }
}






