using HarmonyLib;
using Nebulae.RimWorld.UI;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(RoofUtility), nameof(RoofUtility.IsAnyCellUnderRoof))]
    internal static class RoofUtility_Patch
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> IsAnyCellUnderRoofTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofedMethod = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed), new Type[] { typeof(IntVec3) });

            foreach (var code in instructions)
            {
                if (!patched && code.opcode == OpCodes.Callvirt && (MethodInfo)code.operand == roofedMethod)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.BlockScanner), new Type[] { typeof(RoofGrid), typeof(IntVec3) }));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(RoofUtility), nameof(RoofUtility.IsAnyCellUnderRoof));
        }
    }
}
