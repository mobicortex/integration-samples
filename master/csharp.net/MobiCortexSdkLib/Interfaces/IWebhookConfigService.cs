using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento dos webhooks persistidos na controladora.
    /// </summary>
    public interface IWebhookConfigService
    {
        /// <summary>
        /// Lista todos os slots de webhook.
        /// </summary>
        Task<ApiResult<WebhookListResponse>> ListarAsync();

        /// <summary>
        /// Obtém a configuração de um slot específico.
        /// </summary>
        Task<ApiResult<WebhookConfig>> ObterAsync(int id);

        /// <summary>
        /// Cria ou atualiza a configuração de um slot.
        /// </summary>
        Task<ApiResult<WebhookConfig>> SalvarAsync(int id, WebhookConfig config);

        /// <summary>
        /// Remove a configuração de um slot.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirAsync(int id);
    }
}