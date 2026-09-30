# SmartSdk - Exemplo de Integracao MobiCortex (C# / WinForms)

Aplicacao Windows Forms que demonstra a integracao com a API REST do controlador MobiCortex.
Este projeto e um exemplo de referencia para integradores que desejam gerenciar cadastros, entidades, midias e monitoramento MQTT pela plataforma MobiCortex.

O repositorio tambem inclui a biblioteca/SDK .NET da MobiCortex usada pela aplicacao de exemplo. Voce pode usar, copiar, modificar e distribuir livremente essa SDK em projetos de clientes sob a licenca MIT, desde que a integracao seja utilizada com dispositivos MobiCortex.

## Arquitetura

O aplicativo usa uma arquitetura de launcher com multiplos formularios:

- MainForm - Tela de conexao (IP + senha) e launcher dos formularios de demonstracao.
- FormCadastroCompleto - Modelo completo de 3 niveis: Cadastro Central -> Entidade -> Midia.
- FormCadastroSimples - Modelo simplificado de 2 niveis: Entidade -> Midia (`createid=true`).
- FormMonitoramento - Recebimento de eventos em tempo real via MQTT.
- FormDashboard - Informacoes do dispositivo, uptime e estatisticas.
- FormSaidas - Lista dispositivos e comanda reles / DOUT.

Todos os formularios compartilham uma unica instancia de `MobiCortexClient` (`IMobiCortexClient`, em `../MobiCortexSdkLibCsharp`).

## Funcionalidades

### Cadastro Completo (3 niveis)
- CRUD de Cadastros Centrais com paginacao (20 itens por pagina)
- CRUD de Entidades vinculadas a um cadastro
- CRUD de Midias vinculadas a uma entidade
- Busca por ID (numerico) ou por nome (texto)

### Cadastro Simplificado (2 niveis)
- Criacao de entidades com `createid=true`, permitindo que o controlador gere os IDs automaticamente
- Nao e necessario criar cadastro central previamente
- CRUD de Midias vinculadas a uma entidade
- Busca por `entity_id` ou por nome, com paginacao

### Monitoramento (MQTT)
- Conexao MQTT TCP no listener de export da controladora (porta **1884**, mesmo usuario/senha da tela)
- Topico `mbcortex/export/event` (acesso + LPR). A porta 1883 na controladora e IPC em loopback.
- Credenciais de demo: `mqttuser` / `mqttpass`. **Subscribe** conecta e assina se ainda nao estiver conectado.
- Eventos aparecem em grade (Hora, Evento, Placa, Registered, Topico, Payload). Duplo clique abre o JSON completo. O log embaixo fica para conexao/status.
- O formulario de broker embutido e o caminho **outbound** (a controladora publica neste PC).

### Servidor de webhook
- Escuta em `0.0.0.0` (LAN). Grave na controladora `http://<IP_LAN_DESTE_PC>:<porta>/webhook` — nao use localhost.
- Prefira a porta **9099** se a **8080** estiver ocupada (`filesync-win64`). Use **Allow Windows Firewall** se a placa der `Connection timed out`.
- Duplo clique na linha (ou View Details) abre o JSON completo do evento.

### Dashboard
- Informacoes do dispositivo (modelo, versao de firmware, MAC)
- Estatisticas de cadastros e entidades

### Saidas (DOUT / reles)
- Lista dispositivos via `GET /devices` (SMART, RS485, controladora, externos)
- Comanda saidas via `POST /devices/relay`: pulso, on, off, toggle
- Respeita os `cmd` que cada saida aceita (ex.: DOUT3 so pulso)

## Requisitos

- Windows 10 ou superior
- .NET 8.0 SDK/Runtime
- Acesso a rede onde o controlador MobiCortex esta instalado

## Como Usar

1. Execute o aplicativo `SmartSdk.exe`.
2. Informe o IP do controlador, por exemplo `192.168.120.45`.
3. Informe a senha (padrao: `admin`).
4. Clique em **Conectar**.
5. Apos conectar, abra qualquer formulario de demonstracao pelo launcher principal.

## Estrutura do Projeto

```text
SmartSdk/
|-- Forms/
|   |-- FormCadastroCompleto.cs      # cadastro completo: central + pessoas + veiculos + midias
|   |-- FormCadastroSimples.cs       # fluxo simplificado (createid=true)
|   |-- FormCadastroCentral/Entidade/Pessoa/PessoaEdit/Veiculo/Midia.cs, FormDetalheMidia.cs,
|   |   FormSelecionarTipoEntidade.cs  # dialogos usados pelas telas de cadastro
|   |-- FormDashboard.cs             # /dashboard + /device-info
|   |-- FormMonitoramento.cs         # eventos MQTT TCP 1884 em grade
|   |-- FormMqttCliente.cs           # MQTT client + /mqtt-export/user
|   |-- FormMqttBroker.cs            # broker MQTT local
|   |-- FormWebhookServer.cs         # receptor de webhook na LAN
|   `-- FormSaidas.cs                # /devices + /devices/relay
|-- MainForm.cs / .Designer.cs
|-- Program.cs
|-- SmartSdk.csproj                  # referencia ../MobiCortexSdkLibCsharp/MobiCortex.Sdk.csproj
|-- README.md
`-- README-ptbr.md
```

## Visao Geral da API REST

O servico se comunica com a API REST do controlador via HTTPS (porta 4449, certificado autoassinado).

Prefixo base das rotas: `/mbcortex/master/api/v1`

### Autenticacao
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| POST | `/login` | Login com senha e retorno de `session_key` (Bearer token, TTL de 900s) |
| DELETE | `/login` | Encerra a sessao atual |
| PUT | `/login` | Altera a senha (`pass_atual`, `pass_nova`, `pass_nova2`) |

### Cadastro Central
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| GET | `/central-registry?offset=0&count=20` | Lista cadastros com paginacao |
| GET | `/central-registry?id={id}` | Busca cadastro por ID |
| GET | `/central-registry?name={filter}` | Filtra cadastros por nome |
| POST | `/central-registry` | Cria ou atualiza cadastro |
| DELETE | `/central-registry?id={id}` | Remove cadastro |

### Entidades
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| GET | `/entities?id={entity_id}` | Busca entidade por ID |
| GET | `/entities?central_registry_id={id}` | Lista entidades de um cadastro |
| POST | `/entities` | Cria entidade, incluindo suporte a `createid=true` |
| PUT | `/entities?id={entity_id}` | Atualiza entidade |
| DELETE | `/entities?id={entity_id}` | Remove entidade e as midias relacionadas |

### Midias
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| GET | `/media?entity_id={id}` | Lista midias de uma entidade |
| GET | `/media?id={media_id}` | Busca midia por ID |
| POST | `/media` | Cria midia (RFID, placa, facial e outros formatos) |
| DELETE | `/media?id={media_id}` | Remove midia |

### Dashboard
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| GET | `/dashboard` | Estatisticas do dispositivo |
| GET | `/device-info` | Informacoes de hardware e firmware |

### Dispositivos / saidas (rele e DOUT)
| Metodo | Endpoint | Descricao |
|--------|----------|-----------|
| GET | `/devices` | Lista dispositivos e saidas acionaveis |
| POST | `/devices/relay` | Pulso ou latch em rele/DOUT (por `id` ou `nome`) |

## Exemplos de Codigo

### Criar entidade no fluxo simplificado (`createid=true`)

```csharp
var request = new CreateEntityRequest
{
    CreateId = true,
    Type = (int)EntityType.Person,
    Name = "Joao Silva",
    Doc = "123.456.789-00"
};

var result = await client.Entities.CreateAsync(request);
if (result.Success && result.Data?.Ret == 0)
{
    Console.WriteLine($"entity_id={result.Data.EntityId}, central_registry_id={result.Data.CentralRegistryId}");
}
```

### Criar entidade no fluxo completo (informando `central_registry_id`)

```csharp
var request = new CreateEntityRequest
{
    CentralRegistryId = 42,
    Type = (int)EntityType.Vehicle,
    Brand = "Honda",
    Model = "Civic",
    Color = "Preta",
    Doc = "ABC1D23",
    LprEnabled = true
};

var result = await client.Entities.CreateAsync(request);
```

### Listar entidades com paginacao

```csharp
var cadastros = await client.Registries.ListAsync(offset: 0, count: 20, nameFilter: "Joao");

foreach (var cad in cadastros.Data.Items)
{
    var entidades = await client.Entities.ListByRegistryAsync(cad.Id);
    foreach (var ent in entidades.Data.Items)
        Console.WriteLine($"  {ent.EntityId} - {ent.Name} ({ent.Doc})");
}
```

### Acionar rele / DOUT

```csharp
// Lista dispositivos e saidas
var devices = await client.Devices.ListAsync();
foreach (var d in devices.Data!.Items)
    Console.WriteLine($"{d.Id} {d.Nome} ({d.Tipo}) — {d.Saidas.Count} saida(s)");

// Pulso de 1 s no rele 1 do device "42"
var pulse = await client.Devices.TriggerAsync(
    DeviceRelayRequest.PulseRelay("42", relay: 1, timeMs: 1000));

// Pulso no DOUT 3 (fechadura) da mesma SMART
var dout = await client.Devices.TriggerAsync(
    DeviceRelayRequest.PulseDout("42", dout: 3, timeMs: 500));

// Latch on/off (quando a saida aceitar cmd on/off)
var on = await client.Devices.TriggerAsync(
    DeviceRelayRequest.Latch("42", relay: 1, dout: null, cmd: "on"));
```

### Criar midia RFID

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

### Criar midia LPR (placa de veiculo)

Importante: o backend valida automaticamente o formato da midia. Para LPR, envie `ns32_0` e `ns32_1` para evitar que a placa seja validada como se fosse um dado RFID.

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

Abordagem recomendada: usar `lpr_enabled=true` ao criar ou atualizar a entidade do veiculo. O backend cria ou atualiza a midia LPR automaticamente.

```csharp
var request = new CreateEntityRequest
{
    CentralRegistryId = 42,
    Type = (int)EntityType.Vehicle,
    Brand = "Honda",
    Model = "Civic",
    Color = "Preta",
    Doc = "ABC1D23",
    LprEnabled = true
};
```

## Tipos de Midia Suportados

| Constante | Valor | Formato |
|-----------|-------|---------|
| `Wiegand26` | 1 | `Facility,Card` (exemplo: `123,45678`) |
| `Wiegand34` | 2 | `Facility,Card` |
| `Lpr` | 17 | Placa de veiculo (exemplo: `ABC1D23`) |
| `Facial` | 20 | Imagem facial (base64) |

## Compilacao

```bash
dotnet build SmartSdk.csproj
```

Ou use o script:

```bash
build.bat
```

## Execucao

```bash
dotnet run
```

Ou execute o binario diretamente:

```bash
bin\Debug\net8.0-windows\SmartSdk.exe
```

## Changelog

Consulte [CHANGELOG-ptbr.md](CHANGELOG-ptbr.md) (pt-BR) e [CHANGELOG.md](CHANGELOG.md) (English).

## Licenca

Este projeto esta licenciado sob a Licenca MIT.

O codigo de exemplo e a SDK/biblioteca incluida podem ser usados livremente em aplicacoes de clientes sob a Licenca MIT, desde que a integracao seja destinada a dispositivos MobiCortex.

Para o texto completo da licenca, consulte `LICENSE`.



