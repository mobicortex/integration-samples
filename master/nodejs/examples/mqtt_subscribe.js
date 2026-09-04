#!/usr/bin/env node
/**
 * MQTT export subscriber — TCP 1884, topic mbcortex/export/event.
 * Zero extra packages (MQTT 3.1.1 over net.Socket).
 *
 *   node examples/mqtt_subscribe.js [host] [port] [user] [pass] [topic]
 *
 * Default: 192.168.0.180 1884 mqttuser mqttpass mbcortex/export/event
 *
 * Port 1883 on the controller is loopback IPC only. There is no WebSocket MQTT.
 */
'use strict';

const net = require('net');

const host = process.argv[2] || '192.168.0.180';
const port = parseInt(process.argv[3] || '1884', 10);
const user = process.argv[4] || 'mqttuser';
const pass = process.argv[5] || 'mqttpass';
const topic = process.argv[6] || 'mbcortex/export/event';

function mqttString(s) {
  const buf = Buffer.from(s, 'utf8');
  const out = Buffer.alloc(2 + buf.length);
  out.writeUInt16BE(buf.length, 0);
  buf.copy(out, 2);
  return out;
}

function remainingLength(n) {
  const bytes = [];
  do {
    let enc = n % 128;
    n = Math.floor(n / 128);
    if (n > 0) enc |= 0x80;
    bytes.push(enc);
  } while (n > 0);
  return Buffer.from(bytes);
}

function packet(typeFlags, payload) {
  return Buffer.concat([Buffer.from([typeFlags]), remainingLength(payload.length), payload]);
}

function connectPacket(clientId, username, password) {
  const proto = Buffer.concat([mqttString('MQTT'), Buffer.from([4, 0xc2, 0x00, 0x3c])]);
  const payload = Buffer.concat([mqttString(clientId), mqttString(username), mqttString(password)]);
  return packet(0x10, Buffer.concat([proto, payload]));
}

function subscribePacket(id, topicName) {
  return packet(0x82, Buffer.concat([Buffer.from([id >> 8, id & 0xff]), mqttString(topicName), Buffer.from([0])]));
}

function pingreq() {
  return Buffer.from([0xc0, 0x00]);
}

function pretty(text) {
  try {
    return JSON.stringify(JSON.parse(text), null, 2);
  } catch {
    return text;
  }
}

let buf = Buffer.alloc(0);

function readRemaining(buffer, start) {
  let multiplier = 1;
  let value = 0;
  let i = start;
  for (;;) {
    if (i >= buffer.length) return null;
    const b = buffer[i++];
    value += (b & 127) * multiplier;
    if ((b & 128) === 0) return { value, headerLen: i - start + 1 };
    multiplier *= 128;
    if (multiplier > 128 * 128 * 128) return null;
  }
}

function onData(chunk) {
  buf = Buffer.concat([buf, chunk]);
  while (buf.length >= 2) {
    const rl = readRemaining(buf, 1);
    if (!rl) return;
    const total = rl.headerLen + rl.value;
    if (buf.length < total) return;
    const pkt = buf.subarray(0, total);
    buf = buf.subarray(total);
    handlePacket(pkt, rl.headerLen);
  }
}

function handlePacket(pkt, headerLen) {
  const type = pkt[0] >> 4;
  const body = pkt.subarray(headerLen);
  if (type === 2) {
    const code = body.length > 1 ? body[1] : 255;
    if (code !== 0) {
      console.error('CONNACK refused:', code, '(1=proto 2=id 3=server 4=user 5=auth)');
      process.exit(1);
    }
    console.log('Connected. Subscribing to', topic);
    socket.write(subscribePacket(1, topic));
    return;
  }
  if (type === 9) {
    console.log('SUBACK ok. Waiting for events...\n');
    return;
  }
  if (type === 3) {
    const qos = (pkt[0] >> 1) & 0x03;
    const tlen = body.readUInt16BE(0);
    let off = 2;
    const t = body.subarray(off, off + tlen).toString('utf8');
    off += tlen;
    if (qos > 0) off += 2;
    const payload = body.subarray(off).toString('utf8');
    const ts = new Date().toISOString();
    console.log(`[${ts}] ${t}`);
    console.log(pretty(payload));
    console.log('');
  }
}

const clientId = 'smartsdk-node-' + process.pid;
const socket = net.connect({ host, port }, () => {
  console.log(`mqtt://${host}:${port} as ${user}`);
  socket.write(connectPacket(clientId, user, pass));
});
socket.on('data', onData);
socket.on('error', (err) => {
  console.error('TCP error:', err.message);
  process.exit(1);
});
socket.on('close', () => {
  console.log('Disconnected');
  process.exit(0);
});
setInterval(() => {
  if (socket.writable) socket.write(pingreq());
}, 30000);
