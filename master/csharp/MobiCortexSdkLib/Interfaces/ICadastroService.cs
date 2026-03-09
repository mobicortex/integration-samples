using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento de Cadastros Centrais (Unidades).
    /// </summary>
    public interface ICadastroService
    {
        /// <summary>
        /// Lista cadastros com paginação e filtro opcional.
        /// </summary>
        /// <param name="offset">Índice inicial</param>
        /// <param name="count">Quantidade de registros</param>
        /// <param name="nameFilter">Filtro simples por nome (opcional)</param>
        /// <param name="searchFilter">Busca cruzada canônica do backend (opcional, tem precedência sobre nameFilter)</param>
        /// <returns>Lista de cadastros</returns>
        Task<ApiResult<CadastroListResponse>> ListarAsync(int offset = 0, int count = 20, string? nameFilter = null, string? searchFilter = null);

        /// <summary>
        /// Obtém um cadastro pelo ID.
        /// </summary>
        Task<ApiResult<CadastroCentral>> ObterAsync(uint id);

        /// <summary>
        /// Cria um novo cadastro.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> CriarAsync(CadastroCentral cadastro);

        /// <summary>
        /// Atualiza um cadastro existente.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> AtualizarAsync(CadastroCentral cadastro);

        /// <summary>
        /// Remove um cadastro e todas as entidades/mídias vinculadas.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirAsync(uint id);

        /// <summary>
        /// Obtém estatísticas de capacidade.
        /// </summary>
        Task<ApiResult<CadastroStats>> ObterEstatisticasAsync();
    }
}
