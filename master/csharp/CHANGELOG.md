# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- MQTT client and monitoring now use TCP port 1884 and topic `mbcortex/export/event` (no WebSocket MQTT).
- Added REST `/mqtt-export` to the SDK and a "Create test user" action on the MQTT client form.
- MQTT monitoring and MQTT client logs wrap long event lines and pretty-print JSON payloads (`&` stays as `&`, not `\u0026`).
- Monitoring **Subscribe** connects and subscribes in one step if not already connected.
- MQTT monitoring lists events in a grid (like the webhook form). Double-click a row to open the payload.
- Default MQTT demo credentials: `mqttuser` / `mqttpass`.
- Webhook form shows LAN URLs, exclusive bind on `0.0.0.0`, **Allow Windows Firewall** button, double-click a grid row to open the event.

### Fixed
- Webhook sample HTTP server no longer requires Administrator or `netsh http add urlacl` (listens with TcpListener instead of HttpListener/HTTP.sys).
- Warn when port 8080 is already used (often `filesync-win64.exe` on MCU Windows PCs).

## [2026-03-09]

### Added
- Added the `enable` field to the Central Registry, People, Vehicles, and Media registrations.
- Added a default color list, fetched via the controller.
- Added a list of common vehicle brands in Brazil.

### Changed
- Display registration `created` and `updated` dates.

### Fixed
- Bug fixes and small form adjustments to allow editing in Visual Studio.

