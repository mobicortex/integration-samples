#!/usr/bin/env python3
"""Webhook receiver on 0.0.0.0 (all IPv4 interfaces).

The controller must POST to this PC's LAN IP — not localhost.

    python examples/webhook_server.py [port]

Default port: 9099 (8080 is often taken by filesync-win64 on MCU Windows PCs).
Save on the controller: http://<THIS_PC_LAN_IP>:<port>/webhook
Enable registered + unregistered. Allow Windows Firewall inbound TCP for that port.
"""
from __future__ import annotations

import json
import socket
import sys
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer


PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 9099


def lan_ipv4() -> list[str]:
    ips: list[str] = []
    try:
        hostname = socket.gethostname()
        for info in socket.getaddrinfo(hostname, None, socket.AF_INET):
            ip = info[4][0]
            if ip.startswith("127.") or ip.startswith("169.254."):
                continue
            if ip not in ips:
                ips.append(ip)
    except Exception:
        pass
    return ips


class Handler(BaseHTTPRequestHandler):
    def log_message(self, fmt: str, *args) -> None:
        sys.stderr.write("[%s] %s\n" % (self.log_date_time_string(), fmt % args))

    def do_POST(self) -> None:
        length = int(self.headers.get("Content-Length") or 0)
        raw = self.rfile.read(length) if length else b""
        text = raw.decode("utf-8", "replace")
        print(f"{self.log_date_time_string()} {self.command} {self.path} from {self.client_address[0]}")
        try:
            print(json.dumps(json.loads(text), indent=2, ensure_ascii=False))
        except Exception:
            print(text)
        print()
        body = b'{"status":"ok","received":true}'
        self.send_response(200)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def do_GET(self) -> None:
        self.do_POST()


def main() -> None:
    httpd = ThreadingHTTPServer(("0.0.0.0", PORT), Handler)
    print(f"Listening on 0.0.0.0:{PORT} (all interfaces)")
    ips = lan_ipv4()
    if not ips:
        print("No LAN IPv4 found.")
    for ip in ips:
        print(f"Controller URL: http://{ip}:{PORT}/webhook")
    print("Do not use localhost — the controller is another device on the network.")
    print("Windows: allow inbound TCP in the firewall if the controller times out.\n")
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print("\nStopped")


if __name__ == "__main__":
    main()
