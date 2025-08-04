using RimWorld;
using SimplyMoreRoofs.Utilities;
using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.Designators
{
    public sealed class BuildCustomRoof : Designator_Build
    {
        public BuildCustomRoof(BuildableDef entDef) : base(entDef) { }


        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            var map = Map;

            if (!loc.InBounds(map) || loc.Fogged(map))
            {
                return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
            }

            var roofDef = map.roofGrid.RoofAt(loc);

            if (roofDef != null && roofDef.isThickRoof)
            {
                return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
            }

            if (!loc.AllowBuildRoof(map))
            {
                return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
            }

            return AcceptanceReport.WasAccepted;
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            base.DesignateSingleCell(c);

            if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(TutorTagDesignate, c)))
            {
                return;
            }

            if (eyedropMode)
            {
                return;
            }

            var areas = Map.areaManager;
            areas.BuildRoof[c] = true;
            areas.NoRoof[c] = false;
        }

        protected override void DrawGhost(Color ghostCol)
        {
            base.DrawGhost(ghostCol);

            var map = Map;

            if (map is null)
            {
                return;
            }

            GenUI.RenderMouseoverBracket();

            map.areaManager.BuildRoof.MarkForDraw();
            map.areaManager.NoRoof.MarkForDraw();
            map.roofGrid.Drawer.MarkForDraw();
        }
    }
}
