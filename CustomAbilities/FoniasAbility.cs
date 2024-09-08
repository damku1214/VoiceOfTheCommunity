using System;
using Characters;
using Characters.Abilities;
using Data;

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
            owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(OnCharacterKilled));
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyBossDamage));
        }

        public override void OnDetach()
        {
            owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(OnCharacterKilled));
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyBossDamage));
        }

        private void OnCharacterKilled(ITarget target, ref Damage damage)
        {
            Character character = target.character;
            if (character == null) return;
            if (character.type != Character.Type.Boss) return;
            if (target.character.key == Key.FirstHero1 || target.character.key == Key.FirstHero2 || target.character.key == Key.Alexander1 || target.character.key == Key.Alexander2_Left || target.character.key == Key.Alexander2_Right || target.character.key == Key.Alexander2_Heart || target.character.key == Key.Alexander3_Bomb || target.character.key == Key.DarkSkul1 || target.character.key == Key.Unspecified)
                return;
            ability.component.currentCount++;
        }

        private bool AmplifyBossDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null) return false;
            if (target.character.type != Character.Type.Boss) return false;
            damage.percentMultiplier *= 1.15 + (0.025 * ability.component.currentCount);
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