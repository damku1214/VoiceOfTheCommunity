using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class Ability_Sample : Ability, ICloneable
{
    public class Instance : AbilityInstance<Ability_Sample>
    {
        public Instance(Character owner, Ability_Sample ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
        }

        public override void OnDetach()
        {
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new Ability_Sample()
        {
            _defaultIcon = _defaultIcon
        };
    }
}