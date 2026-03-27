using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for managing Central Registries (Units).
    /// </summary>
    public interface IRegistryService
    {
        /// <summary>
        /// Lists registries with pagination and optional filter.
        /// </summary>
        /// <param name="offset">Start index</param>
        /// <param name="count">Number of records</param>
        /// <param name="nameFilter">Simple name filter (optional)</param>
        /// <param name="searchFilter">Canonical cross-search from backend (optional, takes precedence over nameFilter)</param>
        /// <returns>List of registries</returns>
        Task<ApiResult<RegistryListResponse>> ListAsync(int offset = 0, int count = 20, string? nameFilter = null, string? searchFilter = null);

        /// <summary>
        /// Gets a registry by ID.
        /// </summary>
        Task<ApiResult<CentralRegistry>> GetAsync(uint id);

        /// <summary>
        /// Creates a new registry.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> CreateAsync(CentralRegistry registry);

        /// <summary>
        /// Updates an existing registry.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> UpdateAsync(CentralRegistry registry);

        /// <summary>
        /// Removes a registry and all linked entities/media.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteAsync(uint id);

        /// <summary>
        /// Gets capacity statistics.
        /// </summary>
        Task<ApiResult<RegistryStats>> GetStatisticsAsync();
    }
}
