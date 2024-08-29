using System;
using Characters;
using Characters.Abilities;
using Characters.Gear;
using Characters.Player;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class DiaryOfAnOldMasterAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<DiaryOfAnOldMasterAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new Stat.Values([
            new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, -0.25)
        ]);
        
        private Inventory _inventory;

        public override float iconFillAmount => CritItemCount() >= 2 ? 0 : 1;
        public override int iconStacks => CritItemCount();

        public Instance(Character owner, DiaryOfAnOldMasterAbility ability) : base(owner, ability)
        {
            _inventory = owner.playerComponents.inventory;
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            OnItemInventoryChanged();
            _inventory.item.onChanged += OnItemInventoryChanged;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            _inventory.item.onChanged -= OnItemInventoryChanged;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (damage.motionType == Damage.MotionType.Item)
            {
                damage.critical |= MMMaths.Chance(damage.criticalChance);
            }
            return false;
        }

        private void OnItemInventoryChanged()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(CritItemCount() >= 2 ? 1 : 0);
            }
            owner.stat.SetNeedUpdate();
        }

        private int CritItemCount()
        {
            int critItemCount = 0;
            for (int i = 0; i < _inventory.item.items.Count; i++)
            {
                var item = _inventory.item.items[i];
                if (item == null || item.state != Gear.State.Equipped)
                {
                    continue;
                }
                if (item.name.Equals("InvisibleKnife") || item.name.Equals("GunpowderSword")
                    || item.name.Equals("CrownOfThorns") || item.name.Equals("CrownOfThorns_2"))
                {
                    critItemCount++;
                    continue;
                }
            }
            return critItemCount;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new DiaryOfAnOldMasterAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
