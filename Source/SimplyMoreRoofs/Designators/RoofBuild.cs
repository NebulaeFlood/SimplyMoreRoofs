using RimWorld;
using SimplyMoreRoofs.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.Designators
{
    public sealed class RoofBuild : Designator
    {
        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        #region Public Properties

        public override string Desc => _selectedDesignator.Desc;
        public override string Label => _selectedDesignator.Label;

        #endregion


        static RoofBuild()
        {
            Designators = CreateRoofDesignators().ToArray();
        }

        public RoofBuild()
        {
            _selectedDesignator = Designators[0];

            icon = _selectedDesignator.icon;
            iconDrawScale = _selectedDesignator.iconDrawScale;
            iconProportions = _selectedDesignator.iconProportions;
            iconTexCoords = _selectedDesignator.iconTexCoords;
            iconAngle = _selectedDesignator.iconAngle;
            iconOffset = _selectedDesignator.iconOffset;
        }


        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            return _selectedDesignator.CanDesignateCell(loc);
        }

        public override void DrawPanelReadout(ref float curY, float width)
        {
            _selectedDesignator.DrawPanelReadout(ref curY, width);
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            var result = base.GizmoOnGUI(topLeft, maxWidth, parms);
            var width = GetWidth(maxWidth);
            GUI.DrawTexture(new Rect(topLeft.x + width - 17f, topLeft.y + 1f, 16f, 16f), Designator_Dropdown.PlusTex);
            return result;
        }

        public override void ProcessInput(Event ev)
        {
            var floatMenu = new FloatMenu(CreateMenuOptions(ev).ToList());

            Find.WindowStack.Add(floatMenu);
            Find.DesignatorManager.Select(_selectedDesignator);
        }

        public override void SelectedUpdate()
        {
            _selectedDesignator.SelectedUpdate();
        }

        #endregion


        //------------------------------------------------------
        //
        //  Private Static Methods
        //
        //------------------------------------------------------

        #region Private Static Methods

        private static IEnumerable<Designator> CreateRoofDesignators()
        {
            yield return new Designator_AreaBuildRoof();

            var roofDefs = DefDatabase<RoofDef>.AllDefs.OrderBy(GetUIOrder);

            foreach (var roofDef in roofDefs)
            {
                if (roofDef.IsCustomRoof(out var props) && props.buildable)
                {
                    yield return new BuildCustomRoof(props.builderDef);
                }
                else
                {
                    break;
                }
            }
        }

        public static float GetUIOrder(RoofDef roofDef)
        {
            return (roofDef.IsCustomRoof(out var props) && props.buildable) ? props.builderDef.uiOrder : float.PositiveInfinity;
        }

        #endregion


        private IEnumerable<FloatMenuOption> CreateMenuOptions(Event ev)
        {
            for (int i = 0; i < Designators.Length; i++)
            {
                var designator = Designators[i];

                if (designator.Visible)
                {
                    void OnSelected()
                    {
                        base.ProcessInput(ev);
                        Find.DesignatorManager.Select(designator);

                        icon = designator.icon;
                        iconDrawScale = designator.iconDrawScale;
                        iconProportions = designator.iconProportions;
                        iconTexCoords = designator.iconTexCoords;
                        iconAngle = designator.iconAngle;
                        iconOffset = designator.iconOffset;

                        _selectedDesignator = designator;
                    }

                    yield return new FloatMenuOption(designator.LabelCap, OnSelected, (Texture2D)designator.icon, designator.IconDrawColor);
                }
            }
        }


        private static readonly Designator[] Designators;


        private Designator _selectedDesignator;
    }
}
