using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class AttendantsCuirassAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<AttendantsCuirassAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _statPerStack = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, 0.45),
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.45)
        ]);

        private float _timeout = 12;
        private float _timeRemaining;

        private bool _isActive;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;

        public Instance(Character owner, AttendantsCuirassAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
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
            }

            RefreshStats();
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
            _timeRemaining = _timeout;
            _isActive = true;
            return false;
        }

        private void RefreshStats()
        {
            int isActiveToInt = _isActive ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _statPerStack.values[i].GetStackedValue(isActiveToInt);
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
        return new AttendantsCuirassAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}