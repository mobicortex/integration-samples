# MobiCortex Master - API Endpoints

Documentacao operacional da API HTTP do MobiCortex Master.

## Base URL e autenticacao

Base URLs usuais:

```text
https://<ip-da-controladora>:4449
https://<ip-da-controladora>
```

Notas:
- `https://<ip-da-controladora>` representa HTTPS na porta padrao `443`
- explicite a porta apenas quando ela for diferente da padrao, como `4449`
- `4449` e tipicamente usada em ambiente de desenvolvimento ou debug

Base path:

```text
/mbcortex/master/api/v1
```

Autenticacao:

```text
Authorization: Bearer <session_key>
```

Notas:
- TTL padrao da sessao: 900 segundos
- renovacao deslizante: cada request autenticado renova o TTL
- a collection Postman deste repositorio salva `sessionKey` automaticamente apos o login

## Autenticacao

### POST `/login`

Obtem a `session_key` para todos os endpoints autenticados.

Request:

```json
{
  "pass": "admin"
}
```

Response `200`:

```json
{
  "ret": 0,
  "session_key": "64_hex_chars",
  "expires_in": 900
}
```

Erros comuns:
- `401`: senha incorreta
- `500`: falha interna

### PUT `/login`

Altera a senha atual.

Request:

```json
{
  "pass_atual": "admin",
  "pass_nova": "nova_senha",
  "pass_nova2": "nova_senha"
}
```

Response `200`:

```json
{
  "ret": 0
}
```

Erros comuns:
- `400`: campos ausentes, JSON invalido ou confirmacao divergente
- `401`: senha atual incorreta

### DELETE `/login`

Invalida a sessao atual.

Request:

```json
{
  "session_key": "64_hex_chars"
}
```

Response `200`:

```json
{
  "ret": 0
}
```

## Tokens de API

### GET `/token`

Lista tokens de integracao existentes.

Response `200`:

```json
{
  "ret": 0,
  "tokens": [
    {
      "token": "64_hex_chars",
      "label": "ERP",
      "expires_at": "2026-12-31T23:59:59",
      "expired": 0
    }
  ]
}
```

### POST `/token`

Com `ttl_days`:

```json
{
  "label": "ERP",
  "ttl_days": 365
}
```

Com `expires_at`:

```json
{
  "label": "ERP",
  "expires_at": "2026-12-31T23:59:59"
}
```

Sem expiracao:

```json
{
  "label": "Integracao permanente"
}
```

Response `201`:

```json
{
  "ret": 0,
  "token": "64_hex_chars",
  "label": "ERP",
  "expires_at": "2026-12-31T23:59:59"
}
```

Notas:
- maximo de 64 tokens
- `expires_at` aceita `YYYY-MM-DDTHH:MM:SS` ou `YYYY-MM-DD`

### DELETE `/token`

Request:

```json
{
  "token": "64_hex_chars"
}
```

Response `200`:

```json
{
  "ret": 0
}
```

## Cadastro Central

Contrato publico:
- `id`: `0` para geracao automatica
- `name`: obrigatorio
- `field1`: bloco
- `field2`: apartamento
- `field3`: departamento
- `field4`: divisao
- `enabled`: `1|0` ou `true|false`
- `slots1`, `slots2`, `type`

### GET `/central-registry`

Consultas suportadas:
- `?id=<id>`
- `?search=<texto>`
- `?offset=<n>&count=<n>`
- `?offset=<n>&count=<n>&name=<texto>`

Comportamento atual do backend:
- `offset`: padrao `0`
- `count`: padrao `10`
- `count`: maximo `200`

### GET `/central-registry?id=123`

Response `200`:

```json
{
  "ret": 0,
  "id": 123,
  "name": "Empresa ABC Ltda",
  "enabled": true,
  "type": 0,
  "field1": "Bloco A",
  "field2": "101",
  "field3": "TI",
  "field4": "Matriz",
  "slots1": 0,
  "slots2": 0,
  "people_count": 2,
  "vehicle_count": 1
}
```

### GET `/central-registry?offset=0&count=10&name=Empresa`

Response `200`:

```json
{
  "ret": 0,
  "count": 1,
  "total": 20,
  "items": [
    {
      "id": 123,
      "name": "Empresa ABC Ltda",
      "enabled": true,
      "type": 0,
      "field1": "Bloco A",
      "field2": "101",
      "field3": "TI",
      "field4": "Matriz",
      "slots1": 0,
      "slots2": 0,
      "people_count": 2,
      "vehicle_count": 1
    }
  ]
}
```

### GET `/central-registry?search=ABC1D23`

Busca consolidada por nome do cadastro, entidade, documento/placa ou descricao de midia.

Response `200`:

```json
{
  "ret": 0,
  "count": 1,
  "total": 1,
  "items": [
    {
      "id": 123,
      "name": "Empresa ABC Ltda",
      "enabled": true,
      "type": 0,
      "field1": "Bloco A",
      "field2": "101",
      "field3": "TI",
      "field4": "Matriz",
      "slots1": 0,
      "slots2": 0,
      "people_count": 2,
      "vehicle_count": 1,
      "match_source": "vehicle_plate",
      "match_value": "ABC1D23"
    }
  ]
}
```

### POST `/central-registry`

Create/update do cadastro central.

Request:

```json
{
  "id": 0,
  "name": "Empresa ABC Ltda",
  "field1": "Bloco A",
  "field2": "101",
  "field3": "TI",
  "field4": "Matriz",
  "enabled": true,
  "slots1": 0,
  "slots2": 0,
  "type": 0
}
```

Response `200`:

```json
{
  "ret": 0,
  "id": 4294000001
}
```

Notas:
- `POST` cobre criacao e atualizacao
- `id=0` gera ID automaticamente
- se `id=0` e `name` ja existir, o backend pode retornar `409`
- nao omita `field1..field4` da integracao; envie apenas os que tiver valor

### DELETE `/central-registry?id=123`

Tambem pode receber body com `id`.

Response `200`:

```json
{
  "ret": 0,
  "id": 123
}
```

### GET `/central-registry/stats`

Response `200`:

```json
{
  "ret": 0,
  "max_capacity": 200000,
  "current_total": 1500,
  "usage_percent": 0.8
}
```

## Entidades

Tipos principais:
- `1`: pessoa
- `2`: veiculo

Campos principais de integracao:
- `id`: `0` para auto-ID
- `type`
- `name`
- `doc`
- `central_registry_id`
- `enabled`
- `brand`, `model`, `color` para veiculos
- `lpr_enabled` para veiculos

Regras importantes:
- `doc` de veiculo representa a placa
- normalizar placa para maiusculo e sem separadores
- validar formato brasileiro antes de enviar
- `lpr_enabled=true` cria/atualiza a midia LPR automaticamente

### GET `/entities`

Consultas suportadas:
- `?id=<entity_id>`
- `?central_registry_id=<id>`
- `?offset=<n>&count=<n>`
- `?offset=<n>&count=<n>&name=<texto>`
- `?offset=<n>&count=<n>&doc=<texto>`
- `?offset=<n>&count=<n>&type=1|2`

Comportamento atual do backend:
- `offset=0`
- `count=10`
- `count` maximo:
  - Linux embarcado: `100`
  - ESP32/targets menores: `10`

### GET `/entities?id=456`

Response `200`:

```json
{
  "ret": 0,
  "entity_id": 456,
  "central_registry_id": 123,
  "type": 2,
  "enabled": true,
  "name": "Joao Silva Santos",
  "doc": "ABC1D23",
  "brand": "Toyota",
  "model": "Corolla",
  "color": "Branco",
  "lpr_enabled": true
}
```

### GET `/entities?central_registry_id=123`

Lista completa de pessoas e veiculos vinculados ao cadastro.

Response `200`:

```json
{
  "ret": 0,
  "central_registry_id": 123,
  "count": 2,
  "items": [
    {
      "entity_id": 111,
      "central_registry_id": 123,
      "type": 1,
      "enabled": true,
      "name": "Maria Oliveira",
      "doc": "12345678900"
    },
    {
      "entity_id": 222,
      "central_registry_id": 123,
      "type": 2,
      "enabled": true,
      "name": "Joao Silva Santos",
      "doc": "ABC1D23",
      "brand": "Toyota",
      "model": "Corolla",
      "color": "Branco",
      "lpr_enabled": true
    }
  ]
}
```

### GET `/entities?offset=0&count=10&name=joao&doc=ABC1D23&type=2`

Busca avancada com filtros combinados.

Response `200`:

```json
{
  "ret": 0,
  "count": 1,
  "total": 1,
  "items": [
    {
      "entity_id": 222,
      "central_registry_id": 123,
      "type": 2,
      "enabled": true,
      "name": "Joao Silva Santos",
      "doc": "ABC1D23",
      "brand": "Toyota",
      "model": "Corolla",
      "color": "Branco",
      "lpr_enabled": true
    }
  ]
}
```

### POST `/entities`

Pessoa:

```json
{
  "id": 0,
  "type": 1,
  "name": "Maria Oliveira",
  "doc": "12345678900",
  "central_registry_id": 123,
  "enabled": true
}
```

Veiculo:

```json
{
  "id": 0,
  "type": 2,
  "name": "Joao Silva Santos",
  "doc": "ABC1D23",
  "central_registry_id": 123,
  "lpr_enabled": true,
  "brand": "Toyota",
  "model": "Corolla",
  "color": "Branco",
  "enabled": true
}
```

Auto-ID com criacao automatica:

```json
{
  "id": 0,
  "createid": true,
  "type": 1,
  "name": "Cadastro automatico",
  "doc": "",
  "enabled": true
}
```

Response `200`:

```json
{
  "ret": 0,
  "id": 456,
  "entity_id": 456,
  "central_registry_id": 123,
  "created_central": 0,
  "updated_existing": 0
}
```

Erros comuns:
- `400`: nome ausente, placa invalida, payload invalido
- `404`: cadastro nao encontrado
- `409`: placa duplicada ou ID existente

### PUT `/entities?id=456`

Atualizacao parcial.

Request:

```json
{
  "name": "Joao Silva Santos",
  "brand": "Toyota",
  "model": "Corolla XEI",
  "color": "Prata",
  "lpr_enabled": true,
  "enabled": true
}
```

Response `200`:

```json
{
  "ret": 0,
  "entity_id": 456
}
```

### DELETE `/entities?id=456`

Response `200`:

```json
{
  "ret": 0
}
```

## Condutores de veiculo

### GET `/vehicle-drivers?vehicle_id=456`

Response `200`:

```json
{
  "ret": 0,
  "vehicle_id": 456,
  "driver_ids": [111, 222]
}
```

### PUT `/vehicle-drivers?vehicle_id=456`

Request:

```json
{
  "driver_ids": [111, 222]
}
```

Response `200`:

```json
{
  "ret": 0,
  "vehicle_id": 456,
  "count": 2
}
```

Erros comuns:
- `400`: `driver_ids` invalido, pessoa inexistente ou de outro cadastro
- `404`: veiculo nao encontrado

## Midias

Campos principais:
- `entity_id`
- `central_registry_id`
- `type`
- `description`
- `enabled`
- `expiration`

### GET `/media`

Consultas suportadas:
- `?id=<media_id>`
- `?entity_id=<entity_id>`

### GET `/media?id=789`

Response `200`:

```json
{
  "ret": 0,
  "media_id": 789,
  "entity_id": 456,
  "central_registry_id": 123,
  "type": 21,
  "description": "123,45678",
  "expiration": 0,
  "enabled": true
}
```

### GET `/media?entity_id=456`

Response `200`:

```json
{
  "ret": 0,
  "entity_id": 456,
  "count": 1,
  "items": [
    {
      "media_id": 789,
      "entity_id": 456,
      "central_registry_id": 123,
      "type": 21,
      "description": "123,45678",
      "expiration": 0,
      "enabled": true
    }
  ]
}
```

### POST `/media`

RFID:

```json
{
  "entity_id": 456,
  "central_registry_id": 123,
  "type": 21,
  "description": "123,45678",
  "enabled": true
}
```

LPR manual:

```json
{
  "entity_id": 456,
  "central_registry_id": 123,
  "type": 17,
  "description": "ABC1D23",
  "enabled": true
}
```

Response `200`:

```json
{
  "ret": 0,
  "media_id": 789
}
```

Notas:
- `entity_id` e `central_registry_id` sao obrigatorios
- para veiculos, prefira `lpr_enabled=true` no cadastro da entidade em vez de criar LPR manualmente
- duplicidade de midia pode retornar `409`

### PUT `/media?id=789`

Request:

```json
{
  "enabled": false,
  "expiration": 1767225600
}
```

Response `200`:

```json
{
  "ret": 0,
  "media_id": 789
}
```

### DELETE `/media?id=789`

Response `200`:

```json
{
  "ret": 0
}
```

## Dashboard

### GET `/dashboard`

Response `200`:

```json
{
  "ret": 0,
  "counts": {
    "cadastros": 100,
    "pessoas": 80,
    "veiculos": 20,
    "registros_memoria": 150
  },
  "midias": {
    "rfid": 120,
    "senha": 5,
    "biometria": 3,
    "lpr": 12
  }
}
```

## Device Info

### GET `/device-info`

Response `200`:

```json
{
  "ret": 0,
  "gid": 123456,
  "gid_str": "000-000-123456",
  "hw_model": "RXPPRO-MASTER",
  "fw_version": "1.2.3",
  "datetime_utc": "2026-03-09T12:00:00Z",
  "uptime_sec": 86400
}
```

## Configuracao

### GET `/config`

Retorna configuracao consolidada da controladora.

Campos usuais:
- `model`
- `eth`
- `wifi`
- `volume_master`
- `volumes`
- `LPR`
- `FACIAL`
- `BARCODE`
- `VOIP`
- `masters`
- `cvideoia`
- `master`
- `devgid`
- `ret`

### POST `/config`

Request parcial:

```json
{
  "devgid": 123456,
  "master": {
    "id": 1,
    "frase": "NOVA FRASE",
    "app": "RXPPRO"
  }
}
```

Notas:
- `devgid` deve ser o ID real do equipamento
- o endpoint aceita body parcial

## Webhooks

Slots suportados:
- `id=1..4`

### GET `/webhook`

Response `200`:

```json
{
  "ret": 0,
  "items": [
    {
      "id": 1,
      "url": "https://example.com/webhook",
      "registered": 1,
      "unregistered": 1,
      "sensors": 0,
      "logs": 0
    }
  ]
}
```

### GET `/webhook?id=1`

Consulta um slot especifico.

### POST `/webhook?id=1`

Request:

```json
{
  "url": "https://example.com/webhook",
  "registered": 1,
  "unregistered": 1,
  "sensors": 0,
  "logs": 0
}
```

Response `200`:

```json
{
  "ret": 0
}
```

### DELETE `/webhook?id=1`

Response `200`:

```json
{
  "ret": 0
}
```

## MQTT via WebSocket

Endpoint:

```text
wss://<host>/mbcortex/master/api/v1/mqtt
```

Notas:
- a autenticacao usa a mesma `session_key` do login HTTP
- use este endpoint para integracoes nativas via WebSocket/MQTT
