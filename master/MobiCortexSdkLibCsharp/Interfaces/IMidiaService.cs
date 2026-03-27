using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for managing Access Media (cards, biometrics, plates).
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Lists media of an entity.
        /// </summary>
        /// <param name="entityId">Entity ID</param>
        Task<ApiResult<MediaListResponse>> ListByEntityAsync(uint entityId);

        /// <summary>
        /// Gets a media by ID.
        /// </summary>
        Task<ApiResult<AccessMedia>> GetAsync(uint mediaId);

        /// <summary>
        /// Creates a new media.
        /// </summary>
        Task<ApiResult<CreateMediaResponse>> CreateAsync(CreateMediaRequest request);

        /// <summary>
        /// Updates the enabled status of a media.
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="enabled">true=enabled, false=blocked</param>
        Task<ApiResult<ApiRetResponse>> ChangeStatusAsync(uint mediaId, bool enabled);

        /// <summary>
        /// Updates the expiration of a media.
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="expiration">UNIX timestamp (0 = no expiration)</param>
        Task<ApiResult<ApiRetResponse>> ChangeExpirationAsync(uint mediaId, uint expiration);

        /// <summary>
        /// Removes a media.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteAsync(uint mediaId);
    }
}
