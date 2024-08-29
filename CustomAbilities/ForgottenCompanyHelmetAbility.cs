using System;
using Characters;
using Characters.Abilities;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ForgottenCompanyHelmetAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ForgottenCompanyHelmetAbility>
    {
        public override Sprite icon => null;

        public Instance(Character owner, ForgottenCompanyHelmetAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.AddOrUpdate(this, 0.25);
            GameData.Currency.currencies[GameData.Currency.Type.Gold].onConsume += OnConsumeGold;
        }

        public override void OnDetach()
        {
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.Remove(this);
            GameData.Currency.currencies[GameData.Currency.Type.Gold].onConsume -= OnConsumeGold;
        }

        private void OnConsumeGold(int amount)
        {
            Singleton<Service>.Instance.levelManager.DropGold(amount / 10, amount / 30, owner.transform.position);
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new ForgottenCompanyHelmetAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}