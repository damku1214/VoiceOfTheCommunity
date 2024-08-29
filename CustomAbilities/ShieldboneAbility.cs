using System;
using Characters;
using Characters.Abilities;
using Characters.Abilities.Triggers;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ShieldboneAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ShieldboneAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.7)
        ]);

        private float _timeRemaining;
        private float _timeout = 15;

        private bool _isActive;

        public override float iconFillAmount => ability._isUpgraded ? 1 - _timeRemaining / _timeout : 0;
        public override int iconStacks => Math.Min((int)owner.health.shield.amount, ability._maxStacks);

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

        public Instance(Character owner, ShieldboneAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.health.onChanged += RefreshStats;
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onChanged -= RefreshStats;
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

        private void OnSwap()
        {
            if (!ability._isUpgraded || !BoneBuffReady()) return;
            _isActive = true;
            _timeRemaining = _timeout;
            RefreshStats();
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                if (_stat.values[i].kindIndex == 4)
                {
                    _stats.values[i].value = _stat.values[i].GetMultipliedValue(ability._isUpgraded && _isActive ? 1 : 0);
                    continue;
                }
                _stats.values[i].value = _stat.values[i].value + (Math.Min((int)owner.health.shield.amount, ability._maxStacks) * 0.01);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal bool _isUpgraded;

    [SerializeField]
    internal int _maxStacks;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new ShieldboneAbility()
        {
            _defaultIcon = _defaultIcon,
            _isUpgraded = _isUpgraded,
            _maxStacks = _maxStacks
        };
    }
}
