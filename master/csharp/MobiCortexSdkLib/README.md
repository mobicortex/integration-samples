# MobiCortex.Sdk

SDK .NET para integração com controladores de acesso MobiCortex.

## 📦 Instalação

```bash
dotnet add package MobiCortex.Sdk
```

Ou adicione a referência de projeto:
```xml
<ProjectReference Include="MobiCortexSdkLib\MobiCortex.Sdk.csproj" />
```

## 🚀 Uso Básico

### 1. API REST (Cadastros, Entidades, Mídias)

```csharp
using MobiCortex.Sdk;
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Models;

// Criar cliente
var client = new MobiCortexClient();

// Configurar URL do controlador
client.ConfigureBaseUrl("https://192.168.0.100:4449");

// Testar conexão
var test = await client.TestConnectionAsync();
if (!test.Success) {
    Console.WriteLine($"Erro: {test.Message}");
    return;
}

// Login
var login = await client.LoginAsync("senha_admin");
if (login.Success && login.Data?.Ret == 0)
{
    Console.WriteLine($"Conectado. Session: {login.Data.SessionKey}");
}

// Operações
var cadastros = await client.Cadastros.ListarAsync(offset: 0, count: 20);
var entidades = await client.Entidades.ListarPorCadastroAsync(centralRegistryId: 1);
var midias = await client.Midias.ListarPorEntidadeAsync(entityId: 1);
var device = await client.Sistema.ObterDeviceInfoAsync();
```

### 2. MQTT Client (Receber Eventos em Tempo Real)

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
    Console.WriteLine("Conectado ao MQTT.");
}

await mqttClient.DisconnectAsync();
```

### 3. MQTT Broker (Dispositivos Conectam ao Seu Servidor)

Aviso: esta implementação é destinada apenas a referência e testes. Não foi validada para cenários de produção de alta carga.

```csharp
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

var broker = new MqttBrokerService();

broker.ClientConnected += (s, e) =>
{
    Console.WriteLine($"Dispositivo conectado: {e.ClientId}");
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
    Console.WriteLine("Broker ouvindo na porta 1883");
    Console.WriteLine("Configure as controladoras para publicar MQTT em:");
    Console.WriteLine("  mqtt://seu-servidor:1883");
}

await broker.StopAsync();
```

### 4. Webhook Server (Receber Eventos HTTP)

Aviso: esta implementação é destinada apenas a referência e testes. Não foi validada para cenários de produção de alta carga.

```csharp
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

var server = new WebhookServerService();

server.WebhookReceived += (s, e) =>
{
    Console.WriteLine($"[{e.ReceivedAt:HH:mm:ss}] POST {e.Path}");
    Console.WriteLine($"De: {e.RemoteIp}");
    Console.WriteLine($"Body: {e.Body}");
};

var started = await server.StartAsync(
    port: 8080,
    authToken: null
);

if (started)
{
    Console.WriteLine("Servidor webhook ouvindo em http://localhost:8080/");
    Console.WriteLine("Configure as controladoras para enviar webhooks para:");
    Console.WriteLine("  http://seu-servidor:8080/webhook");
}

await server.StopAsync();
```

## Hierarquia de Dados

```text
Cadastro Central (Unidade/Empresa)
  `-- Entidade (Pessoa/Veículo/Animal)
        `-- Mídia de Acesso (Cartão, Biometria, Placa)
```

## Limitações Importantes

### Servidores Embutidos (MQTT Broker e Webhook Server)

`MqttBrokerService` e `WebhookServerService` são fornecidos como implementações de referência.

| Aspecto | Capacidade Aproximada |
|---------|----------------------|
| MQTT Broker | Até 10-20 conexões simultâneas |
| Webhook Server | Até 10-20 requisições/segundo |
| Uso recomendado | Desenvolvimento, testes, demos |

Não use estas implementações de servidor embutidas como estão para ambientes de produção de alta escala.

### Para Cenários de Maior Carga

Se você precisar suportar dezenas, centenas ou milhares de controladoras, considere:

Para MQTT:
- Eclipse Mosquitto
- EMQX
- HiveMQ
- AWS IoT Core
- Azure IoT Hub

Para webhooks HTTP:
- ASP.NET Core com Kestrel + IIS/NGINX
- AWS API Gateway + Lambda
- Azure Functions
- Google Cloud Functions
- Servidores dedicados com balanceamento de carga

## Exemplos de Serviços

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
    Name = "João Silva",
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

## Notas de Segurança

- O cliente HTTP aceita certificados SSL/TLS autoassinados.
- O cliente MQTT usa `session_key` como senha.
- O servidor webhook suporta autenticação Bearer opcionalmente.
- O broker MQTT suporta autenticação usuário/senha opcionalmente.

## Licença

Este SDK está licenciado sob a Licença MIT.

Você pode usar livremente o SDK em aplicações de clientes sob a Licença MIT, desde que a integração seja destinada a dispositivos MobiCortex.

Para o texto completo da licença, consulte o arquivo `LICENSE` do repositório.
