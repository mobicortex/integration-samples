using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for authentication, password change and API token management.
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Changes the controller administrative password.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ChangePasswordAsync(ChangePasswordRequest request);

        /// <summary>
        /// Lists all registered API tokens.
        /// </summary>
        Task<ApiResult<ApiTokenListResponse>> ListTokensAsync();

        /// <summary>
        /// Creates a new API token.
        /// </summary>
        Task<ApiResult<ApiTokenCreateResponse>> CreateTokenAsync(CreateApiTokenRequest request);

        /// <summary>
        /// Revokes an existing API token.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteTokenAsync(string token);
    }
}
