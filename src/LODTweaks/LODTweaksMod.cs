using System.Reflection;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;
#if USE_RESONITE_HOT_RELOAD_LIB
using ResoniteHotReloadLib;
#endif

namespace LODTweaks;

/// <summary>
/// ResoniteModLoader entry point for LODGroup tweaks.
/// </summary>
public sealed class LODTweaksMod : ResoniteMod
{
    private static readonly Assembly Assembly = typeof(LODTweaksMod).Assembly;
    private static readonly string HarmonyId = $"com.nekometer.esnya.{Assembly.GetName().Name}";
    private static readonly Harmony Harmony = new(HarmonyId);

    /// <inheritdoc />
    public override string Name =>
        Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? Assembly.GetName().Name ?? string.Empty;

    /// <inheritdoc />
    public override string Author =>
        Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? string.Empty;

    /// <inheritdoc />
    public override string Version =>
        Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(static metadata => metadata.Key == "ModVersion")
            ?.Value
        ?? (Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty)
            .Split('+')[0];

    /// <inheritdoc />
    public override string Link =>
        Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(static metadata => metadata.Key == "RepositoryUrl")
            ?.Value ?? string.Empty;

    /// <inheritdoc />
    public override void OnEngineInit()
    {
        Initialize(this);
    }

#if USE_RESONITE_HOT_RELOAD_LIB
    /// <summary>
    /// Removes Harmony patches before a hot reload cycle.
    /// </summary>
    public static void BeforeHotReload()
    {
        Harmony.UnpatchAll(HarmonyId);
    }

    /// <summary>
    /// Reapplies Harmony patches after a hot reload cycle.
    /// </summary>
    /// <param name="mod">The reloaded mod instance.</param>
    public static void OnHotReload(ResoniteMod mod)
    {
        Initialize(mod);
    }
#endif

    private static void Initialize(ResoniteMod mod)
    {
        ArgumentNullException.ThrowIfNull(mod);
        Harmony.PatchAll(Assembly);

#if USE_RESONITE_HOT_RELOAD_LIB
        HotReloader.RegisterForHotReload(mod);
#endif
    }
}

[HarmonyPatch]
internal static class LODGroupOnInitPatch
{
    internal static MethodInfo TargetMethod()
    {
        return typeof(LODGroup).GetMethod("OnInit", BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new MissingMethodException(typeof(LODGroup).FullName, "OnInit");
    }

    internal static void Postfix(LODGroup __instance)
    {
        __instance.UpdateOrder = 1000;
    }
}

[HarmonyPriority(HarmonyLib.Priority.LowerThanNormal)]
[HarmonyPatch(typeof(WorkerInspector), nameof(WorkerInspector.BuildInspectorUI))]
internal static class WorkerInspectorBuildInspectorUIPatch
{
    private const string AddLabel = "[Mod] Add LOD Level from children";
    private const string RemoveLabel = "[Mod] Remove LODGroups from children";

    private static void Postfix(Worker worker, UIBuilder ui)
    {
        if (worker is not LODGroup lodGroup)
        {
            return;
        }

        AddButton(ui, AddLabel, () => lodGroup.AddLOD(0.01f, lodGroup.Slot));
        AddButton(ui, RemoveLabel, () => RemoveFromChildren(lodGroup));
    }

    private static void AddButton(UIBuilder ui, string label, Action action)
    {
        Button button = ui.Button(label);
        button.LocalPressed += (_, _) => action();
    }

    private static void RemoveFromChildren(LODGroup lodGroup)
    {
        foreach (LODGroup childGroup in lodGroup.Slot.GetComponentsInChildren<LODGroup>(group => group != lodGroup))
        {
            childGroup.Destroy();
        }
    }
}
