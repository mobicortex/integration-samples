# MobiCortex Integration Samples

This repository provides official code samples and technical resources to help developers and OEM partners integrate **MobiCortex** technology into their own ecosystems.

## 🚀 Overview
MobiCortex is a high-performance technology platform for intelligent controllers. This repository is designed to be a "plug-and-play" resource, offering clear examples of how to interact with our hardware via REST APIs, MQTT, and other industry-standard protocols.

## 📂 Repository Structure
The organization reflects the system hierarchy, separating central intelligence from execution devices:

### 🧠 `/master`
Resources for the **Master Series** — the "brain" of the system, responsible for high-complexity processing and logic management:
* **LPR (License Plate Recognition):** Handling automated vehicle recognition events (e.g., Master LPR).
* **Biometrics & RFID:** Management of facial recognition, biometric data, and high-level TAG processing.
* **Access Interfaces:** Communication with touchscreen or standard controllers for centralized automation.
* **`/docs`**: Software integration manuals, API references, and logical configuration guides.

### 🔌 `/devices`
Resources for **Edge Devices** — actuators and sensors for command execution or basic data collection:
* **Actuators:** Ethernet, Wi-Fi, and Wireless relays.
* **Interfaces:** Push-button controllers and simple automation interfaces.
* **Sensors:** Monitoring tools, such as water pump sensors and state detectors.
* **Passive Readers:** RFID readers that collect and transmit raw data to a Master unit.
* **`/docs`**: Pinout diagrams, electrical wiring schemes, and physical installation guides.

### 💻 Integration by Language
Within each category, samples are available in:
* **/csharp**: .NET environments and Windows-based systems.
* **/nodejs**: JavaScript and TypeScript server-side applications (fetch nativo).
* **/python**: Clean scripts using only the Python standard library.
* **/golang**: High-performance and scalable backends.
* **/postman**: Ready-to-use collections for immediate API testing.

## 🛠️ Getting Started
1. Ensure your device is reachable within your network.
2. Navigate to the folder corresponding to your hardware (`/master` or `/devices`).
3. Check the `/docs` subfolder to understand the technical requirements before running the sample codes.

## 📖 Full Documentation
For detailed technical specifications, firmware updates, and AI-assisted support, please visit our official portal:
👉 **[mobicortex.com](https://mobicortex.com)**

---
**Note:** MobiCortex is a technology platform. For commercial inquiries or to purchase hardware, please contact your authorized distributor.

**License:** This project is licensed under the MIT License.