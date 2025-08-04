using HarmonyLib;
using Nebulae.RimWorld.UI;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(Need_Outdoors), nameof(Need_Outdoors.NeedInterval))]
    public static class Need_Outdoors_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> NeedIntervalTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patched = false;
            var getRoof = AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.GetRoof), new Type[] { typeof(IntVec3), typeof(Map) });

            foreach (var code in instructions)
            {
                if (!patched && code.Calls(getRoof))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Need_Outdoors_Patch), nameof(GetRoof)));
                    patched = true;
                }
                else
                {
                    yield return code;
                }
            }

            SMR.DebugLabel.TranspileMessage(patched, typeof(Need_Outdoors), nameof(Need_Outdoors.NeedInterval));
        }


        public static RoofDef GetRoof(IntVec3 loc, Map map)
        {
            var roofDef = map.roofGrid.RoofAt(loc);

            if (roofDef is null)
            {
                return null;
            }
            else if (roofDef.IsCustomRoof(out var props))
            {
                if (props.isTransparent)
                {
                    return null;
                }
                else if (props.isArtificial)
                {
                    return RoofDefOf.RoofConstructed;
                }
                else
                {
                    return roofDef;
                }
            }
            else
            {
                return roofDef;
            }
        }
    }
}
