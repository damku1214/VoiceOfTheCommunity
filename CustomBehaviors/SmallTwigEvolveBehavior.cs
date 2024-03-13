using System;
using Characters;
using Characters.Gear.Items;
using Characters.Gear.Weapons;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public sealed class SmallTwigEvolveBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    public Character player = Singleton<Service>.Instance.levelManager.player;

    private void Awake()
    {
        player.playerComponents.inventory.weapon.onSwap += CheckEvolveCondition;
        player.playerComponents.inventory.weapon.onChanged += OnSkullChanged;
        CheckEvolveCondition();
    }

    private void OnDestroy()
    {
        player.playerComponents.inventory.weapon.onSwap -= CheckEvolveCondition;
        player.playerComponents.inventory.weapon.onChanged -= OnSkullChanged;
        CheckEvolveCondition();
    }

    private void OnSkullChanged(Weapon old, Weapon @new)
    {
        CheckEvolveCondition();
    }

    private void CheckEvolveCondition()
    {
        if (player.playerComponents.inventory.weapon.current.name.Equals("Skul") ||
            player.playerComponents.inventory.weapon.current.name.Equals("HeroSkul")) ChangeItem();
    }

    private void ChangeItem()
    {
        ItemReference itemRef;
        if (GearResource.instance.TryGetItemReferenceByName(_item.name + "_2", out itemRef))
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
