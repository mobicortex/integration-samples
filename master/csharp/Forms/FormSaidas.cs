using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;

namespace SmartSdk
{
    // =============================================================================
    //  OUTPUTS (DOUT / RELAYS)
    //
    //  Lists devices from GET /devices and commands outputs via POST /devices/relay.
    //  Same path the Master UI and Rules engine use (SMART, RS485, ctrl, external).
    // =============================================================================

    public partial class FormSaidas : Form
    {
        private IMobiCortexClient _api = null!;
        private List<DeviceEntry> _devices = new();

        public IMobiCortexClient ApiService
        {
            get => _api;
            set => _api = value;
        }

        public FormSaidas()
        {
            InitializeComponent();
        }

        public FormSaidas(IMobiCortexClient api) : this()
        {
            _api = api;
        }

        private async void FormSaidas_Load(object? sender, EventArgs e)
        {
            if (_api == null) return;
            await LoadDevicesAsync();
        }

        private async void btnAtualizar_Click(object? sender, EventArgs e) =>
            await LoadDevicesAsync();

        private async Task LoadDevicesAsync()
        {
            Log("GET /devices ...");
            btnAtualizar.Enabled = false;
            try
            {
                var result = await _api.Devices.ListAsync();
                if (!result.Success || result.Data == null)
                {
                    Log($"Error: {result.Message} {result.Data?.Message}");
                    return;
                }

                // Keep the current selection so a refresh never moves the
                // commands to another device.
                var keepId = SelectedDevice()?.Id;
                var keepOutput = SelectedOutput();

                _devices = result.Data.Items ?? new();
                gridDevices.Rows.Clear();
                gridOutputs.Rows.Clear();
                EnableCommandButtons(false);

                foreach (var d in _devices)
                {
                    var online = d.Online == null ? "—" : (d.Online.Value ? "online" : "offline");
                    var master = d.Master == null
                        ? ""
                        : (d.Master.Local ? "local" : (d.Master.Nome ?? d.Master.Gid ?? "remoto"));
                    var saidas = d.Saidas?.Count ?? 0;
                    gridDevices.Rows.Add(d.Id, d.Nome, d.Tipo, d.Modelo ?? "", online, master, saidas);
                }

                RestoreSelection(keepId, keepOutput);

                Log($"OK — {_devices.Count} device(s)");
            }
            catch (Exception ex)
            {
                Log($"Exception: {ex.Message}");
            }
            finally
            {
                btnAtualizar.Enabled = true;
            }
        }

        private void RestoreSelection(string? deviceId, DeviceOutput? output)
        {
            if (deviceId == null) return;
            foreach (DataGridViewRow row in gridDevices.Rows)
            {
                if (row.Cells[0].Value?.ToString() != deviceId) continue;
                gridDevices.CurrentCell = row.Cells[0];   // fires SelectionChanged -> fills outputs
                break;
            }
            if (SelectedDevice()?.Id != deviceId || output == null) return;

            var kind = output.IsRelay ? "relay" : "dout";
            var num = output.Relay ?? output.Dout ?? 0;
            foreach (DataGridViewRow row in gridOutputs.Rows)
            {
                if (row.Cells[0].Value?.ToString() == kind && row.Cells[1].Value?.ToString() == num.ToString())
                {
                    gridOutputs.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void gridDevices_SelectionChanged(object? sender, EventArgs e)
        {
            gridOutputs.Rows.Clear();
            EnableCommandButtons(false);

            var device = SelectedDevice();
            if (device?.Saidas == null) return;

            foreach (var o in device.Saidas)
            {
                var kind = o.IsRelay ? "relay" : (o.IsDout ? "dout" : "?");
                var num = o.Relay ?? o.Dout ?? 0;
                var cmds = o.Cmd == null ? "" : string.Join(",", o.Cmd);
                gridOutputs.Rows.Add(kind, num, o.Label, cmds, o.Estado ?? "—");
            }

            if (gridOutputs.Rows.Count > 0)
                gridOutputs.Rows[0].Selected = true;
        }

        private void gridOutputs_SelectionChanged(object? sender, EventArgs e) =>
            UpdateCommandButtons();

        // SelectionChanged can fire before CurrentRow is updated, so the
        // highlighted row is the source of truth for display and commands.
        private static DataGridViewRow? ActiveRow(DataGridView grid) =>
            grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : grid.CurrentRow;

        private DeviceEntry? SelectedDevice()
        {
            var row = ActiveRow(gridDevices);
            if (row == null || row.Index < 0)
                return null;
            var id = row.Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return null;
            return _devices.Find(d => d.Id == id);
        }

        private DeviceOutput? SelectedOutput()
        {
            var device = SelectedDevice();
            var row = ActiveRow(gridOutputs);
            if (device?.Saidas == null || row == null || row.Index < 0)
                return null;

            var kind = row.Cells[0].Value?.ToString();
            if (!int.TryParse(row.Cells[1].Value?.ToString(), out var num))
                return null;

            return device.Saidas.Find(o =>
                (kind == "relay" && o.Relay == num) ||
                (kind == "dout" && o.Dout == num));
        }

        private void UpdateCommandButtons()
        {
            var output = SelectedOutput();
            if (output == null)
            {
                EnableCommandButtons(false);
                return;
            }

            btnPulse.Enabled = output.Supports("pulse") || output.Cmd.Count == 0;
            btnOn.Enabled = output.Supports("on");
            btnOff.Enabled = output.Supports("off");
            btnToggle.Enabled = output.Supports("toggle");
        }

        private void EnableCommandButtons(bool enabled)
        {
            btnPulse.Enabled = enabled;
            btnOn.Enabled = enabled;
            btnOff.Enabled = enabled;
            btnToggle.Enabled = enabled;
        }

        private async void btnPulse_Click(object? sender, EventArgs e) =>
            await TriggerAsync(cmd: null, useTime: true);

        private async void btnOn_Click(object? sender, EventArgs e) =>
            await TriggerAsync(cmd: "on", useTime: false);

        private async void btnOff_Click(object? sender, EventArgs e) =>
            await TriggerAsync(cmd: "off", useTime: false);

        private async void btnToggle_Click(object? sender, EventArgs e) =>
            await TriggerAsync(cmd: "toggle", useTime: false);

        private async Task TriggerAsync(string? cmd, bool useTime)
        {
            var device = SelectedDevice();
            var output = SelectedOutput();
            if (device == null || output == null)
            {
                MessageBox.Show("Select a device and an output.", "Outputs",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtTimeMs.Text.Trim(), out var timeMs) || timeMs < 50)
                timeMs = 1000;
            if (timeMs > 30000) timeMs = 30000;

            var request = new DeviceRelayRequest
            {
                Id = device.Id,
                Relay = output.Relay,
                Dout = output.Dout,
                Cmd = cmd,
                Time = useTime ? timeMs : null
            };

            var target = output.Label;
            Log($"POST /devices/relay — id={device.Id} {target}" +
                (cmd != null ? $" cmd={cmd}" : $" time={timeMs}ms"));

            try
            {
                var result = await _api.Devices.TriggerAsync(request);
                if (!result.Success || result.Data == null)
                {
                    var err = result.Data?.Error ?? result.Data?.Message ?? result.Message;
                    var sug = result.Data?.Sugestoes;
                    Log($"Error HTTP {result.StatusCode}: {err}");
                    if (sug is { Count: > 0 })
                        Log("Suggestions: " + string.Join(", ", sug));
                    return;
                }

                foreach (var item in result.Data.Acionados)
                {
                    var outLabel = item.Relay.HasValue ? $"relay:{item.Relay}" :
                                   item.Dout.HasValue ? $"dout:{item.Dout}" : "?";
                    Log($"{(item.Executed ? "OK" : "FAIL")} {item.Id} {outLabel} " +
                        $"cmd={item.Cmd} time={item.Time} — {item.Msg}");
                }

                if (result.Data.Acionados.Count == 0 && result.Data.Ret == 0)
                    Log("ret=0 (no acionados items)");
            }
            catch (Exception ex)
            {
                Log($"Exception: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            if (txtLog.IsDisposed) return;
            if (txtLog.InvokeRequired) { txtLog.Invoke(() => Log(message)); return; }

            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            txtLog.AppendText($"[{ts}] {message}{Environment.NewLine}");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnLimparLog_Click(object? sender, EventArgs e) => txtLog.Clear();
    }
}
