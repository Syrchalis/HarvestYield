using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;
using System.Reflection;
using System.Reflection.Emit;

namespace HarvestYieldPatch
{ 
    [HarmonyPatch]
    public static class HarvestYieldPatch
    {
        public static MethodInfo match = typeof(Plant).GetMethod("YieldNow");
        public static MethodInfo replaceWith = typeof(HarvestYieldPatch).GetMethod("YieldNowPatch");
        public static MethodInfo TargetMethod()
        {
            Type mainType = typeof(JobDriver_PlantWork);
            Log.Message("TargetMethod: Main Type Found");
            Type iteratorType = mainType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First(t => t.FullName.Contains("c_Iterator"));
            Log.Message("TargetMethod: Iterator Type Resolved");
            Type anonStoreyType = iteratorType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First(t => t.FullName.Contains("c_AnonStorey"));
            Log.Message("TargetMethod: AnonStorey Type Resolved");
            return anonStoreyType.GetMethods().First(m => m.ReturnType == typeof(void));
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            foreach (CodeInstruction i in instr)
            {
                if (i.operand == match)
                {
                    Log.Message("Instruction insertion complete!");
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, replaceWith);
                }
                else
                {
                    yield return i;
                }
            }
        }
        public static int YieldNowPatch(Plant p, Pawn actor)
        {
            float harvestFactor = actor.GetStatValue(StatDefOf.PlantHarvestYield, true);
            float harvestAmount = p.YieldNow() * harvestFactor;

            if (harvestFactor < 1)
            {
                Log.Message("harvest Factor <1");
                return p.YieldNow();
            }
            else
            {
                Log.Message("harvest Factor >1");
                return GenMath.RoundRandom(harvestAmount);
            };
        }
    }
}