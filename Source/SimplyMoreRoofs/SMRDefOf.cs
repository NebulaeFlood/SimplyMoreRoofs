using RimWorld;
using Verse;

namespace SimplyMoreRoofs
{
    [DefOf]
    public static class SMRDefOf
    {
        public static readonly DesignatorDropdownGroupDef SMR_CustomRoofGroup;

        public static readonly LetterDef SMR_RoofFlewAway;

        public static readonly ResearchProjectDef SMR_ThickRoof;


        static SMRDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SMRDefOf));
        }
    }
}
