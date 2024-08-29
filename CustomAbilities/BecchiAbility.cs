using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BecchiAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BecchiAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.5),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.5),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.02)
        ]);

        private float _timeout = 5;
        private float _timeRemaining;

        private int _stacks;
        private int _maxStacks = 5;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks == 0 ? null : ability._defaultIcon;

        public Instance(Character owner, BecchiAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(OnTakeDamage));
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_isActive)
            {
                _timeRemaining -= deltaTime;
            }

            if (_timeRemaining < 0f)
            {
                _timeRemaining = _timeout;
                _isActive = false;
                _stacks = 0;
                RefreshStats();
            }
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            if (owner.invulnerable.value)
            {
                return false;
            }
            if (damage.attackType.Equals(Damage.AttackType.None))
            {
                return false;
            }
            if (damage.@null)
            {
                return false;
            }
            if (damage.amount < 1.0)
            {
                return false;
            }
            _stacks = Math.Min(_stacks + 1, _maxStacks);
            _timeRemaining = _timeout;
            _isActive = true;
            return false;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            damage.percentMultiplier *= (0.04 * _stacks + 1);
            return false;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(_stacks);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new BecchiAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
