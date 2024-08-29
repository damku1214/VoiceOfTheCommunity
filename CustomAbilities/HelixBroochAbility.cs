using System;
using Characters;
using Characters.Abilities;
using Characters.Operations;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class HelixBroochAbility : Ability, ICloneable
{
    public HelixBroochAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<HelixBroochAbility>
    {
        public override int iconStacks => ability.component.currentHelixCount;

        public Instance(Character owner, HelixBroochAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, OnGiveDamage);
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(OnGiveDamage);
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            HitInfo hitInfo = new(Damage.AttackType.Additional)
            {
                _attribute = Damage.Attribute.Fixed,
                _motionType = Damage.MotionType.Item,
                _key = "HelixBrooch"
            };
            Damage itemDamage = owner.stat.GetDamage(5 * ability.component.currentHelixCount, MMMaths.RandomPointWithinBounds(target.collider.bounds), hitInfo);
            if (ability.component.currentHelixCount > 0 && damage.key != "HelixBrooch") owner.Attack(target, ref itemDamage);
            if (target.character.type != Character.Type.Dummy)
            {
                if (Southpaw.Random.NextDouble(0, 1) < 0.01618) ability.component.currentHelixCount++;
            }
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new HelixBroochAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
