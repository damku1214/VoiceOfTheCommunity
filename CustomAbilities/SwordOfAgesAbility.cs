using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SwordOfAgesAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SwordOfAgesAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1)
        ]);

        private EnumArray<Inscription.Key, Inscription> _inscriptions = Singleton<Service>.Instance._levelManager.player.playerComponents.inventory.synergy.inscriptions;

        public int ArmsInscriptionCount()
        {
            foreach (var inscription in _inscriptions)
            {
                if (inscription.key == Inscription.Key.Arms)
                {
                    return inscription.count;
                }
            }
            return 0;
        }

        public override int iconStacks => ArmsInscriptionCount();

        public Instance(Character owner, SwordOfAgesAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            owner.onGiveDamage.Add(0, OnGiveDamage);
            owner.playerComponents.inventory.onUpdatedKeywordCounts += RefreshStats;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGiveDamage.Remove(OnGiveDamage);
            owner.playerComponents.inventory.onUpdatedKeywordCounts -= RefreshStats;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            damage.percentMultiplier *= (1 + ArmsInscriptionCount() * 0.1);
            return false;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].value + ArmsInscriptionCount() * 0.1f;
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
        return new SwordOfAgesAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
