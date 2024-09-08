using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class RustyShovelAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<RustyShovelAbility>
    {
        private bool _isActive = false;

        private float _timeout = 1;
        private float _timeRemaining;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        public Instance(Character owner, RustyShovelAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyDamage));
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

        private bool AmplifyDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (_isActive && damage.attribute.Equals(Damage.Attribute.Physical))
            {
                damage.percentMultiplier *= 1.5;
            }
            if (damage.amount >= target.character.health.maximumHealth * 0.5)
            {
                _isActive = true;
                return false;
            } else if (target.character.type.Equals(Character.Type.Boss) || target.character.type.Equals(Character.Type.Adventurer))
            {
                if (damage.amount >= target.character.health.maximumHealth * 0.1)
                {
                    _isActive = true;
                    return false;
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
        return new RustyShovelAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}