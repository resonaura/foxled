<p align="left">
  <img src="logo.png" width="96" alt="FoxLED Logo" />
</p>

# FoxLED

[![Language](https://img.shields.io/badge/Language-C%23-239120.svg?logo=csharp&logoColor=white)](#architecture)
[![Framework](https://img.shields.io/badge/Framework-.NET%20Framework%204.7.2%20%7C%20WPF-512BD4.svg?logo=dotnet&logoColor=white)](#architecture)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010-0078D6.svg?logo=windows&logoColor=white)](#disclaimer--legacy-notice)
[![Audio](https://img.shields.io/badge/Audio-NAudio%20WASAPI%20Loopback-blue.svg)](#-foxled-desktop-client-features)
[![Remote](https://img.shields.io/badge/Remote-Telegram%20Bot%20API-2CA5E0.svg?logo=telegram&logoColor=white)](#-foxled-desktop-client-features)
[![Protocol](https://img.shields.io/badge/Protocol-Adalight%20Serial-orange.svg)](#architecture)
[![Status](https://img.shields.io/badge/Status-Historical%20Archive%20(2018--2019)-yellow.svg)](#disclaimer--legacy-notice)

**FoxLED** is a comprehensive ambient PC backlighting (ambilight) and audio-reactive lighting suite for Windows 10, interfacing with addressable WS2812B / FastLED strips via Arduino microcontrollers using the Adalight protocol.

---

> [!WARNING]
> ### Disclaimer & Legacy Notice
> Developed between **2018 and 2019** for Windows 10 and .NET Framework 4.7.2. Archived as a historical engineering milestone. It has not been tested on Windows 11.

---

## 💡 Origin & Recovery Story

FoxLED started in 2018 as a Python experiment (originally titled *EasyLED*), then evolved into a complete, full-featured **C# WPF desktop workstation** in 2019. While auxiliary server files were preserved in archives, the primary client application's source files were recovered directly from original production binaries and debug symbols (`FoxLED.exe` / `FoxLED.pdb`), restoring the full C# code, vector UI, XAML styles, and custom animation engine.

---

## ✨ FoxLED Desktop Client Features

The desktop client (`FoxLED/`) provides an end-to-end lighting control center:

- 🎵 **Music & Audio Reactivity (`Analyzer.cs`)**:
  - Low-latency audio stream capture via **NAudio WASAPI Loopback** (recording system audio output directly without virtual cables).
  - Real-time FFT spectrum analysis mapping sound frequencies to dynamic color pulses and sound-to-light animations (*цветомузыка*).
- 🌈 **Adaptive Ambilight & Environment Sync (`LEDAnimation.cs`)**:
  - **Screen Perimeter Capture**: Real-time desktop frame sampling syncing ambient LED backlighting with on-screen games and movies.
  - **Wallpaper Adaptive Sync**: Samples the dominant color palette of the active Windows desktop wallpaper.
  - **Windows Accent Color**: Synchronizes ambient backlighting with Windows 10 system theme accent colors.
  - **Hardware Telemetry Backlighting**: Visualizes real-time CPU load and core temperatures directly through color gradients.
  - **Animation Presets**: Rainbow cycles, custom color chases, speed and brightness sliders.
- 🖥️ **Live Desktop Preview (`PseudoLED`)**:
  - Interactive on-screen preview widget simulating the physical LED strip colors in real time.
- 📱 **Telegram Bot Remote Control (`LEDRemote.cs`)**:
  - Built-in Telegram bot integration allowing control over light modes, brightness, animation speeds, and custom color presets directly from a smartphone.
- 🔌 **High-Speed Serial Engine (`LEDConnect.cs`)**:
  - Streams RGB frame packets over Serial COM ports at 115200 baud using the **Adalight** protocol with auto-reconnection.

---

## 🛠️ Solution Architecture

The solution (`FoxLED.sln`) contains two complementary components:

1. **`FoxLED/` (Primary Desktop Client)**:
   - Complete WPF desktop application with frameless vector UI (`MainWindow.xaml`), custom control styles (`AppStyles/Main.xaml`), NAudio WASAPI FFT spectrum analyzer, ambient screen capture engine, and Telegram remote control.
2. **`FoxLED Server/` (Relay Server)**:
   - Lightweight headless/secondary server providing Fluent Acrylic design (`FluentWPF`), Adalight serial streaming, and a local TCP socket listener (`port 1337`) for distributed client commands.

---

## 🔬 Historical Prototypes & Evolution

<p align="left">
  <img src="logo-legacy.png" width="48" height="48" alt="Original EasyLED Prototype Icon" />
  <br />
  <sub><em>Original EasyLED prototype icon (48×48 px)</em></sub>
</p>

Early prototypes and production builds are preserved in [`experiments/`](experiments/):

- **[`experiments/python-prototype/`](experiments/python-prototype/)**: The initial 2018 Python 3.7 / PyQt5 prototype.
- **[`experiments/client-bin/`](experiments/client-bin/)**: Original compiled Windows binaries (`FoxLED.exe`, `NAudio.dll`, `Newtonsoft.Json.dll`, `FoxLED Server.exe`).
- **[`fox-led.svg`](fox-led.svg)**: Original vector asset of the origami geometric fox mascot.

---

## 📦 Project Structure

```
foxled/
├── FoxLED.sln                  # Visual Studio solution (Client + Server)
├── FoxLED/                     # Primary WPF Desktop Client Application
│   ├── MainWindow.xaml         # Custom-styled vector desktop UI
│   ├── MainWindow.xaml.cs      # Core UI logic, mode dispatchers & hotkeys
│   ├── Analyzer.cs             # NAudio WASAPI loopback & FFT spectrum analyzer
│   ├── LEDAnimation.cs         # Ambilight screen grab, rainbow, CPU & wallpaper sync
│   ├── LEDConnect.cs           # Serial COM port Adalight streaming engine
│   ├── LEDRemote.cs            # Telegram bot remote control client
│   ├── BrushAnimation.cs       # Smooth WPF color interpolation animations
│   ├── NativeMethods.cs        # Win32 desktop wallpaper & theme interop
│   ├── AppStyles/
│   │   └── Main.xaml           # Custom dark theme styles & button templates
│   ├── app.ico                 # FoxLED branding icon
│   └── FoxLED.csproj           # Project file (.NET Framework 4.7.2)
├── FoxLED Server/              # Standalone serial relay server (port 1337)
│   ├── MainWindow.xaml         # FluentWPF Acrylic UI
│   ├── MainWindow.xaml.cs      # TCP socket listener (port 1337)
│   ├── LEDConnect.cs           # Serial Adalight transmitter
│   └── FoxLED Server.csproj    # Server project file
├── experiments/                # Historical prototypes & binaries
│   ├── python-prototype/       # 2018 Python PyQt prototype
│   └── client-bin/             # Original compiled Windows binaries
├── logo.png                    # Primary FoxLED emblem (256×256)
├── logo-legacy.png             # Historical EasyLED prototype icon (48×48)
└── fox-led.svg                 # Vector origami fox branding
```
