using RimWorld;
using SimplyMoreRoofs.Utilities;
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
                var loc = parent.Position;
                var map = parent.Map;

                var roofDef = Props.roofDef;

                if (roofDef.AllowFlyAway())
                {
                    if (!RoofCollapseUtility.ConnectedToRoofHolder(loc, map, true))
                    {
                        if (CustomRoofUtility.AllowSendRoofFlewLetter())
                        {
                            Find.LetterStack.ReceiveLetter(
                                "SMR.Letters.RoofFlewAway.Label".Translate(),
                                "SMR.Letters.RoofFlewAway.Text".Translate(),
                                SMRDefOf.SMR_RoofFlewAway,
                                new TargetInfo(loc, map));
                        }
                    }
                    else
                    {
                        map.roofGrid.SetRoof(loc, roofDef);
                        MoteMaker.PlaceTempRoof(loc, map);
                    }
                }
                else if (!RoofCollapseUtility.WithinRangeOfRoofHolder(loc, map))
                {
                    map.roofCollapseBuffer.MarkToCollapse(loc);
                }
                else
                {
                    map.roofGrid.SetRoof(loc, roofDef);
                    MoteMaker.PlaceTempRoof(loc, map);
                }

                parent.Destroy();
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
