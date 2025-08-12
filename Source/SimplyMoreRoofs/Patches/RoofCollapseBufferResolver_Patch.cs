using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(RoofCollapseBufferResolver), nameof(RoofCollapseBufferResolver.CollapseRoofsMarkedToCollapse))]
    public static class RoofCollapseBufferResolver_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(Map ___map)
        {
            bool anyRoofFlewAway = false;
            IntVec3 flewPos = IntVec3.Invalid;

            var cellsMarkedToCollapse = ___map.roofCollapseBuffer.CellsMarkedToCollapse;

            for (int i = cellsMarkedToCollapse.Count - 1; i >= 0; i--)
            {
                if (___map.roofGrid.RoofAt(cellsMarkedToCollapse[i]).AllowFlyAway())
                {
                    ___map.roofGrid.SetRoof(cellsMarkedToCollapse[i], null);

                    if (!anyRoofFlewAway)
                    {
                        anyRoofFlewAway = true;
                        flewPos = cellsMarkedToCollapse[i];
                    }

                    cellsMarkedToCollapse.RemoveAt(i);
                }
            }

            if (anyRoofFlewAway)
            {
                Find.LetterStack.ReceiveLetter(
                    "SMR.Letters.RoofFlewAway.Label".Translate(),
                    "SMR.Letters.RoofFlewAway.Text".Translate(),
                    SMRDefOf.SMR_RoofFlewAway,
                    new TargetInfo(flewPos, ___map));
            }
        }
    }
}
