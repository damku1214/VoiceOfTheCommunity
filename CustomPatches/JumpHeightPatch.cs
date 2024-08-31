using Characters;
using Characters.Operations.Movement;
using HarmonyLib;
using Services;
using Singletons;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class JumpHeightPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Jump), "Run", [typeof(Character)])]
    static bool ModifyJumpHeight(ref Jump __instance, Character owner)
    {
        if (owner != Singleton<Service>.Instance.levelManager.player) return true;
        ref var self = ref __instance;
        var inventory = owner.playerComponents.inventory.item;
        int gryphonsFeatherCount = 0;

        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];

            if (item == null || !item.name.Equals("Custom-GryphonsFeather"))
            {
                continue;
            }

            if (item.state == Characters.Gear.Gear.State.Equipped)
            {
                gryphonsFeatherCount++;
            }
        }
        if (gryphonsFeatherCount > 0)
        {
            owner.movement.Jump(self._jumpHeight * (1 + 0.15f * gryphonsFeatherCount));
            return false;
        }
        return true;
    }
}
