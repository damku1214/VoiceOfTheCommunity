using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ManaAcceleratorAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ManaAcceleratorAbility>
    {
        private EnumArray<Inscription.Key, Inscription> inscriptions;
        private Inventory _inventory;
        private Stat.Values _stats;

        public override int iconStacks => ManaCycleInscriptionCount();

        public int ManaCycleInscriptionCount()
        {
            foreach (var inscription in inscriptions)
            {
                if (inscription.key == Inscription.Key.ManaCycle)
                {
                    return inscription.count;
                }
            }
            return 0;
        }

        public Instance(Character owner, ManaAcceleratorAbility ability) : base(owner, ability)
        {
            _inventory = owner.playerComponents.inventory;
            inscriptions = owner.playerComponents.inventory.synergy.inscriptions;
        }

        public override void OnAttach()
        {
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshManaCycleInscriptionCount();
            _inventory.onUpdatedKeywordCounts += RefreshManaCycleInscriptionCount;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            _inventory.onUpdatedKeywordCounts -= RefreshManaCycleInscriptionCount;
        }

        private void RefreshManaCycleInscriptionCount()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(ManaCycleInscriptionCount());
            }
            owner.stat.SetNeedUpdate();
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
        return new ManaAcceleratorAbility
        {
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}