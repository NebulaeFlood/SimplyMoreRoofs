using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(ShipLandingArea), nameof(ShipLandingArea.RecalculateBlockingThing))]
    public static class ShipLandingArea_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RecalculateBlockingThingTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofed = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.opcode == OpCodes.Call && (MethodInfo)code.operand == roofed)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.RoofedOpaquely), new Type[] { typeof(IntVec3), typeof(Map) }));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(ShipLandingArea), nameof(ShipLandingArea.RecalculateBlockingThing));
        }
    }
}
