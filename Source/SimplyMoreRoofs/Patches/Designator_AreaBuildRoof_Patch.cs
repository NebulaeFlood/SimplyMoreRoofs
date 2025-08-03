using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(Designator_AreaBuildRoof), nameof(Designator_AreaBuildRoof.SelectedUpdate))]
    public static class Designator_AreaBuildRoof_Patch
    {
        [HarmonyPostfix]
        public static void SelectedUpdatePostfix()
        {
            Find.CurrentMap.roofGrid.Drawer.MarkForDraw();
        }
    }
}
