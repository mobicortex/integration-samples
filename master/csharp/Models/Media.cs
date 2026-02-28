using System.Text.Json.Serialization;

namespace SmartSdk.Models
{
    /// <summary>
    /// Representa uma mídia (placa ou RFID) no sistema
    /// </summary>
    public class Media
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("plate")]
        public string? Plate { get; set; }

        [JsonPropertyName("ns320")]
        public long? Ns320 { get; set; }

        [JsonPropertyName("ns321")]
        public long? Ns321 { get; set; }
    }

    /// <summary>
    /// Request para cadastrar placa
    /// </summary>
    public class PlateRequest
    {
        [JsonPropertyName("midia")]
        public string Midia { get; set; } = "plate";

        [JsonPropertyName("plate")]
        public string Plate { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Request para cadastrar RFID
    /// </summary>
    public class RfidRequest
    {
        [JsonPropertyName("midia")]
        public string Midia { get; set; } = "rfid";

        [JsonPropertyName("numero")]
        public string Numero { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}