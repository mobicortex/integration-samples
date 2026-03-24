using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para autenticação, troca de senha e gestão de tokens de API.
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Altera a senha administrativa do controlador.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> AlterarSenhaAsync(ChangePasswordRequest request);

        /// <summary>
        /// Lista todos os tokens de API cadastrados.
        /// </summary>
        Task<ApiResult<ApiTokenListResponse>> ListarTokensAsync();

        /// <summary>
        /// Cria um novo token de API.
        /// </summary>
        Task<ApiResult<ApiTokenCreateResponse>> CriarTokenAsync(CreateApiTokenRequest request);

        /// <summary>
        /// Revoga um token de API existente.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> ExcluirTokenAsync(string token);
    }
}