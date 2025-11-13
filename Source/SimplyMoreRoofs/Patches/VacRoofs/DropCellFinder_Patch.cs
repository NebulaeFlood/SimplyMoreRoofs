using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(DropCellFinder), nameof(DropCellFinder.CanPhysicallyDropInto))]
    public static class DropCellFinder_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CanPhysicallyDropIntoTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var getRoof = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.GetRoof), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(getRoof))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DropCellFinder_Patch), nameof(GetRoof)));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(DropCellFinder), nameof(DropCellFinder.CanPhysicallyDropInto));
        }


        public static RoofDef GetRoof(IntVec3 loc, Map map, bool canRoofPunch)
        {
            var roofDef = map.roofGrid.RoofAt(loc);

            if (canRoofPunch)
            {
                return roofDef;
            }

            return roofDef.AllowFlyThrough() ? null : roofDef;
        }
    }
}
