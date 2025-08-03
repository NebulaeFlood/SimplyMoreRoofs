using HarmonyLib;
using RimWorld;
using SimplyMoreRoofs.Utilities;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SimplyMoreRoofs.Patches
{
    [HarmonyPatch(typeof(JobDriver_RemoveRoof), "DoEffect")]
    public static class JobDriver_RemoveRoof_Patch
    {
        [HarmonyPrefix]
        public static void DoEffectPrefix(JobDriver_RemoveRoof __instance)
        {
            var cell = __instance.job.GetTarget(TargetIndex.A).Cell;
            var map = __instance.pawn.MapHeld;
            var roofDef = map.roofGrid.RoofAt(cell);

            if (roofDef.IsCustomRoof(out var props) && props.buildable && props.builderDef.resourcesFractionWhenDeconstructed > 0f)
            {
                var materials = props.builderDef.costList;
                var thingOwner = new ThingOwner<Thing>();

                for (int i = materials.Count - 1; i >= 0; i--)
                {
                    var material = materials[i];

                    if (!material.IsChanceBased || Rand.Chance(material.DropChance))
                    {
                        int count = Mathf.Min(GenMath.RoundRandom(material.count * props.builderDef.resourcesFractionWhenDeconstructed), material.count);

                        if (count > 0)
                        {
                            var thing = ThingMaker.MakeThing(material.thingDef, material.stuff);
                            thing.stackCount = count;
                            thingOwner.TryAdd(thing);
                        }
                    }
                }

                thingOwner.TryDropAll(cell, map, ThingPlaceMode.Near);
            }
        }
    }
}
