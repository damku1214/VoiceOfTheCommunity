using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CorruptedSymbolAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CorruptedSymbolAbility>
    {
        private Stat.Values _stats;
        private EnumArray<Inscription.Key, Inscription> _inscriptions;
        private Inventory _inventory;

        public override int iconStacks => SpoilsInscriptionCount();

        public int SpoilsInscriptionCount()
        {
            foreach (var inscription in _inscriptions)
            {
                if (inscription.key == Inscription.Key.Spoils)
                {
                    return inscription.count;
                }
            }
            return 0;
        }

        public Instance(Character owner, CorruptedSymbolAbility ability) : base(owner, ability)
        {
            _inventory = owner.playerComponents.inventory;
            _inscriptions = owner.playerComponents.inventory.synergy.inscriptions;
        }

        public override void OnAttach()
        {
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshSpoilsInscriptionCount();
            _inventory.onUpdatedKeywordCounts += RefreshSpoilsInscriptionCount;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            _inventory.onUpdatedKeywordCounts -= RefreshSpoilsInscriptionCount;
        }

        private void RefreshSpoilsInscriptionCount()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(SpoilsInscriptionCount());
            }
        }
    }

    [SerializeField]
    internal Stat.Values _statPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CorruptedSymbolAbility()
        {
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}