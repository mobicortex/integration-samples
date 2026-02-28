using System.Text.Json.Serialization;

namespace SmartSdk.Models
{
    #region Authentication Models

    public class LoginRequest
    {
        [JsonPropertyName("pass")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("session_key")]
        public string? SessionKey { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }
    }

    public class ChangePasswordRequest
    {
        [JsonPropertyName("pass_atual")]
        public string OldPassword { get; set; } = string.Empty;

        [JsonPropertyName("pass_nova")]
        public string NewPassword { get; set; } = string.Empty;

        [JsonPropertyName("pass_nova2")]
        public string NewPasswordConfirm { get; set; } = string.Empty;
    }

    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }

    #endregion

    #region Network Configuration Models

    public class NetworkInterface
    {
        [JsonPropertyName("dhcp")]
        public bool Dhcp { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("mask")]
        public string Mask { get; set; } = string.Empty;

        [JsonPropertyName("gateway")]
        public string Gateway { get; set; } = string.Empty;
    }

    public class WiFiConfig
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("ssid")]
        public string Ssid { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("dhcp")]
        public bool Dhcp { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("mask")]
        public string Mask { get; set; } = string.Empty;

        [JsonPropertyName("gateway")]
        public string Gateway { get; set; } = string.Empty;
    }

    public class ServerConfig
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public string Port { get; set; } = string.Empty;
    }

    public class NetworkConfig
    {
        [JsonPropertyName("ethernet")]
        public NetworkInterface Ethernet { get; set; } = new();

        [JsonPropertyName("wifi")]
        public WiFiConfig WiFi { get; set; } = new();

        [JsonPropertyName("server")]
        public ServerConfig Server { get; set; } = new();
    }

    public class UpdateNetworkRequest
    {
        [JsonPropertyName("ethernet")]
        public NetworkInterface Ethernet { get; set; } = new();

        [JsonPropertyName("wifi")]
        public WiFiConfig WiFi { get; set; } = new();

        [JsonPropertyName("server")]
        public ServerConfig Server { get; set; } = new();
    }

    #endregion

    #region Vehicle Models

    public enum TipoVeiculo
    {
        Morador,
        Visitante,
        Funcionario
    }

    public class Vehicle
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("placa")]
        public string Placa { get; set; } = string.Empty;

        [JsonPropertyName("tag_rfid")]
        public string TagRfid { get; set; } = string.Empty;

        [JsonPropertyName("proprietario")]
        public string Proprietario { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("data_cadastro")]
        public string DataCadastro { get; set; } = string.Empty;
    }

    public class CreateVehicleRequest
    {
        [JsonPropertyName("placa")]
        public string Placa { get; set; } = string.Empty;

        [JsonPropertyName("tag_rfid")]
        public string TagRfid { get; set; } = string.Empty;

        [JsonPropertyName("proprietario")]
        public string Proprietario { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;
    }

    #endregion

    #region Event Models

    public enum TipoEvento
    {
        TAG,
        PLACA,
        FACIAL
    }

    public class Event
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("valor")]
        public string Valor { get; set; } = string.Empty;

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;
    }

    public class CreateEventRequest
    {
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("valor")]
        public string Valor { get; set; } = string.Empty;

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }
    }

    #endregion

    #region Device Models

    public class Device
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("modelo")]
        public string Modelo { get; set; } = string.Empty;

        [JsonPropertyName("ip")]
        public string Ip { get; set; } = string.Empty;

        [JsonPropertyName("sinal")]
        public int Sinal { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("ultimo_visto")]
        public string UltimoVisto { get; set; } = string.Empty;
    }

    #endregion

    #region Log Models

    public class LogEntry
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("dado")]
        public string Dado { get; set; } = string.Empty;

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("ip_origem")]
        public string IpOrigem { get; set; } = string.Empty;
    }

    #endregion

    #region Central Registry Models

    public class CentralRegistryUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("slots1")]
        public int Slots1 { get; set; }

        [JsonPropertyName("slots2")]
        public int Slots2 { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("field1")]
        public string? Field1 { get; set; }

        [JsonPropertyName("field2")]
        public string? Field2 { get; set; }

        [JsonPropertyName("field3")]
        public string? Field3 { get; set; }

        [JsonPropertyName("field4")]
        public string? Field4 { get; set; }

        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public int UpdatedAt { get; set; }
    }

    public class CentralRegistryListResponse
    {
        [JsonPropertyName("ret")]
        public int Ret { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("items")]
        public List<CentralRegistryUser> Items { get; set; } = new();
    }

    public class CentralRegistryStats
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

    #region Master User/Entity Models

    public class MasterUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("id_pos_mem")]
        public int? IdPosMem { get; set; }

        [JsonPropertyName("controle")]
        public int? Controle { get; set; }

        [JsonPropertyName("habilitado")]
        public int? Habilitado { get; set; }

        [JsonPropertyName("vagas")]
        public int? Vagas { get; set; }

        [JsonPropertyName("tipo")]
        public int? Tipo { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("cliente_id")]
        public int? ClienteId { get; set; }

        [JsonPropertyName("mpid")]
        public int? Mpid { get; set; }

        [JsonPropertyName("mveicid")]
        public int? Mveicid { get; set; }

        [JsonPropertyName("owner_id")]
        public string? OwnerId { get; set; }

        [JsonPropertyName("cargo")]
        public string? Cargo { get; set; }

        [JsonPropertyName("depto")]
        public string? Depto { get; set; }

        [JsonPropertyName("divisao")]
        public string? Divisao { get; set; }

        [JsonPropertyName("localidade")]
        public string? Localidade { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("veic_marca")]
        public string? VeicMarca { get; set; }

        [JsonPropertyName("veic_modelo")]
        public string? VeicModelo { get; set; }

        [JsonPropertyName("veic_placa")]
        public string? VeicPlaca { get; set; }

        [JsonPropertyName("veic_cor")]
        public string? VeicCor { get; set; }

        [JsonPropertyName("dt_start")]
        public string? DtStart { get; set; }

        [JsonPropertyName("dt_end")]
        public string? DtEnd { get; set; }

        [JsonPropertyName("criado_em")]
        public string? CriadoEm { get; set; }

        [JsonPropertyName("dtcriacao")]
        public string? DtCriacao { get; set; }

        [JsonPropertyName("editado_em")]
        public string? EditadoEm { get; set; }

        [JsonPropertyName("dtedicao")]
        public string? DtEdicao { get; set; }
    }

    public class MasterMedia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("id_pos_mem")]
        public int? IdPosMem { get; set; }

        [JsonPropertyName("controle")]
        public int? Controle { get; set; }

        [JsonPropertyName("habilitado")]
        public int? Habilitado { get; set; }

        [JsonPropertyName("userid")]
        public int? UserId { get; set; }

        [JsonPropertyName("tipo")]
        public int? Tipo { get; set; }

        [JsonPropertyName("ns32_0")]
        public long? Ns320 { get; set; }

        [JsonPropertyName("ns32_1")]
        public long? Ns321 { get; set; }

        [JsonPropertyName("ns32_0c")]
        public long? Ns320c { get; set; }

        [JsonPropertyName("ns32_1c")]
        public long? Ns321c { get; set; }

        [JsonPropertyName("grupos")]
        public string? Grupos { get; set; }

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("clienteid")]
        public int? ClienteId { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("ownerid")]
        public string? OwnerId { get; set; }

        [JsonPropertyName("template_base64")]
        public string? TemplateBase64 { get; set; }

        [JsonPropertyName("photo_base64")]
        public string? PhotoBase64 { get; set; }

        [JsonPropertyName("criado_em")]
        public string? CriadoEm { get; set; }

        [JsonPropertyName("dtcriacao")]
        public string? DtCriacao { get; set; }

        [JsonPropertyName("editado_em")]
        public string? EditadoEm { get; set; }

        [JsonPropertyName("dtedicao")]
        public string? DtEdicao { get; set; }
    }

    #endregion
}
