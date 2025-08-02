using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(InfestationCellFinder), "GetScoreAt")]
    internal static class InfestationCellFinder_Patch
    {
        [HarmonyPrefix]
        internal static bool GetScoreAtPrefix(IntVec3 cell, Map map, ref float __result)
        {
            if (SMR.Settings.PreventInfestation && map.roofGrid.RoofAt(cell).IsCustomRoof())
            {
                __result = 0f;
                return false;
            }

            return true;
        }
    }
}
