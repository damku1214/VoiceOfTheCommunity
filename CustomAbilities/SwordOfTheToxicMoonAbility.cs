using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SwordOfTheToxicMoonAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SwordOfTheToxicMoonAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.05)
        ]);

        private CharacterStatusKindBoolArray _statusKind;

        private float _timeout = 10;
        private float _timeRemaining;

        private int _stacks;
        private int _maxStacks = 8;

        private bool _isActive;
        
        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks == 0 ? null : ability._defaultIcon;

        public Instance(Character owner, SwordOfTheToxicMoonAbility ability) : base(owner, ability)
        {
            _statusKind = new();
            _statusKind[CharacterStatus.Kind.Poison] = true;
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.onGaveStatus += OnGaveStatus;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGaveStatus -= OnGaveStatus;
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
            _stacks = Math.Min(_stacks + 1, _maxStacks);
            _timeRemaining = _timeout;
            _isActive = true;
            RefreshStats();
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
        return new SwordOfTheToxicMoonAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}