using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(RitualObligationTargetWorker_SkyLanterns), "CanUseTargetInternal")]
    public static class RitualObligationTargetWorker_SkyLanterns_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CanUseTargetInternalTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofedMethod = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new Type[] { typeof(IntVec3), typeof(Map) });

            var codes = instructions.ToArray();

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!patched && code.opcode == OpCodes.Call && (MethodInfo)code.operand == roofedMethod)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.RoofedSolid)));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(RitualObligationTargetWorker_SkyLanterns), "CanUseTargetInternal");
        }
    }
}
