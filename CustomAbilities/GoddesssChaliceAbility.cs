using System;
using Characters;
using Characters.Abilities;
using UnityEngine;
using static Characters.Damage;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class GoddesssChaliceAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<GoddesssChaliceAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _maxStats;
        private float _bonusTimeRemaining;
        public override float iconFillAmount => 1.0f - _bonusTimeRemaining / ability._timeout;

        private MotionTypeBoolArray _attackTypes;
        private AttackTypeBoolArray _damageTypes;

        int _stacks;
        public override int iconStacks => _stacks;
        public override Sprite icon
        {
            get
            {
                if (_stacks == 0)
                {
                    return null;
                }
                return ability._defaultIcon;
            }
        }

        public Instance(Character owner, GoddesssChaliceAbility ability) : base(owner, ability)
        {
            _attackTypes = new();
            _attackTypes[MotionType.Swap] = true;
            _damageTypes = new([true, true, true, true, true]);
        }

        public override void OnAttach()
        {
            _stacks = 0;
            _stats = ability._statPerStack.Clone();
            _maxStats = ability._maxStackStats.Clone();
            owner.stat.AttachValues(_stats);
            owner.stat.AttachValues(_maxStats);
            RefreshStacks();
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifySwapDamageWhenMaxStacks));
        }

        public override void OnDetach()
        {
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifySwapDamageWhenMaxStacks));
            owner.stat.DetachValues(_stats);
            owner.stat.DetachValues(_maxStats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_stacks == 0)
            {
                return;
            }

            _bonusTimeRemaining -= deltaTime;

            if (_bonusTimeRemaining < 0f)
            {
                _stacks = 0;
                RefreshStacks();
            }
        }

        private void OnSwap()
        {
            _stacks = Math.Min(_stacks + 1, ability._maxStack);
            _bonusTimeRemaining = 5.0f;
            RefreshStacks();
        }

        private void RefreshStacks()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(_stacks);
            }
            for (int i = 0; i < _maxStats.values.Length; i++)
            {
                _maxStats.values[i].value = ability._maxStackStats.values[i].GetStackedValue(Math.Max(0, _stacks - 3));
            }
            owner.stat.SetNeedUpdate();
        }

        private bool AmplifySwapDamageWhenMaxStacks(ITarget target, ref Damage damage)
        {
            if (_stacks < ability._maxStack)
            {
                return false;
            }
            if (target == null || target.character == null)
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
            damage.percentMultiplier *= 1.1;
            return false;
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal int _maxStack = 1;

    [SerializeField]
    internal Stat.Values _statPerStack;

    [SerializeField]
    internal Stat.Values _maxStackStats;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new GoddesssChaliceAbility()
        {
            _timeout = _timeout,
            _maxStack = _maxStack,
            _statPerStack = _statPerStack.Clone(),
            _maxStackStats = _maxStackStats.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}