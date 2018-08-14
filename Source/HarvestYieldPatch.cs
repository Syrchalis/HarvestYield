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
#if DEBUG
            Log.Message("[HarvestYieldPatch]TargetMethod: Main Type Found");
#endif
            Type iteratorType = mainType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First(t => t.FullName.Contains("c__Iterator"));
#if DEBUG
            Log.Message("[HarvestYieldPatch]TargetMethod: Iterator Type Resolved");
#endif
            Type anonStoreyType = iteratorType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First(t => t.FullName.Contains("c__AnonStorey"));
#if DEBUG
            Log.Message("[HarvestYieldPatch]TargetMethod: AnonStorey Type Resolved");
#endif
            return anonStoreyType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.Name.Contains("m__") && m.ReturnType == typeof(void));
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            foreach (CodeInstruction i in instr)
            {
                if (i.operand == match)
                {
                    //Log.Message("Instruction insertion complete!");
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
            return GenMath.RoundRandom(p.YieldNow() * actor.GetStatValue(StatDefOf.PlantHarvestYield, true));
        }
    }
}