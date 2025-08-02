using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(RoofGrid))]
    internal static class RoofGrid_Patch
    {
        [HarmonyPatch(nameof(RoofGrid.Color), MethodType.Getter)]
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            yield return new CodeInstruction(OpCodes.Ldc_R4, 1f);
            yield return new CodeInstruction(OpCodes.Ldc_R4, 1f);
            yield return new CodeInstruction(OpCodes.Ldc_R4, 1f);
            yield return new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Color), new[] { typeof(float), typeof(float), typeof(float) }));
            yield return new CodeInstruction(OpCodes.Ret);
        }

        [HarmonyPatch(nameof(RoofGrid.GetCellExtraColor))]
        [HarmonyPostfix]
        internal static void GetCellExtraColorPostfix(RoofDef[] ___roofGrid, int index, ref Color __result)
        {
            if (___roofGrid[index].IsCustomRoof(out var props))
            {
                __result = props.color;
            }
            else if (__result == White)
            {
                __result = DefaultColor;
            }
            else
            {
                __result *= DefaultColor;
            }
        }


        private static readonly Color DefaultColor = new Color(0.3f, 1f, 0.4f);
        private static readonly Color White = Color.white;
    }
}
