using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using TradingControl.functions;
using Verse;

namespace SimplyMoreRoofs.TradingControl.Patches
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches_Patch
    {
        static HarmonyPatches_Patch()
        {
            new Harmony("Nebulae.SimplyMoreRoofs.TradingControl").Patch(AccessTools.Method(typeof(OrbitDropSpot), nameof(OrbitDropSpot.AnyAdjacentGoodDropSpot)),
                prefix: new HarmonyMethod(typeof(HarmonyPatches_Patch), nameof(AnyAdjacentGoodDropSpotPrefix)));
        }


        public static bool AnyAdjacentGoodDropSpotPrefix(IntVec3 c, Map map, ref bool __result)
        {
            if (CanDropPod(c, map))
            {
                __result = true;
                return false;
            }

            return true;
        }

        public static bool CanDropPod(IntVec3 loc, Map map)
        {
            var locs = new IntVec3[] { loc, loc + IntVec3.North, loc + IntVec3.South, loc + IntVec3.East, loc + IntVec3.West };

            if (!locs.InBoundAndStandable(map))
            {
                return false;
            }

            if (!map.roofGrid.AllowFlyThrough(loc))
            {
                if (DebugViewSettings.drawDestSearch)
                {
                    map.debugDrawer.FlashCell(loc, text: "phys");
                }

                return false;
            }

            if (Current.ProgramState is ProgramState.Playing && locs.Fogged(map))
            {
                return false;
            }

            for (int i = 0; i < locs.Length; i++)
            {
                var things = locs[i].GetThingList(map);

                for (int j = things.Count - 1; j >= 0; j--)
                {
                    var thing = things[j];

                    if (thing is IActiveTransporter || thing is Skyfaller)
                    {
                        return false;
                    }

                    if (!(thing is Building building) || !building.IsClearableFreeBuilding)
                    {
                        if (thing.def.IsEdifice()
                            || thing.def.preventSkyfallersLandingOn
                            || (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool InBoundAndStandable(this IntVec3[] locs, Map map)
        {
            for (int i = 0; i < locs.Length; i++)
            {
                var loc = locs[i];

                if (!loc.InBounds(map) || !loc.Standable(map))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Fogged(this IntVec3[] locs, Map map)
        {
            for (int i = 0; i < locs.Length; i++)
            {
                var loc = locs[i];

                if (loc.Fogged(map))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
