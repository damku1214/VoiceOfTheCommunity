using System;
using Characters;
using Characters.Abilities;
using Services;
using Singletons;
using VoiceOfTheCommunity.CustomPatches;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ShadowThiefsSackAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ShadowThiefsSackAbility>
    {
        public override int iconStacks => ItemPurchasePatch._stacks;

        public Instance(Character owner, ShadowThiefsSackAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += OnMapEntered;
        }

        public override void OnDetach()
        {
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= OnMapEntered;
        }

        private void OnMapEntered()
        {
            ItemPurchasePatch._stacks = 0;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new ShadowThiefsSackAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}