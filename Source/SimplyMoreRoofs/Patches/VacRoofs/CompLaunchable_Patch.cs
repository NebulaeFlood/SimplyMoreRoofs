using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(CompLaunchable), "AnyInGroupIsUnderRoof", MethodType.Getter)]
    public static class CompLaunchable_Patch
    {
        [HarmonyPostfix]
        public static void AnyInGroupIsUnderRoofPostfix(CompLaunchable __instance, ref bool __result)
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
