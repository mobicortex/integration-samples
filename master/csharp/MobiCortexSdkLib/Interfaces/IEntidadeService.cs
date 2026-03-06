using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento de Entidades (Pessoas, Veículos).
    /// </summary>
    public interface IEntidadeService
    {
        /// <summary>
        /// Lista entidades de um cadastro específico.
        /// </summary>
        /// <param name="cadastroId">ID do cadastro central</param>
        Task<ApiResult<EntidadeListResponse>> ListarPorCadastroAsync(uint cadastroId);

        /// <summary>
        /// Lista todas as entidades (paginado, opcionalmente filtrado por nome).
        /// </summary>
        Task<ApiResult<EntidadeListResponse>> ListarTodosAsync(int offset = 0, int count = 10, string? nome = null);

        /// <summary>
        /// Obtém uma entidade pelo ID.
        /// </summary>
        Task<ApiResult<Entidade>> ObterAsync(uint entityId);

        /// <summary>
        /// Cria uma nova entidade.
        /// </summary>
        Task<ApiResult<CriarEntidadeResponse>> CriarAsync(CriarEntidadeRequest request);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> AtualizarAsync(uint entityId, Entidade entidade);

        /// <summary>
        /// Remove uma entidade e todas as suas mídias.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirAsync(uint entityId);
    }
}
