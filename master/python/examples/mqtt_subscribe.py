#!/usr/bin/env python3
"""MQTT export subscriber — TCP 1884, topic mbcortex/export/event.

Zero extra packages (MQTT 3.1.1 over sockets).

    python examples/mqtt_subscribe.py [host] [port] [user] [pass] [topic]

Default: 192.168.0.180 1884 mqttuser mqttpass mbcortex/export/event

Port 1883 on the controller is loopback IPC only. There is no WebSocket MQTT.
"""
from __future__ import annotations

import json
import socket
import struct
import sys
import time

host = sys.argv[1] if len(sys.argv) > 1 else "192.168.0.180"
port = int(sys.argv[2]) if len(sys.argv) > 2 else 1884
user = sys.argv[3] if len(sys.argv) > 3 else "mqttuser"
password = sys.argv[4] if len(sys.argv) > 4 else "mqttpass"
topic = sys.argv[5] if len(sys.argv) > 5 else "mbcortex/export/event"


def mqtt_str(s: str) -> bytes:
    b = s.encode("utf-8")
    return struct.pack("!H", len(b)) + b


def remaining_length(n: int) -> bytes:
    out = bytearray()
    while True:
        enc = n % 128
        n //= 128
        if n > 0:
            enc |= 0x80
        out.append(enc)
        if n == 0:
            break
    return bytes(out)


def packet(type_flags: int, payload: bytes) -> bytes:
    return bytes([type_flags]) + remaining_length(len(payload)) + payload


def connect_packet(client_id: str, username: str, pwd: str) -> bytes:
    proto = mqtt_str("MQTT") + bytes([4, 0xC2, 0x00, 0x3C])
    payload = mqtt_str(client_id) + mqtt_str(username) + mqtt_str(pwd)
    return packet(0x10, proto + payload)


def subscribe_packet(pkt_id: int, topic_name: str) -> bytes:
    return packet(0x82, struct.pack("!H", pkt_id) + mqtt_str(topic_name) + bytes([0]))


def pretty(text: str) -> str:
    try:
        return json.dumps(json.loads(text), indent=2, ensure_ascii=False)
    except Exception:
        return text


def recv_exact(sock: socket.socket, n: int) -> bytes:
    buf = bytearray()
    while len(buf) < n:
        chunk = sock.recv(n - len(buf))
        if not chunk:
            raise ConnectionError("closed")
        buf.extend(chunk)
    return bytes(buf)


def read_packet(sock: socket.socket) -> tuple[int, bytes]:
    first = recv_exact(sock, 1)[0]
    multiplier = 1
    length = 0
    while True:
        b = recv_exact(sock, 1)[0]
        length += (b & 127) * multiplier
        if (b & 128) == 0:
            break
        multiplier *= 128
    body = recv_exact(sock, length) if length else b""
    return first >> 4, body


def main() -> None:
    client_id = f"smartsdk-py-{int(time.time())}"
    print(f"mqtt://{host}:{port} as {user}")
    sock = socket.create_connection((host, port), timeout=15)
    sock.settimeout(60)
    sock.sendall(connect_packet(client_id, user, password))
    ptype, body = read_packet(sock)
    if ptype != 2 or (len(body) > 1 and body[1] != 0):
        code = body[1] if len(body) > 1 else -1
        raise SystemExit(f"CONNACK refused: {code} (4=bad user 5=bad pass)")
    print("Connected. Subscribing to", topic)
    sock.sendall(subscribe_packet(1, topic))
    ptype, _body = read_packet(sock)
    if ptype != 9:
        raise SystemExit(f"expected SUBACK, got type {ptype}")
    print("SUBACK ok. Waiting for events...\n")
    sock.settimeout(None)
    while True:
        ptype, body = read_packet(sock)
        if ptype != 3:
            continue
        tlen = struct.unpack("!H", body[:2])[0]
        t = body[2:2 + tlen].decode("utf-8", "replace")
        payload = body[2 + tlen:].decode("utf-8", "replace")
        print(f"[{time.strftime('%H:%M:%S')}] {t}")
        print(pretty(payload))
        print()


if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print("\nDisconnected")
