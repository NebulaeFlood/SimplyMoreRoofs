using HarmonyLib;
using Nebulae.RimWorld.UI;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TradingControl.Harmonize;
using Verse;

namespace SimplyMoreRoofs.TradingControl.Patches
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches_Patch
    {
        static HarmonyPatches_Patch()
        {
            new Harmony("Nebulae.SimplyMoreRoofs.TradingControl").Patch(AccessTools.Method(typeof(HarmonyPatches), nameof(HarmonyPatches.CustomTradeDropSpot)),
                transpiler: new HarmonyMethod(typeof(HarmonyPatches_Patch), nameof(CustomTradeDropSpotTranspiler)));
        }


        public static IEnumerable<CodeInstruction> CustomTradeDropSpotTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofed = AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.Roofed), new Type[] { typeof(IntVec3) });

            var codes = instructions.ToArray();

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!patched && code.Calls(roofed))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.AllowFlyThrough), new Type[] { typeof(RoofGrid), typeof(IntVec3) }));
                    yield return new CodeInstruction(OpCodes.Brfalse, codes[++i].operand);
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(HarmonyPatches), nameof(HarmonyPatches.CustomTradeDropSpot));
        }
    }
}
