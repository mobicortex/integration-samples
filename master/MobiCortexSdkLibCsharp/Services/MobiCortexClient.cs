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
    /// Main client for integration with MobiCortex controllers.
    /// </summary>
    public class MobiCortexClient : IMobiCortexClient, IRegistryService, IEntityService, IMediaService, ISystemService, IAccessService, IWebhookConfigService, IVideoSourceService
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
        public IRegistryService Registries => this;

        /// <inheritdoc/>
        public IEntityService Entities => this;

        /// <inheritdoc/>
        public IMediaService Media => this;

        /// <inheritdoc/>
        public ISystemService SystemInfo => this;

        /// <inheritdoc/>
        public IAccessService Access => this;

        /// <inheritdoc/>
        public IWebhookConfigService Webhooks => this;

        /// <inheritdoc/>
        public IVideoSourceService VideoSources => this;

        /// <summary>
        /// Creates a new instance of the MobiCortex client.
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
                // Check if the base URL has been configured
                if (string.IsNullOrEmpty(_baseUrl))
                    return new ApiResult<bool> { Success = false, Message = "Base URL not configured. Call ConfigureBaseUrl() first." };

                // Try to access the /login endpoint with an empty POST to test connectivity
                // The /login endpoint accepts POST and returns 400 only if the body is incorrect
                var url = $"{_baseUrl}/mbcortex/master/api/v1/login";

                // POST with empty/invalid body - we expect 400 Bad Request if the server responds
                // This is sufficient to confirm the server is online
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(url, content);

                var responseBody = await response.Content.ReadAsStringAsync();

                // If any HTTP response was received (even 400), the server is responding
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    response.IsSuccessStatusCode)
                {
                    return new ApiResult<bool> { Success = true, StatusCode = (int)response.StatusCode, Data = true, RawResponse = $"HTTP {(int)response.StatusCode}" };
                }

                return new ApiResult<bool> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", RawResponse = responseBody };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResult<bool> { Success = false, Message = $"Connection failure: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResult<bool> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        #region IRegistryService
        async Task<ApiResult<RegistryListResponse>> IRegistryService.ListAsync(int offset, int count, string? nameFilter, string? searchFilter)
        {
            var url = $"/central-registry?offset={offset}&count={count}";
            if (!string.IsNullOrEmpty(searchFilter))
                url += $"&search={Uri.EscapeDataString(searchFilter)}";
            else if (!string.IsNullOrEmpty(nameFilter))
                url += $"&name={Uri.EscapeDataString(nameFilter)}";
            return await GetAsync<RegistryListResponse>(url);
        }

        async Task<ApiResult<CentralRegistry>> IRegistryService.GetAsync(uint id)
        {
            return await GetAsync<CentralRegistry>($"/central-registry?id={id}");
        }

        async Task<ApiResult<ApiRetResponse>> IRegistryService.CreateAsync(CentralRegistry registry)
        {
            return await PostAsync<ApiRetResponse>("/central-registry", BuildCentralRegistryPayload(registry));
        }

        async Task<ApiResult<ApiRetResponse>> IRegistryService.UpdateAsync(CentralRegistry registry)
        {
            // API uses POST for create/update (not PUT)
            return await PostAsync<ApiRetResponse>("/central-registry", BuildCentralRegistryPayload(registry));
        }

        async Task<ApiResult<ApiRetResponse>> IRegistryService.DeleteAsync(uint id)
        {
            return await DeleteAsync<ApiRetResponse>($"/central-registry?id={id}");
        }

        async Task<ApiResult<RegistryStats>> IRegistryService.GetStatisticsAsync()
        {
            return await GetAsync<RegistryStats>("/central-registry/stats");
        }
        #endregion

        #region IEntityService
        async Task<ApiResult<EntityListResponse>> IEntityService.ListByRegistryAsync(uint centralRegistryId)
        {
            return await GetAsync<EntityListResponse>($"/entities?central_registry_id={centralRegistryId}");
        }

        async Task<ApiResult<EntityListResponse>> IEntityService.ListAllAsync(int offset, int count, string? name)
        {
            var url = $"/entities?offset={offset}&count={count}";
            if (!string.IsNullOrEmpty(name))
                url += $"&name={Uri.EscapeDataString(name)}";
            return await GetAsync<EntityListResponse>(url);
        }

        async Task<ApiResult<Entity>> IEntityService.GetAsync(uint entityId)
        {
            return await GetAsync<Entity>($"/entities?id={entityId}");
        }

        async Task<ApiResult<CreateEntityResponse>> IEntityService.CreateAsync(CreateEntityRequest request)
        {
            return await PostAsync<CreateEntityResponse>("/entities", BuildEntityCreatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IEntityService.UpdateAsync(uint entityId, UpdateEntityRequest request)
        {
            // API uses PUT /entities?id=X to partially update an entity
            return await PutAsync<ApiRetResponse>($"/entities?id={entityId}", BuildEntityUpdatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IEntityService.DeleteAsync(uint entityId)
        {
            return await DeleteAsync<ApiRetResponse>($"/entities?id={entityId}");
        }

        async Task<ApiResult<ApiRetResponse>> IEntityService.DeleteByPlateAsync(string plate)
        {
            return await DeleteAsync<ApiRetResponse>($"/entities?plate={Uri.EscapeDataString(plate)}");
        }

        async Task<ApiResult<CleanupOrphansResponse>> IEntityService.CleanOrphansAsync()
        {
            return await DeleteAsync<CleanupOrphansResponse>("/entities/cleanup-orphans");
        }

        async Task<ApiResult<VehicleDriverListResponse>> IEntityService.GetVehicleDriversAsync(uint vehicleId)
        {
            return await GetAsync<VehicleDriverListResponse>($"/vehicle-drivers?vehicle_id={vehicleId}");
        }

        async Task<ApiResult<VehicleDriverUpdateResponse>> IEntityService.UpdateVehicleDriversAsync(uint vehicleId, IEnumerable<uint> driverIds)
        {
            return await PutAsync<VehicleDriverUpdateResponse>($"/vehicle-drivers?vehicle_id={vehicleId}", new UpdateVehicleDriversRequest
            {
                DriverIds = driverIds?.Distinct().ToList() ?? new List<uint>()
            });
        }
        #endregion

        #region IMediaService
        async Task<ApiResult<MediaListResponse>> IMediaService.ListByEntityAsync(uint entityId)
        {
            return await GetAsync<MediaListResponse>($"/media?entity_id={entityId}");
        }

        async Task<ApiResult<AccessMedia>> IMediaService.GetAsync(uint mediaId)
        {
            return await GetAsync<AccessMedia>($"/media?id={mediaId}");
        }

        async Task<ApiResult<CreateMediaResponse>> IMediaService.CreateAsync(CreateMediaRequest request)
        {
            return await PostAsync<CreateMediaResponse>("/media", BuildMediaCreatePayload(request));
        }

        async Task<ApiResult<ApiRetResponse>> IMediaService.ChangeStatusAsync(uint mediaId, bool enabled)
        {
            return await PutAsync<ApiRetResponse>($"/media?id={mediaId}", new { enabled });
        }

        async Task<ApiResult<ApiRetResponse>> IMediaService.ChangeExpirationAsync(uint mediaId, uint expiration)
        {
            return await PutAsync<ApiRetResponse>($"/media?id={mediaId}", new { expiration });
        }

        async Task<ApiResult<ApiRetResponse>> IMediaService.DeleteAsync(uint mediaId)
        {
            return await DeleteAsync<ApiRetResponse>($"/media?id={mediaId}");
        }
        #endregion

        #region ISystemService
        async Task<ApiResult<DeviceInfo>> ISystemService.GetDeviceInfoAsync()
        {
            return await GetAsync<DeviceInfo>("/device-info");
        }

        async Task<ApiResult<DashboardStats>> ISystemService.GetDashboardAsync()
        {
            return await GetAsync<DashboardStats>("/dashboard");
        }

        async Task<ApiResult<VehicleCatalogsResponse>> ISystemService.GetVehicleCatalogsAsync()
        {
            return await GetAsync<VehicleCatalogsResponse>("/vehicle-catalogs");
        }

        #endregion

        #region IAccessService
        async Task<ApiResult<ApiRetResponse>> IAccessService.ChangePasswordAsync(ChangePasswordRequest request)
        {
            return await PutAsync<ApiRetResponse>("/login", request);
        }

        async Task<ApiResult<ApiTokenListResponse>> IAccessService.ListTokensAsync()
        {
            return await GetAsync<ApiTokenListResponse>("/token");
        }

        async Task<ApiResult<ApiTokenCreateResponse>> IAccessService.CreateTokenAsync(CreateApiTokenRequest request)
        {
            return await PostAsync<ApiTokenCreateResponse>("/token", request);
        }

        async Task<ApiResult<ApiRetResponse>> IAccessService.DeleteTokenAsync(string token)
        {
            return await DeleteAsync<ApiRetResponse>("/token", new { token });
        }
        #endregion

        #region IWebhookConfigService
        async Task<ApiResult<WebhookListResponse>> IWebhookConfigService.ListAsync()
        {
            return await GetAsync<WebhookListResponse>("/webhook");
        }

        async Task<ApiResult<WebhookConfig>> IWebhookConfigService.GetAsync(int id)
        {
            return await GetAsync<WebhookConfig>($"/webhook?id={id}");
        }

        async Task<ApiResult<WebhookConfig>> IWebhookConfigService.SaveAsync(int id, WebhookConfig config)
        {
            return await PostAsync<WebhookConfig>($"/webhook?id={id}", config);
        }

        async Task<ApiResult<ApiRetResponse>> IWebhookConfigService.DeleteAsync(int id)
        {
            return await DeleteAsync<ApiRetResponse>($"/webhook?id={id}");
        }

        async Task<ApiResult<ApiRetResponse>> IWebhookConfigService.TestAsync(int id)
        {
            return await GetAsync<ApiRetResponse>($"/webhook/test?id={id}");
        }
        #endregion

        #region IVideoSourceService
        async Task<ApiResult<VideoSourceListResponse>> IVideoSourceService.ListAsync()
        {
            return await GetAsync<VideoSourceListResponse>("/video-source");
        }

        async Task<ApiResult<VideoSourceConfig>> IVideoSourceService.GetAsync(int id)
        {
            return await GetAsync<VideoSourceConfig>($"/video-source?id={id}");
        }

        async Task<ApiResult<VideoSourceConfig>> IVideoSourceService.SaveAsync(int id, VideoSourceConfig config)
        {
            return await PostAsync<VideoSourceConfig>($"/video-source?id={id}", config);
        }

        async Task<ApiResult<ApiRetResponse>> IVideoSourceService.DeleteAsync(int id)
        {
            return await DeleteAsync<ApiRetResponse>($"/video-source?id={id}");
        }
        #endregion

        #region Private HTTP Methods
        private static object BuildCentralRegistryPayload(CentralRegistry registry)
        {
            var payload = new Dictionary<string, object?>
            {
                ["id"] = registry.Id,
                ["name"] = registry.Name ?? string.Empty,
                ["enabled"] = registry.Enabled,
                ["type"] = registry.Type,
                ["slots1"] = registry.Slots1,
                ["slots2"] = registry.Slots2
            };

            AddIfNotBlank(payload, "field1", registry.Field1);
            AddIfNotBlank(payload, "field2", registry.Field2);
            AddIfNotBlank(payload, "field3", registry.Field3);
            AddIfNotBlank(payload, "field4", registry.Field4);

            return payload;
        }

        private static object BuildEntityCreatePayload(CreateEntityRequest request)
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

        private static object BuildMediaCreatePayload(CreateMediaRequest request)
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


        private static object BuildEntityUpdatePayload(UpdateEntityRequest request)
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
                payload[key] = value!.Trim();
        }
        private async Task<ApiResult<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var url = $"{_baseUrl}{API}{endpoint}";
                var response = await _http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    T? errorData = default;
                    try { errorData = JsonSerializer.Deserialize<T>(json, _json); } catch { }
                    return new ApiResult<T> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", Data = errorData, RawResponse = json };
                }

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, StatusCode = (int)response.StatusCode, Data = data, RawResponse = json };
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
                {
                    T? errorData = default;
                    try { errorData = JsonSerializer.Deserialize<T>(json, _json); } catch { }
                    return new ApiResult<T> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", Data = errorData, RawResponse = json };
                }

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, StatusCode = (int)response.StatusCode, Data = data, RawResponse = json };
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
                {
                    T? errorData = default;
                    try { errorData = JsonSerializer.Deserialize<T>(json, _json); } catch { }
                    return new ApiResult<T> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", Data = errorData, RawResponse = json };
                }

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, StatusCode = (int)response.StatusCode, Data = data, RawResponse = json };
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
                {
                    T? errorData = default;
                    try { errorData = JsonSerializer.Deserialize<T>(json, _json); } catch { }
                    return new ApiResult<T> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", Data = errorData, RawResponse = json };
                }

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, StatusCode = (int)response.StatusCode, Data = data, RawResponse = json };
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
                {
                    T? errorData = default;
                    try { errorData = JsonSerializer.Deserialize<T>(json, _json); } catch { }
                    return new ApiResult<T> { Success = false, StatusCode = (int)response.StatusCode, Message = $"HTTP {(int)response.StatusCode}", Data = errorData, RawResponse = json };
                }

                var data = JsonSerializer.Deserialize<T>(json, _json);
                return new ApiResult<T> { Success = true, StatusCode = (int)response.StatusCode, Data = data, RawResponse = json };
            }
            catch (Exception ex)
            {
                return new ApiResult<T> { Success = false, Message = ex.Message };
            }
        }
        #endregion
    }
}
