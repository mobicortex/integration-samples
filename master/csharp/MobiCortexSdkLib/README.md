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
if (login.Success && login.Data?.Ret == 0) {
    Console.WriteLine($"Logado! Session: {login.Data.SessionKey}");
}

// Operações CRUD
var cadastros = await client.Cadastros.ListarAsync(offset: 0, count: 20);
var entidades = await client.Entidades.ListarPorCadastroAsync(cadastroId: 1);
var midias = await client.Midias.ListarPorEntidadeAsync(entityId: 1);
var device = await client.Sistema.ObterDeviceInfoAsync();
```

### 2. MQTT Cliente (Receber Eventos em Tempo Real)

```csharp
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

// Criar cliente MQTT
var mqttClient = new MqttClientService();

// Evento de mensagem recebida
mqttClient.MessageReceived += (s, e) => {
    Console.WriteLine($"[{e.ReceivedAt:HH:mm:ss}] {e.Topic}");
    Console.WriteLine($"Payload: {e.Payload}");
};

// Conectar ao broker da controladora
var wsUrl = "wss://192.168.0.100:4449/mbcortex/master/api/v1/mqtt";
var sessionKey = "..."; // obtido no login
var topics = new[] { 
    "mbcortex/master/events/#",
    "mbcortex/master/logs/#" 
};

var connected = await mqttClient.ConnectAsync(wsUrl, sessionKey, topics);
if (connected) {
    Console.WriteLine("Conectado ao MQTT!");
}

// Desconectar quando terminar
await mqttClient.DisconnectAsync();
```

### 3. MQTT Broker (Controladoras Conectam em Você)

⚠️ **ATENÇÃO**: Esta implementação é para **referência e testes**.
Não foi testada para alta carga. Veja seção "Limitações" abaixo.

```csharp
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

// Criar broker
var broker = new MqttBrokerService();

// Eventos
broker.ClientConnected += (s, e) => {
    Console.WriteLine($"Controladora conectada: {e.ClientId}");
};

broker.MessageReceived += (s, e) => {
    Console.WriteLine($"[{e.ClientId}] {e.Topic}: {e.Payload}");
};

// Iniciar broker na porta 1883
var started = await broker.StartAsync(
    port: 1883, 
    allowAnonymous: true  // ou false com usuário/senha
);

if (started) {
    Console.WriteLine("Broker rodando na porta 1883");
    Console.WriteLine("Configure as controladoras para enviar MQTT para:");
    Console.WriteLine($"  mqtt://seu-servidor:1883");
}

// Parar
await broker.StopAsync();
```

### 4. Webhook Server (Receber Eventos HTTP)

⚠️ **ATENÇÃO**: Esta implementação é para **referência e testes**.
Não foi testada para alta carga. Veja seção "Limitações" abaixo.

```csharp
using MobiCortex.Sdk.Interfaces;
using MobiCortex.Sdk.Services;

// Criar servidor
var server = new WebhookServerService();

// Evento de webhook recebido
server.WebhookReceived += (s, e) => {
    Console.WriteLine($"[{e.ReceivedAt:HH:mm:ss}] POST {e.Path}");
    Console.WriteLine($"De: {e.RemoteIp}");
    Console.WriteLine($"Body: {e.Body}");
};

// Iniciar servidor na porta 8080
var started = await server.StartAsync(
    port: 8080,
    authToken: null  // ou "seu-token" para autenticação Bearer
);

if (started) {
    Console.WriteLine("Servidor webhook rodando em http://localhost:8080/");
    Console.WriteLine("Configure as controladoras para enviar webhooks para:");
    Console.WriteLine($"  http://seu-servidor:8080/webhook");
}

// Parar
await server.StopAsync();
```

## 📋 Hierarquia de Dados

```
Cadastro Central (Unidade/Empresa)
  └── Entidade (Pessoa/Veículo/Animal)
        └── Mídia de Acesso (Cartão, Biometria, Placa)
```

## ⚠️ Limitações Importantes

### Servidores Embutidos (MQTT Broker e Webhook)

Os serviços `MqttBrokerService` e `WebhookServerService` são fornecidos como **implementações de referência/exemplo**.

| Aspecto | Capacidade Aproximada |
|---------|----------------------|
| MQTT Broker | Até 10-20 conexões simultâneas |
| Webhook Server | Até 10-20 requisições/segundo |
| Uso recomendado | Desenvolvimento, testes, demonstrações |

**NÃO utilize em produção com muitos dispositivos!**

### Para Cenários de Alta Carga

Se você precisa suportar **dezenas, centenas ou milhares de controladoras**, considere:

**Para MQTT:**
- [Eclipse Mosquitto](https://mosquitto.org/) - Leve e robusto
- [EMQX](https://www.emqx.io/) - Alto desempenho, clustering
- [HiveMQ](https://www.hivemq.com/) - Enterprise
- [AWS IoT Core](https://aws.amazon.com/iot-core/) - Cloud managed
- [Azure IoT Hub](https://azure.microsoft.com/services/iot-hub/) - Cloud managed

**Para Webhooks HTTP:**
- ASP.NET Core com Kestrel + IIS/NGINX
- [AWS API Gateway](https://aws.amazon.com/api-gateway/) + Lambda
- [Azure Functions](https://azure.microsoft.com/services/functions/)
- [Google Cloud Functions](https://cloud.google.com/functions)
- Servidores dedicados com load balancing

## 📚 Exemplos por Serviço

### ICadastroService
```csharp
// Listar cadastros
var lista = await client.Cadastros.ListarAsync(offset: 0, count: 20);

// Criar cadastro
var cadastro = new CadastroCentral { Name = "Apt 101", Enabled = true };
var result = await client.Cadastros.CriarAsync(cadastro);
```

### IEntidadeService
```csharp
// Listar entidades de um cadastro
var entidades = await client.Entidades.ListarPorCadastroAsync(cadastroId: 1);

// Criar pessoa
var pessoa = new CriarEntidadeRequest {
    CadastroId = 1,
    Tipo = (int)TipoEntidade.Pessoa,
    Name = "João Silva",
    Doc = "12345678900"
};
var result = await client.Entidades.CriarAsync(pessoa);
```

### IMidiaService
```csharp
// Listar mídias de uma entidade
var midias = await client.Midias.ListarPorEntidadeAsync(entityId: 1);

// Criar cartão RFID
var cartao = new CriarMidiaRequest {
    EntityId = 1,
    CadastroId = 1,
    Tipo = TipoMidia.Wiegand26,
    Descricao = "123,45678"
};
var result = await client.Midias.CriarAsync(cartao);
```

### ISistemaService
```csharp
// Informações do dispositivo
var info = await client.Sistema.ObterDeviceInfoAsync();

// Estatísticas
var stats = await client.Sistema.ObterDashboardAsync();

// Configuração de rede
var rede = await client.Sistema.ObterConfiguracaoRedeAsync();
```

## 🔒 Segurança

- O cliente HTTP aceita certificados auto-assinados (SSL/TLS)
- MQTT Cliente usa o session_key como senha
- Webhook Server suporta autenticação Bearer (opcional)
- MQTT Broker suporta autenticação por usuário/senha (opcional)

## 📝 Licença

Este SDK é fornecido como exemplo de integração.
