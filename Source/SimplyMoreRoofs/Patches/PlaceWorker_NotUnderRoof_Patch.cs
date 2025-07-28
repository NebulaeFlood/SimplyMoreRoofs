using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(PlaceWorker_NotUnderRoof), nameof(PlaceWorker_NotUnderRoof.AllowsPlacing))]
    internal static class PlaceWorker_NotUnderRoof_Patch
    {
        [HarmonyPostfix]
        internal static void AllowsPlacingPostfix(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, ref AcceptanceReport __result)
        {
            if (__result.Accepted)
            {
                return;
            }

            if (checkingDef.Size.x == 1 && checkingDef.Size.z == 1)
            {
                if (!map.roofGrid.AllowFlyThrough(loc))
                {
                    __result = new AcceptanceReport("MustPlaceUnroofed".Translate());
                    return;
                }
            }
            else
            {
                foreach (IntVec3 intVec in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
                {
                    if (!map.roofGrid.AllowFlyThrough(intVec))
                    {
                        __result = new AcceptanceReport("MustPlaceUnroofed".Translate());
                        return;
                    }
                }
            }

            __result = AcceptanceReport.WasAccepted;
        }
    }
}
