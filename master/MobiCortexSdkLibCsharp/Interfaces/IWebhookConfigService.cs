using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Service for managing webhooks persisted on the controller.
    /// </summary>
    public interface IWebhookConfigService
    {
        /// <summary>
        /// Lists all webhook slots.
        /// </summary>
        Task<ApiResult<WebhookListResponse>> ListAsync();

        /// <summary>
        /// Gets the configuration of a specific slot.
        /// </summary>
        Task<ApiResult<WebhookConfig>> GetAsync(int id);

        /// <summary>
        /// Creates or updates the configuration of a slot.
        /// </summary>
        Task<ApiResult<WebhookConfig>> SaveAsync(int id, WebhookConfig config);

        /// <summary>
        /// Removes the configuration of a slot.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> DeleteAsync(int id);

        /// <summary>
        /// Fires a test event on the webhook.
        /// GET /webhook/test?id=X
        /// </summary>
        Task<ApiResult<ApiRetResponse>> TestAsync(int id);
    }
}
