using Characters;
using Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;
using Characters.Player;
using HarmonyLib;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class OberonPatch
{
    public static bool oberonExists = false;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Oberon), "Initialize")]
    static void InitializeOberon(ref Oberon __instance)
    {
        ref var self = ref __instance;
        self._owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(ModifyOberonDamage));
        oberonExists = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Oberon), "OnDestroy")]
    static void DestroyOberon(ref Oberon __instance)
    {
        ref var self = ref __instance;
        self._owner.onGiveDamage.Remove(new GiveDamageDelegate(ModifyOberonDamage));
        oberonExists = false;
    }

    private static bool ModifyOberonDamage(ITarget target, ref Damage damage)
    {
        ItemInventory inventory = damage.attacker.character.playerComponents.inventory.item;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null)
            {
                continue;
            }
            if (item.name == "Custom-BottledFaeling" && damage.key == "Spirit")
            {
                damage.multiplier *= 1.15;
            }
        }
        return false;
    }
}
