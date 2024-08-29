using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CraftsmansChiselAbility : Ability, ICloneable
{
    public CraftsmansChiselAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<CraftsmansChiselAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.99)
        ]);

        private Stat.Values _statsWhenMaxed;
        private Stat.Values _statWhenMaxed = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4)
        ]);

        private Stat.Values _statsWhenActive;
        private Stat.Values _statWhenActive = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.6)
        ]);

        private float _timeout = 8;
        private float _timeRemaining;

        private bool _isActive;

        private bool IsMaxed()
        {
            return ability.component.currentCount == (ability._isEvolved ? 25 : 15);
        }

        public override float iconFillAmount => _isActive ? 1.0f - _timeRemaining / _timeout : IsMaxed() ? 0 : 1;
        public override int iconStacks => ability.component.currentCount;

        public Instance(Character owner, CraftsmansChiselAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            _statsWhenMaxed = _statWhenMaxed.Clone();
            _statsWhenActive = _statWhenActive.Clone();

            owner.stat.AttachValues(_stats);
            owner.stat.AttachValues(_statsWhenMaxed);
            owner.stat.AttachValues(_statsWhenActive);

            RefreshStats();

            owner.health.onTakeDamage.Add(0, OnTakeDamage);
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.stat.DetachValues(_statsWhenMaxed);
            owner.stat.DetachValues(_statsWhenActive);

            owner.health.onTakeDamage.Remove(OnTakeDamage);
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
                _isActive = false;
                RefreshStats();
            }
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            if (Southpaw.Random.NextInt(0, 99) < 35) ability.component.currentCount = Math.Min(ability.component.currentCount + 1, ability._isEvolved ? 25 : 15);
            if (IsMaxed() && ability._isEvolved)
            {
                _isActive = true;
                _timeRemaining = _timeout;
            }
            RefreshStats();
            return false;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(ability.component.currentCount);
            }
            for (int i = 0; i < _statsWhenMaxed.values.Length; i++)
            {
                _statsWhenMaxed.values[i].value = _statWhenMaxed.values[i].GetStackedValue(IsMaxed() ? 1 : 0);
            }
            for (int i = 0; i < _statsWhenActive.values.Length; i++)
            {
                _statsWhenActive.values[i].value = _statWhenActive.values[i].GetStackedValue(_isActive ? 1 : 0);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal bool _isEvolved;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CraftsmansChiselAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved
        };
    }
}