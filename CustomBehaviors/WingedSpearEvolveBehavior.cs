using System;
using Characters;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
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
    private EnumArray<Inscription.Key, Inscription> inscriptions;

    public int SunAndMoonInscriptionCount()
    {
        foreach (var inscription in inscriptions)
        {
            if (inscription.key == Inscription.Key.SunAndMoon)
            {
                return inscription.count;
            }
        }
        return 0;
    }

    private void Awake()
    {
        player.playerComponents.inventory.onUpdatedKeywordCounts += CheckToUpgradeItem;
        inscriptions = player.playerComponents.inventory.synergy.inscriptions;
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
        bool hasWingedSpear = false;

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
                    hasWingedSpear = true;
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
            if (!hasWingedSpear)
            {
                break;
            }
            try
            {
                var item = inventory.items[i];

                if (item == null)
                {
                    continue;
                }

                if (item.name.Equals("SwordOfSun"))
                {
                    ChangeItem("Custom-WingedSpear_2", item, itemRef, false);
                    inventory.items[positionofOriginialItem].RemoveOnInventory();
                    break;
                }

                if (item.name.Equals("RingOfMoon"))
                {
                    ChangeItem("Custom-WingedSpear_3", item, itemRef, false);
                    inventory.items[positionofOriginialItem].RemoveOnInventory();
                    break;
                }

                if (item.name.Equals("Custom-WingedSpear") && positionofOriginialItem != i)
                {
                    ChangeItem("Custom-WingedSpear_4", item, itemRef, false);
                    inventory.items[positionofOriginialItem].RemoveOnInventory();
                    break;
                }

                if (item.name.Equals("ShardOfDarkness"))
                {
                    ChangeItem("Custom-WingedSpear_5", item, itemRef, true);
                    inventory.items[positionofOriginialItem].RemoveOnInventory();
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[VoiceOfCommunity - Winged Spear] There is no item at item index " + i);
                Debug.LogWarning(e.Message);
            }
        }
        if (hasWingedSpear && SunAndMoonInscriptionCount() >= 2)
        {
            ChangeItem("Custom-WingedSpear_4", inventory.items[positionofOriginialItem], itemRef, false);
        }
    }

    private void ChangeItem(string name, Item item, ItemReference itemRef, bool isOmen)
    {
        if (GearResource.instance.TryGetItemReferenceByName(name, out itemRef))
        {
            ItemRequest request = itemRef.LoadAsync();
            request.WaitForCompletion();

            if (item.state == Gear.State.Equipped)
            {
                Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                if (isOmen)
                {
                    newItem._gearTag = Gear.Tag.Omen;
                }
                item.ChangeOnInventory(newItem);
            }
        }
    }
}
