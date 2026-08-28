# LODTweaks (Archived)

> [!IMPORTANT]
> This standalone mod is archived and is no longer needed on current Resonite versions. Remove `LODTweaks.dll` from every `rml_mods` and `rml_mods/HotReloadMods` directory.

## Migration

The maintained inspector actions have moved to [`EsnyaTweaks.LODGroupTweaks`](https://github.com/esnya/ResoniteEsnyaTweaks):

1. Delete every installed copy of `LODTweaks.dll`.
2. Download [`EsnyaTweaks.LODGroupTweaks.dll`](https://github.com/esnya/ResoniteEsnyaTweaks/releases/latest/download/EsnyaTweaks.LODGroupTweaks.dll).
3. Place the replacement DLL in the same `rml_mods` or `rml_mods/HotReloadMods` locations where you want it loaded.

Do not load the standalone and replacement DLLs together.

## Why this repository was archived

- The automatic `LODGroup.UpdateOrder = 1000` initialization patch is obsolete on current Resonite versions.
- **Add LOD Level from children** and **Remove LODGroups from children** are maintained as expanded inspector actions in `EsnyaTweaks.LODGroupTweaks`.
- There is no remaining functionality that needs a separate `LODTweaks.dll`.

## Historical release

[`v1.0.1`](https://github.com/esnya/ResoniteLODTweaks/releases/tag/v1.0.1) is retained unchanged for historical reference. It is superseded, unsupported, and should not be installed on current Resonite versions.
