using RimWorld;
using SimplyMoreRoofs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SimplyMoreRoofs.ThingComps
{
    public sealed class AsRoofAfterBuild : ThingComp
    {
        public Properties_AsRoofAfterBuild Props
        {
            get
            {
                return (Properties_AsRoofAfterBuild)props;
            }
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            var loc = parent.Position;
            var map = parent.Map;

            var roofDef = Props.roofDef;

            if (roofDef != parent.Map.roofGrid.RoofAt(parent.Position))
            {
                map.roofGrid.SetRoof(loc, roofDef);
                MoteMaker.PlaceTempRoof(loc, map);

                if ((roofDef.canCollapse && !RoofCollapseUtility.WithinRangeOfRoofHolder(loc, map))
                    || (ModsConfig.OdysseyActive && roofDef.AllowFlyThrough() && !GravshipUtility.InsideFootprint(loc, map)))
                {
                    map.roofCollapseBuffer.MarkToCollapse(loc);
                }
            }

            map.events.BuildingSpawned += OnSpawned;
        }


        private static void OnSpawned(Building building)
        {
            building.Map.events.BuildingSpawned -= OnSpawned;
            building.Destroy();
        }
    }


    public sealed class Properties_AsRoofAfterBuild : CompProperties
    {
        public RoofDef roofDef;


        public Properties_AsRoofAfterBuild()
        {
            compClass = typeof(AsRoofAfterBuild);
        }
    }
}
