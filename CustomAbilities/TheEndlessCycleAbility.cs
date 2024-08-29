using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Player;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class TheEndlessCycleAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<TheEndlessCycleAbility>
    {
        private float _timeout = 15;
        private float _timeRemaining;

        private int _stacks;

        private bool _isActive;

        public override float iconFillAmount => _isActive ? 1.0f - _timeRemaining / _timeout : _stacks == 15 ? 0 : 1;
        public override int iconStacks => _stacks;

        public Instance(Character owner, TheEndlessCycleAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onStartAction += OnSwap;
        }

        public override void OnDetach()
        {
            owner.onStartAction += OnSwap;
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
        }

        private void OnSwap(Characters.Actions.Action action)
        {
            if (action.type != Characters.Actions.Action.Type.Swap)
            {
                return;
            }
            if (Southpaw.Random.NextInt(0, 99) < (_isActive ? 40 : 20)) owner.StartCoroutine(CApply()); ;
            _stacks++;
            if (_stacks >= 16)
            {
                _isActive = true;
                _timeRemaining = _timeout;
                _stacks = 0;
            }
        }

        private IEnumerator CApply()
        {
            yield return null;
            owner.playerComponents.inventory.weapon.ResetSwapCooldown();
            yield break;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new TheEndlessCycleAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}