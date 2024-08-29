using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class PowerHalberdAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<PowerHalberdAbility>
    {
        private float _maxCooldown = 1.5f;
        private float _currentCooldown;

        private bool _isReady = true;

        public override float iconFillAmount => _isReady ? 0 : 1.0f - _currentCooldown / _maxCooldown;

        public Instance(Character owner, PowerHalberdAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, OnGiveDamage);
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(OnGiveDamage);
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
            if (damage.key == "Arms")
            {
                damage.percentMultiplier += 0.35;
                if (_isReady)
                {
                    owner.health.Heal(2);
                    _isReady = false;
                }
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
        return new PowerHalberdAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
