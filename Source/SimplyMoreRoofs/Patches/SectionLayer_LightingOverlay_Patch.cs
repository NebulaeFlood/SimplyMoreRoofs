using HarmonyLib;
using Nebulae.RimWorld.UI;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(SectionLayer_LightingOverlay), "GenerateLightingOverlay")]
    public static class SectionLayer_LightingOverlay_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GenerateLightingOverlayTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool firstPointPatched = false;
            bool secondPointPatched = false;

            var isThickRoofField = AccessTools.Field(typeof(RoofDef), nameof(RoofDef.isThickRoof));
            var roofedMethod = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed), new Type[] { typeof(int) });

            var codes = instructions.ToArray();

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!firstPointPatched && code.opcode == OpCodes.Ldfld && (FieldInfo)code.operand == isThickRoofField)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsLighttight), new Type[] { typeof(RoofDef) }));
                    yield return new CodeInstruction(OpCodes.Brfalse, codes[i - 2].operand);
                    yield return codes[i - 1];
                    yield return code;

                    firstPointPatched = true;
                }
                else if (!secondPointPatched && code.opcode == OpCodes.Callvirt && (MethodInfo)code.operand == roofedMethod)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsLighttight), new Type[] { typeof(RoofGrid), typeof(int) }));

                    secondPointPatched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(firstPointPatched && secondPointPatched, typeof(SectionLayer_LightingOverlay), "GenerateLightingOverlay");
        }
    }
}
