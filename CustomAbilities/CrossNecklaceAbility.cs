using System;
using Characters;
using Characters.Abilities;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CrossNecklaceAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CrossNecklaceAbility>
    {
        private LevelManager _levelManager;

        public override Sprite icon => null;

        public Instance(Character owner, CrossNecklaceAbility ability) : base(owner, ability)
        {
            _levelManager = Singleton<Service>.Instance.levelManager;
        }

        public override void OnAttach()
        {
            _levelManager.onMapLoadedAndFadedIn += Heal;
        }

        public override void OnDetach()
        {
            _levelManager.onMapLoadedAndFadedIn -= Heal;
        }

        private void Heal()
        {
            owner.health.Heal(5);
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CrossNecklaceAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
