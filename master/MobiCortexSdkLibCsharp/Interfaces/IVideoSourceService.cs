using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for managing the controller's video channels.
    /// </summary>
    public interface IVideoSourceService
    {
        /// <summary>
        /// Lists all configurable channels.
        /// </summary>
        Task<ApiResult<VideoSourceListResponse>> ListAsync();

        /// <summary>
        /// Gets the configuration of a specific channel.
        /// </summary>
        Task<ApiResult<VideoSourceConfig>> GetAsync(int id);

        /// <summary>
        /// Creates or updates a video channel.
        /// </summary>
        Task<ApiResult<VideoSourceConfig>> SaveAsync(int id, VideoSourceConfig config);

        /// <summary>
        /// Removes the configuration of a channel.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteAsync(int id);
    }
}
