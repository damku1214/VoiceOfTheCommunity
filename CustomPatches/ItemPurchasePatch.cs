using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Items;
using Characters.Player;
using GameResources;
using HarmonyLib;
using Level;
using Level.BlackMarket;
using Services;
using Singletons;
using UnityEngine;
using static Data.GameData;

namespace VoiceOfTheCommunity.CustomPatches;

[HarmonyPatch]
public class ItemPurchasePatch
{
    public static int _stacks = 0;

    public static int ShadowThiefSackCount()
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
            if (item.state == Characters.Gear.Gear.State.Equipped && item.name.Equals("Custom-ShadowThiefsSack"))
            {
                count++;
            }
        }
        return count;
    }

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

    public static float _spiritOfNegotiatorPriceMultiplier = 1;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DroppedGear), "InteractWith")]
    static void ReturnItemPrice(ref DroppedGear __instance)
    {
        ref var self = ref __instance;

        if (self.gear != null && !self.gear.lootable) return;
        Currency currency = Currency.currencies[self.priceCurrency];
        ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
        Collector collector = Map.Instance.GetComponentInChildren<Collector>();
        Item _item = self.gear as Item;
        if (ShadowThiefSackCount() == 0)
        {
            _stacks = 0;
        }
        if (inventory.items.Any((Item i) => i == null) && ShadowThiefSackCount() > 0 && Southpaw.Random.NextInt(0, 99) <= Southpaw.Random.NextDouble(15 * ShadowThiefSackCount(), 25 * ShadowThiefSackCount()) && self.price > 0 && currency.Has(self.price) && collector != null && _item != null)
        {
            currency.Earn(self.price);
            _stacks++;
            collector.additionalPriceMultiplier = _spiritOfNegotiatorPriceMultiplier + (0.1f * _stacks * ShadowThiefSackCount());
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DroppedGear), "InteractWith")]
    static void DropRandomItem(ref DroppedGear __instance)
    {
        ref var self = ref __instance;

        if (self.gear != null && !self.gear.lootable) return;
        Currency currency = Currency.currencies[self.priceCurrency];
        ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
        var gearManager = Singleton<Service>.Instance.gearManager;
        var random = new System.Random();
        Item _item = self.gear as Item;

        if (DamoclesCount() == 0) return;
        if (inventory.items.Any((Item i) => i == null) && self.price > 0 && currency.Has(self.price) && _item != null)
        {
            for (int i = 0; i < DamoclesCount(); i++)
            {
                ItemReference itemReference;
                if (Southpaw.Random.NextInt(0, 99) <= 5)
                {
                    itemReference = gearManager.GetOmenItems(random);
                }
                else
                {
                    Rarity rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings.collectorItemPossibilities.Evaluate(random);
                    itemReference = gearManager.GetItemToTake(random, rarity);
                }
                DropItem(itemReference);
            }
        }
    }

    private static void DropItem(ItemReference itemRef)
    {
        ItemRequest request = itemRef.LoadAsync();
        request.WaitForCompletion();

        Singleton<Service>.Instance.levelManager.DropItem(request, new Vector3(Singleton<Service>.Instance.levelManager.player.collider.bounds.center.x, Singleton<Service>.Instance.levelManager.player.collider.bounds.max.y, 0));
    }
}
