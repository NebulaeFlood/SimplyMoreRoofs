using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SimplyMoreRoofs.DefModExtensions
{
    public sealed class CustomRoof : DefModExtension
    {
        public bool buildable = true;
        public ThingDef builderDef;
        public Color color = Color.white;
        public string iconPath;
        public bool isTransparent;
        public bool vanishOnCollapse = true;
    }
}
