using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class TatteredCatPlushieAbility : Ability, ICloneable
{
    public TatteredCatPlushieAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<TatteredCatPlushieAbility>
    {
        private float _damageMultiplier = 1;
        private float _cooldownRemaining;
        public override float iconFillAmount => _cooldownRemaining / ability._timeout;

        public override int iconStacks => ability.component.currentActivateCount;

        public Instance(Character owner, TatteredCatPlushieAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _cooldownRemaining = ability._timeout;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            owner.onKilled += OnKilledEnemy;
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            owner.onKilled -= OnKilledEnemy;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            _cooldownRemaining -= deltaTime;

            if (_cooldownRemaining < 0f)
            {
                Activate();
            }
        }

        private void Activate()
        {
            owner.health.TakeHealth(owner.health.maximumHealth * 0.1);
            ability.component.currentActivateCount++;

            _damageMultiplier = 1 + ability.component.currentActivateCount * 0.05f;

            _cooldownRemaining = ability._timeout;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            damage.percentMultiplier *= _damageMultiplier;
            return false;
        }

        private void OnKilledEnemy(ITarget target, ref Damage damage)
        {
            owner.health.Heal(owner.health.maximumHealth * 0.04);
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new TatteredCatPlushieAbility()
        {
            _timeout = _timeout,
            _defaultIcon = _defaultIcon,
        };
    }
}