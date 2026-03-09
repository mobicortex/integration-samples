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

Todos os formularios compartilham uma unica instancia de `MobiCortexApiService`.

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
- Conexao MQTT ao controlador (porta 1883)
- Recebimento de eventos de acesso em tempo real
- Exibicao formatada de eventos no console

### Dashboard
- Informacoes do dispositivo (modelo, versao de firmware, MAC)
- Estatisticas de cadastros e entidades

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
| POST | `/logout` | Encerra a sessao atual |
| PUT | `/password` | Altera a senha do dispositivo |

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
| GET | `/entities?cadastro_id={id}` | Lista entidades de um cadastro |
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

## Exemplos de Codigo

### Criar entidade no fluxo simplificado (`createid=true`)

```csharp
var request = new CriarEntidadeRequest
{
    CreateId = true,
    Tipo = (int)TipoEntidade.Pessoa,
    Name = "Joao Silva",
    Doc = "123.456.789-00"
};

var result = await _api.CriarEntidadeAsync(request);
if (result.Success && result.Data?.Ret == 0)
{
    Console.WriteLine($"entity_id={result.Data.EntityId}, cadastro_id={result.Data.CadastroId}");
}
```

### Criar entidade no fluxo completo (informando `cadastro_id`)

```csharp
var request = new CriarEntidadeRequest
{
    CadastroId = 42,
    Tipo = (int)TipoEntidade.Veiculo,
    Name = "Civic Preto",
    Doc = "ABC1D23",
    LprAtivo = 1
};

var result = await _api.CriarEntidadeAsync(request);
```

### Listar entidades com paginacao

```csharp
var cadastros = await _api.ListarCadastrosAsync(offset: 0, count: 20, filtroNome: "Joao");

foreach (var cad in cadastros.Data.Items)
{
    var entidades = await _api.ListarEntidadesAsync(cad.Id);
    foreach (var ent in entidades.Data.Items)
        Console.WriteLine($"  {ent.EntityId} - {ent.Name} ({ent.Doc})");
}
```

### Criar midia RFID

```csharp
var request = new CriarMidiaRequest
{
    EntityId = 4294000123,
    CadastroId = 42,
    Tipo = TipoMidia.Wiegand26,
    Descricao = "123,45678"
};

var result = await _api.CriarMidiaAsync(request);
```

### Criar midia LPR (placa de veiculo)

Importante: o backend valida automaticamente o formato da midia. Para LPR, envie `ns32_0` e `ns32_1` para evitar que a placa seja validada como se fosse um dado RFID.

```csharp
var request = new CriarMidiaRequest
{
    EntityId = 4294000123,
    CadastroId = 42,
    Tipo = TipoMidia.Lpr,
    Descricao = "ABC1D23",
    Ns32_0 = 0,
    Ns32_1 = 0
};

var result = await _api.CriarMidiaAsync(request);
```

Abordagem recomendada: usar `lpr_ativo=true` no cadastro da entidade de veiculo. O backend converte automaticamente a placa para o formato binario exigido.

```csharp
var request = new CriarEntidadeRequest
{
    CadastroId = 42,
    Tipo = (int)TipoEntidade.Veiculo,
    Name = "Civic Preto",
    Doc = "ABC1D23",
    LprAtivo = true
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
