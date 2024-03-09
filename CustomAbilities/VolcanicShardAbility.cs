using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using static Characters.Damage;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class VolcanicShardAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<VolcanicShardAbility>
    {
        private EnumArray<Inscription.Key, Inscription> inscriptions;
        private Inventory _inventory;

        private MotionTypeBoolArray _attackTypes;
        private AttackTypeBoolArray _damageTypes;

        public override int iconStacks
        {
            get
            {
                return BurnInscriptionCount();
            }
        }

        public int BurnInscriptionCount()
        {
            foreach (var inscription in inscriptions)
            {
                if (inscription == null)
                {
                    continue;
                }
                if (inscription.name == "Arson" || inscription.name == "방화")
                {
                    return inscription.count;
                }
            }
            return 0;
        }

        public Instance(Character owner, VolcanicShardAbility ability) : base(owner, ability)
        {
            _inventory = owner.playerComponents.inventory;
            inscriptions = owner.playerComponents.inventory.synergy.inscriptions;
            _attackTypes = new();
            _attackTypes[MotionType.Status] = true;
            _damageTypes = new([true, true, true, true, true]);
        }

        public override void OnAttach()
        {
            RefreshArsonInscriptionCount();
            _inventory.onUpdatedKeywordCounts += RefreshArsonInscriptionCount;
            _inventory.upgrade.onChanged += RefreshArsonInscriptionCount;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyBurnDamage));
        }

        public override void OnDetach()
        {
            owner.status.durationMultiplier[CharacterStatus.Kind.Burn].Remove(this);
            _inventory.onUpdatedKeywordCounts -= RefreshArsonInscriptionCount;
            _inventory.upgrade.onChanged -= RefreshArsonInscriptionCount;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyBurnDamage));
        }

        private void RefreshArsonInscriptionCount()
        {
            owner.status.durationMultiplier[CharacterStatus.Kind.Burn].AddOrUpdate(this, BurnInscriptionCount() * 0.2f);
        }

        private bool AmplifyBurnDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null || target.character.status == null)
            {
                return false;
            }
            if (!_attackTypes[damage.motionType])
            {
                return false;
            }
            if (!_damageTypes[damage.attackType])
            {
                return false;
            }
            if (target.character.status == null || !target.character.status.IsApplying(CharacterStatus.Kind.Burn))
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
        return new VolcanicShardAbility()
        {
            _defaultIcon = _defaultIcon,
        };
    }
}
