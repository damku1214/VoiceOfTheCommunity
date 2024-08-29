using Characters.Gear.Upgrades;
using HarmonyLib;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class SpiritOfNegotiatorPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NegotiatorsCoin), "UpdateRerollPrice")]
    static bool UpdatePriceMultiplier(ref NegotiatorsCoin __instance)
    {
        ref var self = ref __instance;
        ItemPurchasePatch._spiritOfNegotiatorPriceMultiplier = self._collectorPriceMultipliers[self._stack];
        self._collector.additionalPriceMultiplier = self._collectorPriceMultipliers[self._stack] + (ItemPurchasePatch._stacks * ItemPurchasePatch.ShadowThiefSackCount() * 0.1f);
        self._stack = Mathf.Min(self._stack + 1, self._collectorPriceMultipliers.Length - 1);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NegotiatorsCoin), "Detach")]
    static void OnDetach()
    {
        ItemPurchasePatch._spiritOfNegotiatorPriceMultiplier = 1;
    }
}