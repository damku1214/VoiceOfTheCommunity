using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Items;
using GameResources;
using Services;
using Singletons;
using UnityEngine;
using static Characters.Damage;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class RustyChaliceAbility : Ability, ICloneable
{
    public RustyChaliceAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<RustyChaliceAbility>
    {
        private MotionTypeBoolArray _attackTypes;
        private AttackTypeBoolArray _damageTypes;

        public override int iconStacks => ability.component.currentSwapSkillHitCount;

        public Instance(Character owner, RustyChaliceAbility ability) : base (owner, ability)
        {
            _attackTypes = new();
            _attackTypes[MotionType.Swap] = true;
            _damageTypes = new([true, true, true, true, true]);
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (damage.@null)
            {
                return false;
            }
            if (damage.amount < 1.0)
            {
                return false;
            }
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.type == Character.Type.Dummy)
            {
                return false;
            }
            if (!_attackTypes[damage.motionType])
            {
                return false;
            }
            if (!_damageTypes[damage.attackType])
            {
                return false;
            }
            RustyChaliceAbilityComponent component = ability.component;
            component.currentSwapSkillHitCount++;
            if (component.currentSwapSkillHitCount >= 150)
            {
                UpgradeItem();
            }
            return false;
        }

        private void UpgradeItem()
        {
            var inventory = owner.playerComponents.inventory.item;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                try
                {
                    var item = inventory.items[i];

                    if (item == null || !item.name.Equals("Custom-RustyChalice"))
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

                            var spawner = Singleton<Service>.Instance.floatingTextSpawner;
                            var titlePosition = new Vector3(owner.collider.bounds.center.x, owner.collider.bounds.max.y + 1.0f, 0);
                            spawner.SpawnBuff("THE CHALICE GLEAMS WITH HOLY LIGHT", titlePosition, "#FFF51E");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[VoiceOfCommunity - Rusty Chalice] There is no item at item index " + i);
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
        return new RustyChaliceAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
