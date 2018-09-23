using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace HarvestYieldPatch
{
    [HarmonyPatch(typeof(CompHasGatherableBodyResource), nameof(CompHasGatherableBodyResource.Gathered))]
    public static class AnimalYieldPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AnimalYieldPatch_Prefix(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo RoundRandom = AccessTools.Method(typeof(GenMath), nameof(GenMath.RoundRandom));
            FieldInfo AnimalGatherYield = AccessTools.Field(typeof(StatDefOf), nameof(StatDefOf.AnimalGatherYield));
            MethodInfo GetStatValue = AccessTools.Method(typeof(StatExtension), nameof(StatExtension.GetStatValue));
            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Call && i.operand == RoundRandom)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);//doer pawn
                    yield return new CodeInstruction(OpCodes.Ldsfld, AnimalGatherYield);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Call, GetStatValue);
                    yield return new CodeInstruction(OpCodes.Mul);
                }
                yield return i;
            }
        }
    }
}
