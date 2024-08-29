using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class TheSwordOfTheProtectorAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<TheSwordOfTheProtectorAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _activeStat = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.5),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.6),
            new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.75),
            new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.75)
        ]);


        public override float iconFillAmount => owner.health.currentHealth / owner.health.maximumHealth <= 0.5 ? 0 : 1;

        public Instance(Character owner, TheSwordOfTheProtectorAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _activeStat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.health.onChanged += RefreshStats;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.health.onChanged -= RefreshStats;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _activeStat.values[i].GetStackedValue(owner.health.currentHealth / owner.health.maximumHealth <= 0.5 ? 1 : 0);
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
        return new TheSwordOfTheProtectorAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}