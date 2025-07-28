using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
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
