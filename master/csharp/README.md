# SmartSdk — Exemplo de Integração MobiCortex (C# / WinForms)

Aplicação Windows Forms que demonstra a integração com a API REST do controlador MobiCortex.
O projeto é um **exemplo de referência** para integradores que desejam gerenciar cadastros, entidades, mídias, monitoramento MQTT e configuração de rede via API.

## Arquitetura

O aplicativo usa uma arquitetura **multi-form launcher**:

- **MainForm** — Tela de conexão (IP + senha) e launcher dos formulários de demonstração.
- **FormCadastroCompleto** — Modelo completo de 3 níveis: Cadastro Central → Entidade → Mídia.
- **FormCadastroSimplificado** — Modelo simplificado de 2 níveis: Entidade → Mídia (`createid=true`).
- **FormMonitoramento** — Recebimento de eventos em tempo real via MQTT.
- **FormRede** — Leitura e alteração da configuração de rede do controlador.
- **FormDashboard** — Informações do dispositivo (versão, uptime, estatísticas).

Todos os formulários compartilham uma instância única de `MobiCortexApiService`.

## Funcionalidades

### Cadastro Completo (3 níveis)
- CRUD de Cadastros Centrais com paginação (20 itens/página)
- CRUD de Entidades vinculadas a um cadastro
- CRUD de Mídias vinculadas a uma entidade
- Busca por ID (numérico) ou por nome (texto)

### Cadastro Simplificado (2 níveis)
- Criação de entidades com `createid=true` — o controlador gera IDs automaticamente
- Não é necessário criar cadastro central previamente
- CRUD de Mídias vinculadas a uma entidade
- Busca por entity_id ou por nome, com paginação

### Monitoramento (MQTT)
- Conexão MQTT ao controlador (porta 1883)
- Recebimento de eventos de acesso em tempo real
- Exibição de eventos formatados em console

### Rede
- Leitura da configuração de rede atual (IP, máscara, gateway, DNS, DHCP)
- Alteração e envio de nova configuração

### Dashboard
- Informações do dispositivo (modelo, versão de firmware, MAC)
- Estatísticas de cadastros e entidades

## Requisitos

- Windows 10 ou superior
- .NET 8.0 SDK/Runtime
- Acesso à rede onde o controlador MobiCortex está instalado

## Como Usar

1. Execute o aplicativo `SmartSdk.exe`
2. Informe o IP do controlador (ex: `192.168.120.45`)
3. Informe a senha (padrão: `admin`)
4. Clique em **Conectar**
5. Após conectar, clique em qualquer botão para abrir o formulário de demonstração

## Estrutura do Projeto

```
SmartSdk/
├── Forms/
│   ├── FormCadastroCompleto.cs/.Designer.cs   # Modelo completo (3 níveis)
│   ├── FormCadastroSimples.cs/.Designer.cs    # Modelo simplificado (2 níveis)
│   ├── FormMonitoramento.cs/.Designer.cs      # Eventos MQTT em tempo real
│   ├── FormRede.cs/.Designer.cs               # Configuração de rede
│   └── FormDashboard.cs/.Designer.cs          # Informações do dispositivo
├── Models/
│   └── MobiCortexModels.cs    # Todos os modelos (request/response/entidades)
├── Services/
│   └── MobiCortexApiService.cs # Serviço HTTP + autenticação + helpers
├── MainForm.cs / .Designer.cs # Launcher principal
├── Program.cs                 # Ponto de entrada
├── SmartSdk.csproj            # Projeto .NET 8.0 (WinForms + MQTTnet)
└── README.md                  # Este arquivo
```

## API REST

O serviço se comunica com a API REST do controlador via HTTPS (porta 4449, certificado auto-assinado).

Prefixo de todas as rotas: `/mbcortex/master/api/v1`

### Autenticação
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/login` | Login com senha → retorna `session_key` (Bearer token, 900s TTL) |
| POST | `/logout` | Encerra a sessão |
| PUT | `/password` | Altera a senha do dispositivo |

### Cadastro Central
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/central-registry?offset=0&count=20` | Lista cadastros paginados |
| GET | `/central-registry?id={id}` | Busca cadastro por ID |
| GET | `/central-registry?name={filtro}` | Filtra cadastros por nome |
| POST | `/central-registry` | Cria ou atualiza cadastro |
| DELETE | `/central-registry?id={id}` | Remove cadastro |

### Entidades
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/entities?id={entity_id}` | Busca entidade por ID |
| GET | `/entities?cadastro_id={id}` | Lista entidades de um cadastro |
| POST | `/entities` | Cria entidade (com suporte a `createid=true`) |
| PUT | `/entities?id={entity_id}` | Atualiza entidade |
| DELETE | `/entities?id={entity_id}` | Remove entidade e mídias vinculadas |

### Mídias
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/media?entity_id={id}` | Lista mídias de uma entidade |
| GET | `/media?id={media_id}` | Busca mídia por ID |
| POST | `/media` | Cria mídia (RFID, placa, facial, etc.) |
| DELETE | `/media?id={media_id}` | Remove mídia |

### Rede
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/network/cable` | Lê configuração de rede |
| POST | `/network/cable` | Altera configuração de rede |

### Dashboard
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/dashboard` | Estatísticas do dispositivo |
| GET | `/device-info` | Informações de hardware/firmware |

## Exemplos de Código

### Criar entidade no modelo simplificado (`createid=true`)

```csharp
var request = new CriarEntidadeRequest
{
    CreateId = true,     // Controlador gera entity_id e cadastro_id
    Tipo = (int)TipoEntidade.Pessoa,
    Name = "João Silva",
    Doc = "123.456.789-00"
};

var result = await _api.CriarEntidadeAsync(request);
if (result.Success && result.Data?.Ret == 0)
{
    Console.WriteLine($"entity_id={result.Data.EntityId}, cadastro_id={result.Data.CadastroId}");
}
```

### Criar entidade no modelo completo (informando cadastro_id)

```csharp
var request = new CriarEntidadeRequest
{
    CadastroId = 42,       // Cadastro central previamente criado
    Tipo = (int)TipoEntidade.Veiculo,
    Name = "Civic Preto",
    Doc = "ABC1D23",
    LprAtivo = 1           // Cria mídia de placa automaticamente
};

var result = await _api.CriarEntidadeAsync(request);
```

### Listar entidades com paginação

```csharp
// Listar cadastros centrais (paginado, com filtro opcional por nome)
var cadastros = await _api.ListarCadastrosAsync(offset: 0, count: 20, filtroNome: "João");

// Para cada cadastro, listar suas entidades
foreach (var cad in cadastros.Data.Items)
{
    var entidades = await _api.ListarEntidadesAsync(cad.Id);
    foreach (var ent in entidades.Data.Items)
        Console.WriteLine($"  {ent.EntityId} - {ent.Name} ({ent.Doc})");
}
```

### Criar mídia RFID

```csharp
var request = new CriarMidiaRequest
{
    EntityId = 4294000123,
    CadastroId = 42,
    Tipo = TipoMidia.Wiegand26,
    Descricao = "123,45678"     // Facility,Card
};

var result = await _api.CriarMidiaAsync(request);
```

## Tipos de Mídia Suportados

| Constante | Valor | Formato |
|-----------|-------|---------|
| `Wiegand26` | 1 | `Facility,Card` (ex: `123,45678`) |
| `Wiegand34` | 2 | `Facility,Card` |
| `Lpr` | 10 | Placa do veículo (ex: `ABC1D23`) |
| `Facial` | 20 | Imagem facial (base64) |

## Compilação

```bash
dotnet build SmartSdk.csproj
```

Ou use o script:
```bash
build.bat
```

## Execução

```bash
dotnet run
```

Ou execute diretamente:
```bash
bin\Debug\net8.0-windows\SmartSdk.exe
```

## Licença

Copyright (c) 2024 Plataforma MobiCortex. Todos os direitos reservados.