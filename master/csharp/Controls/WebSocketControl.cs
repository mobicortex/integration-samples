using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using SmartSdk.Models;
using SmartSdk.Services;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para testar WebSockets
    /// Testa endpoints: WS /ws/events e WS /ws/devices
    /// </summary>
    public partial class WebSocketControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        private ClientWebSocket? _eventWs;
        private ClientWebSocket? _deviceWs;
        private CancellationTokenSource? _eventCts;
        private CancellationTokenSource? _deviceCts;
        
        private ListBox _lstEvents = null!;
        private DataGridView _dgvDevices = null!;
        private Button _btnConnectEvents = null!;
        private Button _btnConnectDevices = null!;
        private Label _lblEventStatus = null!;
        private Label _lblDeviceStatus = null!;
        private List<Device> _devices = new();

        public WebSocketControl()
        {
            InitializeComponent();
        }

        public WebSocketControl(MobiCortexApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
        }

        public void SetApiService(MobiCortexApiService apiService)
        {
            _apiService = apiService;
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                Panel1MinSize = 0,
                Panel2MinSize = 0
            };
            this.Load += (_, _) =>
            {
                if (split.Width > 0)
                    split.SplitterDistance = Math.Clamp(400, 0, split.Width);
            };
            split.SizeChanged += (_, _) =>
            {
                if (split.Width > 0)
                    split.SplitterDistance = Math.Clamp(400, 0, split.Width);
            };

            // Painel esquerdo - Eventos WebSocket
            var panelEvents = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpEvents = new GroupBox
            {
                Text = "📡 WebSocket de Eventos (/ws/events)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Status
            _lblEventStatus = new Label
            {
                Text = "⚪ Desconectado",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.Gray
            };
            grpEvents.Controls.Add(_lblEventStatus);

            // Botão conectar
            _btnConnectEvents = new Button
            {
                Text = "🔗 Conectar",
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnConnectEvents.Click += async (s, e) => await ToggleEventConnectionAsync();
            grpEvents.Controls.Add(_btnConnectEvents);

            // Lista de eventos
            _lstEvents = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(220, 220, 220),
                HorizontalScrollbar = true
            };
            grpEvents.Controls.Add(_lstEvents);

            panelEvents.Controls.Add(grpEvents);
            split.Panel1.Controls.Add(panelEvents);

            // Painel direito - Dispositivos WebSocket
            var panelDevices = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var grpDevices = new GroupBox
            {
                Text = "📱 WebSocket de Dispositivos (/ws/devices)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Status
            _lblDeviceStatus = new Label
            {
                Text = "⚪ Desconectado",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.Gray
            };
            grpDevices.Controls.Add(_lblDeviceStatus);

            // Botão conectar
            _btnConnectDevices = new Button
            {
                Text = "🔗 Conectar",
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnConnectDevices.Click += async (s, e) => await ToggleDeviceConnectionAsync();
            grpDevices.Controls.Add(_btnConnectDevices);

            // DataGridView de dispositivos
            _dgvDevices = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            _dgvDevices.Columns.Add("Id", "ID");
            _dgvDevices.Columns.Add("Modelo", "Modelo");
            _dgvDevices.Columns.Add("Ip", "IP");
            _dgvDevices.Columns.Add("Sinal", "Sinal (dBm)");
            _dgvDevices.Columns.Add("Status", "Status");
            _dgvDevices.Columns.Add("UltimoVisto", "Último Visto");

            grpDevices.Controls.Add(_dgvDevices);
            panelDevices.Controls.Add(grpDevices);
            split.Panel2.Controls.Add(panelDevices);

            this.Controls.Add(split);
        }

        private async Task ToggleEventConnectionAsync()
        {
            if (_eventWs?.State == WebSocketState.Open)
            {
                await DisconnectEventsAsync();
            }
            else
            {
                await ConnectEventsAsync();
            }
        }

        private async Task ToggleDeviceConnectionAsync()
        {
            if (_deviceWs?.State == WebSocketState.Open)
            {
                await DisconnectDevicesAsync();
            }
            else
            {
                await ConnectDevicesAsync();
            }
        }

        private async Task ConnectEventsAsync()
        {
            try
            {
                _eventCts = new CancellationTokenSource();
                _eventWs = new ClientWebSocket();
                
                var wsUrl = _apiService.BaseUrl.Replace("http://", "ws://").Replace("https://", "wss://");
                await _eventWs.ConnectAsync(new Uri($"{wsUrl}/ws/events"), _eventCts.Token);

                UpdateEventStatus(true);
                AddEventMessage("✅ Conectado ao WebSocket de eventos");

                _ = ReceiveEventsAsync();
            }
            catch (Exception ex)
            {
                AddEventMessage($"❌ Erro ao conectar: {ex.Message}");
            }
        }

        private async Task ConnectDevicesAsync()
        {
            try
            {
                _deviceCts = new CancellationTokenSource();
                _deviceWs = new ClientWebSocket();
                
                var wsUrl = _apiService.BaseUrl.Replace("http://", "ws://").Replace("https://", "wss://");
                await _deviceWs.ConnectAsync(new Uri($"{wsUrl}/ws/devices"), _deviceCts.Token);

                UpdateDeviceStatus(true);

                _ = ReceiveDevicesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ReceiveEventsAsync()
        {
            var buffer = new byte[4096];
            
            while (_eventWs?.State == WebSocketState.Open && !_eventCts!.IsCancellationRequested)
            {
                try
                {
                    var result = await _eventWs.ReceiveAsync(new ArraySegment<byte>(buffer), _eventCts.Token);
                    
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    // Tenta formatar como JSON
                    try
                    {
                        var evt = JsonSerializer.Deserialize<Event>(message);
                        if (evt != null)
                        {
                            AddEventMessage($"[{evt.Timestamp}] {evt.Tipo}: {evt.Valor} ({evt.Nome})");
                        }
                        else
                        {
                            AddEventMessage(message);
                        }
                    }
                    catch
                    {
                        AddEventMessage(message);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    AddEventMessage($"❌ Erro: {ex.Message}");
                    break;
                }
            }

            await DisconnectEventsAsync();
        }

        private async Task ReceiveDevicesAsync()
        {
            var buffer = new byte[4096];
            
            while (_deviceWs?.State == WebSocketState.Open && !_deviceCts!.IsCancellationRequested)
            {
                try
                {
                    var result = await _deviceWs.ReceiveAsync(new ArraySegment<byte>(buffer), _deviceCts.Token);
                    
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    try
                    {
                        var devices = JsonSerializer.Deserialize<List<Device>>(message);
                        if (devices != null)
                        {
                            _devices = devices;
                            RefreshDevicesGrid();
                        }
                    }
                    catch { }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    break;
                }
            }

            await DisconnectDevicesAsync();
        }

        private async Task DisconnectEventsAsync()
        {
            try
            {
                _eventCts?.Cancel();
                
                if (_eventWs?.State == WebSocketState.Open)
                {
                    await _eventWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "User disconnected", CancellationToken.None);
                }
                
                _eventWs?.Dispose();
                _eventWs = null;
            }
            catch { }

            UpdateEventStatus(false);
            AddEventMessage("🔌 Desconectado");
        }

        private async Task DisconnectDevicesAsync()
        {
            try
            {
                _deviceCts?.Cancel();
                
                if (_deviceWs?.State == WebSocketState.Open)
                {
                    await _deviceWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "User disconnected", CancellationToken.None);
                }
                
                _deviceWs?.Dispose();
                _deviceWs = null;
            }
            catch { }

            UpdateDeviceStatus(false);
        }

        private void UpdateEventStatus(bool connected)
        {
            if (InvokeRequired)
            {
                Invoke(() => UpdateEventStatus(connected));
                return;
            }

            if (connected)
            {
                _lblEventStatus.Text = "🟢 Conectado";
                _lblEventStatus.ForeColor = Color.Green;
                _btnConnectEvents.Text = "🔌 Desconectar";
                _btnConnectEvents.BackColor = Color.FromArgb(220, 53, 69);
            }
            else
            {
                _lblEventStatus.Text = "⚪ Desconectado";
                _lblEventStatus.ForeColor = Color.Gray;
                _btnConnectEvents.Text = "🔗 Conectar";
                _btnConnectEvents.BackColor = Color.FromArgb(40, 167, 69);
            }
        }

        private void UpdateDeviceStatus(bool connected)
        {
            if (InvokeRequired)
            {
                Invoke(() => UpdateDeviceStatus(connected));
                return;
            }

            if (connected)
            {
                _lblDeviceStatus.Text = "🟢 Conectado";
                _lblDeviceStatus.ForeColor = Color.Green;
                _btnConnectDevices.Text = "🔌 Desconectar";
                _btnConnectDevices.BackColor = Color.FromArgb(220, 53, 69);
            }
            else
            {
                _lblDeviceStatus.Text = "⚪ Desconectado";
                _lblDeviceStatus.ForeColor = Color.Gray;
                _btnConnectDevices.Text = "🔗 Conectar";
                _btnConnectDevices.BackColor = Color.FromArgb(40, 167, 69);
            }
        }

        private void AddEventMessage(string message)
        {
            if (_lstEvents.InvokeRequired)
            {
                _lstEvents.Invoke(() => AddEventMessage(message));
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            _lstEvents.Items.Insert(0, $"[{timestamp}] {message}");
            
            // Limita a 100 itens
            while (_lstEvents.Items.Count > 100)
            {
                _lstEvents.Items.RemoveAt(_lstEvents.Items.Count - 1);
            }
        }

        private void RefreshDevicesGrid()
        {
            if (_dgvDevices.InvokeRequired)
            {
                _dgvDevices.Invoke(RefreshDevicesGrid);
                return;
            }

            _dgvDevices.Rows.Clear();
            foreach (var d in _devices)
            {
                _dgvDevices.Rows.Add(d.Id, d.Modelo, d.Ip, d.Sinal, d.Status, d.UltimoVisto);
            }
        }

        public void OnConnected()
        {
            // Não conecta automaticamente, usuário deve clicar
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _ = DisconnectEventsAsync();
            _ = DisconnectDevicesAsync();
            base.OnHandleDestroyed(e);
        }
    }
}
