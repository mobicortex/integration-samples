# SmartSdk - MobiCortex Integration Sample (C# / WinForms)

Windows Forms application that demonstrates integration with the MobiCortex controller REST API.
This project is a reference sample for integrators who need to manage registries, entities, media, and MQTT monitoring through the MobiCortex platform.

The repository also includes the MobiCortex .NET SDK library used by the sample application. You may freely use, copy, modify, and distribute the SDK/library in your own customer projects under the MIT License, provided the integration is used with MobiCortex devices.

## Architecture

The application uses a multi-form launcher architecture:

- MainForm - Connection screen (IP + password) and launcher for the demo forms.
- FormCadastroCompleto - Complete 3-level model: Central Registry -> Entity -> Media.
- FormCadastroSimples - Simplified 2-level model: Entity -> Media (`createid=true`).
- FormMonitoramento - Real-time event monitoring through MQTT.
- FormDashboard - Device information, uptime, and statistics.
- FormSaidas - List devices and command relays / DOUT.

All forms share a single `MobiCortexClient` instance (`IMobiCortexClient`, from `../MobiCortexSdkLibCsharp`).

## Features

### Complete Registry Flow (3 levels)
- CRUD for Central Registries with pagination (20 items per page)
- CRUD for Entities linked to a registry
- CRUD for Media linked to an entity
- Search by ID (numeric) or by name (text)

### Simplified Registry Flow (2 levels)
- Entity creation with `createid=true` so the controller generates IDs automatically
- No need to create a central registry in advance
- CRUD for Media linked to an entity
- Search by `entity_id` or by name, with pagination

### Monitoring (MQTT)
- MQTT TCP connection to the controller export listener (port **1884**, same username/password as Settings)
- Topic `mbcortex/export/event` (access + LPR). Port 1883 on the controller is loopback IPC only.
- Demo credentials: `mqttuser` / `mqttpass`. **Subscribe** connects and subscribes if you are not already connected.
- Events appear in a grid (Time, Event, Plate, Registered, Topic, Payload). Double-click a row to open the full JSON. Connection messages stay in the log at the bottom.
- Embedded broker form is the **outbound** path (controller publishes to this PC).

### Webhook server
- Listens on `0.0.0.0` (LAN). Save `http://<THIS_PC_LAN_IP>:<port>/webhook` on the controller — not localhost.
- Prefer port **9099** if **8080** is taken (`filesync-win64`). Use **Allow Windows Firewall** if the controller gets `Connection timed out`.
- Double-click a received row (or View Details) to open the full JSON event.

### Dashboard
- Device information (model, firmware version, MAC)
- Registry and entity statistics

### Outputs (DOUT / relays)
- Lists devices via `GET /devices` (SMART, RS485, controller, external)
- Commands outputs via `POST /devices/relay`: pulse, on, off, toggle
- Enables only the `cmd` values each output accepts (e.g. DOUT3 is pulse-only)

## Requirements

- Windows 10 or later
- .NET 8.0 SDK/Runtime
- Network access to the MobiCortex controller

## How To Use

1. Run the `SmartSdk.exe` application.
2. Enter the controller IP address, for example `192.168.120.45`.
3. Enter the password (default: `admin`).
4. Click **Connect**.
5. After connecting, open any demo form from the main launcher.

## Project Structure

```text
SmartSdk/
|-- Forms/
|   |-- FormCadastroCompleto.cs      # full registry: central registry + people + vehicles + media
|   |-- FormCadastroSimples.cs       # simplified flow (createid=true)
|   |-- FormCadastroCentral/Entidade/Pessoa/PessoaEdit/Veiculo/Midia.cs, FormDetalheMidia.cs,
|   |   FormSelecionarTipoEntidade.cs  # dialogs used by the registry screens
|   |-- FormDashboard.cs             # /dashboard + /device-info
|   |-- FormMonitoramento.cs         # MQTT TCP 1884 events in a grid
|   |-- FormMqttCliente.cs           # MQTT client + /mqtt-export/user
|   |-- FormMqttBroker.cs            # local MQTT broker
|   |-- FormWebhookServer.cs         # LAN webhook receiver
|   `-- FormSaidas.cs                # /devices + /devices/relay
|-- MainForm.cs / .Designer.cs
|-- Program.cs
|-- SmartSdk.csproj                  # references ../MobiCortexSdkLibCsharp/MobiCortex.Sdk.csproj
`-- README.md
```

## REST API Overview

The service communicates with the controller REST API over HTTPS (port 4449, self-signed certificate).

Base route prefix: `/mbcortex/master/api/v1`

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/login` | Login with password and receive `session_key` (Bearer token, 900s TTL) |
| DELETE | `/login` | End the current session |
| PUT | `/login` | Change the password (`pass_atual`, `pass_nova`, `pass_nova2`) |

### Central Registry
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/central-registry?offset=0&count=20` | List registries with pagination |
| GET | `/central-registry?id={id}` | Get registry by ID |
| GET | `/central-registry?name={filter}` | Filter registries by name |
| POST | `/central-registry` | Create or update a registry |
| DELETE | `/central-registry?id={id}` | Delete a registry |

### Entities
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/entities?id={entity_id}` | Get entity by ID |
| GET | `/entities?central_registry_id={id}` | List entities from a registry |
| POST | `/entities` | Create an entity, including `createid=true` support |
| PUT | `/entities?id={entity_id}` | Update an entity |
| DELETE | `/entities?id={entity_id}` | Delete an entity and related media |

### Media
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/media?entity_id={id}` | List media for an entity |
| GET | `/media?id={media_id}` | Get media by ID |
| POST | `/media` | Create media (RFID, plate, facial, and others) |
| DELETE | `/media?id={media_id}` | Delete media |

### Dashboard
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/dashboard` | Device statistics |
| GET | `/device-info` | Hardware and firmware information |

### Devices / outputs (relay and DOUT)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/devices` | List devices and commandable outputs |
| POST | `/devices/relay` | Pulse or latch relay/DOUT (by `id` or `nome`) |

## Code Examples

### Create an entity in the simplified flow (`createid=true`)

```csharp
var request = new CreateEntityRequest
{
    CreateId = true,
    Type = (int)EntityType.Person,
    Name = "John Doe",
    Doc = "123.456.789-00"
};

var result = await client.Entities.CreateAsync(request);
if (result.Success && result.Data?.Ret == 0)
{
    Console.WriteLine($"entity_id={result.Data.EntityId}, central_registry_id={result.Data.CentralRegistryId}");
}
```

### Create an entity in the complete flow (informing `central_registry_id`)

```csharp
var request = new CreateEntityRequest
{
    CentralRegistryId = 42,
    Type = (int)EntityType.Vehicle,
    Brand = "Honda",
    Model = "Civic",
    Color = "Black",
    Doc = "ABC1D23",
    LprEnabled = true
};

var result = await client.Entities.CreateAsync(request);
```

### List entities with pagination

```csharp
var cadastros = await client.Registries.ListAsync(offset: 0, count: 20, nameFilter: "John");

foreach (var cad in cadastros.Data.Items)
{
    var entidades = await client.Entities.ListByRegistryAsync(cad.Id);
    foreach (var ent in entidades.Data.Items)
        Console.WriteLine($"  {ent.EntityId} - {ent.Name} ({ent.Doc})");
}
```

### Command relay / DOUT

```csharp
// List devices and outputs
var devices = await client.Devices.ListAsync();
foreach (var d in devices.Data!.Items)
    Console.WriteLine($"{d.Id} {d.Nome} ({d.Tipo}) — {d.Saidas.Count} output(s)");

// 1 s pulse on relay 1 of device "42"
var pulse = await client.Devices.TriggerAsync(
    DeviceRelayRequest.PulseRelay("42", relay: 1, timeMs: 1000));

// Pulse DOUT 3 (lock) on the same SMART
var dout = await client.Devices.TriggerAsync(
    DeviceRelayRequest.PulseDout("42", dout: 3, timeMs: 500));

// Latch on/off (when the output accepts on/off)
var on = await client.Devices.TriggerAsync(
    DeviceRelayRequest.Latch("42", relay: 1, dout: null, cmd: "on"));
```

### Create RFID media

```csharp
var request = new CreateMediaRequest
{
    EntityId = 4294000123,
    CentralRegistryId = 42,
    Type = MediaType.Wiegand26,
    Description = "123,45678"
};

var result = await client.Media.CreateAsync(request);
```

### Create LPR media (vehicle plate)

Important: the backend validates the media format automatically. For LPR media, send `ns32_0` and `ns32_1` so the backend does not try to validate the plate as RFID data.

```csharp
var request = new CreateMediaRequest
{
    EntityId = 4294000123,
    CentralRegistryId = 42,
    Type = MediaType.Lpr,
    Description = "ABC1D23",
    Ns32_0 = 0,
    Ns32_1 = 0
};

var result = await client.Media.CreateAsync(request);
```

Recommended approach: use `lpr_enabled=true` when creating or updating the vehicle entity. The backend then creates or updates the LPR media automatically.

```csharp
var request = new CreateEntityRequest
{
    CentralRegistryId = 42,
    Type = (int)EntityType.Vehicle,
    Brand = "Honda",
    Model = "Civic",
    Color = "Black",
    Doc = "ABC1D23",
    LprEnabled = true
};
```

## Supported Media Types

| Constant | Value | Format |
|----------|-------|--------|
| `Wiegand26` | 1 | `Facility,Card` (example: `123,45678`) |
| `Wiegand34` | 2 | `Facility,Card` |
| `Lpr` | 17 | Vehicle plate (example: `ABC1D23`) |
| `Facial` | 20 | Facial image (base64) |

## Build

```bash
dotnet build SmartSdk.csproj
```

Or use the script:

```bash
build.bat
```

## Run

```bash
dotnet run
```

Or run the executable directly:

```bash
bin\Debug\net8.0-windows\SmartSdk.exe
```

## Changelog

See [CHANGELOG.md](CHANGELOG.md) (English) and [CHANGELOG-ptbr.md](CHANGELOG-ptbr.md) (pt-BR).

## License

This project is licensed under the MIT License.

The sample code and the included SDK/library can be freely used in customer applications under the MIT License, as long as the integration targets MobiCortex devices.

For the full license text, see `LICENSE`.



