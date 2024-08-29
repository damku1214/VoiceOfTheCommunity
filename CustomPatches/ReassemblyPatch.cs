using HarmonyLib;
using static Characters.WitchBonus;
using VoiceOfTheCommunity.CustomAbilities;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class ReassemblyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ReviveOnce), "Revive")]
    static bool PreventEarlyRevive(ref ReviveOnce __instance)
    {
        ref var self = ref __instance;
        if (self._owner.ability.GetInstance<BottledFaelingAbility>() != null) return false;
        return true;
    }
}
