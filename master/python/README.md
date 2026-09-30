# MobiCortex Master - Demo Interativa

Exemplo simples e interativo de integração com a API REST da controladora **MobiCortex Master**.

> **Um arquivo principal** (`mbcortex_demo.py`) com menu interativo, mais exemplos avulsos em `examples/`.

---

## 📁 Estrutura

```
integration-samples/master/python/
├── mbcortex_demo.py       # Cliente MbcortexClient + menu interativo
├── mbcortex_config.json   # Configuracoes salvas (gerado automaticamente)
├── pyproject.toml         # Opcional para instalacao
├── examples/
│   ├── mqtt_subscribe.py  # MQTT TCP 1884 (mbcortex/export/event)
│   ├── webhook_server.py  # Receptor HTTP em 0.0.0.0 (LAN)
│   └── devices_outputs.py # GET /devices + POST /devices/relay (rele/DOUT)
├── src/                   # models.py / exceptions.py auxiliares (o demo nao importa)
└── README.md              # Este arquivo
```

---

## 🚀 Uso Rápido

### 1. Execute o script

```bash
cd integration-samples/master/python
python mbcortex_demo.py
```

### 2. Siga o menu interativo

```
╔══════════════════════════════════════════════════════════════╗
║           MOBICORTEX MASTER - DEMO INTERATIVA                ║
╚══════════════════════════════════════════════════════════════╝

============================================================
  MENU PRINCIPAL
============================================================

  🔐 CONFIGURAÇÃO
  [C] Configurar Conexão (IP, porta, login, senha)
  [T] Testar Conectividade

  📋 CADASTROS
  [1] Listar Cadastros Centrais (com paginacao)
  [2] Novo Cadastro Central
  [3] Buscar Cadastro por ID

  👥 ENTIDADES
  [4] Nova Pessoa
  [5] Novo Veiculo
  [6] Listar Entidades por Cadastro
  [7] Busca Avancada de Entidades (filtros + paginacao)

  🧪 TESTES RAPIDOS
  [8] Teste Completo - Modo AUTO (IDs automaticos)
  [9] Teste Completo - Modo FIXED (IDs fixos)

  🔌 SAIDAS
  [O] Dispositivos / Reles / DOUT (GET /devices + POST /devices/relay)

  ℹ️  INFORMACOES
  [I] Sobre o Sistema
  [0] Sair

Escolha uma opcao: 
```

---

## 📋 Funcionalidades

O script demonstra:
- ✅ **Login** na controladora
- ✅ Criar **unidades** (central-registry) - ID automático ou fixo
- ✅ Criar **veículos** (entities tipo 2) com LPR
- ✅ **Apagar** registros
- ✅ Tratamento de erros completo
- ✅ REST `/mqtt-export` e `/webhook` no cliente
- ✅ Exemplos de **MQTT TCP 1884** e **servidor webhook** na LAN (`examples/`)
- ✅ **Saídas** (relé / DOUT): `GET /devices` + `POST /devices/relay` (menu `[O]` e `examples/devices_outputs.py`)

---

## 🖥️ Menu Interativo

### [C] / [T] Conexão
Configure IP, porta HTTPS (443 ou 4449), senha e timeout; a configuração fica em `mbcortex_config.json`. `[T]` testa o login.

### [1]–[3] Cadastros centrais
Lista com paginação (`offset`/`count`), cria e busca por ID em `/central-registry`.

### [4] Nova Pessoa / [5] Novo Veículo
Digite **0** no ID da entidade para ID automático e **0** no cadastro central para criar um automaticamente:
- **Veículo:** cadastro central com o nome da **placa**
- **Pessoa:** cadastro central com o **nome da pessoa**

### [6] / [7] Consulta de entidades
Lista por cadastro central ou busca com filtros (`name`, `doc`, `type`) e paginação.

### [8] Modo AUTO / [9] Modo FIXED
Teste completo (cria unidade + veículo com LPR e, opcionalmente, apaga no final). No AUTO a controladora gera os IDs; no FIXED você informa (recomendado: >= 2.000.000).

### [O] Saídas (relé / DOUT)
Lista os dispositivos (`GET /devices`) com as saídas e os `cmd` aceitos, e aciona uma saída (`POST /devices/relay`) por `id`, com pulso em ms ou `on`/`off`/`toggle`.

---

## 📖 Exemplo de Execução

```bash
$ python mbcortex_demo.py

╔══════════════════════════════════════════════════════════════╗
║           MOBICORTEX MASTER - DEMO INTERATIVA                ║
╚══════════════════════════════════════════════════════════════╝

============================================================
  MENU PRINCIPAL
============================================================

  (...)

Escolha uma opcao: 8

============================================================
  TESTE MODO AUTO
============================================================

Neste modo, a controladora gera automaticamente os IDs.

============================================================
  CONFIGURACAO DA CONEXAO
============================================================
  [Valores anteriores carregados - pressione ENTER para manter]

IP ou hostname da controladora [192.168.0.21]: 
Porta HTTPS [4449]: 
Usuario [master]: 
Senha [1234]: 
Timeout (segundos) [10]: 

============================================================
  Dados do Veiculo
============================================================
Placa do veiculo [ABC1234]: XYZ9876
Ativar LPR? (S/n): 
Apagar unidade no final? (s/N): 

============================================================
  RESUMO DA CONFIGURACAO
============================================================

  URL:      https://192.168.0.10
  Usuario:  master
  Senha:    1234
  Timeout:  10s
  Modo:     AUTO
  Placa:    XYZ9876
  LPR:      Sim
  Cleanup:  Nao

Confirmar e executar? (S/n): s

────────────────────────────────────────────────────────────
► PASSO 1: Autenticacao (login)
────────────────────────────────────────────────────────────
Conectando a: https://192.168.0.10
  ✓ Login OK - Session: a1b2c3d4e5f6...

────────────────────────────────────────────────────────────
► PASSO 2: Criar unidade (ID automatico)
────────────────────────────────────────────────────────────
  ✓ Unidade criada: ID=4294000000

────────────────────────────────────────────────────────────
► PASSO 3: Criar veiculo (ID automatico)
────────────────────────────────────────────────────────────
  ✓ Veiculo criado: ID=12345

────────────────────────────────────────────────────────────
► PASSO 4: Apagar veiculo
────────────────────────────────────────────────────────────
  ✓ Veiculo apagado

============================================================
  TESTE CONCLUIDO COM SUCESSO!
============================================================

Pressione ENTER para voltar ao menu...
```

---

## 🔧 Requisitos

- **Python 3.8+**
- **Apenas biblioteca padrão** (urllib, json, getpass)
- Acesso à controladora MobiCortex Master na rede

---

## 🐍 API Python (Uso Programático)

Você também pode importar o cliente em seu código:

```python
from mbcortex_demo import MbcortexClient, MbcortexError

# Cria cliente
client = MbcortexClient(
    base_url="https://192.168.0.10",
    username="master",
    password="1234",
    timeout=10.0
)

# Login
session = client.login()

# Criar unidade com ID automatico
unit_id = client.create_unit_auto(name="Minha Unidade", enabled=1)

# Criar veiculo com LPR
vehicle_id = client.create_vehicle_auto(
    unit_id=unit_id,
    name="Meu Carro",
    plate="ABC1234",
    lpr_ativo=1
)

# Apagar
client.delete_vehicle(vehicle_id)
client.delete_unit(unit_id)
```

---

## MQTT export e webhook

A controladora **não** expõe MQTT via WebSocket. Porta **1884** (usuário/senha de Settings > MQTT), tópico `mbcortex/export/event`. Porta **1883** no equipamento é IPC em loopback.

```bash
# Assina o tópico de export (apenas biblioteca padrão)
python examples/mqtt_subscribe.py 192.168.0.180 1884 mqttuser mqttpass

# Recebe POST da placa em 0.0.0.0:9099
python examples/webhook_server.py 9099

# Relés / DOUT (Linux Master)
python examples/devices_outputs.py
python examples/devices_outputs.py 42 relay 1 1000
python examples/devices_outputs.py 42 dout 3 500
```

Grave na controladora `http://<IP_LAN_DESTE_PC>:9099/webhook` — **não** use localhost. Ative registered + unregistered. No Windows, libere o firewall se a placa der `Connection timed out`. Evite a porta 8080 se o `filesync-win64` já estiver nela.

REST no cliente (`login()` primeiro):

```python
client.get_mqtt_export()
client.save_mqtt_export_user(1, "SDK test", "mqttuser", "mqttpass")
client.save_webhook(1, "http://192.168.0.3:9099/webhook", registered=1, unregistered=1)
```

## 🔌 Endpoints da API

| Operação | Método | Endpoint |
|----------|--------|----------|
| Login | POST | `/mbcortex/master/api/v1/login` |
| Criar Unidade | POST | `/mbcortex/master/api/v1/central-registry` |
| Apagar Unidade | DELETE | `/mbcortex/master/api/v1/central-registry?id=X` |
| Criar Veículo | POST | `/mbcortex/master/api/v1/entities` |
| Apagar Veículo | DELETE | `/mbcortex/master/api/v1/entities?id=X` |
| MQTT export | GET/POST | `/mbcortex/master/api/v1/mqtt-export` |
| Devices / outputs | GET/POST | `/mbcortex/master/api/v1/devices`, `/devices/relay` |
| Webhook | GET/POST/DELETE | `/mbcortex/master/api/v1/webhook?id=1..4` |

---

## ⚠️ Tratamento de Erros

O script lida automaticamente com:
- **401** - Falha de autenticação (senha incorreta)
- **400** - Dados inválidos (placa mal formatada)
- **404** - Recurso não encontrado
- **409** - Conflito (ID/placa duplicado)
- **5xx** - Erros do servidor

---

## 📝 Notas

- **Placa**: É normalizada automaticamente (maiúsculas, sem espaços/hífens)
- **ID Automático**: Controladora usa faixa 4294000000+
- **ID Fixo**: Use valores >= 2000000 para testes
- **LPR**: Cria mídia de reconhecimento de placa automaticamente
- **Sessão**: Expira em 900 segundos (15 minutos)
- **Configurações Salvas**: O script salva automaticamente IP, porta, usuário e senha no arquivo `mbcortex_config.json` para facilitar testes futuros. Pressione ENTER para usar os valores salvos.

---

## 📄 Licença

MIT License - Exemplo educativo para integração com MobiCortex Master.
