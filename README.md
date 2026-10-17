# Motion Anchor

![Motion Anchor app icon](Assets/MotionAnchor.svg)

Motion Anchor is a lightweight, click-through Windows overlay that draws a fixed
matrix of subtle white dots across the primary display. It stays above other
windows, including games running in borderless windowed mode, and does not
intercept mouse input.

## Run

Install the .NET 8 SDK, then run:

```powershell
dotnet run --project .\MotionAnchor.csproj
```

To publish a standalone Windows executable:

```powershell
dotnet publish .\MotionAnchor.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

The executable is written to
`bin\Release\net8.0-windows\win-x64\publish\MotionAnchor.exe`.

The application and tray icon use the custom gray anchor and green waves artwork
in `Assets\MotionAnchor.svg`.

The overlay starts immediately on the primary display. Use **Ctrl+Alt+O** or the
tray icon's **Show/Hide overlay** action to toggle it. Double-click the tray icon
to toggle the overlay, or choose **Exit Motion Anchor** from its menu to quit.
**Ctrl+Alt+Q** also exits. The dot spacing, size, and overall opacity are set
near the top of `OverlayForm.cs`.
