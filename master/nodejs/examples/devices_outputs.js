#!/usr/bin/env node

/**
 * Outputs (DOUT / Relays) — GET /devices + POST /devices/relay
 *
 * USO:
 *   NODE_TLS_REJECT_UNAUTHORIZED=0 node examples/devices_outputs.js
 *   MOBICORTEX_URL=https://192.168.120.45:4449 MOBICORTEX_PASS=admin node examples/devices_outputs.js
 *
 * Args opcionais:
 *   node examples/devices_outputs.js [deviceId] [relay|dout] [n] [timeMs|cmd]
 *   node examples/devices_outputs.js 42 relay 1 1000
 *   node examples/devices_outputs.js 42 dout 3 500
 *   node examples/devices_outputs.js 42 relay 1 on
 */

if (process.env.NODE_TLS_REJECT_UNAUTHORIZED === undefined) {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

const { MbcortexClient } = require('../src');

function formatOutput(o) {
  if (o.relay != null) return `relay:${o.relay}`;
  if (o.dout != null) return `dout:${o.dout}${o.nome ? ` (${o.nome})` : ''}`;
  return JSON.stringify(o);
}

async function main() {
  const baseUrl = process.env.MOBICORTEX_URL || 'https://192.168.120.45:4449';
  const password = process.env.MOBICORTEX_PASS || 'admin';
  const [deviceId, kind, numStr, actionOrTime] = process.argv.slice(2);

  const client = new MbcortexClient(baseUrl, password);
  console.log(`Login @ ${baseUrl}`);
  await client.login();

  const list = await client.listDevices();
  const items = list.items || [];
  console.log(`\nGET /devices — ${items.length} device(s)\n`);

  for (const d of items) {
    const online = d.online == null ? '—' : (d.online ? 'online' : 'offline');
    console.log(`  [${d.id}] ${d.nome}  tipo=${d.tipo}  modelo=${d.modelo || ''}  ${online}`);
    for (const o of d.saidas || []) {
      const cmds = (o.cmd || []).join(',');
      console.log(`      ${formatOutput(o)}  cmd=[${cmds}]  estado=${o.estado || '—'}`);
    }
  }

  if (!deviceId) {
    console.log('\nSem comando. Passe args para acionar, ex.:');
    console.log('  node examples/devices_outputs.js 42 relay 1 1000');
    console.log('  node examples/devices_outputs.js 42 dout 3 500');
    console.log('  node examples/devices_outputs.js 42 relay 1 on');
    return;
  }

  const n = parseInt(numStr, 10);
  const body = { id: deviceId };
  if (kind === 'dout') body.dout = n;
  else body.relay = n;

  if (actionOrTime === 'on' || actionOrTime === 'off' || actionOrTime === 'toggle' || actionOrTime === 'pulse') {
    body.cmd = actionOrTime;
  } else {
    const time = parseInt(actionOrTime || '1000', 10);
    body.time = Number.isFinite(time) ? time : 1000;
  }

  console.log(`\nPOST /devices/relay`, JSON.stringify(body));
  const result = await client.triggerRelay(body);
  for (const a of result.acionados || []) {
    const out = a.relay != null ? `relay:${a.relay}` : `dout:${a.dout}`;
    console.log(`  ${a.executed ? 'OK' : 'FAIL'} ${a.id} ${out} cmd=${a.cmd} time=${a.time} — ${a.msg}`);
  }
  if (result.error) console.log('  error:', result.error);
  if (result.sugestoes) console.log('  sugestoes:', result.sugestoes.join(', '));
}

main().catch((err) => {
  console.error('Erro:', err.message || err);
  process.exit(1);
});
