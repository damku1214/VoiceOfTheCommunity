using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CursedDaggersAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CursedDaggersAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.01),
        ]);

        private double _exceedingCritChance;
        private double _currentMultiplier;

        public override int iconStacks => (int)Math.Round(_currentMultiplier);

        public Instance(Character owner, CursedDaggersAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats(owner.stat._values);
            owner.stat._onUpdated.Add(int.MinValue, RefreshStats);
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.stat._onUpdated.Remove(RefreshStats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
        }

        private double[] RefreshStats(double[,] values)
        {
            _exceedingCritChance = (values[3, 9] - 0.4) * 100;

            if (_exceedingCritChance == 0) return [];
            for (int i = 0; i < _stats.values.Length; i++)
            {
                if (_exceedingCritChance > 0)
                {
                    values[4, 9] = 1.4;
                    _stats.values[i].value = _stat.values[i].GetStackedValue(_exceedingCritChance);
                    _currentMultiplier = _exceedingCritChance;
                }
                else
                {
                    _stats.values[i].value = _stat.values[i].GetStackedValue(0);
                    _currentMultiplier = 0;
                }
            }
            owner.stat.SetNeedUpdate();
            return [];
        }
    }
    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CursedDaggersAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
