using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class RingTargetAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<RingTargetAbility>
    {
        public override Sprite icon => null;

        public Instance(Character owner, RingTargetAbility ability) : base(owner, ability)
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

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (damage.attackType == Damage.AttackType.Projectile)
            {
                damage.percentMultiplier *= 1.3;
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
        return new RingTargetAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
