# Changelog (pt-BR)

Todas as mudanças relevantes neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
e este projeto segue [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Não Lançado]

### Alterado
- Cliente MQTT e monitoramento passam a usar TCP 1884 e o tópico `mbcortex/export/event` (sem MQTT via WebSocket).
- SDK com REST `/mqtt-export` e botão para criar usuário de teste no formulário MQTT.
- Logs de monitoramento MQTT e do cliente MQTT quebram linhas longas e formatam o JSON do payload.

### Corrigido
- Servidor HTTP de webhook do exemplo não exige mais Administrador nem `netsh http add urlacl` (usa TcpListener em vez de HttpListener/HTTP.sys).

## [2026-03-09]

### Adicionado
- Adicionado o campo `enable` no Cadastro Central, Pessoas, Veículos e Mídias.
- Adicionada uma lista padrão de cores (busca via controladora).
- Adicionada uma lista de marcas de veículos comuns no Brasil.

### Alterado
- Exibição das datas de cadastro e atualização (`created` e `updated`).

### Corrigido
- Correção de bugs e pequenos ajustes nos formulários para permitir edição no Visual Studio.
