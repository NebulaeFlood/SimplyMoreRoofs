using RimWorld;
using Verse;

namespace SimplyMoreRoofs
{
    [DefOf]
    internal static class SMRDefOf
    {
        public static readonly ResearchProjectDef SMR_ThickRoof;


        static SMRDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SMRDefOf));
        }
    }
}
