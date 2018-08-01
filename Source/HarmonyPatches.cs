using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace HarvestYieldPatch
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        // this static constructor runs to create a HarmonyInstance and install a patch.
        static HarmonyPatches()
        {
            var harmony = HarmonyInstance.Create("syrchalis.rimworld.harvestYieldPatch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("[HarvestYieldPatch] loaded...");
        }
    }
}