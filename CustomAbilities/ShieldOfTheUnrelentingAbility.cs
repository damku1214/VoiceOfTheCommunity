using System;
using Characters;
using Characters.Abilities;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ShieldOfTheUnrelentingAbility : Ability, ICloneable
{
    public ShieldOfTheUnrelentingAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<ShieldOfTheUnrelentingAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _statPerStack = new Stat.Values([new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.05)]);

        private float _maxCooldown = 5;
        private float _currentCooldown;

        private int _maxStack = 40;

        private bool _isMaxStacks;

        private Characters.Shield.Instance _shieldInstance;

        public override float iconFillAmount => _isMaxStacks ? 0 : 1.0f - _currentCooldown / _maxCooldown;
        public override int iconStacks => Math.Min((int) owner.health.shield.amount / 5 * 5, 40);

        public Instance(Character owner, ShieldOfTheUnrelentingAbility ability) : base(owner, ability)
        {
        }

        public override void Refresh()
        {
            base.Refresh();
            _shieldInstance.amount = ability.component.savedShieldAmount;
            RefreshStats();
        }

        public override void OnAttach()
        {
            _stats = _statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats();
            _shieldInstance = owner.health.shield.Add(ability, ability.component.savedShieldAmount);
            owner.health.onChanged += RefreshStats;
            owner.health.onTookDamage += OnTookDamage;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            if (owner.health.shield.Remove(ability))
            {
                _shieldInstance = null;
            }
            owner.health.onChanged -= RefreshStats;
            owner.health.onTookDamage -= OnTookDamage;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            _shieldInstance.amount = ability.component.savedShieldAmount;
            if (_isMaxStacks) { return; }
            _currentCooldown += deltaTime;

            if (_currentCooldown > _maxCooldown)
            {
                _currentCooldown = 0;
                ability.component.savedShieldAmount = (float) Math.Min(ability.component.savedShieldAmount + 5, _maxStack);
                _shieldInstance.amount = ability.component.savedShieldAmount;
                if (ability.component.savedShieldAmount == _maxStack) { _isMaxStacks = true; }
            }
        }

        private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
        {
            if (owner.invulnerable.value)
            {
                return;
            }
            if (tookDamage.attackType.Equals(Damage.AttackType.None))
            {
                return;
            }
            if (tookDamage.@null)
            {
                return;
            }
            if (tookDamage.amount < 1.0)
            {
                return;
            }
            ability.component.savedShieldAmount = (float)_shieldInstance.amount;
            if (ability.component.savedShieldAmount != _maxStack) { _isMaxStacks = false; }
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _statPerStack.values[i].GetStackedValue(Math.Min((int) owner.health.shield.amount / 5, 8));
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
        return new ShieldOfTheUnrelentingAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}