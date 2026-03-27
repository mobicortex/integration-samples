using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Main interface of the MobiCortex SDK client.
    /// </summary>
    public interface IMobiCortexClient
    {
        /// <summary>
        /// Configures the controller base URL.
        /// </summary>
        /// <param name="baseUrl">Base URL (e.g.: https://192.168.0.100:4449)</param>
        void ConfigureBaseUrl(string baseUrl);

        /// <summary>
        /// Configured controller base URL.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Indicates whether the client is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Current session key (if authenticated).
        /// </summary>
        string? SessionKey { get; }

        /// <summary>
        /// Central registries service.
        /// </summary>
        IRegistryService Registries { get; }

        /// <summary>
        /// Entities service.
        /// </summary>
        IEntityService Entities { get; }

        /// <summary>
        /// Access media service.
        /// </summary>
        IMediaService Media { get; }

        /// <summary>
        /// System settings service.
        /// </summary>
        ISystemService SystemInfo { get; }

        /// <summary>
        /// Authentication, password and access tokens service.
        /// </summary>
        IAccessService Access { get; }

        /// <summary>
        /// Controller webhook configuration service.
        /// </summary>
        IWebhookConfigService Webhooks { get; }

        /// <summary>
        /// Video channel configuration service.
        /// </summary>
        IVideoSourceService VideoSources { get; }

        /// <summary>
        /// Logs in to the controller.
        /// </summary>
        /// <param name="password">Administrator password</param>
        /// <returns>Login result with session key</returns>
        Task<ApiResult<LoginResponse>> LoginAsync(string password);

        /// <summary>
        /// Tests TCP connectivity with the controller.
        /// </summary>
        Task<ApiResult<bool>> TestConnectionAsync();
    }
}
