using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(Skyfaller), "HitRoof")]
    internal static class Skyfaller_Patch
    {
        [HarmonyPrefix]
        internal static bool HitRoofPrefix(Skyfaller __instance)
        {
            if (!__instance.def.skyfaller.hitRoof)
            {
                return false;
            }

            var map = __instance.Map;
            var occupiedRect = __instance.OccupiedRect();

            foreach (var cell in occupiedRect.Cells)
            {
                if (cell.InBounds(map) && map.roofGrid.IsLighttight(cell))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
