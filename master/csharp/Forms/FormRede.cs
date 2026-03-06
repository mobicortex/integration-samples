using MobiCortex.Sdk;
using MobiCortex.Sdk.Services;
using MobiCortex.Sdk.Models;
using MobiCortex.Sdk.Interfaces;

namespace SmartSdk
{
    // =============================================================================
    //  CONFIGURAÇÃO DE REDE
    //
    //  Este formulário demonstra como ler e alterar a configuração de rede
    //  (ethernet cabo) do controlador.
    //
    //  ENDPOINTS:
    //  GET  /network-config-cable → Lê a configuração atual
    //  POST /network-config-cable → Salva nova configuração
    //
    //  CAMPOS:
    //  - dhcp: 1=DHCP, 0=IP fixo
    //  - ip, mask, gateway, dns1, dns2
    //
    //  ATENÇÃO: Alterar o IP do controlador pode desconectar a sessão.
    //  O controlador reinicia a interface de rede após a alteração.
    // =============================================================================

    public partial class FormRede : Form
    {
        private IMobiCortexClient _api = null!;

        /// <summary>
        /// Serviço da API. Pode ser definido via propriedade para uso no designer.
        /// </summary>
        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

        /// <summary>
        /// Construtor padrão para o Designer do Visual Studio.
        /// </summary>
        public FormRede()
        {
            InitializeComponent();
        }

        public FormRede(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        // =====================================================================
        //  CARREGAR CONFIGURAÇÃO
        // =====================================================================

        private async void FormRede_Load(object? sender, EventArgs e)
        {
            // No modo design do VS, _api pode ser null - não carregar dados
            if (_api == null) return;
            await CarregarConfiguracao();
        }

        /// <summary>
        /// Lê a configuração de rede atual do controlador.
        /// GET /network-config-cable
        /// </summary>
        private async Task CarregarConfiguracao()
        {
            Log("Lendo configuração de rede...");
            var result = await _api.Sistema.ObterConfiguracaoRedeAsync();

            if (result.Success && result.Data != null)
            {
                var cfg = result.Data;
                chkDhcp.Checked = cfg.Dhcp == 1;
                txtIp.Text = cfg.Ip;
                txtMascara.Text = cfg.Mask;
                txtGateway.Text = cfg.Gateway;
                txtDns1.Text = cfg.Dns1;
                txtDns2.Text = cfg.Dns2;
                AtualizarEstadoCampos();
                Log($"Configuração carregada: {(cfg.Dhcp == 1 ? "DHCP" : cfg.Ip)}");
            }
            else
            {
                Log($"Erro ao carregar: {result.Message}");
            }
        }

        /// <summary>
        /// Quando DHCP está ativo, desabilita os campos de IP manual.
        /// </summary>
        private void AtualizarEstadoCampos()
        {
            var manual = !chkDhcp.Checked;
            txtIp.Enabled = manual;
            txtMascara.Enabled = manual;
            txtGateway.Enabled = manual;
            txtDns1.Enabled = manual;
            txtDns2.Enabled = manual;
        }

        private void chkDhcp_CheckedChanged(object? sender, EventArgs e)
        {
            AtualizarEstadoCampos();
        }

        // =====================================================================
        //  SALVAR CONFIGURAÇÃO
        // =====================================================================

        /// <summary>
        /// Salva a configuração de rede no controlador.
        /// POST /network-config-cable
        ///
        /// ATENÇÃO: Se o IP for alterado, a sessão será perdida e o controlador
        /// ficará acessível no novo IP.
        /// </summary>
        private async void btnSalvar_Click(object? sender, EventArgs e)
        {
            // Confirmação antes de salvar
            var confirm = MessageBox.Show(
                "Salvar configuração de rede?\n\n" +
                "ATENÇÃO: Se o IP for alterado, você precisará\n" +
                "reconectar usando o novo endereço.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var config = new NetworkCableConfig
            {
                Dhcp = chkDhcp.Checked ? 1 : 0,
                Ip = txtIp.Text.Trim(),
                Mask = txtMascara.Text.Trim(),
                Gateway = txtGateway.Text.Trim(),
                Dns1 = txtDns1.Text.Trim(),
                Dns2 = txtDns2.Text.Trim()
            };

            Log("Salvando configuração...");
            var result = await _api.Sistema.SalvarConfiguracaoRedeAsync(config);

            if (result.Success)
                Log("Configuração salva com sucesso! A rede será reiniciada.");
            else
                Log($"Erro ao salvar: {result.Message}");
        }

        /// <summary>
        /// Recarrega a configuração atual do controlador.
        /// </summary>
        private async void btnRecarregar_Click(object? sender, EventArgs e)
        {
            await CarregarConfiguracao();
        }

        // =====================================================================
        //  HELPERS
        // =====================================================================

        private void Log(string msg)
        {
            var ts = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{ts}] {msg}{Environment.NewLine}");
        }
    }
}
