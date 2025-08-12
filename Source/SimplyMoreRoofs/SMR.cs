using HarmonyLib;
using Nebulae.RimWorld.UI;
using Nebulae.RimWorld.UI.Automation;
using Nebulae.RimWorld.UI.Controls.Basic;
using SimplyMoreRoofs.Utilities;
using System.Reflection;
using Verse;

namespace SimplyMoreRoofs
{
    public sealed class SMR : NebulaeMod<SMRSettings>
    {
        public const string DebugLabel = "Simply More Roof";


        public static readonly Harmony HarmonyInstance;


        static SMR()
        {
            HarmonyInstance = new Harmony("Nebulae.SimplyMoreRoofs");
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        public SMR(ModContentPack content) : base(content) { }


        public override string SettingsCategory()
        {
            return "SMR.Settings.Category.Label".Translate();
        }


        protected override Control CreateContent()
        {
            return Settings.GenerateLayout();
        }

        protected override void OnInitializing()
        {
            if (Settings.SaperateRoofDesignators)
            {
                CustomRoofDesignatorUtility.SeperateDesignators();
            }
        }

        public override void WriteSettings()
        {
            base.WriteSettings();

            if (Settings.SaperateRoofDesignators)
            {
                CustomRoofDesignatorUtility.SeperateDesignators();
            }
            else
            {
                CustomRoofDesignatorUtility.MergeDesignators();
            }
        }
    }
}
