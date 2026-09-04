#!/usr/bin/env node
/**
 * Webhook receiver on 0.0.0.0 (all IPv4 interfaces).
 * The controller must POST to this PC's LAN IP — not localhost.
 *
 *   node examples/webhook_server.js [port]
 *
 * Default port: 9099 (8080 is often taken by filesync-win64 on MCU Windows PCs).
 * Save on the controller: http://<THIS_PC_LAN_IP>:<port>/webhook
 * Enable registered + unregistered. Allow Windows Firewall inbound TCP for that port.
 */
'use strict';

const http = require('http');
const os = require('os');

const port = parseInt(process.argv[2] || '9099', 10);

function lanIpv4() {
  const out = [];
  for (const list of Object.values(os.networkInterfaces())) {
    for (const a of list || []) {
      if (a.family === 'IPv4' && !a.internal && !a.address.startsWith('169.254.'))
        out.push(a.address);
    }
  }
  return out;
}

function pretty(text) {
  try {
    return JSON.stringify(JSON.parse(text), null, 2);
  } catch {
    return text;
  }
}

const server = http.createServer((req, res) => {
  const chunks = [];
  req.on('data', (c) => chunks.push(c));
  req.on('end', () => {
    const body = Buffer.concat(chunks).toString('utf8');
    const from = req.socket.remoteAddress || '?';
    console.log(`[${new Date().toISOString()}] ${req.method} ${req.url} from ${from}`);
    if (body) console.log(pretty(body));
    console.log('');
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ status: 'ok', received: true }));
  });
});

server.listen(port, '0.0.0.0', () => {
  console.log(`Listening on 0.0.0.0:${port} (all interfaces)`);
  const ips = lanIpv4();
  if (ips.length === 0) console.log('No LAN IPv4 found.');
  for (const ip of ips) console.log(`Controller URL: http://${ip}:${port}/webhook`);
  console.log('Do not use localhost — the controller is another device on the network.');
  console.log('Windows: allow inbound TCP in the firewall if the controller times out.\n');
});
