# MobiCortex Master - API Endpoints (RXPPRO)

Documentação completa dos endpoints da API da Controladora Master MobiCortex (RXPPRO).

## Base URL

```
https://<ip-da-controladora>:4449  (modo desktop/debug)
https://<ip-da-controladora>:443   (modo produção ARM)
http://<ip-da-controladora>:80     (redireciona para HTTPS)
```

## Base Path

```
/mbcortex/master/api/v1
```

## Autenticação

A API utiliza autenticação por session key. Para obter uma session key, use o endpoint de login.

### Header de Autenticação

```
Authorization: Bearer <session_key>
```

A session key é um token de 64 caracteres hexadecimais (SHA256) válido por 15 minutos (TTL = 900 segundos). A sessão é renovada automaticamente a cada requisição válida (sliding TTL).

> **💡 Dica Postman**: A collection Postman disponível neste repositório inclui um script automático que extrai a `session_key` do response de login e salva na variável `sessionKey`. Após fazer login, todas as requisições subsequentes usarão automaticamente o token salvo.

**Limites:**
- Máximo de 10 sessões simultâneas por dispositivo
- Máximo de 64 tokens de API por dispositivo

---

## Índice

1. [Endpoints de Autenticação](#endpoints-de-autenticação)
2. [Endpoints de Tokens de API](#endpoints-de-tokens-de-api)
3. [Endpoints de Configuração](#endpoints-de-configuração)
4. [Endpoints de Rede](#endpoints-de-rede)
5. [Endpoints de Central Registry](#endpoints-de-central-registry)
6. [Endpoints de Entidades](#endpoints-de-entidades)
7. [Endpoints de Mídias](#endpoints-de-mídias)
8. [Endpoints de Video Source](#endpoints-de-video-source)
9. [Endpoints de Webhook](#endpoints-de-webhook)
10. [Endpoints de Device Info](#endpoints-de-device-info)
11. [Endpoints de Dashboard](#endpoints-de-dashboard)
12. [Endpoints de Vehicle Drivers](#endpoints-de-vehicle-drivers)
13. [Endpoints de Smart Slaves](#endpoints-de-smart-slaves)
14. [Endpoints de Arquivos](#endpoints-de-arquivos)
15. [Endpoints de Comandos](#endpoints-de-comandos)
16. [Endpoints de Teste](#endpoints-de-teste)
17. [Compatibilidade CTID](#compatibilidade-ctid)
18. [Compatibilidade Hikvision](#compatibilidade-hikvision)
19. [Modelos de Dados](#modelos-de-dados)
20. [Códigos de Retorno](#códigos-de-retorno)

---

## Endpoints de Autenticação

### 1. Login

**Endpoint:** `POST /mbcortex/master/api/v1/login`

Autentica o usuário e retorna uma session key.

#### Request Body
```json
{
  "pass": "senha_do_usuario"
}
```

#### Response (200 OK)
```json
{
  "ret": 0,
  "session_key": "a1b2c3d4e5f6789012345678901234567890abcdef1234567890abcdef123456",
  "expires_in": 900
}
```

> **⚠️ Importante**: O campo `ret` é numérico (`0` = sucesso), não string. A Postman Collection inclui um script automático que extrai a `session_key` e salva na variável `sessionKey` para uso automático nas próximas requisições.

#### Response (401 Unauthorized)
```json
{
  "ret": -1,
  "error": "senha incorreta"
}
```

**Notas:**
- Em caso de falha de autenticação, há um delay de 5 segundos (anti brute-force)
- A senha é armazenada como MD5 hex em `/var/mcut/.data/passwd`
- Senha padrão quando o arquivo não existe: "admin" (MD5: 21232f297a57a5a743894a0e4a801fc3)

---

### 2. Alterar Senha

**Endpoint:** `PUT /mbcortex/master/api/v1/login`

Altera a senha do usuário autenticado.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Request Body
```json
{
  "pass_atual": "senha_atual",
  "pass_nova": "nova_senha",
  "pass_nova2": "nova_senha"
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

#### Response (400 Bad Request)
```json
{
  "ret": -2,
  "error": "confirmacao de senha nao confere"
}
```

#### Response (401 Unauthorized)
```json
{
  "ret": -3,
  "error": "senha atual incorreta"
}
```

---

### 3. Logout

**Endpoint:** `DELETE /mbcortex/master/api/v1/login`

Invalida a session key atual.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Request Body
```json
{
  "session_key": "a1b2c3d4e5f6..."
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Tokens de API

Tokens de API são credenciais de longa duração para integrações sistêmicas.

### 4. Listar Tokens

**Endpoint:** `GET /mbcortex/master/api/v1/token`

Lista todos os tokens de API salvos.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "tokens": [
    {
      "token": "a1b2c3d4...",
      "label": "ERP Principal",
      "expires_at": "2026-12-31T23:59:59",
      "expired": false
    },
    {
      "token": "e5f6g7h8...",
      "label": "Integração Mobile",
      "expires_at": "",
      "expired": false
    }
  ],
  "ret": "OK"
}
```

**Notas:**
- `expires_at` vazio indica token sem data de expiração
- `expired` é `true` se o token expirou

---

### 5. Criar Token

**Endpoint:** `POST /mbcortex/master/api/v1/token`

Cria um novo token de API.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body (com data de expiração)
```json
{
  "label": "Integração ERP",
  "expires_at": "2026-12-31T23:59:59"
}
```

#### Request Body (com TTL em dias)
```json
{
  "label": "Integração ERP",
  "ttl_days": 365
}
```

#### Request Body (sem expiração)
```json
{
  "label": "Token Permanente"
}
```

#### Response (201 Created)
```json
{
  "token": "a1b2c3d4e5f6...",
  "label": "Integração ERP",
  "expires_at": "2026-12-31T23:59:59",
  "ret": "OK"
}
```

**Notas:**
- Máximo de 64 tokens por dispositivo
- Formato de data aceito: `YYYY-MM-DDTHH:MM:SS` ou `YYYY-MM-DD`

---

### 6. Remover Token

**Endpoint:** `DELETE /mbcortex/master/api/v1/token`

Remove um token de API.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "token": "a1b2c3d4e5f6..."
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

#### Response (404 Not Found)
```json
{
  "ret": -4,
  "error": "token nao encontrado"
}
```

---

## Endpoints de Configuração

### 7. Obter Configuração

**Endpoint:** `GET /mbcortex/master/api/v1/config`

Retorna a configuração completa do dispositivo.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "model": {
    "hw_modeln": 1,
    "hw_model": "RXPPRO-MASTER",
    "versao_string": "1.0.0",
    "builddate": "2026-02-27"
  },
  "eth": [...],
  "wifi": [...],
  "master": {
    "id": 1,
    "frase": "FRASE_DE_ACESSO",
    "app": "RXPPRO"
  },
  "devgid": 123456789,
  "idShowStr": "000-000-001"
}
```

---

### 8. Atualizar Configuração

**Endpoint:** `POST /mbcortex/master/api/v1/config`

Atualiza a configuração do dispositivo.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "master": {
    "frase": "NOVA_FRASE"
  }
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Rede

### 9. Configuração de Rede Cabo (GET)

**Endpoint:** `GET /mbcortex/master/api/v1/network-config-cable`

Retorna configuração da interface Ethernet.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ip": "192.168.1.100",
  "mask": "255.255.255.0",
  "gw": "192.168.1.1",
  "ip2": "",
  "mask2": "",
  "ret": "OK"
}
```

---

### 10. Configuração de Rede Cabo (POST)

**Endpoint:** `POST /mbcortex/master/api/v1/network-config-cable`

Atualiza configuração da interface Ethernet.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body (IP Estático)
```json
{
  "ip": "192.168.1.100",
  "mask": "255.255.255.0",
  "gw": "192.168.1.1"
}
```

#### Request Body (DHCP)
```json
{
  "ip": "dhcp"
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

### 11. Configuração WiFi AP (GET)

**Endpoint:** `GET /mbcortex/master/api/v1/network-config-wifi-ap`

Retorna configuração do Access Point WiFi.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "enabled": 1,
  "ssid": "MobiCortex-AP",
  "channel": 6,
  "ret": "OK"
}
```

---

### 12. Configuração WiFi AP (POST)

**Endpoint:** `POST /mbcortex/master/api/v1/network-config-wifi-ap`

Atualiza configuração do Access Point WiFi.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "enabled": 1,
  "ssid": "MobiCortex-AP",
  "password": "senha123",
  "channel": 6
}
```

---

### 13. Configuração WiFi Station (GET)

**Endpoint:** `GET /mbcortex/master/api/v1/network-config-wifi-st`

Retorna configuração do cliente WiFi (station).

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "enabled": 1,
  "ssid": "Rede-WiFi",
  "ret": "OK"
}
```

---

### 14. Configuração WiFi Station (POST)

**Endpoint:** `POST /mbcortex/master/api/v1/network-config-wifi-st`

Atualiza configuração do cliente WiFi (station).

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "enabled": 1,
  "ssid": "Rede-WiFi",
  "password": "senha123"
}
```

---

### 15. Scan WiFi

**Endpoint:** `GET /mbcortex/master/api/v1/network-wifi-scan`

Escaneia redes WiFi disponíveis.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "networks": [
    {
      "ssid": "Rede-1",
      "bssid": "AA:BB:CC:DD:EE:FF",
      "channel": 6,
      "rssi": -45,
      "auth": "WPA2-PSK"
    }
  ],
  "ret": "OK"
}
```

---

### 16. Clientes WiFi Conectados

**Endpoint:** `GET /mbcortex/master/api/v1/network-wifi-clients`

Lista clientes conectados ao AP.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "clients": [
    {
      "mac": "AA:BB:CC:DD:EE:FF",
      "ip": "192.168.4.2",
      "rssi": -50
    }
  ],
  "ret": "OK"
}
```

---

### 17. Interfaces de Rede

**Endpoint:** `GET /mbcortex/master/api/v1/network-interfaces`

Lista interfaces de rede.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "interfaces": [
    {
      "name": "eth0",
      "ip": "192.168.1.100",
      "mac": "AA:BB:CC:DD:EE:FF",
      "up": true
    }
  ],
  "ret": "OK"
}
```

---

### 18. Sinal WiFi

**Endpoint:** `GET /mbcortex/master/api/v1/network-wifi-signal`

Retorna qualidade do sinal WiFi.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "rssi": -50,
  "quality": "good",
  "ret": "OK"
}
```

---

## Endpoints de Central Registry

### 19. Listar Cadastros (Paginado)

**Endpoint:** `GET /mbcortex/master/api/v1/central-registry`

Lista cadastros com paginação e filtros.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Parâmetros de Query
| Parâmetro | Tipo | Default | Descrição |
|-----------|------|---------|-----------|
| `offset` | int | 0 | Registro inicial |
| `count` | int | 20 | Registros por página |
| `name` | string | - | Filtro por nome (parcial) |

#### Response (200 OK)
```json
{
  "ret": "OK",
  "data": [
    {
      "id": 12345,
      "name": "Cadastro Exemplo",
      "enabled": 1,
      "slots1": 5,
      "slots2": 3,
      "type": 0
    }
  ],
  "total": 150,
  "offset": 0,
  "count": 20
}
```

---

### 20. Buscar Cadastro por ID

**Endpoint:** `GET /mbcortex/master/api/v1/central-registry?id=<cadastro_id>`

Busca cadastro específico por ID.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "data": {
    "id": 12345,
    "name": "Cadastro Exemplo",
    "enabled": 1,
    "slots1": 5,
    "slots2": 3,
    "type": 0
  }
}
```

---

### 21. Criar/Atualizar Cadastro

**Endpoint:** `POST /mbcortex/master/api/v1/central-registry`

Cria ou atualiza cadastro.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body (Criar Novo)
```json
{
  "id": 0,
  "name": "Cadastro Principal",
  "enabled": 1,
  "slots1": 0,
  "slots2": 0,
  "type": 0
}
```

#### Request Body (Atualizar Existente)
```json
{
  "id": 12345,
  "name": "Cadastro Atualizado",
  "enabled": 1,
  "slots1": 10,
  "slots2": 5,
  "type": 0
}
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "id": 12345
}
```

---

### 22. Remover Cadastro

**Endpoint:** `DELETE /mbcortex/master/api/v1/central-registry?id=<cadastro_id>`

Remove cadastro.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

### 23. Estatísticas do Registro Central

**Endpoint:** `GET /mbcortex/master/api/v1/central-registry/stats`

Retorna estatísticas do registro central.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "total_cadastros": 150,
  "total_entidades": 5000,
  "total_midias": 8500
}
```

---

## Endpoints de Entidades

### 24. Listar Entidades (Paginado)

**Endpoint:** `GET /mbcortex/master/api/v1/entities`

Lista entidades com paginação e filtros.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Parâmetros de Query
| Parâmetro | Tipo | Default | Descrição |
|-----------|------|---------|-----------|
| `id` | uint32 | - | ID da entidade (retorna única) |
| `cadastro_id` | uint32 | - | Filtra por cadastro |
| `offset` | int | 0 | Registro inicial |
| `count` | int | 20 | Registros por página (max: 100 Linux, 10 ESP32) |
| `name` | string | - | Filtro por nome (parcial) |
| `doc` | string | - | Filtro por documento |
| `tipo` | int | - | Filtro por tipo (1=pessoa, 2=veículo) |

#### Response (200 OK)
```json
{
  "ret": "OK",
  "data": [
    {
      "entity_id": 12345,
      "cadastro_id": 100,
      "tipo": 1,
      "habilitado": 1,
      "name": "João Silva",
      "doc": "12345678900",
      "lpr_ativo": 0,
      "created_at": 1709000000,
      "updated_at": 1709000000
    }
  ],
  "total": 5000,
  "offset": 0,
  "count": 20
}
```

---

### 25. Buscar Entidade por ID

**Endpoint:** `GET /mbcortex/master/api/v1/entities?id=<entity_id>`

Busca entidade específica por ID.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "data": {
    "entity_id": 12345,
    "cadastro_id": 100,
    "tipo": 1,
    "habilitado": 1,
    "name": "João Silva",
    "doc": "12345678900",
    "lpr_ativo": 0,
    "created_at": 1709000000,
    "updated_at": 1709000000
  }
}
```

---

### 26. Criar Entidade

**Endpoint:** `POST /mbcortex/master/api/v1/entities`

Cria uma nova entidade.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body (Pessoa)
```json
{
  "cadastro_id": 100,
  "tipo": 1,
  "habilitado": 1,
  "name": "João Silva",
  "doc": "12345678900"
}
```

#### Request Body (Veículo com LPR)
```json
{
  "cadastro_id": 100,
  "tipo": 2,
  "habilitado": 1,
  "name": "Civic Preto",
  "doc": "ABC1D23",
  "lpr_ativo": 1
}
```

#### Request Body (Auto-ID)
```json
{
  "id": 0,
  "cadastro_id": 0,
  "createid": true,
  "tipo": 1,
  "habilitado": 1,
  "name": "Nova Entidade",
  "doc": ""
}
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "entity_id": 12345
}
```

**Notas:**
- `lpr_ativo=1` cria automaticamente uma mídia LPR para a entidade
- `createid=true` gera automaticamente cadastro_id e entity_id
- Placas de veículos são normalizadas (maiúsculas, sem espaços/hífens)
- IDs altos (>=4294000000) são reservados para uso web

---

### 27. Atualizar Entidade

**Endpoint:** `PUT /mbcortex/master/api/v1/entities`

Atualiza uma entidade existente.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "entity_id": 12345,
  "name": "João Silva Atualizado",
  "habilitado": 1,
  "lpr_ativo": 1
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

**Notas:**
- Apenas os campos fornecidos serão atualizados
- `lpr_ativo=1` cria mídia LPR se não existir
- `lpr_ativo=0` remove mídia LPR se existir

---

### 28. Remover Entidade

**Endpoint:** `DELETE /mbcortex/master/api/v1/entities?id=<entity_id>`

Remove uma entidade e suas mídias vinculadas (cascade delete).

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Mídias

### 29. Listar Mídias

**Endpoint:** `GET /mbcortex/master/api/v1/media`

Lista mídias com filtros.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Parâmetros de Query
| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `id` | uint32 | ID da mídia (retorna única) |
| `entity_id` | uint32 | Filtra por entidade |

#### Response (200 OK)
```json
{
  "ret": "OK",
  "data": [
    {
      "media_id": 54321,
      "entity_id": 12345,
      "cadastro_id": 100,
      "tipo": 21,
      "descricao": "123,45678",
      "habilitado": 1,
      "ns32_0": 123,
      "ns32_1": 45678
    }
  ]
}
```

---

### 30. Criar Mídia

**Endpoint:** `POST /mbcortex/master/api/v1/media`

Cria uma nova mídia.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body (RFID Wiegand 26)
```json
{
  "entity_id": 12345,
  "cadastro_id": 100,
  "tipo": 21,
  "descricao": "123,45678"
}
```

#### Request Body (LPR - Placa)
```json
{
  "entity_id": 12345,
  "cadastro_id": 100,
  "tipo": 17,
  "descricao": "ABC1D23",
  "ns32_0": 0,
  "ns32_1": 0
}
```

> **Nota**: Para LPR, os campos `ns32_0=0` e `ns32_1=0` são obrigatórios para evitar validação RFID.

#### Response (200 OK)
```json
{
  "ret": "OK",
  "media_id": 54321
}
```

---

### 31. Atualizar Mídia

**Endpoint:** `PUT /mbcortex/master/api/v1/media`

Atualiza uma mídia existente.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "media_id": 54321,
  "descricao": "124,45679",
  "habilitado": 1
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

### 32. Remover Mídia

**Endpoint:** `DELETE /mbcortex/master/api/v1/media?id=<media_id>`

Remove uma mídia.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Video Source

### 33. Listar Fontes de Vídeo

**Endpoint:** `GET /mbcortex/master/api/v1/video-source`

Retorna configuração das fontes de vídeo.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "sources": [
    {
      "id": 0,
      "name": "Camera 1",
      "type": "rtsp",
      "url": "rtsp://192.168.1.50:554/stream1",
      "enabled": 1
    }
  ]
}
```

---

### 34. Criar Fonte de Vídeo

**Endpoint:** `POST /mbcortex/master/api/v1/video-source`

Cria uma nova fonte de vídeo.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "name": "Camera 2",
  "type": "rtsp",
  "url": "rtsp://192.168.1.51:554/stream1",
  "enabled": 1
}
```

---

### 35. Atualizar Fonte de Vídeo

**Endpoint:** `PUT /mbcortex/master/api/v1/video-source`

Atualiza uma fonte de vídeo existente.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "id": 0,
  "name": "Camera 1 Atualizada",
  "enabled": 1
}
```

---

### 36. Remover Fonte de Vídeo

**Endpoint:** `DELETE /mbcortex/master/api/v1/video-source?id=<source_id>`

Remove uma fonte de vídeo.

#### Headers
```
Authorization: Bearer <session_key>
```

---

## Endpoints de Webhook

### 37. Obter Configuração do Webhook

**Endpoint:** `GET /mbcortex/master/api/v1/webhook`

Retorna configuração do webhook.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "enabled": 1,
  "url": "https://example.com/webhook",
  "events": ["access_granted", "access_denied"],
  "secret": "webhook_secret"
}
```

---

### 38. Criar/Atualizar Webhook

**Endpoint:** `POST /mbcortex/master/api/v1/webhook`

Cria ou atualiza configuração do webhook.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "enabled": 1,
  "url": "https://example.com/webhook",
  "events": ["access_granted", "access_denied"],
  "secret": "webhook_secret"
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

### 39. Remover Webhook

**Endpoint:** `DELETE /mbcortex/master/api/v1/webhook`

Remove a configuração do webhook.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Device Info

### 40. Informações do Dispositivo

**Endpoint:** `GET /mbcortex/master/api/v1/device-info`

Retorna informações do hardware.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "hw_model": "RXPPRO-MASTER",
  "hw_version": "1.0",
  "fw_version": "1.0.0",
  "build_date": "2026-02-27",
  "uptime": 86400,
  "mac": "AA:BB:CC:DD:EE:FF",
  "ip": "192.168.1.100"
}
```

---

## Endpoints de Dashboard

### 41. Estatísticas do Dashboard

**Endpoint:** `GET /mbcortex/master/api/v1/dashboard`

Retorna estatísticas do sistema.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "total_cadastros": 150,
  "total_entidades": 5000,
  "total_midias": 8500,
  "eventos_hoje": 1250,
  "ultimos_eventos": [...]
}
```

---

## Endpoints de Vehicle Drivers

### 42. Listar Motoristas de Veículos

**Endpoint:** `GET /mbcortex/master/api/v1/vehicle-drivers`

Retorna lista de motoristas vinculados a veículos.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "drivers": [
    {
      "vehicle_id": 12345,
      "driver_id": 67890,
      "vinculado_em": 1709000000
    }
  ]
}
```

---

### 43. Atualizar Motorista de Veículo

**Endpoint:** `PUT /mbcortex/master/api/v1/vehicle-drivers`

Vincula/desvincula motorista a veículo.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "vehicle_id": 12345,
  "driver_id": 67890
}
```

#### Response (200 OK)
```json
{
  "ret": "OK"
}
```

---

## Endpoints de Smart Slaves

### 44. Comunicação com Slave Smart

**Endpoint:** `POST /mbcortex/master/api/v1/SlaveSmart.MCUT`

Comunicação com slaves smart.

#### Headers
```
Authorization: Bearer <session_key>
```

---

### 45. Comunicação com M1127

**Endpoint:** `GET /mbcortex/master/api/v1/M1127.MCUT`

Comunicação com módulo M1127.

#### Headers
```
Authorization: Bearer <session_key>
```

---

### 46. Lista de Slaves Smart

**Endpoint:** `GET /mbcortex/master/api/v1/SlaveSmartList.MCUT`

Lista slaves smart conectados.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "slaves": [
    {
      "id": 1,
      "type": "M1127",
      "address": "192.168.1.50",
      "status": "online"
    }
  ]
}
```

---

## Endpoints de Arquivos

### 47. Listar Arquivos

**Endpoint:** `GET /mbcortex/master/api/v1/files`

Lista arquivos do sistema.

#### Headers
```
Authorization: Bearer <session_key>
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "files": [
    {
      "name": "config.json",
      "size": 1024,
      "modified": "2026-02-27T10:00:00"
    }
  ]
}
```

---

## Endpoints de Comandos

### 48. Executar Comando

**Endpoint:** `GET /mbcortex/master/api/v1/command`

Envia comandos para a controladora.

#### Headers
```
Authorization: Bearer <session_key>
```

---

## Endpoints de Teste

### 49. Auditoria de Integridade de Dados

**Endpoint:** `POST /mbcortex/master/api/v1/test/data-integrity-audit`

Executa auditoria de integridade dos dados.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "check_entities": true,
  "check_media": true,
  "check_indexes": true
}
```

#### Response (200 OK)
```json
{
  "ret": "OK",
  "audit_result": {
    "entities_ok": true,
    "media_ok": true,
    "indexes_ok": true,
    "errors_found": 0,
    "fixed": 0
  }
}
```

---

## Compatibilidade CTID

Estes endpoints mantêm compatibilidade com o protocolo ControlID.

### 50. Login CTID

**Endpoint:** `POST /login.fcgi`

Login compatível com CTID.

#### Request Body
```json
{
  "login": "admin",
  "password": "admin"
}
```

#### Response (200 OK)
```json
{
  "session": "session_id"
}
```

---

### 51. Criar Objetos CTID

**Endpoint:** `POST /create_objects.fcgi`

Cria objetos (compatível CTID).

#### Headers
```
Content-Type: application/json
```

---

### 52. Carregar Objetos CTID

**Endpoint:** `POST /load_objects.fcgi`

Carrega objetos (compatível CTID).

#### Headers
```
Content-Type: application/json
```

---

### 53. Destruir Objetos CTID

**Endpoint:** `POST /destroy_objects.fcgi`

Remove objetos (compatível CTID).

#### Headers
```
Content-Type: application/json
```

---

### 54. Validar Sessão CTID

**Endpoint:** `POST /session_is_valid.fcgi`

Valida sessão CTID.

---

### 55. Definir Imagem do Usuário CTID

**Endpoint:** `POST /user_set_image.fcgi`

Define imagem do usuário.

---

### 56. Obter Imagem do Usuário CTID

**Endpoint:** `GET /user_get_image.fcgi`

Obtém imagem do usuário.

---

### 57. Remover Imagem do Usuário CTID

**Endpoint:** `POST /user_destroy_image.fcgi`

Remove imagem do usuário.

---

## Compatibilidade Hikvision

Estes endpoints mantêm compatibilidade com o protocolo Hikvision ISAPI.

**Autenticação:** Digest Auth

### 58. Registrar Usuário Hikvision

**Endpoint:** `POST /ISAPI/AccessControl/UserInfo/Record`

Cria/atualiza usuário (formato Hikvision).

---

### 59. Buscar Usuários Hikvision

**Endpoint:** `POST /ISAPI/AccessControl/UserInfo/Search`

Busca usuários (formato Hikvision).

---

### 60. Remover Usuários Hikvision

**Endpoint:** `POST /ISAPI/AccessControl/UserInfo/Delete`

Remove usuários (formato Hikvision).

---

### 61. Registrar Cartão Hikvision

**Endpoint:** `POST /ISAPI/AccessControl/CardInfo/Record`

Cria/atualiza cartão (formato Hikvision).

---

### 62. Capacidades Access Control Hikvision

**Endpoint:** `GET /ISAPI/AccessControl/Capabilities`

Retorna capacidades de controle de acesso.

---

### 63. Registrar Face Hikvision

**Endpoint:** `POST /ISAPI/Intelligent/FD/FaceData/Record`

Cria dados faciais (formato Hikvision).

---

### 64. Verificar Usuário Hikvision

**Endpoint:** `GET /ISAPI/Security/userCheck`

Verifica credenciais do usuário.

---

### 65. Capacidades de Segurança Hikvision

**Endpoint:** `GET /ISAPI/Security/capabilities`

Retorna capacidades de segurança.

---

### 66. Login de Sessão Hikvision

**Endpoint:** `POST /ISAPI/Security/sessionLogin`

Login de sessão (formato XML).

---

### 67. Token de Segurança Hikvision

**Endpoint:** `GET /ISAPI/Security/token`

Obtém token de segurança.

---

## Endpoints de Informações

### 68. Informações do Sistema

**Endpoint:** `GET /mbcortex/master/api/v1/info`

Retorna informações gerais do sistema.

#### Response (200 OK)
```json
{
  "ret": "OK",
  "name": "MobiCortex Master",
  "version": "1.0.0"
}
```

---

## Modelos de Dados

### Entity (Entidade)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| entity_id | uint32 | ID único da entidade |
| cadastro_id | uint32 | ID do cadastro vinculado |
| tipo | int | Tipo da entidade (1=pessoa, 2=veículo, etc) |
| habilitado | int | 1=habilitado, 0=desabilitado |
| name | string | Nome da entidade |
| doc | string | Documento (CPF/CNPJ/Placa) |
| lpr_ativo | int | 1=possui mídia LPR, 0=não possui |
| created_at | int | Timestamp de criação |
| updated_at | int | Timestamp de atualização |

### Media (Mídia)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| media_id | uint32 | ID único da mídia |
| entity_id | uint32 | ID da entidade vinculada |
| cadastro_id | uint32 | ID do cadastro |
| tipo | int | Tipo da mídia (veja tabela abaixo) |
| descricao | string | Descrição da mídia |
| ns32_0 | int | Dados binários da mídia (parte baixa) |
| ns32_1 | int | Dados binários da mídia (parte alta) |
| habilitado | int | 1=habilitado, 0=desabilitado |

#### Tipos de Mídia

| Valor | Constante | Descrição | Observações |
|-------|-----------|-----------|-------------|
| 0 | ControleRemoto | Controle remoto HT | - |
| 1 | Hcs | HCS | - |
| 5 | Bio | Biometria | - |
| 8 | Teclado | Senha via teclado | - |
| 15 | Bio3 | Biometria 3 | - |
| 17 | Lpr | Placa (LPR) | Requer ns32_0=0, ns32_1=0 |
| 18 | BioHikvision | Biometria Hikvision | - |
| 20 | Facial | Reconhecimento facial | - |
| 21 | Wiegand26 | RFID Wiegand 26 bits | Formato: "123,45678" |
| 22 | Wiegand34 | RFID Wiegand 34 bits | Formato: "1234,567890" |

#### Campos ns32_0 e ns32_1

Estes campos armazenam os dados binários da mídia (número serial, código RFID, etc).

- **RFID**: Não enviar - o backend calcula automaticamente a partir da `descricao`
- **LPR/Facial/Outros**: Enviar `0` para evitar validação de formato RFID

### Central Registry (Cadastro)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | uint32 | ID único do cadastro |
| name | string | Nome do cadastro |
| enabled | int | 1=habilitado, 0=desabilitado |
| slots1 | int | Slots tipo 1 |
| slots2 | int | Slots tipo 2 |
| type | int | Tipo do cadastro |

---

## Códigos de Retorno

| Código | Descrição |
|--------|-----------|
| "OK" | Sucesso |
| 0 | Sucesso (numérico) |
| -1 | Erro genérico |
| -2 | Parâmetros inválidos |
| -3 | Não autorizado |
| -4 | Recurso não encontrado |
| -5 | Recurso já existe |

---

## Notas Gerais

- Todas as requisições protegidas requerem o header `Authorization: Bearer <session_key>`
- A session key expira após 15 minutos de inatividade (renew automático a cada requisição válida)
- O arquivo de senha fica em `/var/mcut/.data/passwd` (MD5 hex)
- Senha padrão quando o arquivo não existe: "admin" (MD5: 21232f297a57a5a743894a0e4a801fc3)
- Máximo de 10 sessões simultâneas por dispositivo
- Máximo de 64 tokens de API por dispositivo
- IDs na faixa >= 4294000000 são reservados para uso web
- Placas de veículos são normalizadas (maiúsculas, sem espaços/hífens) antes de salvar
- UTF-8 é usado em todos os campos de texto

---

## Changelog

### 2026-03-05
- Adicionados endpoints de Vehicle Drivers
- Adicionados endpoints de Data Integrity Audit
- Documentado campo `lpr_ativo` em entidades
- Adicionados métodos PUT para entities e media
- Adicionado endpoint DELETE para webhook
- Documentados endpoints de Dashboard e Info

### 2026-02-28
- Adicionado suporte a paginação em entidades
- Adicionado cascade delete em entidades
- Adicionado campo lpr_ativo

### 2026-02-27
- Documentação inicial
