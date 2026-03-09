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
    /// Cliente principal para integração com controladores MobiCortex.
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
        /// Cria uma nova instância do cliente MobiCortex.
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
                    return new ApiResult<bool> { Success = false, Message = "URL base não configurada. Chame ConfigureBaseUrl() primeiro." };

                // Tenta acessar o endpoint /login com POST vazio para testar conectividade
                // O endpoint /login aceita POST e retorna 400 apenas se o body estiver incorreto
                var url = $"{_baseUrl}/mbcortex/master/api/v1/login";
                
                // Faz POST com body vazio/inválido - esperamos 400 Bad Request se servidor responder
                // Isso é suficiente para confirmar que o servidor está online
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);
                
                var responseBody = await response.Content.ReadAsStringAsync();
                
                // Se recebeu qualquer resposta HTTP (mesmo 400), o servidor está respondendo
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
                return new ApiResult<bool> { Success = false, Message = $"Falha na conexão: {ex.Message}" };
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
            return await PostAsync<ApiRetResponse>("/central-registry", cadastro);
        }

        async Task<ApiResult<ApiRetResponse>> ICadastroService.AtualizarAsync(CadastroCentral cadastro)
        {
            // API usa POST para criar/atualizar (não PUT)
            return await PostAsync<ApiRetResponse>("/central-registry", cadastro);
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
            return await PostAsync<CriarEntidadeResponse>("/entities", request);
        }

        async Task<ApiResult<ApiRetResponse>> IEntidadeService.AtualizarAsync(uint entityId, AtualizarEntidadeRequest request)
        {
            // API usa PUT /entities?id=X para atualizar entidade parcialmente
            return await PutAsync<ApiRetResponse>($"/entities?id={entityId}", request);
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
            return await PostAsync<CriarMidiaResponse>("/media", request);
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

        async Task<ApiResult<NetworkCableConfig>> ISistemaService.ObterConfiguracaoRedeAsync()
        {
            return await GetAsync<NetworkCableConfig>("/network-config-cable");
        }

        async Task<ApiResult<ApiRetResponse>> ISistemaService.SalvarConfiguracaoRedeAsync(NetworkCableConfig config)
        {
            return await PostAsync<ApiRetResponse>("/network-config-cable", config);
        }

        async Task<ApiResult<NetworkWifiApConfig>> ISistemaService.ObterConfiguracaoWifiApAsync()
        {
            return await GetAsync<NetworkWifiApConfig>("/network-config-wifi-ap");
        }

        async Task<ApiResult<ApiRetResponse>> ISistemaService.SalvarConfiguracaoWifiApAsync(NetworkWifiApConfig config)
        {
            return await PostAsync<ApiRetResponse>("/network-config-wifi-ap", config);
        }

        async Task<ApiResult<NetworkWifiStationConfig>> ISistemaService.ObterConfiguracaoWifiStationAsync()
        {
            return await GetAsync<NetworkWifiStationConfig>("/network-config-wifi-st");
        }

        async Task<ApiResult<ApiRetResponse>> ISistemaService.SalvarConfiguracaoWifiStationAsync(NetworkWifiStationConfig config)
        {
            return await PostAsync<ApiRetResponse>("/network-config-wifi-st", config);
        }

        async Task<ApiResult<NetworkInterfacesResponse>> ISistemaService.ListarInterfacesRedeAsync()
        {
            return await GetAsync<NetworkInterfacesResponse>("/network-interfaces");
        }

        async Task<ApiResult<WifiScanResponse>> ISistemaService.EscanearRedesWifiAsync()
        {
            return await GetAsync<WifiScanResponse>("/network-wifi-scan");
        }

        async Task<ApiResult<WifiApClientsResponse>> ISistemaService.ListarClientesWifiApAsync()
        {
            return await GetAsync<WifiApClientsResponse>("/network-wifi-clients");
        }

        async Task<ApiResult<WifiSignalResponse>> ISistemaService.ObterSinalWifiAsync()
        {
            return await GetAsync<WifiSignalResponse>("/network-wifi-signal");
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

        #region Métodos HTTP Privados
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
