using Characters;
using Characters.Abilities;
using System;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BrutalWhipAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BrutalWhipAbility>
    {
        public override int iconStacks => Math.Min((int)(20 + (100 - owner.health.currentHealth / owner.health.maximumHealth * 100) / 7 * 8), 100);

        public Instance(Character owner, BrutalWhipAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onStartMotion += OnStartMotion;
        }

        public override void OnDetach()
        {
            owner.onStartMotion -= OnStartMotion;
        }

        private void OnStartMotion(Characters.Actions.Motion motion, float runSpeed)
        {
            if (motion.action.type != Characters.Actions.Action.Type.Skill) return;

            Random random = new();
            if (random.Next(100) <= (int)(20 + (100 - owner.health.currentHealth / owner.health.maximumHealth * 100) / 7 * 8))
            {
                Damage _damage = new(owner, 1, MMMaths.RandomPointWithinBounds(owner.collider.bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item);
                
                owner.Attack(owner, ref _damage);
            }
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new BrutalWhipAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
