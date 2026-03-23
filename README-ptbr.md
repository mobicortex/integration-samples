# MobiCortex Integration Samples

Este diretorio concentra exemplos de integracao e referencias tecnicas do ecossistema MobiCortex.

## Conteudo Disponivel

### [`/master`](./master)
Recursos relacionados a linha **Master**, incluindo documentacao da API e exemplos por linguagem.

Principais subpastas:

- [`/master/docs`](./master/docs)
  Documentacao funcional e tecnica da API Master.

  Arquivos importantes:
  - [`MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md): referencia dos endpoints.
  - [`MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json): collection do Postman para testes da API.

- [`/master/csharp`](./master/csharp)
  Exemplo completo em **C# / .NET**, incluindo aplicacao Windows Forms, SDK e documentacao propria.

  Arquivos importantes:
  - [`README.md`](./master/csharp/README.md): documentacao em ingles.
  - [`README-ptbr.md`](./master/csharp/README-ptbr.md): documentacao em portugues do Brasil.
  - [`CHANGELOG.md`](./master/csharp/CHANGELOG.md): historico de alteracoes em ingles.
  - [`CHANGELOG-ptbr.md`](./master/csharp/CHANGELOG-ptbr.md): historico de alteracoes em portugues do Brasil.
  - [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md): detalhes da biblioteca SDK.

- [`/master/nodejs`](./master/nodejs)
  Exemplo de integracao em **Node.js**, com client, exemplos de uso e arquivos auxiliares.

  Arquivos importantes:
  - [`README.md`](./master/nodejs/README.md): instrucoes de uso.
  - [`TESTE.md`](./master/nodejs/TESTE.md): notas e exemplos de teste.
  - [`/examples`](./master/nodejs/examples): exemplos prontos para execucao.
  - [`/src`](./master/nodejs/src): implementacao do client e dos modelos.

- [`/master/python`](./master/python)
  Exemplo de integracao em **Python**, com script de demonstracao, configuracao e modelos.

  Arquivos importantes:
  - [`README.md`](./master/python/README.md): instrucoes de uso.
  - [`mbcortex_demo.py`](./master/python/mbcortex_demo.py): script principal de exemplo.
  - [`mbcortex_config.json`](./master/python/mbcortex_config.json): configuracao do ambiente.
  - [`/src`](./master/python/src): modelos e excecoes.

## Mapa Detalhado de Caminhos

Abaixo esta o inventario atual, caminho por caminho, dos exemplos e documentos disponiveis neste diretorio.

### 1. Documentacao da API Master

Pasta base:

- [`/master/docs`](./master/docs)

Arquivos:

- [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
  Referencia funcional e tecnica principal da API REST Master.

- [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)
  Collection do Postman para validar os endpoints descritos na documentacao da API.

Uso recomendado:

1. Ler [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
2. Importar [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)
3. Depois seguir para o exemplo da linguagem desejada

### 2. Exemplo em C# / .NET / Windows Forms

Pasta base:

- [`/master/csharp`](./master/csharp)

Arquivos principais:

- [`/master/csharp/README.md`](./master/csharp/README.md)
  Documentacao em ingles do exemplo C#.

- [`/master/csharp/README-ptbr.md`](./master/csharp/README-ptbr.md)
  Documentacao em portugues do Brasil do exemplo C#.

- [`/master/csharp/CHANGELOG.md`](./master/csharp/CHANGELOG.md)
  Historico de alteracoes em ingles do exemplo C#.

- [`/master/csharp/CHANGELOG-ptbr.md`](./master/csharp/CHANGELOG-ptbr.md)
  Historico de alteracoes em portugues do Brasil do exemplo C#.

- [`/master/csharp/SmartSdk.csproj`](./master/csharp/SmartSdk.csproj)
  Arquivo principal do projeto.

- [`/master/csharp/MobiCortexIntegration.sln`](./master/csharp/MobiCortexIntegration.sln)
  Solucao Visual Studio da aplicacao de exemplo e do SDK.

- [`/master/csharp/Program.cs`](./master/csharp/Program.cs)
  Ponto de entrada da aplicacao.

- [`/master/csharp/MainForm.cs`](./master/csharp/MainForm.cs)
  Formulario principal e launcher das demos.

Subpastas importantes:

- [`/master/csharp/Forms`](./master/csharp/Forms)
  Telas WinForms para fluxos de cadastro, entidade, midia, dashboard, MQTT, webhook e monitoramento.

- [`/master/csharp/MobiCortexSdkLib`](./master/csharp/MobiCortexSdkLib)
  Biblioteca SDK .NET reutilizavel usada pela aplicacao de exemplo.

Arquivos importantes do SDK:

- [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md)
  Documentacao especifica do SDK.

- [`/master/csharp/MobiCortexSdkLib/MobiCortex.Sdk.csproj`](./master/csharp/MobiCortexSdkLib/MobiCortex.Sdk.csproj)
  Arquivo do projeto da biblioteca SDK.

- [`/master/csharp/MobiCortexSdkLib/Services/MobiCortexClient.cs`](./master/csharp/MobiCortexSdkLib/Services/MobiCortexClient.cs)
  Implementacao principal do client HTTP/API.

- [`/master/csharp/MobiCortexSdkLib/Models/MobiCortexModels.cs`](./master/csharp/MobiCortexSdkLib/Models/MobiCortexModels.cs)
  DTOs e modelos usados pelo SDK.

- [`/master/csharp/MobiCortexSdkLib/Interfaces`](./master/csharp/MobiCortexSdkLib/Interfaces)
  Interfaces de servicos.

- [`/master/csharp/MobiCortexSdkLib/Exceptions`](./master/csharp/MobiCortexSdkLib/Exceptions)
  Tipos de excecao customizados.

Resumo da tecnologia:

- Linguagem: C#
- Runtime: .NET 8
- UI: Windows Forms
- Integracoes: REST API, MQTT, webhook/helpers de servidor presentes na estrutura da amostra

### 3. Exemplo em Node.js

Pasta base:

- [`/master/nodejs`](./master/nodejs)

Arquivos principais:

- [`/master/nodejs/README.md`](./master/nodejs/README.md)
  Documentacao principal do exemplo Node.js.

- [`/master/nodejs/package.json`](./master/nodejs/package.json)
  Definicao de scripts e metadados do pacote.

- [`/master/nodejs/main.js`](./master/nodejs/main.js)
  Ponto de entrada legado/deprecated mantido no repositorio.

- [`/master/nodejs/TESTE.md`](./master/nodejs/TESTE.md)
  Notas complementares de teste.

Subpastas importantes:

- [`/master/nodejs/src`](./master/nodejs/src)
  Codigo reutilizavel da biblioteca.

- [`/master/nodejs/examples`](./master/nodejs/examples)
  Exemplos executaveis e arquivos do CLI.

Arquivos importantes da biblioteca:

- [`/master/nodejs/src/index.js`](./master/nodejs/src/index.js)
  Exports do pacote.

- [`/master/nodejs/src/mbcortexClient.js`](./master/nodejs/src/mbcortexClient.js)
  Implementacao principal do client da API.

- [`/master/nodejs/src/models.js`](./master/nodejs/src/models.js)
  Definicao dos modelos.

- [`/master/nodejs/src/exceptions.js`](./master/nodejs/src/exceptions.js)
  Excecoes customizadas e tipos de erro.

Arquivos importantes de exemplo:

- [`/master/nodejs/examples/cli.js`](./master/nodejs/examples/cli.js)
  Exemplo de CLI interativo.

- [`/master/nodejs/examples/basic_usage.js`](./master/nodejs/examples/basic_usage.js)
  Exemplo basico de uso como biblioteca.

- [`/master/nodejs/examples/cli-config.json`](./master/nodejs/examples/cli-config.json)
  Arquivo de configuracao salvo pelo CLI.

Resumo da tecnologia:

- Linguagem: JavaScript
- Runtime: Node.js
- Integracoes: REST API, fluxo de testes via CLI

### 4. Exemplo em Python

Pasta base:

- [`/master/python`](./master/python)

Arquivos principais:

- [`/master/python/README.md`](./master/python/README.md)
  Documentacao principal do exemplo Python.

- [`/master/python/mbcortex_demo.py`](./master/python/mbcortex_demo.py)
  Demo interativa principal e definicoes reutilizaveis do client.

- [`/master/python/mbcortex_config.json`](./master/python/mbcortex_config.json)
  Configuracao salva em execucao para a demo Python.

- [`/master/python/pyproject.toml`](./master/python/pyproject.toml)
  Metadados do projeto.

Subpastas importantes:

- [`/master/python/src`](./master/python/src)
  Modulos auxiliares em Python.

Arquivos importantes dos modulos:

- [`/master/python/src/models.py`](./master/python/src/models.py)
  Definicao dos modelos.

- [`/master/python/src/exceptions.py`](./master/python/src/exceptions.py)
  Definicao de excecoes customizadas.

Resumo da tecnologia:

- Linguagem: Python
- Runtime: Python 3
- Integracoes: REST API, demo interativa em terminal

## Mapa da Documentacao

- Documentacao geral da API Master:
  [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)

- Collection do Postman:
  [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)

- Documentacao C#:
  [`/master/csharp/README.md`](./master/csharp/README.md)
  [`/master/csharp/README-ptbr.md`](./master/csharp/README-ptbr.md)
  [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md)

- Documentacao Node.js:
  [`/master/nodejs/README.md`](./master/nodejs/README.md)

- Documentacao Python:
  [`/master/python/README.md`](./master/python/README.md)

## Ponto de Partida Recomendado

Se voce estiver comecando do zero, a sequencia mais simples e:

1. Ler [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
2. Importar [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json) no Postman
3. Escolher a linguagem desejada em [`/master/csharp`](./master/csharp), [`/master/nodejs`](./master/nodejs) ou [`/master/python`](./master/python)
4. Abrir o `README.md` da linguagem escolhida

## Versoes do Documento

- English: `README.md`
- Português do Brasil: `README-ptbr.md`
