using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SpikyRapidaAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SpikyRapidaAbility>
    {
        private int _currentBasicAttackCount;
        private bool _canApplyBleed = false;

        public override int iconStacks => _currentBasicAttackCount;

        public Instance(Character owner, SpikyRapidaAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onStartAction += OnStartAction;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.onStartAction -= OnStartAction;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        private void OnStartAction(Characters.Actions.Action action)
        {
            _canApplyBleed = false;
            if (action.type != Characters.Actions.Action.Type.BasicAttack && action.type != Characters.Actions.Action.Type.JumpAttack)
            {
                return;
            }
            _currentBasicAttackCount++;
            if (_currentBasicAttackCount < ability._basicAttackCount)
            {
                return;
            }
            _canApplyBleed = true;
            _currentBasicAttackCount = 0;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (damage.motionType != Damage.MotionType.Basic)
            {
                return false;
            }
            if (!_canApplyBleed)
            {
                return false;
            }
            target.character.status.ApplyWound(owner);
            return false;
        }
    }

    [SerializeField]
    private float _basicAttackCount = 3f;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new SpikyRapidaAbility
        {
            _basicAttackCount = _basicAttackCount,
            _defaultIcon = _defaultIcon,
        };
    }
}
