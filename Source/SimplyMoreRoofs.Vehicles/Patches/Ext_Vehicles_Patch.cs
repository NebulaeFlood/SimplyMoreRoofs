using HarmonyLib;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicles;
using Verse;

namespace SimplyMoreRoofs.Vehicles.Patches
{
    [StaticConstructorOnStartup]
    internal static class Ext_Vehicles_Patch
    {
        static Ext_Vehicles_Patch()
        {
            new Harmony("Nebulae.SimplyMoreRoofs.Vehicles").Patch(AccessTools.Method(typeof(Ext_Vehicles), nameof(Ext_Vehicles.IsRoofed)),
                postfix: new HarmonyMethod(typeof(Ext_Vehicles_Patch), nameof(IsRoofedPostfix)));
        }


        internal static void IsRoofedPostfix(IntVec3 cell, Map map, ref bool __result)
        {
            if (__result && map.roofGrid.RoofAt(cell).AllowFlyThrough())
            {
                __result = false;
            }
        }
    }
}
