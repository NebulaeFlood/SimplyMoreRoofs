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
    [HarmonyPatch(typeof(RoyalTitlePermitWorker_CallShuttle), "GetReportFromCell")]
    internal static class RoyalTitlePermitWorker_CallShuttle_Patch
    {
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> GetReportFromCellTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToArray();
            bool patched = false;
            var isThickRoofField = AccessTools.Field(typeof(RoofDef), nameof(RoofDef.isThickRoof));

            for (int i = 0; i < codes.Length; i++)
            {
                var code = codes[i];

                if (!patched && code.opcode == OpCodes.Ldfld && (FieldInfo)code.operand == isThickRoofField)
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.AllowFlyThrough), new Type[] { typeof(RoofDef) }));
                    yield return new CodeInstruction(OpCodes.Brtrue, codes[++i].operand);

                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(RoyalTitlePermitWorker_CallShuttle), "GetReportFromCell");
        }
    }
}
