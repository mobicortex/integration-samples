using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento dos canais de vídeo da controladora.
    /// </summary>
    public interface IVideoSourceService
    {
        /// <summary>
        /// Lista todos os canais configuráveis.
        /// </summary>
        Task<ApiResult<VideoSourceListResponse>> ListarAsync();

        /// <summary>
        /// Obtém a configuração de um canal específico.
        /// </summary>
        Task<ApiResult<VideoSourceConfig>> ObterAsync(int id);

        /// <summary>
        /// Cria ou atualiza um canal de vídeo.
        /// </summary>
        Task<ApiResult<VideoSourceConfig>> SalvarAsync(int id, VideoSourceConfig config);

        /// <summary>
        /// Remove a configuração de um canal.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirAsync(int id);
    }
}