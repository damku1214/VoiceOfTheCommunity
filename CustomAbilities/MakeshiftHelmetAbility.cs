using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class MakeshiftHelmetAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<MakeshiftHelmetAbility>
    {
        public override Sprite icon => null;

        public Instance(Character owner, MakeshiftHelmetAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(OnTakeDamage));
        }

        public override void OnDetach()
        {
            owner.health.onTakeDamage.Remove(new TakeDamageDelegate(OnTakeDamage));
        }

        private bool OnTakeDamage(ref Damage damage)
        {
            Vector2 ptr = MMMaths.Vector3ToVector2(damage.attacker.transform.position);
            Vector2 vector = MMMaths.Vector3ToVector2(owner.transform.position);
            float num = Mathf.Abs(ptr.x - vector.x);
            if (num >= 4) damage.percentMultiplier *= 0.5;
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new MakeshiftHelmetAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}