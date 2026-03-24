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

All forms share a single `MobiCortexApiService` instance.

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
- MQTT connection to the controller (port 1883)
- Real-time access events
- Formatted event output in the console

### Dashboard
- Device information (model, firmware version, MAC)
- Registry and entity statistics

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
|   |-- FormCadastroCompleto.cs/.Designer.cs
|   |-- FormCadastroSimples.cs/.Designer.cs
|   |-- FormMonitoramento.cs/.Designer.cs
|   `-- FormDashboard.cs/.Designer.cs
|-- Models/
|   `-- MobiCortexModels.cs
|-- Services/
|   `-- MobiCortexApiService.cs
|-- MobiCortexSdkLib/
|   `-- MobiCortex.Sdk.csproj
|-- MainForm.cs / .Designer.cs
|-- Program.cs
|-- SmartSdk.csproj
`-- README.md
```

## REST API Overview

The service communicates with the controller REST API over HTTPS (port 4449, self-signed certificate).

Base route prefix: `/mbcortex/master/api/v1`

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/login` | Login with password and receive `session_key` (Bearer token, 900s TTL) |
| POST | `/logout` | End the current session |
| PUT | `/password` | Change the device password |

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

## Code Examples

### Create an entity in the simplified flow (`createid=true`)

```csharp
var request = new CriarEntidadeRequest
{
    CreateId = true,
    Type = (int)TipoEntidade.Pessoa,
    Name = "John Doe",
    Doc = "123.456.789-00"
};

var result = await client.Entidades.CriarAsync(request);
if (result.Success && result.Data?.Ret == 0)
{
    Console.WriteLine($"entity_id={result.Data.EntityId}, central_registry_id={result.Data.CadastroId}");
}
```

### Create an entity in the complete flow (informing `central_registry_id`)

```csharp
var request = new CriarEntidadeRequest
{
    CentralRegistryId = 42,
    Type = (int)TipoEntidade.Veiculo,
    Brand = "Honda",
    Model = "Civic",
    Color = "Black",
    Doc = "ABC1D23",
    LprEnabled = true
};

var result = await client.Entidades.CriarAsync(request);
```

### List entities with pagination

```csharp
var cadastros = await client.Cadastros.ListarAsync(offset: 0, count: 20, nameFilter: "John");

foreach (var cad in cadastros.Data.Items)
{
    var entidades = await client.Entidades.ListarPorCadastroAsync(cad.Id);
    foreach (var ent in entidades.Data.Items)
        Console.WriteLine($"  {ent.EntityId} - {ent.Name} ({ent.Doc})");
}
```

### Create RFID media

```csharp
var request = new CriarMidiaRequest
{
    EntityId = 4294000123,
    CentralRegistryId = 42,
    Type = TipoMidia.Wiegand26,
    Description = "123,45678"
};

var result = await client.Midias.CriarAsync(request);
```

### Create LPR media (vehicle plate)

Important: the backend validates the media format automatically. For LPR media, send `ns32_0` and `ns32_1` so the backend does not try to validate the plate as RFID data.

```csharp
var request = new CriarMidiaRequest
{
    EntityId = 4294000123,
    CentralRegistryId = 42,
    Type = TipoMidia.Lpr,
    Description = "ABC1D23",
    Ns32_0 = 0,
    Ns32_1 = 0
};

var result = await client.Midias.CriarAsync(request);
```

Recommended approach: use `lpr_enabled=true` when creating or updating the vehicle entity. The backend then creates or updates the LPR media automatically.

```csharp
var request = new CriarEntidadeRequest
{
    CentralRegistryId = 42,
    Type = (int)TipoEntidade.Veiculo,
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



