using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class TheSecretOfTheKingAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<TheSecretOfTheKingAbility>
    {
        private float _timeout = 1;
        private float _timeRemaining;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        public Instance(Character owner, TheSecretOfTheKingAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(int.MaxValue, OnGiveDamage);
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(OnGiveDamage);
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
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
                _isActive = false;
            }
        }

        private void OnSwap()
        {
            _isActive = true;
            _timeRemaining = _timeout;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (damage.motionType == Damage.MotionType.Status)
            {
                damage.critical |= MMMaths.Chance(damage.criticalChance);
            }
            if (_isActive && Southpaw.Random.NextInt(0, 99) < 10)
            {
                switch (Southpaw.Random.NextInt(0, 4))
                {
                    case 0:
                        target.character.status.ApplyBurn(owner); break;
                    case 1:
                        target.character.status.ApplyFreeze(owner); break;
                    case 2:
                        target.character.status.ApplyPoison(owner); break;
                    case 3:
                        target.character.status.ApplyStun(owner); break;
                    case 4:
                        target.character.status.ApplyWound(owner); break;
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
        return new TheSecretOfTheKingAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}