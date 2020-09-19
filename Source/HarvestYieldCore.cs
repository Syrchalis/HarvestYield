using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RimWorld;
using Verse;
using UnityEngine;

namespace HarvestYieldPatch
{
    public class HarvestYieldCore : Mod
    {
        public static HarvestYieldSettings settings;

        public HarvestYieldCore(ModContentPack content) : base(content)
        {
            settings = GetSettings<HarvestYieldSettings>();
        }

        public override string SettingsCategory() => "HarvestYieldSettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            checked
            {
                Listing_Standard listing_Standard = new Listing_Standard();
                listing_Standard.Begin(inRect);
                if (HarvestYieldSettings.plantYieldMax > 5.0f)
                {
                    listing_Standard.Label("HarvestYieldMax".Translate() + ": " + "Unlimited".Translate(), -1, "HarvestYieldMaxTooltip".Translate());
                }
                else
                {
                    listing_Standard.Label("HarvestYieldMax".Translate() + ": " + HarvestYieldSettings.plantYieldMax.ToStringPercent(), -1, "HarvestYieldMaxTooltip".Translate());
                }
                listing_Standard.Gap(6f);
                HarvestYieldSettings.plantYieldMax = listing_Standard.Slider(GenMath.RoundTo(HarvestYieldSettings.plantYieldMax, 0.1f), 1f, 5.1f);
                listing_Standard.Gap(12f);

                if (HarvestYieldSettings.animalYieldMax > 5.0f)
                {
                    listing_Standard.Label("AnimalYieldMax".Translate() + ": " + "Unlimited".Translate(), -1, "AnimalYieldMaxTooltip".Translate());
                }
                else
                {
                    listing_Standard.Label("AnimalYieldMax".Translate() + ": " + HarvestYieldSettings.animalYieldMax.ToStringPercent(), -1, "AnimalYieldMaxTooltip".Translate());
                }
                listing_Standard.Gap(6f);
                HarvestYieldSettings.animalYieldMax = listing_Standard.Slider(GenMath.RoundTo(HarvestYieldSettings.animalYieldMax, 0.1f), 1f, 5.1f);
                listing_Standard.Gap(12f);

                if (HarvestYieldSettings.miningYieldMax > 5.0f)
                {
                    listing_Standard.Label("MiningYieldMax".Translate() + ": " + "Unlimited".Translate(), -1, "MiningYieldMaxTooltip".Translate());
                }
                else
                {
                    listing_Standard.Label("MiningYieldMax".Translate() + ": " + HarvestYieldSettings.miningYieldMax.ToStringPercent(), -1, "MiningYieldMaxTooltip".Translate());
                }
                listing_Standard.Gap(6f);
                HarvestYieldSettings.miningYieldMax = listing_Standard.Slider(GenMath.RoundTo(HarvestYieldSettings.miningYieldMax, 0.1f), 1f, 5.1f);
                listing_Standard.Gap(12f);

                if (HarvestYieldSettings.butcherYieldMax > 5.0f)
                {
                    listing_Standard.Label("ButcherYieldMax".Translate() + ": " + "Unlimited".Translate(), -1, "ButcherYieldMaxTooltip".Translate());
                }
                else
                {
                    listing_Standard.Label("ButcherYieldMax".Translate() + ": " + HarvestYieldSettings.butcherYieldMax.ToStringPercent(), -1, "ButcherYieldMaxTooltip".Translate());
                }
                listing_Standard.Gap(6f);
                HarvestYieldSettings.butcherYieldMax = listing_Standard.Slider(GenMath.RoundTo(HarvestYieldSettings.butcherYieldMax, 0.1f), 1f, 5.1f);
                listing_Standard.Gap(12f);

                listing_Standard.End();
                settings.Write();
            }
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            UpdateMaxValues();
        }
        public static void UpdateMaxValues()
        {
            StatDefOf.PlantHarvestYield.maxValue = HarvestYieldSettings.plantYieldMax > 5.0f ? 9999999f : HarvestYieldSettings.plantYieldMax;
            StatDefOf.AnimalGatherYield.maxValue = HarvestYieldSettings.animalYieldMax > 5.0f ? 9999999f : HarvestYieldSettings.plantYieldMax;
            StatDefOf.MiningYield.maxValue = HarvestYieldSettings.miningYieldMax > 5.0f ? 9999999f : HarvestYieldSettings.plantYieldMax;
            HarvestYieldDefOf.ButcheryFleshEfficiency.maxValue = HarvestYieldSettings.butcherYieldMax > 5.0f ? 9999999f : HarvestYieldSettings.plantYieldMax;
        }
    }

    public class HarvestYieldSettings : ModSettings
    {
        public static float plantYieldMax = 2;
        public static float animalYieldMax = 2;
        public static float miningYieldMax = 2;
        public static float butcherYieldMax = 2;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref plantYieldMax, "HarvestYieldMax", 2, false);
            Scribe_Values.Look<float>(ref animalYieldMax, "AnimalYieldMax", 2, false);
            Scribe_Values.Look<float>(ref miningYieldMax, "MiningYieldMax", 2, false);
            Scribe_Values.Look<float>(ref butcherYieldMax, "ButcherYieldMax", 2, false);
        }
    }
}
