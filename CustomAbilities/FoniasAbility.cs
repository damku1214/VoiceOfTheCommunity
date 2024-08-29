using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class FoniasAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<FoniasAbility>
    {
        public override Sprite icon
        {
            get
            {
                return null;
            }
        }

        public Instance(Character owner, FoniasAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyBossDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyBossDamage));
        }

        private bool AmplifyBossDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.type != Character.Type.Boss && target.character.type != Character.Type.Adventurer)
            {
                return false;
            }
            damage.percentMultiplier *= 1.05;
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new FoniasAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}