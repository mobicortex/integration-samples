# Changelog (pt-BR)

Todas as mudanças relevantes neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
e este projeto segue [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Não Lançado]

### Alterado
- Cliente MQTT e monitoramento passam a usar TCP 1884 e o tópico `mbcortex/export/event` (sem MQTT via WebSocket).
- SDK com REST `/mqtt-export` e botão para criar usuário de teste no formulário MQTT.
- Logs de monitoramento MQTT e do cliente MQTT quebram linhas longas e formatam o JSON (`&` permanece `&`, não `\u0026`).
- No monitoramento, **Subscribe** conecta e assina o tópico se ainda não estiver conectado.
- Credenciais MQTT de demo: `mqttuser` / `mqttpass`.
- Webhook mostra URL da LAN, escuta em `0.0.0.0`, botão **Allow Windows Firewall**, duplo clique na grade abre o evento.

### Corrigido
- Servidor HTTP de webhook do exemplo não exige mais Administrador nem `netsh http add urlacl` (usa TcpListener em vez de HttpListener/HTTP.sys).
- Aviso quando a porta 8080 já está em uso (no Windows MCU costuma ser o `filesync-win64.exe`).

## [2026-03-09]

### Adicionado
- Adicionado o campo `enable` no Cadastro Central, Pessoas, Veículos e Mídias.
- Adicionada uma lista padrão de cores (busca via controladora).
- Adicionada uma lista de marcas de veículos comuns no Brasil.

### Alterado
- Exibição das datas de cadastro e atualização (`created` e `updated`).

### Corrigido
- Correção de bugs e pequenos ajustes nos formulários para permitir edição no Visual Studio.
