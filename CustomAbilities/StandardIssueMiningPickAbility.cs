using System;
using Characters;
using Characters.Abilities;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class StandardIssueMiningPickAbility : Ability, ICloneable
{
    public class Instance : AbilityInstance<StandardIssueMiningPickAbility>
    {
        private Stat.Values _stats;

        private float _timeout = 3;
        private float _timeRemaining;

        private bool _isActive;

        public override float iconFillAmount => 1.0f - _timeRemaining / _timeout;
        public override int iconStacks => _stacks;
        public override Sprite icon
        {
            get
            {
                if (ability._isEvolved) return _isActive ? ability._defaultIcon : null;
                else return ability._defaultIcon;
            }
        }

        private int _stacks;

        public Instance(Character owner, StandardIssueMiningPickAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _stats = ability._stat.Clone();
            owner.stat.AttachValues(_stats);
            RefreshStats(false);
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            GameData.Currency.currencies[GameData.Currency.Type.Gold].onEarn += OnEarnGold;
        }

        public override void OnDetach()
        {
            owner.stat.DetachValues(_stats);
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            GameData.Currency.currencies[GameData.Currency.Type.Gold].onEarn -= OnEarnGold;
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (_isActive)
            {
                _timeRemaining -= deltaTime;
            }

            if (_timeRemaining < 0)
            {
                _isActive = false;
                RefreshStats(false);
            }

            Quintessence _quintessence = owner.playerComponents.inventory.quintessence.items[0];
            string _quintessenceName = _quintessence != null ? _quintessence.name : "";
            if (_stacks >= 1500 || _quintessenceName.Equals("Dwarf") || _quintessenceName.Equals("KingDwarf")) ChangeItem();
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.type == Character.Type.Dummy)
            {
                return false;
            }
            System.Random random = new System.Random();
            if (random.Next(10) < 3) {
                int goldAmount = random.Next(ability._minGold, ability._maxGold + 1);
                Singleton<Service>.Instance.levelManager.DropGold(goldAmount, goldAmount, owner.transform.position);
                if (!ability._isEvolved) _stacks += goldAmount;
            }
            return false;
        }

        private void OnEarnGold(int amount)
        {
            _timeRemaining = _timeout;
            _isActive = true;
            RefreshStats(true);
        }

        private void RefreshStats(bool shouldEnableStats)
        {
            int boolToInt = shouldEnableStats ? 1 : 0;
            for (int i = 0; i < _stats.values.Length; i++)
            {
                _stats.values[i].value = ability._stat.values[i].GetStackedValue(boolToInt);
            }
            owner.stat.SetNeedUpdate();
        }

        private void ChangeItem()
        {
            var inventory = owner.playerComponents.inventory.item;

            for (int i = 0; i < inventory.items.Count; i++)
            {
                var item = inventory.items[i];

                if (item == null || !item.name.Equals("Custom-StandardIssueMiningPick"))
                {
                    continue;
                }

                ItemReference itemRef;

                if (GearResource.instance.TryGetItemReferenceByName(item.name + "_2", out itemRef))
                {
                    ItemRequest request = itemRef.LoadAsync();
                    request.WaitForCompletion();

                    if (item.state == Characters.Gear.Gear.State.Equipped)
                    {
                        Item newItem = Singleton<Service>.Instance.levelManager.DropItem(request, Vector3.zero);
                        item.ChangeOnInventory(newItem);
                    }
                }
            }
        }
    }

    [SerializeField]
    internal bool _isEvolved;

    [SerializeField]
    internal int _minGold;

    [SerializeField]
    internal int _maxGold;

    [SerializeField]
    internal Stat.Values _stat;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new StandardIssueMiningPickAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved,
            _minGold = _minGold,
            _maxGold = _maxGold,
            _stat = _stat
        };
    }
}
