using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class HappiestMaskAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<HappiestMaskAbility>
    {
        public override Sprite icon => null;

        public Instance(Character owner, HappiestMaskAbility ability) : base(owner, ability)
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
            if (target == null || target.character == null || damage.attacker.character == null)
            {
                return false;
            }
            if (damage.attacker.character.type == Character.Type.PlayerMinion)
            {
                damage.percentMultiplier += 0.3;
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
        return new HappiestMaskAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
