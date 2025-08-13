using Nebulae.RimWorld.UI;
using Nebulae.RimWorld.UI.Automation.Attributes;
using Verse;

namespace SimplyMoreRoofs
{
    [LayoutModel("SMR.Settings")]
    public sealed class SMRSettings : NebulaeModSettings<SMRSettings>
    {
        [BooleanEntry]
        public bool AllowRemoveAnyRoof = false;
        [BooleanEntry]
        public bool PreventInfestation = true;
        [BooleanEntry]
        public bool SeparateRoofDesignators = false;


        public override void ExposeData()
        {
            Scribe_Values.Look(ref AllowRemoveAnyRoof, nameof(AllowRemoveAnyRoof), defaultValue: false);
            Scribe_Values.Look(ref PreventInfestation, nameof(PreventInfestation), defaultValue: true);
            Scribe_Values.Look(ref SeparateRoofDesignators, nameof(SeparateRoofDesignators), defaultValue: false);
        }

        public void Reset()
        {
            AllowRemoveAnyRoof = false;
            PreventInfestation = true;
            SeparateRoofDesignators = false;
        }
    }
}
