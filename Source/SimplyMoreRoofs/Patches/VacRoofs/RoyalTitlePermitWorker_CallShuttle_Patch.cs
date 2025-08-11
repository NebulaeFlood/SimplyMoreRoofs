using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(RoyalTitlePermitWorker_CallShuttle), "GetReportFromCell")]
    public static class RoyalTitlePermitWorker_CallShuttle_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GetReportFromCellTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var isThickRoofField = AccessTools.Field(typeof(RoofDef), nameof(RoofDef.isThickRoof));

            var codes = instructions.ToArray();

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
