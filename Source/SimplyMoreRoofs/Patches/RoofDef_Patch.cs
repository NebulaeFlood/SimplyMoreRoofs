using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(RoofDef), nameof(RoofDef.VanishOnCollapse), MethodType.Getter)]
    internal static class RoofDef_Patch
    {
        [HarmonyPrefix]
        internal static bool VanishOnCollapsePrefix(RoofDef __instance, ref bool __result)
        {
            if (__instance.IsCustomRoof(out var props) && props.vanishOnCollapse)
            {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
