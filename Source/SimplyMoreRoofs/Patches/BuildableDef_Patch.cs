using HarmonyLib;
using SimplyMoreRoofs.ThingComps;
using SimplyMoreRoofs.Utilities;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(BuildableDef), nameof(BuildableDef.BuildableByPlayer), MethodType.Getter)]
    internal static class BuildableDef_Patch
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> BuildableByPlayerTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var designationCategory = AccessTools.Field(typeof(BuildableDef), nameof(BuildableDef.designationCategory));
            var label = il.DefineLabel();

            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldfld, designationCategory);
            yield return new CodeInstruction(OpCodes.Brfalse_S, label);
            yield return new CodeInstruction(OpCodes.Ldc_I4_1);
            yield return new CodeInstruction(OpCodes.Ret);
            yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(label);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BuildableDef_Patch), nameof(IsCustomRoofBuilder)));
            yield return new CodeInstruction(OpCodes.Ret);
        }


        private static bool IsCustomRoofBuilder(BuildableDef def)
        {
            if (def is ThingDef thingDef)
            {
                var comp = thingDef.GetCompProperties<Properties_AsRoofAfterBuild>();
                return comp != null && comp.roofDef.IsCustomRoof(out var props) && props.buildable;
            }

            return false;
        }
    }
}
