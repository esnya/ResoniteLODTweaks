using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Reflection;

namespace LODTweaksMod
{
    public class LODTweaks : ResoniteMod
    {
        public override string Name => "LODTweaks";
        public override string Author => "esnya";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/esnya/ResoniteLODTweaks";

        public override void OnEngineInit()
        {
            var harmony = new Harmony("com.nekometer.esnya.LODTweaks");
            harmony.PatchAll();
        }

        [HarmonyPatch]
        class LODGroupPatch
        {
            static MethodBase TargetMethod()
            {
                return typeof(LODGroup).GetMethod("OnInit", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            static void Postfix(object __instance)
            {
                if (__instance is LODGroup lodGroup)
                {
                    lodGroup.UpdateOrder = 1000;
                }
            }
        }

        [HarmonyPriority(HarmonyLib.Priority.LowerThanNormal)]
        [HarmonyPatch(typeof(WorkerInspector), nameof(WorkerInspector.BuildInspectorUI))]
        class WorkerInspectorPatch
        {
            static void Postfix(WorkerInspector __instance, Worker worker, UIBuilder ui)
            {
                if (worker is LODGroup lodGroup)
                {
                    BuildInspectorUI(lodGroup, ui);
                }
            }

            private static void BuildInspectorUI(LODGroup lodGroup, UIBuilder ui)
            {
                Button(ui, "[Mod] Add LOD Level from children", () => SetupFromChildren(lodGroup));
                Button(ui, "[Mod] Remove LODGroups from children", () => RemoveFromChildren(lodGroup));
            }

            private static void Button(UIBuilder ui, string text, System.Action onClick)
            {
                var button = ui.Button(text);
                button.IsPressed.OnValueChange += (value) =>
                {
                    if (value) onClick();
                };
            }

            private static void SetupFromChildren(LODGroup lodGroup)
            {
                lodGroup.AddLOD(0.01f, lodGroup.Slot);
            }

            private static void RemoveFromChildren(LODGroup lodGroup)
            {
                lodGroup.Slot.GetComponentsInChildren<LODGroup>(c => c != lodGroup).ForEach(c => c.Destroy());
            }
        }
    }
}
