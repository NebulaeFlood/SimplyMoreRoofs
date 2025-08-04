using RimWorld;
using SimplyMoreRoofs.Utilities;
using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.Designators
{
    public sealed class AreaNoAnyRoof : Designator_Cells
    {
        public readonly string allowRemoveAnyRoofDesc = "SMR.Designators.AreaNoAnyRoof.Descripton".Translate();

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        #region Public Properties

        public bool AllowRemoveAnyRoof => SMR.Settings.AllowRemoveAnyRoof && SMRDefOf.SMR_ThickRoof.IsFinished;

        public override bool DragDrawMeasurements => true;

        public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Areas;

        public override string Desc => AllowRemoveAnyRoof ? allowRemoveAnyRoofDesc : defaultDesc;

        public override string DescPostfix => AllowRemoveAnyRoof ? defaultDescPostfix : string.Empty;

        #endregion


        public AreaNoAnyRoof()
        {
            defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
            defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea", true);
            hotKey = KeyBindingDefOf.Misc5;
            soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
            soundDragChanged = null;
            soundSucceeded = SoundDefOf.Designate_ZoneAdd;
            useMouseIcon = true;
        }


        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            var map = Map;

            if (!loc.InBounds(map) || loc.Fogged(map))
            {
                return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
            }

            var roofDef = map.roofGrid.RoofAt(loc);

            if (roofDef != null && roofDef.isThickRoof && !AllowRemoveAnyRoof && !roofDef.IsCustomRoof())
            {
                return new AcceptanceReport("MessageNothingCanRemoveThickRoofs".Translate());
            }

            return !map.areaManager.NoRoof[loc];
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            var map = Find.CurrentMap;

            map.areaManager.BuildRoof[c] = false;
            map.areaManager.NoRoof[c] = true;
        }

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();

            var map = Find.CurrentMap;

            map.areaManager.NoRoof.MarkForDraw();
            map.areaManager.BuildRoof.MarkForDraw();
            map.roofGrid.Drawer.MarkForDraw();
        }

        #endregion
    }
}
