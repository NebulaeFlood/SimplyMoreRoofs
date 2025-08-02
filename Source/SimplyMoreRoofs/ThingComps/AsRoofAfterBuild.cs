using RimWorld;
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


        public override void CompTick()
        {
            if (!parent.Destroyed)
            {
                parent.Destroy();
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

                if (roofDef.canCollapse && !RoofCollapseUtility.WithinRangeOfRoofHolder(loc, map))
                {
                    map.roofCollapseBuffer.MarkToCollapse(loc);
                }
            }
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
