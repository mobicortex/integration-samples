#!/usr/bin/env python3
"""
Outputs (DOUT / Relays) — GET /devices + POST /devices/relay

USO:
  python examples/devices_outputs.py
  python examples/devices_outputs.py 42 relay 1 1000
  python examples/devices_outputs.py 42 dout 3 500
  python examples/devices_outputs.py 42 relay 1 on

Env:
  MOBICORTEX_URL=https://192.168.120.45:4449
  MOBICORTEX_PASS=admin
"""

from __future__ import annotations

import json
import os
import sys

# Allow importing mbcortex_demo client from parent folder
sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from mbcortex_demo import MbcortexClient, MbcortexError  # noqa: E402


def format_output(o: dict) -> str:
    if o.get("relay") is not None:
        return f"relay:{o['relay']}"
    if o.get("dout") is not None:
        nome = o.get("nome")
        return f"dout:{o['dout']}" + (f" ({nome})" if nome else "")
    return json.dumps(o, ensure_ascii=False)


def main() -> int:
    base_url = os.environ.get("MOBICORTEX_URL", "https://192.168.120.45:4449")
    password = os.environ.get("MOBICORTEX_PASS", "admin")
    args = sys.argv[1:]

    client = MbcortexClient(base_url, password=password)
    print(f"Login @ {base_url}")
    client.login()

    data = client.list_devices()
    items = data.get("items") or []
    print(f"\nGET /devices — {len(items)} device(s)\n")

    for d in items:
        online = d.get("online")
        online_s = "—" if online is None else ("online" if online else "offline")
        print(f"  [{d.get('id')}] {d.get('nome')}  tipo={d.get('tipo')}  modelo={d.get('modelo') or ''}  {online_s}")
        for o in d.get("saidas") or []:
            cmds = ",".join(o.get("cmd") or [])
            print(f"      {format_output(o)}  cmd=[{cmds}]  estado={o.get('estado') or '—'}")

    if not args:
        print("\nSem comando. Passe args para acionar, ex.:")
        print("  python examples/devices_outputs.py 42 relay 1 1000")
        print("  python examples/devices_outputs.py 42 dout 3 500")
        print("  python examples/devices_outputs.py 42 relay 1 on")
        return 0

    device_id, kind, num_s, *rest = args + ["", "", ""]
    num = int(num_s)
    action = rest[0] if rest else "1000"

    body: dict = {"id": device_id}
    if kind == "dout":
        body["dout"] = num
    else:
        body["relay"] = num

    if action in ("on", "off", "toggle", "pulse"):
        body["cmd"] = action
    else:
        try:
            body["time"] = int(action)
        except ValueError:
            body["time"] = 1000

    print(f"\nPOST /devices/relay {json.dumps(body)}")
    result = client.trigger_relay(body)
    for a in result.get("acionados") or []:
        out = f"relay:{a['relay']}" if a.get("relay") is not None else f"dout:{a.get('dout')}"
        ok = "OK" if a.get("executed") else "FAIL"
        print(f"  {ok} {a.get('id')} {out} cmd={a.get('cmd')} time={a.get('time')} — {a.get('msg')}")
    if result.get("error"):
        print("  error:", result["error"])
    if result.get("sugestoes"):
        print("  sugestoes:", ", ".join(result["sugestoes"]))
    return 0


if __name__ == "__main__":
    try:
        raise SystemExit(main())
    except MbcortexError as e:
        print(f"Erro API: {e}", file=sys.stderr)
        raise SystemExit(1)
    except Exception as e:
        print(f"Erro: {e}", file=sys.stderr)
        raise SystemExit(1)
