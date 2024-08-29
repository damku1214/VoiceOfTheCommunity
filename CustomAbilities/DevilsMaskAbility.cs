using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DevilsMaskAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<DevilsMaskAbility>
    {
        private Stat.Values _stats;

        private float remainingTime = 0;
        private float currentCooldown = 0;
        public override float iconFillAmount => isCooldown ? 1.0f - currentCooldown / ability._cooldown : 1.0f - remainingTime / ability._timeout;

        private bool isActive = false;
        private bool isCooldown = false;

        public Instance(Character owner, DevilsMaskAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            remainingTime = ability._timeout;
            _stats = ability._statWhenActive.Clone();
            owner.stat.AttachValues(_stats);
            owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (isActive)
            {
                remainingTime -= deltaTime;
            }

            if (remainingTime < 0f)
            {
                remainingTime = ability._timeout;
                isActive = false;
                isCooldown = true;
            }

            if (isCooldown)
            {
                currentCooldown += deltaTime;
            }

            if (currentCooldown > ability._cooldown)
            {
                currentCooldown = 0;
                isCooldown = false;
            }

            RefreshStats(isActive);
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
            if (isCooldown || isActive)
            {
                return false;
            }
            isActive = true;
            remainingTime = ability._timeout;

            return false;
        }

        private void RefreshStats(bool shouldEnableStats)
        {
            int boolToInt = shouldEnableStats ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statWhenActive.values[i].GetStackedValue(boolToInt);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal float _cooldown = 1;

    [SerializeField]
    internal Stat.Values _statWhenActive;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new DevilsMaskAbility()
        {
            _defaultIcon = _defaultIcon,
            _timeout = _timeout,
            _cooldown = _cooldown,
            _statWhenActive = _statWhenActive,
        };
    }
}