using System;
using Characters;
using Characters.Abilities;
using Data;
using FX.SpriteEffects;
using GameResources;
using UnityEngine;
using VoiceOfTheCommunity.CustomPatches;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class BottledFaelingAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<BottledFaelingAbility>
    {
        public override Sprite icon
        {
            get
            {
                return OberonPatch.oberonExists ? ability._defaultIcon : null;
            }
        }

        public Instance(Character owner, BottledFaelingAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(ModifyDamage));
            owner.health.onDie += Revive;
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(ModifyDamage));
            owner.health.onDie -= Revive;
        }

        private bool ModifyDamage(ITarget target, ref Damage damage)
        {
            if (damage.key == "Spirit")
            {
                damage.multiplier *= 1.05;
            }
            return false;
        }

        private void Revive()
        {
            if (owner.health.currentHealth > 0.0) return;

            owner.health.onDie -= Revive;

            owner.health.PercentHeal(0.25f);
            GameData.Progress.revive++;

            CommonResource.instance.reassembleParticle.Emit(owner.transform.position, owner.collider.bounds, owner.movement.push);
            owner.CancelAction();
            owner.chronometer.master.AttachTimeScale(this, 0.01f, 0.5f);
            owner.spriteEffectStack.Add(new ColorBlend(int.MaxValue, Color.clear, 0.5f));

            GetInvulnerable getInvulnerable = new()
            {
                duration = 3
            };
            owner.spriteEffectStack.Add(new Invulnerable(0, 0.2f, getInvulnerable.duration));
            owner.ability.Add(getInvulnerable);

            RemoveItem();
            owner.ability.Remove(this);
        }

        private void RemoveItem()
        {
            var inventory = owner.playerComponents.inventory.item;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                try
                {
                    var item = inventory.items[i];

                    if (item == null || !item.name.Equals("Custom-BottledFaeling"))
                    {
                        continue;
                    }

                    if (item.state == Characters.Gear.Gear.State.Equipped)
                    {
                        item.RemoveOnInventory();
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[VoiceOfCommunity - Bottled Faeling] There is no item at item index " + i);
                    Debug.LogWarning(e.Message);
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
        return new BottledFaelingAbility()
        {
            _defaultIcon = _defaultIcon
        };
    }
}
