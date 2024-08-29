using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class GingaPachinkoAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<GingaPachinkoAbility>
    {
        private bool _isActive;

        private float _timeout = 8.0f;
        private float _timeRemaining = 1.0f;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        public Instance(Character owner, GingaPachinkoAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.health.onTakeDamage.Add(int.MinValue, OnTakeDamage);
            owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.health.onTakeDamage.Remove(OnTakeDamage);
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
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

            _isActive = true;
            _timeRemaining = _timeout;

            return false;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (damage.@null)
            {
                return false;
            }
            if (damage.amount < 1.0)
            {
                return false;
            }
            if (damage.motionType != Damage.MotionType.Basic && damage.motionType != Damage.MotionType.Skill && damage.motionType != Damage.MotionType.Swap)
            {
                return false;
            }
            if (!_isActive)
            {
                return false;
            }

            if (new System.Random().Next(100) <= ability._woundChance) target.character.status.ApplyWound(owner);

            return false;
        }
    }

    [SerializeField]
    internal int _woundChance = 0;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new GingaPachinkoAbility()
        {
            _defaultIcon = _defaultIcon,
            _woundChance = _woundChance
        };
    }
}
