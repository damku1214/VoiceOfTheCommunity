using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

public class AccursedSabreAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<AccursedSabreAbility>
    {
        private CharacterStatusKindBoolArray _statusKind;
        private int _currentBleedCount;

        public override int iconStacks => _currentBleedCount;

        public Instance(Character owner, AccursedSabreAbility ability) : base(owner, ability)
        {
            _statusKind = new();
            _statusKind[CharacterStatus.Kind.Wound] = true;
        }

        public override void OnAttach()
        {
            owner.status.onApplyBleed += OnApplyBleed;
        }

        public override void OnDetach()
        {
            owner.status.onApplyBleed -= OnApplyBleed;
        }

        private void OnApplyBleed(Character attacker, Character target)
        {
            if (target == null)
            {
                return;
            }
            _currentBleedCount++;
            if (_currentBleedCount < ability._bleedCount)
            {
                return;
            }
            _currentBleedCount = 0;
            target.status.ApplyWound(owner);
            target.status.ApplyWound(owner);
            _currentBleedCount = 0;
        }
    }

    [SerializeField]
    private float _bleedCount = 3;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new AccursedSabreAbility
        {
            _bleedCount = _bleedCount,
            _defaultIcon = _defaultIcon,
        };
    }
}