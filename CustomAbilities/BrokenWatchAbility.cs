using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BrokenWatchAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BrokenWatchAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.3),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.3),
            new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, -0.2)
        ]);

        private float _timeout = 5;
        private float _timeRemaining;

        private int _stacks;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks == 0 ? null : ability._defaultIcon;

        public Instance(Character owner, BrokenWatchAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.onStartMotion += OnStartMotion;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onStartMotion -= OnStartMotion;
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

        private void OnStartMotion(Characters.Actions.Motion motion, float runSpeed)
        {
            if (motion.action.type != Characters.Actions.Action.Type.Skill) return;
            if (owner.stat._values[4, 22] > 0.2) _stacks++;
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
        return new BrokenWatchAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}