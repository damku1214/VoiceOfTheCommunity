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
        private bool isActive = false;

        public override Sprite icon
        {
            get
            {
                if (isActive)
                {
                    return ability._defaultIcon;
                }
                return null;
            }
        }

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

        private bool AmplifyDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (isActive && damage.attribute.Equals(Damage.Attribute.Physical))
            {
                damage.percentMultiplier *= 1.5;
                isActive = false;
            }
            if (damage.amount >= target.character.health.maximumHealth * 0.5)
            {
                isActive = true;
                return false;
            } else if (target.character.type.Equals(Character.Type.Boss) || target.character.type.Equals(Character.Type.Adventurer))
            {
                if (damage.amount >= target.character.health.maximumHealth * 0.1)
                {
                    isActive = true;
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