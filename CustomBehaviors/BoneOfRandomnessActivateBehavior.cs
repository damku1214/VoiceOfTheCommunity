using System;
using System.Collections.Generic;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class BoneOfRandomnessActivateBehavior : MonoBehaviour
{
    [SerializeField]
    private Item _item = null;

    [SerializeField]
    private bool _isKeeped = false;

    public Character player = Singleton<Service>.Instance.levelManager.player;
    public ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

    private void Update()
    {
        if (_item.state != Gear.State.Equipped || _isKeeped) return;

        bool hasBone = false;
        List<Item> havingBoneItemList = [];

        for (int i = 0; i < inventory.items.Count; i++)
        {
            var item = inventory.items[i];
            if (item == null || item.name == _item.name)
            {
                continue;
            }
            if (item.keyword1 == Inscription.Key.Bone || item.keyword2 == Inscription.Key.Bone)
            {
                // Made for items that have the bone evolution aspects but are a prevolution: for example, idol of insanity with the bone inscription.
                if (item.name == "OmenClone_2")
                {
                    hasBone = true;
                    havingBoneItemList.Add(item);
                    continue;
                }
                if (item.name.EndsWith("_2") || item.name.EndsWith("_3") || item.name.EndsWith("_BoneUpgrade")) continue;
                hasBone = true;
                havingBoneItemList.Add(item);
            }
        }

        if (hasBone)
        {
            ChangeItem(havingBoneItemList[Southpaw.Random.NextInt(0, havingBoneItemList.Count - 1)]);
            _item.RemoveOnInventory();
        } else if (Southpaw.Random.NextInt(0, 9) > 0)
        {
            List<ItemReference> boneItemReferenceList = [];
            foreach (ItemReference itemReference in GearResource.instance._items)
            {
                if (!itemReference.obtainable && itemReference.gearTag != Gear.Tag.Omen) continue;
                if (itemReference.prefabKeyword1 == Inscription.Key.Bone || itemReference.prefabKeyword2 == Inscription.Key.Bone) boneItemReferenceList.Add(itemReference);
            }
            DropItem(boneItemReferenceList[Southpaw.Random.NextInt(0, boneItemReferenceList.Count - 1)].name);
            _item.RemoveOnInventory();
        } else
        {
            _isKeeped = true;
        }
    }

    private void ChangeItem(Item item)
    {
        ItemReference itemRef;
        if (GearResource.instance.TryGetItemReferenceByName(item.name.StartsWith("Custom-") ? item.name + "_BoneUpgrade" : item.name == "CloneStamp" || item.name == "PrincesBox" ? item.name + "_3" : item.name + "_2", out itemRef))
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

    private void DropItem(string name)
    {
        ItemReference itemRef;
        if (GearResource.instance.TryGetItemReferenceByName(name, out itemRef))
        {
            ItemRequest request = itemRef.LoadAsync();
            request.WaitForCompletion();

            Singleton<Service>.Instance.levelManager.DropItem(request, new Vector3(player.collider.bounds.center.x, player.collider.bounds.max.y, 0));
        }
    }
}
