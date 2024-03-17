using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class UnstableSizePotionAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<UnstableSizePotionAbility>
    {
        private Stat.Values _shrinkingStats;
        private Stat.Values _growingStats;
        private float _bonusTimeRemaining;
        private bool isShrinking = false;

        public override float iconFillAmount => 1.0f - _bonusTimeRemaining / ability._timeout;

        public Instance(Character owner, UnstableSizePotionAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _shrinkingStats = ability._shrinkingStat.Clone();
            _growingStats = ability._growingStat.Clone();
            owner.stat.AttachValues(_shrinkingStats);
            owner.stat.AttachValues(_growingStats);
            RefreshStats();
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_shrinkingStats);
            owner.stat.DetachValues(_growingStats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            _bonusTimeRemaining -= deltaTime;

            if (_bonusTimeRemaining < 0f)
            {
                RefreshStats();
            }
        }

        private void RefreshStats()
        {
            if (isShrinking)
            {
                for (int i = 0; i < _shrinkingStats.values.Length; i++)
                {
                    _shrinkingStats.values[i].value = ability._shrinkingStat.values[i].GetMultipliedValue(0);
                }
                for (int i = 0; i < _growingStats.values.Length; i++)
                {
                    _growingStats.values[i].value = ability._growingStat.values[i].value;
                }
                isShrinking = false;
            } else
            {
                for (int i = 0; i < _shrinkingStats.values.Length; i++)
                {
                    _shrinkingStats.values[i].value = ability._shrinkingStat.values[i].value;
                }
                for (int i = 0; i < _growingStats.values.Length; i++)
                {
                    _growingStats.values[i].value = ability._growingStat.values[i].GetMultipliedValue(0);
                }
                isShrinking = true;
            }
            _bonusTimeRemaining = ability._timeout;
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal float _timeout = 1;

    [SerializeField]
    internal Stat.Values _shrinkingStat;

    [SerializeField]
    internal Stat.Values _growingStat;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new UnstableSizePotionAbility()
        {
            _timeout = _timeout,
            _shrinkingStat = _shrinkingStat.Clone(),
            _growingStat = _growingStat.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}