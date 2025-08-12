using RimWorld;
using SimplyMoreRoofs.ThingComps;
using System;
using UnityEngine;
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
                    if (things[i] is Blueprint_Build blueprint && blueprint.def.entityDefToBuild.IsRoofBuilder())
                    {
                        return false;
                    }
                    else if (things[i] is Frame frame && frame.def.entityDefToBuild.IsRoofBuilder())
                    {
                        return false;
                    }
                }
            }

            return !map.roofGrid.RoofAt(loc).IsCustomRoof();
        }

        public static bool AllowFlyAway(this RoofDef roofDef)
        {
            return roofDef != null && !roofDef.canCollapse && roofDef.AllowFlyThrough();
        }

        public static bool AllowFlyThrough(this RoofDef roofDef)
        {
            if (roofDef is null)
            {
                return true;
            }

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
            return roofDef is null || roofDef.AllowFlyThrough();
        }

        public static bool AllowSendRoofFlewLetter()
        {
            var time = Time.time;
            var letters = Find.LetterStack.LettersListForReading;

            for (int i = letters.Count - 1; i >= 0; i--)
            {
                var letter = letters[i];

                if (letter.def == SMRDefOf.SMR_RoofFlewAway && MathF.Abs(letter.arrivalTime - time) < 0.45f)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool BlockScanner(this RoofGrid roofGrid, IntVec3 loc)
        {
            return roofGrid.RoofAt(loc).IsCustomRoof(out var props) && props.blockScanner;
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

        public static bool IsCustomRoofBuilder(BuildableDef def)
        {
            if (def is ThingDef thingDef && thingDef.comps != null)
            {
                for (int i = thingDef.comps.Count - 1; i >= 0; i--)
                {
                    if (thingDef.comps[i] is Properties_AsRoofAfterBuild builderProps)
                    {
                        return builderProps.roofDef.IsCustomRoof(out var props) && props.buildable;
                    }
                }
            }

            return false;
        }

        public static bool IsLighttight(this RoofDef roofDef)
        {
            if (roofDef is null)
            {
                return false;
            }

            if (roofDef.modExtensions is null)
            {
                return true;
            }

            for (int i = roofDef.modExtensions.Count - 1; i >= 0; i--)
            {
                if (roofDef.modExtensions[i] is DefModExtensions.CustomRoof extension)
                {
                    return !extension.isTransparent;
                }
            }

            return true;
        }

        public static bool IsLighttight(this RoofGrid roofGrid, int index)
        {
            return roofGrid.RoofAt(index).IsLighttight();
        }

        public static bool IsLighttight(this RoofGrid roofGrid, IntVec3 loc)
        {
            return roofGrid.RoofAt(loc).IsLighttight();
        }

        public static bool RoofedOpaquely(this IntVec3 loc, Map map)
        {
            return map.roofGrid.RoofAt(loc).IsLighttight();
        }

        public static bool RoofedSolid(this IntVec3 loc, Map map)
        {
            return !map.roofGrid.AllowFlyThrough(loc);
        }


        private static bool IsRoofBuilder(this BuildableDef def)
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
