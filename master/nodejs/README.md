# MobiCortex Master - Exemplo Node.js

Exemplo simples de integracao com a API REST da controladora **MobiCortex Master** usando Node.js.

Este projeto demonstra como:
- Autenticar (login) na controladora
- Criar unidades (central-registry) com ID automatico ou fixo
- Criar veiculos (entities tipo 2) com LPR ativo
- Apagar veiculos e unidades

> Nota: Este exemplo usa apenas recursos nativos do Node.js (fetch). Sem dependencias externas.

---

## Requisitos

- Node.js 18 ou superior
- Acesso a controladora MobiCortex Master na rede

---

## Estrutura (esperada)

```
.
├── package.json
├── README.md
├── main.js
└── src/
    └── mbcortexClient.js
```

---

## Base Path

Prefixo das rotas REST:

```
/mbcortex/master/api/v1
```

---

## CLI (fluxo esperado)

O exemplo deve:
1. Fazer login e obter `session_key`
2. Criar unidade (auto ou fixed)
3. Criar veiculo (auto ou fixed) com `lpr_ativo=1`
4. Apagar o veiculo criado
5. Opcionalmente apagar a unidade

---

## Como rodar (quando implementado)

```bash
node main.js --base-url http://192.168.0.10 --mode auto
```

Flags esperadas:
- `--base-url` (obrigatorio)
- `--password` (default: admin)
- `--mode` (auto|fixed)
- `--unit-id` (modo fixed)
- `--vehicle-id` (modo fixed)
- `--plate` (default: ABC1234)
- `--cleanup-unit` (opcional)

---

## Endpoints usados

- POST `/mbcortex/master/api/v1/login`
- POST `/mbcortex/master/api/v1/central-registry`
- DELETE `/mbcortex/master/api/v1/central-registry?id=<id>`
- POST `/mbcortex/master/api/v1/entities`
- DELETE `/mbcortex/master/api/v1/entities?id=<id>`
