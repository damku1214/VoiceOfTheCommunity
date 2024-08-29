using System;
using Characters;
using Characters.Abilities;
using Characters.Player;
using Data;
using Services;
using Singletons;
using VoiceOfTheCommunity.CustomPatches;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DamoclesAbility : Ability, ICloneable
{
    public DamoclesAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<DamoclesAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1),
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.2)
        ]);

        private int DamoclesCount()
        {
            int count = 0;
            ItemInventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
            for (int i = 0; i < inventory.items.Count; i++)
            {
                var item = inventory.items[i];
                if (item == null)
                {
                    continue;
                }
                if (item.state == Characters.Gear.Gear.State.Equipped && item.name.Equals("Custom-Damocles"))
                {
                    count++;
                }
            }
            return count;
        }

        public override int iconStacks => ability.component.currentCount;

        public Instance(Character owner, DamoclesAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.AddOrUpdate(this, -0.3);
            if (ItemDestroyPatch._stacks > ability.component.currentCount && DamoclesCount() < 2)
            {
                ItemDestroyPatch._stacks = ability.component.currentCount;
            }
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            GameData.Currency.currencies[GameData.Currency.Type.Gold].multiplier.Remove(this);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (ItemDestroyPatch._stacks > ability.component.currentCount)
            {
                ability.component.currentCount = ItemDestroyPatch._stacks;
            }

            if (ItemDestroyPatch._stacks < ability.component.currentCount)
            {
                ItemDestroyPatch._stacks = ability.component.currentCount;
            }

            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(Math.Floor((double)ability.component.currentCount / 20));
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
        return new DamoclesAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}