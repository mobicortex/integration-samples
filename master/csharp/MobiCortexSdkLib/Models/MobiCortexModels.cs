using System.Text.Json;
using System.Text.Json.Serialization;

namespace MobiCortex.Sdk.Models

{
    /// <summary>
    /// Conversor JSON para campos booleanos que o backend retorna como int (1/0).
    /// </summary>
    public class BoolIntConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Aceita tanto booleano quanto inteiro na leitura
            if (reader.TokenType == JsonTokenType.True)
                return true;
            if (reader.TokenType == JsonTokenType.False)
                return false;
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt32() != 0;
            
            throw new JsonException($"Unexpected token type for boolean: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            // Escreve como booleano true/false
            writer.WriteBooleanValue(value);
        }
    }

    /// <summary>
    /// Conversor JSON para campos booleanos nullable que o backend retorna como int (1/0).
    /// </summary>
    public class BoolIntNullableConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Aceita tanto booleano quanto inteiro na leitura
            if (reader.TokenType == JsonTokenType.True)
                return true;
            if (reader.TokenType == JsonTokenType.False)
                return false;
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt32() != 0;
            if (reader.TokenType == JsonTokenType.Null)
                return null;
            
            throw new JsonException($"Unexpected token type for boolean: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            // Escreve como booleano true/false ou null
            if (value.HasValue)
                writer.WriteBooleanValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }

    // =============================================================================
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

    /// <summary>
    /// Token de API listado por GET /token.
    /// </summary>
    public class ApiTokenInfo
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("expires_at")]
        public string ExpiresAt { get; set; } = string.Empty;

        [JsonPropertyName("expired")]
        public int Expired { get; set; }
    }

    /// <summary>
    /// Resposta de listagem de tokens.
    /// </summary>
    public class ApiTokenListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("tokens")]
        public List<ApiTokenInfo> Tokens { get; set; } = new();
    }

    /// <summary>
    /// Body para criação de token de API.
    /// </summary>
    public class CreateApiTokenRequest
    {
        [JsonPropertyName("label")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Label { get; set; }

        [JsonPropertyName("expires_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExpiresAt { get; set; }

        [JsonPropertyName("ttl_days")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TtlDays { get; set; }
    }

    /// <summary>
    /// Resposta de criação de token.
    /// </summary>
    public class ApiTokenCreateResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("expires_at")]
        public string ExpiresAt { get; set; } = string.Empty;
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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>true = habilitado, false = desabilitado</summary>
        [JsonPropertyName("enabled")]
        [JsonConverter(typeof(BoolIntConverter))]
        public bool Enabled { get; set; } = true;

        /// <summary>Tipo de cadastro (uso livre pelo integrador)</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>Campos livres para dados adicionais</summary>
        [JsonPropertyName("field1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Field1 { get; set; }

        [JsonPropertyName("field2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Field2 { get; set; }

        [JsonPropertyName("field3")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Field3 { get; set; }

        [JsonPropertyName("field4")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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

        /// <summary>Motivo do match retornado pela busca cruzada (?search=).</summary>
        [JsonPropertyName("match_source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MatchSource { get; set; }

        /// <summary>Valor exato que causou o match retornado pela busca cruzada.</summary>
        [JsonPropertyName("match_value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MatchValue { get; set; }

        [JsonPropertyName("created_at")]
        public uint CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public uint UpdatedAt { get; set; }

        /// <summary>
        /// Alias local legado para a composição interna de vagas.
        /// O contrato público usa apenas slots1/slots2.
        /// </summary>
        [JsonIgnore]
        public int Vagas
        {
            get => (Slots2 << 4) | (Slots1 & 0x0F);
            set
            {
                Slots1 = value & 0x0F;
                Slots2 = (value >> 4) & 0x0F;
            }
        }

        // Helpers para exibição - Converte de UTC para horário de Brasília (UTC-3)
        [JsonIgnore]
        public string CriadoEm => CreatedAt > 0
            ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTimeOffset.FromUnixTimeSeconds(CreatedAt).UtcDateTime, 
                "E. South America Standard Time").ToString("dd/MM/yyyy HH:mm")
            : "-";

        [JsonIgnore]
        public string AtualizadoEm => UpdatedAt > 0
            ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTimeOffset.FromUnixTimeSeconds(UpdatedAt).UtcDateTime, 
                "E. South America Standard Time").ToString("dd/MM/yyyy HH:mm")
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

        [JsonPropertyName("central_registry_id")]
        public uint CentralRegistryId { get; set; }

        /// <summary>1=Pessoa, 2=Veículo, 3=Animal</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        /// <summary>true = habilitado, false = desabilitado</summary>
        [JsonPropertyName("enabled")]
        [JsonConverter(typeof(BoolIntConverter))]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Documento (CPF, placa, etc)</summary>
        [JsonPropertyName("doc")]
        public string Doc { get; set; } = string.Empty;

        /// <summary>Marca do veículo (opcional, usado quando tipo=2)</summary>
        [JsonPropertyName("brand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Brand { get; set; }

        /// <summary>Modelo do veículo (opcional, usado quando tipo=2)</summary>
        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Model { get; set; }

        /// <summary>Cor do veículo (opcional, usado quando tipo=2)</summary>
        [JsonPropertyName("color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Color { get; set; }

        /// <summary>Observações livres (opcional)</summary>
        [JsonPropertyName("obs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Obs { get; set; }

        /// <summary>
        /// true = LPR ativo. Quando ativado, o controlador cria automaticamente
        /// uma mídia do tipo LPR usando o campo "doc" como placa.
        /// </summary>
        [JsonPropertyName("lpr_enabled")]
        [JsonConverter(typeof(BoolIntConverter))]
        public bool LprEnabled { get; set; }

        [JsonPropertyName("created_at")]
        public uint CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public uint UpdatedAt { get; set; }

        // Helpers
        [JsonIgnore]
        public uint CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }

        [JsonIgnore]
        public int Tipo
        {
            get => Type;
            set => Type = value;
        }

        [JsonIgnore]
        public bool LprAtivo
        {
            get => LprEnabled;
            set => LprEnabled = value;
        }

        [JsonIgnore]
        public string TipoNome => (TipoEntidade)Type switch
        {
            TipoEntidade.Pessoa => "Pessoa",
            TipoEntidade.Veiculo => "Veículo",
            TipoEntidade.Animal => "Animal",
            _ => $"Tipo {Tipo}"
        };

        [JsonIgnore]
        public string NomeExibicao
        {
            get
            {
                if ((TipoEntidade)Type != TipoEntidade.Veiculo)
                {
                    return Name;
                }

                var partes = new[] { Brand, Model, Color }
                    .Where(valor => !string.IsNullOrWhiteSpace(valor))
                    .Select(valor => valor!.Trim());

                var descricao = string.Join(" / ", partes);
                if (!string.IsNullOrWhiteSpace(descricao))
                {
                    return descricao;
                }

                if (!string.IsNullOrWhiteSpace(Doc))
                {
                    return $"Veículo {Doc}";
                }

                return Name;
            }
        }

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
    /// POST /entities - Criar entidade.
    /// Use createid=true para gerar IDs automaticamente (modelo simples).
    /// </summary>
    public class CriarEntidadeRequest
    {
        /// <summary>
        /// ID da entidade.
        /// - 0: controlador gera automaticamente
        /// - &gt;0: usa o ID informado
        /// </summary>
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        /// <summary>
        /// ID do cadastro central ao qual vincular a entidade.
        /// Obrigatório no modelo MobiCortex. Ignorado se createid=true.
        /// </summary>
        [JsonPropertyName("central_registry_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public uint? CentralRegistryId { get; set; }

        /// <summary>1=Pessoa, 2=Veículo, 3=Animal</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; } = 1;

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Documento (CPF, placa, etc)</summary>
        [JsonPropertyName("doc")]
        public string Doc { get; set; } = string.Empty;

        /// <summary>Marca do veículo (opcional)</summary>
        [JsonPropertyName("brand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Brand { get; set; }

        /// <summary>Modelo do veículo (opcional)</summary>
        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Model { get; set; }

        /// <summary>Cor do veículo (opcional)</summary>
        [JsonPropertyName("color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Color { get; set; }

        /// <summary>Observações livres (opcional)</summary>
        [JsonPropertyName("obs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Obs { get; set; }

        /// <summary>true = ativar reconhecimento de placa via LPR</summary>
        [JsonPropertyName("lpr_enabled")]
        public bool LprEnabled { get; set; }

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

        [JsonIgnore]
        public uint? CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }

        [JsonIgnore]
        public int Tipo
        {
            get => Type;
            set => Type = value;
        }

        [JsonIgnore]
        public bool LprAtivo
        {
            get => LprEnabled;
            set => LprEnabled = value;
        }
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

        [JsonPropertyName("central_registry_id")]
        public uint CentralRegistryId { get; set; }

        /// <summary>1 se criou o cadastro central automaticamente</summary>
        [JsonPropertyName("created_central")]
        public int CreatedCentral { get; set; }

        [JsonIgnore]
        public uint CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }
    }

    /// <summary>
    /// PUT /entities?id=X - Atualizar entidade (parcial).
    /// 
    /// Modelo específico para atualizações. Diferente de CriarEntidadeRequest,
    /// este modelo permite atualização parcial: preencha apenas os campos que
    /// deseja modificar. Campos null são ignorados na serialização (não enviados).
    /// 
    /// Exemplo: para alterar só o nome, defina apenas Name = "Novo Nome",
    /// deixando os outros campos null.
    /// </summary>
    public class AtualizarEntidadeRequest
    {
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(BoolIntNullableConverter))]
        public bool? Enabled { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("doc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Doc { get; set; }

        [JsonPropertyName("brand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Brand { get; set; }

        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Model { get; set; }

        [JsonPropertyName("color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Color { get; set; }

        [JsonPropertyName("obs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Obs { get; set; }

        [JsonPropertyName("lpr_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(BoolIntNullableConverter))]
        public bool? LprEnabled { get; set; }

        [JsonIgnore]
        public bool? LprAtivo
        {
            get => LprEnabled;
            set => LprEnabled = value;
        }
    }

    /// <summary>
    /// GET /entities?central_registry_id=X  → lista entidades de um cadastro (sem paginação)
    /// GET /entities?offset=X&amp;count=Y  → lista global paginada
    /// GET /entities?offset=X&amp;count=Y&amp;name=filtro  → com filtro server-side
    /// </summary>
    public class EntidadeListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        /// <summary>Total de entidades no controlador (presente na listagem global paginada)</summary>
        [JsonPropertyName("total")]
        public uint Total { get; set; }

        [JsonPropertyName("count")]
        public uint Count { get; set; }

        /// <summary>Presente apenas quando filtrado por central_registry_id</summary>
        [JsonPropertyName("central_registry_id")]
        public uint? CentralRegistryId { get; set; }

        [JsonPropertyName("items")]
        public List<Entidade> Items { get; set; } = new();

        [JsonIgnore]
        public uint? CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }
    }

    /// <summary>
    /// GET /vehicle-drivers?vehicle_id=X
    /// </summary>
    public class VehicleDriverListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("vehicle_id")]
        public uint VehicleId { get; set; }

        [JsonPropertyName("driver_ids")]
        public List<uint> DriverIds { get; set; } = new();
    }

    /// <summary>
    /// PUT /vehicle-drivers?vehicle_id=X
    /// </summary>
    public class UpdateVehicleDriversRequest
    {
        [JsonPropertyName("driver_ids")]
        public List<uint> DriverIds { get; set; } = new();
    }

    /// <summary>
    /// Resposta da atualização de vínculos condutor-veículo.
    /// </summary>
    public class VehicleDriverUpdateResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("vehicle_id")]
        public uint VehicleId { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    #endregion

    #region Mídias de Acesso - GET/POST/PUT/DELETE /media

    /// <summary>
    /// Tipos de mídia de acesso suportados pelo controlador.
    /// 
    /// IMPORTANTE SOBRE LPR (PLACA):
    /// - Tipo 17 = LPR (reconhecimento de placa de veículo)
    /// - Ao criar uma mídia LPR manualmente via POST /media, é necessário enviar
    ///   os campos ns32_0 e ns32_1 para evitar que o backend tente validar a placa
    ///   como se fosse um formato RFID (Wiegand/CODE/HEX).
    /// - A forma RECOMENDADA de criar LPR é usando lpr_ativo=true no cadastro da entidade
    ///   (veículo), pois o backend converte automaticamente a placa em dados binários.
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
        public const int Lpr = 17;             // Placa (LPR) - veja nota acima
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

        [JsonPropertyName("central_registry_id")]
        public uint CentralRegistryId { get; set; }

        /// <summary>Tipo da mídia (ver constantes em TipoMidia)</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>true = habilitada, false = desabilitada</summary>
        [JsonPropertyName("enabled")]
        [JsonConverter(typeof(BoolIntConverter))]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("created_at")]
        public uint CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public uint UpdatedAt { get; set; }

        /// <summary>Data/hora de expiração (timestamp UNIX). 0 = sem expiração.</summary>
        [JsonPropertyName("expiration")]
        public uint Expiration { get; set; }

        [JsonIgnore]
        public uint DtBlock
        {
            get => Expiration;
            set => Expiration = value;
        }

        [JsonIgnore]
        public uint CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }

        [JsonIgnore]
        public int Tipo
        {
            get => Type;
            set => Type = value;
        }

        [JsonIgnore]
        public string Descricao
        {
            get => Description;
            set => Description = value;
        }

        [JsonIgnore]
        public string TipoNome => TipoMidia.GetNome(Type);

        [JsonIgnore]
        public bool TemBloqueioPorData => Expiration > 0 && Expiration > DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        [JsonIgnore]
        public string BloqueioAte => Expiration > 0
            ? DateTimeOffset.FromUnixTimeSeconds(Expiration).LocalDateTime.ToString("dd/MM/yyyy HH:mm")
            : "-";

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
    /// POST /media - Criar mídia de acesso
    /// 
    /// COMO CADASTRAR DIFERENTES TIPOS DE MÍDIA:
    /// 
    /// 1. RFID (Wiegand/CODE/HEX):
    ///    - Envie apenas entity_id, central_registry_id, type e description
    ///    - O backend detecta automaticamente o formato (Wiegand 123,45678 / CODE / HEX)
    ///    - Exemplo: descricao = "123,45678" ou "HEX: FF FF FF"
    /// 
    /// 2. LPR (Placa de veículo):
    ///    - Envie entity_id, central_registry_id, type=17, description="ABC1D23"
    ///    - IMPORTANTE: também envie ns32_0=0 e ns32_1=0
    ///    - Sem ns32_0/ns32_1, o backend tenta validar a placa como RFID e dá erro
    ///    - NOTA: A forma recomendada é usar lpr_ativo=true no cadastro da entidade (veículo)
    ///      pois o backend converte a placa automaticamente em dados binários
    /// 
    /// 3. Facial, Biometria, etc:
    ///    - Envie entity_id, central_registry_id, type e description
    ///    - Para evitar validação RFID, envie ns32_0=0 e ns32_1=0
    /// </summary>
    public class CriarMidiaRequest
    {
        [JsonPropertyName("entity_id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("central_registry_id")]
        public uint CentralRegistryId { get; set; }

        /// <summary>Tipo da mídia (ver constantes TipoMidia.*)</summary>
        [JsonPropertyName("type")]
        public int Type { get; set; } = TipoMidia.Wiegand26;

        /// <summary>
        /// Descrição/valor da mídia.
        /// - RFID: código do cartão (ex: "123,45678" ou "HEX: FF FF FF")
        /// - LPR: placa do veículo (ex: "ABC1D23")
        /// - Facial/Outros: identificador descritivo
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Valor numérico NS32_0 - usado internamente pelo backend para armazenar
        /// dados binários da mídia (RFID, placa convertida, etc).
        /// 
        /// QUANDO USAR:
        /// - RFID: não enviar (o backend calcula a partir da descricao)
        /// - LPR/Facial/Outros: enviar 0 para evitar validação RFID
        /// </summary>
        [JsonPropertyName("ns32_0")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public uint? Ns32_0 { get; set; }

        /// <summary>
        /// Valor numérico NS32_1 - usado internamente pelo backend para armazenar
        /// dados binários da mídia (parte alta de IDs longos).
        /// 
        /// QUANDO USAR:
        /// - RFID: não enviar (o backend calcula a partir da descricao)
        /// - LPR/Facial/Outros: enviar 0 para evitar validação RFID
        /// </summary>
        [JsonPropertyName("ns32_1")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public uint? Ns32_1 { get; set; }

        [JsonIgnore]
        public uint CadastroId
        {
            get => CentralRegistryId;
            set => CentralRegistryId = value;
        }

        [JsonIgnore]
        public int Tipo
        {
            get => Type;
            set => Type = value;
        }

        [JsonIgnore]
        public string Descricao
        {
            get => Description;
            set => Description = value;
        }
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
    /// Item de cor padrão retornado por GET /vehicle-catalogs.
    /// </summary>
    public class VehicleColorCatalogItem
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
    }

    /// <summary>
    /// Item de marca sugerida retornado por GET /vehicle-catalogs.
    /// </summary>
    public class VehicleBrandCatalogItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// GET /vehicle-catalogs - catálogos auxiliares para cadastro de veículos.
    /// </summary>
    public class VehicleCatalogsResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("colors")]
        public List<VehicleColorCatalogItem> Colors { get; set; } = new();

        [JsonPropertyName("brands")]
        public List<VehicleBrandCatalogItem> Brands { get; set; } = new();

        [JsonPropertyName("colors_count")]
        public int ColorsCount { get; set; }

        [JsonPropertyName("brands_count")]
        public int BrandsCount { get; set; }
    }

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

    /// <summary>
    /// Resposta de listagem de webhooks.
    /// </summary>
    public class WebhookListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("items")]
        public List<WebhookConfig> Items { get; set; } = new();
    }

    #endregion

    #region Video Source - GET/POST/PUT/DELETE /video-source

    /// <summary>
    /// Configuração de um canal de vídeo da controladora.
    /// </summary>
    public class VideoSourceConfig
    {
        [JsonPropertyName("ret")]
        public int? Ret { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("auto_detect")]
        public int AutoDetect { get; set; }

        [JsonPropertyName("channel_name")]
        public string ChannelName { get; set; } = string.Empty;

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("rtsp_port")]
        public int RtspPort { get; set; } = 554;

        [JsonPropertyName("uri_hd")]
        public string UriHd { get; set; } = string.Empty;

        [JsonPropertyName("uri_sd")]
        public string UriSd { get; set; } = string.Empty;

        [JsonPropertyName("detected_uri_hd")]
        public string DetectedUriHd { get; set; } = string.Empty;

        [JsonPropertyName("detected_uri_sd")]
        public string DetectedUriSd { get; set; } = string.Empty;

        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("mac_address")]
        public string MacAddress { get; set; } = string.Empty;

        [JsonPropertyName("cloud_id")]
        public string CloudId { get; set; } = string.Empty;

        [JsonPropertyName("lpr_enabled")]
        public int LprEnabled { get; set; }

        [JsonPropertyName("face_enabled")]
        public int FaceEnabled { get; set; }

        [JsonPropertyName("car_enabled")]
        public int CarEnabled { get; set; }

        [JsonPropertyName("animal_enabled")]
        public int AnimalEnabled { get; set; }

        [JsonPropertyName("face_rec_enabled")]
        public int FaceRecEnabled { get; set; }

        [JsonPropertyName("lpr_threshold")]
        public int LprThreshold { get; set; } = 70;

        [JsonPropertyName("face_threshold")]
        public int FaceThreshold { get; set; } = 70;

        [JsonPropertyName("car_threshold")]
        public int CarThreshold { get; set; } = 70;

        [JsonPropertyName("animal_threshold")]
        public int AnimalThreshold { get; set; } = 70;

        [JsonPropertyName("face_rec_threshold")]
        public int FaceRecThreshold { get; set; } = 70;

        [JsonIgnore]
        public bool Active => !string.IsNullOrWhiteSpace(Ip);
    }

    /// <summary>
    /// Resposta de listagem dos canais de vídeo.
    /// </summary>
    public class VideoSourceListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("max_channels")]
        public int MaxChannels { get; set; }

        [JsonPropertyName("items")]
        public List<VideoSourceConfig> Items { get; set; } = new();
    }

    #endregion
}
