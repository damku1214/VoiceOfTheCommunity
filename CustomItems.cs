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
     * Change inscription of Mana Accelerator: Artifact to Mana Cycle - done
     * Nerf Flask of Botulism to reducing 0.01 sec tick rate - done
     * Fix description of Blood-Soaked Javelin - done
     */

    private static List<CustomItemReference> InitializeItems()
    {
        List<CustomItemReference> items = new();
        {
            var item = new CustomItemReference();
            item.name = "VaseOfTheFallen";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Vase of the Fallen";
            item.itemName_KR = "영혼이 담긴 도자기";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (stacks up to 200% and 1/2 of total charge is lost when hit.)\n"
                                    + "Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit (cooldown: 3 seconds).";

            item.itemDescription_KR = "처치한 적의 수에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 5% 증가합니다 (최대 200% 증가, 피격시 증가치의 절반이 사라집니다).\n"
                                    + "피격 후 1초 내로 적 공격 시 감소한 증가치의 절반을 되돌려 받습니다 (쿨타임: 3초).";

            item.itemLore_EN = "Souls of the Eastern Kingdom's fallen warriors shall aid you in battle.";
            item.itemLore_KR = "장렬히 전사했던 동쪽 왕국의 병사들의 혼이 담긴 영물";

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

            item.itemName_EN = "Broken Heart";
            item.itemName_KR = "찢어진 심장";

            item.itemDescription_EN = "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                    + "Increases Quintessence cooldown speed by 30%.\n"
                                    + "Amplifies Quintessence damage by 15%.\n"
                                    + "If the 'Succubus' Quintessence is in your possession, this item turns into 'Lustful Heart'.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 20% 증가합니다.\n"
                                    + "정수 쿨다운 속도가 30% 증가합니다.\n"
                                    + "적에게 정수로 입히는 데미지가 15% 증폭됩니다.\n"
                                    + "'서큐버스' 정수 소지 시 이 아이템은 '색욕의 심장'으로 변합니다.";

            item.itemLore_EN = "Some poor being must have their heart torn both metaphorically and literally.";
            item.itemLore_KR = "딱한 것, 심장이 은유적으로도 물리적으로도 찢어지다니.";

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

            item.itemName_EN = "Lustful Heart";
            item.itemName_KR = "색욕의 심장";

            item.itemDescription_EN = "Amplifies <color=#1787D8>Magic Attack</color> by 20%.\n"
                                    + "Increases Quintessence cooldown speed by 60%.\n"
                                    + "Amplifies Quintessence damage by 30%.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 20% 증폭됩니다.\n"
                                    + "정수 쿨다운 속도가 60% 증가합니다.\n"
                                    + "적에게 정수로 입히는 데미지가 30% 증폭됩니다.\n";

            item.itemLore_EN = "Given to the greatest Incubus or Succubus directly from the demon prince of lust, Asmodeus.";
            item.itemLore_KR = "색욕의 마신 아스모데우스로부터 가장 위대한 인큐버스 혹은 서큐버스에게 하사된 증표";

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

            item.itemName_EN = "Small Twig";
            item.itemName_KR = "작은 나뭇가지";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 35%.\n"
                                    + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                    + "Increases Attack Speed by 15%.\n"
                                    + "All effects double and Amplifies damage dealt to enemies by 10% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 35% 증가합니다.\n"
                                    + "스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n"
                                    + "공격 속도가 15% 증가합니다.\n"
                                    + "'스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 스탯 증가치가 두배가 되며 적에게 입히는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "A really cool looking twig, but for some reason I feel sad...";
            item.itemLore_KR = "정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.35),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.35),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.15),
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

            item.itemName_EN = "Small Twig";
            item.itemName_KR = "작은 나뭇가지";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 35%.\n"
                                    + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                    + "Increases Attack Speed by 15%.\n"
                                    + "All effects double and Amplifies damage dealt to enemies by 10% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 35% 증가합니다.\n"
                                    + "스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n"
                                    + "공격 속도가 15% 증가합니다.\n"
                                    + "'스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 스탯 증가치가 두배가 되며 적에게 입히는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "A really cool looking twig, but for some reason I feel sad...";
            item.itemLore_KR = "정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.7),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.7),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.3),
            ]);

            ModifyDamage amplifyDamage = new();

            amplifyDamage._attackTypes = new([true, true, true, true, true, true, true, true, true]);

            amplifyDamage._damageTypes = new([true, true, true, true, true]);

            amplifyDamage._damagePercent = 1.1f;

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

            item.itemName_EN = "Volcanic Shard";
            item.itemName_KR = "화산의 일각";

            item.itemDescription_EN = "Increases <color=#1787D8>Magic Attack</color> by 80%.\n"
                                    + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                    + "Amplifies damage dealt to Burning enemies by 25%.\n"
                                    + "Burn duration decreases by 5% for each Arson inscription in possession.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 80% 증가합니다.\n"
                                    + "적 공격 시 20% 확률로 화상을 부여합니다.\n"
                                    + "화상 상태의 적에게 입히는 데미지가 25% 증폭됩니다.\n"
                                    + "가지고 있는 방화 각인에 비례하여 화상의 지속시간이 5%씩 감소합니다.";

            item.itemLore_EN = "Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.";
            item.itemLore_KR = "전설의 흑요석 화산의 폭발에서 만들어졌다고 전해진, 세상에서 가장 뜨거운 칼날";

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

            item.itemName_EN = "Rusty Chalice";
            item.itemName_KR = "녹슨 성배";

            item.itemDescription_EN = "Increases swap cooldown speed by 15%.\n"
                                    + "Upon hitting enemies with a swap skill 150 times, this item transforms into 'Goddess's Chalice'.";

            item.itemDescription_KR = "교대 쿨다운 속도가 15% 증가합니다.\n"
                                    + "적에게 교대스킬로 데미지를 150번 줄 시 해당 아이템은 '여신의 성배'로 변합니다.";

            item.itemLore_EN = "This thing? I found it at a pawn shop and it seemed interesting";
            item.itemLore_KR = "아 이거? 암시장에서 예뻐 보이길래 샀는데, 어때?";

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

            item.itemName_EN = "Goddess's Chalice";
            item.itemName_KR = "여신의 성배";

            item.itemDescription_EN = "Increases swap cooldown speed by 40%.\n"
                                    + "Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n"
                                    + "At maximum stacks, swap cooldown speed is increased by 25%.";

            item.itemDescription_KR = "교대 쿨다운 속도가 40% 증가합니다.\n"
                                    + "교대 시 6초 동안 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증가합니다 (최대 60%).\n"
                                    + "공격력 증가치가 최대일 시, 교대 쿨다운 속도가 25% 증가합니다.";

            item.itemLore_EN = "Chalice used by Leonia herself that seems to never run dry";
            item.itemLore_KR = "여신 레오니아 본인께서 쓰시던 절대 비워지지 않는 성배";

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

            item.itemName_EN = "Omen: Flask of Botulism";
            item.itemName_KR = "흉조: 역병의 플라스크";

            item.itemDescription_EN = "The interval between poison damage ticks is further decreased.";

            item.itemDescription_KR = "중독 데미지가 발생하는 간격이 더욱 줄어듭니다.";

            item.itemLore_EN = "Only the mad and cruel would consider using this as a weapon.";
            item.itemLore_KR = "정말 미치지 않고서야 이걸 무기로 쓰는 일은 없을 것이다.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Poisoning;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Constant, Stat.Kind.PoisonTickFrequency, 0.01),
            ]);

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CorruptedSymbol";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: Corrupted Symbol";
            item.itemName_KR = "흉조: 오염된 상징";

            item.itemDescription_EN = "For every Spoils inscription owned, increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 80%.";

            item.itemDescription_KR = "보유하고 있는 전리품 각인 1개당 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 80% 증가합니다.";

            item.itemLore_EN = "Where's your god now?";
            item.itemLore_KR = "자, 이제 네 신은 어딨지?";

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

            item.itemName_EN = "Tainted Finger";
            item.itemName_KR = "오염된 손가락";

            item.itemDescription_EN = "Skill damage dealt to enemies is amplified by 25%.\n"
                                    + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                    + "Increases incoming damage by 40%.";

            item.itemDescription_KR = "적에게 스킬로 입히는 데미지가 25% 증폭됩니다.\n"
                                    + "<color=#1787D8>마법공격력</color>이 60% 증가합니다.\n"
                                    + "받는 데미지가 40% 증가합니다.";

            item.itemLore_EN = "A finger from a god tainted by dark quartz";
            item.itemLore_KR = "검은 마석에 의해 침식된 신의 손가락";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.4),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.25f;

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

            item.itemName_EN = "Recovered Fingers";
            item.itemName_KR = "복구된 손가락들";

            item.itemDescription_EN = "Skill damage dealt to enemies is amplified by 25%.\n"
                                    + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                    + "Decreases incoming damage by 10%.\n"
                                    + "If the item 'Grace of Leonia' is in your possession, this item turns into 'Corrupted God's Hand'.";

            item.itemDescription_KR = "적에게 스킬로 입히는 데미지가 25% 증폭됩니다.\n"
                                    + "<color=#1787D8>마법공격력</color>이 60% 증가합니다.\n"
                                    + "받는 데미지가 10% 감소합니다.\n"
                                    + "'레오니아의 은총'을 함께 보유하고 있을 시 이 아이템은 '침식된 신의 손'으로 변합니다.";

            item.itemLore_EN = "The fingers of a no longer corrupted god";
            item.itemLore_KR = "더 이상 침식되지 않은 신의 손가락들";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.9),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.25f;

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

            item.itemName_EN = "Corrupted God's Hand";
            item.itemName_KR = "침식된 신의 손";

            item.itemDescription_EN = "Skill damage dealt to enemies is amplified by 55%.\n"
                                    + "Increases <color=#1787D8>Magic Attack</color> by 90%.\n"
                                    + "Increases incoming damage by 75%.";

            item.itemDescription_KR = "적에게 스킬로 입히는 데미지가 55% 증폭됩니다.\n"
                                    + "<color=#1787D8>마법공격력</color>이 90% 증가합니다.\n"
                                    + "받는 데미지가 75% 증가합니다.";

            item.itemLore_EN = "A corrupt hand from Leonia's supposed god";
            item.itemLore_KR = "레오니아로 추정되는 신의 침식된 손";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.9),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.75),
            ]);

            ModifyDamage amplifySkillDamage = new();

            amplifySkillDamage._attackTypes = new();
            amplifySkillDamage._attackTypes[MotionType.Skill] = true;

            amplifySkillDamage._damageTypes = new([true, true, true, true, true]);

            amplifySkillDamage._damagePercent = 1.55f;

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

            item.itemName_EN = "Dream Catcher";
            item.itemName_KR = "드림캐처";

            item.itemDescription_EN = "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                    + "<color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n"
                                    + "<color=#1787D8>Magic Attack</color> increases by 8% each time an Omen or a Legendary item is destroyed.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 50% 증가합니다.\n"
                                    + "현재 체력이 40% 이하인 적에게 입히는 <color=#1787D8>마법데미지</color>가 25% 증폭됩니다.\n"
                                    + "흉조 혹은 레전더리 등급을 가진 아이템을 파괴할 때마다 <color=#1787D8>마법공격력</color>이 8% 증가합니다.";

            item.itemLore_EN = "Acceptance is the first step towards death.";
            item.itemLore_KR = "수용은 죽음을 향한 첫 걸음이다.";

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

            item.itemName_EN = "Blood-Soaked Javelin";
            item.itemName_KR = "피투성이 투창";

            item.itemDescription_EN = "Increases Crit Damage by 20%.\n"
                                    + "Critical hits have a 15% chance to apply Wound (cooldown: 0.5 seconds).";

            item.itemDescription_KR = "치명타 데미지가 20% 증가합니다.\n"
                                    + "치명타 시 15% 확률로 적에게 상처를 부여합니다 (쿨타임: 0.5초).";

            item.itemLore_EN = "A javelin that always hits vital organs, and drains all the blood out of whichever one it hits";
            item.itemLore_KR = "적의 심장을 정확히 노려 시체에 피 한방울 남기지 않는 투창";

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

            item.itemName_EN = "Frozen Spear";
            item.itemName_KR = "얼음의 창";

            item.itemDescription_EN = "Normal attacks and skills have a 10% chance to inflict Freeze.\n"
                                    + "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                    + "After applying freeze 250 times, this item turns into 'Spear of the Frozen Moon'.";

            item.itemDescription_KR = "적에게 기본공격 혹은 스킬로 공격시 10% 확률로 빙결을 부여합니다.\n"
                                    + "<color=#1787D8>마법공격력</color>가 20% 증가합니다.\n"
                                    + "적에게 빙결을 250번 부여할 시 해당 아이템은 '얼어붙은 달의 창'으로 변합니다.";

            item.itemLore_EN = "A sealed weapon waiting the cold time to revealed it's true form.";
            item.itemLore_KR = "해방의 혹한을 기다리는 봉인된 무기";

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
            applyStatus._attackTypes[MotionType.Basic] = true;

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

            item.itemName_EN = "Spear of the Frozen Moon";
            item.itemName_KR = "얼어붙은 달의 창";

            item.itemDescription_EN = "Normal attacks and skills have a 20% chance to inflict Freeze.\n"
                                    + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                    + "Attacking frozen enemies increases the number of hits to remove Freeze by 1.\n"
                                    + "Amplifies damage dealt to frozen enemies by 25%.";

            item.itemDescription_KR = "적에게 기본공격 혹은 스킬로 공격시 20% 확률로 빙결을 부여합니다.\n"
                                    + "<color=#1787D8>마법공격력</color>가 60% 증가합니다.\n"
                                    + "빙결 상태의 적 공격 시 빙결이 해제되는데 필요한 타수가 1 증가합니다.\n"
                                    + "빙결 상태의 적에게 입히는 데미지가 25% 증가합니다.";

            item.itemLore_EN = "When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears.";
            item.itemLore_KR = "전장에 눈보라가 휘몰아칠 때, 얼음 괴수 발펜이 한 때 들었던 창이 나타난다.";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6),
            ]);

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Freeze;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 20;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Skill] = true;
            applyStatus._attackTypes[MotionType.Basic] = true;

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

            item.itemName_EN = "Cross Necklace";
            item.itemName_KR = "십자 목걸이";

            item.itemDescription_EN = "Recover 5 HP upon entering a map.";
            item.itemDescription_KR = "맵 입장 시 체력을 5 회복합니다.";

            item.itemLore_EN = "When all is lost, we turn to hope";
            item.itemLore_KR = "모든 것을 잃었을 때, 희망을 바라볼지니";

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

            item.itemName_EN = "Rotten Wings";
            item.itemName_KR = "썩은 날개";

            item.itemDescription_EN = "Crit Rate increases by 12% while in midair.\n"
                                    + "Your normal attacks have a 15% chance to inflict Poison.";

            item.itemDescription_KR = "공중에 있을 시 치명타 확률이 12% 증가합니다.\n"
                                    + "적에게 기본공격 시 15% 확률로 중독을 부여합니다.";

            item.itemLore_EN = "Wings of a zombie wyvern";
            item.itemLore_KR = "좀비 와이번의 썩어 문드러진 날개";

            item.prefabKeyword1 = Inscription.Key.Poisoning;
            item.prefabKeyword2 = Inscription.Key.Soar;

            StatBonusByAirTime bonus = new();

            bonus._timeToMaxStat = 0.01f;
            bonus._remainTimeOnGround = 0.0f;
            bonus._maxStat = new Stat.Values(new Stat.Value[] {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.12),
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

            item.itemName_EN = "Shrinking Potion";
            item.itemName_KR = "난쟁이 물약";

            item.itemDescription_EN = "Decreases character size by 20%.\n"
                                    + "Increases Movement Speed by 15%.\n"
                                    + "Incoming damage increases by 10%.";

            item.itemDescription_KR = "캐릭터 크기가 20% 감소합니다.\n"
                                    + "이동속도가 15% 증가합니다.\n"
                                    + "받는 데미지가 10% 증가합니다.";

            item.itemLore_EN = "I think it was meant to be used on the enemies...";
            item.itemLore_KR = "왠지 적에게 써야 할 것 같은데...";

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

            item.itemName_EN = "Unstable Size Potion";
            item.itemName_KR = "불안정한 크기 조정 물약";

            item.itemDescription_EN = "Alters between the effects of 'Shrinking Potion' and 'Growing Potion' every 10 seconds.";

            item.itemDescription_KR = "10초 마다 '난쟁이 물약'과 '성장 물약'의 효과를 번갈아가며 적용합니다.";

            item.itemLore_EN = "Mixing those potions together was a bad idea";
            item.itemLore_KR = "이 물약들을 섞는 것은 좋은 생각이 아니었다";

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

            item.itemName_EN = "Growing Potion";
            item.itemName_KR = "성장 물약";

            item.itemDescription_EN = "Increases character size by 20%.\n"
                                    + "Decreases Movement Speed by 15%.\n"
                                    + "Incoming damage decreases by 10%.";

            item.itemDescription_KR = "캐릭터 크기가 20% 증가합니다.\n"
                                    + "이동속도가 15% 감소합니다.\n"
                                    + "받는 데미지가 10% 감소합니다.";

            item.itemLore_EN = "Made from some weird size changing mushrooms deep within The Forest of Harmony";
            item.itemLore_KR = "하모니아 숲 깊숙이 있는 수상한 버섯으로 만들어진 물약";

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

            item.itemName_EN = "Mana Accelerator";
            item.itemName_KR = "마나 가속기";

            item.itemDescription_EN = "Skill casting speed increases by 10% for each Mana Cycle inscription in possession.";

            item.itemDescription_KR = "보유중인 마나순환 각인 1개당 스킬 시전 속도가 10% 증가합니다.";

            item.itemLore_EN = "In a last ditch effort, mages may turn to this device to overcharge their mana. Though the high stress on the mage's mana can often strip them of all magic.";
            item.itemLore_KR = "마나를 극한까지 끌어올리는 마법사들의 최후의 수단.\n너무 강한 과부하는 사용자를 불구로 만들 수 있으니 조심해야 한다.";

            item.prefabKeyword1 = Inscription.Key.Manatech;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

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

            item.itemName_EN = "Beginner's Lance";
            item.itemName_KR = "초보자용 창";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> by 20%.\n"
                                    + "Damage dealt to enemies with a dash attack is amplified by 30%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color>이 20% 증가합니다.\n"
                                    + "적에게 대쉬공격으로 입히는 데미지가 30% 증폭됩니다.";

            item.itemLore_EN = "Perfect! Now all I need is a noble steed...";
            item.itemLore_KR = "완벽해! 이제 좋은 말만 있으면 되는데...";

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

            item.itemName_EN = "Winged Spear";
            item.itemName_KR = "날개달린 창";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                    + "Increases Attack Speed by 15%.\n"
                                    + "Increases skill cooldown speed by 15%.\n"
                                    + "Increases swap cooldown speed by 15%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 15% 증가합니다.\n"
                                    + "공격속도가 15% 증가합니다.\n"
                                    + "스킬 쿨다운 속도가 15% 증가합니다.\n"
                                    + "교대 쿨다운 속도가 15% 증가합니다.";

            item.itemLore_EN = "A golden spear ornamented with the wings of dawn.";
            item.itemLore_KR = "여명의 날개로 치장된 금색 창";

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

            item.itemName_EN = "Solar-Winged Sword";
            item.itemName_KR = "태양빛 날개달린 검";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> by 55%.\n"
                                    + "Increases Attack Speed by 25%.\n"
                                    + "Increases swap cooldown speed by 25%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color>이 55% 증가합니다.\n"
                                    + "공격속도가 25% 증가합니다.\n"
                                    + "교대 쿨다운 속도가 25% 증가합니다.";

            item.itemLore_EN = "A golden sword ornamented with the wings of dawn.";
            item.itemLore_KR = "여명의 날개로 치장된 금색 검";

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

            item.itemName_EN = "Lunar-Winged Insignia";
            item.itemName_KR = "달빛 날개달린 휘장";

            item.itemDescription_EN = "Increases <color=#1787D8>Magic Attack</color> by 55%.\n"
                                    + "Increases skill cooldown speed by 25%.\n"
                                    + "Increases swap cooldown speed by 25%.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 55% 증가합니다.\n"
                                    + "스킬 쿨다운 속도가 25% 증가합니다.\n"
                                    + "교대 쿨다운 속도가 25% 증가합니다.";

            item.itemLore_EN = "A golden insignia ornamented with the wings of dawn.";
            item.itemLore_KR = "여명의 날개로 치장된 금색 휘장";

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

            item.itemName_EN = "Wings of Dawn";
            item.itemName_KR = "여명의 날개";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 75%.\n"
                                    + "Increases Attack Speed by 45%.\n"
                                    + "Increases skill cooldown speed by 45%.\n"
                                    + "Increases swap cooldown speed by 45%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 75% 증가합니다.\n"
                                    + "공격속도가 45% 증가합니다.\n"
                                    + "스킬 쿨다운 속도가 45% 증가합니다.\n"
                                    + "교대 쿨다운 속도가 45% 증가합니다.";

            item.itemLore_EN = "A divine spear donning the wings of dawn.";
            item.itemLore_KR = "여명의 날개가 현현한 신성한 창";

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

            item.itemName_EN = "Omen: Last Dawn";
            item.itemName_KR = "흉조: 최후의 여명";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 110%.\n"
                                    + "Increases Attack Speed by 65%.\n"
                                    + "Increases skill cooldown speed by 65%.\n"
                                    + "Increases swap cooldown speed by 65%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 110% 증가합니다.\n"
                                    + "공격속도가 65% 증가합니다.\n"
                                    + "스킬 쿨다운 속도가 65% 증가합니다.\n"
                                    + "교대 쿨다운 속도가 65% 증가합니다.";

            item.itemLore_EN = "The sky cracks, darkness fills the world within.";
            item.itemLore_KR = "하늘은 갈라져 속세를 어둠에 물들였다.";

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

            item.itemName_EN = "Fonias";
            item.itemName_KR = "포니아스";

            item.itemDescription_EN = "Increases Crit Chance by 5%.\n"
                                    + "Increases Crit Damage by 25%.\n"
                                    + "Amplifies damage dealt to enemies by 10%.\n"
                                    + "Amplfies damage dealt to an adventurer or a boss by 5%.";

            item.itemDescription_KR = "치명타 확률이 5% 증가합니다.\n"
                                    + "치명타 피해가 25% 증가합니다.\n"
                                    + "적에게 입히는 데미지가 10% 증폭됩니다.\n"
                                    + "모험가 혹은 보스에게 입히는 데미지가 5% 증폭됩니다.";

            item.itemLore_EN = "An ancient scythe imbued with cursed power.\nIt was once wielded by a former demon king.";
            item.itemLore_KR = "전대 마왕중 한명이 사용했다는 저주의 기운을 뿜어내는 고대의 낫";

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

            item.itemName_EN = "Spiky Rapida";
            item.itemName_KR = "가시덤불 레이피어";

            item.itemDescription_EN = "Increases Attack Speed by 20%.\n"
                                    + "Every 3rd normal attack, inflicts Wound to enemies that were hit.";

            item.itemDescription_KR = "공격속도가 20% 증가합니다.\n"
                                    + "3회 째 기본공격마다 피격된 적에게 상처를 입힙니다.";

            item.itemLore_EN = "In ancient times, when there was no English language yet, you would have been called 'Victor'.....";
            item.itemLore_KR = "태초의 시절, 이곳의 언어도 없던 때에 당신은 '빅토르' 라고 불렸던 것 같다.....";

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

            item.itemName_EN = "Weird Herbs";
            item.itemName_KR = "수상한 허브";

            item.itemDescription_EN = "Swapping increases skill cooldown speed by 25% and Crit Rate by 12% for 6 seconds.";

            item.itemDescription_KR = "교대 시 6초 동안 스킬 쿨다운 속도가 25% 증가하고 치명타 확률이 12% 증가합니다.";

            item.itemLore_EN = "Quartz-infused herbs which you can find all over the dark forest.";
            item.itemLore_KR = "어둠의 숲 전역에서 찾을 수 있는 마석과 융합된 허브";

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

            item.itemName_EN = "Omen: Accursed Sabre";
            item.itemName_KR = "흉조: 저주받은 단도";

            item.itemDescription_EN = "Basic attacks and skills have a 10% chance to apply Wound.\n"
                                    + "Every 2nd Bleed inflicts Bleed twice.";

            item.itemDescription_KR = "적 공격 시 10% 확률로 상처를 부여합니다.\n"
                                    + "2회 째 출혈마다 출혈을 한번 더 부여합니다.";

            item.itemLore_EN = "Sabre of the great duelist Sly who left his final memento in the form of never-ending anarchy and bloodshed.";
            item.itemLore_KR = "끝없는 반역과 학살을 낳았던 세계 제일의 결투가 슬라이의 단도";

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

            item.itemName_EN = "Heavy-Duty Carleon Helmet";
            item.itemName_KR = "중보병용 칼레온 투구";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n"
                                    + "For every 'Carleon' item owned, increase Max HP by 15.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가합니다.\n"
                                    + "보유하고 있는 '칼레온' 아이템 1개당 최대 체력이 15 증가합니다.";

            item.itemLore_EN = "Only the strongest of Carleon's front line soldiers can wear this.\nThat... isn't saying very much, but still.";
            item.itemLore_KR = "가장 강한 칼레온의 최전선에 선 병사들만이 쓸 수 있는 투구.\n하지만 큰 의미는 없어보인다.";

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

            item.itemName_EN = "Cursed Hourglass";
            item.itemName_KR = "저주받은 모래시계";

            item.itemDescription_EN = "Upon entering a map or hitting a boss phase for the first time, amplifies damage dealt to enemies by 30% for 30 seconds.\n"
                                    + "When the effect is not active, increases damage received by 30%.";

            item.itemDescription_KR = "맵 입장 혹은 보스 페이즈 에게 처음 데미지를 줄 시 30초 동안 적에게 입히는 데미지가 30% 증폭됩니다.\n"
                                    + "해당 효과가 발동 중이지 않을 때, 받는 데미지가 30% 증가합니다.";

            item.itemLore_EN = "To carry such a burden voluntarily... You're either the bravest person I've ever met, or the most foolish.";
            item.itemLore_KR = "이런 짐을 짊어지다니... 넌 아마 이 세상에서 가장 용감하거나 멍청한 사람이겠지.";

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

            item.itemName_EN = "Lucky Coin";
            item.itemName_KR = "행운의 동전";

            item.itemDescription_EN = "Increases Crit Rate by 5%.\n"
                                    + "Increases Gold gain by 10%.";

            item.itemDescription_KR = "치명타 확률이 5% 증가합니다.\n"
                                    + "금화 획득량이 10% 증가합니다.";

            item.itemLore_EN = "Oh, must be my lucky day!";
            item.itemLore_KR = "오늘은 운수가 좋은 날인가 보군!";

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

            item.itemName_EN = "Tainted Red Scarf";
            item.itemName_KR = "변색된 붉은 목도리";

            item.itemDescription_EN = "Increases dash cooldown speed by 20%.\n"
                                    + "Decreases dash distance by 30%.";

            item.itemDescription_KR = "대쉬 쿨다운 속도가 30% 증가합니다.\n"
                                    + "대쉬 거리가 30% 감소합니다.";

            item.itemLore_EN = "A small scarf that was once part of an old doll";
            item.itemLore_KR = "어떤 인형에서 떨어져 나온 작은 목도리";

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
            item.name = "TatteredCatPlushie";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "Tattered Cat Plushie";
            item.itemName_KR = "해진 고양이 인형";

            item.itemDescription_EN = "Every 5 seconds, depletes 10% of your Max HP and permanently grants you 5% amplification on damage dealt to enemies.\n"
                                    + "Upon killing an enemy, recovers 2% of your Max HP.";

            item.itemDescription_KR = "5초마다 최대 체력의 10%에 달하는 피해를 입고 영구적으로 적들에게 입히는 데미지가 5% 증폭됩니다.\n"
                                    + "적을 처치할 때마다 최대 체력의 2%를 회복합니다.";

            item.itemLore_EN = "bEsT FrIenDs fOrEveR... rIGht..?";
            item.itemLore_KR = "우린 영원히 함께야... 그렇지..?";

            item.prefabKeyword1 = Inscription.Key.Mystery;
            item.prefabKeyword2 = Inscription.Key.Sin;

            TatteredCatPlushieAbility ability = new()
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
