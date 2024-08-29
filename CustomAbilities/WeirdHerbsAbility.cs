using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class WeirdHerbsAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<WeirdHerbsAbility>
    {
        private Stat.Values _stats;
        private float _timeRemaining;
        public override float iconFillAmount => 1.0f - _timeRemaining / ability._timeout;

        private bool _isActive = false;

        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        public Instance(Character owner, WeirdHerbsAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _isActive = false;
            _stats = ability._stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
        }

        public override void OnDetach()
        {
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
            owner.stat.DetachValues(_stats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (!_isActive)
            {
                return;
            }

            _timeRemaining -= deltaTime;

            if (_timeRemaining < 0f)
            {
                _isActive = false;
                RefreshStats();
            }
        }

        private void OnSwap()
        {
            _isActive = true;
            _timeRemaining = ability._timeout;
            RefreshStats();
        }

        private void RefreshStats()
        {
            int statMultiplier = _isActive ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._stat.values[i].GetMultipliedValue(statMultiplier);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal Stat.Values _stat;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new WeirdHerbsAbility()
        {
            _timeout = _timeout,
            _stat = _stat.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}