using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento de Mídias de Acesso (cartões, biometrias, placas).
    /// </summary>
    public interface IMidiaService
    {
        /// <summary>
        /// Lista mídias de uma entidade.
        /// </summary>
        /// <param name="entityId">ID da entidade</param>
        Task<ApiResult<MidiaListResponse>> ListarPorEntidadeAsync(uint entityId);

        /// <summary>
        /// Obtém uma mídia pelo ID.
        /// </summary>
        Task<ApiResult<MidiaAcesso>> ObterAsync(uint mediaId);

        /// <summary>
        /// Cria uma nova mídia.
        /// </summary>
        Task<ApiResult<CriarMidiaResponse>> CriarAsync(CriarMidiaRequest request);

        /// <summary>
        /// Atualiza o status de habilitação de uma mídia.
        /// </summary>
        /// <param name="mediaId">ID da mídia</param>
        /// <param name="enabled">true=habilitada, false=bloqueada</param>
        Task<ApiResult<ApiRetResponse>> AlterarStatusAsync(uint mediaId, bool enabled);

        /// <summary>
        /// Atualiza a data de bloqueio de uma mídia.
        /// </summary>
        /// <param name="mediaId">ID da mídia</param>
        /// <param name="dtBlock">Timestamp UNIX (0 = sem bloqueio)</param>
        Task<ApiResult<ApiRetResponse>> AlterarDataBloqueioAsync(uint mediaId, uint dtBlock);

        /// <summary>
        /// Remove uma mídia.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirAsync(uint mediaId);
    }
}
