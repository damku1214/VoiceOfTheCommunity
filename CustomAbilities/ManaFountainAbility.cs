using System;
using Characters;
using Characters.Abilities;
using Characters.Player;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class ManaFountainAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<ManaFountainAbility>
    {
        public override Sprite icon => null;

        public Instance(Character owner, ManaFountainAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (damage.motionType == Damage.MotionType.Skill)
            {
                WeaponInventory weapon = owner.playerComponents.inventory.weapon;
                Reduce(weapon.current.actionsByType[Characters.Actions.Action.Type.Skill]);
                if (weapon.next != null)
                {
                    Reduce(weapon.next.actionsByType[Characters.Actions.Action.Type.Skill]);
                }
            }
            return false;
        }

        private void Reduce(Characters.Actions.Action[] actions)
        {
            foreach (Characters.Actions.Action action in actions)
            {
                if (action.type == Characters.Actions.Action.Type.Skill)
                {
                    if (action.cooldown.time == null)
                    {
                        return;
                    }
                    action.cooldown.time.ReduceCooldown(action.cooldown.time.cooldownTime * 0.02f);
                }
            }
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new ManaFountainAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
