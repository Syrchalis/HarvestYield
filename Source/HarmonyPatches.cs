using HarmonyLib;
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
        static HarmonyPatches()
        {
            var harmony = new Harmony("syrchalis.rimworld.harvestYieldPatch");
            harmony.Patch(typeof(RimWorld.JobDriver_PlantWork).
                GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First().
                GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
                MaxBy(mi => mi.GetMethodBody()?.GetILAsByteArray().Length ?? -1),
                transpiler: new HarmonyMethod(typeof(HarvestYieldPatch).GetMethod("Transpiler")));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        //Debug Options to set Milk Fullness and Wool Growth to max
        [DebugAction("Pawns", "Force Wool Growth", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ForceWool(Pawn p)
        {
            CompShearable compShearable = p.TryGetComp<CompShearable>();
            if (compShearable != null)
            {
                while (compShearable.Fullness < 1)
                {
                    compShearable.CompTick();
                }
                DebugActionsUtility.DustPuffFrom(p);
            }
        }
        [DebugAction("Pawns", "Force Milk Production", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ForceMilk(Pawn p)
        {
            CompMilkable compMilkable = p.TryGetComp<CompMilkable>();
            if (compMilkable != null)
            {
                while (compMilkable.Fullness < 1)
                {
                    compMilkable.CompTick();
                }
                DebugActionsUtility.DustPuffFrom(p);
            }
        }
    }
}