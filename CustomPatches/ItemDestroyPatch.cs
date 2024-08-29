using System;
using Characters.Gear.Items;
using Characters.Player;
using GameResources;
using HarmonyLib;
using Level;
using Services;
using Singletons;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class ItemDestroyPatch
{
    public static int _stacks;

    public static int DamoclesCount()
    {
        int count = 0;
        ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null)
            {
                continue;
            }
            if (item.state == Characters.Gear.Gear.State.Equipped && item.name.Equals("Custom-Damocles"))
            {
                count++;
            }
        }
        return count;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemInventory), "Discard", [typeof(int)])]
    static void AddStacks()
    {
        if (DamoclesCount() <= 0) _stacks = 0;
        else _stacks++;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DroppedGear), "InteractWithByPressing")]
    static void AddStacksAsWell(ref DroppedGear __instance)
    {
        ref var self = ref __instance;

        if (self.gear != null && !self.gear.lootable) return;
        if (self.gear.type != Characters.Gear.Gear.Type.Item) return;
        if (DamoclesCount() <= 0) _stacks = 0;
        else _stacks++;
    }
}
