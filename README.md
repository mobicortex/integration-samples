# MobiCortex Integration Samples

This directory contains integration samples and technical references for the MobiCortex ecosystem.

## Available Content

### [`/master`](./master)
Resources related to the **Master** product line, including API documentation and language-specific examples.

Main subdirectories:

- [`/master/docs`](./master/docs)
  Functional and technical documentation for the Master API.

  Important files:
  - [`MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md): endpoint reference.
  - [`MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json): Postman collection for API testing.

- [`/master/csharp`](./master/csharp)
  Full **C# / .NET** sample, including a Windows Forms application, SDK, and dedicated documentation.

  Important files:
  - [`README.md`](./master/csharp/README.md): documentation in English.
  - [`README-ptbr.md`](./master/csharp/README-ptbr.md): documentation in Brazilian Portuguese.
  - [`CHANGELOG.md`](./master/csharp/CHANGELOG.md): changelog in English.
  - [`CHANGELOG-ptbr.md`](./master/csharp/CHANGELOG-ptbr.md): changelog in Brazilian Portuguese.
  - [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md): SDK library details.

- [`/master/csharp.net`](./master/csharp.net)
  **C# / .NET** integration example with WinForms application and reusable SDK library.

  Important files:
  - [`README.md`](./master/csharp.net/README.md): documentation in English.
  - [`README-ptbr.md`](./master/csharp.net/README-ptbr.md): documentation in Brazilian Portuguese.
  - [`CHANGELOG.md`](./master/csharp.net/CHANGELOG.md): changelog in English.
  - [`CHANGELOG-ptbr.md`](./master/csharp.net/CHANGELOG-ptbr.md): changelog in Brazilian Portuguese.
  - [`MobiCortexIntegration.sln`](./master/csharp.net/MobiCortexIntegration.sln): Visual Studio solution.

- [`/master/nodejs`](./master/nodejs)
  **Node.js** integration sample with client code, usage examples, and supporting files.

  Important files:
  - [`README.md`](./master/nodejs/README.md): usage instructions.
  - [`TESTE.md`](./master/nodejs/TESTE.md): test notes and examples.
  - [`/examples`](./master/nodejs/examples): runnable examples.
  - [`/src`](./master/nodejs/src): client and model implementation.

- [`/master/python`](./master/python)
  **Python** integration sample with demo script, configuration, and models.

  Important files:
  - [`README.md`](./master/python/README.md): usage instructions.
  - [`mbcortex_demo.py`](./master/python/mbcortex_demo.py): main sample script.
  - [`mbcortex_config.json`](./master/python/mbcortex_config.json): environment configuration.
  - [`/src`](./master/python/src): models and exceptions.

## Detailed Path Map

Below is the current path-by-path inventory of the examples and documentation available in this directory.

### 1. Master API documentation

Base folder:

- [`/master/docs`](./master/docs)

Files:

- [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
  Main functional and technical reference for the Master REST API.

- [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)
  Postman collection for validating the endpoints described in the API documentation.

Recommended use:

1. Read [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
2. Import [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)
3. Then move to the language-specific sample you want to use

### 2. C# / .NET / Windows Forms example

Base folder:

- [`/master/csharp`](./master/csharp)

Main files:

- [`/master/csharp/README.md`](./master/csharp/README.md)
  English documentation for the C# sample.

- [`/master/csharp/README-ptbr.md`](./master/csharp/README-ptbr.md)
  Brazilian Portuguese documentation for the C# sample.

- [`/master/csharp/CHANGELOG.md`](./master/csharp/CHANGELOG.md)
  English changelog for the C# sample.

- [`/master/csharp/CHANGELOG-ptbr.md`](./master/csharp/CHANGELOG-ptbr.md)
  Brazilian Portuguese changelog for the C# sample.

- [`/master/csharp/SmartSdk.csproj`](./master/csharp/SmartSdk.csproj)
  Main project file.

- [`/master/csharp/MobiCortexIntegration.sln`](./master/csharp/MobiCortexIntegration.sln)
  Visual Studio solution for the sample application and SDK.

- [`/master/csharp/Program.cs`](./master/csharp/Program.cs)
  Application entry point.

- [`/master/csharp/MainForm.cs`](./master/csharp/MainForm.cs)
  Main launcher form.

Important subfolders:

- [`/master/csharp/Forms`](./master/csharp/Forms)
  WinForms screens for registry, entity, media, dashboard, MQTT, webhook, and monitoring flows.

- [`/master/csharp/MobiCortexSdkLib`](./master/csharp/MobiCortexSdkLib)
  Reusable .NET SDK library used by the sample app.

Important SDK files:

- [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md)
  SDK-specific documentation.

- [`/master/csharp/MobiCortexSdkLib/MobiCortex.Sdk.csproj`](./master/csharp/MobiCortexSdkLib/MobiCortex.Sdk.csproj)
  SDK project file.

- [`/master/csharp/MobiCortexSdkLib/Services/MobiCortexClient.cs`](./master/csharp/MobiCortexSdkLib/Services/MobiCortexClient.cs)
  Main HTTP/API client implementation.

- [`/master/csharp/MobiCortexSdkLib/Models/MobiCortexModels.cs`](./master/csharp/MobiCortexSdkLib/Models/MobiCortexModels.cs)
  DTOs and models used by the SDK.

- [`/master/csharp/MobiCortexSdkLib/Interfaces`](./master/csharp/MobiCortexSdkLib/Interfaces)
  Service interfaces.

- [`/master/csharp/MobiCortexSdkLib/Exceptions`](./master/csharp/MobiCortexSdkLib/Exceptions)
  Custom exception types.

Technology summary:

- Language: C#
- Runtime: .NET 8
- UI: Windows Forms
- Integration types: REST API, MQTT, webhook/server helpers in the sample structure

### 3. C# / .NET example (csharp.net)

Base folder:

- [`/master/csharp.net`](./master/csharp.net)

Main files:

- [`/master/csharp.net/README.md`](./master/csharp.net/README.md)
  English documentation for the C# sample.

- [`/master/csharp.net/README-ptbr.md`](./master/csharp.net/README-ptbr.md)
  Brazilian Portuguese documentation for the C# sample.

- [`/master/csharp.net/CHANGELOG.md`](./master/csharp.net/CHANGELOG.md)
  English changelog for the C# sample.

- [`/master/csharp.net/CHANGELOG-ptbr.md`](./master/csharp.net/CHANGELOG-ptbr.md)
  Brazilian Portuguese changelog for the C# sample.

- [`/master/csharp.net/SmartSdk.csproj`](./master/csharp.net/SmartSdk.csproj)
  Main project file.

- [`/master/csharp.net/MobiCortexIntegration.sln`](./master/csharp.net/MobiCortexIntegration.sln)
  Visual Studio solution for the sample application and SDK.

- [`/master/csharp.net/Program.cs`](./master/csharp.net/Program.cs)
  Application entry point.

- [`/master/csharp.net/MainForm.cs`](./master/csharp.net/MainForm.cs)
  Main launcher form.

Important subfolders:

- [`/master/csharp.net/Forms`](./master/csharp.net/Forms)
  WinForms screens for registry, entity, media, dashboard, MQTT, webhook, and monitoring flows.

- [`/master/csharp.net/MobiCortexSdkLib`](./master/csharp.net/MobiCortexSdkLib)
  Reusable .NET SDK library used by the sample app.

Important SDK files:

- [`/master/csharp.net/MobiCortexSdkLib/README.md`](./master/csharp.net/MobiCortexSdkLib/README.md)
  SDK-specific documentation.

- [`/master/csharp.net/MobiCortexSdkLib/MobiCortex.Sdk.csproj`](./master/csharp.net/MobiCortexSdkLib/MobiCortex.Sdk.csproj)
  SDK project file.

- [`/master/csharp.net/MobiCortexSdkLib/Services/MobiCortexClient.cs`](./master/csharp.net/MobiCortexSdkLib/Services/MobiCortexClient.cs)
  Main HTTP/API client implementation.

- [`/master/csharp.net/MobiCortexSdkLib/Models/MobiCortexModels.cs`](./master/csharp.net/MobiCortexSdkLib/Models/MobiCortexModels.cs)
  DTOs and models used by the SDK.

Technology summary:

- Language: C#
- Runtime: .NET 8
- UI: Windows Forms
- Integration types: REST API, MQTT, webhook/server helpers

### 4. Node.js example

Base folder:

- [`/master/nodejs`](./master/nodejs)

Main files:

- [`/master/nodejs/README.md`](./master/nodejs/README.md)
  Main Node.js documentation.

- [`/master/nodejs/package.json`](./master/nodejs/package.json)
  Script definitions and package metadata.

- [`/master/nodejs/main.js`](./master/nodejs/main.js)
  Legacy/deprecated entry point kept in the repository.

- [`/master/nodejs/TESTE.md`](./master/nodejs/TESTE.md)
  Additional test notes.

Important subfolders:

- [`/master/nodejs/src`](./master/nodejs/src)
  Reusable library code.

- [`/master/nodejs/examples`](./master/nodejs/examples)
  Runnable examples and CLI files.

Important library files:

- [`/master/nodejs/src/index.js`](./master/nodejs/src/index.js)
  Package exports.

- [`/master/nodejs/src/mbcortexClient.js`](./master/nodejs/src/mbcortexClient.js)
  Main API client implementation.

- [`/master/nodejs/src/models.js`](./master/nodejs/src/models.js)
  Model definitions.

- [`/master/nodejs/src/exceptions.js`](./master/nodejs/src/exceptions.js)
  Custom exceptions and error handling types.

Important example files:

- [`/master/nodejs/examples/cli.js`](./master/nodejs/examples/cli.js)
  Interactive CLI example.

- [`/master/nodejs/examples/basic_usage.js`](./master/nodejs/examples/basic_usage.js)
  Basic library usage example.

- [`/master/nodejs/examples/cli-config.json`](./master/nodejs/examples/cli-config.json)
  Saved CLI configuration example file.

Technology summary:

- Language: JavaScript
- Runtime: Node.js
- Integration types: REST API, CLI-based test flow

### 5. Python example

Base folder:

- [`/master/python`](./master/python)

Main files:

- [`/master/python/README.md`](./master/python/README.md)
  Main Python documentation.

- [`/master/python/mbcortex_demo.py`](./master/python/mbcortex_demo.py)
  Main interactive demo and reusable client definitions.

- [`/master/python/mbcortex_config.json`](./master/python/mbcortex_config.json)
  Saved runtime configuration for the Python demo.

- [`/master/python/pyproject.toml`](./master/python/pyproject.toml)
  Project metadata.

Important subfolders:

- [`/master/python/src`](./master/python/src)
  Supporting Python modules.

Important module files:

- [`/master/python/src/models.py`](./master/python/src/models.py)
  Model definitions.

- [`/master/python/src/exceptions.py`](./master/python/src/exceptions.py)
  Custom exception definitions.

Technology summary:

- Language: Python
- Runtime: Python 3
- Integration types: REST API, interactive terminal demo

## Documentation Map

- General Master API documentation:
  [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)

- Postman collection:
  [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json)

- C# documentation:
  [`/master/csharp/README.md`](./master/csharp/README.md)
  [`/master/csharp/README-ptbr.md`](./master/csharp/README-ptbr.md)
  [`/master/csharp/MobiCortexSdkLib/README.md`](./master/csharp/MobiCortexSdkLib/README.md)

- C# / .NET (csharp.net) documentation:
  [`/master/csharp.net/README.md`](./master/csharp.net/README.md)
  [`/master/csharp.net/README-ptbr.md`](./master/csharp.net/README-ptbr.md)
  [`/master/csharp.net/MobiCortexSdkLib/README.md`](./master/csharp.net/MobiCortexSdkLib/README.md)

- Node.js documentation:
  [`/master/nodejs/README.md`](./master/nodejs/README.md)

- Python documentation:
  [`/master/python/README.md`](./master/python/README.md)

## Recommended Starting Point

If you are starting from scratch, the simplest sequence is:

1. Read [`/master/docs/MobiCortex-Master-Endpoints.md`](./master/docs/MobiCortex-Master-Endpoints.md)
2. Import [`/master/docs/MobiCortex-Master-Endpoints-Postman.json`](./master/docs/MobiCortex-Master-Endpoints-Postman.json) into Postman
3. Choose your target language in [`/master/csharp`](./master/csharp), [`/master/csharp.net`](./master/csharp.net), [`/master/nodejs`](./master/nodejs), or [`/master/python`](./master/python)
4. Open the `README.md` for the selected language

## Language Versions

- English: `README.md`
- Brazilian Portuguese: `README-ptbr.md`
