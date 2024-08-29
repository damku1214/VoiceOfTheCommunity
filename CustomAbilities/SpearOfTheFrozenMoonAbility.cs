using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class SpearOfTheFrozenMoonAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<SpearOfTheFrozenMoonAbility>
    {
        private EnumArray<Inscription.Key, Inscription> inscriptions;

        public override Sprite icon
        {
            get
            {
                return null;
            }
        }

        public int AbsoluteZeroInscriptionCount()
        {
            foreach (var inscription in inscriptions)
            {
                if (inscription.key == Inscription.Key.AbsoluteZero)
                {
                    return inscription.count;
                }
            }
            return 0;
        }

        public int ItemCount()
        {
            var itemInventory = owner.playerComponents.inventory.item;
            int currentCount = 0;

            for (int i = 0; i < itemInventory.items.Count; i++)
            {
                try
                {
                    var item = itemInventory.items[i];

                    if (item == null || !item.name.Equals("Custom-FrozenSpear_2"))
                    {
                        continue;
                    }

                    currentCount++;
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[VoiceOfCommunity - Spear of the Frozen Moon] There is no item at item index " + i);
                    Debug.LogWarning(e.Message);
                }
            }

            return currentCount;
        }

        public int MaximumFreezeHitCount()
        {
            if (AbsoluteZeroInscriptionCount() < 4)
            {
                return 1 + ItemCount();
            } else
            {
                return 3 + ItemCount();
            }
        }

        public Instance (Character owner, SpearOfTheFrozenMoonAbility ability) : base(owner, ability)
        {
            inscriptions = owner.playerComponents.inventory.synergy.inscriptions;
        }

        public override void OnAttach()
        {
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyDamageOnFreeze));
        }

        public override void OnDetach()
        {
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyDamageOnFreeze));
            if (AbsoluteZeroInscriptionCount() < 4)
            {
                owner.status.freezeMaxHitStack = 1;
            }
            else
            {
                owner.status.freezeMaxHitStack = 3;
            }
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);
            if (owner.status.freezeMaxHitStack != MaximumFreezeHitCount()) RefreshFreezeMaxHitCount();
        }

        private void RefreshFreezeMaxHitCount()
        {
            owner.status.freezeMaxHitStack = MaximumFreezeHitCount();
        }

        private bool AmplifyDamageOnFreeze(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null || target.character.status == null)
            {
                return false;
            }
            if (!target.character.status.IsApplying(CharacterStatus.Kind.Freeze))
            {
                return false;
            }
            damage.percentMultiplier *= 1.25;
            return false;
        }
    }

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new SpearOfTheFrozenMoonAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}