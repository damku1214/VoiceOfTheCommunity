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
using Characters.Gear.Items;

namespace VoiceOfTheCommunity;

public class CustomItems
{
    public static readonly List<CustomItemReference> Items = InitializeItems();

    /**
     * TODO
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

            item.itemDescription_KR = "처치한 적의 수에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 5% 증가합니다 (최대 200%, 피격시 증가치의 절반이 사라집니다).\n"
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
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.05)
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
                quintDamage,
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
                quintDamage,
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
                                    + "All effects double and amplifies damage dealt to enemies by 10% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 35% 증가합니다.\n"
                                    + "스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n"
                                    + "공격 속도가 15% 증가합니다.\n"
                                    + "'스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 수치 증가치가 두배가 되며 적에게 입히는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "A really cool looking twig, but for some reason I feel sad...";
            item.itemLore_KR = "정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.35),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.35),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.45),
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
                                    + "All effects double and amplifies damage dealt to enemies by 10% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 35% 증가합니다.\n"
                                    + "스킬 쿨다운 속도 및 스킬 시전 속도가 30% 증가합니다.\n"
                                    + "공격 속도가 15% 증가합니다.\n"
                                    + "'스컬' 혹은 '용사 리틀본' 스컬을 사용 중일 시 이 아이템의 모든 수치 증가치가 두배가 되며 적에게 입히는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "A really cool looking twig, but for some reason I feel sad...";
            item.itemLore_KR = "정말 멋있어 보이는 나뭇가지일 터인데, 왜 볼 때 마다 슬퍼지는 걸까...";

            item.prefabKeyword1 = Inscription.Key.Duel;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.7),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.7),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.9),
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
                                    + "Decreases Burn duration and increases Burn damage dealt to enemies by 20%.\n"
                                    + "Amplifies damage dealt to burning enemies by 5% for each Arson inscription in possession.";

            item.itemDescription_KR = "<color=#1787D8>마법공격력</color>이 80% 증가합니다.\n"
                                    + "적 공격 시 20% 확률로 화상을 부여합니다.\n"
                                    + "화상의 지속시간이 20% 감소하고 적에게 입히는 화상 데미지가 20% 증가합니다.\n"
                                    + "가지고 있는 방화 각인에 비례하여 화상 상태의 적에게 입히는 데미지 5%씩 증폭됩니다.";

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
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.2),
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
            item.itemLore_KR = "전장에 눈보라가 휘몰아칠 때, 전설의 얼음 괴수가 한 때 들었던 창이 나타난다.";

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

            applyStatus._defaultIcon = null;

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
                                    + "Normal attacks have a 15% chance to inflict Poison.";

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
            item.itemLore_KR = "이거 적에게 쓰는 것 같은데...";

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

            amplifyDashDamage._defaultIcon = null;

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
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.15),
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
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.25),
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
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.45),
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
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.65),
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

            item.itemDescription_EN = "Amplifies damage dealt to a boss by 15%.\n"
                                    + "Killing a boss further increases this amplification by 2.5%.";

            item.itemDescription_KR = "보스에게 입히는 데미지가 15% 증폭됩니다.\n"
                                    + "보스 처치 시 이 아이템의 데미지 증폭량이 2.5% 증가합니다.";

            item.itemLore_EN = "An ancient scythe imbued with cursed power.\nIt was once wielded by a former demon king.";
            item.itemLore_KR = "전대 마왕중 한명이 사용했다는 저주의 기운을 뿜어내는 고대의 낫";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.05),
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.25),
            ]);

            item.abilities = [
                new FoniasAbility()
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
            item.itemLore_KR = "태초의 시절, 이곳의 언어도 없던 때에 피어오른 한 송이의 꽃";

            item.prefabKeyword1 = Inscription.Key.ExcessiveBleeding;
            item.prefabKeyword2 = Inscription.Key.Rapidity;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.2),
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

            item.itemLore_EN = "Sabre of the great duelist Sly who left his final memento in the form of never-ending anarchy and bloodshed";
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
            item.itemLore_KR = "가장 강한 칼레온의 최전선에 선 병사들만이 쓸 수 있는 투구.\n... 큰 의미는 없어보인다.";

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

            item.itemDescription_KR = "맵 입장 혹은 보스 페이즈에게 처음 데미지를 줄 시 30초 동안 적에게 입히는 데미지가 30% 증폭됩니다.\n"
                                    + "해당 효과가 발동 중이지 않을 때 받는 데미지가 30% 증가합니다.";

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

            item.itemDescription_EN = "Increases dash cooldown speed by 30%.\n"
                                    + "Decreases dash distance by 30%.";

            item.itemDescription_KR = "대쉬 쿨다운 속도가 30% 증가합니다.\n"
                                    + "대쉬 거리가 30% 감소합니다.";

            item.itemLore_EN = "A small scarf that was once part of an old doll";
            item.itemLore_KR = "어떤 인형에서 떨어져 나온 작은 목도리";

            item.prefabKeyword1 = Inscription.Key.Mystery;
            item.prefabKeyword2 = Inscription.Key.Chase;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, 0.3),
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
                                    + "If your Max HP is lower than 150, deplete 15 HP instead."
                                    + "Upon killing an enemy, recovers 4% of your Max HP.";

            item.itemDescription_KR = "5초마다 최대 체력의 10%에 달하는 피해를 입고 영구적으로 적들에게 입히는 데미지가 5% 증폭됩니다.\n"
                                    + "만약 최대 체력이 150 미만일 경우, 대신 15의 피해를 입습니다."
                                    + "적을 처치할 때마다 최대 체력의 4%를 회복합니다.";

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
        {
            var item = new CustomItemReference();
            item.name = "DisorientationDevice";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Disorientation Device";
            item.itemName_KR = "감각 혼란 장치";

            item.itemDescription_EN = "Increases stun duration by 0.25 seconds.\n"
                                    + "Amplifies damage dealt to stunned enemies by 10%.";

            item.itemDescription_KR = "기절의 지속시간이 0.25초 증가합니다.\n"
                                    + "기절 상태의 적에게 입히는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "Nobody can get close enough to turn it off.\nThey all end up going unconscious or losing their sense of direction.";
            item.itemLore_KR = "접근하는 사람의 방향 감각을 상실하게 하는, 절대 끌 수 없는 장치";

            item.prefabKeyword1 = Inscription.Key.Dizziness;
            item.prefabKeyword2 = Inscription.Key.Manatech;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.Constant, Stat.Kind.StunDuration, 0.25)
            ]);

            item.abilities = [
                new DisorientationDeviceAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SoulFlameScythe";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Soul Flame Scythe";
            item.itemName_KR = "영혼불의 낫";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 50%.\n"
                                    + "Increases Attack Speed and skill cooldown speed by 20%.\n"
                                    + "When an enemy is killed, there is a 45% chance to increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 1% (Stacks up to 300%)."; ;

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 50% 증가합니다.\n"
                                    + "공격속도 및 스킬 쿨다운 속도가 20% 증가합니다.\n"
                                    + "적 처치 시 45% 확률로 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 1% 증가합니다 (최대 300% 증가).";

            item.itemLore_EN = "Once the soul mage Bear attempted to harness the powers of the god of the netherworld, but what remains is his scythe...";
            item.itemLore_KR = "한 때 영혼술사 베어가 마신의 힘을 흡수하려고 했으나, 남겨져 있는 것은 그의 낫 뿐이다...";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Heirloom;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.5),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.5),
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.2)
            ]);

            SoulFlameScytheAbility ability = new()
            {
                _maxStack = 300,
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.01),
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.01)
                ])
            };

            item.abilities = [
                ability
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CarleonCommandersBihander";
            item.rarity = Rarity.Legendary;

            item.gearTag = Gear.Tag.Carleon;

            item.itemName_EN = "Carleon Commander's Bihänder";
            item.itemName_KR = "칼레온 사령관의 양손검";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 40%.\n"
                                    + "When standing near the flag made by the Spoils inscription, amplify damage dealt to enemies by 20% and decrease incoming damage by 15%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 40% 증가합니다.\n"
                                    + "전리품 각인이 생성한 깃발 주변에 있을 시 적에게 입히는 데미지가 20% 증폭되고 받는 데미지가 15% 감소합니다.";

            item.itemLore_EN = "A legendary sword passed down between commanders. Bears the crest and halo of Carleon.";
            item.itemLore_KR = "대대로 칼레온의 사령관들에게 하사되는 전설의 검. 칼레온의 업적과 정신이 담겨져 있다.";

            item.prefabKeyword1 = Inscription.Key.Spoils;
            item.prefabKeyword2 = Inscription.Key.Arms;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.4)
            ]);

            CarleonCommandersBihanderAbility ability = new()
            {
                _statInFlag = new Stat.Values(
                [
                    new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.85)
                ])
            };

            item.abilities = [
                ability,
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RustyShovel";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Rusty Shovel";
            item.itemName_KR = "녹슨 삽";

            item.itemDescription_EN = "Damaging 50% of an enemy's Max HP (10% for adventurers and bosses) amplifies the damage of your next <color=#F25D1C>physical attack</color> by 50%.";

            item.itemDescription_KR = "적 최대체력의 50% (모험가 및 보스의 경우 10%) 이상의 데미지를 입힐 시 다음 <color=#F25D1C>물리공격</color>의 데미지가 50% 증폭됩니다.";

            item.itemLore_EN = "Anything can be lethal if wielded with enough conviction";
            item.itemLore_KR = "투지만 있다면 무엇이든 무기가 될 수 있다";

            item.prefabKeyword1 = Inscription.Key.Brave;
            item.prefabKeyword2 = Inscription.Key.Arms;

            item.abilities = [
                new RustyShovelAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CrimsonCap";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Crimson Cap";
            item.itemName_KR = "진홍빛 모자";

            item.itemDescription_EN = "Upon entering a room, sets the amount of Crimson Essences you have to 1 and gains a Crimson Essence for every piece of hazardous equipment in your inventory.\n"
                                    + "For every Crimson Essence in possession, increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 40%.\n"
                                    + "Upon being hit, consume a Crimson Essence and negates that hit (This effect occurs before Mercury Heart does).";

            item.itemDescription_KR = "방 입장 시 가지고 있던 진홍빛 정수를 1로 만든 뒤 인벤토리에 있는 위험한 장비만큼 진홍빛 정수를 획득합니다."
                                    + "가지고 있는 진홍빛 정수만큼 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 40% 증가합니다.\n"
                                    + "피격 시 진홍빛 정수를 한개 소모하고 해당 피격을 무효화합니다 (해당 효과는 수은 심장이 발동하기 전에 발동합니다).";

            item.itemLore_EN = "The signature attire of a particularly bold adventurer named Will";
            item.itemLore_KR = "어느 한 대범한 모험가가 동료처럼 항상 갖고 다니던 모자";

            item.prefabKeyword1 = Inscription.Key.Heirloom;
            item.prefabKeyword2 = Inscription.Key.Execution;

            item.abilities = [
                new CrimsonCapAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "DevilsMask";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Devil's Mask";
            item.itemName_KR = "악마의 가면";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> by 40%.\n"
                                    + "Increases skill casting speed by 25%.\n"
                                    + "Increases skill cooldown speed by 25%.\n"
                                    + "Upon being hit, increases <color=#F25D1C>Physical Attack</color> by 40%, skill casting speed by 15%, and skill cooldown speed by 15% for 5 seconds (cooldown: 15 seconds).";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 이 40% 증가합니다.\n"
                                    + "스킬 시전 속도가 25% 증가합니다.\n"
                                    + "스킬 쿨다운 속도가 25% 증가합니다.\n"
                                    + "피격 시 5초간 <color=#F25D1C>물리공격력</color> 이 40% 증가하고 스킬 시전 속도 및 스킬 쿨다운 속도가 15% 증가합니다 (쿨타임: 15초).";

            item.itemLore_EN = "The visage of the vessel that filled the continent with terror and destruction";
            item.itemLore_KR = "대륙을 공포와 파멸로 가득 채운 흉물의 형상";

            item.prefabKeyword1 = Inscription.Key.Brave;
            item.prefabKeyword2 = Inscription.Key.ManaCycle;

            item.stats = new Stat.Values(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.25),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.25)
            ]);

            DevilsMaskAbility ability = new()
            {
                _timeout = 5,
                _cooldown = 15,
                _statWhenActive = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.4),
                    new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.15),
                    new(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 0.15)
                ])
            };

            item.abilities = [
                ability
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SwordOfAges";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Sword of Ages";
            item.itemName_KR = "세기의 검";

            item.itemDescription_EN = "Amplifies damage dealt to enemies and damage taken by 10% for every Arms inscription you have.";

            item.itemDescription_KR = "보유중인 무구 각인 하나당 적에게 입히는 데미지와 받는 데미지가 10% 증폭됩니다.";

            item.itemLore_EN = "an abomination forged over time from dozens of enchanted swords, it's magic has has become highly unstable as a result.";
            item.itemLore_KR = "세대를 거칠 때 마다 칼날이 하나씩 더 붙은 검. 오랜 시간이 지나 한데 뭉쳐진 여러 검들의 마법이 한없이 축적되여 사용자까지 위험에 처하게 만들었다.";

            item.prefabKeyword1 = Inscription.Key.Arms;
            item.prefabKeyword2 = Inscription.Key.Arms;

            item.abilities = [
                new SwordOfAgesAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "DemonGuardsTrainingSword";
            item.rarity = Rarity.Common;

            item.itemName_EN = "Demon-guard's Training Sword";
            item.itemName_KR = "마족 경비병의 훈련용 검";

            item.itemDescription_EN = "Skills have a 15% chance to inflict Wound.";

            item.itemDescription_KR = "스킬로 적 공격 시 15% 확률로 상처를 부여합니다.";

            item.itemLore_EN = "\"Alright cadets, it's training day! Now for financial reasons we only have one sword available, so make sure you share it.\"";
            item.itemLore_KR = "\"모두들 오늘도 힘차게 훈련할 준비 됐나?! 경제적인 문제로 인해서 검이 하나밖에 없으니, 잘 나눠서 쓰도록!\"";

            item.prefabKeyword1 = Inscription.Key.ManaCycle;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Wound;
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
                applyStatus
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ShieldOfTheUnrelenting";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Shield of the Unrelenting";
            item.itemName_KR = "용서하지 않는 자의 방패";

            item.itemDescription_EN = "Gain 5 Shields every 5 seconds (Up to 40).\n"
                                    + "Increases <color=#F25D1C>Physical Attack</color> by 5% for every 5 Shields (Maximum 40%).";

            item.itemDescription_KR = "5초마다 5의 방어막을 얻습니다 (최대 40).\n"
                                    + "보유한 방어막 5당 <color=#F25D1C>물리공격력</color>이 5% 증가합니다 (최대 40% 증가).";

            item.itemLore_EN = "Now men, This is where we shall stand, and this is where we shall stay!";
            item.itemLore_KR = "설령 육체가 한계를 맞이하더라도 우리는 결코 물러서지 않는다!";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Brave;

            item.abilities = [
                new ShieldOfTheUnrelentingAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TheSwordOfTheProtector";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "The Sword of the Protector";
            item.itemName_KR = "수호자의 검";

            item.itemDescription_EN = "When below 50% HP, increases <color=#F25D1C>Physical Attack</color> by 150%, reduces damage taken by 40%, and increases Attack Speed by 75%.";

            item.itemDescription_KR = "체력이 50% 이하일 때, <color=#F25D1C>물리공격력</color>이 150% 증가하고, 받는 데미지가 40% 감소하고, 공격속도가 75% 증가합니다.";

            item.itemLore_EN = "The sword that belonged to the last line of defense in Carleon";
            item.itemLore_KR = "칼레온의 마지막 전선에서 싸우던 자들의 의지가 담긴 검";

            item.prefabKeyword1 = Inscription.Key.Arms;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.abilities = [
                new TheSwordOfTheProtectorAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CursedShield";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: Cursed Shield";
            item.itemName_KR = "흉조: 저주받은 방패";

            item.itemDescription_EN = "Reduces damage taken by 25% while a HP barrier is active.\n"
                                    + "Every 1.5 seconds, lose 3 HP and gain 3 Shields (maximum 300 Shields).\n"
                                    + "The effect deactivates when gained maximum amount of Shields or when current HP is 1.";

            item.itemDescription_KR = "방어막을 소지하고 있을 시 받는 데미지가 25% 감소합니다.\n"
                                    + "1.5초 마다 체력을 3 잃고 3의 방어막을 얻습니다 (최대 300의 방어막).\n"
                                    + "최대 개수의 방어막을 소지하고 있거나 현재 체력이 1 이하일 경우 해당 효과는 발생하지 않습니다.";

            item.itemLore_EN = "This shield will protect me from everything!\nEven if... it costs my... life...";
            item.itemLore_KR = "무적이 되는 대가로 생명을 내어준 어리석은 자의 방패";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.abilities = [
                new CursedShieldAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "HorcruxPendant";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: Horcrux Pendant";
            item.itemName_KR = "흉조: 호크룩스 목걸이";

            item.itemDescription_EN = "Amplifies all stats by 10%.\n"
                                    + "Additionally amplifies all stats by 10% when the Heirloom inscription effect is active (lasts for 10 seconds when not active).";

            item.itemDescription_KR = "모든 수치가 10% 증폭됩니다.\n"
                                    + "가보 각인의 효과가 발동중일 시 추가로 모든 수치가 10% 증폭됩니다 (발동중이 아닐 시 10초 동안 유지됩니다).";

            item.itemLore_EN = "A pendant worn only by the most foolish of sorcerers.\nGives the wearer unparalleled strength at the cost of self stability.";
            item.itemLore_KR = "마법사들 중 가장 멍청한 자들만이 입는 목걸이.\n자신의 안전을 대가로 엄청난 힘을 얻게 해준다.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Heirloom;

            item.stats = new(
            [
                new(Stat.Category.Percent, Stat.Kind.Health, 1.1),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.1),
                new(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 1.1),
                new(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 1.1),
                new(Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.MovementSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.ChargingSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.SkillCooldownSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.SwapCooldownSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.EssenceCooldownSpeed, 1.1),
                new(Stat.Category.Percent, Stat.Kind.CriticalChance, 1.1),
                new(Stat.Category.Percent, Stat.Kind.CriticalDamage, 1.1)
            ]);

            item.abilities = [
                new HorcruxPendantAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BottledFaeling";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Bottled Faeling";
            item.itemName_KR = "병에 담은 요정";

            item.itemDescription_EN = "Increases cooldown speed of all spirits by 15%.\n"
                                    + "Amplifies damage dealt to enemies with spirits by 5%.\n"
                                    + "When Oberon is existent, additionally amplify damage dealt to enemies with spirits by 15%.\n"
                                    + "Upon receiving fatal damage, release the Bottled Faeling and restore 25% of HP to survive death (destroys the item).";

            item.itemDescription_KR = "모든 정령들의 쿨다운 속도가 15% 증가합니다.\n"
                                    + "정령들이 적에게 입히는 데미지가 5% 증폭됩니다.\n"
                                    + "오베론이 존재할 시 정령들이 적에게 입히는 데미지가 추가로 15% 증폭됩니다.\n"
                                    + "죽음에 이르는 데미지를 입을 시 이 아이템은 파괴되며 25%의 체력을 회복하고 죽음을 극복합니다.";

            item.itemLore_EN = "Hey, I ain't much of a fighter but I can offer my support. Also it's comfy in here...";
            item.itemLore_KR = "이봐, 난 싸움은 못하지만 도움은 줄 수 있다구. 그리고 여기 엄청 편안해...";

            item.prefabKeyword1 = Inscription.Key.FairyTale;
            item.prefabKeyword2 = Inscription.Key.Antique;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.SpiritAttackCooldownSpeed, 0.15),
            ]);

            item.abilities = [
                new BottledFaelingAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GingaPachinko";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Ginga Pachinko";
            item.itemName_KR = "은하 새총";

            item.itemDescription_EN = "Increases Crit Damage by 20%.\n"
                                    + "Upon being hit, attacks gain a 15% chance to inflict Wound for 8 seconds.\n"
                                    + "If you possess Veiled Mask, the mask transforms into something new.";

            item.itemDescription_KR = "치명타 데미지가 20% 증가합니다.\n"
                                    + "피격 시 8초 동안 공격 시 15% 확률로 상처를 부여합니다.\n"
                                    + "가려진 가면을 가지고 있을 시 가면이 다른 모습으로 변합니다.";

            item.itemLore_EN = "The weapon of the deadliest sniper in the Grand Line, at least he'd have you think.";
            item.itemLore_KR = "위대한 항로 제일의 저격수가 쓰는 무기... 처럼 보인다.";

            item.prefabKeyword1 = Inscription.Key.Strike;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.2)
            ]);

            GingaPachinkoAbility ability = new()
            {
                _woundChance = 15
            };

            item.abilities = [
                ability
            ];

            item.extraComponents = [
                typeof(GingaPachinkoEvolveBehavior)
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GingaPachinko_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "Kabuto";
            item.itemName_KR = "투구";

            item.itemDescription_EN = "Increases Crit Damage by 25%.\n"
                                    + "Upon being hit, attacks gain a 20% chance to inflict Wound for 8 seconds.\n"
                                    + "Reduces damage taken by 20%.\n"
                                    + "When 'Mask of Sogeking' is not in possession, turns back into 'Ginga Pachinko'.";

            item.itemDescription_KR = "치명타 데미지가 25% 증가합니다.\n"
                                    + "피격 시 8초 동안 공격 시 20% 확률로 상처를 부여합니다.\n"
                                    + "받는 데미지가 20% 감소합니다.\n"
                                    + "'저격왕의 가면'이 인벤토리 내에 없을 시 이 아이템은 다시 '은하 새총' 으로 변합니다.";

            item.itemLore_EN = "The very weapon used to shoot down the World Government's flag, thus declaring war on the entire world.";
            item.itemLore_KR = "세계정부의 깃발을 쏘아내려 전 세계에 전쟁을 선포한 바로 그 무기";

            item.prefabKeyword1 = Inscription.Key.Strike;
            item.prefabKeyword2 = Inscription.Key.ExcessiveBleeding;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.25),
                new(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.8)
            ]);

            GingaPachinkoAbility ability = new()
            {
                _woundChance = 20
            };

            item.abilities = [
                ability
            ];

            item.extraComponents = [
                typeof(KabutoRevertBehavior)
            ];

            item.forbiddenDrops = ["Custom-GingaPachinko"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "MaskOfSogeking";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "Mask of Sogeking";
            item.itemName_KR = "저격왕의 가면";

            item.itemDescription_EN = "Increases Crit Damage by 70%.\n"
                                    + "When 'Kabuto' is not in possession, turns back into 'Veiled Mask'.";

            item.itemDescription_KR = "치명타 데미지가 70% 증가합니다.\n"
                                    + "'투구'가 인벤토리 내에 없을 시 이 아이템은 다시 '가려진 가면' 으로 변합니다.";

            item.itemLore_EN = "Lu lu lala lu~";
            item.itemLore_KR = "룰루랄라루~";

            item.prefabKeyword1 = Inscription.Key.Strike;
            item.prefabKeyword2 = Inscription.Key.Duel;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.7)
            ]);

            item.extraComponents = [
                typeof(MaskOfSogekingRevertBehavior)
            ];

            item.forbiddenDrops = ["VeiledMask"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Becchi";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Becchi";
            item.itemName_KR = "베티";

            item.itemDescription_EN = "Upon being hit, increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 50%, amplifies damage dealt to enemies by 4%, and increases damage taken by 2% for 5 seconds (stacks up to 5 times).";

            item.itemDescription_KR = "피격 시 5초 동안 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 50% 증가하고, 적에게 입히는 데미지가 4% 증폭되며, 받는 데미지가 2% 증가합니다 (최대 5번 중첩 가능).";

            item.itemLore_EN = "Cared for by the mighty warrior Puck.";
            item.itemLore_KR = "위대한 전사 파크가 열심히 관리한 수수께기의 물건";

            item.prefabKeyword1 = Inscription.Key.Revenge;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.abilities = [
                new BecchiAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "StandardIssueMiningPick";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Standard-issue Mining Pick";
            item.itemName_KR = "보급형 곡괭이";

            item.itemDescription_EN = "Hitting enemies have a 30% chance to drop 1~3 gold.\n"
                                    + "Collecting gold increases Crit Damage by 10% for 3 seconds.\n"
                                    + "Upon collecting 1500 gold via this item, or when the 'Dwarf' or 'King Dwarf' Quintessence is in possesion, it transforms.";

            item.itemDescription_KR = "적 공격 시 30% 확률로 1~3 금화를 획득합니다.\n"
                                    + "금화를 획득할 시 3초 동안 치명타 데미지가 10% 증가합니다.\n"
                                    + "이 아이템으로 1500 금화를 획득하거나 '드워프' 혹은 '드워프 대왕' 정수를 소지할 시 진화합니다.";

            item.itemLore_EN = "An old model intergalactic pickaxe made by the R&D department.\nSeems to be made for underground mining on the most mineral rich and dangerous planet orbiting the blue star Creus.";
            item.itemLore_KR = "연구 및 개발 부서에서 만든 구식 우주 곡괭이.\n푸른 별 크레우스를 공전하는 가장 위험하고 광물이 많은 행성에서 자원을 캐기 위해 만든 것 같다.";

            item.prefabKeyword1 = Inscription.Key.Treasure;
            item.prefabKeyword2 = Inscription.Key.Strike;

            StandardIssueMiningPickAbility ability = new()
            {
                _isEvolved = false,
                _minGold = 1,
                _maxGold = 3,
                _stat = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.1)
                ])
            };

            item.abilities = [
                ability
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "StandardIssueMiningPick_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "Dwarven Legend's Pickaxe";
            item.itemName_KR = "전설의 드워프의 곡괭이";

            item.itemDescription_EN = "Hitting enemies have a 30% chance to drop 2~4 gold.\n"
                                    + "Collecting gold increases Crit Damage by 20% for 3 seconds.";

            item.itemDescription_KR = "적 공격 시 30% 확률로 2~4 골드를 획득합니다.\n"
                                    + "골드를 획득할 시 3초 동안 치명타 데미지가 20% 증가합니다.";

            item.itemLore_EN = "A pickaxe that belonged to a man named Karl. Has a simple phrase engraved on the head,\n\"We fight, we mine, for rock and stone!\"";
            item.itemLore_KR = "칼이라는 자가 가지고 있던 곡괭이. 머리 부분에 간단한 문구가 적혀있다.\n\"우리는 싸운다! 우리는 캔다! 락 앤 스톤!\"";

            item.prefabKeyword1 = Inscription.Key.Treasure;
            item.prefabKeyword2 = Inscription.Key.Strike;

            StandardIssueMiningPickAbility ability = new()
            {
                _isEvolved = true,
                _minGold = 2,
                _maxGold = 4,
                _stat = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.2)
                ])
            };

            item.abilities = [
                ability
            ];

            item.forbiddenDrops = ["Custom-StandardIssueMiningPick"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GazingEyeBrooch";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Gazing Eye Brooch";
            item.itemName_KR = "응시하는 눈 브로치";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 25%.\n"
                                    + "Amplifies damage dealt to enemies under the Duel effect by 10%.\n"
                                    + "Decreases damage dealt by enemies under the Duel effect by 10%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 25% 증가합니다.\n"
                                    + "결투 각인의 효과를 받고 있는 적에게 입히는 데미지가 10% 증폭됩니다.\n"
                                    + "결투 각인의 효과를 받고 있는 적이 입히는 데미지가 10% 감소합니다.";

            item.itemLore_EN = "Once the eye of a ancient Demon King, now glares into the soul of many to see their fears and sins.";
            item.itemLore_KR = "한 때 고대 마왕의 눈이었던 것이, 이제는 당신의 영혼에 담겨있는 공포와 죄를 바라보고 있다.";

            item.prefabKeyword1 = Inscription.Key.Sin;
            item.prefabKeyword2 = Inscription.Key.Duel;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.25),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.25)
            ]);

            item.abilities = [
                new GazingEyeBroochAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ManaFountain";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Mana Fountain";
            item.itemName_KR = "마나 분수";

            item.itemDescription_EN = "Every time you deal skill damage, reduce all skill cooldowns by 2%.";

            item.itemDescription_KR = "스킬로 데미지를 입힐 시 모든 스킬의 쿨타임이 2% 감소합니다.";

            item.itemLore_EN = "A limitless fountain of mana";
            item.itemLore_KR = "영원히 마르지 않는 마나의 분수";

            item.prefabKeyword1 = Inscription.Key.ManaCycle;
            item.prefabKeyword2 = Inscription.Key.Heirloom;

            item.abilities = [
                new ManaFountainAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "AttendantsCuirass";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Attendant's Cuirass";
            item.itemName_KR = "수행자의 흉갑";

            item.itemDescription_EN = "Increases concentration speed by 45%.\n"
                                    + "Upon being hit, increases concentration speed and <color=#F25D1C>Physical Attack</color> by 45% for 12 seconds.";

            item.itemDescription_KR = "정신집중 속도가 45% 증가합니다.\n"
                                    + "피격 시 정신집중 속도 및 <color=#F25D1C>물리공격력</color>이 12초 동안 45% 증가합니다.";

            item.itemLore_EN = "Do not disturb me of my meditation, you will regret it";
            item.itemLore_KR = "내 명상을 방해하면 후회하게 될거야";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Revenge;

            item.stats = new(
            [
                new(Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, 0.45)
            ]);

            item.abilities = [
                new AttendantsCuirassAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SagittariussChakram";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Sagittarius's Chakram";
            item.itemName_KR = "사수자리의 차크람";

            item.itemDescription_EN = "Any enemy under 70% HP is subjected to Sagittarius's rage, amplifying damage dealt to that enemy by 15% and slowing the enemy by 10%.\n"
                                    + "This effect is further increased to 20% amplification and 15% slow to any enemy under 25% HP.";

            item.itemDescription_KR = "적 체력이 70% 이하일 때, 그 적에게 입히는 데미지가 15% 증폭되며 해당 적이 10% 느려집니다.\n"
                                    + "적 체력이 25% 이하일 때, 해당 효과가 입히는 데미지 20% 증폭 및 15% 감속으로 변합니다.";

            item.itemLore_EN = "Well, well, well... Look at what the cat dragged in...\nA weakling. Poor little weakling.";
            item.itemLore_KR = "이게 누구야... 완전 약골이잖아? 참... 불쌍하다.";

            item.prefabKeyword1 = Inscription.Key.Execution;
            item.prefabKeyword2 = Inscription.Key.Heirloom;

            item.abilities = [
                new SagittariussChakramAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GryphonsFeather";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Gryphon's Feather";
            item.itemName_KR = "그리폰의 깃털";

            item.itemDescription_EN = "Increases jump height, dash cooldown speed, and dash distance by 15%.";

            item.itemDescription_KR = "점프 높이, 대쉬 쿨다운 속도 및 대쉬 거리가 15% 증가합니다.";

            item.itemLore_EN = "Many adventurers forget that it takes a thousand feathers to master the sky like a gryphon.";
            item.itemLore_KR = "많은 모험가들이 이 깃털에 하늘을 나는 힘이 깃들어 있다고 말하자만, 그건 사실이 아니다.";

            item.prefabKeyword1 = Inscription.Key.Soar;
            item.prefabKeyword2 = Inscription.Key.Chase;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.DashCooldownSpeed, 0.15),
                new(Stat.Category.Percent, Stat.Kind.DashDistance, 1.15)
            ]);

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "HappiestMask";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Happiest Mask";
            item.itemName_KR = "환희의 가면";

            item.itemDescription_EN = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n"
                                    + "Increases damage dealt to enemies with summons by 30%.";

            item.itemDescription_KR = "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가합니다.\n"
                                    + "소환물들이 적에게 입히는 데미지가 30% 증가합니다.";

            item.itemLore_EN = "I'm happy! I'M HAPPY! SEE! I SAID I'M HAPPY PLEASE DON'T.";
            item.itemLore_KR = "난 행복해! 세상에서 제일 행복하다고! 행복하다니까 제발 그만-";

            item.prefabKeyword1 = Inscription.Key.ManaCycle;
            item.prefabKeyword2 = Inscription.Key.Relic;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.3)
            ]);

            item.abilities = [
                new HappiestMaskAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BloomingEden";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Blooming Eden";
            item.itemName_KR = "만개하는 낙원";
            
            item.itemDescription_EN = "Critical hits increase Attack Speed by 10% for 4 seconds (maximum 50%).";

            item.itemDescription_KR = "치명타 시 4초 동안 공격속도가 10% 증가합니다 (최대 50%).";

            item.itemLore_EN = "By my rapier, I shall protect the buds of Eden until the very last one has bloomed!";
            item.itemLore_KR = "이 레이피어로, 낙원의 마지막 꽃이 필 때까지 이 꽃봉우리들을 보호하겠어!";

            item.prefabKeyword1 = Inscription.Key.Misfortune;
            item.prefabKeyword2 = Inscription.Key.Rapidity;

            item.abilities = [
                new BloomingEdenAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Shieldbone";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Shieldbone";
            item.itemName_KR = "수호의 뼈";

            item.itemDescription_EN = "Ampilifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by up to 40%, equal to the amount of Shields you have.";

            item.itemDescription_KR = "보유한 방어막의 양에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 최대 40%까지 증폭됩니다.";

            item.itemLore_EN = "The bone of a nameless warrior, defender of a castle fallen during its siege.";
            item.itemLore_KR = "최후의 순간까지 성루를 지키던 이름 없는 전사의 뼈";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.abilities = [
                new ShieldboneAbility()
                {
                    _isUpgraded = false,
                    _maxStacks = 40
                }
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Shieldbone_BoneUpgrade";
            item.rarity = Rarity.Rare;

            item.obtainable = false;

            item.itemName_EN = "Bone of Eternity: Shield";
            item.itemName_KR = "영원의 뼈: 수호";

            item.itemDescription_EN = "Ampilifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by up to 60%, equal to the amount of Shields you have.\n"
                                    + "Activating a Bone insctipion effect will reduce damage taken by 30% for 15 seconds.";

            item.itemDescription_KR = "보유한 방어막의 양에 비례하여 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 최대 60%까지 증폭됩니다.\n"
                                    + "뼈 각인 효과 발동 시 15초간 받는 데미지가 30% 감소합니다.";

            item.itemLore_EN = "The bone of a nameless warrior, defender of a castle fallen during its siege.";
            item.itemLore_KR = "최후의 순간까지 성루를 지키던 이름 없는 전사의 뼈";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.Fortress;
            
            item.abilities = [
                new ShieldboneAbility()
                {
                    _isUpgraded = true,
                    _maxStacks = 60
                }
            ];

            item.forbiddenDrops = ["Custom-Shieldbone"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "DiaryOfAnOldMaster";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Diary of an Old Master";
            item.itemName_KR = "누군가의 낡은 일기";

            item.itemDescription_EN = "Reduces Crit Rate by 10%.\n"
                                    + "Reduces Crit Rate by 25% when having 2 of the following items: Invisible Knife, Gunpowder Sword, Crown of Thorns, Bloody Crown of Thorns.\n"
                                    + "Items can now deal critical hits.\n"
                                    + "This item gains two of the same random inscription amogst these inscriptions: Hidden Blade, Heritage, Strike, Relic.";

            item.itemDescription_KR = "치명타 확률이 10% 감소합니다.\n"
                                    + "해당 아이템들 중 2개 이상 소지할 시 치명타 확률이 25% 감소합니다: 보이지 않는 검, 화약검, 면류관, 핏빛 면류관.\n"
                                    + "아이템으로 입힌 데미지가 치명타로 가해질 수 있습니다.\n"
                                    + "이 아이템은 해당 각인들 중 무작위의 같은 각인 두개를 보유합니다: 암기, 고대의 힘, 강타, 성물.";

            item.itemLore_EN = "I don't know whose diary this is, but if I were him I'd take care of it, at least I wouldn't lose it.";
            item.itemLore_KR = "누가 쓴 일기인지는 모르겠지만, 내가 주인이었다면 잃어버리지 않게 잘 보관했을 텐데";

            item.prefabKeyword1 = Inscription.Key.None;
            item.prefabKeyword2 = Inscription.Key.None;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, -0.1)
            ]);

            item.abilities = [
                new DiaryOfAnOldMasterAbility()
            ];

            item.extraComponents = [
                typeof(DiaryOfAnOldMasterRandomInscriptionBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SteelAegis";
            item.rarity = Rarity.Common;

            item.itemName_EN = "Steel Aegis";
            item.itemName_KR = "강철 아이기스";

            item.itemDescription_EN = "Upon being hit, no damage will be taken (twice per room).";

            item.itemDescription_KR = "피격 시 데미지를 입지 않습니다 (방마다 2회 제한).";

            item.itemLore_EN = "My shield shall protect me from anything you throw at me! At least, the first few.";
            item.itemLore_KR = "네가 무슨 수를 써도 이 방패는 못 뚫을걸?\n... 근데 이거 왜 이렇게 내구도가 낮지?";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Brawl;

            item.abilities = [
                new SteelAegisAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CursedDaggers";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: Cursed Daggers";
            item.itemName_KR = "흉조: 저주받은 단도들";

            item.itemDescription_EN = "All Crit Rate that exceeds 40% is converted into Crit Damage.";

            item.itemDescription_KR = "치명타 확률이 40% 이상일 때, 40%를 초과한 치명타 확률은 모두 치명타 데미지로 치환됩니다.";

            item.itemLore_EN = "A dagger cursed from the former user to weaken beings that use it after him... If he cursed it right";
            item.itemLore_KR = "본래 주인 이외에 사용하는 이들을 취약하게 만드는 저주가 걸려있는 단검들.\n허나 많은 사람들이 저주가 아닌 축복으로 치부하고 있다.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Strike;

            item.abilities = [
                new CursedDaggersAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SwordOfTheToxicMoon";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Sword of the Toxic Moon";
            item.itemName_KR = "염월도";

            item.itemDescription_EN = "Normal Attacks have a 10% chance to inflict Poison.\n"
                                    + "Increases Attack Speed by 20%.\n"
                                    + "Upon inflicting Poison, increases <color=#F25D1C>Physical Attack</color> by 5% for 10 seconds (maximum 40%).";

            item.itemDescription_KR = "적에게 기본공격 시 10% 확률로 중독을 부여합니다.\n"
                                    + "공격속도가 20% 증가합니다.\n"
                                    + "중독을 부여할 시, <color=#F25D1C>물리공격력</color>이 10초 동안 5% 증가합니다 (최대 40%).";

            item.itemLore_EN = "A deadly sword made from the body of the poison beast Neflaav";
            item.itemLore_KR = "맹독 괴수 네플라브의 몸에서 연성된 모두를 두려움에 떨게 하는 검";

            item.prefabKeyword1 = Inscription.Key.Poisoning;
            item.prefabKeyword2 = Inscription.Key.Rapidity;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.2)
            ]);

            var applyStatus = new ApplyStatusOnGaveDamage();
            var status = Kind.Poison;
            applyStatus._cooldownTime = 0.1f;
            applyStatus._chance = 10;
            applyStatus._attackTypes = new();
            applyStatus._attackTypes[MotionType.Basic] = true;

            applyStatus._types = new();
            applyStatus._types[AttackType.Melee] = true;
            applyStatus._types[AttackType.Ranged] = true;
            applyStatus._types[AttackType.Projectile] = true;

            applyStatus._status = new ApplyInfo(status);

            item.abilities = [
                new SwordOfTheToxicMoonAbility(),
                applyStatus
            ];

            item.extraComponents = [
                typeof(SwordOfTheToxicMoonEvolveBehavior),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "SwordOfTheToxicMoon_2";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "Scythe of the Twin Moons";
            item.itemName_KR = "이월의 낫";

            item.itemDescription_EN = "Normal Attacks have a 15% chance to inflict Poison.\n"
                                    + "Skills have a 15% chance to inflict Freeze.\n"
                                    + "Increases Attack Speed by 20%.\n"
                                    + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 60%.\n"
                                    + "When a Freeze status ailment is canceled, there's a 20% chance of inflicting Poison.\n"
                                    + "When a Poison status ailment is canceled, there's a 10% chance of inflicting Freeze.";

            item.itemDescription_KR = "적에게 기본공격 시 15% 확률로 중독을 부여합니다.\n"
                                    + "적에게 스킬로 공격 시 15% 확률로 빙결을 부여합니다.\n"
                                    + "공격속도가 20% 증가합니다.\n"
                                    + "<color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 60% 증가합니다.\n"
                                    + "빙결 상태이상이 종료될 시 20% 확률로 중독을 부여합니다.\n"
                                    + "중독 상태이상이 종료될 시 10% 확률로 빙결을 부여합니다.";

            item.itemLore_EN = "When the Frozen Moon and the Toxic Moon rise, the fusion of their powers appear where their lights collide";
            item.itemLore_KR = "얼어붙은 달과 오염된 달이 함께 뜰 때, 두 빛의 충돌점에 위대한 힘이 탄생할지니";

            item.prefabKeyword1 = Inscription.Key.AbsoluteZero;
            item.prefabKeyword2 = Inscription.Key.Poisoning;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.2),
                new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.6),
                new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.6)
            ]);

            var applyPoison = new ApplyStatusOnGaveDamage();
            var applyFreeze = new ApplyStatusOnGaveDamage();
            var poison = Kind.Poison;
            var freeze = Kind.Freeze;

            applyPoison._cooldownTime = 0.1f;
            applyPoison._chance = 15;
            applyPoison._attackTypes = new();
            applyPoison._attackTypes[MotionType.Basic] = true;

            applyPoison._types = new();
            applyPoison._types[AttackType.Melee] = true;
            applyPoison._types[AttackType.Ranged] = true;
            applyPoison._types[AttackType.Projectile] = true;

            applyFreeze._cooldownTime = 0.1f;
            applyFreeze._chance = 15;
            applyFreeze._attackTypes = new();
            applyFreeze._attackTypes[MotionType.Skill] = true;

            applyFreeze._types = new();
            applyFreeze._types[AttackType.Melee] = true;
            applyFreeze._types[AttackType.Ranged] = true;
            applyFreeze._types[AttackType.Projectile] = true;

            applyPoison._status = new ApplyInfo(poison);
            applyFreeze._status = new ApplyInfo(freeze);

            var reApplyPoison = new ApplyStatusOnApplyStatus();
            reApplyPoison._chance = 20;
            reApplyPoison._timing = Timing.Release;
            reApplyPoison._self = false;
            reApplyPoison._kinds = [freeze];
            reApplyPoison._status = new ApplyInfo(poison);

            var reApplyFreeze = new ApplyStatusOnApplyStatus();
            reApplyFreeze._chance = 10;
            reApplyFreeze._timing = Timing.Release;
            reApplyFreeze._self = false;
            reApplyFreeze._kinds = [poison];
            reApplyFreeze._status = new ApplyInfo(freeze);

            item.abilities = [
                reApplyPoison,
                reApplyFreeze,
                applyPoison,
                applyFreeze
            ];

            item.forbiddenDrops = ["Custom-FrozenSpear", "Custom-SwordOfTheToxicMoon"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BoneOfRandomness";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "B0n3 0f R4nD0mn3ss";
            item.itemName_KR = "무작위의 뼈";

            item.itemDescription_EN = "This item's second inscription is randomly chosen.\n"
                                    + "Enhances a random Bone item you have and disappears.\n"
                                    + "When not having a Bone item, drops a random Bone item and disappears.\n"
                                    + "When dropping a random Bone item, there is a small chance a Bone item will not be dropped and this item will remain.\n"
                                    + "Every 25 seconds, amplify a random stat by 25% for 15 seconds.";

            item.itemDescription_KR = "이 아이템은 임의의 두번째 각인을 갖습니다.\n"
                                    + "가지고 있는 임의의 뼈 아이템을 강화하고 사라집니다.\n"
                                    + "뼈 아이템을 소지하고 있지 않을 시 임의의 뼈 아이템을 떨구고 사라집니다.\n"
                                    + "뼈 아이템을 떨굴 때 적은 확률로 아이템을 떨구지 않고 이 아이템이 사라지지 않습니다.\n"
                                    + "25초 마다 임의의 능력치 하나가 15초 동안 25% 증폭됩니다.";

            item.itemLore_EN = "Is it this? Or This? How about this?";
            item.itemLore_KR = "이건가? 아니면 저거? 요건 어때?";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.None;

            item.abilities = [
                new BoneOfRandomnessAbility()
            ];

            item.extraComponents = [
                typeof(BoneOfRandomnessRandomInscriptionBehavior),
                typeof(BoneOfRandomnessActivateBehavior)
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BoneOfRandomness_BoneUpgrade";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "B0n3 0f 3t3rn1ty: Randomness";
            item.itemName_KR = "영원의 뼈: 무작위";

            item.itemDescription_EN = "Every 25 seconds, amplify a random stat by 25% for 15 seconds.\n"
                                    + "Activating a Bone insctipion effect will increase the amplfication to 35% for the current amplification.";

            item.itemDescription_KR = "25초 마다 임의의 능력치 하나가 15초 동안 25% 증폭됩니다.\n"
                                    + "뼈 각인 효과 발동 시 이번 능력치 증폭의 양이 35%로 증가합니다.";

            item.itemLore_EN = "Is it this? Or This? How about this?";
            item.itemLore_KR = "이건가? 아니면 저거? 요건 어때?";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.None;

            item.abilities = [
                new BoneOfRandomnessAbility() {
                    _isEvolved = true,
                }
            ];

            item.extraComponents = [
                typeof(BoneOfRandomnessRandomInscriptionBehavior)
            ];

            item.forbiddenDrops = ["Custom-BoneOfRandomness"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "BrokenWatch";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Broken Watch";
            item.itemName_KR = "부러진 손목시계";

            item.itemDescription_EN = "Upon using a skill, increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30% and decreases skill cooldown speed by 20% for 5 seconds (stacks infinitely).\n"
                                    + "The effect does not activate if your skill cooldown speed stat is 20% or below.\n"
                                    + "If your skill cooldown speed is lower than 20%, decrease stacks until your skill cooldown speed is 20% or higher.";

            item.itemDescription_KR = "스킬 사용 시 5초 동안 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가하고 스킬 쿨다운 속도가 20% 감소합니다 (무한으로 중첩 가능).\n"
                                    + "스킬 쿨다운 속도 수치가 20% 이하일 때 이 효과는 발동하지 않습니다.\n"
                                    + "만약 스킬 쿨다운 속도가 20% 미만이라면 스킬 쿨다운 속도가 20% 이상이 될 때 까지 이 아이템의 중첩 수가 감소합니다.";

            item.itemLore_EN = "Cling to the past all you want, you'll have to face the present eventually.";
            item.itemLore_KR = "네가 아무리 과거에 매달려 있어도 결국 현재를 마주하게 될거야.";

            item.prefabKeyword1 = Inscription.Key.Manatech;
            item.prefabKeyword2 = Inscription.Key.Antique;

            item.abilities = [
                new BrokenWatchAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RingTarget";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Ring Target";
            item.itemName_KR = "원형 표적";

            item.itemDescription_EN = "Amplifies damage dealt to enemies with a projectile by 30%.";

            item.itemDescription_KR = "적에게 투사체로 입히는 데미지가 30% 증폭됩니다.";

            item.itemLore_EN = "Nice shot.";
            item.itemLore_KR = "네가 과연 이걸 맞출 수 있을까?";

            item.prefabKeyword1 = Inscription.Key.Strike;
            item.prefabKeyword2 = Inscription.Key.Brawl;

            item.abilities = [
                new RingTargetAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "HelixBrooch";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Helix Brooch";
            item.itemName_KR = "나선형 브로치";

            item.itemDescription_EN = "Hitting enemies have a 1.618% chance to gain a Helix for 8 seconds (infinitely stackable).\n"
                                    + "Upon dealing damage to an enemy, deal 5 true damage for each Helix you have.";

            item.itemDescription_KR = "적에게 데미지를 입힐 시 1.618% 확률로 8초 동안 나선을 하나 획득합니다 (무한으로 중첩 가능).\n"
                                    + "적에게 데미지를 입힐 시 보유중인 나선 하나당 5의 고정 데미지를 추가로 입힙니다.";

            item.itemLore_EN = "A mysterious golden brooch that spirals into infinity";
            item.itemLore_KR = "무한을 향해 영원히 돌아가는 금색 브로치";

            item.prefabKeyword1 = Inscription.Key.Heirloom;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.abilities = [
                new HelixBroochAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "PowerHalberd";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Power Halberd";
            item.itemName_KR = "힘의 창";

            item.itemDescription_EN = "Increases Arms damage by 35%.\n"
                                    + "Hitting an enemy with Arms heals 2 HP (cooldown: 1.5 seconds).";

            item.itemDescription_KR = "적에게 무구로 입히는 데미지가 35% 증가합니다.\n"
                                    + "적에게 무구를 휘두를 시 체력을 2 회복합니다 (쿨타임: 1.5초).";

            item.itemLore_EN = "The legends speak of one Adeptus Astartes. One of the Salamander chapter and used a weapon like no other. He was unkown, and it shall stay that way.";
            item.itemLore_KR = "먼 옛날 한 아답투스 아스타르테스가 있었다. 그는 샐러맨더 챕터의 일원이며, 무기를 그 누구보다 더 잘 다뤘다고 한다. 그의 신원은 알 수 없으며, 앞으로도 알 수 없을 것이다.";

            item.prefabKeyword1 = Inscription.Key.Relic;
            item.prefabKeyword2 = Inscription.Key.Arms;

            item.abilities = [
                new PowerHalberdAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ShadowThiefsSack";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Shadow Thief's Sack";
            item.itemName_KR = "그림자 도둑의 보따리";

            item.itemDescription_EN = "When purchasing a good from the collector, there's a 15~25% chance to gain the same amount of gold spent on the purchased good.\n"
                                    + "Upon each success, the price of goods sold by the collector increases by 10%.\n"
                                    + "The increased price resets upon entering the Black Market.";

            item.itemDescription_KR = "수집가한테서 물품을 구입할 때, 15~25% 확률로 구매한 물품의 가격을 그대로 돌려받습니다.\n"
                                    + "가격을 한번 돌려받을 때마다 수집가가 판매하는 물품의 가격이 10% 상승합니다.\n"
                                    + "상승된 물품의 가격은 암시장 입장 시 초기화됩니다.";

            item.itemLore_EN = "Boo! Boo I say! I'm gonna take these items anyway, see?!";
            item.itemLore_KR = "야! 여기! 이제 이 아이템들은 다 내꺼야, 알아 들어?!";

            item.prefabKeyword1 = Inscription.Key.Treasure;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.abilities = [
                new ShadowThiefsSackAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Tetronimo";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "Tetronimo";
            item.itemName_KR = "테트로니모";

            item.itemDescription_EN = "This item randomly gains 2 inscriptions.\n"
                                    + "Upon having 4 items in your inventory with the same inscription, any item in your inventory with that inscription are immediately destroyed (other than Tetronimo).\n"
                                    + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30% for every item destroyed via this item.";

            item.itemDescription_KR = "이 아이템은 임의의 각인들을 갖습니다.\n"
                                    + "현재 인벤토리 내에 같은 각인을 보유한 아이템이 4개 이상 있을 시, 해당 각인을 보유한 아이템들이 모두 파괴됩니다 (테트로니모 제외).\n"
                                    + "이 아이템으로 파괴된 아이템 하나당 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가합니다.";

            item.itemLore_EN = "Where is that line piece when you need it?";
            item.itemLore_KR = "작대기 블록이 어디갔지?";

            item.prefabKeyword1 = Inscription.Key.None;
            item.prefabKeyword2 = Inscription.Key.None;

            item.abilities = [
                new TetronimoAbility() {
                    _isEvolved = false,
                }
            ];

            item.extraComponents = [
                typeof(KeywordRandomizer)
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Tetronimo_BoneUpgrade";
            item.rarity = Rarity.Legendary;

            item.obtainable = false;

            item.itemName_EN = "T-Bone";
            item.itemName_KR = "T-본";

            item.itemDescription_EN = "This item's second inscription is randomly chosen.\n"
                                    + "Upon having 4 items in your inventory with the same inscription, any item in your inventory with that inscription are immediately destroyed (other than Tetronimo).\n"
                                    + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30% for every item destroyed via this item.\n"
                                    + "Activating a Bone insctipion effect will additionally increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15% for every item destroyed via this item. for 15 seconds.";

            item.itemDescription_KR = "이 아이템은 임의의 두번째 각인을 갖습니다.\n"
                                    + "현재 인벤토리 내에 같은 각인을 보유한 아이템이 4개 이상 있을 시, 해당 각인을 보유한 아이템들이 모두 파괴됩니다 (테트로니모 제외).\n"
                                    + "이 아이템으로 파괴된 아이템 하나당 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 30% 증가합니다.\n"
                                    + "뼈 각인 효과 발동 시 15초간 이 아이템으로 파괴된 아이템 하나당 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 추가로 15% 증가합니다.";

            item.itemLore_EN = "Time for a T-Spin";
            item.itemLore_KR = "T-스핀 할 시간이야";

            item.prefabKeyword1 = Inscription.Key.Bone;
            item.prefabKeyword2 = Inscription.Key.None;

            item.abilities = [
                new TetronimoAbility() {
                    _isEvolved = true,
                }
            ];

            item.extraComponents = [
                typeof(BoneOfRandomnessRandomInscriptionBehavior)
            ];

            item.forbiddenDrops = ["Custom-Tetronimo"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "DemomansBottle";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Demoman's Bottle";
            item.itemName_KR = "데모맨의 화염병";

            item.itemDescription_EN = "For every 5 Shields in possesion, skills and basic attacks have a 2% chance to inflict Poison to enemies.";

            item.itemDescription_KR = "보유중인 방어막 5 당 스킬과 기본공격이 적에게 중독을 부여할 확률이 2% 증가합니다.";

            item.itemLore_EN = "Does this have whisky in? Or was it nitroglycerine?";
            item.itemLore_KR = "여기에 위스키가 들었었나? 아니면 니트로글리세린이었나?";

            item.prefabKeyword1 = Inscription.Key.Poisoning;
            item.prefabKeyword2 = Inscription.Key.Fortress;

            item.abilities = [
                new DemomansBottleAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ForgottenCompanyHelmet";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Forgotten Company Helmet";
            item.itemName_KR = "잊혀진 회사의 헬멧";

            item.itemDescription_EN = "Upon spending Gold, gain 10% of Gold spent.\n"
                                    + "Increases Gold gain by 25%.";

            item.itemDescription_KR = "금화를 소비할 때 소비한 금화의 10% 만큼 금화를 획득합니다.\n"
                                    + "금화 획득량이 25% 증가합니다.";

            item.itemLore_EN = "We're scrappy and resiliant, we're happy and we're diligent. But that's just if you're listening to marketing transmissions.";
            item.itemLore_KR = "저희 회사는 다인의 의사와 행동을 존중합니다. 명령만 잘 따른다면 말이죠.";

            item.prefabKeyword1 = Inscription.Key.Treasure;
            item.prefabKeyword2 = Inscription.Key.Misfortune;

            item.abilities = [
                new ForgottenCompanyHelmetAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "Damocles";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: Damocles";
            item.itemName_KR = "흉조: 다모클레스";

            item.itemDescription_EN = "Decreases Gold gain by 30%.\n"
                                    + "Every item purchased from the Black Market grants another random item to drop.\n"
                                    + "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 100% and damage taken by 20% for every 20 items destroyed.";

            item.itemDescription_KR = "금화 획득량이 30% 감소합니다.\n"
                                    + "암시장에서 아이템을 구매할 때 마다 임의의 다른 아이템이 떨궈집니다.\n"
                                    + "아이템을 20개 파괴할 때마다 <color=#F25D1C>물리공격력</color> 및 <color=#1787D8>마법공격력</color>이 100% 증가하고 받는 데미지가 20% 증가합니다.";

            item.itemLore_EN = "The wealth of a king, but at what cost?";
            item.itemLore_KR = "왕의 부를 얻기 위한 대가";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.Mystery;

            item.abilities = [
                new DamoclesAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TheEndlessCycle";
            item.rarity = Rarity.Legendary;

            item.itemName_EN = "The Endless Cycle";
            item.itemName_KR = "무한의 굴레";

            item.itemDescription_EN = "Swapping has a 20% chance to reset swap cooldown.\n"
                                    + "Upon swapping 16 times, the chance of resetting is increased to 40% for 15 seconds.";

            item.itemDescription_KR = "교대 시 20% 확률로 교대 쿨타임이 초기화됩니다.\n"
                                    + "16번째로 교대할 시 15초 동안 교대 쿨타임이 초기화될 확률이 40%로 증가합니다.";

            item.itemLore_EN = "I will use this cycle to obtain endless knowledge and look at the Demiurge in the eye!";
            item.itemLore_KR = "이 굴레를 이용해 무한한 지식을 얻어 창조자를 목도할 테다!";

            item.prefabKeyword1 = Inscription.Key.Mutation;
            item.prefabKeyword2 = Inscription.Key.Mystery;

            item.abilities = [
                new TheEndlessCycleAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CraftsmansChisel";
            item.rarity = Rarity.Rare;

            item.itemName_EN = "Craftsman's Chisel";
            item.itemName_KR = "장인의 끌";

            item.itemDescription_EN = "Taking damage has a 35% chance to reduce damage taken by 1% (maximum 15%).\n"
                                    + "Upon reaching 15% damage reduction via this item, increases <color=#F25D1C>Physical Attack</color> by 40%.";

            item.itemDescription_KR = "피격 시 35% 확률로 받는 데미지가 1% 감소합니다 (최대 15%).\n"
                                    + "이 아이템의 받는 피해 감소량이 15%가 될 때, <color=#F25D1C>물리공격력</color>이 40% 증가합니다.";

            item.itemLore_EN = "You're not quite sure how to use it but you keep trying anyway.";
            item.itemLore_KR = "사용법을 모른다고 해도 절대 시도를 멈추지 마.";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.abilities = [
                new CraftsmansChiselAbility() {
                    _isEvolved = false
                }
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "CraftsmansChisel_2";
            item.rarity = Rarity.Rare;

            item.obtainable = false;

            item.itemName_EN = "Master Craftsman's Chisel";
            item.itemName_KR = "유일무이한 달인의 끌";

            item.itemDescription_EN = "Taking damage has a 35% chance to reduce damage taken by 1% (maximum 25%).\n"
                                    + "Upon reaching 25% damage reduction via this item, increases <color=#F25D1C>Physical Attack</color> by 40%.\n"
                                    + "Upon reaching 25% damage reduction via this item, taking damage increases <color=#F25D1C>Physical Attack</color> by 60% for 8 seconds.";

            item.itemDescription_KR = "피격 시 35% 확률로 받는 데미지가 1% 감소합니다 (최대 25%).\n"
                                    + "이 아이템의 받는 피해 감소량이 25%가 됐을 시 <color=#F25D1C>물리공격력</color>이 40% 증가합니다.\n"
                                    + "이 아이템의 받는 피해 감소량이 25%가 됐을 때 피격 시 8초 동안 <color=#F25D1C>물리공격력</color>이 60% 증가합니다.";

            item.itemLore_EN = "Like a marble statue chiseled away by countless strikes, the mind and body is sculpted through adversity.";
            item.itemLore_KR = "수없이 마모된 석상처럼, 정신과 육체는 고난과 역경을 통해 조각된다.";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.abilities = [
                new CraftsmansChiselAbility() {
                    _isEvolved = true
                }
            ];

            item.forbiddenDrops = ["Custom-CraftsmansChisel"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "ManatechSequenceBreaker";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Manatech Sequence Breaker";
            item.itemName_KR = "마공학 인과 재구성기";

            item.itemDescription_EN = "Hitting an enemy slows them by 50% for 2.5 seconds (cooldown: 5 seconds).";

            item.itemDescription_KR = "적 공격 시 해당 적이 2.5초 동안 50% 느려집니다 (쿨타임: 5초).";

            item.itemLore_EN = "Product ID: 654, class: time manipulation, current status: inactive, notes: avoid interaction, or you may trigger a time slowdown.";
            item.itemLore_KR = "제품 ID: 654, 종류: 시간 조종, 현재 상태: 비활성, 주의: 섣부른 상호작용은 시간 감속을 일으킬 수 있습니다.";

            item.prefabKeyword1 = Inscription.Key.Rapidity;
            item.prefabKeyword2 = Inscription.Key.Manatech;

            item.abilities = [
                new ManatechSequenceBreakerAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "MonksBracers";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Monk's Bracers";
            item.itemName_KR = "수도승의 보호구";

            item.itemDescription_EN = "Increases Attack Speed by 15%.\n"
                                    + "Normal attacks amplify basic attack damage dealt to enemies by 2% for 3 seconds (maximum 10%).";

            item.itemDescription_KR = "공격속도가 15% 증가합니다.\n"
                                    + "적에게 기본공격 시 적에게 기본공격으로 입히는 데미지가 3초 동안 2% 증폭됩니다 (최대 10%).";

            item.itemLore_EN = "While Monks in the eastern kingdom were not known for violence, Carleon's invasion forced their hands.";
            item.itemLore_KR = "동쪽 나라의 수도승들은 본래 평화를 추구했지만, 칼레온의 침략은 이들에게 선택지를 주지 않았다.";

            item.prefabKeyword1 = Inscription.Key.Rapidity;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.15),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.15)
            ]);

            item.abilities = [
                new MonksBracersAbility() {
                    _isEvolved = false
                }
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "MonksBracers_2";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Monk's Tiger Claw Bracers";
            item.itemName_KR = "수도승의 호랑이 발톱 보호구";

            item.obtainable = false;

            item.itemDescription_EN = "Increases Attack Speed by 30%.\n"
                                    + "Normal attacks amplify basic attack damage dealt to enemies by 2% for 3 seconds (maximum 20%).\n"
                                    + "Enables Auto-Swing (when using a normal attack, you can now hold down the attack to continuously use the normal attack).";

            item.itemDescription_KR = "공격속도가 30% 증가합니다.\n"
                                    + "적에게 기본공격 시 적에게 기본공격으로 입히는 데미지가 3초 동안 2% 증폭됩니다 (최대 20%).\n"
                                    + "자동 공격이 활성화됩니다 (기본공격 키를 꾹 눌러 계속 기본공격을 할 수 있습니다).";

            item.itemLore_EN = "Monks that survived the Carleon invasion of the Eastern Kingdom have improved their striking speed and strength to that of tigers.";
            item.itemLore_KR = "칼레온 침략을 살아남은 수도승들은 그들의 민첩함과 힘을 호랑이에 버금가는 수준으로 성장했다.";

            item.prefabKeyword1 = Inscription.Key.Rapidity;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

            item.stats = new([
                new(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.3),
                new(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.3)
            ]);

            item.abilities = [
                new MonksBracersAbility() {
                    _isEvolved = true
                }
            ];

            item.forbiddenDrops = ["Custom-MonksBracers"];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "MakeshiftHelmet";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Makeshift Helmet";
            item.itemName_KR = "조잡한 투구";

            item.itemDescription_EN = "Decreases damage taken from enemies far away by 50%.";

            item.itemDescription_KR = "멀리 있는 적에게 받는 데미지가 50% 감소합니다.";

            item.itemLore_EN = "Such is Life";
            item.itemLore_KR = "이 또한 삶일지니";

            item.prefabKeyword1 = Inscription.Key.Fortress;
            item.prefabKeyword2 = Inscription.Key.Mystery;

            item.abilities = [
                new MakeshiftHelmetAbility()
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "TheSecretOfTheKing";
            item.rarity = Rarity.Unique;

            item.gearTag = Gear.Tag.Omen;
            item.obtainable = false;

            item.itemName_EN = "Omen: The Secret of the King";
            item.itemName_KR = "흉조: 왕의 비밀";

            item.itemDescription_EN = "For 1 second after swapping a skull, attacks have a 10% chance to apply a random status effect.\n"
                                    + "Statuses can now deal critical hits.\n"
                                    + "This item's second inscription is randomly chosen between Poisoning, Excessive Bleeding, Dizziness, Abosulte Zero, and Arson.";

            item.itemDescription_KR = "교대 시 1초 동안 적 공격 시 10% 확률로 임의의 상태이상을 부여합니다.\n"
                                    + "상태이상으로 입힌 데미지가 치명타로 가해질 수 있습니다.\n"
                                    + "이 아이템의 두번째 각인은 독살, 과다출혈, 현기증, 절대영도, 그리고 방화 중에서 임의로 정해집니다.";

            item.itemLore_EN = "What is it, a cell? No one can reliably answer that question, except the king....";
            item.itemLore_KR = "국왕만이 이 정체불명의 세포의 용도를 알고 있다.";

            item.prefabKeyword1 = Inscription.Key.Omen;
            item.prefabKeyword2 = Inscription.Key.None;

            item.abilities = [
                new TheSecretOfTheKingAbility()
            ];

            item.extraComponents = [
                typeof(TheSecretOfTheKingRandomInscriptionBehavior)
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GoldenMegaphone";
            item.rarity = Rarity.Unique;

            item.itemName_EN = "Golden Megaphone";
            item.itemName_KR = "황금 메가폰";

            item.itemDescription_EN = "Upon entering a map or hitting a new boss phase, the item becomes active.\n"
                                    + "Upon hitting an enemy while the item is active, deactivates the item and completely stop them for 10 seconds.";

            item.itemDescription_KR = "맵 입장 혹은 보스 페이즈에게 처음 데미지를 줄 시 해당 아이템이 활성화됩니다.\n"
                                    + "이 아이템이 활성화됐을 때 적 공격 시 아이템이 비활성화되며 해당 적은 10초 동안 움직일 수 없게 됩니다.";

            item.itemLore_EN = "THIS! IS THE VOICE! OF THE COMMUNITY!!!\n... Wait was that too loud?";
            item.itemLore_KR = "이게! 커뮤니티의! 목소리다!!!\n... 아 너무 크게 말했나;;";

            item.prefabKeyword1 = Inscription.Key.Dizziness;
            item.prefabKeyword2 = Inscription.Key.Heirloom;

            item.abilities = [
                new GoldenMegaphoneAbility()
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

    internal static List<Bone.SuperAbility.EnhancementMap> ListUpgradableBones()
    {
        var items = Items.ToDictionary(i => i.name);

        return items.Where(item => items.ContainsKey(item.Key + "_BoneUpgrade"))
                    .Select(item => new Bone.SuperAbility.EnhancementMap()
                    {
                        _from = new AssetReference(item.Value.guid),
                        _to = new AssetReference(items[item.Key + "_BoneUpgrade"].guid),
                    })
                    .ToList();
    }
}
