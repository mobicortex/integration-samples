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

        /// <summary>
        /// Obtém a configuração do Wi-Fi em modo Access Point.
        /// </summary>
        Task<ApiResult<NetworkWifiApConfig>> ObterConfiguracaoWifiApAsync();

        /// <summary>
        /// Salva a configuração do Wi-Fi em modo Access Point.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> SalvarConfiguracaoWifiApAsync(NetworkWifiApConfig config);

        /// <summary>
        /// Obtém a configuração do Wi-Fi em modo cliente/station.
        /// </summary>
        Task<ApiResult<NetworkWifiStationConfig>> ObterConfiguracaoWifiStationAsync();

        /// <summary>
        /// Salva a configuração do Wi-Fi em modo cliente/station.
        /// </summary>
        Task<ApiResult<ApiRetResponse>> SalvarConfiguracaoWifiStationAsync(NetworkWifiStationConfig config);

        /// <summary>
        /// Lista as interfaces de rede detectadas no equipamento.
        /// </summary>
        Task<ApiResult<NetworkInterfacesResponse>> ListarInterfacesRedeAsync();

        /// <summary>
        /// Executa um scan das redes Wi-Fi visíveis.
        /// </summary>
        Task<ApiResult<WifiScanResponse>> EscanearRedesWifiAsync();

        /// <summary>
        /// Lista os clientes conectados ao Access Point do equipamento.
        /// </summary>
        Task<ApiResult<WifiApClientsResponse>> ListarClientesWifiApAsync();

        /// <summary>
        /// Obtém a qualidade do link Wi-Fi atual.
        /// </summary>
        Task<ApiResult<WifiSignalResponse>> ObterSinalWifiAsync();
    }
}
