using System;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class SwordOfTheToxicMoonEvolveBehavior : MonoBehaviour
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
        bool hasGrowingPotion = false;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null)
            {
                continue;
            }
            if (_item.state == Characters.Gear.Gear.State.Equipped && item.name.Equals("Custom-FrozenSpear"))
            {
                hasGrowingPotion = true;
                item.RemoveOnInventory();
            }
        }
        if (hasGrowingPotion)
        {
            ItemReference itemRef;
            if (GearResource.instance.TryGetItemReferenceByName("Custom-SwordOfTheToxicMoon_2", out itemRef))
            {
                ItemRequest request = itemRef.LoadAsync();
                request.WaitForCompletion();

                if (_item.state == Characters.Gear.Gear.State.Equipped)
                {
                    Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                    _item.ChangeOnInventory(newItem);

                    var spawner = Singleton<Service>.Instance.floatingTextSpawner;
                    var titlePosition = new Vector3(player.collider.bounds.center.x, player.collider.bounds.max.y + 1.0f, 0);
                    if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("두개의 달이 하늘에 현현합니다", titlePosition, "#4D3496");
                    else spawner.SpawnBuff("The Twin Moons Rise", titlePosition, "#4D3496");
                }
            }
        }
    }
}

