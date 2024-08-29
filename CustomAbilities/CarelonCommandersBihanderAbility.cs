using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class CarleonCommandersBihanderAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<CarleonCommandersBihanderAbility>
    {
        private Stat.Values _stats;
        public override Sprite icon
        {
            get
            {
                if (isInFlag())
                {
                    return ability._defaultIcon;
                }
                return null;
            }
        }

        private bool isInFlag()
        {
            for (int i = 0; i < owner.ability._abilities.Count; i++)
            {
                if (owner.ability._abilities[i].GetType() == GetType())
                {
                    continue;
                }
                if (owner.ability._abilities[i].icon != null && owner.ability._abilities[i].icon.name.Equals("Empire"))
                {
                    RefreshStats(true);
                    return true;
                }
            }
            RefreshStats(false);
            return false;
        }

        public Instance(Character owner, CarleonCommandersBihanderAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = ability._statInFlag.Clone();
            owner.stat.AttachValues(_stats);
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(AmplifyDamage));
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGiveDamage.Remove(new GiveDamageDelegate(AmplifyDamage));
        }

        private bool AmplifyDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (!isInFlag())
            {
                return false;
            }
            damage.percentMultiplier *= 1.2;
            return false;
        }
        private void RefreshStats(bool shouldEnableStats)
        {
            int boolToInt = shouldEnableStats ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._statInFlag.values[i].GetStackedValue(boolToInt);
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal Stat.Values _statInFlag;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new CarleonCommandersBihanderAbility()
        {
            _defaultIcon = _defaultIcon,
            _statInFlag = _statInFlag,
        };
    }
}