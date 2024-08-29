using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class HorcruxPendantAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<HorcruxPendantAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _activeStats = new(
        [
            new(Stat.Category.Percent, Stat.Kind.Health, 1.1),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.1),
            new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.1),
            new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.1),
            new(Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.ChargingSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.SkillCooldownSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.SwapCooldownSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.EssenceCooldownSpeed, 1.1),
            new(Stat.Category.Percent, Stat.Kind.CriticalChance, 1.1),
            new(Stat.Category.Percent, Stat.Kind.CriticalDamage, 1.1)
        ]);

        private float _timeout = 10;
        private float _timeRemaining = 10;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override Sprite icon => hasMirage() || _isActive ? ability._defaultIcon : null;

        private bool hasMirage()
        {
            for (int i = 0; i < owner.ability._abilities.Count; i++)
            {
                if (owner.ability._abilities[i].GetType() == GetType())
                {
                    continue;
                }
                if (owner.ability._abilities[i].icon != null && owner.ability._abilities[i].icon.name.Equals("Heirloom"))
                {
                    if (owner.ability._abilities[i].iconFillAmount == 0)
                    {
                        _timeRemaining = _timeout;
                        _isActive = false;
                        RefreshStats(true);
                        return true;
                    }
                    else
                    {
                        if (_timeRemaining < 0)
                        {
                            RefreshStats(false);
                            return false;
                        }
                        if (!_isActive)
                        {
                            _timeRemaining = _timeout;
                            _isActive = true;
                            RefreshStats(true);
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        public Instance(Character owner, HorcruxPendantAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _activeStats.Clone();
            owner.stat.AttachValues(_stats);
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_isActive)
            {
                _timeRemaining -= deltaTime;
            }

            if (_timeRemaining < 0)
            {
                _isActive = false;
                RefreshStats(false);
            }
        }

        private void RefreshStats(bool shouldEnableStats)
        {
            int boolToInt = shouldEnableStats ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _activeStats.values[i].GetMultipliedValue(boolToInt);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new HorcruxPendantAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
