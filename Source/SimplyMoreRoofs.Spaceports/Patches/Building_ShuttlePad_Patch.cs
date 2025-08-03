using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using Spaceports.Buildings;
using Verse;

namespace SimplyMoreRoofs.Spaceports.Patches
{
    [StaticConstructorOnStartup]
    public static class Building_ShuttlePad_Patch
    {
        static Building_ShuttlePad_Patch()
        {
            new Harmony("Nebulae.SimplyMoreRoofs.Spaceports").Patch(AccessTools.Method(typeof(Building_ShuttlePad), nameof(Building_ShuttlePad.IsUnroofed)),
                postfix: new HarmonyMethod(typeof(Building_ShuttlePad_Patch), nameof(IsUnroofedPostfix)));
        }


        public static void IsUnroofedPostfix(Building_ShuttlePad __instance, ref bool __result)
        {
            if (__result)
            {
                return;
            }

            var map = __instance.Map;

            foreach (var cell in __instance.OccupiedRect().Cells)
            {
                if (!map.roofGrid.AllowFlyThrough(cell))
                {
                    return;
                }
            }

            __result = true;
        }
    }
}
