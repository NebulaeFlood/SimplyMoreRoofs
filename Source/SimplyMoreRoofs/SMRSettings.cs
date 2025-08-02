using Nebulae.RimWorld.UI;
using Nebulae.RimWorld.UI.Automation.Attributes;
using Verse;

namespace SimplyMoreRoofs
{
    [LayoutModel("SMR.Settings")]
    public sealed class SMRSettings : NebulaeModSettings<SMRSettings>
    {
        [BooleanEntry]
        public bool PreventInfestation = true;


        public override void ExposeData()
        {
            Scribe_Values.Look(ref PreventInfestation, nameof(PreventInfestation), defaultValue: true);
        }

        public void Reset()
        {
            PreventInfestation = true;
        }
    }
}
