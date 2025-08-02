using HarmonyLib;
using Nebulae.RimWorld.UI;
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
    [HarmonyPatch(typeof(GlowGrid), nameof(GlowGrid.GroundGlowAt))]
    [HarmonyAfter("ReBuildDoorsAndCornersMod")]
    internal static class GlowGrid_Patch
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> GroundGlowAtTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofedMethod = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed), new Type[] { typeof(IntVec3) });

            foreach (var code in instructions)
            {
                if (!patched && code.opcode == OpCodes.Callvirt && (MethodInfo)code.operand == roofedMethod)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsLighttight), new Type[] { typeof(RoofGrid), typeof(IntVec3) }));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(GlowGrid), nameof(GlowGrid.GroundGlowAt));
        }
    }
}
