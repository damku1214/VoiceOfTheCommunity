using System;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class GingaPachinkoEvolveBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    public Character player = Singleton<Service>.Instance.levelManager.player;
    public ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

    private void Awake()
    {
        player.playerComponents.inventory.onUpdatedKeywordCounts += CheckToUpgradeItem;
    }

    private void OnDestroy()
    {
        player.playerComponents.inventory.onUpdatedKeywordCounts -= CheckToUpgradeItem;
    }

    private void CheckToUpgradeItem()
    {
        bool hasMask = false;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null || item.state != Gear.State.Equipped)
            {
                continue;
            }
            if (item.name.Equals("VeiledMask"))
            {
                hasMask = true;
                ChangeItem("Custom-MaskOfSogeking", item);
            }
            if (item.name.Equals("Custom-MaskOfSogeking"))
            {
                hasMask = true;
            }
        }
        if (_item.name == "Custom-GingaPachinko" && hasMask) ChangeItem("Custom-GingaPachinko_2", _item);
    }

    private void ChangeItem(string name, Item item, ItemReference itemRef = null)
    {
        if (GearResource.instance.TryGetItemReferenceByName(name, out itemRef))
        {
            ItemRequest request = itemRef.LoadAsync();
            request.WaitForCompletion();

            if (item.state == Gear.State.Equipped)
            {
                Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                item.ChangeOnInventory(newItem);
            }
        }
    }
}
