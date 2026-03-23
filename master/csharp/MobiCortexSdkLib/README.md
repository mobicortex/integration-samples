# MobiCortex.Sdk

SDK .NET para integraÃ§Ã£o com controladores de acesso MobiCortex.

## ðŸ“¦ InstalaÃ§Ã£o

```bash
dotnet add package MobiCortex.Sdk
```

Ou adicione a referÃªncia de projeto:
```xml
<ProjectReference Include="MobiCortexSdkLib\MobiCortex.Sdk.csproj" />
```

## ðŸš€ Uso BÃ¡sico

### 1. API REST (Cadastros, Entidades, MÃ­dias)

```csharp
using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;

// Criar cliente
var client = new MobiCortexClient();

// Configurar URL do controlador
client.ConfigureBaseUrl("https://192.168.0.100:4449");

// Testar conexÃ£o
var test = await client.TestConnectionAsync();
if (!test.Success) {
    Console.WriteLine($"Erro: {test.Message}");
    # MobiCortex.Sdk

    .NET SDK for integration with MobiCortex access control devices.

    This SDK can be embedded or referenced in your own customer projects. You may freely use, copy, modify, and distribute it under the MIT License, provided the integration is used with MobiCortex devices.

    ## Installation

    ```bash
    dotnet add package MobiCortex.Sdk
    ```

    Or add a project reference:

    ```xml
    <ProjectReference Include="MobiCortexSdkLib\MobiCortex.Sdk.csproj" />
    ```

    ## Basic Usage

    ### 1. REST API (Registries, Entities, Media)

    ```csharp
    using MobiCortex.Sdk;
    using MobiCortex.Sdk.Interfaces;
    using MobiCortex.Sdk.Models;

    var client = new MobiCortexClient();

    client.ConfigureBaseUrl("https://192.168.0.100:4449");

    var test = await client.TestConnectionAsync();
    if (!test.Success)
    {
        Console.WriteLine($"Error: {test.Message}");
        return;
    }

    var login = await client.LoginAsync("admin_password");
    if (login.Success && login.Data?.Ret == 0)
    {
        Console.WriteLine($"Connected. Session: {login.Data.SessionKey}");
    }

    var cadastros = await client.Cadastros.ListarAsync(offset: 0, count: 20);
    var entidades = await client.Entidades.ListarPorCadastroAsync(centralRegistryId: 1);
    var midias = await client.Midias.ListarPorEntidadeAsync(entityId: 1);
    var device = await client.Sistema.ObterDeviceInfoAsync();
    ```

    ### 2. MQTT Client (Receive Real-Time Events)

    ```csharp
    using MobiCortex.Sdk.Interfaces;
    using MobiCortex.Sdk.Services;

    var mqttClient = new MqttClientService();

    mqttClient.MessageReceived += (s, e) =>
    {
        Console.WriteLine($"[{e.ReceivedAt:HH:mm:ss}] {e.Topic}");
        Console.WriteLine($"Payload: {e.Payload}");
    };

    var wsUrl = "wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt";
    var sessionKey = "...";
    var topics = new[]
    {
        "mbcortex/master/events/#",
        "mbcortex/master/logs/#"
    };

    var connected = await mqttClient.ConnectAsync(wsUrl, sessionKey, topics);
    if (connected)
    {
        Console.WriteLine("Connected to MQTT.");
    }

    await mqttClient.DisconnectAsync();
    ```

    ### 3. MQTT Broker (Devices Connect To Your Server)

    Warning: this implementation is intended for reference and testing only. It has not been validated for high-load production scenarios.

    ```csharp
    using MobiCortex.Sdk.Interfaces;
    using MobiCortex.Sdk.Services;

    var broker = new MqttBrokerService();

    broker.ClientConnected += (s, e) =>
    {
        Console.WriteLine($"Device connected: {e.ClientId}");
    };

    broker.MessageReceived += (s, e) =>
    {
        Console.WriteLine($"[{e.ClientId}] {e.Topic}: {e.Payload}");
    };

    var started = await broker.StartAsync(
        port: 1883,
        allowAnonymous: true
    );

    if (started)
    {
        Console.WriteLine("Broker listening on port 1883");
        Console.WriteLine("Configure the controllers to publish MQTT to:");
        Console.WriteLine("  mqtt://your-server:1883");
    }

    await broker.StopAsync();
    ```

    ### 4. Webhook Server (Receive HTTP Events)

    Warning: this implementation is intended for reference and testing only. It has not been validated for high-load production scenarios.

    ```csharp
    using MobiCortex.Sdk.Interfaces;
    using MobiCortex.Sdk.Services;

    var server = new WebhookServerService();

    server.WebhookReceived += (s, e) =>
    {
        Console.WriteLine($"[{e.ReceivedAt:HH:mm:ss}] POST {e.Path}");
        Console.WriteLine($"From: {e.RemoteIp}");
        Console.WriteLine($"Body: {e.Body}");
    };

    var started = await server.StartAsync(
        port: 8080,
        authToken: null
    );

    if (started)
    {
        Console.WriteLine("Webhook server listening on http://localhost:8080/");
        Console.WriteLine("Configure the controllers to send webhooks to:");
        Console.WriteLine("  http://your-server:8080/webhook");
    }

    await server.StopAsync();
    ```

    ## Data Hierarchy

    ```text
    Central Registry (Unit/Company)
      `-- Entity (Person/Vehicle/Animal)
            `-- Access Media (Card, Biometric, Plate)
    ```

    ## Important Limitations

    ### Embedded Servers (MQTT Broker and Webhook)

    `MqttBrokerService` and `WebhookServerService` are provided as reference implementations.

    | Aspect | Approximate Capacity |
    |--------|----------------------|
    | MQTT Broker | Up to 10-20 concurrent connections |
    | Webhook Server | Up to 10-20 requests/second |
    | Recommended use | Development, tests, demos |

    Do not use these embedded server implementations as-is for high-scale production environments.

    ### For Higher-Load Scenarios

    If you need to support dozens, hundreds, or thousands of controllers, consider:

    For MQTT:
    - Eclipse Mosquitto
    - EMQX
    - HiveMQ
    - AWS IoT Core
    - Azure IoT Hub

    For HTTP webhooks:
    - ASP.NET Core with Kestrel + IIS/NGINX
    - AWS API Gateway + Lambda
    - Azure Functions
    - Google Cloud Functions
    - Dedicated servers with load balancing

    ## Service Examples

    ### ICadastroService

    ```csharp
    var lista = await client.Cadastros.ListarAsync(offset: 0, count: 20);

    var cadastro = new CadastroCentral { Id = 0, Name = "Apt 101", Enabled = true };
    var result = await client.Cadastros.CriarAsync(cadastro);
    ```

    ### IEntidadeService

    ```csharp
    var entidades = await client.Entidades.ListarPorCadastroAsync(centralRegistryId: 1);

    var pessoa = new CriarEntidadeRequest
    {
        CentralRegistryId = 1,
        Type = (int)TipoEntidade.Pessoa,
        Name = "John Doe",
        Doc = "12345678900"
    };
    var result = await client.Entidades.CriarAsync(pessoa);
    ```

    ### IMidiaService

    ```csharp
    var midias = await client.Midias.ListarPorEntidadeAsync(entityId: 1);

    var cartao = new CriarMidiaRequest
    {
        EntityId = 1,
        CentralRegistryId = 1,
        Type = TipoMidia.Wiegand26,
        Description = "123,45678"
    };
    var result = await client.Midias.CriarAsync(cartao);
    ```

    ### ISistemaService

    ```csharp
    var info = await client.Sistema.ObterDeviceInfoAsync();
    var stats = await client.Sistema.ObterDashboardAsync();
    ```

    ## Security Notes

    - The HTTP client accepts self-signed SSL/TLS certificates.
    - The MQTT client uses `session_key` as the password.
    - The webhook server optionally supports Bearer authentication.
    - The MQTT broker optionally supports username/password authentication.

    ## License

    This SDK is licensed under the MIT License.

    You may freely use the SDK in customer applications under the MIT License, provided the integration targets MobiCortex devices.

    For the full license text, see the repository `LICENSE` file.


