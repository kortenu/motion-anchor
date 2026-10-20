# Motion Anchor

![Motion Anchor icon](Assets/MotionAnchor.svg)

Motion Anchor is a client-side Fabric mod for Minecraft 26.3. It draws 16
white dots in a fixed, reference-matched layout directly in the game's HUD.
Their positions scale with Minecraft's GUI resolution and do not intercept
input. Press **O** (configurable in Minecraft's Controls settings) to toggle
them.

## Build and install

1. Build the mod from source with Java 25 by running `.\gradlew.bat build`.
2. Install Fabric Loader for Minecraft 26.3 and Fabric API in the same profile.
3. Copy `build\libs\motion-anchor-1.0.0.jar` into that profile's `mods` folder.
4. Launch the Fabric profile.

The mod is client-only and does not need to be installed on a server. It is
Modrinth-compatible, but has not yet been published as a Modrinth project.
The built JAR is written to `build\libs\motion-anchor-1.0.0.jar`.

## Riot Vanguard

Unlike the legacy Windows overlay, this mod draws only inside Minecraft and
does not create a separate always-on-top desktop window. It cannot guarantee
Riot Vanguard compatibility or prevent Vanguard from flagging other software.
Close Minecraft before launching League of Legends if Vanguard has raised a
warning, and contact Riot Support about Vanguard alerts.

The previous Windows overlay source is retained in `MotionAnchor.csproj`,
`OverlayForm.cs`, and related files; do not run that overlay alongside Riot
games.
