using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;
using System.Reflection.Emit;

namespace HarvestYieldPatch
{
    public static class HarvestYieldPatch
    {
        public static MethodInfo NewYieldNowMethod = typeof(HarvestYieldPatch).GetMethod("YieldNowPatch");
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo YieldNow = AccessTools.Method(typeof(Plant), nameof(Plant.YieldNow));
            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Callvirt && (MethodInfo)i.operand == YieldNow)
                {

                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, NewYieldNowMethod);
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