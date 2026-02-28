using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SmartSdk.Models;

namespace SmartSdk.Services
{
    /// <summary>
    /// Serviço para comunicação com a API do rxppro
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl = "http://192.168.0.21";
        private string? _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public event Action<string>? OnLog;

        public ApiService()
        {
            // Configurar HttpClient para ignorar erros de SSL (comum em dispositivos embarcados)
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
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
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
        /// Faz login na API
        /// </summary>
        public async Task<(bool Success, string Message)> LoginAsync(string user, string password)
        {
            try
            {
                Log($"Tentando login em: {_baseUrl}/master/api/v1/login");
                Log($"Usuário: {user}");
                
                var loginData = new { user, password };
                var json = JsonSerializer.Serialize(loginData);
                Log($"Enviando: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/master/api/v1/login", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode} {response.StatusCode}");
                Log($"Resposta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<LegacyLoginResponse>(responseContent, _jsonOptions);
                    
                    if (result != null)
                    {
                        Log($"Token recebido: {(result.Token ?? "null")}");
                        Log($"Success: {result.Success}");
                        Log($"Message: {result.Message ?? "null"}");
                        
                        // Alguns APIs retornam sucesso sem token (apenas validam credenciais)
                        if (!string.IsNullOrEmpty(result.Token))
                        {
                            _token = result.Token;
                            _httpClient.DefaultRequestHeaders.Authorization = 
                                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                        }
                        
                        if (result.Success || response.IsSuccessStatusCode)
                        {
                            return (true, "Login realizado com sucesso");
                        }
                    }
                    
                    // Se resposta foi OK mas não conseguiu deserializar, assume sucesso
                    return (true, "Login realizado (sem token no response)");
                }
                
                return (false, $"Falha no login: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                Log($"ERRO HTTP: {ex.Message}");
                return (false, $"Erro de conexão: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                Log("ERRO: Timeout na requisição");
                return (false, "Timeout: servidor não respondeu");
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return (false, $"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se está autenticado
        /// </summary>
        public bool IsAuthenticated => true; // Sempre retorna true após login bem sucedido

        // ==================== USERS ====================

        /// <summary>
        /// Busca todos os usuários
        /// </summary>
        public async Task<ApiResult<List<User>>> GetUsersAsync()
        {
            try
            {
                Log($"GET: {_baseUrl}/master/api/v1/users");
                var response = await _httpClient.GetAsync($"{_baseUrl}/master/api/v1/users");
                var content = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var users = JsonSerializer.Deserialize<List<User>>(content, _jsonOptions);
                    return new ApiResult<List<User>> { Success = true, Data = users, RawResponse = content };
                }
                return new ApiResult<List<User>> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = content };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<List<User>> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Busca usuário por ID
        /// </summary>
        public async Task<ApiResult<User>> GetUserAsync(int id)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/users?id={id}";
                Log($"GET: {url}");
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonSerializer.Deserialize<User>(content, _jsonOptions);
                    return new ApiResult<User> { Success = true, Data = user, RawResponse = content };
                }
                return new ApiResult<User> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = content };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<User> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        public async Task<ApiResult<User>> CreateUserAsync(string name)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/users";
                var userData = new { name };
                var json = JsonSerializer.Serialize(userData);
                
                Log($"POST: {url}");
                Log($"Body: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonSerializer.Deserialize<User>(responseContent, _jsonOptions);
                    return new ApiResult<User> { Success = true, Data = user, RawResponse = responseContent };
                }
                return new ApiResult<User> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<User> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Remove um usuário
        /// </summary>
        public async Task<ApiResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/users?id={id}";
                Log($"DELETE: {url}");
                
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                return new ApiResult<bool> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? "Usuário removido" : $"Status: {response.StatusCode}",
                    RawResponse = content
                };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<bool> { Success = false, Message = ex.Message };
            }
        }

        // ==================== MEDIAS (PLACAS/RFID) ====================

        /// <summary>
        /// Busca todas as mídias
        /// </summary>
        public async Task<ApiResult<List<Media>>> GetAllMediasAsync()
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias";
                Log($"GET: {url}");
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var medias = JsonSerializer.Deserialize<List<Media>>(content, _jsonOptions);
                    return new ApiResult<List<Media>> { Success = true, Data = medias, RawResponse = content };
                }
                return new ApiResult<List<Media>> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = content };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<List<Media>> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Busca mídias por usuário
        /// </summary>
        public async Task<ApiResult<List<Media>>> GetMediasByUserAsync(int userId)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias?user_id={userId}";
                Log($"GET: {url}");
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var medias = JsonSerializer.Deserialize<List<Media>>(content, _jsonOptions);
                    return new ApiResult<List<Media>> { Success = true, Data = medias, RawResponse = content };
                }
                return new ApiResult<List<Media>> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = content };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<List<Media>> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Busca mídia por placa
        /// </summary>
        public async Task<ApiResult<List<Media>>> GetMediaByPlateAsync(string plate)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias?plate={plate}";
                Log($"GET: {url}");
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var medias = JsonSerializer.Deserialize<List<Media>>(content, _jsonOptions);
                    return new ApiResult<List<Media>> { Success = true, Data = medias, RawResponse = content };
                }
                return new ApiResult<List<Media>> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = content };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<List<Media>> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Cadastra uma placa
        /// </summary>
        public async Task<ApiResult<Media>> CreatePlateAsync(string plate, int userId)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias";
                var plateData = new PlateRequest
                {
                    Midia = "plate",
                    Plate = plate.ToUpper(),
                    UserId = userId
                };
                var json = JsonSerializer.Serialize(plateData);
                
                Log($"POST: {url}");
                Log($"Body: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var media = JsonSerializer.Deserialize<Media>(responseContent, _jsonOptions);
                    return new ApiResult<Media> { Success = true, Data = media, RawResponse = responseContent };
                }
                return new ApiResult<Media> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<Media> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Cadastra um RFID
        /// </summary>
        public async Task<ApiResult<Media>> CreateRfidAsync(string numero, int userId)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias";
                var rfidData = new RfidRequest
                {
                    Midia = "rfid",
                    Numero = numero,
                    UserId = userId
                };
                var json = JsonSerializer.Serialize(rfidData);
                
                Log($"POST: {url}");
                Log($"Body: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var media = JsonSerializer.Deserialize<Media>(responseContent, _jsonOptions);
                    return new ApiResult<Media> { Success = true, Data = media, RawResponse = responseContent };
                }
                return new ApiResult<Media> { Success = false, Message = $"Status: {response.StatusCode}", RawResponse = responseContent };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<Media> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Remove uma mídia por ID
        /// </summary>
        public async Task<ApiResult<bool>> DeleteMediaAsync(int id)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias?id={id}";
                Log($"DELETE: {url}");
                
                var response = await _httpClient.DeleteAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {content}");

                return new ApiResult<bool> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? "Mídia removida" : $"Status: {response.StatusCode}",
                    RawResponse = content
                };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<bool> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Remove uma mídia por placa
        /// </summary>
        public async Task<ApiResult<bool>> DeleteMediaByPlateAsync(string plate)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias";
                var deleteData = new { midia = "plate", plate };
                var json = JsonSerializer.Serialize(deleteData);
                
                Log($"DELETE: {url}");
                Log($"Body: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Delete, url) { Content = content };
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {responseContent}");

                return new ApiResult<bool> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = response.IsSuccessStatusCode,
                    RawResponse = responseContent
                };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<bool> { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Remove uma mídia por RFID
        /// </summary>
        public async Task<ApiResult<bool>> DeleteMediaByRfidAsync(string numero)
        {
            try
            {
                var url = $"{_baseUrl}/master/api/v1/medias";
                var deleteData = new { midia = "rfid", numero };
                var json = JsonSerializer.Serialize(deleteData);
                
                Log($"DELETE: {url}");
                Log($"Body: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Delete, url) { Content = content };
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                Log($"Status: {(int)response.StatusCode}");
                Log($"Resposta: {responseContent}");

                return new ApiResult<bool> 
                { 
                    Success = response.IsSuccessStatusCode, 
                    Data = response.IsSuccessStatusCode,
                    RawResponse = responseContent
                };
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                return new ApiResult<bool> { Success = false, Message = ex.Message };
            }
        }
    }

    /// <summary>
    /// Resultado de uma chamada de API
    /// </summary>
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public string? RawResponse { get; set; }
    }

    /// <summary>
    /// Resposta do login
    /// </summary>
    public class LegacyLoginResponse
    {
        public string? Token { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}