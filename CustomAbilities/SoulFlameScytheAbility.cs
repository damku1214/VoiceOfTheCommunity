using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SoulFlameScytheAbility : Ability, ICloneable
{
    public SoulFlameScytheAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<SoulFlameScytheAbility>
    {
        private Stat.Values _stats;
        public override int iconStacks => ability.component.currentKillCount;

        public Instance(Character owner, SoulFlameScytheAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStacks();
            owner.onKilled += OnKilledEnemy;

        }

        public override void OnDetach()
        {
            owner.onKilled -= OnKilledEnemy;
            owner.stat.DetachValues(_stats);
        }

        private void OnKilledEnemy(ITarget target, ref Damage damage)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 100);
            if (randomNumber > 45)
            {
                return;
            }
            if (target.character.type == Character.Type.Dummy)
            {
                return;
            }
            SoulFlameScytheAbilityComponent component = ability.component;
            int currentKillCount = component.currentKillCount;
            component.currentKillCount = Math.Min(currentKillCount + 1, ability._maxStack);
            RefreshStacks();
        }

        private void RefreshStacks()
        {
            SoulFlameScytheAbilityComponent component = ability.component;
            int currentKillCount = component.currentKillCount;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(currentKillCount);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal int _maxStack = 1;

    [SerializeField]
    internal Stat.Values _statPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new SoulFlameScytheAbility()
        {
            _maxStack = _maxStack,
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}
