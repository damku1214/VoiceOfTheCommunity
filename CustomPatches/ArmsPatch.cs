using Characters.Gear.Synergy.Inscriptions;
using Characters.Operations.Attack;
using Characters.Operations.Summon;
using HarmonyLib;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class ArmsPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Arms.AdditionalHit), "HandleOnStartAction")]
    static void ModifyArmsHit(ref Arms.AdditionalHit __instance)
    {
        ref var self = ref __instance;
        SummonOperationRunner armsAtk = null;
        SweepAttack sweepAttack = null;
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    if (self._additionalAttackOnGround._components[0]._operation is SummonOperationRunner) armsAtk = self._additionalAttackOnGround._components[0]._operation as SummonOperationRunner;
                    break;
                case 1:
                    if (self._additionalAttackOnAir._components[0]._operation is SummonOperationRunner) armsAtk = self._additionalAttackOnAir._components[0]._operation as SummonOperationRunner;
                    break;
                case 2:
                    if (self._additionalEnhancedAttackOnGround._components[0]._operation is SummonOperationRunner) armsAtk = self._additionalEnhancedAttackOnGround._components[0]._operation as SummonOperationRunner;
                    break;
                case 3:
                    if (self._additionalEnhancedAttackOnAir._components[0]._operation is SummonOperationRunner) armsAtk = self._additionalEnhancedAttackOnAir._components[0]._operation as SummonOperationRunner;
                    break;
                case 4:
                    if (self._superAttackOnGround._components[0]._operation is SummonOperationRunner) armsAtk = self._superAttackOnGround._components[0]._operation as SummonOperationRunner;
                    break;
                case 5:
                    if (self._superAttackOnAir._components[0]._operation is SummonOperationRunner) armsAtk = self._superAttackOnAir._components[0]._operation as SummonOperationRunner;
                    break;
            }
            if (i == 3 || i == 5)
            {
                if (armsAtk._operationRunner._operationInfos._operations._components[1]._operation is SweepAttack) sweepAttack = armsAtk._operationRunner._operationInfos._operations._components[1]._operation as SweepAttack;
            } else
            {
                if (armsAtk._operationRunner._operationInfos._operations._components[0]._operation is SweepAttack) sweepAttack = armsAtk._operationRunner._operationInfos._operations._components[0]._operation as SweepAttack;
            }
            sweepAttack._hitInfo._key = "Arms";
        }
    }
}
