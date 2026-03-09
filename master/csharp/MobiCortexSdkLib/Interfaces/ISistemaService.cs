using MobiCortex.Sdk.Models;

namespace MobiCortex.Sdk.Interfaces
{
    /// <summary>
    /// Serviço para informações e configurações do sistema.
    /// </summary>
    public interface ISistemaService
    {
        /// <summary>
        /// Obtém informações do dispositivo (modelo, firmware, etc).
        /// </summary>
        Task<ApiResult<DeviceInfo>> ObterDeviceInfoAsync();

        /// <summary>
        /// Obtém estatísticas do dashboard (cadastros, pessoas, veículos, mídias).
        /// </summary>
        Task<ApiResult<DashboardStats>> ObterDashboardAsync();

        /// <summary>
        /// Obtém catálogos de veículo expostos pelo backend (cores padrão e marcas sugeridas).
        /// </summary>
        Task<ApiResult<VehicleCatalogsResponse>> ObterCatalogosVeiculoAsync();

        /// <summary>
        /// Obtém configuração de rede (ethernet).
        /// </summary>
        Task<ApiResult<NetworkCableConfig>> ObterConfiguracaoRedeAsync();

        /// <summary>
        /// Salva configuração de rede.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> SalvarConfiguracaoRedeAsync(NetworkCableConfig config);
    }
}
