using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ManatechSequenceBreakerAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ManatechSequenceBreakerAbility>
    {
        private float _currentCooldown;
        private float _maxCooldown = 5;

        private bool _isReady = true;

        public override float iconFillAmount => _isReady ? 0 : 1.0f - _currentCooldown / _maxCooldown;

        public Instance(Character owner, ManatechSequenceBreakerAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (!_isReady)
            {
                _currentCooldown += deltaTime;
            }

            if (_currentCooldown > _maxCooldown)
            {
                _currentCooldown = 0;
                _isReady = true;
            }
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (_isReady)
            {
                target.character.chronometer.animation.AttachTimeScale(this, 0.5f, 2.5f);
                _isReady = false;
            }
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new ManatechSequenceBreakerAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}