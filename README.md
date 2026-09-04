<img src="logo.png" width="80" alt="FoxLED Logo" />

# FoxLED

[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Arduino%20%7C%20Serial-0078D6.svg?logo=windows&logoColor=white)](#overview)
[![Python](https://img.shields.io/badge/Language-Python%203.7%20%7C%20C%23-3776AB.svg?logo=python&logoColor=white)](core)
[![Status](https://img.shields.io/badge/Status-Historical%20Archive%20(2018--2019)-orange.svg)](#disclaimer--legacy-notice)

Real-time PC ambient backlighting (ambilight), reactive sound visualizer, system telemetry monitor, and multi-zone LED strip controller built with Python, PyQT, and serial microcontrollers.

---

> [!WARNING]
> ### Disclaimer & Legacy Notice
> Developed between **2018 and 2019** for Windows 10 and Arduino/ESP microcontrollers. Preserved as an open-source engineering milestone. Modern Windows environments and updated serial USB driver stacks may require adjustments.

---

## ✨ Features & Architecture

- 🌈 **Screen-Adaptive Ambilight**: High-framerate desktop screen capture analyzing screen perimeter colors and syncing ambient LED strips in real time with zero visible lag.
- 🎵 **Music & Beat Visualization**: Audio FFT spectrum analyzer sampling active Windows audio endpoints to generate reactive sound-to-light animations.
- 🌡️ **Hardware Telemetry**: Visualizes live CPU load, GPU metrics, and core temperatures directly through animated color gradients.
- 🎛️ **Modular Controller (`core/`)**:
  - `easyled.py`: High-speed binary protocol transmitting RGB frames across serial COM ports.
  - `easyaudio.py`: Low-latency audio stream processing and frequency filtering.
  - `gui.pyw`: Interactive desktop control panel with custom animation presets (`anim.py`).

---

## 🔬 Experiments & Server Suite

Archived auxiliary tools live in [`experiments/`](experiments/):
- **`experiments/foxled-server`**: C# / WPF Fluent network relay distributing lighting states to distributed ESP32/ESP8266 nodes.
- **`experiments/foxled-wpf-client`**: Standalone Windows WPF visualizer prototype.
