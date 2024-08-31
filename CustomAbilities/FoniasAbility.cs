using System;
using Characters;
using Characters.Abilities;
using Data;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class FoniasAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<FoniasAbility>
    {
        public override int iconStacks => GameData.Progress.instance._bossKills.value;

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
            if (target == null || target.character == null) return false;
            if (target.character.type != Character.Type.Boss) return false;
            damage.percentMultiplier *= 1.15 + (0.025 * GameData.Progress.instance._bossKills.value);
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