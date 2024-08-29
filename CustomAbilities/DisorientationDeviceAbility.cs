using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DisorientationDeviceAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<DisorientationDeviceAbility>
    {
        public override Sprite icon
        {
            get
            {
                return null;
            }
        }

        public Instance(Character owner, DisorientationDeviceAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyDamageOnStun));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyDamageOnStun));
        }

        private bool AmplifyDamageOnStun(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null || target.character.status == null)
            {
                return false;
            }
            if (!target.character.status.IsApplying(CharacterStatus.Kind.Stun))
            {
                return false;
            }
            damage.percentMultiplier *= 1.1;
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new DisorientationDeviceAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}