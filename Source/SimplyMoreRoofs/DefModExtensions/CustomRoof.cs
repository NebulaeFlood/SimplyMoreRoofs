using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.DefModExtensions
{
    public sealed class CustomRoof : DefModExtension
    {
        public bool blockScanner = true;
        public bool buildable = true;
        public ThingDef builderDef;
        public Color color = Color.white;
        public string iconPath;
        public bool isArtificial = true;
        public bool isTransparent;
        public bool vanishOnCollapse = true;
    }
}
