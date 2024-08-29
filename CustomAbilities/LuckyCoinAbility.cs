using System;
using Characters;
using Characters.Abilities;
using Data;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class LuckyCoinAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<LuckyCoinAbility>
    {
        public override Sprite icon
        {
            get
            {
                return null;
            }
        }

        public Instance(Character owner, LuckyCoinAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.AddOrUpdate(this, 0.1);
        }

        public override void OnDetach()
        {
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.Remove(this);
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new LuckyCoinAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
