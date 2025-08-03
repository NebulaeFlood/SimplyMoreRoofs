using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(CompPowerPlantSolar), "RoofedPowerOutputFactor", MethodType.Getter)]
    public static class CompPowerPlantSolar_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RoofedPowerOutputFactorTranpiler(IEnumerable<CodeInstruction> instructions)
        {
            var patched = false;
            var roofed = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed), new Type[] { typeof(IntVec3) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(roofed))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsLighttight), new Type[] { typeof(RoofGrid), typeof(IntVec3) }));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(CompPowerPlantSolar), "RoofedPowerOutputFactor");
        }
    }
}
