using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for listing devices and commanding their outputs (relays / DOUT).
    /// Linux Master only — see GET /devices and POST /devices/relay.
    /// </summary>
    public interface IDevicesService
    {
        /// <summary>
        /// Lists all devices (local and federated) with available outputs.
        /// GET /devices
        /// </summary>
        Task<ApiResult<DeviceListResponse>> ListAsync();

        /// <summary>
        /// Commands one or more relay/DOUT outputs on a device (by id or name).
        /// POST /devices/relay
        /// </summary>
        Task<ApiResult<DeviceRelayResponse>> TriggerAsync(DeviceRelayRequest request);
    }
}
