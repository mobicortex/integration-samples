using System.Text.Json.Serialization;

namespace SmartSdk.Models
{
    // =============================================================================
    //  MODELOS DE DADOS - API MobiCortex Master
    //  
    //  Todos os endpoints usam o prefixo: /mbcortex/master/api/v1/
    //  Exemplo: https://192.168.0.100:4449/mbcortex/master/api/v1/login
    //
    //  HIERARQUIA DE DADOS (Modelo MobiCortex):
    //  ┌─────────────────────────────────────────────────────────┐
    //  │ Cadastro Central (central-registry / USER6.MCU)        │
    //  │   └── Entidade (entities / ENTITY6.MCU)                │
    //  │         └── Mídia de Acesso (media / MEDIA6.MCU)       │
    //  └─────────────────────────────────────────────────────────┘
    //
    //  Um Cadastro pode ter várias Entidades (pessoas, veículos).
    //  Uma Entidade pode ter várias Mídias (cartão, biometria, placa, etc).
    // =============================================================================

    #region Resposta Genérica

    /// <summary>
    /// Resposta genérica da API com campo "ret" (0 = sucesso)
    /// </summary>
    public class ApiRetResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    /// <summary>
    /// Wrapper para resultado de chamadas à API
    /// </summary>
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public string? RawResponse { get; set; }
    }

    #endregion

    #region Autenticação - POST/PUT/DELETE /login

    /// <summary>
    /// POST /login - Login
    /// Body: { "pass": "senha" }
    /// </summary>
    public class LoginRequest
    {
        [JsonPropertyName("pass")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resposta do login
    /// { "ret": 0, "session_key": "abc123...", "expires_in": 900 }
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        /// <summary>Token de sessão (64 caracteres hex, SHA256)</summary>
        [JsonPropertyName("session_key")]
        public string? SessionKey { get; set; }

        /// <summary>Tempo de expiração em segundos (padrão: 900 = 15 min)</summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// PUT /login - Alterar senha
    /// </summary>
    public class ChangePasswordRequest
    {
        [JsonPropertyName("pass_atual")]
        public string SenhaAtual { get; set; } = string.Empty;

        [JsonPropertyName("pass_nova")]
        public string SenhaNova { get; set; } = string.Empty;

        [JsonPropertyName("pass_nova2")]
        public string SenhaNovaConfirm { get; set; } = string.Empty;
    }

    #endregion

    #region Cadastro Central - GET/POST/DELETE /central-registry

    /// <summary>
    /// Cadastro central (ex: "apartamento", "empresa", "morador").
    /// É o nó raiz da hierarquia: Cadastro → Entidades → Mídias
    /// </summary>
    public class CadastroCentral
    {
        /// <summary>
        /// ID do cadastro. Usa uint porque IDs criados via web
        /// começam em 4294000000 (acima de Int32.MaxValue).
        /// </summary>
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// true = habilitado, false = desabilitado.
        /// A API retorna JSON boolean (true/false), não inteiro.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>Tipo de cadastro (uso livre pelo integrador)</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>Vagas permitidas (para controle de estacionamento)</summary>
        [JsonPropertyName("vagas")]
        public int Vagas { get; set; }

        /// <summary>Campos livres para dados adicionais</summary>
        [JsonPropertyName("field1")]
        public string? Field1 { get; set; }

        [JsonPropertyName("field2")]
        public string? Field2 { get; set; }

        [JsonPropertyName("field3")]
        public string? Field3 { get; set; }

        [JsonPropertyName("field4")]
        public string? Field4 { get; set; }

        [JsonPropertyName("slots1")]
        public int Slots1 { get; set; }

        [JsonPropertyName("slots2")]
        public int Slots2 { get; set; }

        /// <summary>Quantidade de pessoas vinculadas (somente leitura)</summary>
        [JsonPropertyName("people_count")]
        public uint PeopleCount { get; set; }

        /// <summary>Quantidade de veículos vinculados (somente leitura)</summary>
        [JsonPropertyName("vehicle_count")]
        public uint VehicleCount { get; set; }

        [JsonPropertyName("created_at")]
        public uint CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public uint UpdatedAt { get; set; }

        // Helpers para exibição
        [JsonIgnore]
        public string CriadoEm => CreatedAt > 0
            ? DateTimeOffset.FromUnixTimeSeconds(CreatedAt).LocalDateTime.ToString("dd/MM/yyyy HH:mm")
            : "-";

        [JsonIgnore]
        public string AtualizadoEm => UpdatedAt > 0
            ? DateTimeOffset.FromUnixTimeSeconds(UpdatedAt).LocalDateTime.ToString("dd/MM/yyyy HH:mm")
            : "-";
    }

    /// <summary>
    /// GET /central-registry?offset=0&count=20&name=filtro
    /// Resposta paginada da listagem de cadastros
    /// </summary>
    public class CadastroListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        /// <summary>Quantidade de itens retornados nesta página</summary>
        [JsonPropertyName("count")]
        public uint Count { get; set; }

        /// <summary>Total de registros no controlador (para paginação)</summary>
        [JsonPropertyName("total")]
        public uint Total { get; set; }

        [JsonPropertyName("items")]
        public List<CadastroCentral> Items { get; set; } = new();
    }

    /// <summary>
    /// GET /central-registry/stats
    /// Estatísticas de uso do cadastro central
    /// </summary>
    public class CadastroStats
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("max_capacity")]
        public int MaxCapacity { get; set; }

        [JsonPropertyName("current_total")]
        public int CurrentTotal { get; set; }

        [JsonPropertyName("usage_percent")]
        public double UsagePercent { get; set; }
    }

    #endregion

    #region Entidades - GET/POST/PUT/DELETE /entities

    /// <summary>
    /// Tipos de entidade disponíveis
    /// </summary>
    public enum TipoEntidade
    {
        Pessoa = 1,
        Veiculo = 2,
        Animal = 3
    }

    /// <summary>
    /// Entidade (pessoa, veículo ou animal) vinculada a um Cadastro Central.
    /// Cada entidade pode ter várias mídias de acesso.
    /// </summary>
    public class Entidade
    {
        [JsonPropertyName("ret")]
        public int? Ret { get; set; }

        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("cadastro_id")]
        public uint CadastroId { get; set; }

        /// <summary>1=Pessoa, 2=Veículo, 3=Animal</summary>
        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        /// <summary>1=habilitado, 0=desabilitado</summary>
        [JsonPropertyName("habilitado")]
        public int Habilitado { get; set; } = 1;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Documento (CPF, placa, etc)</summary>
        [JsonPropertyName("doc")]
        public string Doc { get; set; } = string.Empty;

        /// <summary>
        /// 1 = LPR ativo. Quando ativado, o controlador cria automaticamente
        /// uma mídia do tipo LPR usando o campo "doc" como placa.
        /// </summary>
        [JsonPropertyName("lpr_ativo")]
        public int LprAtivo { get; set; }

        [JsonPropertyName("created_at")]
        public uint CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public uint UpdatedAt { get; set; }

        // Helpers
        [JsonIgnore]
        public string TipoNome => (TipoEntidade)Tipo switch
        {
            TipoEntidade.Pessoa => "Pessoa",
            TipoEntidade.Veiculo => "Veículo",
            TipoEntidade.Animal => "Animal",
            _ => $"Tipo {Tipo}"
        };

        [JsonIgnore]
        public string CriadoEm => CreatedAt > 0
            ? DateTimeOffset.FromUnixTimeSeconds(CreatedAt).LocalDateTime.ToString("dd/MM/yyyy HH:mm")
            : "-";
    }

    /// <summary>
    /// POST /entities - Criar entidade.
    /// Use createid=true para gerar IDs automaticamente (modelo simples).
    /// </summary>
    public class CriarEntidadeRequest
    {
        /// <summary>
        /// ID do cadastro central ao qual vincular a entidade.
        /// Obrigatório no modelo MobiCortex. Ignorado se createid=true.
        /// </summary>
        [JsonPropertyName("cadastro_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public uint? CadastroId { get; set; }

        /// <summary>1=Pessoa, 2=Veículo, 3=Animal</summary>
        [JsonPropertyName("tipo")]
        public int Tipo { get; set; } = 1;

        [JsonPropertyName("habilitado")]
        public int Habilitado { get; set; } = 1;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Documento (CPF, placa, etc)</summary>
        [JsonPropertyName("doc")]
        public string Doc { get; set; } = string.Empty;

        /// <summary>1 = ativar reconhecimento de placa via LPR</summary>
        [JsonPropertyName("lpr_ativo")]
        public int LprAtivo { get; set; }

        /// <summary>
        /// true = gera entity_id e cadastro_id automaticamente.
        /// Usado no modelo simples (sem criar cadastro central antes).
        /// O controlador cria tudo internamente.
        /// </summary>
        [JsonPropertyName("createid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CreateId { get; set; }

        /// <summary>true = sobrescreve se a entidade já existir</summary>
        [JsonPropertyName("overwrite")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Overwrite { get; set; }
    }

    /// <summary>
    /// Resposta de POST /entities
    /// </summary>
    public class CriarEntidadeResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("cadastro_id")]
        public uint CadastroId { get; set; }

        /// <summary>1 se criou o cadastro central automaticamente</summary>
        [JsonPropertyName("created_central")]
        public int CreatedCentral { get; set; }
    }

    /// <summary>
    /// PUT /entities?id=X - Atualizar entidade (parcial)
    /// Só envie os campos que deseja alterar.
    /// </summary>
    public class AtualizarEntidadeRequest
    {
        [JsonPropertyName("habilitado")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Habilitado { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("doc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Doc { get; set; }

        [JsonPropertyName("lpr_ativo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LprAtivo { get; set; }
    }

    /// <summary>
    /// GET /entities?cadastro_id=X - Lista entidades de um cadastro
    /// </summary>
    public class EntidadeListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("cadastro_id")]
        public uint CadastroId { get; set; }

        [JsonPropertyName("count")]
        public uint Count { get; set; }

        [JsonPropertyName("items")]
        public List<Entidade> Items { get; set; } = new();
    }

    #endregion

    #region Mídias de Acesso - GET/POST/PUT/DELETE /media

    /// <summary>
    /// Tipos de mídia de acesso suportados pelo controlador
    /// </summary>
    public static class TipoMidia
    {
        public const int ControleRemoto = 0;   // Controle remoto HT
        public const int Hcs = 1;              // HCS
        public const int Bio = 5;              // Biometria
        public const int Teclado = 8;          // Senha via teclado
        public const int Bio2 = 12;            // Biometria 2
        public const int Mc = 14;              // Controle MC
        public const int Bio3 = 15;            // Biometria 3
        public const int Lpr = 17;             // Placa (LPR)
        public const int BioHikvision = 18;    // Biometria Hikvision
        public const int BioNiceWego = 19;     // Biometria Nice Wego
        public const int Facial = 20;          // Reconhecimento facial
        public const int Wiegand26 = 21;       // Cartão RFID Wiegand 26 bits
        public const int Wiegand34 = 22;       // Cartão RFID Wiegand 34 bits

        /// <summary>Retorna o nome legível do tipo de mídia</summary>
        public static string GetNome(int tipo) => tipo switch
        {
            ControleRemoto => "Controle Remoto",
            Hcs => "HCS",
            Bio or Bio2 or Bio3 => "Biometria",
            Teclado => "Senha/Teclado",
            Mc => "Controle MC",
            Lpr => "Placa (LPR)",
            BioHikvision => "Bio Hikvision",
            BioNiceWego => "Bio Nice Wego",
            Facial => "Facial",
            Wiegand26 => "RFID Wiegand 26",
            Wiegand34 => "RFID Wiegand 34",
            _ => $"Tipo {tipo}"
        };
    }

    /// <summary>
    /// Mídia de acesso (cartão, biometria, placa, etc).
    /// Vinculada a uma Entidade e a um Cadastro Central.
    /// </summary>
    public class MidiaAcesso
    {
        [JsonPropertyName("ret")]
        public int? Ret { get; set; }

        [JsonPropertyName("media_id")]
        public uint MediaId { get; set; }

        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("cadastro_id")]
        public uint CadastroId { get; set; }

        /// <summary>Tipo da mídia (ver constantes em TipoMidia)</summary>
        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>1=habilitada, 0=desabilitada</summary>
        [JsonPropertyName("habilitado")]
        public int Habilitado { get; set; }

        [JsonIgnore]
        public string TipoNome => TipoMidia.GetNome(Tipo);
    }

    /// <summary>
    /// POST /media - Criar mídia de acesso
    /// </summary>
    public class CriarMidiaRequest
    {
        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("cadastro_id")]
        public uint CadastroId { get; set; }

        /// <summary>Tipo da mídia (ver TipoMidia.*)</summary>
        [JsonPropertyName("tipo")]
        public int Tipo { get; set; } = TipoMidia.Wiegand26;

        /// <summary>
        /// Descrição/valor da mídia.
        /// Para LPR: a placa (ex: "ABC1234")
        /// Para RFID: o código do cartão
        /// </summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resposta de POST /media
    /// </summary>
    public class CriarMidiaResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("media_id")]
        public uint MediaId { get; set; }
    }

    /// <summary>
    /// GET /media?entity_id=X - Lista mídias de uma entidade
    /// </summary>
    public class MidiaListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("items")]
        public List<MidiaAcesso> Items { get; set; } = new();
    }

    #endregion

    #region Dashboard e Dispositivo

    /// <summary>
    /// GET /dashboard - Estatísticas gerais do controlador
    /// </summary>
    public class DashboardStats
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("cadastros")]
        public int Cadastros { get; set; }

        [JsonPropertyName("pessoas")]
        public int Pessoas { get; set; }

        [JsonPropertyName("veiculos")]
        public int Veiculos { get; set; }

        [JsonPropertyName("total_midias")]
        public int TotalMidias { get; set; }

        [JsonPropertyName("facial")]
        public int Facial { get; set; }

        [JsonPropertyName("rfid")]
        public int Rfid { get; set; }

        [JsonPropertyName("lpr")]
        public int Lpr { get; set; }

        [JsonPropertyName("controle_remoto")]
        public int ControleRemoto { get; set; }
    }

    /// <summary>
    /// GET /device-info - Informações do hardware do controlador
    /// </summary>
    public class DeviceInfo
    {
        [JsonPropertyName("gid")]
        public int Gid { get; set; }

        [JsonPropertyName("gid_str")]
        public string GidStr { get; set; } = string.Empty;

        [JsonPropertyName("hw_model")]
        public string HwModel { get; set; } = string.Empty;

        [JsonPropertyName("fw_version")]
        public string FwVersion { get; set; } = string.Empty;

        [JsonPropertyName("uptime_str")]
        public string UptimeStr { get; set; } = string.Empty;

        [JsonPropertyName("cpu_temp_c")]
        public double CpuTempC { get; set; }

        [JsonPropertyName("cpu_load_1")]
        public double CpuLoad1 { get; set; }

        [JsonPropertyName("mem_used_pct")]
        public int MemUsedPct { get; set; }
    }

    #endregion

    #region Configuração de Rede - GET/POST /network-config-cable

    /// <summary>
    /// Configuração de rede ethernet (cabo)
    /// GET/POST /network-config-cable
    /// </summary>
    public class NetworkCableConfig
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        /// <summary>1=DHCP, 0=IP fixo</summary>
        [JsonPropertyName("dhcp")]
        public int Dhcp { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("mask")]
        public string Mask { get; set; } = string.Empty;

        [JsonPropertyName("gateway")]
        public string Gateway { get; set; } = string.Empty;

        [JsonPropertyName("dns1")]
        public string Dns1 { get; set; } = string.Empty;

        [JsonPropertyName("dns2")]
        public string Dns2 { get; set; } = string.Empty;
    }

    #endregion

    #region Webhook - GET/POST/DELETE /webhook

    /// <summary>
    /// Configuração de webhook (até 4 slots, id=1..4)
    /// O controlador envia um POST HTTP para a URL configurada quando ocorre um evento.
    /// </summary>
    public class WebhookConfig
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>1 = notificar acesso registrado</summary>
        [JsonPropertyName("registered")]
        public int Registered { get; set; }

        /// <summary>1 = notificar acesso não registrado</summary>
        [JsonPropertyName("unregistered")]
        public int Unregistered { get; set; }

        /// <summary>1 = notificar eventos de sensores</summary>
        [JsonPropertyName("sensors")]
        public int Sensors { get; set; }

        /// <summary>1 = notificar logs</summary>
        [JsonPropertyName("logs")]
        public int Logs { get; set; }
    }

    #endregion
}
