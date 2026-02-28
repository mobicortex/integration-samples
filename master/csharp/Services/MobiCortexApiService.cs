using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SmartSdk.Models;

namespace SmartSdk.Services
{
    /// <summary>
    /// Serviço para comunicação com a API MobiCortex Master
    /// Documentação completa em: docs/MobiCortex-Master-Endpoints.md
    /// </summary>
    public class MobiCortexApiService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl = "https://192.168.120.45";
        private string? _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public event Action<string>? OnLog;

        public MobiCortexApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                AllowAutoRedirect = true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        private void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var logMessage = $"[{timestamp}] {message}";
            OnLog?.Invoke(logMessage);
        }

        /// <summary>
        /// Configura a URL base da API
        /// </summary>
        public void ConfigureBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            Log($"URL configurada: {_baseUrl}");
        }

        /// <summary>
        /// Retorna a URL base configurada
        /// </summary>
        public string BaseUrl => _baseUrl;

        /// <summary>
        /// Verifica se está autenticado
        /// </summary>
        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        /// <summary>
        /// Retorna o token atual
        /// </summary>
        public string? Token => _token;

        // ==================== AUTENTICAÇÃO ====================

        /// <summary>
        /// POST /master/api/v1/login - Faz login na API
        /// Body: { "pass": "senha" }
        /// Response: { "ret": "OK", "session_key": "...", "expires_in": 900 }
        /// </summary>
        public async Task<ApiResult<LoginResponse>> LoginAsync(string password)
        {
            try
            {
                Log($"=== POST /master/api/v1/login ===");
                var request = new LoginRequest { Password = password };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/master/api/v1/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);
                    // A API retorna session_key (64 chars hex) em vez de token JWT
                    if (!string.IsNullOrEmpty(result?.SessionKey))
                    {
                        _token = result.SessionKey;
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                        Log("✅ SessionKey armazenada com sucesso");
                    }
                    // Também suporta o formato antigo com Token
                    else if (!string.IsNullOrEmpty(result?.Token))
                    {
                        _token = result.Token;
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                        Log("✅ Token armazenado com sucesso");
                    }
                    // ret=0 indica sucesso na API rxppro
                    return new ApiResult<LoginResponse> { 
                        Success = result?.Ret == 0 && !string.IsNullOrEmpty(result?.SessionKey), 
                        Data = result, 
                        RawResponse = responseContent 
                    };
                }

                return new ApiResult<LoginResponse> { Success = false, Message = $"Login falhou: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<LoginResponse> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// PUT /master/api/v1/login - Altera a senha do usuário
        /// Body: { "pass_atual": "...", "pass_nova": "...", "pass_nova2": "..." }
        /// </summary>
        public async Task<ApiResult<ApiResponse>> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            try
            {
                Log($"=== PUT /master/api/v1/login ===");
                var request = new ChangePasswordRequest { 
                    OldPassword = oldPassword, 
                    NewPassword = newPassword,
                    NewPasswordConfirm = newPassword
                };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/master/api/v1/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse>(responseContent, _jsonOptions);
                    return new ApiResult<ApiResponse> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<ApiResponse> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<ApiResponse> { Success = false, Message = ex.Message };
            }
        }

        // ==================== CONFIGURAÇÃO DE REDE ====================

        /// <summary>
        /// GET /api/network - Obtém configuração de rede
        /// </summary>
        public async Task<ApiResult<NetworkConfig>> GetNetworkConfigAsync()
        {
            try
            {
                Log($"=== GET /master/api/v1/network-config-cable ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/network-config-cable");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<NetworkConfig>(responseContent, _jsonOptions);
                    return new ApiResult<NetworkConfig> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<NetworkConfig> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<NetworkConfig> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// PUT /api/network - Atualiza configuração de rede
        /// </summary>
        public async Task<ApiResult<ApiResponse>> UpdateNetworkConfigAsync(NetworkInterface ethernet, WiFiConfig wifi, ServerConfig server)
        {
            try
            {
                Log($"=== PUT /master/api/v1/network-config-cable ===");
                var request = new UpdateNetworkRequest { Ethernet = ethernet, WiFi = wifi, Server = server };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/master/api/v1/network-config-cable", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse>(responseContent, _jsonOptions);
                    return new ApiResult<ApiResponse> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<ApiResponse> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<ApiResponse> { Success = false, Message = ex.Message };
            }
        }

        // ==================== VEÍCULOS ====================

        /// <summary>
        /// GET /api/veiculos - Lista todos os veículos
        /// </summary>
        public async Task<ApiResult<List<Vehicle>>> GetVehiclesAsync(string? filtro = null)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/vehicles";
                if (!string.IsNullOrEmpty(filtro))
                    url += $"?filtro={Uri.EscapeDataString(filtro)}";

                Log($"=== GET /master/api/v1/vehicles ===");
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<Vehicle>>(responseContent, _jsonOptions);
                    return new ApiResult<List<Vehicle>> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<List<Vehicle>> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<List<Vehicle>> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// GET /api/veiculos/{id} - Obtém um veículo específico
        /// </summary>
        public async Task<ApiResult<Vehicle>> GetVehicleAsync(long id)
        {
            try
            {
                Log($"=== GET /master/api/v1/vehicles/{id} ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/vehicles/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Vehicle>(responseContent, _jsonOptions);
                    return new ApiResult<Vehicle> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<Vehicle> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<Vehicle> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// POST /api/veiculos - Cria um novo veículo
        /// </summary>
        public async Task<ApiResult<Vehicle>> CreateVehicleAsync(string placa, string tagRfid, string proprietario, string tipo)
        {
            try
            {
                Log($"=== POST /master/api/v1/vehicles ===");
                var request = new CreateVehicleRequest 
                { 
                    Placa = placa, 
                    TagRfid = tagRfid, 
                    Proprietario = proprietario, 
                    Tipo = tipo 
                };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/master/api/v1/vehicles", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Vehicle>(responseContent, _jsonOptions);
                    return new ApiResult<Vehicle> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<Vehicle> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<Vehicle> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// PUT /api/veiculos/{id} - Atualiza um veículo
        /// </summary>
        public async Task<ApiResult<Vehicle>> UpdateVehicleAsync(long id, string placa, string tagRfid, string proprietario, string tipo)
        {
            try
            {
                Log($"=== PUT /master/api/v1/vehicles/{id} ===");
                var request = new CreateVehicleRequest 
                { 
                    Placa = placa, 
                    TagRfid = tagRfid, 
                    Proprietario = proprietario, 
                    Tipo = tipo 
                };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/master/api/v1/vehicles/{id}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Vehicle>(responseContent, _jsonOptions);
                    return new ApiResult<Vehicle> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<Vehicle> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<Vehicle> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// DELETE /api/veiculos/{id} - Remove um veículo
        /// </summary>
        public async Task<ApiResult<ApiResponse>> DeleteVehicleAsync(long id)
        {
            try
            {
                Log($"=== DELETE /master/api/v1/vehicles/{id} ===");
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/master/api/v1/vehicles/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<ApiResponse>(responseContent, _jsonOptions);
                    return new ApiResult<ApiResponse> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<ApiResponse> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<ApiResponse> { Success = false, Message = ex.Message };
            }
        }

        // ==================== EXPORTAÇÃO ====================

        /// <summary>
        /// GET /api/veiculos.xlsx - Exporta veículos em Excel
        /// </summary>
        public async Task<ApiResult<byte[]>> ExportVehiclesXlsxAsync()
        {
            try
            {
                Log($"=== GET /master/api/v1/vehicles.xlsx ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/vehicles.xlsx");
                var bytes = await response.Content.ReadAsByteArrayAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Tamanho: {bytes.Length} bytes");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult<byte[]> { Success = true, Data = bytes };
                }

                return new ApiResult<byte[]> { Success = false, Message = $"Erro: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<byte[]> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// GET /api/veiculos.pdf - Exporta veículos em PDF
        /// </summary>
        public async Task<ApiResult<byte[]>> ExportVehiclesPdfAsync()
        {
            try
            {
                Log($"=== GET /master/api/v1/vehicles.pdf ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/vehicles.pdf");
                var bytes = await response.Content.ReadAsByteArrayAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Tamanho: {bytes.Length} bytes");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult<byte[]> { Success = true, Data = bytes };
                }

                return new ApiResult<byte[]> { Success = false, Message = $"Erro: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<byte[]> { Success = false, Message = ex.Message };
            }
        }

        // ==================== EVENTOS ====================

        /// <summary>
        /// POST /api/events/new - Registra um novo evento
        /// </summary>
        public async Task<ApiResult<Event>> CreateEventAsync(string tipo, string valor, string? nome = null)
        {
            try
            {
                Log($"=== POST /master/api/v1/events/new ===");
                var request = new CreateEventRequest { Tipo = tipo, Valor = valor, Nome = nome };
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/master/api/v1/events/new", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Event>(responseContent, _jsonOptions);
                    return new ApiResult<Event> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<Event> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<Event> { Success = false, Message = ex.Message };
            }
        }

        // ==================== LOGS ====================

        /// <summary>
        /// GET /api/logs - Obtém logs de acesso
        /// </summary>
        public async Task<ApiResult<List<LogEntry>>> GetLogsAsync(string? filtro = null)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/logs";
                if (!string.IsNullOrEmpty(filtro))
                    url += $"?filtro={Uri.EscapeDataString(filtro)}";

                Log($"=== GET /master/api/v1/logs ===");
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<LogEntry>>(responseContent, _jsonOptions);
                    return new ApiResult<List<LogEntry>> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<List<LogEntry>> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<List<LogEntry>> { Success = false, Message = ex.Message };
            }
        }

        // ==================== PROXY MASTER ====================

        /// <summary>
        /// GET/POST /master/{path} - Proxy para aplicação pro.exe
        /// </summary>
        public async Task<ApiResult<string>> ProxyMasterAsync(string path, HttpMethod? method = null, object? body = null)
        {
            try
            {
                method ??= HttpMethod.Get;
                Log($"=== {method} /master/{path} ===");

                HttpRequestMessage request;
                if (body != null)
                {
                    var json = JsonSerializer.Serialize(body, _jsonOptions);
                    Log($"Request: {json}");
                    request = new HttpRequestMessage(method, $"{_baseUrl}/master/{path}")
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    request = new HttpRequestMessage(method, $"{_baseUrl}/master/{path}");
                }

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response length: {responseContent.Length} chars");

                return new ApiResult<string> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = responseContent, 
                    RawResponse = responseContent 
                };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<string> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// GET /cws/acesso/FOTO.MCUT - Obtém foto de acesso
        /// </summary>
        public async Task<ApiResult<byte[]>> GetFotoAcessoAsync(string fname)
        {
            try
            {
                Log($"=== GET /cws/acesso/FOTO.MCUT?fname={fname} ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/cws/acesso/FOTO.MCUT?fname={Uri.EscapeDataString(fname)}");
                var bytes = await response.Content.ReadAsByteArrayAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Tamanho: {bytes.Length} bytes");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult<byte[]> { Success = true, Data = bytes };
                }

                return new ApiResult<byte[]> { Success = false, Message = $"Erro: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<byte[]> { Success = false, Message = ex.Message };
            }
        }

        // ==================== CENTRAL REGISTRY ====================

        /// <summary>
        /// GET /master/api/v1/central-registry/stats - Obtém estatísticas do cadastro
        /// </summary>
        public async Task<ApiResult<CentralRegistryStats>> GetCentralRegistryStatsAsync()
        {
            try
            {
                Log($"=== GET /master/api/v1/central-registry/stats ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/central-registry/stats");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<CentralRegistryStats>(responseContent, _jsonOptions);
                    return new ApiResult<CentralRegistryStats> { Success = result?.Ret == 0, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<CentralRegistryStats> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<CentralRegistryStats> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// GET /master/api/v1/central-registry - Lista cadastros paginados
        /// </summary>
        public async Task<ApiResult<CentralRegistryListResponse>> GetCentralRegistryAsync(int offset = 0, int count = 10, string? name = null)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/central-registry?offset={offset}&count={count}";
                if (!string.IsNullOrEmpty(name))
                    url += $"&name={Uri.EscapeDataString(name)}";

                Log($"=== GET /master/api/v1/central-registry ===");
                Log($"Params: offset={offset}, count={count}, name={name}");
                
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<CentralRegistryListResponse>(responseContent, _jsonOptions);
                    return new ApiResult<CentralRegistryListResponse> { Success = result?.Ret == 0, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<CentralRegistryListResponse> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<CentralRegistryListResponse> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// GET /master/api/v1/central-registry?id={id} - Obtém um usuário específico
        /// </summary>
        public async Task<ApiResult<CentralRegistryUser>> GetCentralRegistryUserAsync(int id)
        {
            try
            {
                Log($"=== GET /master/api/v1/central-registry?id={id} ===");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/central-registry?id={id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<CentralRegistryUser>(responseContent, _jsonOptions);
                    return new ApiResult<CentralRegistryUser> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<CentralRegistryUser> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<CentralRegistryUser> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// POST /master/api/v1/central-registry - Cria ou atualiza usuário
        /// </summary>
        public async Task<ApiResult<CentralRegistryUser>> SaveCentralRegistryUserAsync(CentralRegistryUser user)
        {
            try
            {
                Log($"=== POST /master/api/v1/central-registry ===");
                var json = JsonSerializer.Serialize(user, _jsonOptions);
                Log($"Request: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/master/api/v1/central-registry", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<CentralRegistryUser>(responseContent, _jsonOptions);
                    return new ApiResult<CentralRegistryUser> { Success = true, Data = result, RawResponse = responseContent };
                }

                return new ApiResult<CentralRegistryUser> { Success = false, Message = $"Erro: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<CentralRegistryUser> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// DELETE /master/api/v1/central-registry?id={id} - Remove um usuário
        /// </summary>
        public async Task<ApiResult<bool>> DeleteCentralRegistryUserAsync(int id)
        {
            try
            {
                Log($"=== DELETE /master/api/v1/central-registry?id={id} ===");
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/master/api/v1/central-registry?id={id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Response: {responseContent}");

                return new ApiResult<bool> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = response.IsSuccessStatusCode,
                    RawResponse = responseContent 
                };
            }
            catch (Exception ex)
            {
                Log($"❌ ERRO: {ex.Message}");
                return new ApiResult<bool> { Success = false, Message = ex.Message };
            }
        }
    }

}
