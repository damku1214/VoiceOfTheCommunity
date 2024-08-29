using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Items;
using Characters.Gear.Upgrades;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CrimsonCapAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CrimsonCapAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.4)
        ]);

        private int _stacks;

        private int HazardousItemCount()
        {
            int count = 0;
            string[] hazardousItemNames = ["VeiledMask", "ContaminatedCore", "Custom-CursedSword", "Custom-TaintedFinger",
                "Custom-TaintedFinger_2", "Custom-TaintedFinger_3", "Custom-ShrinkingPotion", "Custom-GrowingPotion" , "Custom-ShrinkingPotion_2",
                "Custom-CursedHourglass", "Custom-TaintedRedScarf", "Custom-TatteredCatPlushie", "Custom-Becchi", "Custom-DiaryOfAnOldMaster",
                "Custom-BrokenWatch", "Custom-Tetronimo", "Custom-Tetronimo_BoneUpgrade"];
            Inventory inventory = owner.playerComponents.inventory;
            foreach (Item item in inventory.item.items)
            {
                if (item == null || item.state != Characters.Gear.Gear.State.Equipped) continue;
                if (item.keyword1 == Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen || item.keyword2 == Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen)
                {
                    count++;
                    continue;
                }
                foreach (string name in hazardousItemNames)
                {
                    if (item.name.Equals(name)) { count++; break; }
                }
            }
            if (inventory.quintessence.items[0] != null && inventory.quintessence.items[0].name == "Orc") count++;
            if (inventory.upgrade.upgrades != null) {
                foreach (UpgradeObject darkAbility in inventory.upgrade.upgrades)
                {
                    if (darkAbility == null) continue;
                    if (darkAbility._type == UpgradeObject.Type.Cursed) count++;
                }
            }
            return count;
        }

        public override int iconStacks => _stacks;
        public override Sprite icon => _stacks > 0 ? ability._defaultIcon : null;

        public Instance(Character owner, CrimsonCapAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += OnMapLoadedAndFadedIn;
            owner.health.onTakeDamage.Add(500, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= OnMapLoadedAndFadedIn;
            owner.health.onTakeDamage.Add(500, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
        }

        private void OnMapLoadedAndFadedIn()
        {
            _stacks = HazardousItemCount();
            RefreshStats();
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            if (owner.invulnerable.value) return false;
            if (damage.attackType.Equals(Damage.AttackType.None)) return false;
            if (damage.@null) return false;
            if (damage.amount < 1.0) return false;
            if (_stacks <= 0) return false;
            _stacks--;
            damage.@null = true;
            RefreshStats();
            return false;
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(_stacks);
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
        return new CrimsonCapAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}