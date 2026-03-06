using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Interface principal do cliente MobiCortex SDK.
    /// </summary>
    public interface IMobiCortexClient
    {
        /// <summary>
        /// Configura a URL base do controlador.
        /// </summary>
        /// <param name="baseUrl">URL base (ex: https://192.168.0.100:4449)</param>
        void ConfigureBaseUrl(string baseUrl);

        /// <summary>
        /// URL base configurada do controlador.
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// Indica se o cliente está autenticado.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Chave de sessão atual (se autenticado).
        /// </summary>
        string? SessionKey { get; }

        /// <summary>
        /// Serviço de cadastros centrais.
        /// </summary>
        ICadastroService Cadastros { get; }

        /// <summary>
        /// Serviço de entidades.
        /// </summary>
        IEntidadeService Entidades { get; }

        /// <summary>
        /// Serviço de mídias de acesso.
        /// </summary>
        IMidiaService Midias { get; }

        /// <summary>
        /// Serviço de configurações do sistema.
        /// </summary>
        ISistemaService Sistema { get; }

        /// <summary>
        /// Realiza login no controlador.
        /// </summary>
        /// <param name="password">Senha do administrador</param>
        /// <returns>Resultado do login com session key</returns>
        Task<ApiResult<LoginResponse>> LoginAsync(string password);

        /// <summary>
        /// Testa a conectividade TCP com o controlador.
        /// </summary>
        Task<ApiResult<bool>> TestConnectionAsync();
    }
}
