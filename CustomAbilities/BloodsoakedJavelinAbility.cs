using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BloodSoakedJavelinAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BloodSoakedJavelinAbility>
    {
        private bool _isReady = true;
        private float _cooldownRemaining;
        public override float iconFillAmount => _isReady ? 0 : _cooldownRemaining / ability._timeout;

        public Instance(Character owner, BloodSoakedJavelinAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(ApplyBleedOnCrit));
        }

        public override void OnDetach()
        {
            owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(ApplyBleedOnCrit));
        }

        private void ApplyBleedOnCrit(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
        {
            if (!_isReady)
            {
                return;
            }
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
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 20);
            if (randomNumber != 0)
            {
                return;
            }
            target.character.status.ApplyWound(owner);
            _cooldownRemaining = ability._timeout;
            _isReady = false;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            _cooldownRemaining -= deltaTime;

            if (_cooldownRemaining < 0f)
            {
                _isReady = true;
            }
        }
    }

    [SerializeField]
    internal float _timeout = 0.5f;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new BloodSoakedJavelinAbility()
        {
            _timeout = _timeout,
            _defaultIcon = _defaultIcon,
        };
    }
}