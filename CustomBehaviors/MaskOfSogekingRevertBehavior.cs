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
public sealed class MaskOfSogekingRevertBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    public Character player = Singleton<Service>.Instance.levelManager.player;
    public ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

    private void Awake()
    {
        player = Singleton<Service>.Instance.levelManager.player;
        inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
        inventory.onChanged += CheckToUpgradeItem;
    }

    private void OnDestroy()
    {
        player = Singleton<Service>.Instance.levelManager.player;
        inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
        inventory.onChanged -= CheckToUpgradeItem;
    }

    private void CheckToUpgradeItem()
    {
        if (_item.state != Gear.State.Equipped) return;
        OnDestroy();
        bool hasKabuto = false;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null || item.state != Gear.State.Equipped)
            {
                continue;
            }
            if (item.name.Equals("Custom-GingaPachinko_2"))
            {
                hasKabuto = true;
            }
        }
        if (!hasKabuto)
        {
            ChangeItem("VeiledMask", _item);
        }
        else
        {
            Awake();
        }
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
