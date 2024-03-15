using System;
using Characters;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class WingedSpearEvolveBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    private Character player = Singleton<Service>.Instance.levelManager.player;

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
        var inventory = player.playerComponents.inventory.item;
        ItemReference itemRef = null;
        int positionofOriginialItem = 0;

        for (int i = 0; i < inventory.items.Count; i++)
        {
            try
            {
                var item = inventory.items[i];

                if (item == null)
                {
                    continue;
                }

                if (item.name.Equals("Custom-WingedSpear"))
                {
                    positionofOriginialItem = i;
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[VoiceOfCommunity - Winged Spear] There is no item at item index " + i);
                Debug.LogWarning(e.Message);
            }
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            try
            {
                var item = inventory.items[i];

                if (item == null)
                {
                    continue;
                }

                if (item.name.Equals("SwordOfSun"))
                {
                    inventory.items.RemoveAt(positionofOriginialItem);
                    ChangeItem("Custom-WingedSpear_2", item, itemRef);
                    break;
                }

                if (item.name.Equals("RingOfMoon"))
                {
                    inventory.items.RemoveAt(positionofOriginialItem);
                    ChangeItem("Custom-WingedSpear_3", item, itemRef);
                    break;
                }

                if (item.name.Equals("Custom-WingedSpear") && positionofOriginialItem != i)
                {
                    inventory.items.RemoveAt(positionofOriginialItem);
                    ChangeItem("Custom-WingedSpear_4", item, itemRef);
                    break;
                }

                if (item.name.Equals("ShardOfDarkness"))
                {
                    inventory.items.RemoveAt(positionofOriginialItem);
                    ChangeItem("Custom-WingedSpear_5", item, itemRef);
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[VoiceOfCommunity - Winged Spear] There is no item at item index " + i);
                Debug.LogWarning(e.Message);
            }
        }
    }

    private void ChangeItem(string name, Item item, ItemReference itemRef)
    {
        if (GearResource.instance.TryGetItemReferenceByName(name, out itemRef))
        {
            ItemRequest request = itemRef.LoadAsync();
            request.WaitForCompletion();

            if (item.state == Characters.Gear.Gear.State.Equipped)
            {
                Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                item.ChangeOnInventory(newItem);
            }
        }
    }
}
