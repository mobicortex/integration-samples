# MobiCortex Master - SDK Node.js 📚

**SDK completo e bem comentado** para integração com a API REST da controladora **MobiCortex Master** usando Node.js.

## 🎯 Estruturado como Biblioteca Reutilizável

✅ **USO DUPLO:** CLI interativo + Biblioteca para seu código  
✅ **BEM COMENTADO:** Código educativo com comentários extensos  
✅ **ZERO DEPENDÊNCIAS:** Usa apenas recursos nativos do Node.js 18+  
✅ **CAMPOS OPCIONAIS:** Suporte completo a marca, modelo, cor para veículos  
✅ **ROBUSTO:** Tratamento específico de erros e tipos de exceção  

---

## 📋 Funcionalidades Completas

### 🚀 Como CLI Interativo:
- 🔐 **Menu de configuração** (IP, porta, login, senha)
- 🧪 **Testes rápidos** (AUTO e FIXED IDs)  
- 🏢 **Gestão de cadastros** centrais (unidades/empresas)
- 👥 **Gestão de entidades** (pessoas e veículos)
- 🚗 **Campos opcionais** para veículos (marca, modelo, cor)
- 🆕 **Cadastro automático**: 
  - **Veículo:** Cria cadastro com nome da **placa** quando informar 0
  - **Pessoa:** Cria cadastro com **nome da pessoa** quando informar 0
- 🔍 **Consultas e paginação**
- 📊 **Estatísticas do sistema**

### 📚 Como Biblioteca:
- ✅ Autenticação Bearer token (session_key, 15min TTL)
- ✅ Criar unidades (central-registry) com ID automático ou fixo  
- ✅ Criar veículos (entities tipo 2) com campos opcionais
- ✅ Apagar veículos e unidades
- ✅ Listagens com paginação  
- ✅ Filtros e busca avançada
- ✅ Tratamento robusto de erros HTTP com classes específicas

---

## 📁 Estrutura da Biblioteca

```
nodejs/
├── package.json              # Configuração do SDK
├── README.md                 # Documentação (este arquivo)
├── main.js                   # [DEPRECATED] Redirecionamento
│
├── src/                      # 📚 BIBLIOTECA (reutilizável)
│   ├── index.js              # Exports principais
│   ├── mbcortexClient.js     # Cliente HTTP da API
│   ├── models.js             # Classes (Vehicle, CentralRegistry, etc)
│   └── exceptions.js         # Exceções customizadas
│
└── examples/                 # 📋 EXEMPLOS DE USO
    ├── cli.js                # CLI interativo completo
    └── basic_usage.js        # Exemplo básico como biblioteca
```

---

## 🚀 Requisitos

- **Node.js 18+** (para fetch nativo)
- Acesso à controladora MobiCortex Master na rede local
- Credenciais válidas (usuário/senha)

---

## 💻 Como Usar

### 1. 📥 Instalação
```bash
# Apenas Node.js é necessário - sem npm install
cd integration-samples/master/nodejs
```

### 2. 🖥️ CLI Interativo (Recomendado)
```bash
# Menu completo com configuração
npm run cli
# ou
node examples/cli.js
```

**🔧 Problemas de Conectividade SSL:**

Controladores industriais frequentemente usam **certificados self-signed** que o Node.js rejeita por padrão. Se você vir erros como:
- `fetch failed`
- `certificate error` 
- `SSL error`
- `ECONNRESET`

**Solução para ambiente de desenvolvimento/teste:**
```bash
# Windows (PowerShell)
$env:NODE_TLS_REJECT_UNAUTHORIZED="0"
npm run cli

# Linux/Mac (Bash)
export NODE_TLS_REJECT_UNAUTHORIZED=0
npm run cli

# Ou diretamente no comando
NODE_TLS_REJECT_UNAUTHORIZED=0 npm run cli
```

⚠️ **Importante:** Use `NODE_TLS_REJECT_UNAUTHORIZED=0` **apenas em desenvolvimento/teste**. Em produção, configure certificados válidos.

**🔍 Diagnóstico de Problemas:**
- `Conexão recusada` → Verifique IP/porta e se a controladora está ligada
- `Host não encontrado` → Verifique o IP/hostname  
- `Conexão resetada` → Verifique se a porta HTTPS está correta (443 ou 4449)
- `fetch failed` → Geralmente problema SSL ou controladora desligada

**Menu do CLI:**
```
🔐 CONFIGURAÇÃO
[C] Configurar Conexão (IP, porta, login, senha)  
[T] Testar Conectividade

🧪 TESTES RÁPIDOS  
[1] Teste Completo - Modo AUTO (IDs automáticos)
[2] Teste Completo - Modo FIXED (IDs fixos)

🏢 CADASTROS CENTRAIS
[3] Listar Cadastros com Paginação
[4] Novo Cadastro Central  
[5] Buscar Cadastro por ID

👥 ENTIDADES
[6] Nova Pessoa (0 no cadastro = cria automático com nome)
[7] Novo Veículo (0 no cadastro = cria automático com placa)
[8] Listar Entidades por Cadastro
[9] Busca Avançada de Entidades

ℹ️  INFORMAÇÕES
[I] Sobre o Sistema
[0] Sair
```

### 3. 📚 Como Biblioteca no Seu Código

```javascript
// Import da biblioteca
const { MbcortexClient } = require('./src');

// Instancia cliente
const client = new MbcortexClient('https://192.168.0.21:4449', 'admin');

// Autentica
await client.login();

// Cria unidade
const unitId = await client.createUnitAuto("Minha Empresa");

// Cria veículo COM CAMPOS OPCIONAIS
const vehicleId = await client.createVehicleAuto(
  unitId,
  "João Silva",      // Nome
  "ABC-1234",        // Placa (normalizada automaticamente)  
  1,                 // LPR ativo
  {                  // 🆕 CAMPOS OPCIONAIS
    brand: "Toyota", 
    model: "Corolla",
    color: "Prata",
    obs: "Veículo da diretoria"
  }
);

console.log(`Veículo criado: ${vehicleId}`);
```

### 4. 📖 Exemplo Básico Completo
```bash
# Executa exemplo completo como biblioteca
npm run basic
# ou  
node examples/basic_usage.js
```

---

## 🔧 Configuração

### Primeira execução:
1. Execute `npm run cli`
2. Escolha **[C] Configurar Conexão**
3. Informe IP/porta da controladora
4. Configure usuário/senha
5. Teste conectividade com **[T]**

### Configuração salva em: `examples/cli-config.json`

---

## 🔍 Funcionalidades de Consulta (Novo!)

### 📋 Listar Cadastros Centrais com Paginação

Lista todos os cadastros centrais (unidades/empresas) cadastrados na controladora:

```javascript
// Via CLI: Opção [3] Listar Cadastros com Paginação
// Navegação interativa com [P]anterior [N]próxima [0]sair

// Via biblioteca:
const result = await client.listCentralRegistries(offset=0, count=10);
console.log(`Total: ${result.total} cadastros`);
result.items.forEach(item => {
  console.log(`ID: ${item.id} | Nome: ${item.name} | Status: ${item.enabled ? 'Ativo' : 'Inativo'}`);
});
```

### 🔎 Buscar Cadastro por ID

Busca detalhes de um cadastro específico:

```javascript
// Via CLI: Opção [5] Buscar Cadastro por ID
// Mostra detalhes completos + opção de listar entidades

// Via biblioteca:
const registry = await client.getCentralRegistry(unitId);
if (registry) {
  console.log(`Nome: ${registry.name}`);
  console.log(`Status: ${registry.enabled ? 'Ativo' : 'Inativo'}`);
  console.log(`Slots: T1=${registry.slots1}, T2=${registry.slots2}`);
}
```

### 👥 Listar Entidades por Cadastro

Lista pessoas e veículos de um cadastro específico:

```javascript
// Via CLI: Opção [8] Listar Entidades por Cadastro
// Mostra pessoas e veículos com paginação

// Via biblioteca:
const entities = await client.listEntitiesByUnit(unitId, offset=0, count=10);
entities.items.forEach(entity => {
  const type = entity.type === 1 ? 'Pessoa' : 'Veículo';
  console.log(`${type}: ${entity.name} | Doc: ${entity.doc}`);
});
```

### 🔍 Busca Avançada de Entidades

Busca entidades com filtros personalizados:

```javascript
// Via CLI: Opção [9] Busca Avançada de Entidades
// Filtros por tipo, nome, documento e cadastro

// Via biblioteca:
const filters = {
  type: 2,           // 1=pessoa, 2=veículo
  name: "João",      // Parte do nome
  doc: "ABC",        // Parte do documento/placa
  cadastro_id: 123   // ID do cadastro específico
};

const result = await client.searchEntities(filters, offset=0, count=10);
console.log(`Encontrados: ${result.total} resultados`);
```

---

## 🆕 Cadastro Automático (Novo!)

Quando você quer cadastrar uma pessoa ou veículo **sem associar a um cadastro central existente**, informe **0** no ID do Cadastro Central:

- **Veículo:** Cadastro criado com o nome da **placa**
- **Pessoa:** Cadastro criado com o **nome da pessoa**

### Exemplo - Veículo:
```
============================================================
  NOVO VEÍCULO
============================================================

(digite 0 para gerar ID automaticamente)
ID do Veículo [0]: 0

(digite 0 para criar cadastro automático com nome da placa)
ID do Cadastro Central [0]: 0          ← 0 = cria automático!

Nome do Proprietário: João Silva
Placa: ABC1234

🏢 Criando cadastro central automático com nome 'ABC1234'...
   ✅ Cadastro criado: ID=4294000001

🚗 Criando veículo...
   ✅ Veículo criado: ID=12345
```

### Exemplo - Pessoa:
```
============================================================
  NOVA PESSOA
============================================================

(digite 0 para gerar ID automaticamente)
ID da Pessoa [0]: 0

(digite 0 para criar cadastro automático com nome da pessoa)
ID do Cadastro Central [0]: 0          ← 0 = cria automático!

Nome: Maria Oliveira
CPF/Documento: 12345678901

🏢 Criando cadastro central automático com nome 'Maria Oliveira'...
   ✅ Cadastro criado: ID=4294000002

👤 Criando pessoa...
   ✅ Pessoa criada: ID=12346
```

### Como Biblioteca:
```javascript
// VEÍCULO: Cria cadastro automaticamente com nome da placa
const unitId = 0; // 0 = cria automático
const plate = "ABC1234";
const vehicleId = await client.createVehicleAuto(unitId, "João Silva", plate, 1);

// PESSOA: Cria cadastro automaticamente com nome da pessoa
const unitId = 0; // 0 = cria automático
const personName = "Maria Oliveira";
const personId = await client.createPersonAuto(unitId, personName, "12345678901");
```

---

## 🚗 Campos Opcionais para Veículos

O SDK suporta **todos os campos opcionais** da API:

```javascript
const options = {
  brand: "Toyota",           // Marca do veículo
  model: "Corolla",          // Modelo do veículo  
  color: "Prata",            // Cor do veículo
  obs: "Observações gerais"  // Campo livre para anotações
};

// Usar com qualquer método de criação
await client.createVehicleAuto(unitId, name, plate, 1, options);
await client.createVehicleFixed(vehicleId, unitId, name, plate, 1, options);
```

---

## 🎯 Campos Obrigatórios vs Opcionais

### 📋 Cadastro Central (Unidade):
```javascript
// Obrigatórios
name: "Nome da Unidade"

// Opcionais (com padrões)  
enabled: 1,        // 1=ativo, 0=inativo
slots1: 8,         // Slots comunicação (0-15)
slots2: 0,         // Slots comunicação (0-15)
type: 0            // Tipo unidade (0=padrão)
```

### 🚗 Veículo:
```javascript
// Obrigatórios
unitId: 123,           // ID do cadastro central
name: "João Silva",    // Nome proprietário 
plate: "ABC1234",      // Placa (normalizada automaticamente)

// Opcionais
lpr_ativo: 1,          // 1=LPR ativo, 0=inativo (padrão: 1)
brand: "Toyota",       // Marca do veículo
model: "Corolla",      // Modelo do veículo
color: "Prata",        // Cor do veículo  
obs: "Observações"     // Campo livre
```

---

## 🛡️ Tratamento de Erros

O SDK usa **exceções específicas** para cada tipo de erro:

```javascript
const { AuthError, ValidationError, ConflictError } = require('./src');

try {
  await client.login();
} catch (error) {
  if (error instanceof AuthError) {
    console.log('🔐 Erro de autenticação - verifique senha');
  } else if (error instanceof ValidationError) {
    console.log('📝 Dados inválidos - verifique formato');  
  } else if (error instanceof ConflictError) {
    console.log('⚔️ ID/placa já existe - use outros valores');
  }
}
```

**Tipos de exceção:**
- `AuthError` - Login/senha inválidos ou token expirado
- `ValidationError` - Dados mal formatados (HTTP 400)
- `ConflictError` - ID/placa duplicados (HTTP 409)  
- `NotFoundError` - Recurso não existe (HTTP 404)
- `ServerError` - Erro interno controladora (HTTP 5xx)

---

## 🔍 Normalização de Placas

Placas são **automaticamente normalizadas:**

```javascript
const { Vehicle } = require('./src');

// Todas viram "ABC1234"
Vehicle.normalizePlate("abc-1234");  // → "ABC1234"
Vehicle.normalizePlate("ABC 1234");  // → "ABC1234" 
Vehicle.normalizePlate("abc1234");   // → "ABC1234"

// Validação de formato
const veiculo = new Vehicle(0, "João", "ABC1234", unitId);
console.log(veiculo.validatePlate()); // true para formato brasileiro
```

---  
node main.js --base-url http://192.168.0.10 --password minhasenha
```

### 3. Modo Avançado (IDs Específicos)
```bash
# IDs fixos para teste (faixa 2000000+)
node main.js --base-url http://192.168.0.10 --mode fixed --unit-id 2000050 --vehicle-id 2000051

# Com limpeza automática da unidade
node main.js --base-url http://192.168.0.10 --mode auto --cleanup-unit
```

### 4. Ajuda Completa
```bash
node main.js --help
```

---

## Fluxo de Execução

O CLI executa automaticamente este fluxo:

1. **🔑 Login**: Autentica e obtém `session_key` (15min TTL)
2. **🏢 Criar Unidade**: Central-registry com configurações padrão
3. **🚗 Criar Veículo**: Entity tipo 2 com placa e `lpr_ativo=1` 
4. **🗑️ Apagar Veículo**: Remove o veículo criado
5. **🗑️ Apagar Unidade**: Remove unidade (se `--cleanup-unit`)

### Exemplo de Saída:
```
🚀 MobiCortex Master - Exemplo Node.js
=====================================
URL: http://192.168.0.10
Modo: auto
Placa: ABC1234

📝 Passo 1: Fazendo login...
→ POST http://192.168.0.10/mbcortex/master/api/v1/login
← 200 OK
✅ Login realizado com sucesso

🏢 Passo 2: Criando unidade...
→ POST http://192.168.0.10/mbcortex/master/api/v1/central-registry
✅ Unidade criada com ID: 4294000001

🚗 Passo 3: Criando veículo... 
→ POST http://192.168.0.10/mbcortex/master/api/v1/entities
✅ Veículo criado com ID: 1234567
   Placa: ABC1234 | LPR: ATIVO

🗑️  Passo 4: Removendo veículo...
✓ Veículo 1234567 removido

🎉 Fluxo concluído com sucesso!
```

---

## Parâmetros Disponíveis

| Flag | Obrigatório | Padrão | Descrição |
|------|-------------|--------|-----------|
| `--base-url` | ✅ Sim | - | URL da controladora |
| `--password` | ❌ Não | `admin` | Senha da controladora |
| `--mode` | ❌ Não | `auto` | `auto` ou `fixed` |
| `--unit-id` | ❌ Não | `2000001` | ID específico da unidade (modo `fixed`) |
| `--vehicle-id` | ❌ Não | `2000002` | ID específico do veículo (modo `fixed`) |  
| `--plate` | ❌ Não | `ABC1234` | Placa do veículo de teste |
| `--cleanup-unit` | ❌ Não | `false` | Remove unidade ao final |
| `--help` | ❌ Não | - | Mostra ajuda completa |

---

## Como Usar Como Biblioteca

O cliente pode ser usado programaticamente:

```javascript
const MbcortexClient = require('./src/mbcortexClient');

async function exemplo() {
  const client = new MbcortexClient('http://192.168.0.10');
  
  // Login
  await client.login();
  
  // Criar unidade (auto-ID)
  const unitId = await client.createUnitAuto('Minha Unidade'); 
  
  // Criar veículo (auto-ID)
  const vehicleId = await client.createVehicleAuto(unitId, 'Carro Teste', 'XYZ9999');
  
  // Cleanup
  await client.deleteVehicle(vehicleId);
  await client.deleteUnit(unitId);
}
```

---

## Endpoints da API Utilizados

**Base Path**: `/mbcortex/master/api/v1/`

### Autenticação
- **POST** `/login`
  - Body: `{"pass":"admin"}`  
  - Response: `{"ret":0,"session_key":"<64hex>","expires_in":900}`
  - **Uso**: Todas as chamadas seguintes usam `Authorization: Bearer <session_key>`

### Unidades (Central Registry)
- **POST** `/central-registry`
  - Body: `{"id":0|<uint>, "name":"...", "enabled":1, "slots1":8, "slots2":0, "type":0}`
  - **id=0**: Auto-ID (faixa `4294000000+`)
  - **id>0**: ID específico (teste: faixa `2000000+`)

- **DELETE** `/central-registry?id=<id>`
  - Remove unidade específica

### Veículos (Entities)  
- **POST** `/entities`
  - **Auto-ID**: `{"createid":true, "tipo":2, "name":"...", "doc":"PLACA", "cadastro_id":<unit_id>, "lpr_ativo":1}`
  - **ID fixo**: `{"id":<uint>, "tipo":2, "name":"...", "doc":"PLACA", "cadastro_id":<unit_id>, "lpr_ativo":1}`

- **DELETE** `/entities?id=<entity_id>`
  - Remove veículo específico

---

## Notas Técnicas

### IDs e Ranges
- **Auto-ID Unidades**: Faixa `4294000000+` (WEB_PRINCIPAL_ID_BASE)
- **Auto-ID Veículos**: Gerados dinamicamente pela controladora
- **IDs de Teste**: Use faixa `2000000+` (2E6) para evitar conflitos

### Placas
- **Normalização**: Automática (maiúscula, sem espaços/hífens)
- **Validação**: Placa inválida retorna `400 "placa invalida"`
- **Duplicidade**: Placa duplicada retorna `409 Conflict`

### Autenticação
- **Session Key**: Token de 64 caracteres hexadecimais
- **TTL**: 15 minutos (900 segundos)
- **Renovação**: Faça novo login quando expirar

### LPR (License Plate Recognition)
- **Campo**: `lpr_ativo: 1` habilita reconhecimento de placa
- **Uso**: Permite que câmeras reconheçam automaticamente o veículo pela placa

---

## Solução de Problemas

### Erros Comuns

**401 Unauthorized**
```
→ Session key expirou ou inválido
→ Solução: Fazer novo login()
```

**400 Bad Request** 
```
→ Placa inválida ou campos obrigatórios ausentes
→ Verificar formato da placa e payload
```

**409 Conflict**
```
→ ID ou placa já existem
→ Usar IDs diferentes ou deletar recursos existentes
```

**Conexão Recusada**
``` 
→ Controladora offline ou URL incorreta
→ Verificar IP e porta da controladora
```

### Debug Avançado
```bash
# Executar com logs detalhados
node main.js --base-url http://192.168.0.10 --mode auto

# O cliente exibe automaticamente:
# → Requisições HTTP (método + URL + body)  
# ← Respostas HTTP (status + body JSON)
```

---

## Desenvolvimento

### Estrutura do Código
- **`src/mbcortexClient.js`**: Cliente HTTP reutilizável com comentários educacionais
- **`main.js`**: CLI demonstrativo com parse manual de argumentos
- **Zero dependências**: Apenas Node.js 18+ (fetch nativo)

### Comentários Educacionais
O código inclui comentários explicativos sobre:
- ✅ Conceitos da API MobiCortex (IDs, ranges, LPR)
- ✅ Padrões de payload (campos obrigatórios, valores padrão)
- ✅ Comportamentos não óbvios (auto vs fixed ID)
- ✅ Tratamento de erros específicos HTTP
- ✅ Exemplos de uso de cada função

---

## 📚 Exports da Biblioteca

```javascript
const {
  // Cliente principal
  MbcortexClient,
  
  // Exceções específicas  
  MbcortexError, AuthError, ValidationError, 
  ConflictError, NotFoundError, ServerError,
  
  // Classes de modelo
  CentralRegistry, Entity, Person, Vehicle,
  PaginatedResponse
  
} = require('./src');
```

---

## 🤝 Contribuição

Este é um **SDK educativo** para demonstrar integração com MobiCortex Master.

---

**MobiCortex Team** © 2026
