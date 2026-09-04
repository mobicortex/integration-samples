# Teste manual

## MQTT export (TCP 1884)

```bash
node examples/mqtt_subscribe.js 192.168.0.180 1884 mqttuser mqttpass
```

Esperado: CONNACK, SUBACK, depois JSON indentado de `status` / `lpr` no tópico `mbcortex/export/event`.

## Webhook na LAN

```bash
node examples/webhook_server.js 9099
```

Grave `http://<IP_LAN>:9099/webhook` na placa. Não use localhost. Se timeout, libere o firewall.

## CLI

Teste das novas funcionalidades:

Teste das novas funcionalidades:

1. Execute: npm run cli
2. Digite: 3
3. Deve mostrar: LISTAR CADASTROS CENTRAIS
4. Digite: 0 para voltar

Teste também:
- Opção 4: Novo Cadastro Central  
- Opção 5: Buscar Cadastro por ID