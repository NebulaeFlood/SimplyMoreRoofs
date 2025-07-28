using RimWorld;
using SimplyMoreRoofs.ThingComps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Verse;

namespace SimplyMoreRoofs.Utilities
{
    public static class CustomRoofUtility
    {
        public static bool AllowBuildRoof(this IntVec3 loc, Map map)
        {
            var things = map.thingGrid.ThingsListAtFast(loc);

            if (things != null)
            {
                for (int i = things.Count - 1; i >= 0; i--)
                {
                    if (things[i] is Blueprint_Build blueprint && blueprint.def.entityDefToBuild.IsLighttightRoof())
                    {
                        return false;
                    }
                    else if (things[i] is Frame frame && frame.def.entityDefToBuild.IsLighttightRoof())
                    {
                        return false;
                    }
                }
            }

            var roofDef = map.roofGrid.RoofAt(loc);

            if (roofDef is null || roofDef.modExtensions is null)
            {
                return true;
            }

            for (int i = roofDef.modExtensions.Count - 1; i >= 0; i--)
            {
                if (roofDef.modExtensions[i] is DefModExtensions.CustomRoof)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllowFlyThrough(this RoofDef roofDef)
        {
            if (roofDef.modExtensions is null)
            {
                return false;
            }

            for (int i = roofDef.modExtensions.Count - 1; i >= 0; i--)
            {
                if (roofDef.modExtensions[i] is DefModExtensions.VacProofRoof)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AllowFlyThrough(this RoofGrid roofGrid, IntVec3 loc)
        {
            var roofDef = roofGrid.RoofAt(loc);
            return roofDef != null && roofDef.AllowFlyThrough();
        }

        public static bool IsCustomRoof(this RoofDef roofDef)
        {
            if (roofDef is null || roofDef.modExtensions is null)
            {
                return false;
            }

            for (int i = roofDef.modExtensions.Count - 1; i >= 0; i--)
            {
                if (roofDef.modExtensions[i] is DefModExtensions.CustomRoof)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsCustomRoof(this RoofDef roofDef, out DefModExtensions.CustomRoof props)
        {
            if (roofDef is null || roofDef.modExtensions is null)
            {
                props = null;
                return false;
            }

            for (int i = roofDef.modExtensions.Count - 1; i >= 0; i--)
            {
                if (roofDef.modExtensions[i] is DefModExtensions.CustomRoof extension)
                {
                    props = extension;
                    return true;
                }
            }

            props = null;
            return false;
        }

        public static bool IsLighttight(this RoofDef roofDef)
        {
            return !roofDef.IsCustomRoof(out var props) || !props.isTransparent;
        }

        public static bool IsLighttight(this RoofGrid roofGrid, int index)
        {
            var roofDef = roofGrid.RoofAt(index);
            return roofDef != null && roofDef.IsLighttight();
        }

        public static bool IsLighttight(this RoofGrid roofGrid, IntVec3 loc)
        {
            var roofDef = roofGrid.RoofAt(loc);
            return roofDef != null && roofDef.IsLighttight();
        }

        public static bool RoofedOpaquely(this IntVec3 loc, Map map)
        {
            var roofDef = map.roofGrid.RoofAt(loc);
            return roofDef != null && roofDef.IsLighttight();
        }


        private static bool IsLighttightRoof(this BuildableDef def)
        {
            if (def is null)
            {
                return false;
            }

            if (def is ThingDef thingDef && thingDef.comps != null)
            {
                for (int i = thingDef.comps.Count - 1; i >= 0; i--)
                {
                    if (thingDef.comps[i] is Properties_AsRoofAfterBuild)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
