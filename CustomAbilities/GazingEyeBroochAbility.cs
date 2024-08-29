using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class GazingEyeBroochAbility : Ability, ICloneable
{ 
    public class Instance : AbilityInstance<GazingEyeBroochAbility>
    {
        public override Sprite icon => null;

        private bool hasDuelEffect(Character character)
        {
            for (int i = 0; i < character.ability._abilities.Count; i++)
            {
                if (character.ability._abilities[i] == null)
                {
                    continue;
                }
                if (character.ability._abilities[i].GetType() == typeof(Duel.StatBonus))
                {
                    return true;
                }
            }
            return false;
        }

        public Instance(Character owner, GazingEyeBroochAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyDamage));
            owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyDamage));
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
        }

        private bool AmplifyDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (hasDuelEffect(target.character))
            {
                damage.percentMultiplier *= 1.1;
            }
            return false;
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
            if (hasDuelEffect(damage.attacker.character))
            {
                damage.percentMultiplier *= 0.9;
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
        return new GazingEyeBroochAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}