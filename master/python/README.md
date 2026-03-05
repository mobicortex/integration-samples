# MobiCortex Master - Demo Interativa

Exemplo simples e interativo de integração com a API REST da controladora **MobiCortex Master**.

> **Um único arquivo Python** - apenas execute e siga o menu interativo!

---

## 📁 Estrutura

```
integration-samples/master/python/
├── mbcortex_demo.py       # ← UNICO ARQUIVO (contem tudo!)
├── mbcortex_config.json   # Configuracoes salvas (gerado automaticamente)
├── pyproject.toml         # Opcional para instalacao
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

  [1] Teste Rapido - Modo AUTO (IDs automaticos)
  [2] Teste Rapido - Modo FIXED (IDs fixos)
  [3] Cadastrar um carro
  [4] Sobre
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

---

## 🖥️ Menu Interativo

### Opção [1] Modo AUTO (IDs automáticos)
A controladora gera automaticamente os IDs para unidade e veículo.

### Opção [2] Modo FIXED (IDs fixos)
Você informa os IDs desejados (recomendado: >= 2.000.000).

### Opção [3] Cadastrar um carro ⭐ **RECOMENDADO**
Modo flexível - digite **0** para IDs automáticos ou informe IDs específicos.

**🆕 NOVO:** Se informar **0** no ID da Unidade:
- **Veículo:** Cria cadastro central com o nome da **placa**
- **Pessoa:** Cria cadastro central com o **nome da pessoa**

### Fluxo de perguntas:
1. **IP/Hostname** da controladora
2. **Porta** HTTPS (padrão: 443)
3. **Usuário** (padrão: master) - informativo
4. **Senha** (padrão: 1234)
5. **Timeout** (padrão: 10s)
6. **ID da Unidade** (0 = cria automático com nome da placa)
7. **ID do Veículo** (0 = automático)
8. **Placa** do veículo (padrão: ABC1234)
9. **LPR** ativar? (padrão: sim)
10. **Cleanup** apagar unidade no final? (padrão: não)

Após configurar, mostra um **resumo** e pede confirmação antes de executar.

---

## 🚗 Opção [3] Cadastrar um carro

A opção mais flexível! Você escolhe se quer IDs automáticos ou fixos:

```
============================================================
  CADASTRAR UM CARRO
============================================================

Informe os IDs desejados ou 0 para gerar automaticamente.

============================================================
  IDs dos Registros
============================================================
  (digite 0 para gerar automaticamente)

ID da Unidade [0]: 0          ← digite 0 para auto, ou um ID específico
ID do Veiculo [0]: 0          ← digite 0 para auto, ou um ID específico
```

**Comportamento:**
| Unidade | Veículo | Resultado |
|---------|---------|-----------|
| 0 | 0 | Cadastro criado com nome da placa, veículo automático |
| 2000001 | 0 | Unidade fixa existente, veículo automático |
| 0 | 3000001 | Cadastro criado com nome da placa, veículo fixo |
| 2000001 | 3000001 | Ambos fixos |

> 💡 **Dica:** Quando informar **0** no ID da Unidade, o sistema cria automaticamente um cadastro central:
> - Para **veículos**: usa a **placa** como nome do cadastro
> - Para **pessoas**: usa o **nome** como nome do cadastro
> 
> Útil quando não quer associar a um cadastro existente!

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

  [1] Teste Rapido - Modo AUTO (IDs automaticos)
  [2] Teste Rapido - Modo FIXED (IDs fixos)
  [3] Cadastrar um carro
  [4] Sobre
  [0] Sair

Escolha uma opcao: 3

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

## 🔌 Endpoints da API

| Operação | Método | Endpoint |
|----------|--------|----------|
| Login | POST | `/mbcortex/master/api/v1/login` |
| Criar Unidade | POST | `/mbcortex/master/api/v1/central-registry` |
| Apagar Unidade | DELETE | `/mbcortex/master/api/v1/central-registry?id=X` |
| Criar Veículo | POST | `/mbcortex/master/api/v1/entities` |
| Apagar Veículo | DELETE | `/mbcortex/master/api/v1/entities?id=X` |

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
