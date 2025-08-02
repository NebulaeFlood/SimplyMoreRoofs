using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(Designator_AreaNoRoof))]
    internal static class Designator_AreaNoRoof_Patch
    {
        [HarmonyPatch(nameof(Designator_AreaNoRoof.CanDesignateCell))]
        [HarmonyPostfix]
        internal static void CanDesignateCellPostfix(Designator_AreaNoRoof __instance, IntVec3 c, ref AcceptanceReport __result)
        {
            if (!__result.Accepted && __instance.Map.roofGrid.RoofAt(c).IsCustomRoof())
            {
                __result = AcceptanceReport.WasAccepted;
            }
        }


        [HarmonyPatch(nameof(Designator_AreaNoRoof.SelectedUpdate))]
        [HarmonyPostfix]
        internal static void SelectedUpdatePostfix()
        {
            Find.CurrentMap.roofGrid.Drawer.MarkForDraw();
        }
    }
}
