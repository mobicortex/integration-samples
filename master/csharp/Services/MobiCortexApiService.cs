using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SmartSdk.Models;

namespace SmartSdk.Services
{
    // =============================================================================
    //  SERVIÇO DE API - MobiCortex Master
    //
    //  Este serviço encapsula todas as chamadas HTTP à API REST do controlador.
    //  Todas as rotas usam o prefixo: /mbcortex/master/api/v1/
    //
    //  FLUXO DE USO:
    //  1. Configurar a URL base (IP do controlador + porta 4449)
    //  2. Fazer login (POST /login) - recebe session_key
    //  3. Usar os métodos CRUD normalmente (o token é enviado automaticamente)
    //
    //  AUTENTICAÇÃO:
    //  O controlador usa session_key (SHA256 hex, 64 chars) enviado como
    //  header "Authorization: Bearer <session_key>".
    //  A sessão expira em 900 segundos (15 minutos) sem uso.
    //
    //  SSL:
    //  O controlador usa certificado SSL auto-assinado.
    //  Por isso, desabilitamos a validação de certificado no HttpClient.
    // =============================================================================

    public class MobiCortexApiService
    {
        // Prefixo de todas as rotas da API
        private const string API = "/mbcortex/master/api/v1";

        private readonly HttpClient _http;
        private string _baseUrl = "";
        private string? _sessionKey;

        private readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>Evento disparado para cada log de operação</summary>
        public event Action<string>? OnLog;

        public MobiCortexApiService()
        {
            // Desabilita validação de SSL (o controlador usa certificado auto-assinado)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                // Permite conexões com qualquer protocolo SSL/TLS
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls,
                // Desabilita verificação de revogação de certificado
                CheckCertificateRevocationList = false
            };
            _http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(15) };
        }

        /// <summary>URL base configurada (ex: https://192.168.0.100:4449)</summary>
        public string BaseUrl => _baseUrl;

        /// <summary>Verifica se está autenticado</summary>
        public bool IsAuthenticated => !string.IsNullOrEmpty(_sessionKey);

        /// <summary>Session key atual (para uso no WebSocket/MQTT)</summary>
        public string? SessionKey => _sessionKey;

        /// <summary>Configura a URL base do controlador</summary>
        public void ConfigureBaseUrl(string url)
        {
            _baseUrl = url.TrimEnd('/');
            Log($"URL base: {_baseUrl}");
        }

        /// <summary>
        /// Testa conectividade TCP básica antes de tentar HTTPS.
        /// Útil para diagnosticar problemas de firewall/redirecionamento de porta.
        /// </summary>
        public async Task<(bool Success, string Message)> TestTcpConnectionAsync()
        {
            if (string.IsNullOrEmpty(_baseUrl))
                return (false, "URL base não configurada");

            try
            {
                var uri = new Uri(_baseUrl);
                var host = uri.Host;
                var port = uri.Port;

                Log($"Testando conexão TCP com {host}:{port}...");

                using var tcp = new System.Net.Sockets.TcpClient();
                var connectTask = tcp.ConnectAsync(host, port);
                var timeoutTask = Task.Delay(5000);

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    var msg = $"Timeout: Não foi possível conectar a {host}:{port} em 5 segundos";
                    Log($"  ERRO: {msg}");
                    return (false, msg + "\n\nPossíveis causas:\n- IP/porta incorretos\n- Firewall bloqueando\n- Controlador offline");
                }

                await connectTask; // Propaga exceção se houver
                Log($"  ✓ Conexão TCP OK");
                return (true, $"Conexão TCP com {host}:{port} bem-sucedida");
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                var msg = GetSocketErrorMessage(ex);
                Log($"  ERRO: {msg}");
                return (false, msg);
            }
            catch (Exception ex)
            {
                Log($"  ERRO: {ex.Message}");
                return (false, ex.Message);
            }
        }

        /// <summary>Traduz códigos de erro de socket para mensagens em português</summary>
        private static string GetSocketErrorMessage(System.Net.Sockets.SocketException ex)
        {
            return ex.SocketErrorCode switch
            {
                System.Net.Sockets.SocketError.AccessDenied =>
                    "Acesso negado pelo firewall ou antivírus.\n\nSoluções:\n" +
                    "1. Execute o programa como Administrador\n" +
                    "2. Adicione uma exceção no Windows Defender/Firewall\n" +
                    "3. Desative temporariamente o antivírus para testar",
                System.Net.Sockets.SocketError.ConnectionRefused =>
                    "Conexão recusada - o controlador não está aceitando conexões nesta porta",
                System.Net.Sockets.SocketError.TimedOut =>
                    "Tempo esgotado - o controlador não respondeu",
                System.Net.Sockets.SocketError.HostUnreachable =>
                    "Host inalcançável - verifique o IP e a rota de rede",
                System.Net.Sockets.SocketError.NetworkUnreachable =>
                    "Rede inalcançável - verifique sua conexão de rede",
                _ => $"Erro de socket ({ex.SocketErrorCode}): {ex.Message}"
            };
        }

        // =====================================================================
        //  HELPERS HTTP (GET, POST, PUT, DELETE)
        //  Todos os métodos adicionam o prefixo da API automaticamente.
        // =====================================================================

        private void Log(string msg) => OnLog?.Invoke(msg);

        /// <summary>Monta a URL completa: baseUrl + /mbcortex/master/api/v1 + endpoint</summary>
        private string Url(string endpoint) => $"{_baseUrl}{API}{endpoint}";

        /// <summary>GET genérico com deserialização</summary>
        private async Task<ApiResult<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                Log($"GET {API}{endpoint}");
                var resp = await _http.GetAsync(Url(endpoint));
                var body = await resp.Content.ReadAsStringAsync();
                Log($"  → {(int)resp.StatusCode} | {Truncate(body)}");

                if (!resp.IsSuccessStatusCode)
                    return Fail<T>($"HTTP {(int)resp.StatusCode}", body);

                var data = JsonSerializer.Deserialize<T>(body, _json);
                return Ok(data, body);
            }
            catch (Exception ex)
            {
                Log($"  ERRO: {ex.Message}");
                return Fail<T>(ex.Message);
            }
        }

        /// <summary>POST genérico com body JSON</summary>
        private async Task<ApiResult<T>> PostAsync<T>(string endpoint, object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body, _json);
                Log($"POST {API}{endpoint} → {Truncate(json)}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _http.PostAsync(Url(endpoint), content);
                var respBody = await resp.Content.ReadAsStringAsync();
                Log($"  → {(int)resp.StatusCode} | {Truncate(respBody)}");

                if (!resp.IsSuccessStatusCode)
                    return Fail<T>($"HTTP {(int)resp.StatusCode}", respBody);

                var data = JsonSerializer.Deserialize<T>(respBody, _json);
                return Ok(data, respBody);
            }
            catch (Exception ex)
            {
                Log($"  ERRO: {ex.Message}");
                return Fail<T>(ex.Message);
            }
        }

        /// <summary>PUT genérico com body JSON</summary>
        private async Task<ApiResult<T>> PutAsync<T>(string endpoint, object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body, _json);
                Log($"PUT {API}{endpoint} → {Truncate(json)}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _http.PutAsync(Url(endpoint), content);
                var respBody = await resp.Content.ReadAsStringAsync();
                Log($"  → {(int)resp.StatusCode} | {Truncate(respBody)}");

                if (!resp.IsSuccessStatusCode)
                    return Fail<T>($"HTTP {(int)resp.StatusCode}", respBody);

                var data = JsonSerializer.Deserialize<T>(respBody, _json);
                return Ok(data, respBody);
            }
            catch (Exception ex)
            {
                Log($"  ERRO: {ex.Message}");
                return Fail<T>(ex.Message);
            }
        }

        /// <summary>DELETE genérico</summary>
        private async Task<ApiResult<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                Log($"DELETE {API}{endpoint}");
                var resp = await _http.DeleteAsync(Url(endpoint));
                var body = await resp.Content.ReadAsStringAsync();
                Log($"  → {(int)resp.StatusCode} | {Truncate(body)}");

                if (!resp.IsSuccessStatusCode)
                    return Fail<T>($"HTTP {(int)resp.StatusCode}", body);

                var data = JsonSerializer.Deserialize<T>(body, _json);
                return Ok(data, body);
            }
            catch (Exception ex)
            {
                Log($"  ERRO: {ex.Message}");
                return Fail<T>(ex.Message);
            }
        }

        private static ApiResult<T> Ok<T>(T? data, string raw) =>
            new() { Success = true, Data = data, Message = ExtractBodyMessage(raw), RawResponse = raw };

        private static ApiResult<T> Fail<T>(string msg, string? raw = null) =>
            new() { Success = false, Message = BuildErrorMessage(msg, raw), RawResponse = raw };

        /// <summary>
        /// Extrai e retorna apenas o campo de mensagem do body JSON, se existir.
        /// Útil para capturar mensagens de erro retornadas com HTTP 200.
        /// </summary>
        private static string? ExtractBodyMessage(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;
                foreach (var field in new[] { "message", "msg", "error", "detail" })
                    if (root.TryGetProperty(field, out var prop) &&
                        prop.ValueKind == JsonValueKind.String)
                    {
                        var v = prop.GetString();
                        if (!string.IsNullOrEmpty(v)) return v;
                    }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Monta mensagem de erro detalhada combinando: código HTTP + descrição + mensagem do body JSON.
        /// </summary>
        private static string BuildErrorMessage(string httpCode, string? raw)
        {
            // Descrições amigáveis para os códigos HTTP mais comuns
            var description = httpCode switch
            {
                "HTTP 400" => "Requisição inválida",
                "HTTP 401" => "Não autorizado — sessão expirada ou senha incorreta",
                "HTTP 403" => "Acesso negado",
                "HTTP 404" => "Recurso não encontrado",
                "HTTP 409" => "Conflito — registro já existe ou viola unicidade",
                "HTTP 422" => "Dados inválidos ou incompletos",
                "HTTP 429" => "Muitas requisições — aguarde e tente novamente",
                "HTTP 500" => "Erro interno do servidor",
                "HTTP 503" => "Serviço indisponível",
                _ => ""
            };

            var bodyMsg = ExtractBodyMessage(raw) ?? "";

            var parts = new List<string> { httpCode };
            if (!string.IsNullOrEmpty(description)) parts.Add(description);
            if (!string.IsNullOrEmpty(bodyMsg))      parts.Add(bodyMsg);
            return string.Join(" — ", parts);
        }

        private static string Truncate(string s, int max = 200) =>
            s.Length <= max ? s : s[..max] + "...";

        // =====================================================================
        //  AUTENTICAÇÃO
        //  POST /login   → fazer login (retorna session_key)
        //  PUT  /login   → alterar senha
        //  DELETE /login → logout
        // =====================================================================

        /// <summary>
        /// Faz login no controlador.
        /// A senha padrão de fábrica é "admin".
        /// </summary>
        public async Task<ApiResult<LoginResponse>> LoginAsync(string password)
        {
            var result = await PostAsync<LoginResponse>("/login", new LoginRequest { Password = password });

            // Se login OK, armazena o session_key para usar nas próximas chamadas
            if (result.Success && result.Data?.Ret == 0 && !string.IsNullOrEmpty(result.Data.SessionKey))
            {
                _sessionKey = result.Data.SessionKey;
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sessionKey);
                Log($"Autenticado! Session expira em {result.Data.ExpiresIn}s");
            }

            return result;
        }

        /// <summary>Faz logout (invalida a sessão)</summary>
        public async Task<ApiResult<ApiRetResponse>> LogoutAsync()
        {
            var result = await DeleteAsync<ApiRetResponse>("/login");
            _sessionKey = null;
            _http.DefaultRequestHeaders.Authorization = null;
            return result;
        }

        /// <summary>Altera a senha do controlador</summary>
        public async Task<ApiResult<ApiRetResponse>> ChangePasswordAsync(string senhaAtual, string senhaNova)
        {
            return await PutAsync<ApiRetResponse>("/login", new ChangePasswordRequest
            {
                SenhaAtual = senhaAtual,
                SenhaNova = senhaNova,
                SenhaNovaConfirm = senhaNova
            });
        }

        // =====================================================================
        //  CADASTRO CENTRAL (Central Registry)
        //  GET    /central-registry?offset=0&count=20&name=filtro
        //  GET    /central-registry?id=123
        //  POST   /central-registry  (body: CadastroCentral)
        //  DELETE /central-registry?id=123
        //  GET    /central-registry/stats
        // =====================================================================

        /// <summary>Lista cadastros (paginado, com filtro opcional por nome)</summary>
        public async Task<ApiResult<CadastroListResponse>> ListarCadastrosAsync(
            int offset = 0, int count = 20, string? filtroNome = null)
        {
            var query = $"/central-registry?offset={offset}&count={count}";
            if (!string.IsNullOrWhiteSpace(filtroNome))
                query += $"&name={Uri.EscapeDataString(filtroNome)}";
            return await GetAsync<CadastroListResponse>(query);
        }

        /// <summary>Busca um cadastro pelo ID</summary>
        public async Task<ApiResult<CadastroCentral>> ObterCadastroAsync(uint id)
        {
            return await GetAsync<CadastroCentral>($"/central-registry?id={id}");
        }

        /// <summary>Cria ou atualiza um cadastro central (upsert pelo id)</summary>
        public async Task<ApiResult<ApiRetResponse>> SalvarCadastroAsync(CadastroCentral cadastro)
        {
            return await PostAsync<ApiRetResponse>("/central-registry", cadastro);
        }

        /// <summary>Remove um cadastro central e todas as entidades/mídias vinculadas</summary>
        public async Task<ApiResult<ApiRetResponse>> ExcluirCadastroAsync(uint id)
        {
            return await DeleteAsync<ApiRetResponse>($"/central-registry?id={id}");
        }

        /// <summary>Obtém estatísticas (capacidade, uso, percentual)</summary>
        public async Task<ApiResult<CadastroStats>> ObterEstatisticasAsync()
        {
            return await GetAsync<CadastroStats>("/central-registry/stats");
        }

        // =====================================================================
        //  ENTIDADES (Entities)
        //  GET    /entities?id=123           → busca por entity_id
        //  GET    /entities?cadastro_id=456  → lista entidades do cadastro
        //  POST   /entities                  → cria entidade
        //  PUT    /entities?id=123           → atualiza parcialmente
        //  DELETE /entities?id=123           → exclui (cascade: remove mídias)
        // =====================================================================

        /// <summary>Busca uma entidade pelo entity_id</summary>
        public async Task<ApiResult<Entidade>> ObterEntidadeAsync(uint entityId)
        {
            return await GetAsync<Entidade>($"/entities?id={entityId}");
        }

        /// <summary>Lista entidades de um cadastro (retorna todas, sem paginação)</summary>
        public async Task<ApiResult<EntidadeListResponse>> ListarEntidadesAsync(uint cadastroId)
        {
            return await GetAsync<EntidadeListResponse>($"/entities?cadastro_id={cadastroId}");
        }

        /// <summary>
        /// Lista entidades com paginação global e filtros server-side.
        /// GET /entities?offset=X&amp;count=Y[&amp;name=filtro][&amp;doc=filtro][&amp;tipo=N]
        /// </summary>
        public async Task<ApiResult<EntidadeListResponse>> ListarEntidadesGlobalAsync(
            int offset = 0, int count = 10,
            string? nome = null, string? doc = null, int? tipo = null)
        {
            var q = $"/entities?offset={offset}&count={count}";
            if (!string.IsNullOrEmpty(nome)) q += $"&name={Uri.EscapeDataString(nome)}";
            if (!string.IsNullOrEmpty(doc))  q += $"&doc={Uri.EscapeDataString(doc)}";
            if (tipo.HasValue)               q += $"&tipo={tipo}";
            return await GetAsync<EntidadeListResponse>(q);
        }

        /// <summary>
        /// Cria uma entidade.
        /// - Modelo MobiCortex: informe cadastro_id (cadastro central deve existir)
        /// - Modelo Simples: use createid=true (gera IDs automaticamente)
        /// </summary>
        public async Task<ApiResult<CriarEntidadeResponse>> CriarEntidadeAsync(CriarEntidadeRequest request)
        {
            return await PostAsync<CriarEntidadeResponse>("/entities", request);
        }

        /// <summary>Atualiza uma entidade parcialmente (só os campos informados)</summary>
        public async Task<ApiResult<ApiRetResponse>> AtualizarEntidadeAsync(uint entityId, AtualizarEntidadeRequest request)
        {
            return await PutAsync<ApiRetResponse>($"/entities?id={entityId}", request);
        }

        /// <summary>Exclui uma entidade e todas as suas mídias vinculadas</summary>
        public async Task<ApiResult<ApiRetResponse>> ExcluirEntidadeAsync(uint entityId)
        {
            return await DeleteAsync<ApiRetResponse>($"/entities?id={entityId}");
        }

        // =====================================================================
        //  MÍDIAS DE ACESSO (Media)
        //  GET    /media?id=123        → busca mídia por media_id
        //  GET    /media?entity_id=456 → lista mídias da entidade
        //  POST   /media               → cria mídia
        //  PUT    /media?id=123        → atualiza mídia
        //  DELETE /media?id=123        → remove mídia
        // =====================================================================

        /// <summary>Lista mídias de uma entidade</summary>
        public async Task<ApiResult<MidiaListResponse>> ListarMidiasAsync(uint entityId)
        {
            return await GetAsync<MidiaListResponse>($"/media?entity_id={entityId}");
        }

        /// <summary>Busca uma mídia pelo media_id</summary>
        public async Task<ApiResult<MidiaAcesso>> ObterMidiaAsync(uint mediaId)
        {
            return await GetAsync<MidiaAcesso>($"/media?id={mediaId}");
        }

        /// <summary>Cria uma nova mídia de acesso vinculada a uma entidade</summary>
        public async Task<ApiResult<CriarMidiaResponse>> CriarMidiaAsync(CriarMidiaRequest request)
        {
            return await PostAsync<CriarMidiaResponse>("/media", request);
        }

        /// <summary>Remove uma mídia de acesso</summary>
        public async Task<ApiResult<ApiRetResponse>> ExcluirMidiaAsync(uint mediaId)
        {
            return await DeleteAsync<ApiRetResponse>($"/media?id={mediaId}");
        }

        // =====================================================================
        //  DASHBOARD E DISPOSITIVO
        //  GET /dashboard   → estatísticas gerais
        //  GET /device-info → informações do hardware
        // =====================================================================

        /// <summary>Obtém estatísticas gerais (cadastros, pessoas, veículos, mídias)</summary>
        public async Task<ApiResult<DashboardStats>> ObterDashboardAsync()
        {
            return await GetAsync<DashboardStats>("/dashboard");
        }

        /// <summary>Obtém informações do hardware (modelo, firmware, CPU, memória)</summary>
        public async Task<ApiResult<DeviceInfo>> ObterDeviceInfoAsync()
        {
            return await GetAsync<DeviceInfo>("/device-info");
        }

        // =====================================================================
        //  REDE
        //  GET  /network-config-cable → configuração de rede (cabo)
        //  POST /network-config-cable → salvar configuração
        // =====================================================================

        /// <summary>Obtém configuração de rede (ethernet cabo)</summary>
        public async Task<ApiResult<NetworkCableConfig>> ObterRedeAsync()
        {
            return await GetAsync<NetworkCableConfig>("/network-config-cable");
        }

        /// <summary>Salva configuração de rede (ethernet cabo)</summary>
        public async Task<ApiResult<ApiRetResponse>> SalvarRedeAsync(NetworkCableConfig config)
        {
            return await PostAsync<ApiRetResponse>("/network-config-cable", config);
        }

        // =====================================================================
        //  WEBHOOKS
        //  GET    /webhook?id=1   → busca webhook (id=1..4)
        //  POST   /webhook?id=1   → salva webhook
        //  DELETE /webhook?id=1   → remove webhook
        // =====================================================================

        /// <summary>Obtém configuração de um webhook (id=1..4)</summary>
        public async Task<ApiResult<WebhookConfig>> ObterWebhookAsync(int id)
        {
            return await GetAsync<WebhookConfig>($"/webhook?id={id}");
        }

        /// <summary>Salva configuração de um webhook</summary>
        public async Task<ApiResult<ApiRetResponse>> SalvarWebhookAsync(int id, WebhookConfig config)
        {
            return await PostAsync<ApiRetResponse>($"/webhook?id={id}", config);
        }

        /// <summary>Remove um webhook</summary>
        public async Task<ApiResult<ApiRetResponse>> ExcluirWebhookAsync(int id)
        {
            return await DeleteAsync<ApiRetResponse>($"/webhook?id={id}");
        }
    }
}
