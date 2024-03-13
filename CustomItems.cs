using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using VoiceOfTheCommunity.CustomAbilities;
using UnityEngine.AddressableAssets;
using static Characters.Damage;
using static Characters.CharacterStatus;
using Characters.Abilities.CharacterStat;

namespace CustomItems;

public class CustomItems
{
    public static readonly List<CustomItemReference> Items = InitializeItems();

    /**
     * TODO
     * 
     */

    private static List<CustomItemReference> InitializeItems()
    {
        List<CustomItemReference> items = new();
        {
            var item = new CustomItemReference();
            item.name = "VaseOfTheFallen";
            item.rarity = Rarity.Unique;

            // EN: Vase of the Fallen
            // KR: 영혼이 담긴 도자기
            item.itemName = "Vase of the Fallen";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (Stacks up to 200% and 1/2 of total charge is lost when hit).\n
            // Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit (Cooldown: 3 seconds).

            // KR: 처치한 적의 수에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 5% 증가합니다 (최대 200% 증가, 피격시 증가치의 절반이 사라집니다).\n
            // 피격 후 1초 내로 적 공격 시 감소한 증가치의 절반을 되돌려 받습니다 (쿨타임: 3초).

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (Stacks up to 200% and 1/2 of total charge is lost when hit.)\n"
                                           + "Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit. (Cooldown: 3 seconds)";

            // EN: Souls of the Eastern Kingdom's fallen warriors shall aid you in battle.
            // KR: 장렬히 전사했던 동쪽 왕국의 병사들의 혼이 담긴 영물
            item.itemLore = "Souls of the Eastern Kingdom's fallen warriors shall aid you in battle.";

            item.prefabKeyword1 = Inscription.Key.Heritage;
            item.prefabKeyword2 = Inscription.Key.Revenge;

            VaseOfTheFallenAbility ability = new()
            {
                _revengeTimeout = 1.0f,
                _revengeCooldown = 3.0f,
                _maxStack = 40,
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.05),
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.05),
                ])
            };

            item.abilities = [
                ability
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BrokenHeart";
            item.rarity = Rarity.Unique;

            // EN: Broken Heart
            // KR: 찢어진 심장
            item.itemName = "Broken Heart";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // Increases Quintessence cooldown speed by 30%.\n
            // Amplifies Quintessence damage by 15%.\n
            // If the 'Succubus' Quintessence is in your possession, this item turns into 'Lustful Heart'.

            // KR: <color=#1787D8>마법공격력</color>이 20% 증가합니다.\n
            // 정수 쿨다운 속도가 30% 증가합니다.\n
            // 적에게 정수로 입히는 데미지가 15% 증폭됩니다.\n
            // '서큐버스' 정수 소지 시 이 아이템은 '색욕의 심장'으로 변합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                           + "Increases Quintessence cooldown speed by 30%.\n"
                                           + "Amplifies Quintessence damage by 15%.\n"
                                           + "If the Succubus Quintessence is in your possession, this item turns into 'Lustful Heart'.";

            // EN: Some poor being must have their heart torn both metaphorically and literally.
            // KR: 딱한 것, 심장이 은유적으로도 물리적으로도 찢어지다니.
            item.itemLore = "Some poor being must have their heart torn both metaphorically and literally.";

            item.prefabKeyword1 = Inscription.Key.Heritage;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(
            [
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.2),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, 0.3),
            ]);

            ModifyDamage quintDamage = new();

            quintDamage._attackTypes = new();
            quintDamage._attackTypes[Damage.MotionType.Quintessence] = true;

            quintDamage._damageTypes = new([true, true, true, true, true]);

            quintDamage._damagePercent = 1.15f;

            item.abilities = [
                quintDamage
            ];

            item.extraComponents = [
                typeof(BrokenHeartEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BrokenHeart_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Lustful Heart
            // KR: 색욕의 심장
            item.itemName = "Lustful Heart";

            // EN: Amplifies <color=#1787D8>Magic Attack</color> by 20%.\n
            // Increases Quintessence cooldown speed by 60%.\n
            // Amplifies Quintessence damage by 30%.\n

            // KR: <color=#1787D8>마법공격력</color>이 20% 증폭됩니다.\n
            // 정수 쿨다운 속도가 60% 증가합니다.\n
            // 적에게 정수로 입히는 데미지가 30% 증폭됩니다.\n

            item.itemDescription = "Amplifies <color=#1787D8>Magic Attack</color> by 20%.\n"
                                           + "Increases Quintessence cooldown speed by 60%.\n"
                                           + "Amplifies Quintessence damage by 30%.";

            // EN: Given to the greatest Incubus or Succubus directly from the demon prince of lust, Asmodeus.
            // KR: 색욕의 마신 아스모데우스로부터 가장 위대한 인큐버스 혹은 서큐버스에게 하사된 증표
            item.itemLore = "Given to the greatest Incubus or Succubus directly from the demon prince of lust, Asmodeus.";

            item.prefabKeyword1 = Inscription.Key.Heritage;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(
            [
                new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.2),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, 0.6),
            ]);

            ModifyDamage quintDamage = new();

            quintDamage._attackTypes = new();
            quintDamage._attackTypes[Damage.MotionType.Quintessence] = true;

            quintDamage._damageTypes = new([true, true, true, true, true]);

            quintDamage._damagePercent = 1.3f;

            item.abilities = [
                quintDamage
            ];

            item.forbiddenDrops = new[] { "BrokenHeart" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SmallTwig";
            item.rarity = Rarity.Legendary;

            // EN: Small Twig
            // KR: 작은 나뭇가지
            item.itemName = "Small Twig";

            // EN: Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases skill cooldown speed and skill casting speed by 30%.\n
            // Increases Crit Rate and Crit Damage by 10%.\n
            // All effects double and Amplifies damage dealt to enemies by 20% when "Skul" or "Hero Little Bone" is your current active skull.

            // KR: <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증폭됩니다.\n
            // 스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n
            // 치명타 확률 및 치명타 피해가 10% 증가합니다.\n
            // "스컬" 혹은 "용사 리틀본" 스컬을 사용 중일 시 이 아이템의 모든 스탯 증가치가 두배가 되며 적에게 입히는 데미지가 20% 증폭됩니다.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                           + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                           + "Increases Crit Rate and Crit Damage by 10%.\n"
                                           + "All effects double and Amplifies damage dealt to enemies by 20% when \"Skul\" or \"Hero Little Bone\" is your current active skull.";

            // EN: A really cool looking twig, but for some reason I feel sad...
            // KR: 정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...
            item.itemLore = "A really cool looking twig, but for some reason I feel sad...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.15),
                new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.15),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.1),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.1),
            ]);

            item.extraComponents = [
                typeof(SmallTwigEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SmallTwig_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Small Twig
            // KR: 작은 나뭇가지
            item.itemName = "Small Twig";

            // EN: Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases skill cooldown speed and skill casting speed by 30%.\n
            // Increases Crit Rate and Crit Damage by 10%.\n
            // All effects double and Amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.

            // KR: <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증폭됩니다.\n
            // 스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n
            // 치명타 확률 및 치명타 피해가 10% 증가합니다.\n
            // '스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 스탯 증가치가 두배가 되며 적에게 입히는 데미지가 20% 증폭됩니다.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                           + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                           + "Increases Crit Rate and Crit Damage by 10%.\n"
                                           + "All effects double and Amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            // EN: A really cool looking twig, but for some reason I feel sad...
            // KR: 정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...
            item.itemLore = "A really cool looking twig, but for some reason I feel sad...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.3),
                new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.3),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.2),
            ]);

            ModifyDamage amplifyDamage = new();

            amplifyDamage._attackTypes = new();
            amplifyDamage._attackTypes[Damage.MotionType.Basic] = true;

            amplifyDamage._damageTypes = new([true, true, true, true, true]);

            amplifyDamage._damagePercent = 1.2f;

            item.abilities = [
                amplifyDamage
            ];

            item.extraComponents = [
                typeof(SmallTwigRevertBehavior),
            ];

            item.forbiddenDrops = new[] { "SmallTwig" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "VolcanicShard";
            item.rarity = Rarity.Legendary;

            // EN: Volcanic Shard
            // KR: 화산의 일각
            item.itemName = "Volcanic Shard";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 100%.\n
            // Normal attacks and skills have a 20% chance to inflict Burn.\n
            // Amplifies Damage to Burning enemies by 25%.\n
            // Burn duration increases by 20% for each Arson inscription in possession.

            // KR: <color=#1787D8>마법공격력</color>이 100% 증가합니다.\n
            // 적 공격 시 20% 확률로 화상을 입힙니다.\n
            // 적에게 화상으로 입히는 데미지가 25% 증폭됩니다.\n
            // 가지고 있는 방화 각인에 비례하여 화상의 지속시간이 20%씩 증가합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 100%.\n"
                                           + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                           + "Amplifies Damage to Burning enemies by 25%.\n"
                                           + "Burn duration increases by 20% for each Arson inscription in possession.";

            // EN: Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.
            // KR: 전설의 흑요석 화산의 폭발에서 만들어졌다고 전해진, 세상에서 가장 뜨거운 칼날
            item.itemLore = "Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Arson;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.5),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.5),
            ]);

            var applyBurnWhenAttacked = new ApplyStatusOnGaveDamage();
            var status = Kind.Burn;
            applyBurnWhenAttacked._cooldownTime = 0.1f;
            applyBurnWhenAttacked._chance = 20;
            applyBurnWhenAttacked._attackTypes = new();
            applyBurnWhenAttacked._attackTypes[MotionType.Basic] = true;
            applyBurnWhenAttacked._attackTypes[MotionType.Skill] = true;

            applyBurnWhenAttacked._types = new();
            applyBurnWhenAttacked._types[AttackType.Melee] = true;
            applyBurnWhenAttacked._types[AttackType.Ranged] = true;
            applyBurnWhenAttacked._types[AttackType.Projectile] = true;

            applyBurnWhenAttacked._status = new ApplyInfo(status);

            item.abilities = [
                new VolcanicShardAbility(),
                applyBurnWhenAttacked,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RustyChalice";
            item.rarity = Rarity.Unique;

            // EN: Rusty Chalice
            // KR: 녹슨 성배
            item.itemName = "Rusty Chalice";

            // EN: Increases swap cooldown speed by 15%.\n
            // Upon hitting enemies with a swap skill 150 times, this item transforms into 'Goddess's Chalice.'

            // KR: 교대 쿨다운 속도가 15% 증가홥니다.\n
            // 적에게 교대스킬로 데미지를 150번 줄 시 해당 아이템은 '여신의 성배'로 변합니다.

            item.itemDescription = "Increases swap cooldown speed by 15%.\n"
                                           + "Upon hitting enemies with a swap skill 150 times, this item transforms into 'Goddess's Chalice.'";

            // EN: This thing? I found it at a pawn shop and it seemed interesting
            // KR: 아 이거? 암시장에서 예뻐 보이길래 샀는데, 어때?
            item.itemLore = "This thing? I found it at a pawn shop and it seemed interesting";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Mystery;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.15),
            ]);

            RustyChaliceAbility ability = new();

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RustyChalice_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Goddess's Chalice
            // KR: 여신의 성배
            item.itemName = "Goddess's Chalice";

            // EN: Increases swap cooldown speed by 40%.\n
            // Damage dealt to enemies through a swap skill is amplified by 35%.\n
            // Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n
            // At maximum stacks, swap cooldown speed is increased by 25%.

            // KR: 교대 쿨다운 속도가 40% 증가홥니다.\n
            // 적에게 교대스킬로 입히는 데미지가 35% 증폭됩니다.\n
            // 교대 시 6초 동안 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증가합니다 (최대 60%).\n
            // 공격력 증가치가 최대일 시, 교대 쿨다운 속도가 25% 증가합니다.

            item.itemDescription = "Increases swap cooldown speed by 40%.\n"
                                           + "Damage dealt to enemies through a swap skill is amplified by 35%.\n"
                                           + "Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n"
                                           + "At maximum stacks, swap cooldown speed is increased by 25%.";

            // EN: Chalice used by Leonia herself that seems to never run dry
            // KR: 여신 레오니아 본인께서 쓰시던 절대 비워지지 않는 성배
            item.itemLore = "Chalice used by Leonia herself that seems to never run dry";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Mystery;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.4),
            ]);

            ModifyDamage amplifySwapDamage = new();

            amplifySwapDamage._attackTypes = new();
            amplifySwapDamage._attackTypes[Damage.MotionType.Swap] = true;

            amplifySwapDamage._damageTypes = new([true, true, true, true, true]);

            amplifySwapDamage._damagePercent = 1.35f;

            GoddesssChaliceAbility goddesssChaliceAbility = new()
            {
                _timeout = 6.0f,
                _maxStack = 4,
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.1),
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.1),
                ]),
                _maxStackStats = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.25),
                ]),
            };

            item.abilities = [
                goddesssChaliceAbility,
                amplifySwapDamage,
            ];

            item.forbiddenDrops = new[] { "RustyChalice" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "FlaskOfBotulism";
            item.rarity = Rarity.Unique;

            item.gearTag = Characters.Gear.Gear.Tag.Omen;
            item.obtainable = false;

            // EN: Omen: Flask of Botulism
            // KR: 흉조: 역병의 플라스크
            item.itemName = "Omen: Flask of Botulism";

            // EN: The interval between poison damage ticks is further decreased.\n
            // Increases <color=#F25D1C>Physical Attack</color> by 50%.

            // KR: 중독 데미지가 발생하는 간격이 더욱 줄어듭니다.\n
            // <color=#F25D1C>물리공격력</color>이 50% 증가합니다.

            item.itemDescription = "The interval between poison damage ticks is further decreased\n"
                                           + "Increases <color=#F25D1C>Physical Attack</color> by 50%.";

            // EN: Only the mad and cruel would consider using this as a weapon.
            // KR: 정말 미치지 않고서야 이걸 무기로 쓰는 일은 없을 것이다.
            item.itemLore = "Only the mad and cruel would consider using this as a weapon.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Poisoning;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Constant, Stat.Kind.PoisonTickFrequency, 0.1),
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.5),
            ]);

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CorruptedSymbol";
            item.rarity = Rarity.Unique;

            item.gearTag = Characters.Gear.Gear.Tag.Omen;
            item.obtainable = false;

            // EN: Omen: Corrupted Symbol
            // KR: 흉조: 오염된 상징
            item.itemName = "Omen: Corrupted Symbol";

            // EN: For every Spoils inscription owned, increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 80%.

            // KR: 보유하고 있는 '칼레온' 아이템 1개당 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 80% 증가합니다.

            item.itemDescription = "For every Spoils inscription owned, increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 80%.";

            // EN: Where's your god now?
            // KR: 자, 이제 네 신은 어딨지?
            item.itemLore = "Where's your god now?";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Spoils;

            StatBonusPerGearTag statBonusPerCarleonItem = new();

            statBonusPerCarleonItem._tag = Characters.Gear.Gear.Tag.Carleon;

            statBonusPerCarleonItem._statPerGearTag = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.8),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.8),
            });

            item.abilities = [
                statBonusPerCarleonItem
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TaintedFinger";
            item.rarity = Rarity.Legendary;

            // EN: Tainted Finger
            // KR: 침식된 손가락
            item.itemName = "Tainted Finger";

            // EN: Skill damage dealt to enemies is amplified by 30%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.

            // KR: 적에게 스킬로 입히는 데미지가 30% 증폭됩니다.\n
            // <color=#1787D8>마법공격력</color>이 60% 증가합니다.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 30%.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 60%.";

            // EN: A finger from a god tainted by dark quartz
            // KR: 검은 마석에 의해 침식된 신의 손가락
            item.itemLore = "A finger from a god tainted by dark quartz";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[Damage.MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.3f;

            item.abilities = [
                amplifySkillDamage,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TaintedFinger_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Tainted Finger
            // KR: 침식된 손가락
            item.itemName = "Tainted Finger";

            // EN: Skill damage dealt to enemies is amplified by 30%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.\n
            // If the item 'Grace of Leonia' is in your possession, this item turns into 'Corrupted God's Hand'.

            // KR: 적에게 스킬로 입히는 데미지가 30% 증폭됩니다.\n
            // <color=#1787D8>마법공격력</color>이 60% 증가합니다.\n
            // 현재 '레오니아의 은총' 아이템을 소지하고 있으면 해당 아이템은 '침식된 신의 손' 으로 변합니다.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 30%.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                           + "If the item 'Grace of Leonia' is in your possession, this item turns into 'Corrupted God's Hand'.";

            // EN: Nothing happened. It seems like it needs something else.
            // KR: 아무 일도 일어나지 않았다. 뭔가 더 필요한 것 같다.
            item.itemLore = "Nothing happened. It seems like it needs something else.";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[Damage.MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.3f;

            item.abilities = [
                amplifySkillDamage,
            ];

            item.extraComponents = [
                typeof(TaintedFingerEvolveBehavior),
            ];

            item.forbiddenDrops = new[] { "TaintedFinger" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TaintedFinger_3";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Corrupted God's Hand
            // KR: 침식된 신의 손
            item.itemName = "Corrupted God's Hand";

            // EN: Skill damage dealt to enemies is amplified by 100%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 100%.\n
            // Max HP decreases by 30% for all enemies.

            // KR: 적에게 스킬로 입히는 데미지가 100% 증폭됩니다.\n
            // <color=#1787D8>마법공격력</color>이 100% 증가합니다.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 100%.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 100%.";

            // EN: A corrupt hand from Leonia's supposed god
            // KR: 레오니아로 추정되는 신의 침식된 손
            item.itemLore = "A corrupt hand from Leonia's supposed god";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[Damage.MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 2f;

            item.abilities = [
                amplifySkillDamage,
            ];

            item.forbiddenDrops = new[] { "TaintedFinger" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "DreamCatcher";
            item.rarity = Rarity.Legendary;

            // EN: Dream Catcher
            // KR: 드림캐처
            item.itemName = "Dream Catcher";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 50%.\n
            // <color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n
            // <color=#1787D8>Magic Attack</color> increases by 8% each time a Omen or a Legendary item is destroyed.

            // KR: <color=#1787D8>마법공격력</color>이 50% 증가합니다.\n
            // 현재 체력이 40% 이하인 적에게 입히는 <color=#1787D8>마법데미지</color>가 25% 증폭됩니다.\n
            // 흉조 혹은 레전더리 등급을 가진 아이템을 파괴할 때마다 <color=#1787D8>마법공격력</color>이 8% 증가합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                           + "<color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n"
                                           + "<color=#1787D8>Magic Attack</color> increases by 8% each time a Omen or a Legendary item is destroyed.";

            // EN: Acceptance is the first step towards death.
            // KR: 수용하는 것은 죽음을 향한 첫 걸음이다.
            item.itemLore = "Acceptance is the first step towards death.";

            item.prefabKeyword1 = Inscription.Key.Wisdom;
            item.prefabKeyword2 = Inscription.Key.Execution;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.5),
            ]);

            DreamCatcherAbility ability = new()
            {
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.08),
                ])
            };

            item.abilities = [
                ability
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BloodsoakedJavelin";
            item.rarity = Rarity.Rare;

            // EN: Bloodsoaked Javelin
            // KR: 피투성이 투창
            item.itemName = "Bloodsoaked Javelin";

            // EN: Increases Crit Damage by 25%.\n
            // Critical hits have a 20% chance to apply Bleed to the enemy.

            // KR: 치명타 데미지가 25% 증가합니다.\n
            // 치명타 시 20% 확률로 적에게 출혈을 부여합니다.

            item.itemDescription = "Increases Crit Damage by 25%.\n"
                                           + "Critical hits have a 20% chance to apply Bleed to the enemy.";

            // EN: A javelin that always hits vital organs, and drains all the blood out of whichever one it hits
            // KR: 적의 심장을 정확히 노려 시체에 피 한방울 남기지 않는 투창
            item.itemLore = "A javelin that always hits vital organs, and drains all the blood out of whichever one it hits";

            item.prefabKeyword1 = Inscription.Key.Misfortune;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.25),
            ]);

            BloodsoakedJavelinAbility ability = new();

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "FrozenSpear";
            item.rarity = Rarity.Rare;

            // EN: Frozen Spear
            // KR: 얼음의 창
            item.itemName = "Frozen Spear";

            // EN: Skills have a 2% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // After applying freeze 200 times, this item turns into 'Spear of the Frozen Moon'.

            // KR: 적에게 스킬로 공격시 2% 확률로 빙결을 부여합니다.\n
            // <color=#1787D8>마법공격력</color>가 20% 증가합니다.\n
            // 적에게 빙결을 200번 부여할 시 해당 아이템은 '얼어붙은 달의 창'으로 변합니다.

            item.itemDescription = "Skills have a 2% chance to inflict Freeze.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                           + "After applying freeze 200 times, this item turns into 'Spear of the Frozen Moon'.";

            // EN: A sealed weapon waiting the cold time to revealed it's true form.
            // KR: 해방의 혹한을 기다리는 봉인된 무기
            item.itemLore = "A sealed weapon waiting the cold time to revealed it's true form.";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.2),
            ]);

            var applyFreezeWhenSkillAttacked = new ApplyStatusOnGaveDamage();
            var status = Kind.Freeze;
            applyFreezeWhenSkillAttacked._cooldownTime = 0.1f;
            applyFreezeWhenSkillAttacked._chance = 2;
            applyFreezeWhenSkillAttacked._attackTypes = new();
            applyFreezeWhenSkillAttacked._attackTypes[MotionType.Skill] = true;

            applyFreezeWhenSkillAttacked._types = new();
            applyFreezeWhenSkillAttacked._types[AttackType.Melee] = true;
            applyFreezeWhenSkillAttacked._types[AttackType.Ranged] = true;
            applyFreezeWhenSkillAttacked._types[AttackType.Projectile] = true;

            applyFreezeWhenSkillAttacked._status = new ApplyInfo(status);

            FrozenSpearAbility ability = new();

            item.abilities = [
                ability,
                applyFreezeWhenSkillAttacked,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "FrozenSpear_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Spear of the Frozen Moon
            // KR: 얼어붙은 달의 창
            item.itemName = "Spear of the Frozen Moon";

            // EN: Skills have a 5% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 50%.\n
            // Attacking frozen enemies increases the number of hits to remove Freeze by 3.\n
            // Amplifies damage to frozen enemies by 50%.

            // KR: 적에게 스킬로 공격시 5% 확률로 빙결을 부여합니다.\n
            // <color=#1787D8>마법공격력</color>가 50% 증가합니다.\n
            // 빙결 상태의 적 공격 시 빙결이 해제되는데 필요한 타수가 3 증가합니다.\n
            // 빙결 상태의 적에게 입히는 데미지가 50% 증가합니다.

            item.itemDescription = "Skills have a 5% chance to inflict Freeze.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                           + "Attacking frozen enemies increases the number of hits to remove Freeze by 3.\n"
                                           + "Amplifies damage dealt to frozen enemies by 50%.";

            // EN: When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. 
            // KR: 전장에 눈보라가 휘몰아칠 때, 얼음 괴수 발펜의 창이 나타날지니
            item.itemLore = "When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. ";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.5),
            ]);

            var applyFreezeWhenSkillAttacked = new ApplyStatusOnGaveDamage();
            var status = Kind.Freeze;
            applyFreezeWhenSkillAttacked._cooldownTime = 0.1f;
            applyFreezeWhenSkillAttacked._chance = 5;
            applyFreezeWhenSkillAttacked._attackTypes = new();
            applyFreezeWhenSkillAttacked._attackTypes[MotionType.Skill] = true;

            applyFreezeWhenSkillAttacked._types = new();
            applyFreezeWhenSkillAttacked._types[AttackType.Melee] = true;
            applyFreezeWhenSkillAttacked._types[AttackType.Ranged] = true;
            applyFreezeWhenSkillAttacked._types[AttackType.Projectile] = true;

            applyFreezeWhenSkillAttacked._status = new ApplyInfo(status);

            SpearOfTheFrozenMoonAbility ability = new();

            item.abilities = [
                ability,
                applyFreezeWhenSkillAttacked,
            ];

            item.forbiddenDrops = new[] { "FrozenSpear" };

            items.Add(item);
        }

        return items;
    }

    internal static void LoadSprites()
    {
        Items.ForEach(item => item.LoadSprites());
    }

    internal static Dictionary<string, string> MakeStringDictionary()
    {
        Dictionary<string, string> strings = new(Items.Count * 8);

        foreach (var item in Items)
        {
            strings.Add("item/" + item.name + "/name", item.itemName);
            strings.Add("item/" + item.name + "/desc", item.itemDescription);
            strings.Add("item/" + item.name + "/flavor", item.itemLore);
        }

        return strings;
    }

    internal static List<Masterpiece.EnhancementMap> ListMasterpieces()
    {
        var masterpieces = Items.Where(i => (i.prefabKeyword1 == Inscription.Key.Masterpiece) || (i.prefabKeyword2 == Inscription.Key.Masterpiece))
                                .ToDictionary(i => i.name);

        return masterpieces.Where(item => masterpieces.ContainsKey(item.Key + "_2"))
                           .Select(item => new Masterpiece.EnhancementMap()
                           {
                               _from = new AssetReference(item.Value.guid),
                               _to = new AssetReference(masterpieces[item.Key + "_2"].guid),
                           })
                           .ToList();
    }
}
