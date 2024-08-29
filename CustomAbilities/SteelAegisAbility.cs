using System;
using Characters;
using Characters.Abilities;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SteelAegisAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SteelAegisAbility>
    {
        int _stacks;

        private LevelManager _levelManager;

        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks == 0 ? null : ability._defaultIcon;

        public Instance(Character owner, SteelAegisAbility ability) : base(owner, ability)
        {
            _levelManager = Singleton<Service>.Instance.levelManager;
        }

        public override void OnAttach()
        {
            owner.health.onTakeDamage.Add(1000, new TakeDamageDelegate(OnTakeDamage));
            _levelManager.onMapLoadedAndFadedIn += ResetStack;
            _stacks = 2;
        }

        public override void OnDetach()
        {
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
            _levelManager.onMapLoadedAndFadedIn -= ResetStack;
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            if (owner.invulnerable.value) return false;
            if (damage.attackType.Equals(Damage.AttackType.None)) return false;
            if (damage.@null) return false;
            if (damage.amount < 1.0) return false;
            if (_stacks <= 0) return false;
            _stacks--;
            damage.@null = true;
            return false;
        }

        private void ResetStack()
        {
            _stacks = 2;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new SteelAegisAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}