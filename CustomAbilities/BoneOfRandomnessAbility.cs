using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BoneOfRandomnessAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BoneOfRandomnessAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.Percent, Stat.Kind.Health, 1),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.ChargingSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.SkillCooldownSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.SwapCooldownSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.EssenceCooldownSpeed, 1),
            new(Stat.Category.Percent, Stat.Kind.CriticalChance, 1),
            new(Stat.Category.Percent, Stat.Kind.CriticalDamage, 1)
        ]);

        private float _currentCooldown;
        private float _maxCooldown = 25;

        private float _timeRemaining = 15;
        private float _timeout = 15;

        private bool _isActive;
        private bool _isCooldown;

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

        public override float iconFillAmount => _isCooldown ? 1.0f - _currentCooldown / _maxCooldown : 1.0f - _timeRemaining / _timeout;

        public Instance(Character owner, BoneOfRandomnessAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
            _isCooldown = false;
            _isActive = true;
            RefreshStats(true, Southpaw.Random.NextInt(0, 12));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_isActive)
            {
                _timeRemaining -= deltaTime;
            }

            if (_timeRemaining < 0f)
            {
                _timeRemaining = _timeout;
                _isActive = false;
                _isCooldown = true;
                RefreshStats(false, 0);
            }

            if (_isCooldown)
            {
                _currentCooldown += deltaTime;
            }

            if (_currentCooldown > _maxCooldown)
            {
                _currentCooldown = 0;
                _isCooldown = false;
                _isActive = true;
                RefreshStats(true, Southpaw.Random.NextInt(0, 12));
            }
        }

        private void OnSwap()
        {
            if (!ability._isEvolved || !BoneBuffReady()) return;
            EnhanceBuff();
        }

        private void RefreshStats(bool isActivating, int index)
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = 1;
            }
            if (isActivating) _stats.values[index].value = 1.25;
            owner.stat.SetNeedUpdate();
        }

        private void EnhanceBuff()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                if (_stats.values[i].value == 1.25)
                {
                    _stats.values[i].value = 1.35;
                    break;
                }
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
        return new BoneOfRandomnessAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved
        };
    }
}
