using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class FoniasAbility : Ability, ICloneable
{
    public FoniasAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<FoniasAbility>
    {
        public override int iconStacks => ability.component.currentCount;

        public Instance(Character owner, FoniasAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyBossDamage));
            owner.onKilled += OnKill;
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyBossDamage));
            owner.onKilled -= OnKill;
        }

        private bool AmplifyBossDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null) return false;
            if (target.character.type != Character.Type.Boss) return false;
            damage.percentMultiplier *= 1.15 + (0.025 * ability.component.currentCount);
            return false;
        }

        private void OnKill(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null) return;
            if (target.character.type != Character.Type.Boss) return;
            ability.component.currentCount++;
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