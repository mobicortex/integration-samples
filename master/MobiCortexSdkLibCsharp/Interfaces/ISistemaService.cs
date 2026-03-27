using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for system information and settings.
    /// </summary>
    public interface ISystemService
    {
        /// <summary>
        /// Gets device information (model, firmware, etc.).
        /// </summary>
        Task<ApiResult<DeviceInfo>> GetDeviceInfoAsync();

        /// <summary>
        /// Gets dashboard statistics (registries, people, vehicles, media).
        /// </summary>
        Task<ApiResult<DashboardStats>> GetDashboardAsync();

        /// <summary>
        /// Gets vehicle catalogs exposed by the backend (default colors and suggested brands).
        /// </summary>
        Task<ApiResult<VehicleCatalogsResponse>> GetVehicleCatalogsAsync();

    }
}
