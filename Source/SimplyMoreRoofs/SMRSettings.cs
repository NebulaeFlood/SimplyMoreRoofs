using Nebulae.RimWorld.UI;
using Nebulae.RimWorld.UI.Automation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
