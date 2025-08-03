using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(BuildableDef), nameof(BuildableDef.BuildableByPlayer), MethodType.Getter)]
    public static class BuildableDef_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BuildableByPlayerTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var designationCategory = AccessTools.Field(typeof(BuildableDef), nameof(BuildableDef.designationCategory));
            var label = il.DefineLabel();

            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldfld, designationCategory);
            yield return new CodeInstruction(OpCodes.Brfalse_S, label);
            yield return new CodeInstruction(OpCodes.Ldc_I4_1);
            yield return new CodeInstruction(OpCodes.Ret);
            yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(label);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsCustomRoofBuilder)));
            yield return new CodeInstruction(OpCodes.Ret);
        }
    }
}
