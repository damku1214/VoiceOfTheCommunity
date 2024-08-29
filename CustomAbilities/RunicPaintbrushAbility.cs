using System;
using Characters;
using Characters.Abilities;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

[Serializable]
public class RunicPaintbrushAbility : Ability, ICloneable
{
    public RunicPaintbrushAbilityComponent component { get; set; }

    public class Instance : AbilityInstance<RunicPaintbrushAbility>
    {
        private Stat.Values _atkStats;
        private Stat.Values _atkStatPerStack = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.08),
            new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.08)
        ]);

        private Stat.Values _spdStats;
        private Stat.Values _spdStatPerStack = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.08),
            new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.08),
            new(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.08)
        ]);

        private Stat.Values _cdStats;
        private Stat.Values _cdStatPerStack = new(
        [
            new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.05),
            new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.1)
        ]);

        private float _timeRemaining;
        private float _timeout = 15;

        private bool _isActive;

        public override float iconFillAmount => ability._isEvolved ? 1 - _timeRemaining / _timeout : 0;
        public override Sprite icon => _isActive ? ability._defaultIcon : null;

        private bool boneBuffReady()
        {
            for (int i = 0; i < owner.ability._abilities.Count; i++)
            {
                if (owner.ability._abilities[i].GetType() == GetType())
                {
                    continue;
                }
                if (owner.ability._abilities[i].icon != null && owner.ability._abilities[i].icon.name.Equals("Bone"))
                {
                    return owner.ability._abilities[i].iconFillAmount == 0;
                }
            }
            return false;
        }

        public Instance(Character owner, RunicPaintbrushAbility ability) : base(owner, ability)
        {
        }

        public override void OnAttach()
        {
            _atkStats = _atkStatPerStack.Clone();
            _spdStats = _spdStatPerStack.Clone();
            _cdStats = _cdStatPerStack.Clone();

            owner.stat.AttachValues(_atkStats);
            owner.stat.AttachValues(_spdStats);
            owner.stat.AttachValues(_cdStats);

            RefreshStats();

            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += OnMapLoadedAndFadedIn;
            owner.onGiveDamage.Add(0, new GiveDamageDelegate(OnGiveDamage));
            owner.playerComponents.inventory.weapon.onSwap += OnSwap;
        }

        public override void OnDetach()
        {
            Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= OnMapLoadedAndFadedIn;
            owner.onGiveDamage.Remove(new GiveDamageDelegate(OnGiveDamage));
            owner.playerComponents.inventory.weapon.onSwap -= OnSwap;

            owner.stat.DetachValues(_atkStats);
            owner.stat.DetachValues(_spdStats);
            owner.stat.DetachValues(_cdStats);
        }

        public override void UpdateTime(float deltaTime)
        {
            base.UpdateTime(deltaTime);

            if (!_isActive)
            {
                return;
            }

            _timeRemaining -= deltaTime;

            if (_timeRemaining < 0f)
            {
                _isActive = false;
                RefreshStats();
            }
        }

        private void OnMapLoadedAndFadedIn()
        {
            foreach (var enemy in Map.Instance.waveContainer.GetAllEnemies())
            {
                if (enemy.type == Character.Type.Boss)
                {
                    Activate(ability._isEvolved, ability._isEvolved);
                    RefreshStats();
                    return;
                }
            }

            if (!ability._isEvolved) Activate(false, false);
            RefreshStats();
        }

        private bool OnGiveDamage(ITarget target, ref Damage damage)
        {
            if (target == null || target.character == null)
            {
                return false;
            }
            if (target.character.type == Character.Type.Boss && target.character.health.currentHealth == target.character.health.maximumHealth)
            {
                Activate(ability._isEvolved, false);
                RefreshStats();
            }
            return false;
        }

        private void OnSwap()
        {
            if (!ability._isEvolved || !boneBuffReady()) return;
            _isActive = true;
            _timeRemaining = _timeout;
            RefreshStats();
        }

        private void Activate(bool isBoss, bool isEntering)
        {
            var spawner = Singleton<Service>.Instance.floatingTextSpawner;
            var titlePosition = new Vector3(owner.collider.bounds.center.x, owner.collider.bounds.max.y + 1.0f, 0);

            for (int i = 0; i < (isEntering ? 2 : 1); i++) {
                if (i > 0) titlePosition.y++;
                switch (Southpaw.Random.NextInt(0, isBoss ? 2 : 3))
                {
                    case 0:
                        ability.component.currentAtkBuffCount++;
                        if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("모필이 불타는 전장을 그려냅니다", titlePosition, "#FF0000");
                        else spawner.SpawnBuff("The brush paints a fiery battleground", titlePosition, "#FF0000");
                        break;
                    case 1:
                        ability.component.currentSpdBuffCount++;
                        if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("모필이 재빠른 매를 그려냅니다", titlePosition, "#72FFDE");
                        else spawner.SpawnBuff("The brush paints a swift hawk", titlePosition, "#72FFDE");
                        break;
                    case 2:
                        ability.component.currentCdBuffCount++;
                        if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("모필이 윤나는 기계를 그려냅니다", titlePosition, "#E48006");
                        else spawner.SpawnBuff("The brush paints a changing machine", titlePosition, "#E48006");
                        break;
                    case 3:
                        owner.health.Heal(10);
                        if (new CustomItemReference().lang == "kr") spawner.SpawnBuff("모필이 고요한 초원을 그려냡니다", titlePosition, "#FF0000");
                        else spawner.SpawnBuff("The brush paints a quiet plateau", titlePosition, "#81FF65");
                        break;
                }
            }
        }

        private void RefreshStats()
        {
            for (int i = 0; i < _atkStats.values.Length; i++)
            {
                _atkStats.values[i].value = _atkStatPerStack.values[i].GetStackedValue(ability.component.currentAtkBuffCount * (_isActive ? 2 : 1));
            }
            for (int i = 0; i < _spdStats.values.Length; i++)
            {
                _spdStats.values[i].value = _spdStatPerStack.values[i].GetStackedValue(ability.component.currentSpdBuffCount * (_isActive ? 2 : 1));
            }
            for (int i = 0; i < _cdStats.values.Length; i++)
            {
                _cdStats.values[i].value = _cdStatPerStack.values[i].GetStackedValue(ability.component.currentCdBuffCount * (_isActive ? 2 : 1));
            }
            owner.stat.SetNeedUpdate();
        }
    }

    [SerializeField]
    internal bool _isEvolved;

    public override IAbilityInstance CreateInstance(Character owner)
    {
        return new Instance(owner, this);
    }

    public object Clone()
    {
        return new RunicPaintbrushAbility()
        {
            _defaultIcon = _defaultIcon,
            _isEvolved = _isEvolved
        };
    }
}