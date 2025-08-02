using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(CompLaunchable), "AnyInGroupIsUnderRoof", MethodType.Getter)]
    internal static class CompLaunchable_Patch
    {
        [HarmonyPostfix]
        internal static void AnyInGroupIsUnderRoofPostfix(CompLaunchable __instance, ref bool __result)
        {
            if (!__result)
            {
                return;
            }

            var map = __instance.parent.Map;
            var transportersInGroup = __instance.parent.GetComp<CompTransporter>().TransportersInGroup(map);

            for (int i = transportersInGroup.Count - 1; i >= 0; i--)
            {
                if (!map.roofGrid.AllowFlyThrough(transportersInGroup[i].parent.Position))
                {
                    return;
                }
            }

            __result = false;
        }
    }
}
