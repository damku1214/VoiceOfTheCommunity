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
using Characters.Gear;
using VoiceOfTheCommunity.CustomBehaviors;

namespace VoiceOfTheCommunity;

public class CustomItems
{
    public static readonly List<CustomItemReference> Items = InitializeItems();

    /**
     * TODO
     * 
     * Add magic atk boost to Tainted Finger line - done
     * Change Volcanic Shard from amping burn to amping burning enemies - done
     * Change name of Tainted Red Scarf in Thunderstore README
     * Change description of Heavy-Duty Carleon Helmet - done
     * Block Growing Potion spawning with Unstable Size Potion - done
     * Fix Shrinking Potion not disappearing on evolution with full inventory
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

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (stacks up to 200% and 1/2 of total charge is lost when hit).\n
            // Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit (Cooldown: 3 seconds).

            // KR: 처치한 적의 수에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 5% 증가합니다 (최대 200% 증가, 피격시 증가치의 절반이 사라집니다).\n
            // 피격 후 1초 내로 적 공격 시 감소한 증가치의 절반을 되돌려 받습니다 (쿨타임: 3초).

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (stacks up to 200% and 1/2 of total charge is lost when hit.)\n"
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
            quintDamage._attackTypes[MotionType.Quintessence] = true;

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
            quintDamage._attackTypes[MotionType.Quintessence] = true;

            quintDamage._damageTypes = new([true, true, true, true, true]);

            quintDamage._damagePercent = 1.3f;

            item.abilities = [
                quintDamage
            ];

            item.forbiddenDrops = new[] { "Custom-BrokenHeart" };

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
            // All effects double and amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.

            // KR: <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증폭됩니다.\n
            // 스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n
            // 치명타 확률 및 치명타 피해가 10% 증가합니다.\n
            // '스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 스탯 증가치가 두배가 되며 적에게 입히는 데미지가 20% 증폭됩니다.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                 + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                 + "Increases Crit Rate and Crit Damage by 10%.\n"
                                 + "All effects double and amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.";

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

            amplifyDamage._attackTypes = new([true, true, true, true, true, true, true, true, true]);

            amplifyDamage._damageTypes = new([true, true, true, true, true]);

            amplifyDamage._damagePercent = 1.2f;

            item.abilities = [
                amplifyDamage
            ];

            item.extraComponents = [
                typeof(SmallTwigRevertBehavior),
            ];

            item.forbiddenDrops = new[] { "Custom-SmallTwig" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "VolcanicShard";
            item.rarity = Rarity.Legendary;

            // EN: Volcanic Shard
            // KR: 화산의 일각
            item.itemName = "Volcanic Shard";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 80%.\n
            // Normal attacks and skills have a 20% chance to inflict Burn.\n
            // Amplifies damage dealt to Burning enemies by 25%.\n
            // Burn duration decreases by 5% for each Arson inscription in possession.

            // KR: <color=#1787D8>마법공격력</color>이 80% 증가합니다.\n
            // 적 공격 시 20% 확률로 화상을 부여합니다.\n
            // 적에게 화상으로 입히는 데미지가 25% 증폭됩니다.\n
            // 가지고 있는 방화 각인에 비례하여 화상의 지속시간이 5%씩 감소합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 80%.\n"
                                 + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                 + "Amplifies damage dealt to Burning enemies by 25%.\n"
                                 + "Burn duration decreases by 5% for each Arson inscription in possession.";

            // EN: Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.
            // KR: 전설의 흑요석 화산의 폭발에서 만들어졌다고 전해진, 세상에서 가장 뜨거운 칼날
            item.itemLore = "Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Arson;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.8),
            ]);

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Burn;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 20;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Basic] = true;
            applyStatus._attackTypes[MotionType.Skill] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                new VolcanicShardAbility(),
                applyStatus,
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

            // KR: 교대 쿨다운 속도가 15% 증가합니다.\n
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

            item.abilities = [
                new RustyChaliceAbility(),
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
            // Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n
            // At maximum stacks, swap cooldown speed is increased by 25%.

            // KR: 교대 쿨다운 속도가 40% 증가합니다.\n
            // 교대 시 6초 동안 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증가합니다 (최대 60%).\n
            // 공격력 증가치가 최대일 시, 교대 쿨다운 속도가 25% 증가합니다.

            item.itemDescription = "Increases swap cooldown speed by 40%.\n"
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
            ];

            item.forbiddenDrops = new[] { "Custom-RustyChalice" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "FlaskOfBotulism";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            // EN: Omen: Flask of Botulism
            // KR: 흉조: 역병의 플라스크
            item.itemName = "Omen: Flask of Botulism";

            // EN: The interval between poison damage ticks is further decreased.

            // KR: 중독 데미지가 발생하는 간격이 더욱 줄어듭니다.

            item.itemDescription = "The interval between poison damage ticks is further decreased.";

            // EN: Only the mad and cruel would consider using this as a weapon.
            // KR: 정말 미치지 않고서야 이걸 무기로 쓰는 일은 없을 것이다.
            item.itemLore = "Only the mad and cruel would consider using this as a weapon.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Poisoning;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Constant, Stat.Kind.PoisonTickFrequency, 0.1),
            ]);

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CorruptedSymbol";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
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

            CorruptedSymbolAbility ability = new()
            {
                _statPerStack = new Stat.Values(
                [
                    new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.8),
                    new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.8),
                ])
            };

            item.abilities = [
                ability,
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

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

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

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.3f;

            item.abilities = [
                amplifySkillDamage,
            ];

            item.extraComponents = [
                typeof(TaintedFingerEvolveBehavior),
            ];

            item.forbiddenDrops = new[] { "Custom-TaintedFinger" };

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

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 2f;

            item.abilities = [
                amplifySkillDamage,
            ];

            item.forbiddenDrops = new[] { "Custom-TaintedFinger" };

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
            // <color=#1787D8>Magic Attack</color> increases by 8% each time an Omen or a Legendary item is destroyed.

            // KR: <color=#1787D8>마법공격력</color>이 50% 증가합니다.\n
            // 현재 체력이 40% 이하인 적에게 입히는 <color=#1787D8>마법데미지</color>가 25% 증폭됩니다.\n
            // 흉조 혹은 레전더리 등급을 가진 아이템을 파괴할 때마다 <color=#1787D8>마법공격력</color>이 8% 증가합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                 + "<color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n"
                                 + "<color=#1787D8>Magic Attack</color> increases by 8% each time an Omen or a Legendary item is destroyed.";

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
            item.name = "BloodSoakedJavelin";
            item.rarity = Rarity.Rare;

            // EN: Blood-Soaked Javelin
            // KR: 피투성이 투창
            item.itemName = "Blood-Soaked Javelin";

            // EN: Increases Crit Damage by 20%.\n
            // Critical hits have a 10% chance to apply Wound (Cooldown: 0.5 seconds).

            // KR: 치명타 데미지가 20% 증가합니다.\n
            // 치명타 시 10% 확률로 적에게 상처를 부여합니다 (쿨타임: 0.5초).

            item.itemDescription = "Increases Crit Damage by 20%.\n"
                                 + "Critical hits have a 10% chance to apply Wound.";

            // EN: A javelin that always hits vital organs, and drains all the blood out of whichever one it hits
            // KR: 적의 심장을 정확히 노려 시체에 피 한방울 남기지 않는 투창
            item.itemLore = "A javelin that always hits vital organs, and drains all the blood out of whichever one it hits";

            item.prefabKeyword1 = Inscription.Key.Misfortune;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.25),
            ]);

            item.abilities = [
                new BloodSoakedJavelinAbility(),
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

            // EN: Skills have a 10% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // After applying freeze 250 times, this item turns into 'Spear of the Frozen Moon'.

            // KR: 적에게 스킬로 공격시 10% 확률로 빙결을 부여합니다.\n
            // <color=#1787D8>마법공격력</color>가 20% 증가합니다.\n
            // 적에게 빙결을 250번 부여할 시 해당 아이템은 '얼어붙은 달의 창'으로 변합니다.

            item.itemDescription = "Skills have a 10% chance to inflict Freeze.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                 + "After applying freeze 250 times, this item turns into 'Spear of the Frozen Moon'.";

            // EN: A sealed weapon waiting the cold time to revealed it's true form.
            // KR: 해방의 혹한을 기다리는 봉인된 무기
            item.itemLore = "A sealed weapon waiting the cold time to revealed it's true form.";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.2),
            ]);

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Freeze;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 10;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Skill] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                new FrozenSpearAbility(),
                applyStatus,
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

            // EN: Skills have a 15% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.\n
            // Attacking frozen enemies increases the number of hits to remove Freeze by 1.\n
            // Amplifies damage to frozen enemies by 25%.

            // KR: 적에게 스킬로 공격시 15% 확률로 빙결을 부여합니다.\n
            // <color=#1787D8>마법공격력</color>가 60% 증가합니다.\n
            // 빙결 상태의 적 공격 시 빙결이 해제되는데 필요한 타수가 1 증가합니다.\n
            // 빙결 상태의 적에게 입히는 데미지가 25% 증가합니다.

            item.itemDescription = "Skills have a 15% chance to inflict Freeze.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                 + "Attacking frozen enemies increases the number of hits to remove Freeze by 1.\n"
                                 + "Amplifies damage dealt to frozen enemies by 25%.";

            // EN: When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. 
            // KR: 전장에 눈보라가 휘몰아칠 때, 얼음 괴수 발펜의 창이 나타날지니
            item.itemLore = "When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. ";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
            ]);

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Freeze;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 15;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Skill] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                new SpearOfTheFrozenMoonAbility(),
                applyStatus,
            ];

            item.forbiddenDrops = new[] { "Custom-FrozenSpear" };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CrossNecklace";
            item.rarity = Rarity.Common;

            // EN: Cross Necklace
            // KR: 십자 목걸이
            item.itemName = "Cross Necklace";

            // EN: Recover 5 HP upon entering a map.

            // KR: 맵 입장 시 체력을 5 회복합니다.

            item.itemDescription = "Recover 5 HP upon entering a map.";

            // EN: When all is lost, we turn to hope
            // KR: 모든 것을 잃었을 때, 희망을 바라볼지니
            item.itemLore = "Acceptance is the first step towards death.";

            item.prefabKeyword1 = Inscription.Key.Relic;
            item.prefabKeyword2 = Inscription.Key.Heritage;

            item.abilities = [
                new CrossNecklaceAbility(),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RottenWings";
            item.rarity = Rarity.Rare;

            // EN: Rotten Wings
            // KR: 썩은 날개
            item.itemName = "Rotten Wings";

            // EN: Crit Rate increases by 15% while in midair.\n
            // Your normal attacks have a 15% chance to inflict Poison.

            // KR: 공중에 있을 시 치명타 확률이 15% 증가합니다.\n
            // 적에게 기본공격 시 15% 확률로 중독을 부여합니다.

            item.itemDescription = "Crit Rate increases by 15% while in midair.\n"
                                 + "Your normal attacks have a 15% chance to inflict Poison.";

            // EN: Wings of a zombie wyvern
            // KR: 좀비 와이번의 썩어 문드러진 날개
            item.itemLore = "Wings of a zombie wyvern";

            item.prefabKeyword1 = Inscription.Key.Poisoning;
            item.prefabKeyword2 = Inscription.Key.Soar;

            StatBonusByAirTime bonus = new();

            bonus._timeToMaxStat = 0.01f;
            bonus._remainTimeOnGround = 0.0f;
            bonus._maxStat = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.15),
            });

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Poison;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 15;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Basic] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                bonus,
                applyStatus,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ShrinkingPotion";
            item.rarity = Rarity.Rare;

            // EN: Shrinking Potion
            // KR: 난쟁이 물약
            item.itemName = "Shrinking Potion";

            // EN: Decreases character size by 20%.\n
            // Increases Movement Speed by 15%.\n
            // Incoming damage increases by 10%.

            // KR: 캐릭터 크기가 20% 감소합니다.\n
            // 이동속도가 15% 증가합니다.\n
            // 받는 데미지가 10% 증가합니다.

            item.itemDescription = "Decreases character size by 20%.\n"
                                 + "Increases Movement Speed by 15%.\n"
                                 + "Incoming damage increases by 10%.";

            // EN: I think it was meant to be used on the enemies...
            // KR: 왠지 적에게 써야 할 것 같은데...
            item.itemLore = "I think it was meant to be used on the enemies...";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Chase;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Percent, Stat.Kind.CharacterSize, 0.8),
                new(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.15),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.1),
            ]);

            item.extraComponents = [
                typeof(ShrinkingPotionEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ShrinkingPotion_2";
            item.rarity = Rarity.Unique;

            item.obtainable = false;

            // EN: Unstable Size Potion
            // KR: 불안정한 크기 조정 물약
            item.itemName = "Unstable Size Potion";

            // EN: Alters between the effects of 'Shrinking Potion' and 'Growing Potion' every 10 seconds.

            // KR: 10초 마다 '난쟁이 물약'과 '성장 물약'의 효과를 번갈아가며 적용합니다.

            item.itemDescription = "Alters between the effects of 'Shrinking Potion' and 'Growing Potion' every 10 seconds.";

            // EN: Mixing those potions together was a bad idea
            // KR: 이 물약들을 섞는 것은 좋은 생각이 아니었다.
            item.itemLore = "Mixing those potions together was a bad idea";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Antique;

            UnstableSizePotionAbility ability = new()
            {
                _timeout = 10,
                _shrinkingStat = new Stat.Values(
                [
                    new(Stat.Category.Percent, Stat.Kind.CharacterSize, 0.8),
                    new(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.15),
                    new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.1),
                ]),
                _growingStat = new Stat.Values([
                    new(Stat.Category.Percent, Stat.Kind.CharacterSize, 1.2),
                    new(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, -0.15),
                    new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.9),
                ])
            };

            item.abilities = [
                ability,
            ];

            item.forbiddenDrops = new[] {
                "Custom-ShrinkingPotion",
                "Custom-GrowingPotion",
            };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GrowingPotion";
            item.rarity = Rarity.Rare;

            // EN: Growing Potion
            // KR: 성장 물약
            item.itemName = "Growing Potion";

            // EN: Increases character size by 20%.\n
            // Decreases Movement Speed by 15%.\n
            // Incoming damage decreases by 10%.

            // KR: 캐릭터 크기가 20% 증가합니다.\n
            // 이동속도가 15% 감소합니다.\n
            // 받는 데미지가 10% 감소합니다.

            item.itemDescription = "Increases character size by 20%.\n"
                                 + "Decreases Movement Speed by 15%.\n"
                                 + "Incoming damage decreases by 10%.";

            // EN: Made from some weird size changing mushrooms deep within The Forest of Harmony
            // KR: 하모니아 숲 깊숙이 있는 수상한 버섯으로 만들어진 물약
            item.itemLore = "Made from some weird size changing mushrooms deep within The Forest of Harmony";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Percent, Stat.Kind.CharacterSize, 1.2),
                new(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, -0.15),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.9),
            ]);

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ManaAccelerator";
            item.rarity = Rarity.Rare;

            // EN: Mana Accelerator
            // KR: 마나 가속기
            item.itemName = "Mana Accelerator";

            // EN: Skill casting speed increases by 10% for each Mana Cycle inscription in possession.

            // KR: 보유중인 마나순환 각인 1개당 스킬 시전 속도가 10% 증가합니다.

            item.itemDescription = "Skill casting speed increases by 10% for each Mana Cycle inscription in possession.";

            // EN: In a last ditch effort, mages may turn to this device to overcharge their mana. Though the high stress on the mage's mana can often strip them of all magic.
            // KR: 마나를 극한까지 과부하시키는 마법사들의 최후의 수단.\n너무 강한 과부하는 사용자를 불구로 만들 수 있으니 조심해야 한다
            item.itemLore = "In a last ditch effort, mages may turn to this device to overcharge their mana. Though the high stress on the mage's mana can often strip them of all magic.";

            item.prefabKeyword1 = Inscription.Key.Manatech;
            item.prefabKeyword2 = Inscription.Key.Artifact;

            ManaAcceleratorAbility ability = new()
            {
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.1),
                ])
            };

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BeginnersLance";
            item.rarity = Rarity.Common;

            // EN: Beginner's Lance
            // KR: 초보자용 창
            item.itemName = "Beginner's Lance";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 20%.\n
            // Damage dealt to enemies with a dash attack is amplified by 30%.

            // KR: <color=#F25D1C>물리공격력</color>이 20% 증가합니다.\n
            // 적에게 대쉬공격으로 입히는 데미지가 30% 증폭됩니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 20%.\n"
                                 + "Damage dealt to enemies with a dash attack is amplified by 30%.";

            // EN: Perfect! Now all I need is a noble steed...
            // KR: 완벽해! 이제 좋은 말만 있으면 되는데...
            item.itemLore = "Perfect! Now all I need is a noble steed...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Chase;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.2),
            ]);

            ModifyDamage amplifyDashDamage = new();

            amplifyDashDamage._attackTypes = new();
            amplifyDashDamage._attackTypes[MotionType.Dash] = true;

            amplifyDashDamage._damageTypes = new([true, true, true, true, true]);

            amplifyDashDamage._damagePercent = 1.3f;

            item.abilities = [
                amplifyDashDamage,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WingedSpear";
            item.rarity = Rarity.Common;

            // EN: Winged Spear
            // KR: 날개달린 창
            item.itemName = "Winged Spear";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 15%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases Attack Speed by 15%.\n
            // Increases skill cooldown speed by 15%.\n
            // Increases swap cooldown speed by 15%.

            // KR: <color=#F25D1C>물리공격력</color>이 15% 증가합니다.\n
            // <color=#1787D8>마법공격력</color>이 15% 증가합니다.\n
            // 공격속도가 15% 증가합니다.\n
            // 스킬 쿨다운 속도가 15% 증가합니다.\n
            // 교대 쿨다운 속도가 15% 증가합니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 15%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 15%.\n"
                                 + "Increases Attack Speed by 15%.\n"
                                 + "Increases skill cooldown speed by 15%.\n"
                                 + "Increases swap cooldown speed by 15%.";

            // EN: A golden spear ornamented with the wings of dawn.
            // KR: 여명의 날개로 치장된 금색 창
            item.itemLore = "A golden spear ornamented with the wings of dawn.";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.SunAndMoon;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.15),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.15),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.15),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.15),
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.15),
            ]);

            item.extraComponents = [
                typeof(WingedSpearEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WingedSpear_2";
            item.rarity = Rarity.Unique;

            item.obtainable = false;

            // EN: Solar-Winged Sword
            // KR: 햇빛 날개달린 검
            item.itemName = "Solar-Winged Sword";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 55%.\n
            // Increases Attack Speed by 25%.\n
            // Increases swap cooldown speed by 25%.

            // KR: <color=#F25D1C>물리공격력</color>이 55% 증가합니다.\n
            // 공격속도가 25% 증가합니다.\n
            // 교대 쿨다운 속도가 25% 증가합니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 55%.\n"
                                 + "Increases Attack Speed by 25%.\n"
                                 + "Increases swap cooldown speed by 25%.";

            // EN: A golden sword ornamented with the wings of dawn.
            // KR: 여명의 날개로 치장된 금색 검
            item.itemLore = "A golden sword ornamented with the wings of dawn.";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Arms;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.55),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.25),
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.25),
            ]);

            item.forbiddenDrops = new[]
            {
                "SwordOfSun",
                "RingOfMoon",
                "ShardOfDarkness",
                "Custom-WingedSpear"
            };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WingedSpear_3";
            item.rarity = Rarity.Unique;

            item.obtainable = false;

            // EN: Lunar-Winged Insignia
            // KR: 달빛 날개달린 휘장
            item.itemName = "Lunar-Winged Insignia";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 55%.\n
            // Increases skill cooldown speed by 15%.\n
            // Increases swap cooldown speed by 15%.

            // KR: <color=#1787D8>마법공격력</color>이 55% 증가합니다.\n
            // 스킬 쿨다운 속도가 25% 증가합니다.\n
            // 교대 쿨다운 속도가 25% 증가합니다.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 55%.\n"
                                 + "Increases skill cooldown speed by 25%.\n"
                                 + "Increases swap cooldown speed by 25%.";

            // EN: A golden insignia ornamented with the wings of dawn.
            // KR: 여명의 날개로 치장된 금색 휘장
            item.itemLore = "A golden insignia ornamented with the wings of dawn.";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Artifact;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.55),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.25),
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.25),
            ]);

            item.forbiddenDrops = new[]
            {
                "SwordOfSun",
                "RingOfMoon",
                "ShardOfDarkness",
                "Custom-WingedSpear"
            };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WingedSpear_4";
            item.rarity = Rarity.Unique;

            item.obtainable = false;

            // EN: Wings of Dawn
            // KR: 여명의 날개
            item.itemName = "Wings of Dawn";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 75%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 75%.\n
            // Increases Attack Speed by 45%.\n
            // Increases skill cooldown speed by 45%.\n
            // Increases swap cooldown speed by 45%.

            // KR: <color=#F25D1C>물리공격력</color>이 75% 증가합니다.\n
            // <color=#1787D8>마법공격력</color>이 75% 증가합니다.\n
            // 공격속도가 45% 증가합니다.\n
            // 스킬 쿨다운 속도가 45% 증가합니다.\n
            // 교대 쿨다운 속도가 45% 증가합니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 75%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 75%.\n"
                                 + "Increases Attack Speed by 45%.\n"
                                 + "Increases skill cooldown speed by 45%.\n"
                                 + "Increases swap cooldown speed by 45%.";

            // EN: A divine spear donning the wings of dawn.
            // KR: 여명의 날개를 흡수한 신성한 창
            item.itemLore = "A divine spear donning the wings of dawn.";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Duel;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.75),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.75),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.45),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.45),
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.45),
            ]);

            item.forbiddenDrops = new[]
            {
                "SwordOfSun",
                "RingOfMoon",
                "ShardOfDarkness",
                "Custom-WingedSpear"
            };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WingedSpear_5";
            item.rarity = Rarity.Unique;

            item.obtainable = false;

            // EN: Omen: Last Dawn
            // KR: 흉조: 최후의 여명
            item.itemName = "Omen: Last Dawn";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 110%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 110%.\n
            // Increases Attack Speed by 65%.\n
            // Increases skill cooldown speed by 65%.\n
            // Increases swap cooldown speed by 65%.

            // KR: <color=#F25D1C>물리공격력</color>이 110% 증가합니다.\n
            // <color=#1787D8>마법공격력</color>이 110% 증가합니다.\n
            // 공격속도가 65% 증가합니다.\n
            // 스킬 쿨다운 속도가 65% 증가합니다.\n
            // 교대 쿨다운 속도가 65% 증가합니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 110%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 110%.\n"
                                 + "Increases Attack Speed by 65%.\n"
                                 + "Increases skill cooldown speed by 65%.\n"
                                 + "Increases swap cooldown speed by 65%.";

            // EN: The sky cracks, darkness fills the world within.
            // KR: 하늘은 갈라져 속세를 어둠에 물들지니.
            item.itemLore = "The sky cracks, darkness fills the world within.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Duel;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.1),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.1),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.65),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.65),
                new(Stat.Category.PercentPoint, Stat.Kind.SwapCooldownSpeed, 0.65),
            ]);

            item.forbiddenDrops = new[]
            {
                "SwordOfSun",
                "RingOfMoon",
                "ShardOfDarkness",
                "Custom-WingedSpear"
            };

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Fonias";
            item.rarity = Rarity.Unique;

            // EN: Fonias
            // KR: 포니아스
            item.itemName = "Fonias";

            // EN: Increases Crit Chance by 5%.\n
            // Increases Crit Damage by 25%.\n
            // Amplifies damage dealt to enemies by 10%.\n
            // Amplfies damage dealt to an adventurer or a boss by 5%.

            // KR: 치명타 확률이 5% 증가합니다.\n
            // 치평타 피해가 25% 증가합니다.\n
            // 적에게 입히는 데미지가 10% 증폭됩니다.\n
            // 모험가 혹은 보스에게 입히는 데미지가 5% 증폭됩니다.

            item.itemDescription = "Increases Crit Chance by 5%.\n"
                                 + "Increases Crit Damage by 25%.\n"
                                 + "Amplifies damage dealt to enemies by 10%.\n"
                                 + "Amplfies damage dealt to an adventurer or a boss by 5%.";

            // EN: An ancient scythe imbued with cursed power.\nIt was once wielded by a former demon king.
            // KR: 전대 마왕중 한명이 사용했다는 저주의 기운을 뿜어내는 고대의 낫
            item.itemLore = "An ancient scythe imbued with cursed power.\nIt was once wielded by a former demon king.";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.05),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.25),
            ]);

            ModifyDamage amplifyDamage = new();

            amplifyDamage._attackTypes = new([true, true, true, true, true, true, true, true, true]);

            amplifyDamage._damageTypes = new([true, true, true, true, true]);

            amplifyDamage._damagePercent = 1.1f;

            item.abilities = [
                new FoniasAbility(),
                amplifyDamage,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SpikyRapida";
            item.rarity = Rarity.Rare;

            // EN: Spiky Rapida
            // KR: 가시덤불 레이피어
            item.itemName = "Spiky Rapida";

            // EN: Increases Attack Speed by 20%.\n
            // Every 3rd normal attack, inflicts Wound to enemies that were hit.

            // KR: 공격속도가 20% 증가합니다.\n
            // 3회 째 기본공격마다 피격된 적에게 상처를 입힙니다.

            item.itemDescription = "Increases Attack Speed by 20%.\n"
                                 + "Every 3rd normal attack, inflicts Wound to enemies that were hit.";

            // EN: In ancient times, when there was no English language yet, you would have been called "Victor".....
            // KR: 태초의 시절, 이곳의 언어도 없던 때에 당신은 "빅토르" 라고 불렸던 것 같다.....
            item.itemLore = "In ancient times, when there was no English language yet, you would have been called \"Victor\".....";

            item.prefabKeyword1 = Inscription.Key.ExcessiveBleeding;
            item.prefabKeyword2 = Inscription.Key.Rapidity;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.2),
            ]);

            item.abilities = [
                new SpikyRapidaAbility(),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "WeirdHerbs";
            item.rarity = Rarity.Rare;

            // EN: Weird Herbs
            // KR: 수상한 허브
            item.itemName = "Weird Herbs";

            // EN: Swapping increases skill cooldown speed by 25% and Crit Rate by 12% for 6 seconds.

            // KR: 교대 시 6초 동안 스킬 쿨다운 속도가 25% 증가하고 치명타 확률이 12% 증가합니다.

            item.itemDescription = "Swapping increases skill cooldown speed by 25% and Crit Rate by 12% for 6 seconds.";

            // EN: Quartz-infused herbs which you can find all over the dark forest.
            // KR: 어둠의 숲 전역에서 찾을 수 있는 마석과 융합된 허브
            item.itemLore = "Quartz-infused herbs which you can find all over the dark forest.";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            WeirdHerbsAbility ability = new()
            {
                _timeout = 6.0f,
                _stat = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.25),
                    new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.12),
                ]),
            };

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "AccursedSabre";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            // EN: Omen: Accursed Sabre
            // KR: 저주받은 단도
            item.itemName = "Omen: Accursed Sabre";

            // EN: Basic attacks and skills have a 10% chance to apply Wound.\n
            // Every 2nd Bleed inflicts Bleed twice.

            // KR: 적 공격 시 10% 확률로 상처를 부여합니다.\n
            // 2회 째 출혈마다 출혈을 한번 더 부여합니다.

            item.itemDescription = "Basic attacks and skills have a 10% chance to apply Wound.\n"
                                 + "Every 2nd Bleed inflicts Bleed twice.";

            // EN: Sabre of the great duelist Sly who left his final memento in the form of never-ending anarchy and bloodshed.
            // KR: 끝없는 반역과 학살을 낳았던 세계 제일의 결투가 슬라이의 단도
            item.itemLore = "Sabre of the great duelist Sly who left his final memento in the form of never-ending anarchy and bloodshed.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Wound;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 10;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Basic] = true;
            applyStatus._attackTypes[MotionType.Skill] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                new AccursedSabreAbility(),
                applyStatus,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "HeavyDutyCarleonHelmet";
            item.rarity = Rarity.Rare;

            item.gearTag = Gear.Tag.Carleon;

            // EN: Heavy-Duty Carleon Helmet
            // KR: 중보병용 칼레온 투구
            item.itemName = "Heavy-Duty Carleon Helmet";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n
            // For every 'Carleon' item owned, increase Max HP by 15.

            // KR: <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가합니다.\n
            // 보유하고 있는 '칼레온' 아이템 1개당 최대 체력이 15 증가합니다.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n"
                                 + "For every 'Carleon' item owned, increase Max HP by 15.";

            // EN: Only the strongest of Carleon's front line soldiers can wear this.\nThat... isn't saying very much, but still.
            // KR: 가장 강한 칼레온의 최전선에 선 병사들만이 쓸 수 있는 투구.\n하지만 큰 의미는 없어보인다.
            item.itemLore = "Only the strongest of Carleon's front line soldiers can wear this.\nThat... isn't saying very much, but still.";

            item.prefabKeyword1 = Inscription.Key.Antique;
            item.prefabKeyword2 = Inscription.Key.Spoils;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.3),
            ]);

            StatBonusPerGearTag statBonusPerCarleonItem = new();

            statBonusPerCarleonItem._tag = Gear.Tag.Carleon;

            statBonusPerCarleonItem._statPerGearTag = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 15),
            });

            item.abilities = [
                statBonusPerCarleonItem,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CursedHourglass";
            item.rarity = Rarity.Legendary;

            // EN: Cursed Hourglass
            // KR: 저주받은 모래시계
            item.itemName = "Cursed Hourglass";

            // EN: Upon entering a map or hitting a boss phase for the first time, amplifies damage dealt to enemies by 30% for 30 seconds.\n
            // When the effect is not active, increases damage received by 30%.

            // KR: 맵 입장 혹은 보스(페이즈 포함) 에게 처음 데미지를 줄 시 30초 동안 적에게 입히는 데미지가 30% 증폭됩니다.\n
            // 해당 효과가 발동 중이지 않을 때, 받는 데미지가 30% 증가합니다.

            item.itemDescription = "Upon entering a map or hitting a boss phase for the first time, amplifies damage dealt to enemies by 30% for 30 seconds.\n"
                                 + "When the effect is not active, increases damage received by 30%.";

            // EN: To carry such a burden voluntarily... You're either the bravest person I've ever met, or the most foolish.
            // KR: 이런 짐을 짊어지다니... 넌 아마 이 세상에서 가장 용감하거나 멍청한 사람이겠지.
            item.itemLore = "To carry such a burden voluntarily... You're either the bravest person I've ever met, or the most foolish.";

            item.prefabKeyword1 = Inscription.Key.ManaCycle;
            item.prefabKeyword2 = Inscription.Key.Execution;

            CursedHourglassAbility ability = new()
            {
                _timeout = 30,
                _inactiveStat = new Stat.Values(
                [
                    new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.3),
                ])
            };

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "LuckyCoin";
            item.rarity = Rarity.Common;

            // EN: Lucky Coin
            // KR: 행운의 동전
            item.itemName = "Lucky Coin";

            // EN: Increases Crit Rate by 5%.\n
            // Increases Gold gain by 10%.

            // KR: 치명타 확률이 5% 증가합니다.\n
            // 금화 획득량이 10% 증가합니다.

            item.itemDescription = "Increases Crit Rate by 5%.\n"
                                 + "Increases Gold gain by 10%.";

            // EN: Oh, must be my lucky day!
            // KR: 오늘은 운수가 좋은 날인가 보군!
            item.itemLore = "Oh, must be my lucky day!";

            item.prefabKeyword1 = Inscription.Key.Treasure;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.05),
            ]);

            item.abilities = [
                new LuckyCoinAbility(),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TaintedRedScarf";
            item.rarity = Rarity.Rare;

            // EN: Tainted Red Scarf
            // KR: 변색된 붉은 목도리
            item.itemName = "Tainted Red Scarf";

            // EN: Increases dash cooldown speed by 30%.\n
            // Decreases dash distance by 30%.

            // KR: 대쉬 쿨다운 속도가 30% 증가합니다.\n
            // 대쉬 거리가 30% 감소합니다.

            item.itemDescription = "Increases dash cooldown speed by 20%.\n"
                                 + "Decreases dash distance by 30%.";

            // EN: A small scarf that was once part of an old doll
            // KR: 어떤 인형에서 떨어져 나온 작은 목도리
            item.itemLore = "A small scarf that was once part of an old doll";

            item.prefabKeyword1 = Inscription.Key.Mystery;
            item.prefabKeyword2 = Inscription.Key.Chase;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, 0.2),
                new(Stat.Category.Percent, Stat.Kind.DashDistance, 0.7),
            ]);

            item.extraComponents = [
                typeof(TaintedRedScarfEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TatteredPlushie";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            // EN: Tattered Plushie
            // KR: 해진 인형
            item.itemName = "Tattered Plushie";

            // EN: Every 5 seconds, depletes 10% of your Max HP and permanently grants you 5% amplification on damage dealt to enemies.\n
            // Upon killing an enemy, recovers 2% of your Max HP.

            // KR: 5초마다 최대 체력의 10%에 달하는 피해를 입고 영구적으로 적들에게 입히는 데미지가 5% 증폭됩니다.\n
            // 적을 처치할 때마다 최대 체력의 2%를 회복합니다.

            item.itemDescription = "Every 5 seconds, depletes 10% of your Max HP and permanently grants you 5% amplification on damage dealt to enemies.\n"
                                 + "Upon killing an enemy, recovers 2% of your Max HP.";

            // EN: bEsT FrIenDs fOrEveR
            // KR: 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께 영원히 함께
            item.itemLore = "bEsT FrIenDs fOrEveR";

            item.prefabKeyword1 = Inscription.Key.Mystery;
            item.prefabKeyword2 = Inscription.Key.Sin;

            TatteredPlushieAbility ability = new()
            {
                _timeout = 5.0f,
            };

            item.abilities = [
                ability,
            ];

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
