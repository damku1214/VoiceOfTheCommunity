using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BloodSoakedJavelinAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BloodSoakedJavelinAbility>
    {
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
            if (target == null || target.character == null)
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
            Random random = new Random();
            int randomNumber = random.Next(0, 5);
            if (randomNumber != 0)
            {
                return;
            }
            target.character.status.ApplyWound(owner);
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new BloodSoakedJavelinAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}