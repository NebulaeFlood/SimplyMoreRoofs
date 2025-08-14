using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.CanConstruct), typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool), typeof(JobDef))]
    public static class GenConstruct_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> CanConstructPostfix(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool patched = false;
            var thingDef = typeof(ThingDef);

            var codes = instructions.ToArray();

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!patched && code.opcode == OpCodes.Isinst && (Type)code.operand == thingDef)
                {
                    Label notRoofBuilder = il.DefineLabel();
                    var customRoofProps = il.DeclareLocal(typeof(DefModExtensions.CustomRoof));

                    yield return code;
                    yield return codes[++i];    // stloc.1
                    yield return codes[++i];    // ldloc.1
                    yield return codes[++i];    // brfalse
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ThingDef), nameof(ThingDef.comps)));
                    yield return new CodeInstruction(OpCodes.Ldloca_S, customRoofProps);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GenConstruct_Patch), nameof(IsConstructableRoof)));
                    yield return new CodeInstruction(OpCodes.Brfalse, notRoofBuilder);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, customRoofProps);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GenConstruct_Patch), nameof(AllowBuildRoof)));
                    yield return new CodeInstruction(OpCodes.Ret);
                    yield return codes[++i].WithLabels(notRoofBuilder);
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(GenConstruct), nameof(GenConstruct.CanConstruct));
        }


        public static bool AllowBuildRoof(Thing roofBuilder, DefModExtensions.CustomRoof roofProps)
        {
            return roofProps.allowFlyThrough
                ? RoofCollapseUtility.ConnectedToRoofHolder(roofBuilder.Position, roofBuilder.Map, true)
                : RoofCollapseUtility.WithinRangeOfRoofHolder(roofBuilder.Position, roofBuilder.Map);
        }

        public static bool IsConstructableRoof(List<CompProperties> comps, out DefModExtensions.CustomRoof props)
        {
            if (comps != null)
            {
                for (int i = comps.Count - 1; i >= 0; i--)
                {
                    if (comps[i] is ThingComps.Properties_AsRoofAfterBuild compProps)
                    {
                        return compProps.roofDef.IsCustomRoof(out props);
                    }
                }
            }

            props = null;
            return false;
        }
    }
}
