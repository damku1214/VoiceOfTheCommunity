using System;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

[Serializable]
public sealed class BrokenHeartEvolveBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    private void Update()
    {
        var player = Singleton<Service>.Instance.levelManager.player;
        if (player.playerComponents.inventory.quintessence.items.Random().name.Equals("Succubus")) ChangeItem();
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
