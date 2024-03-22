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
     * Remove phys atk from Flask of Botulism - done
     * Put the CORRECT files for the Heavy-Duty Carleon Helmet - done
     * Remove amp from Goddess's Chalice - done
     * Mana Accelerator 15% -> 10% - done
     * Accursed Sabre 20% -> 10% - done
     * Frozen Spear 2% -> 10% - done
     * Frozen Spear evolution 300 -> 250 - done
     * Resprite Winged Spear line - done
     * Winged Sword -> Solar-Winged Sword - done
     * Winged Insignia -> Lunar-Winged Insignia - done
     * Add item list - done
     * Renamed Behavior file name for Tainted Red Scarf - done
     * Resprite Dream Catcher - done
     * Change description of Beginner's Lance - done
     * Omen: Last Dawn becomes obtainable in the Dev Menu mod - done
     * Volcanic Shard burn duration decrease 10% -> 5% - done
     * Volcanic Shard magic atk 100% -> 80% - done
     */

    private static List<CustomItemReference> InitializeItems()
    {
        List<CustomItemReference> items = new();
        {
            var item = new CustomItemReference();
            item.name = "VaseOfTheFallen";
            item.rarity = Rarity.Unique;

            // EN: Vase of the Fallen
            // KR: ��ȥ�� ��� ���ڱ�
            item.itemName = "Vase of the Fallen";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (stacks up to 200% and 1/2 of total charge is lost when hit).\n
            // Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit (Cooldown: 3 seconds).

            // KR: óġ�� ���� ���� ����Ͽ� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 5% �����մϴ� (�ִ� 200% ����, �ǰݽ� ����ġ�� ������ ������ϴ�).\n
            // �ǰ� �� 1�� ���� �� ���� �� ������ ����ġ�� ������ �ǵ��� �޽��ϴ� (��Ÿ��: 3��).

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (stacks up to 200% and 1/2 of total charge is lost when hit.)\n"
                                 + "Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit. (Cooldown: 3 seconds)";

            // EN: Souls of the Eastern Kingdom's fallen warriors shall aid you in battle.
            // KR: ����� �����ߴ� ���� �ձ��� ������� ȥ�� ��� ����
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
            // KR: ������ ����
            item.itemName = "Broken Heart";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // Increases Quintessence cooldown speed by 30%.\n
            // Amplifies Quintessence damage by 15%.\n
            // If the 'Succubus' Quintessence is in your possession, this item turns into 'Lustful Heart'.

            // KR: <color=#1787D8>�������ݷ�</color>�� 20% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 30% �����մϴ�.\n
            // ������ ������ ������ �������� 15% �����˴ϴ�.\n
            // '��ť����' ���� ���� �� �� �������� '������ ����'���� ���մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                 + "Increases Quintessence cooldown speed by 30%.\n"
                                 + "Amplifies Quintessence damage by 15%.\n"
                                 + "If the Succubus Quintessence is in your possession, this item turns into 'Lustful Heart'.";

            // EN: Some poor being must have their heart torn both metaphorically and literally.
            // KR: ���� ��, ������ ���������ε� ���������ε� �������ٴ�.
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
            // KR: ������ ����
            item.itemName = "Lustful Heart";

            // EN: Amplifies <color=#1787D8>Magic Attack</color> by 20%.\n
            // Increases Quintessence cooldown speed by 60%.\n
            // Amplifies Quintessence damage by 30%.\n

            // KR: <color=#1787D8>�������ݷ�</color>�� 20% �����˴ϴ�.\n
            // ���� ��ٿ� �ӵ��� 60% �����մϴ�.\n
            // ������ ������ ������ �������� 30% �����˴ϴ�.\n

            item.itemDescription = "Amplifies <color=#1787D8>Magic Attack</color> by 20%.\n"
                                 + "Increases Quintessence cooldown speed by 60%.\n"
                                 + "Amplifies Quintessence damage by 30%.";

            // EN: Given to the greatest Incubus or Succubus directly from the demon prince of lust, Asmodeus.
            // KR: ������ ���� �ƽ��𵥿콺�κ��� ���� ������ ��ť���� Ȥ�� ��ť�������� �ϻ�� ��ǥ
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
            // KR: ���� ��������
            item.itemName = "Small Twig";

            // EN: Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases skill cooldown speed and skill casting speed by 30%.\n
            // Increases Crit Rate and Crit Damage by 10%.\n
            // All effects double and Amplifies damage dealt to enemies by 20% when "Skul" or "Hero Little Bone" is your current active skull.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // ��ų ��ٿ� �ӵ� �� ��ų ���� �ӵ��� 30% �����մϴ�.\n
            // ġ��Ÿ Ȯ�� �� ġ��Ÿ ���ذ� 10% �����մϴ�.\n
            // "����" Ȥ�� "��� ��Ʋ��" ������ ��� ���� �� �� �������� ��� ���� ����ġ�� �ι谡 �Ǹ� ������ ������ �������� 20% �����˴ϴ�.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                 + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                 + "Increases Crit Rate and Crit Damage by 10%.\n"
                                 + "All effects double and Amplifies damage dealt to enemies by 20% when \"Skul\" or \"Hero Little Bone\" is your current active skull.";

            // EN: A really cool looking twig, but for some reason I feel sad...
            // KR: ���� ���־� ���̴� ���������� ���ε�, �� �� �� ���� �������� �ɱ�...
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
            // KR: ���� ��������
            item.itemName = "Small Twig";

            // EN: Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases skill cooldown speed and skill casting speed by 30%.\n
            // Increases Crit Rate and Crit Damage by 10%.\n
            // All effects double and amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // ��ų ��ٿ� �ӵ� �� ��ų ���� �ӵ��� 30% �����մϴ�.\n
            // ġ��Ÿ Ȯ�� �� ġ��Ÿ ���ذ� 10% �����մϴ�.\n
            // '����' Ȥ�� '��� ��Ʋ��' ������ ��� ���� �� �� �������� ��� ���� ����ġ�� �ι谡 �Ǹ� ������ ������ �������� 20% �����˴ϴ�.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                 + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                 + "Increases Crit Rate and Crit Damage by 10%.\n"
                                 + "All effects double and amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.";

            // EN: A really cool looking twig, but for some reason I feel sad...
            // KR: ���� ���־� ���̴� ���������� ���ε�, �� �� �� ���� �������� �ɱ�...
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
            // KR: ȭ���� �ϰ�
            item.itemName = "Volcanic Shard";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 80%.\n
            // Normal attacks and skills have a 20% chance to inflict Burn.\n
            // Amplifies damage dealt to Burning enemies by 25%.\n
            // Burn duration decreases by 5% for each Arson inscription in possession.

            // KR: <color=#1787D8>�������ݷ�</color>�� 80% �����մϴ�.\n
            // �� ���� �� 20% Ȯ���� ȭ���� �ο��մϴ�.\n
            // ������ ȭ������ ������ �������� 25% �����˴ϴ�.\n
            // ������ �ִ� ��ȭ ���ο� ����Ͽ� ȭ���� ���ӽð��� 5%�� �����մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 80%.\n"
                                 + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                 + "Amplifies damage dealt to Burning enemies by 25%.\n"
                                 + "Burn duration decreases by 5% for each Arson inscription in possession.";

            // EN: Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.
            // KR: ������ ��伮 ȭ���� ���߿��� ��������ٰ� ������, ���󿡼� ���� �߰ſ� Į��
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
            // KR: �콼 ����
            item.itemName = "Rusty Chalice";

            // EN: Increases swap cooldown speed by 15%.\n
            // Upon hitting enemies with a swap skill 150 times, this item transforms into 'Goddess's Chalice.'

            // KR: ���� ��ٿ� �ӵ��� 15% �����d�ϴ�.\n
            // ������ ���뽺ų�� �������� 150�� �� �� �ش� �������� '������ ����'�� ���մϴ�.

            item.itemDescription = "Increases swap cooldown speed by 15%.\n"
                                 + "Upon hitting enemies with a swap skill 150 times, this item transforms into 'Goddess's Chalice.'";

            // EN: This thing? I found it at a pawn shop and it seemed interesting
            // KR: �� �̰�? �Ͻ��忡�� ���� ���̱淡 ��µ�, �?
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
            // KR: ������ ����
            item.itemName = "Goddess's Chalice";

            // EN: Increases swap cooldown speed by 40%.\n
            // Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n
            // At maximum stacks, swap cooldown speed is increased by 25%.

            // KR: ���� ��ٿ� �ӵ��� 40% �����d�ϴ�.\n
            // ���� �� 6�� ���� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����մϴ� (�ִ� 60%).\n
            // ���ݷ� ����ġ�� �ִ��� ��, ���� ��ٿ� �ӵ��� 25% �����մϴ�.

            item.itemDescription = "Increases swap cooldown speed by 40%.\n"
                                 + "Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n"
                                 + "At maximum stacks, swap cooldown speed is increased by 25%.";

            // EN: Chalice used by Leonia herself that seems to never run dry
            // KR: ���� �����Ͼ� ���β��� ���ô� ���� ������� �ʴ� ����
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
            // KR: ����: ������ �ö�ũ
            item.itemName = "Omen: Flask of Botulism";

            // EN: The interval between poison damage ticks is further decreased.

            // KR: �ߵ� �������� �߻��ϴ� ������ ���� �پ��ϴ�.

            item.itemDescription = "The interval between poison damage ticks is further decreased.";

            // EN: Only the mad and cruel would consider using this as a weapon.
            // KR: ���� ��ġ�� �ʰ��� �̰� ����� ���� ���� ���� ���̴�.
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
            // KR: ����: ������ ��¡
            item.itemName = "Omen: Corrupted Symbol";

            // EN: For every Spoils inscription owned, increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 80%.

            // KR: �����ϰ� �ִ� 'Į����' ������ 1���� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 80% �����մϴ�.

            item.itemDescription = "For every Spoils inscription owned, increase <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 80%.";

            // EN: Where's your god now?
            // KR: ��, ���� �� ���� �����?
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
            // KR: ħ�ĵ� �հ���
            item.itemName = "Tainted Finger";

            // EN: Skill damage dealt to enemies is amplified by 30%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.

            // KR: ������ ��ų�� ������ �������� 30% �����˴ϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 60% �����մϴ�.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 30%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 60%.";

            // EN: A finger from a god tainted by dark quartz
            // KR: ���� ������ ���� ħ�ĵ� ���� �հ���
            item.itemLore = "A finger from a god tainted by dark quartz";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

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
            // KR: ħ�ĵ� �հ���
            item.itemName = "Tainted Finger";

            // EN: Skill damage dealt to enemies is amplified by 30%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.\n
            // If the item 'Grace of Leonia' is in your possession, this item turns into 'Corrupted God's Hand'.

            // KR: ������ ��ų�� ������ �������� 30% �����˴ϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 60% �����մϴ�.\n
            // ���� '�����Ͼ��� ����' �������� �����ϰ� ������ �ش� �������� 'ħ�ĵ� ���� ��' ���� ���մϴ�.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 30%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                 + "If the item 'Grace of Leonia' is in your possession, this item turns into 'Corrupted God's Hand'.";

            // EN: Nothing happened. It seems like it needs something else.
            // KR: �ƹ� �ϵ� �Ͼ�� �ʾҴ�. ���� �� �ʿ��� �� ����.
            item.itemLore = "Nothing happened. It seems like it needs something else.";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

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
            // KR: ħ�ĵ� ���� ��
            item.itemName = "Corrupted God's Hand";

            // EN: Skill damage dealt to enemies is amplified by 100%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 100%.\n
            // Max HP decreases by 30% for all enemies.

            // KR: ������ ��ų�� ������ �������� 100% �����˴ϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 100% �����մϴ�.

            item.itemDescription = "Skill damage dealt to enemies is amplified by 100%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 100%.";

            // EN: A corrupt hand from Leonia's supposed god
            // KR: �����ϾƷ� �����Ǵ� ���� ħ�ĵ� ��
            item.itemLore = "A corrupt hand from Leonia's supposed god";

            item.prefabKeyword1 = Inscription.Key.Artifact;
            item.prefabKeyword2 = Inscription.Key.Masterpiece;

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
            // KR: �帲ĳó
            item.itemName = "Dream Catcher";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 50%.\n
            // <color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n
            // <color=#1787D8>Magic Attack</color> increases by 8% each time an Omen or a Legendary item is destroyed.

            // KR: <color=#1787D8>�������ݷ�</color>�� 50% �����մϴ�.\n
            // ���� ü���� 40% ������ ������ ������ <color=#1787D8>����������</color>�� 25% �����˴ϴ�.\n
            // ���� Ȥ�� �������� ����� ���� �������� �ı��� ������ <color=#1787D8>�������ݷ�</color>�� 8% �����մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                 + "<color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n"
                                 + "<color=#1787D8>Magic Attack</color> increases by 8% each time an Omen or a Legendary item is destroyed.";

            // EN: Acceptance is the first step towards death.
            // KR: �����ϴ� ���� ������ ���� ù �����̴�.
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
            // KR: �������� ��â
            item.itemName = "Blood-Soaked Javelin";

            // EN: Increases Crit Damage by 20%.\n
            // Critical hits have a 10% chance to apply Wound (Cooldown: 0.5 seconds).

            // KR: ġ��Ÿ �������� 20% �����մϴ�.\n
            // ġ��Ÿ �� 10% Ȯ���� ������ ��ó�� �ο��մϴ� (��Ÿ��: 0.5��).

            item.itemDescription = "Increases Crit Damage by 20%.\n"
                                 + "Critical hits have a 10% chance to apply Wound.";

            // EN: A javelin that always hits vital organs, and drains all the blood out of whichever one it hits
            // KR: ���� ������ ��Ȯ�� ��� ��ü�� �� �ѹ�� ������ �ʴ� ��â
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
            // KR: ������ â
            item.itemName = "Frozen Spear";

            // EN: Skills have a 10% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // After applying freeze 250 times, this item turns into 'Spear of the Frozen Moon'.

            // KR: ������ ��ų�� ���ݽ� 10% Ȯ���� ������ �ο��մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 20% �����մϴ�.\n
            // ������ ������ 250�� �ο��� �� �ش� �������� '������ ���� â'���� ���մϴ�.

            item.itemDescription = "Skills have a 10% chance to inflict Freeze.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                 + "After applying freeze 250 times, this item turns into 'Spear of the Frozen Moon'.";

            // EN: A sealed weapon waiting the cold time to revealed it's true form.
            // KR: �ع��� Ȥ���� ��ٸ��� ���ε� ����
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
            // KR: ������ ���� â
            item.itemName = "Spear of the Frozen Moon";

            // EN: Skills have a 15% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 60%.\n
            // Attacking frozen enemies increases the number of hits to remove Freeze by 1.\n
            // Amplifies damage to frozen enemies by 25%.

            // KR: ������ ��ų�� ���ݽ� 15% Ȯ���� ������ �ο��մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 60% �����մϴ�.\n
            // ���� ������ �� ���� �� ������ �����Ǵµ� �ʿ��� Ÿ���� 1 �����մϴ�.\n
            // ���� ������ ������ ������ �������� 25% �����մϴ�.

            item.itemDescription = "Skills have a 15% chance to inflict Freeze.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 60%.\n"
                                 + "Attacking frozen enemies increases the number of hits to remove Freeze by 1.\n"
                                 + "Amplifies damage dealt to frozen enemies by 25%.";

            // EN: When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. 
            // KR: ���忡 ������ �ָ���ĥ ��, ���� ���� ������ â�� ��Ÿ������
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
            // KR: ���� �����
            item.itemName = "Cross Necklace";

            // EN: Recover 5 HP upon entering a map.

            // KR: �� ���� �� ü���� 5 ȸ���մϴ�.

            item.itemDescription = "Recover 5 HP upon entering a map.";

            // EN: When all is lost, we turn to hope
            // KR: ��� ���� �Ҿ��� ��, ����� �ٶ�����
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
            // KR: ���� ����
            item.itemName = "Rotten Wings";

            // EN: Crit Rate increases by 15% while in midair.\n
            // Your normal attacks have a 15% chance to inflict Poison.

            // KR: ���߿� ���� �� ġ��Ÿ Ȯ���� 15% �����մϴ�.\n
            // ������ �⺻���� �� 15% Ȯ���� �ߵ��� �ο��մϴ�.

            item.itemDescription = "Crit Rate increases by 15% while in midair.\n"
                                 + "Your normal attacks have a 15% chance to inflict Poison.";

            // EN: Wings of a zombie wyvern
            // KR: ���� ���̹��� ��� ���巯�� ����
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
            // KR: ������ ����
            item.itemName = "Shrinking Potion";

            // EN: Decreases character size by 20%.\n
            // Increases Movement Speed by 15%.\n
            // Incoming damage increases by 10%.

            // KR: ĳ���� ũ�Ⱑ 20% �����մϴ�.\n
            // �̵��ӵ��� 15% �����մϴ�.\n
            // �޴� �������� 10% �����մϴ�.

            item.itemDescription = "Decreases character size by 20%.\n"
                                 + "Increases Movement Speed by 15%.\n"
                                 + "Incoming damage increases by 10%.";

            // EN: I think it was meant to be used on the enemies...
            // KR: ���� ������ ��� �� �� ������...
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
            // KR: �Ҿ����� ũ�� ���� ����
            item.itemName = "Unstable Size Potion";

            // EN: Alters between the effects of 'Shrinking Potion' and 'Growing Potion' every 10 seconds.

            // KR: 10�� ���� '������ ����'�� '���� ����'�� ȿ���� �����ư��� �����մϴ�.

            item.itemDescription = "Alters between the effects of 'Shrinking Potion' and 'Growing Potion' every 10 seconds.";

            // EN: Mixing those potions together was a bad idea
            // KR: �� ������� ���� ���� ���� ������ �ƴϾ���.
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

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "GrowingPotion";
            item.rarity = Rarity.Rare;

            // EN: Growing Potion
            // KR: ���� ����
            item.itemName = "Growing Potion";

            // EN: Increases character size by 20%.\n
            // Decreases Movement Speed by 15%.\n
            // Incoming damage decreases by 10%.

            // KR: ĳ���� ũ�Ⱑ 20% �����մϴ�.\n
            // �̵��ӵ��� 15% �����մϴ�.\n
            // �޴� �������� 10% �����մϴ�.

            item.itemDescription = "Increases character size by 20%.\n"
                                 + "Decreases Movement Speed by 15%.\n"
                                 + "Incoming damage decreases by 10%.";

            // EN: Made from some weird size changing mushrooms deep within The Forest of Harmony
            // KR: �ϸ�Ͼ� �� ����� �ִ� ������ �������� ������� ����
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
            // KR: ���� ���ӱ�
            item.itemName = "Mana Accelerator";

            // EN: Skill casting speed increases by 10% for each Mana Cycle inscription in possession.

            // KR: �������� ������ȯ ���� 1���� ��ų ���� �ӵ��� 10% �����մϴ�.

            item.itemDescription = "Skill casting speed increases by 10% for each Mana Cycle inscription in possession.";

            // EN: In a last ditch effort, mages may turn to this device to overcharge their mana. Though the high stress on the mage's mana can often strip them of all magic.
            // KR: ������ ���ѱ��� �����Ͻ�Ű�� ��������� ������ ����.\n�ʹ� ���� �����ϴ� ����ڸ� �ұ��� ���� �� ������ �����ؾ� �Ѵ�
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
            // KR: �ʺ��ڿ� â
            item.itemName = "Beginner's Lance";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 20%.\n
            // Damage dealt to enemies with a dash attack is amplified by 30%.

            // KR: <color=#F25D1C>�������ݷ�</color>�� 20% �����մϴ�.\n
            // ������ �뽬�������� ������ �������� 30% �����˴ϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 20%.\n"
                                 + "Damage dealt to enemies with a dash attack is amplified by 30%.";

            // EN: Perfect! Now all I need is a noble steed...
            // KR: �Ϻ���! ���� ���� ���� ������ �Ǵµ�...
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
            // KR: �����޸� â
            item.itemName = "Winged Spear";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 15%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases Attack Speed by 15%.\n
            // Increases skill cooldown speed by 15%.\n
            // Increases swap cooldown speed by 15%.

            // KR: <color=#F25D1C>�������ݷ�</color>�� 15% �����մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 15% �����մϴ�.\n
            // ���ݼӵ��� 15% �����մϴ�.\n
            // ��ų ��ٿ� �ӵ��� 15% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 15% �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 15%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 15%.\n"
                                 + "Increases Attack Speed by 15%.\n"
                                 + "Increases skill cooldown speed by 15%.\n"
                                 + "Increases swap cooldown speed by 15%.";

            // EN: A golden spear ornamented with the wings of dawn.
            // KR: ������ ������ ġ��� �ݻ� â
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
            // KR: �޺� �����޸� ��
            item.itemName = "Solar-Winged Sword";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 55%.\n
            // Increases Attack Speed by 25%.\n
            // Increases swap cooldown speed by 25%.

            // KR: <color=#F25D1C>�������ݷ�</color>�� 55% �����մϴ�.\n
            // ���ݼӵ��� 25% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 25% �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 55%.\n"
                                 + "Increases Attack Speed by 25%.\n"
                                 + "Increases swap cooldown speed by 25%.";

            // EN: A golden sword ornamented with the wings of dawn.
            // KR: ������ ������ ġ��� �ݻ� ��
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
            // KR: �޺� �����޸� ����
            item.itemName = "Lunar-Winged Insignia";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 55%.\n
            // Increases skill cooldown speed by 15%.\n
            // Increases swap cooldown speed by 15%.

            // KR: <color=#1787D8>�������ݷ�</color>�� 55% �����մϴ�.\n
            // ��ų ��ٿ� �ӵ��� 25% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 25% �����մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 55%.\n"
                                 + "Increases skill cooldown speed by 25%.\n"
                                 + "Increases swap cooldown speed by 25%.";

            // EN: A golden insignia ornamented with the wings of dawn.
            // KR: ������ ������ ġ��� �ݻ� ����
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
            // KR: ������ ����
            item.itemName = "Wings of Dawn";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 75%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 75%.\n
            // Increases Attack Speed by 45%.\n
            // Increases skill cooldown speed by 45%.\n
            // Increases swap cooldown speed by 45%.

            // KR: <color=#F25D1C>�������ݷ�</color>�� 75% �����մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 75% �����մϴ�.\n
            // ���ݼӵ��� 45% �����մϴ�.\n
            // ��ų ��ٿ� �ӵ��� 45% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 45% �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 75%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 75%.\n"
                                 + "Increases Attack Speed by 45%.\n"
                                 + "Increases skill cooldown speed by 45%.\n"
                                 + "Increases swap cooldown speed by 45%.";

            // EN: A divine spear donning the wings of dawn.
            // KR: ������ ������ ����� �ż��� â
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
            // KR: ����: ������ ����
            item.itemName = "Omen: Last Dawn";

            // EN: Increases <color=#F25D1C>Physical Attack</color> by 110%.\n
            // Increases <color=#1787D8>Magic Attack</color> by 110%.\n
            // Increases Attack Speed by 65%.\n
            // Increases skill cooldown speed by 65%.\n
            // Increases swap cooldown speed by 65%.

            // KR: <color=#F25D1C>�������ݷ�</color>�� 110% �����մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 110% �����մϴ�.\n
            // ���ݼӵ��� 65% �����մϴ�.\n
            // ��ų ��ٿ� �ӵ��� 65% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 65% �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> by 110%.\n"
                                 + "Increases <color=#1787D8>Magic Attack</color> by 110%.\n"
                                 + "Increases Attack Speed by 65%.\n"
                                 + "Increases skill cooldown speed by 65%.\n"
                                 + "Increases swap cooldown speed by 65%.";

            // EN: The sky cracks, darkness fills the world within.
            // KR: �ϴ��� ������ �Ӽ��� ��ҿ� ��������.
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
            // KR: ���Ͼƽ�
            item.itemName = "Fonias";

            // EN: Increases Crit Chance by 5%.\n
            // Increases Crit Damage by 25%.\n
            // Amplifies damage dealt to enemies by 10%.\n
            // Amplfies damage dealt to an adventurer or a boss by 5%.

            // KR: ġ��Ÿ Ȯ���� 5% �����մϴ�.\n
            // ġ��Ÿ ���ذ� 25% �����մϴ�.\n
            // ������ ������ �������� 10% �����˴ϴ�.\n
            // ���谡 Ȥ�� �������� ������ �������� 5% �����˴ϴ�.

            item.itemDescription = "Increases Crit Chance by 5%.\n"
                                 + "Increases Crit Damage by 25%.\n"
                                 + "Amplifies damage dealt to enemies by 10%.\n"
                                 + "Amplfies damage dealt to an adventurer or a boss by 5%.";

            // EN: An ancient scythe imbued with cursed power.\nIt was once wielded by a former demon king.
            // KR: ���� ������ �Ѹ��� ����ߴٴ� ������ ����� �վ�� ����� ��
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
            // KR: ���ô��� �����Ǿ�
            item.itemName = "Spiky Rapida";

            // EN: Increases Attack Speed by 20%.\n
            // Every 3rd normal attack, inflicts Wound to enemies that were hit.

            // KR: ���ݼӵ��� 20% �����մϴ�.\n
            // 3ȸ ° �⺻���ݸ��� �ǰݵ� ������ ��ó�� �����ϴ�.

            item.itemDescription = "Increases Attack Speed by 20%.\n"
                                 + "Every 3rd normal attack, inflicts Wound to enemies that were hit.";

            // EN: In ancient times, when there was no English language yet, you would have been called "Victor".....
            // KR: ������ ����, �̰��� �� ���� ���� ����� "���丣" ��� �ҷȴ� �� ����.....
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
            // KR: ������ ���
            item.itemName = "Weird Herbs";

            // EN: Swapping increases skill cooldown speed by 25% and Crit Rate by 12% for 6 seconds.

            // KR: ���� �� 6�� ���� ��ų ��ٿ� �ӵ��� 25% �����ϰ� ġ��Ÿ Ȯ���� 12% �����մϴ�.

            item.itemDescription = "Swapping increases skill cooldown speed by 25% and Crit Rate by 12% for 6 seconds.";

            // EN: Quartz-infused herbs which you can find all over the dark forest.
            // KR: ����� �� �������� ã�� �� �ִ� ������ ���յ� ���
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
            // KR: ���ֹ��� �ܵ�
            item.itemName = "Omen: Accursed Sabre";

            // EN: Basic attacks and skills have a 10% chance to apply Wound.\n
            // Every 2nd Bleed inflicts Bleed twice.

            // KR: �� ���� �� 10% Ȯ���� ��ó�� �ο��մϴ�.\n
            // 2ȸ ° �������� ������ �ѹ� �� �ο��մϴ�.

            item.itemDescription = "Basic attacks and skills have a 10% chance to apply Wound.\n"
                                 + "Every 2nd Bleed inflicts Bleed twice.";

            // EN: Sabre of the great duelist Sly who left his final memento in the form of never-ending anarchy and bloodshed.
            // KR: ������ �ݿ��� �л��� ���Ҵ� ���� ������ ������ �������� �ܵ�
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
            // KR: �ߺ����� Į���� ����
            item.itemName = "Heavy-Duty Carleon Helmet";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n
            // For every Spoils inscription owned, increase Max HP by 15.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 30% �����մϴ�.\n
            // �����ϰ� �ִ� 'Į����' ������ 1���� �ִ� ü���� 15 �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 30%.\n"
                                 + "For every Spoils inscription owned, increase Max HP by 15.";

            // EN: Only the strongest of Carleon's front line soldiers can wear this.\nThat... isn't saying very much, but still.
            // KR: ���� ���� Į������ �������� �� ����鸸�� �� �� �ִ� ����.\n������ ū �ǹ̴� ����δ�.
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
            // KR: ���ֹ��� �𷡽ð�
            item.itemName = "Cursed Hourglass";

            // EN: Upon entering a map or hitting a boss phase for the first time, amplifies damage dealt to enemies by 30% for 30 seconds.\n
            // When the effect is not active, increases damage received by 30%.

            // KR: �� ���� Ȥ�� ����(������ ����) ���� ó�� �������� �� �� 30�� ���� ������ ������ �������� 30% �����˴ϴ�.\n
            // �ش� ȿ���� �ߵ� ������ ���� ��, �޴� �������� 30% �����մϴ�.

            item.itemDescription = "Upon entering a map or hitting a boss phase for the first time, amplifies damage dealt to enemies by 30% for 30 seconds.\n"
                                 + "When the effect is not active, increases damage received by 30%.";

            // EN: To carry such a burden voluntarily... You're either the bravest person I've ever met, or the most foolish.
            // KR: �̷� ���� �������ٴ�... �� �Ƹ� �� ���󿡼� ���� �밨�ϰų� ��û�� ����̰���.
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
            // KR: ����� ����
            item.itemName = "Lucky Coin";

            // EN: Increases Crit Rate by 5%.\n
            // Increases Gold gain by 10%.

            // KR: ġ��Ÿ Ȯ���� 5% �����մϴ�.\n
            // ��ȭ ȹ�淮�� 10% �����մϴ�.

            item.itemDescription = "Increases Crit Rate by 5%.\n"
                                 + "Increases Gold gain by 10%.";

            // EN: Oh, must be my lucky day!
            // KR: ������ ����� ���� ���ΰ� ����!
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
            // KR: ������ ���� �񵵸�
            item.itemName = "Tainted Red Scarf";

            // EN: Increases dash cooldown speed by 30%.\n
            // Decreases dash distance by 30%.

            // KR: �뽬 ��ٿ� �ӵ��� 30% �����մϴ�.\n
            // �뽬 �Ÿ��� 30% �����մϴ�.

            item.itemDescription = "Increases dash cooldown speed by 20%.\n"
                                 + "Decreases dash distance by 30%.";

            // EN: A small scarf that was once part of an old doll
            // KR: � �������� ������ ���� ���� �񵵸�
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
            // KR: ���� ����
            item.itemName = "Tattered Plushie";

            // EN: Every 5 seconds, depletes 10% of your Max HP and permanently grants you 5% amplification on damage dealt to enemies.\n
            // Upon killing an enemy, recovers 2% of your Max HP.

            // KR: 5�ʸ��� �ִ� ü���� 10%�� ���ϴ� ���ظ� �԰� ���������� ���鿡�� ������ �������� 5% �����˴ϴ�.\n
            // ���� óġ�� ������ �ִ� ü���� 2%�� ȸ���մϴ�.

            item.itemDescription = "Every 5 seconds, depletes 10% of your Max HP and permanently grants you 5% amplification on damage dealt to enemies.\n"
                                 + "Upon killing an enemy, recovers 2% of your Max HP.";

            // EN: bEsT FrIenDs fOrEveR
            // KR: ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ� ������ �Բ�
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
