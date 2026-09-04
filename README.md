<img src="logo.png" width="80" alt="FoxLED Logo" />

# FoxLED

[![Language](https://img.shields.io/badge/Language-C%23-239120.svg?logo=csharp&logoColor=white)](#architecture)
[![Framework](https://img.shields.io/badge/Framework-.NET%20Framework%204.7.2-512BD4.svg?logo=dotnet&logoColor=white)](#architecture)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010-0078D6.svg?logo=windows&logoColor=white)](#disclaimer--legacy-notice)
[![UI](https://img.shields.io/badge/UI-WPF%20%7C%20FluentWPF-68217A.svg)](#architecture)
[![Protocol](https://img.shields.io/badge/Protocol-Adalight%20Serial-orange.svg)](#architecture)
[![Status](https://img.shields.io/badge/Status-Historical%20Archive%20(2018--2019)-yellow.svg)](#disclaimer--legacy-notice)

**FoxLED** is a real-time PC ambient backlighting (ambilight) and audio-reactive lighting controller built in C# with WPF. It interfaces with addressable LED strips (WS2812B / Neopixel) connected via Arduino microcontrollers using the Adalight serial streaming protocol.

---

> [!WARNING]
> ### Disclaimer & Legacy Notice
> FoxLED was originally developed between **2018 and 2019** for Windows 10 and .NET Framework 4.7.2. It is archived here as a historical engineering milestone and has not been tested or updated for Windows 11.

---

## 💡 Overview & Origin

FoxLED originated in late 2018 as a personal project to create low-latency ambient screen lighting and music visualization for desktop setups. While initial experiments were conducted in Python, the project was completely rewritten in **C# with WPF** to achieve:

- High-throughput serial transfer with minimal CPU overhead.
- Native Windows 10 Fluent Design aesthetics featuring Acrylic blur effects (`FluentWPF`).
- A modular client-server architecture decoupling the serial hardware controller from lighting source clients.

---

## 🛠️ Architecture

The solution (`FoxLED Server.sln`) provides the core server and hardware driver:

- **Adalight Protocol Driver (`LEDConnect.cs`)**:
  - Automatically constructs Adalight frame packets (`['A', 'd', 'a', high_byte, low_byte, checksum]`) over a high-speed COM port (115200 baud).
  - Handles brightness scaling and RGB byte mapping for WS2812B / FastLED strips.
- **Local Socket Server (`MainWindow.xaml.cs`)**:
  - Listens on TCP port `1337` (`SocketListener`) for incoming lighting updates.
  - Parses real-time frame buffers transmitted by companion visualizers and ambilight clients.
- **Fluent Acrylic Interface**:
  - Uses `FluentWPF` (`AcrylicWindow`) to match Windows 10 dark-mode translucent titlebars and reveal-highlight buttons.

---

## 🔬 Historical Prototypes & Experiments

Early prototypes and auxiliary tools are preserved in [`experiments/`](experiments/):

- **[`experiments/python-prototype/`](experiments/python-prototype/)**: The initial 2018 Python 3.7 / PyQt5 prototype containing early screen-capture ambilight scripts and audio FFT visualizers.
- **[`experiments/client-bin/`](experiments/client-bin/)**: Compiled standalone FoxLED Windows client with NAudio integration for audio spectrum analysis.

---

## 📦 Project Structure

```
foxled/
├── FoxLED Server.sln           # Visual Studio solution file
├── FoxLED Server/               # C# WPF server project
│   ├── MainWindow.xaml         # FluentWPF Acrylic UI layout
│   ├── MainWindow.xaml.cs      # TCP socket server (port 1337) & UI logic
│   ├── LEDConnect.cs           # Serial COM port Adalight streaming engine
│   ├── App.xaml / App.xaml.cs  # Application entry point
│   └── FoxLED Server.csproj    # Project definition (.NET Framework 4.7.2)
├── experiments/                # Historical prototypes & binaries
│   ├── python-prototype/       # 2018 Python PyQt prototype
│   └── client-bin/             # NAudio-based reactive client binaries
└── logo.png                    # Project branding
```
