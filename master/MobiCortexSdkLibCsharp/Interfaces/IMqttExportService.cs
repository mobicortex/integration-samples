using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// REST for MQTT export: inbound credentials (port 1884) and outbound client.
    /// Linux controllers only. Does not exist on ESP32/M3127.
    /// </summary>
    public interface IMqttExportService
    {
        Task<ApiResult<MqttExportStatus>> GetAsync();

        Task<ApiResult<MqttExportUser>> SaveUserAsync(int id, MqttExportUserRequest request);

        Task<ApiResult<ApiRetResponse>> DeleteUserAsync(int id);

        Task<ApiResult<MqttExportClientConfig>> SaveClientAsync(MqttExportClientRequest request);
    }
}
