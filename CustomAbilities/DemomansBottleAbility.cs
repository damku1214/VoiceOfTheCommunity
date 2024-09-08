using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DemomansBottleAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<DemomansBottleAbility>
    {
        public override int iconStacks => (int)owner.health.shield.amount / 5 * 2;

        public Instance(Character owner, DemomansBottleAbility ability) : base(owner, ability)
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
            if (target == null || target.character == null || target.character.status == null) return false;
            if (damage.motionType == Damage.MotionType.Skill || damage.motionType == Damage.MotionType.Basic)
            {
                if (Southpaw.Random.NextInt(0, 99) < owner.health.shield.amount / 5 * 2)
                {
                    target.character.status.ApplyPoison(owner);
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
        return new DemomansBottleAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}