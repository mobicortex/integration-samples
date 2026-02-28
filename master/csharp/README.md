# Smart SDK - Sistema de Cadastro para Controladora RXPPRO

Aplicação Windows Forms para gerenciamento de cadastros na controladora RXPPRO.

## Funcionalidades

### Cadastro de Entidades (Usuários)
- Listar todas as entidades cadastradas
- Cadastrar nova entidade
- Excluir entidade
- Buscar entidades por nome ou ID

### Cadastro de Placas/RFID
- Listar mídias por entidade
- Cadastrar placa de veículo
- Cadastrar RFID (Wiegand, XCODE ou 10 dígitos)
- Excluir mídia

## Requisitos

- Windows 10 ou superior
- .NET 8.0 Runtime
- Acesso à rede onde a controladora RXPPRO está instalada

## Como Usar

1. Execute o aplicativo `SmartSdk.exe`
2. Configure a URL do servidor (ex: `http://192.168.1.1`)
3. Informe usuário e senha (padrão: admin/admin)
4. Clique em "Conectar"
5. Navegue entre as abas para gerenciar entidades e mídias

## Estrutura do Projeto

```
SmartSdk/
├── Controls/
│   ├── IConnectionAware.cs    # Interface para controles
│   ├── UsersControl.cs        # Controle de cadastro de entidades
│   └── MediasControl.cs       # Controle de cadastro de placas/RFID
├── Models/
│   ├── User.cs                # Modelo de usuário/entidade
│   └── Media.cs               # Modelo de mídia (placa/RFID)
├── Services/
│   └── ApiService.cs          # Serviço de comunicação com API
├── MainForm.cs                # Formulário principal
├── Program.cs                 # Ponto de entrada
└── README.md                  # Este arquivo
```

## APIs Utilizadas

O sistema se comunica com a controladora RXPPRO através dos seguintes endpoints:

### Autenticação
- `POST /master/api/v1/login` - Login

### Usuários
- `GET /master/api/v1/users` - Lista usuários
- `GET /master/api/v1/users?id={id}` - Busca usuário por ID
- `POST /master/api/v1/users` - Cria usuário
- `DELETE /master/api/v1/users?id={id}` - Remove usuário

### Mídias
- `GET /master/api/v1/medias?user_id={id}` - Lista mídias por usuário
- `GET /master/api/v1/medias?plate={placa}` - Busca mídia por placa
- `POST /master/api/v1/medias` - Cria mídia (placa ou RFID)
- `DELETE /master/api/v1/medias?id={id}` - Remove mídia

## Formatos de RFID Suportados

1. **Wiegand**: `Facility,Card` (ex: `123,45678`)
2. **XCODE**: `Empresa,Facility,Card` (ex: `12345,123,45678`)
3. **10 Dígitos**: Número com 10 dígitos (ex: `1234567890`)

## Compilação

```bash
dotnet build SmartSdk.csproj
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