using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class VaseOfTheFallenAbility : Ability, ICloneable
{
    public VaseOfTheFallenAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<VaseOfTheFallenAbility>
    {
        private Stat.Values _stats;

        private float _currentRevengeCooldown;
        private float _revengeTimeRemaining = 1.0f;
        public override float iconFillAmount => _isCooldown ? 1.0f - _currentRevengeCooldown / ability._revengeCooldown : 1.0f - _revengeTimeRemaining / ability._revengeTimeout;

        public bool _canRevenge = false;
        public bool _isCooldown = false;
        int _lostStacks;

        public override int iconStacks => ability.component.currentKillCount;

        public override Sprite icon
        {
            get
            {
                VaseOfTheFallenAbilityComponent component = ability.component;
                int currentKillCount = component.currentKillCount;
                if (currentKillCount == 0)
                {
                    return null;
                }
                return ability._defaultIcon;
            }
        }

        public Instance(Character owner, VaseOfTheFallenAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = ability._statPerStack.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStacks();
            owner.onKilled += OnKilledEnemy;
            owner.health.onTakeDamage.Add(-1000, new TakeDamageDelegate(OnTakeDamage));

        }

        public override void OnDetach()
        {
            owner.onKilled -= OnKilledEnemy;
            owner.stat.DetachValues(_stats);
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
        }

        private void OnKilledEnemy(ITarget target, ref Damage damage)
        {
            if (target.character.type == Character.Type.Dummy)
            {
                return;
            }
            VaseOfTheFallenAbilityComponent component = ability.component;
            int currentKillCount = component.currentKillCount;
            component.currentKillCount = Math.Min(currentKillCount + 1, ability._maxStack);
            RefreshStacks();
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            if (owner.invulnerable.value)
            {
                return false;
            }
            if (damage.attackType.Equals(Damage.AttackType.None))
            {
                return false;
            }
            if (damage.@null)
            {
                return false;
            }
            if (damage.amount < 1.0)
            {
                return false;
            }
            
            VaseOfTheFallenAbilityComponent component = ability.component;
            int currentKillCount = component.currentKillCount;

            _lostStacks = currentKillCount / 2;
            component.currentKillCount /= 2;
            RefreshStacks();

            _canRevenge = true;
            _revengeTimeRemaining = ability._revengeTimeout;
            owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(OnRevenge));

            return false;
        }

        private bool OnRevenge(ITarget target, ref Damage damage)
        {
            if (damage.@null)
            {
                return false;
            }
            if (damage.amount < 1.0)
            {
                return false;
            }
            _canRevenge = false;
            _revengeTimeRemaining = ability._revengeTimeout;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnRevenge));
            if (_isCooldown)
            {
                return false;
            }
            VaseOfTheFallenAbilityComponent component = ability.component;
            component.currentKillCount += _lostStacks / 2;
            _isCooldown = true;
            _currentRevengeCooldown = 0;
            return false;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_canRevenge)
            {
                _revengeTimeRemaining -= deltaTime;
            }

            if (_revengeTimeRemaining < 0f) {
                _revengeTimeRemaining = ability._revengeTimeout;
                _canRevenge = false;
                owner.onGiveDamage.Remove(new GiveDamageDelegate(OnRevenge));
            }

            if (_isCooldown)
            {
                _currentRevengeCooldown += deltaTime;
            }

            if (_currentRevengeCooldown > ability._revengeCooldown)
            {
                _currentRevengeCooldown = 0;
                _isCooldown = false;
            }
        }

        private void RefreshStacks()
        {
            VaseOfTheFallenAbilityComponent component = ability.component;
            int currentKillCount = component.currentKillCount;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statPerStack.values[i].GetStackedValue(currentKillCount);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal float _revengeTimeout = 1;

    [SerializeField]
    internal float _revengeCooldown = 1;

    [SerializeField]
    internal int _maxStack = 1;

    [SerializeField]
    internal Stat.Values _statPerStack;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new VaseOfTheFallenAbility()
        {
            _revengeTimeout = _revengeTimeout,
            _revengeCooldown = _revengeCooldown,
            _maxStack = _maxStack,
            _statPerStack = _statPerStack.Clone(),
            _defaultIcon = _defaultIcon,
        };
    }
}
