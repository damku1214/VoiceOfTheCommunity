using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class FrozenSpearAbility : Ability, ICloneable
{
    public FrozenSpearAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<FrozenSpearAbility>
    {
        private CharacterStatusKindBoolArray _statusKind;

        public override int iconStacks => ability.component.currentFreezeCount;

        public Instance(Character owner, FrozenSpearAbility ability) : base(owner, ability)
        {
            _statusKind = new();
            _statusKind[CharacterStatus.Kind.Freeze] = true;
        }

        public override void OnAttach()
        {
            owner.onGaveStatus += OnGaveStatus;
        }

        public override void OnDetach()
        {
            owner.onGaveStatus -= OnGaveStatus;
        }

        private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
        {
            if (target == null)
            {
                return;
            }
            if (!_statusKind[applyInfo.kind])
            {
                return;
            }
            if (!result)
            {
                return;
            }
            if (target.type == Character.Type.Dummy)
            {
                return;
            }
            FrozenSpearAbilityComponent component = ability.component;
            component.currentFreezeCount++;
            if (component.currentFreezeCount >= 250)
            {
                UpgradeItem();
            }
        }

        private void UpgradeItem()
        {
            var inventory = owner.playerComponents.inventory.item;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                try
                {
                    var item = inventory.items[i];

                    if (item == null || !item.name.Equals("Custom-FrozenSpear"))
                    {
                        continue;
                    }

                    ItemReference itemRef;

                    if (GearResource.instance.TryGetItemReferenceByName(item.name + "_2", out itemRef))
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
                catch (Exception e)
                {
                    Debug.LogWarning("[VoiceOfCommunity - Frozen Spear] There is no item at item index " + i);
                    Debug.LogWarning(e.Message);
                }
            }
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new FrozenSpearAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
