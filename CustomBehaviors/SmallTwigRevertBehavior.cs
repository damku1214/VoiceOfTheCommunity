using System;
using Characters;
using Characters.Gear.Items;
using Characters.Gear.Weapons;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public sealed class SmallTwigRevertBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    public Character player = Singleton<Service>.Instance.levelManager.player;

    private void Awake()
    {
        player.playerComponents.inventory.weapon.onSwap += RefreshStats;
        player.playerComponents.inventory.weapon.onChanged += OnSkullChanged;
        RefreshStats();
    }

    private void OnDestroy()
    {
        player.playerComponents.inventory.weapon.onSwap -= RefreshStats;
        player.playerComponents.inventory.weapon.onChanged -= OnSkullChanged;
        RefreshStats();
    }

    private void OnSkullChanged(Weapon old, Weapon @new)
    {
        RefreshStats();
    }

    private void RefreshStats()
    {
        if (!player.playerComponents.inventory.weapon.current.name.Equals("Skul") &&
            !player.playerComponents.inventory.weapon.current.name.Equals("HeroSkul")) ChangeItem();
    }

    private void ChangeItem()
    {
        ItemReference itemRef;
        if (GearResource.instance.TryGetItemReferenceByName(_item.name.Substring(0, _item.name.Length - 2), out itemRef))
        {
            ItemRequest request = itemRef.LoadAsync();
            request.WaitForCompletion();

            if (_item.state == Characters.Gear.Gear.State.Equipped)
            {
                Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                _item.ChangeOnInventory(newItem);
            }
        }
    }
}
