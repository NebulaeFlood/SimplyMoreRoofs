using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches.VacRoofs
{
    [HarmonyPatch(typeof(Building_TurretGun))]
    public static class Building_TurretGun_Patch
    {
        [HarmonyPatch(nameof(Building_TurretGun.GetGizmos), MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GetGizmosTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofed = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(roofed))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.RoofedSolid)));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(Building_TurretGun), nameof(Building_TurretGun.GetGizmos));
        }


        [HarmonyPatch(nameof(Building_TurretGun.GetInspectString))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GetInspectStringTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var roofed = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(roofed))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CustomRoofUtility), nameof(CustomRoofUtility.RoofedSolid)));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(Building_TurretGun), nameof(Building_TurretGun.GetInspectString));
        }

        [HarmonyPatch(nameof(Building_TurretGun.TryStartShootSomething))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TryStartShootSomethingTranspiler(IEnumerable<CodeInstruction> instructions)
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

            SMR.DebugLabel.TranspileMessage(patched, typeof(Building_TurretGun), nameof(Building_TurretGun.TryStartShootSomething));
        }
    }
}
