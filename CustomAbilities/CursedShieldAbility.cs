using System;
using Characters;
using Characters.Abilities;
using Services;
using Singletons;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CursedShieldAbility : Ability, ICloneable
{
    public CursedShieldAbilityComponent component {  get; set; }

    public class Instance : AbilityInstance<CursedShieldAbility>
    {
        private Stat.Values _stats;
        private Stat.Values _stat = new(
        [
            new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.75)
        ]);

        private float _maxCooldown = 1.5f;
        private float _currentCooldown;

        private int _maxStack = 300;
        private bool _isMaxStacks;

        private Characters.Shield.Instance _shieldInstance;


        public override float iconFillAmount => _isMaxStacks || owner.health.currentHealth <= 1 ? 0 : 1.0f - _currentCooldown / _maxCooldown;
        public override int iconStacks => (int)ability.component.savedShieldAmount;

        public Instance(Character owner, CursedShieldAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = _stat.Clone();
            owner.stat.AttachValues(_stats);
            HealthOnChanged();

            _shieldInstance = owner.health.shield.Add(ability, ability.component.savedShieldAmount);
            owner.health.onTookDamage += OnTookDamage;
            owner.health.onChanged += HealthOnChanged;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);

            if (owner.health.shield.Remove(ability)) _shieldInstance = null;
            owner.health.onTookDamage -= OnTookDamage;
            owner.health.onChanged -= HealthOnChanged;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            _shieldInstance.amount = ability.component.savedShieldAmount;
            if (_isMaxStacks || owner.health.currentHealth <= 1) { return; }
            _currentCooldown += deltaTime;

            if (_currentCooldown > _maxCooldown)
            {
                _currentCooldown = 0;

                owner.health.TakeHealth(Math.Min(3, owner.health.currentHealth - 1));
                Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(3, MMMaths.RandomPointWithinBounds(owner.collider.bounds));

                ability.component.savedShieldAmount = (float)Math.Min(ability.component.savedShieldAmount + 3, _maxStack);
                _shieldInstance.amount = ability.component.savedShieldAmount;

                if (ability.component.savedShieldAmount == _maxStack) { _isMaxStacks = true; }
            }
        }

        private void HealthOnChanged()
        {
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = _stat.values[i].GetStackedValue(owner.health.shield.amount > 0 ? 1 : 0);
            }
            owner.stat.SetNeedUpdate();
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
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CursedShieldAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}