using System;
using Characters;
using Characters.Abilities;
using Characters.Gear;
using Characters.Gear.Items;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DreamCatcherAbility : Ability, ICloneable
{
    public DreamCatcherAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<DreamCatcherAbility>
    {
        private Stat.Values _stats;

        public override int iconStacks
        {
            get
            {
                return ability.component.currentItemBreakCount;
            }
        }

        public Instance(Character owner, DreamCatcherAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyMagicDamageToLowHP));

            Singleton<Service>.Instance.gearManager.onItemInstanceChanged += HandleItemInstanceChanged;
            HandleItemInstanceChanged();
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            UpdateStack();
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyMagicDamageToLowHP));

            Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= HandleItemInstanceChanged;
            foreach (Item item in Singleton<Service>.Instance.gearManager._itemInstances)
            {
                item.onDiscard -= TryStackUp;
            }
            owner.stat.DetachValues(_stats);
        }

        private bool AmplifyMagicDamageToLowHP(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (damage.attribute != Damage.Attribute.Magic)
            {
                return false;
            }
            if (target.character.health.currentHealth > target.character.health.maximumHealth * 0.4)
            {
                return false;
            }
            damage.percentMultiplier *= 1.25;
            return false;
        }

        private void HandleItemInstanceChanged()
        {
            foreach (Item item in Singleton<Service>.Instance.gearManager._itemInstances)
            {              
                if (item.rarity == Rarity.Legendary || item.gearTag == Gear.Tag.Omen)
                {
                    item.onDiscard -= TryStackUp;
                    item.onDiscard += TryStackUp;
                }
            }
        }

        public void TryStackUp(Gear gear)
        {
            if (gear.destructible)
            {
                DreamCatcherAbilityComponent component = ability.component;
                component.currentItemBreakCount ++;
                UpdateStack();
            }
        }

        public void UpdateStack()
        {
            DreamCatcherAbilityComponent component = ability.component;
            int stack = component.currentItemBreakCount;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(stack);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal Stat.Values _statPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new DreamCatcherAbility()
        {
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}
