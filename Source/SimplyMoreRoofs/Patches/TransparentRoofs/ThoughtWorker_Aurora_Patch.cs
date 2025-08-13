using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.TransparentRoofs
{
    [HarmonyPatch(typeof(ThoughtWorker_Aurora), "CurrentStateInternal")]
    public static class ThoughtWorker_Aurora_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CurrentStateInternalTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofed = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(roofed))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.RoofedOpaquely), new Type[] { typeof(IntVec3), typeof(Map) }));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(ThoughtWorker), "CurrentStateInternal");
        }
    }
}
