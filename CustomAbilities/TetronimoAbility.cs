using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;
using VoiceOfTheCommunity.CustomPatches;
using VoiceOfTheCommunity.Other;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class TetronimoAbility : Ability, ICloneable
{
    public TetronimoAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<TetronimoAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.01),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.01)
        ]);

        private float _timeRemaining;
        private float _timeout = 15;

        private bool _isActive;

        private EnumArray<Inscription.Key, Inscription> _inscriptions = Singleton<Service>.Instance._levelManager.player.playerComponents.inventory.synergy.inscriptions;
        public ItemInventory _inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

        private int TetronimoCount()
        {
            int count = 0;
            ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
            for (int i = 0; i < inventory.items.Count; i++)
            {
                var item = inventory.items[i];
                if (item == null || item.state != Characters.Gear.Gear.State.Equipped)
                {
                    continue;
                }
                if (item.name.Equals("Custom-Tetronimo") || item.name.Equals("Custom-Tetronimo_BoneUpgrade"))
                {
                    count++;
                }
            }
            return count;
        }

        private bool BoneBuffReady()
        {
            for (int i = 0; i < owner.ability._abilities.Count; i++)
            {
                if (owner.ability._abilities[i].GetType() == GetType())
                {
                    continue;
                }
                if (owner.ability._abilities[i].icon != null && owner.ability._abilities[i].icon.name.Equals("Bone"))
                {
                    return owner.ability._abilities[i].iconFillAmount == 0;
                }
            }
            return false;
        }

        public override float iconFillAmount => ability._isEvolved ? 1 - _timeRemaining / _timeout : 0;
        public override int iconStacks => ability.component.currentCount;

        public Instance(Character owner, TetronimoAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            owner.playerComponents.inventory.onUpdatedKeywordCounts += Activate;
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
            if (TetronimoStorage._stacks > ability.component.currentCount && TetronimoCount() < 2 && !ability._isEvolved)
            {
                TetronimoStorage._stacks = ability.component.currentCount;
            }
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.playerComponents.inventory.onUpdatedKeywordCounts -= Activate;
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            if (TetronimoStorage._stacks > ability.component.currentCount)
            {
                ability.component.currentCount = TetronimoStorage._stacks;
            }

            if (TetronimoStorage._stacks < ability.component.currentCount)
            {
                TetronimoStorage._stacks = ability.component.currentCount;
            }
            RefreshStats();

            if (!_isActive)
            {
                return;
            }

            _timeRemaining -= deltaTime;

            if (_timeRemaining < 0f)
            {
                _isActive = false;
            }
        }

        private void Activate()
        {
            foreach (var inscription in _inscriptions)
            {
                if (inscription.count < 4) continue;
                for (int i = 0; i < _inventory.items.Count; i++)
                {
                    var item = _inventory.items[i];
                    if (item == null || item.name.StartsWith("Custom-Tetronimo") || item.name.StartsWith("Custom-Tetronimo_BoneUpgrade")) continue;
                    if (item.keyword1 == inscription.key || item.keyword2 == inscription.key)
                    {
                        int stackAmount = 0;
                        switch (item.rarity)
                        {
                            case Rarity.Common: stackAmount = 15; break;
                            case Rarity.Rare: stackAmount = 30; break;
                            case Rarity.Unique: stackAmount = 45; break;
                            case Rarity.Legendary: stackAmount = 60; break;
                        }
                        if (item.gearTag == Characters.Gear.Gear.Tag.Omen) stackAmount = 75;
                        item.DiscardOnInventory();
                        ability.component.currentCount += stackAmount;
                        TetronimoStorage._stacks += stackAmount;
                        i--;
                    }
                }
            }
        }

        private void OnSwap()
        {
            if (!ability._isEvolved || !BoneBuffReady()) return;
            _isActive = true;
            _timeRemaining = _timeout;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(ability.component.currentCount * (_isActive ? 1.4 : 1));
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal bool _isEvolved;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new TetronimoAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved
        };
    }
}
