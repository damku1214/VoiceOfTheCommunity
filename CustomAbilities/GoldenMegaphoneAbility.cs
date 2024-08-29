using System;
using Characters;
using Characters.Abilities;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class GoldenMegaphoneAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<GoldenMegaphoneAbility>
    {
        private int _stacks;

        public override float iconFillAmount => _stacks == 1 ? 0 : 1;

        public Instance(Character owner, GoldenMegaphoneAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += OnMapLoadedAndFadedIn;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
        }

        public override void OnDetach()
        {
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= OnMapLoadedAndFadedIn;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
        }

        private void OnMapLoadedAndFadedIn()
        {
            _stacks = 1;
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (_stacks == 1)
            {
                _stacks = 0;
                target.character.chronometer.animation.AttachTimeScale(this, 0f, 10f);
                var spawner = Singleton<Service>.Instance.floatingTextSpawner;
                var titlePosition = new Vector3(target.collider.bounds.center.x, target.collider.bounds.max.y + 1.0f, 0);
                if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("아아악 내귀!!!", titlePosition, "#E0A330");
                else spawner.SpawnBuff("NOOOOOOO MY EARS!!!", titlePosition, "#E0A330");
            }
            if (target.character.type == Character.Type.Boss && target.character.health.currentHealth == target.character.health.maximumHealth)
            {
                _stacks = 1;
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
        return new GoldenMegaphoneAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}