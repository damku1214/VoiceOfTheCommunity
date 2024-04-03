using System;
using Characters;
using Characters.Gear.Items;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public sealed class TaintedFingerEvolveBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    public Character player = Singleton<Service>.Instance.levelManager.player;
    public ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

    private void Awake()
    {
        player.playerComponents.inventory.item.onChanged += CheckEvolveCondition;
    }

    private void OnDestroy()
    {
        player.playerComponents.inventory.item.onChanged -= CheckEvolveCondition;
    }

    private void CheckEvolveCondition()
    {
        bool hasGraceOfLeonia = false;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null)
            {
                continue;
            }
            if (item.name.Equals("GraceOfLeonia"))
            {
                hasGraceOfLeonia = true;
            }
        }
        if (hasGraceOfLeonia)
        {
            ItemReference itemRef;
            if (GearResource.instance.TryGetItemReferenceByName("Custom-TaintedFinger_3", out itemRef))
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
}
