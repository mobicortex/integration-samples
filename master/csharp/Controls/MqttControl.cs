using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using SmartSdk.Services;
using MQTTnet.Formatter;
using System.Text;

namespace SmartSdk.Controls
{
    /// <summary>
    /// Controle para conexão MQTT/WSS e recebimento de eventos em tempo real
    /// </summary>
    public class MqttControl : UserControl, IConnectionAware
    {
        private MobiCortexApiService _apiService = null!;
        private IMqttClient? _mqttClient;
        private MqttFactory? _mqttFactory;
        private bool _isMqttConnected = false;
        private string _baseUrl = "192.168.120.45"; // URL base da controladora (do painel principal)

        // Controles da interface
        private TextBox _txtMqttServer = null!;
        private TextBox _txtMqttPort = null!;
        private TextBox _txtMqttTopic = null!;
        private TextBox _txtClientId = null!;
        private ComboBox _cmbProtocol = null!;
        private Button _btnConnectMqtt = null!;
        private Button _btnClear = null!;
        private TextBox _txtLog = null!;
        private Label _lblStatus = null!;
        private CheckBox _chkAutoScroll = null!;
        private CheckBox _chkFormatJson = null!;
        private CheckBox _chkIgnoreCert = null!;

        public MqttControl()
        {
            InitializeComponent();
        }

        public void SetApiService(MobiCortexApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Define a URL base da controladora (usada para WSS)
        /// </summary>
        public void SetBaseUrl(string baseUrl)
        {
            // Parseia a URL para extrair servidor e porta
            var url = baseUrl.Trim();
            
            // Remove protocolo
            url = url.Replace("https://", "").Replace("http://", "");
            
            // Separa servidor e porta
            var parts = url.Split(':');
            _baseUrl = parts[0].Trim();
            
            // Extrai porta (padrão 443 para HTTPS se não especificado)
            string wssPort = "443";
            if (parts.Length > 1 && int.TryParse(parts[1], out int parsedPort))
            {
                wssPort = parsedPort.ToString();
            }
            
            // Atualiza campos se estiver usando WSS (mas permite edição pelo usuário)
            if (_cmbProtocol.SelectedIndex == 1 && !string.IsNullOrEmpty(_baseUrl))
            {
                _txtMqttServer.Text = _baseUrl;
                // Só preenche a porta se o campo estiver vazio (respeita o que o usuário digitou)
                if (string.IsNullOrEmpty(_txtMqttPort.Text))
                {
                    _txtMqttPort.Text = wssPort;
                }
            }
        }

        public void OnConnected()
        {
            // Habilita o botão de conexão MQTT quando a API principal estiver conectada
            _btnConnectMqtt.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(10);

            // Layout principal
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));   // Painel de conexão
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // Log
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));   // Status

            // ========== PAINEL DE CONEXÃO ==========
            var panelConnection = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Protocolo (MQTT ou WSS)
            var lblProtocol = new Label
            {
                Text = "Protocolo:",
                Location = new Point(5, 8),
                Size = new Size(60, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            _cmbProtocol = new ComboBox
            {
                Location = new Point(70, 6),
                Size = new Size(100, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbProtocol.Items.AddRange(new object[] { "MQTT", "WSS" });
            _cmbProtocol.SelectedIndex = 0;
            _cmbProtocol.SelectedIndexChanged += (s, e) => OnProtocolChanged();

            // Server
            var lblServer = new Label
            {
                Text = "Servidor:",
                Location = new Point(175, 8),
                Size = new Size(55, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            _txtMqttServer = new TextBox
            {
                Location = new Point(235, 6),
                Size = new Size(140, 23),
                Text = "192.168.120.45"
            };

            // Port
            var lblPort = new Label
            {
                Text = "Porta:",
                Location = new Point(380, 8),
                Size = new Size(40, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            _txtMqttPort = new TextBox
            {
                Location = new Point(425, 6),
                Size = new Size(50, 23),
                Text = "1883"
            };

            // Topic (readonly - sempre usa # para todos)
            var lblTopic = new Label
            {
                Text = "Tópico:",
                Location = new Point(480, 8),
                Size = new Size(45, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            _txtMqttTopic = new TextBox
            {
                Location = new Point(530, 6),
                Size = new Size(120, 23),
                Text = "#",
                ReadOnly = true,
                BackColor = Color.FromArgb(230, 230, 230)
            };

            // Client ID
            var lblClientId = new Label
            {
                Text = "Client ID:",
                Location = new Point(5, 38),
                Size = new Size(60, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            _txtClientId = new TextBox
            {
                Location = new Point(70, 36),
                Size = new Size(150, 23),
                Text = $"SmartSDK_{Guid.NewGuid().ToString()[..8]}"
            };

            // Checkbox opções
            _chkAutoScroll = new CheckBox
            {
                Text = "Auto-scroll",
                Location = new Point(230, 38),
                Size = new Size(90, 20),
                Checked = true
            };
            _chkFormatJson = new CheckBox
            {
                Text = "Formatar JSON",
                Location = new Point(325, 38),
                Size = new Size(110, 20),
                Checked = true
            };
            _chkIgnoreCert = new CheckBox
            {
                Text = "Ignorar certificado SSL",
                Location = new Point(440, 38),
                Size = new Size(160, 20),
                Checked = true
            };

            // Botão Conectar
            _btnConnectMqtt = new Button
            {
                Location = new Point(660, 5),
                Size = new Size(120, 55),
                Text = "🔌 Conectar",
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            _btnConnectMqtt.Click += OnConnectMqttClick;

            panelConnection.Controls.AddRange(new Control[]
            {
                lblProtocol, _cmbProtocol,
                lblServer, _txtMqttServer,
                lblPort, _txtMqttPort,
                lblTopic, _txtMqttTopic,
                lblClientId, _txtClientId,
                _chkAutoScroll, _chkFormatJson, _chkIgnoreCert,
                _btnConnectMqtt
            });

            // ========== ÁREA DE LOG ==========
            var panelLog = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 5, 0, 0)
            };

            _txtLog = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9F),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(220, 220, 220),
                ReadOnly = true
            };

            // Botão limpar log (dock no bottom)
            _btnClear = new Button
            {
                Text = "🗑️ Limpar Log",
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 5, 0, 0)
            };
            _btnClear.Click += (s, e) =>
            {
                _txtLog.Clear();
                Log("🗑️ Log limpo");
            };

            panelLog.Controls.Add(_txtLog);
            panelLog.Controls.Add(_btnClear);

            // ========== STATUS BAR ==========
            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                Text = "⚪ Desconectado | Protocolo: MQTT (1883) | Tópico: # (todos)",
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Montar layout
            mainLayout.Controls.Add(panelConnection, 0, 0);
            mainLayout.Controls.Add(panelLog, 0, 1);
            mainLayout.Controls.Add(_lblStatus, 0, 2);

            this.Controls.Add(mainLayout);
        }

        private void OnProtocolChanged()
        {
            var isWss = _cmbProtocol.SelectedIndex == 1;
            
            if (isWss)
            {
                // WSS: preenche com dados da controladora (API) se disponíveis, mas permite edição
                if (!string.IsNullOrEmpty(_baseUrl))
                {
                    _txtMqttServer.Text = _baseUrl;
                }
                if (string.IsNullOrEmpty(_txtMqttPort.Text))
                {
                    _txtMqttPort.Text = "443";
                }
                // Campos permanecem editáveis para o usuário ajustar se necessário
                _txtMqttServer.ReadOnly = false;
                _txtMqttServer.BackColor = SystemColors.Window;
                _txtMqttPort.ReadOnly = false;
                _txtMqttPort.BackColor = SystemColors.Window;
                var port = _txtMqttPort.Text;
                var server = _txtMqttServer.Text;
                _lblStatus.Text = $"⚪ Desconectado | Protocolo: WSS ({port}) | Tópico: # (todos) | Servidor: {server}";
            }
            else
            {
                // MQTT: permite edição
                _txtMqttServer.Text = "192.168.120.45";
                _txtMqttPort.Text = "1883";
                _txtMqttServer.ReadOnly = false;
                _txtMqttServer.BackColor = SystemColors.Window;
                _txtMqttPort.ReadOnly = false;
                _txtMqttPort.BackColor = SystemColors.Window;
                _lblStatus.Text = "⚪ Desconectado | Protocolo: MQTT (1883) | Tópico: # (todos)";
            }
        }

        private async void OnConnectMqttClick(object? sender, EventArgs e)
        {
            if (_isMqttConnected)
            {
                await DisconnectMqttAsync();
            }
            else
            {
                await ConnectMqttAsync();
            }
        }

        private async Task ConnectMqttAsync()
        {
            try
            {
                var server = _txtMqttServer.Text.Trim();
                var portText = _txtMqttPort.Text.Trim();
                var topic = _txtMqttTopic.Text.Trim(); // Sempre "#" para todos os tópicos
                var clientId = _txtClientId.Text.Trim();
                var useWss = _cmbProtocol.SelectedIndex == 1;

                if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(portText))
                {
                    MessageBox.Show("Preencha servidor e porta", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(portText, out int port))
                {
                    MessageBox.Show("Porta inválida", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _btnConnectMqtt.Enabled = false;
                _btnConnectMqtt.Text = "⏳ Conectando...";
                
                if (useWss)
                {
                    Log($"🔌 Conectando via WSS em {server}:{port}/master/api/v1/mqtt...");
                    Log($"📡 Inscrevendo em todos os tópicos (#)...");
                }
                else
                {
                    Log($"🔌 Conectando via MQTT em {server}:{port}...");
                    Log($"📡 Inscrevendo em todos os tópicos (#)...");
                }

                _mqttFactory = new MqttFactory();
                _mqttClient = _mqttFactory.CreateMqttClient();

                MqttClientOptions options;
                
                if (useWss)
                {
                    // Conexão WebSocket Secure - usa endpoint /master/mqtt
                    var wsUri = $"wss://{server}:{port}/master/api/v1/mqtt";
                    Log($"   URI: {wsUri}");
                    
                    var wsOptions = new MqttClientOptionsBuilder()
                        .WithWebSocketServer(o => o.WithUri(wsUri))
                        .WithClientId(clientId)
                        .WithCleanSession();

                    if (_chkIgnoreCert.Checked)
                    {
                        wsOptions = wsOptions.WithTlsOptions(
                            new MqttClientTlsOptions
                            {
                                AllowUntrustedCertificates = true,
                                IgnoreCertificateChainErrors = true,
                                IgnoreCertificateRevocationErrors = true,
                                CertificateValidationHandler = (context) => true
                            });
                    }
                    else
                    {
                        wsOptions = wsOptions.WithTlsOptions(
                            new MqttClientTlsOptions
                            {
                                UseTls = true
                            });
                    }

                    options = wsOptions.Build();
                }
                else
                {
                    // Conexão MQTT padrão (TCP)
                    var tcpOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer(server, port)
                        .WithClientId(clientId)
                        .WithCleanSession();

                    // Se porta for 8883 (MQTTS) ou marcar ignorar certificado, usar TLS
                    if (port == 8883 || _chkIgnoreCert.Checked)
                    {
                        tcpOptions = tcpOptions.WithTlsOptions(
                            new MqttClientTlsOptions
                            {
                                AllowUntrustedCertificates = _chkIgnoreCert.Checked,
                                IgnoreCertificateChainErrors = _chkIgnoreCert.Checked,
                                IgnoreCertificateRevocationErrors = _chkIgnoreCert.Checked,
                                CertificateValidationHandler = _chkIgnoreCert.Checked 
                                    ? (context) => true 
                                    : null
                            });
                    }

                    options = tcpOptions.Build();
                }

                _mqttClient.ApplicationMessageReceivedAsync += OnMqttMessageReceived;

                var result = await _mqttClient.ConnectAsync(options, CancellationToken.None);

                if (result.ResultCode == MqttClientConnectResultCode.Success)
                {
                    _isMqttConnected = true;
                    Log($"✅ Conectado ao servidor MQTT via {_cmbProtocol.SelectedItem}!");

                    // Subscrever em TODOS os tópicos usando wildcard #
                    var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                        .WithTopicFilter(new MqttTopicFilterBuilder()
                            .WithTopic("#")
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .Build())
                        .Build();

                    await _mqttClient.SubscribeAsync(subscribeOptions, CancellationToken.None);
                    Log($"📡 Inscrito em TODOS os tópicos (#)");

                    _btnConnectMqtt.Text = "🔴 Desconectar";
                    _btnConnectMqtt.BackColor = Color.FromArgb(220, 53, 69);
                    _btnConnectMqtt.Enabled = true;
                    
                    if (useWss)
                    {
                        _lblStatus.Text = $"🟢 Conectado | WSS | {server}:{port}/master/mqtt | Todos os tópicos";
                    }
                    else
                    {
                        _lblStatus.Text = $"🟢 Conectado | MQTT | {server}:{port} | Todos os tópicos";
                    }
                    _lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    Log($"❌ Falha na conexão: {result.ResultCode}");
                    _btnConnectMqtt.Enabled = true;
                    _btnConnectMqtt.Text = "🔌 Conectar";
                }
            }
            catch (Exception ex)
            {
                Log($"❌ Erro: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Log($"   Inner: {ex.InnerException.Message}");
                }
                _btnConnectMqtt.Enabled = true;
                _btnConnectMqtt.Text = "🔌 Conectar";
            }
        }

        private async Task DisconnectMqttAsync()
        {
            try
            {
                if (_mqttClient != null && _mqttClient.IsConnected)
                {
                    await _mqttClient.DisconnectAsync();
                    Log("🔌 Desconectado do servidor MQTT");
                }
            }
            catch (Exception ex)
            {
                Log($"⚠️ Erro ao desconectar: {ex.Message}");
            }
            finally
            {
                _isMqttConnected = false;
                _btnConnectMqtt.Text = "🔌 Conectar";
                _btnConnectMqtt.BackColor = Color.FromArgb(40, 167, 69);
                _btnConnectMqtt.Enabled = true;
                
                var isWss = _cmbProtocol.SelectedIndex == 1;
                var port = _txtMqttPort.Text;
                var protocol = isWss ? $"WSS ({port})" : "MQTT (1883)";
                _lblStatus.Text = $"⚪ Desconectado | Protocolo: {protocol} | Tópico: # (todos)";
                _lblStatus.ForeColor = Color.Gray;
            }
        }

        private Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMqttMessageReceived(args));
                return Task.CompletedTask;
            }

            try
            {
                var payload = Encoding.UTF8.GetString(
                    args.ApplicationMessage.PayloadSegment.Array ?? Array.Empty<byte>(),
                    args.ApplicationMessage.PayloadSegment.Offset,
                    args.ApplicationMessage.PayloadSegment.Count);
                var topic = args.ApplicationMessage.Topic;
                var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");

                string displayMessage;
                if (_chkFormatJson.Checked)
                {
                    displayMessage = FormatJson(payload);
                }
                else
                {
                    displayMessage = payload;
                }

                Log($"[{timestamp}] 📨 Tópico: {topic}");
                Log(displayMessage);
                Log(new string('-', 80));
            }
            catch (Exception ex)
            {
                Log($"⚠️ Erro ao processar mensagem: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private string FormatJson(string json)
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                return System.Text.Json.JsonSerializer.Serialize(doc, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch
            {
                return json;
            }
        }

        private void Log(string message)
        {
            if (_txtLog == null || _txtLog.IsDisposed) return;

            if (_txtLog.InvokeRequired)
            {
                _txtLog.Invoke(() => Log(message));
                return;
            }

            _txtLog.AppendText(message + Environment.NewLine);

            if (_chkAutoScroll.Checked)
            {
                _txtLog.SelectionStart = _txtLog.Text.Length;
                _txtLog.ScrollToCaret();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                _ = DisconnectMqttAsync();
            }
            base.OnHandleDestroyed(e);
        }
    }
}
