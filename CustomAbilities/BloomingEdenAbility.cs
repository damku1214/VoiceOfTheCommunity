using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BloomingEdenAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BloomingEdenAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _statPerStack = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.1),
            new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.1)
        ]);

        private float _timeout = 4;
        private float _timeRemaining;

        private int _stacks;
        private int _maxStacks = 3;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks == 0 ? null : ability._defaultIcon;

        public Instance(Character owner, BloomingEdenAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(OnCrit));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(OnCrit));
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
            }

            RefreshStats();
        }

        private void OnCrit(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
        {
            if (target == null || target.character.status == null)
            {
                return;
            }
            if (gaveDamage.@null || gaveDamage.amount < 1)
            {
                return;
            }
            if (!gaveDamage.critical)
            {
                return;
            }
            _stacks = Math.Min(_stacks + 1, _maxStacks);
            _timeRemaining = _timeout;
            _isActive = true;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _statPerStack.values[i].GetStackedValue(_stacks);
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
        return new BloomingEdenAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
