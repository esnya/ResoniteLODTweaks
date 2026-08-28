# LODTweaks

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that gives newly initialized `LODGroup` components a late update order and adds two maintenance actions to their inspectors.

## Features

- Sets `LODGroup.UpdateOrder` to `1000` when a new group initializes.
- Adds **Add LOD Level from children** to collect child renderers into a new level.
- Adds **Remove LODGroups from children** to remove nested groups while preserving the inspected group.

`EsnyaTweaks.LODGroupTweaks` contains expanded versions of the two inspector actions, but it does not apply the automatic update-order behavior to every newly initialized `LODGroup`. The standalone mod therefore remains useful when that behavior is required.

## Installation

1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
1. Place [LODTweaks.dll](https://github.com/esnya/ResoniteLODTweaks/releases/latest/download/LODTweaks.dll) into your `rml_mods` folder. This folder is normally at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods`.
1. Start the game. If you want to verify that the mod is working you can check your Resonite logs.

## Compatibility

Release builds target .NET 10 and are compiled against the Resonite `2026.8.27.1094` public game assemblies. The modernized compatibility release is intended for `v1.0.1`.

## Development

```powershell
dotnet build .\LODTweaks.slnx -c Release -p:ResonitePath="C:\Program Files (x86)\Steam\steamapps\common\Resonite"
dotnet test .\LODTweaks.slnx -c Release -p:ResonitePath="C:\Program Files (x86)\Steam\steamapps\common\Resonite"
```

Pass `CopyToMods=true` to copy a build to `rml_mods`. Add `EnableHotReloadLibs=true` to also copy it to `rml_mods\HotReloadMods` when ResoniteHotReloadLib is installed.

Release versions come from `vX.Y.Z` tags through MinVer. A tag runs the build, tests, and GitHub Release workflow.
