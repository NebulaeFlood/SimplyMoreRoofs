using RimWorld;
using SimplyMoreRoofs.Designators;
using System;
using Verse;

namespace SimplyMoreRoofs.Utilities
{
    public static class CustomRoofDesignatorUtility
    {
        public static void MergeDesignators()
        {
            foreach (var roofDef in DefDatabase<RoofDef>.AllDefs)
            {
                if (roofDef.IsCustomRoof(out var props) && props.buildable)
                {
                    props.builderDef.designationCategory = null;
                    props.builderDef.designatorDropdown = null;
                }
            }

            DesignationCategoryDefOf.Zone.DirtyCache();
            DesignationCategoryDefOf.Zone.specialDesignatorClasses.Replace(typeof(Designator_AreaBuildRoof), typeof(RoofBuild));
            DesignationCategoryDefOf.Zone.ResolveReferences();
        }

        public static void SeparateDesignators()
        {
            foreach (var roofDef in DefDatabase<RoofDef>.AllDefs)
            {
                if (roofDef.IsCustomRoof(out var props) && props.buildable)
                {
                    props.builderDef.designationCategory = DesignationCategoryDefOf.Zone;
                    props.builderDef.designatorDropdown = SMRDefOf.SMR_CustomRoofGroup;
                }
            }

            DesignationCategoryDefOf.Zone.DirtyCache();
            DesignationCategoryDefOf.Zone.specialDesignatorClasses.Replace(typeof(RoofBuild), typeof(Designator_AreaBuildRoof));
            DesignationCategoryDefOf.Zone.ResolveReferences();
        }
    }
}
