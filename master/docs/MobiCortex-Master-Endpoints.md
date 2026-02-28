# MobiCortex Master - API Endpoints (RXPPRO)

Documentação completa dos endpoints da API da Controladora Master MobiCortex (RXPPRO).

## Base URL

```
https://<ip-da-controladora>:4449  (modo desktop/debug)
https://<ip-da-controladora>:443   (modo produção ARM)
http://<ip-da-controladora>:80     (redireciona para HTTPS)
```

## Autenticação

A API utiliza autenticação por session key. Para obter uma session key, use o endpoint de login.

### Header de Autenticação

```
Authorization: Bearer <session_key>
```

A session key é um token de 64 caracteres hexadecimais (SHA256) válido por 15 minutos (TTL = 900 segundos). A sessão é renovada automaticamente a cada requisição válida (sliding TTL).

---

## Endpoints de Autenticação

### 1. Login

**Endpoint:** `POST /master/api/v1/login`

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
  "ret": "OK",
  "session_key": "a1b2c3d4e5f6...",
  "expires_in": 900
}
```

#### Response (401 Unauthorized)
```json
{
  "ret": -1,
  "error": "senha incorreta"
}
```

---

### 2. Alterar Senha

**Endpoint:** `PUT /master/api/v1/login`

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

---

### 3. Logout

**Endpoint:** `DELETE /master/api/v1/login`

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

### 4. Tokens de API

**Endpoint:** `GET /master/api/v1/token`

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
    }
  ],
  "ret": "OK"
}
```

---

**Endpoint:** `POST /master/api/v1/token`

Cria um novo token de API.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

#### Request Body
```json
{
  "label": "Integração ERP",
  "ttl_days": 365
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

---

**Endpoint:** `DELETE /master/api/v1/token`

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

---

## Endpoints de Configuração

### 5. Configuração Geral

**Endpoint:** `GET /master/api/v1/config`

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

**Endpoint:** `POST /master/api/v1/config`

Atualiza a configuração do dispositivo.

#### Headers
```
Authorization: Bearer <session_key>
Content-Type: application/json
```

---

## Endpoints de Rede

### 6. Configuração de Rede Cabo

**Endpoint:** `GET /master/api/v1/network-config-cable`

Retorna configuração da interface Ethernet.

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

**Endpoint:** `POST /master/api/v1/network-config-cable`

Atualiza configuração da interface Ethernet.

#### Request Body
```json
{
  "ip": "192.168.1.100",
  "mask": "255.255.255.0",
  "gw": "192.168.1.1"
}
```

Use `"ip": "dhcp"` para configurar DHCP.

---

### 7. Configuração WiFi AP

**Endpoint:** `GET /master/api/v1/network-config-wifi-ap`

Retorna configuração do Access Point WiFi.

---

**Endpoint:** `POST /master/api/v1/network-config-wifi-ap`

Atualiza configuração do Access Point WiFi.

---

### 8. Configuração WiFi Station

**Endpoint:** `GET /master/api/v1/network-config-wifi-st`

Retorna configuração do cliente WiFi (station).

---

**Endpoint:** `POST /master/api/v1/network-config-wifi-st`

Atualiza configuração do cliente WiFi (station).

---

### 9. Ferramentas de Rede

**Endpoint:** `GET /master/api/v1/network-wifi-scan`

Escaneia redes WiFi disponíveis.

---

**Endpoint:** `GET /master/api/v1/network-wifi-clients`

Lista clientes conectados ao AP.

---

**Endpoint:** `GET /master/api/v1/network-interfaces`

Lista interfaces de rede.

---

**Endpoint:** `GET /master/api/v1/network-wifi-signal`

Retorna qualidade do sinal WiFi.

---

## Endpoints de Entidades (ENTITY6)

### 10. Entidades

**Endpoint:** `GET /master/api/v1/entities?id=<entity_id>`

Busca uma entidade específica por ID.

---

**Endpoint:** `GET /master/api/v1/entities?cadastro_id=<cadastro_id>`

Conta entidades de um cadastro.

---

**Endpoint:** `POST /master/api/v1/entities`

Cria uma nova entidade.

#### Request Body
```json
{
  "cadastro_id": 1,
  "tipo": 1,
  "habilitado": 1,
  "name": "Nome da Entidade",
  "doc": "12345678900"
}
```

---

**Endpoint:** `DELETE /master/api/v1/entities?id=<entity_id>`

Remove uma entidade.

---

## Endpoints de Mídias (MEDIA6)

### 11. Mídias

**Endpoint:** `GET /master/api/v1/media?id=<media_id>`

Busca uma mídia específica por ID.

---

**Endpoint:** `GET /master/api/v1/media?entity_id=<entity_id>`

Conta mídias de uma entidade.

---

**Endpoint:** `POST /master/api/v1/media`

Cria uma nova mídia.

#### Request Body
```json
{
  "entity_id": 1,
  "cadastro_id": 1,
  "tipo": 1,
  "descricao": "Placa ABC1234"
}
```

---

**Endpoint:** `DELETE /master/api/v1/media?id=<media_id>`

Remove uma mídia.

---

## Endpoints de Comandos

### 12. Comandos

**Endpoint:** `POST /master/api/v1/command`

Envia comandos para a controladora.

---

## Endpoints de Arquivos

### 13. Arquivos

**Endpoint:** `GET /master/api/v1/files`

Lista arquivos do sistema.

---

**Endpoint:** `POST /master/api/v1/files`

Envia arquivo para a controladora.

---

## Endpoints de Webhook

### 14. Webhook

**Endpoint:** `GET /master/api/v1/webhook`

Retorna configuração do webhook.

---

**Endpoint:** `POST /master/api/v1/webhook`

Atualiza configuração do webhook.

---

## Endpoints de Video Source

### 15. Fontes de Vídeo

**Endpoint:** `GET /master/api/v1/video-source`

Retorna configuração das fontes de vídeo.

---

**Endpoint:** `POST /master/api/v1/video-source`

Atualiza configuração das fontes de vídeo.

---

## Endpoints de Device Info

### 16. Informações do Dispositivo

**Endpoint:** `GET /master/api/v1/device-info`

Retorna informações do hardware.

---

## Endpoints de Central Registry

### 17. Registro Central

**Endpoint:** `GET /master/api/v1/central-registry`

Acessa o registro central.

---

**Endpoint:** `GET /master/api/v1/central-registry/stats`

Retorna estatísticas do registro central.

---

## Endpoints Smart Slaves

### 18. Slaves Smart

**Endpoint:** `GET /master/api/v1/SlaveSmart.MCUT`

Comunicação com slaves smart.

---

**Endpoint:** `GET /master/api/v1/M1127.MCUT`

Comunicação com módulo M1127.

---

**Endpoint:** `GET /master/api/v1/SlaveSmartList.MCUT`

Lista slaves smart conectados.

---

## Endpoints de Compatibilidade CTID

### 19. CTID Legacy

**Endpoint:** `POST /login.fcgi`

Login compatível com CTID.

---

**Endpoint:** `POST /create_objects.fcgi`

Cria objetos (compatível CTID).

---

**Endpoint:** `POST /load_objects.fcgi`

Carrega objetos (compatível CTID).

---

**Endpoint:** `POST /destroy_objects.fcgi`

Remove objetos (compatível CTID).

---

## Modelos de Dados

### Entity (Entidade)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| entity_id | int | ID único da entidade |
| cadastro_id | int | ID do cadastro vinculado |
| tipo | int | Tipo da entidade (1=pessoa, 2=veículo, etc) |
| habilitado | int | 1=habilitado, 0=desabilitado |
| name | string | Nome da entidade |
| doc | string | Documento (CPF/CNPJ/Placa) |
| created_at | int | Timestamp de criação |
| updated_at | int | Timestamp de atualização |

### Media (Mídia)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| media_id | int | ID único da mídia |
| entity_id | int | ID da entidade vinculada |
| cadastro_id | int | ID do cadastro |
| tipo | int | Tipo da mídia (RFID, Placa, etc) |
| descricao | string | Descrição da mídia |
| habilitado | int | 1=habilitado, 0=desabilitado |

---

## Códigos de Retorno

| Código | Descrição |
|--------|-----------|
| "OK" | Sucesso |
| -1 | Erro genérico |
| -2 | Parâmetros inválidos |
| -3 | Não autorizado |
| -4 | Recurso não encontrado |
| -5 | Recurso já existe |

---

## Notas

- Todas as requisições protegidas requerem o header `Authorization: Bearer <session_key>`
- A session key expira após 15 minutos de inatividade (renew automático a cada requisição válida)
- O arquivo de senha fica em `/var/mcut/.data/passwd` (MD5 hex)
- Senha padrão quando o arquivo não existe: "admin" (MD5: 21232f297a57a5a743894a0e4a801fc3)
- Máximo de 10 sessões simultâneas por dispositivo
- Máximo de 64 tokens de API por dispositivo
