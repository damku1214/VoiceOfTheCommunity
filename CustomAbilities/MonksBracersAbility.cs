using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Actions;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class MonksBracersAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<MonksBracersAbility>
    {
        private float _timeRemaining;
        private float _timeout = 3;

        private bool _isActive;

        private int _stacks;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;
        public override int iconStacks => _stacks;

        public Instance(Character owner, MonksBracersAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            owner.playerComponents.inventory.weapon.onChanged += OnChangeWeapon;
            EnableTurboAttack(ability._isEvolved ? 0 : 1);
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            owner.playerComponents.inventory.weapon.onChanged -= OnChangeWeapon;
            EnableTurboAttack(1);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            EnableTurboAttack(ability._isEvolved ? 0 : 1);

            if (_isActive)
            {
                _timeRemaining -= deltaTime;
            }

            if (_timeRemaining < 0)
            {
                _isActive = false;
                _stacks = 0;
            }
        }

        private void OnChangeWeapon(Weapon old, Weapon @new)
        {
            EnableTurboAttack(ability._isEvolved ? 0 : 1);
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null) return false;
            if (damage.motionType == Damage.MotionType.Basic)
            {
                damage.percentMultiplier *= 1 + 0.02 * _stacks;
                _stacks = Math.Min(_stacks + 1, ability._isEvolved ? 10 : 5);
                _timeRemaining = _timeout;
                _isActive = true;
            }
            return false;
        }

        private void EnableTurboAttack(int isEnabling)
        {
            List<Characters.Actions.Action.Type> attackActions = new() { Characters.Actions.Action.Type.BasicAttack, Characters.Actions.Action.Type.JumpAttack };
            List<Characters.Actions.Action> actions = Singleton<Service>.Instance.levelManager.player.actions.Where(x => attackActions.Contains(x.type)).ToList();

            foreach (Characters.Actions.Action action in actions)
                action._inputMethod = (Characters.Actions.Action.InputMethod)isEnabling;
        }
    }

    [SerializeField]
    internal bool _isEvolved;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new MonksBracersAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved
        };
    }
}