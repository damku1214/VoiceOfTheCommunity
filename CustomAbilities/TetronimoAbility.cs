using System;
using Characters;
using Characters.Abilities;
using Characters.Abilities.Triggers;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

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
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.15),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.15)
        ]);

        private float _timeRemaining;
        private float _timeout = 15;

        private bool _isActive;

        private EnumArray<Inscription.Key, Inscription> _inscriptions = Singleton<Service>.Instance._levelManager.player.playerComponents.inventory.synergy.inscriptions;
        public ItemInventory _inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;

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
            RefreshStats();
            owner.playerComponents.inventory.onUpdatedKeywordCounts += Activate;
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
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

            if (!_isActive)
            {
                return;
            }

            _timeRemaining -= deltaTime;

            if (_timeRemaining < 0f)
            {
                _isActive = false;
                RefreshStats();
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
                    if (item == null || item.name.StartsWith("Custom-Tetronimo")) continue;
                    if (item.keyword1 == inscription.key || item.keyword2 == inscription.key)
                    {
                        item.DiscardOnInventory();
                        ability.component.currentCount++;
                        RefreshStats();
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
            RefreshStats();
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(ability.component.currentCount * 2 + (_isActive ? ability.component.currentCount : 0));
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
