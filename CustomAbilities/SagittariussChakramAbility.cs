using System;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SagittariussChakramAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SagittariussChakramAbility>
    {
        public override Sprite icon => null;

        private List<Character> _enemies = [];

        public Instance(Character owner, SagittariussChakramAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += OnMapLoadedAndFadedIn;
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= OnMapLoadedAndFadedIn;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            foreach (var enemy in _enemies) RefreshChrono(enemy);
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.health.currentHealth > target.character.health.maximumHealth * 0.7)
            {
                return false;
            }
            if (target.character.health.currentHealth <= target.character.health.maximumHealth * 0.25) damage.percentMultiplier *= 1.2;
            else damage.percentMultiplier *= 1.15;
            return false;
        }

        private void OnMapLoadedAndFadedIn()
        {
            _enemies.Clear();
            foreach (var enemy in Map.Instance.waveContainer.GetAllEnemies())
            {
                _enemies.Add(enemy);
            }
        }

        private void RefreshChrono(Character character)
        {
            character.chronometer.animation.DetachTimeScale(this);
            int healthStatusToInt = 0;
            if (character.health.currentHealth <= character.health.maximumHealth * 0.25)
            {
                healthStatusToInt = 2;
            } else if (character.health.currentHealth <= character.health.maximumHealth * 0.7)
            {
                healthStatusToInt = 1;
            }
            if (healthStatusToInt != 0)
            {
                character.chronometer.animation.AttachTimeScale(this, 0.95f - (0.05f * healthStatusToInt));
            }
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new SagittariussChakramAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}