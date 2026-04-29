# Stroke.TrayIcon

A system tray plugin for [Stroke](https://github.com/poerin/Stroke). It provides a notification area icon with pause / resume and exit controls.

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Usage](#usage)
    - [Gesture Scripts](#gesture-scripts)
- [License](#license)

## Features

- **Tray Icon**: Displays the Stroke icon in the system notification area.
- **Pause / Resume**: Left‑click the icon or use the context menu to suspend gesture detection, and repeat to resume.
- **Exit Stroke**: Right‑click the icon and choose “Exit” to shut down the engine.

## Requirements

- Stroke engine.
- .NET Framework 4.8 runtime.

## Usage

### Gesture Scripts

The plugin exposes a single method `TrayIcon.SetVisibility(bool visible)` to show or hide the tray icon.

The following toggle script uses `Base.Data` to persist the visibility state. Executing the associated gesture switches the icon between shown and hidden.

```csharp
if (!Base.Data.ContainsKey("tray"))
{
    Base.Data["tray"] = true;
    TrayIcon.SetVisibility(true);
}
else
{
    bool current = (bool)Base.Data["tray"];
    TrayIcon.SetVisibility(!current);
    Base.Data["tray"] = !current;
}
```

## License

Distributed under the MIT License.