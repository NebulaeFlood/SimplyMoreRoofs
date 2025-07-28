using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(GravshipUtility), nameof(GravshipUtility.PreLaunchConfirmation))]
    internal static class GravshipUtility_Patch
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> PreLaunchConfirmationTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToArray();
            bool patched = false;
            var isThickRoofField = AccessTools.Field(typeof(RoofDef), nameof(RoofDef.isThickRoof));

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!patched && code.opcode == OpCodes.Ldfld && (FieldInfo)code.operand == isThickRoofField)
                {
                    yield return code;

                    var ldloc = codes[i - 1];
                    code = codes[++i];
                    var label = code.operand;

                    yield return code;
                    yield return ldloc;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.IsCustomRoof), new Type[] { typeof(RoofDef) }));
                    yield return new CodeInstruction(OpCodes.Brtrue, label);

                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(GravshipUtility), nameof(GravshipUtility.PreLaunchConfirmation));
        }
    }
}
