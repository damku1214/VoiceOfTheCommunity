using System;
using Characters;
using Characters.Abilities;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CursedHourglassAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CursedHourglassAbility>
    {
        private bool _isActive = false;
        private Stat.Values _inactiveStats;
        private LevelManager _levelManager;
        private float _bonusTimeRemaining;
        public override float iconFillAmount => 1.0f - _bonusTimeRemaining / ability._timeout;

        public override Sprite icon
        {
            get
            {
                if (!_isActive)
                {
                    return null;
                }
                return ability._defaultIcon;
            }
        }

        public Instance(Character owner, CursedHourglassAbility ability) : base(owner, ability)
        {
            _levelManager = Singleton<Service>.Instance.levelManager;
        }

        public override void OnAttach()
        {
            _inactiveStats = ability._inactiveStat.Clone();
            owner.stat.AttachValues(_inactiveStats);
            _levelManager.onMapLoadedAndFadedIn += StartBuff;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            EndBuff();
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_inactiveStats);
            _levelManager.onMapLoadedAndFadedIn -= StartBuff;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (!_isActive)
            {
                return;
            }

            _bonusTimeRemaining -= deltaTime;

            if (_bonusTimeRemaining < 0f)
            {
                _isActive = false;
                EndBuff();
            }
        }

        private void StartBuff()
        {
            _isActive = true;
            _bonusTimeRemaining = ability._timeout;
            for (int i = 0; i < _inactiveStats.values.Length; i++)
            {
                _inactiveStats.values[i].value = ability._inactiveStat.values[i].GetStackedValue(0);
            }
            owner.stat.SetNeedUpdate();
        }

        private void EndBuff()
        {
            _isActive = false;
            for (int i = 0; i < _inactiveStats.values.Length; i++)
            {
                _inactiveStats.values[i].value = ability._inactiveStat.values[i].value;
            }
            owner.stat.SetNeedUpdate();
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.type == Character.Type.Boss && target.character.health.currentHealth == target.character.health.maximumHealth)
            {
                StartBuff();
            }
            if (!_isActive)
            {
                return false;
            }
            damage.percentMultiplier *= 1.3;
            return false;
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal Stat.Values _inactiveStat;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CursedHourglassAbility()
        {
            _timeout = _timeout,
            _inactiveStat = _inactiveStat,
            _defaultIcon = _defaultIcon,
        };
    }
}
