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
            // KR: ��ȥ�� ��� ���ڱ�
            item.itemName = "Vase of the Fallen";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (Stacks up to 200% and 1/2 of total charge is lost when hit).\n
            // Attacking an enemy within 1 second of taking from a hit restores half of the charge lost from the hit (Cooldown: 3 seconds).

            // KR: óġ�� ���� ���� ����Ͽ� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 5% �����մϴ� (�ִ� 200% ����, �ǰݽ� ����ġ�� ������ ������ϴ�).\n
            // �ǰ� �� 1�� ���� �� ���� �� ������ ����ġ�� ������ �ǵ��� �޽��ϴ� (��Ÿ��: 3��).

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 5% per enemy killed (Stacks up to 200% and 1/2 of total charge is lost when hit.)\n"
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
            // All effects double and Amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // ��ų ��ٿ� �ӵ� �� ��ų ���� �ӵ��� 30% �����մϴ�.\n
            // ġ��Ÿ Ȯ�� �� ġ��Ÿ ���ذ� 10% �����մϴ�.\n
            // '����' Ȥ�� '��� ��Ʋ��' ������ ��� ���� �� �� �������� ��� ���� ����ġ�� �ι谡 �Ǹ� ������ ������ �������� 20% �����˴ϴ�.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                           + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                           + "Increases Crit Rate and Crit Damage by 10%.\n"
                                           + "All effects double and Amplifies damage dealt to enemies by 20% when 'Skul' or 'Hero Little Bone' is your current active skull.";

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
            // KR: ȭ���� �ϰ�
            item.itemName = "Volcanic Shard";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 100%.\n
            // Normal attacks and skills have a 20% chance to inflict Burn.\n
            // Amplifies Damage to Burning enemies by 25%.\n
            // Burn duration increases by 20% for each Arson inscription in possession.

            // KR: <color=#1787D8>�������ݷ�</color>�� 100% �����մϴ�.\n
            // �� ���� �� 20% Ȯ���� ȭ���� �����ϴ�.\n
            // ������ ȭ������ ������ �������� 25% �����˴ϴ�.\n
            // ������ �ִ� ��ȭ ���ο� ����Ͽ� ȭ���� ���ӽð��� 20%�� �����մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 100%.\n"
                                           + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                           + "Amplifies Damage to Burning enemies by 25%.\n"
                                           + "Burn duration increases by 20% for each Arson inscription in possession.";

            // EN: Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.
            // KR: ������ ��伮 ȭ���� ���߿��� ��������ٰ� ������, ���󿡼� ���� �߰ſ� Į��
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
            // KR: ������ ����
            item.itemName = "Goddess's Chalice";

            // EN: Increases swap cooldown speed by 40%.\n
            // Damage dealt to enemies through a swap skill is amplified by 35%.\n
            // Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 10% for 6 seconds (maximum 40%).\n
            // At maximum stacks, swap cooldown speed is increased by 25%.

            // KR: ���� ��ٿ� �ӵ��� 40% �����d�ϴ�.\n
            // ������ ���뽺ų�� ������ �������� 35% �����˴ϴ�.\n
            // ���� �� 6�� ���� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����մϴ� (�ִ� 60%).\n
            // ���ݷ� ����ġ�� �ִ��� ��, ���� ��ٿ� �ӵ��� 25% �����մϴ�.

            item.itemDescription = "Increases swap cooldown speed by 40%.\n"
                                           + "Damage dealt to enemies through a swap skill is amplified by 35%.\n"
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
            // KR: ����: ������ �ö�ũ
            item.itemName = "Omen: Flask of Botulism";

            // EN: The interval between poison damage ticks is further decreased.\n
            // Increases <color=#F25D1C>Physical Attack</color> by 50%.

            // KR: �ߵ� �������� �߻��ϴ� ������ ���� �پ��ϴ�.\n
            // <color=#F25D1C>�������ݷ�</color>�� 50% �����մϴ�.

            item.itemDescription = "The interval between poison damage ticks is further decreased\n"
                                           + "Increases <color=#F25D1C>Physical Attack</color> by 50%.";

            // EN: Only the mad and cruel would consider using this as a weapon.
            // KR: ���� ��ġ�� �ʰ��� �̰� ����� ���� ���� ���� ���̴�.
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
            // KR: �帲ĳó
            item.itemName = "Dream Catcher";

            // EN: Increases <color=#1787D8>Magic Attack</color> by 50%.\n
            // <color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n
            // <color=#1787D8>Magic Attack</color> increases by 8% each time a Omen or a Legendary item is destroyed.

            // KR: <color=#1787D8>�������ݷ�</color>�� 50% �����մϴ�.\n
            // ���� ü���� 40% ������ ������ ������ <color=#1787D8>����������</color>�� 25% �����˴ϴ�.\n
            // ���� Ȥ�� �������� ����� ���� �������� �ı��� ������ <color=#1787D8>�������ݷ�</color>�� 8% �����մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                           + "<color=#1787D8>Magic damage</color> dealt to enemies under 40% HP is amplified by 25%.\n"
                                           + "<color=#1787D8>Magic Attack</color> increases by 8% each time a Omen or a Legendary item is destroyed.";

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
            item.name = "BloodsoakedJavelin";
            item.rarity = Rarity.Rare;

            // EN: Bloodsoaked Javelin
            // KR: �������� ��â
            item.itemName = "Bloodsoaked Javelin";

            // EN: Increases Crit Damage by 25%.\n
            // Critical hits have a 20% chance to apply Bleed to the enemy.

            // KR: ġ��Ÿ �������� 25% �����մϴ�.\n
            // ġ��Ÿ �� 20% Ȯ���� ������ ������ �ο��մϴ�.

            item.itemDescription = "Increases Crit Damage by 25%.\n"
                                           + "Critical hits have a 20% chance to apply Bleed to the enemy.";

            // EN: A javelin that always hits vital organs, and drains all the blood out of whichever one it hits
            // KR: ���� ������ ��Ȯ�� ��� ��ü�� �� �ѹ�� ������ �ʴ� ��â
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
            // KR: ������ â
            item.itemName = "Frozen Spear";

            // EN: Skills have a 2% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 20%.\n
            // After applying freeze 200 times, this item turns into 'Spear of the Frozen Moon'.

            // KR: ������ ��ų�� ���ݽ� 2% Ȯ���� ������ �ο��մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 20% �����մϴ�.\n
            // ������ ������ 200�� �ο��� �� �ش� �������� '������ ���� â'���� ���մϴ�.

            item.itemDescription = "Skills have a 2% chance to inflict Freeze.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                           + "After applying freeze 200 times, this item turns into 'Spear of the Frozen Moon'.";

            // EN: A sealed weapon waiting the cold time to revealed it's true form.
            // KR: �ع��� Ȥ���� ��ٸ��� ���ε� ����
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
            // KR: ������ ���� â
            item.itemName = "Spear of the Frozen Moon";

            // EN: Skills have a 5% chance to inflict Freeze.\n
            // Increases <color=#1787D8>Magic Attack</color> by 50%.\n
            // Attacking frozen enemies increases the number of hits to remove Freeze by 3.\n
            // Amplifies damage to frozen enemies by 50%.

            // KR: ������ ��ų�� ���ݽ� 5% Ȯ���� ������ �ο��մϴ�.\n
            // <color=#1787D8>�������ݷ�</color>�� 50% �����մϴ�.\n
            // ���� ������ �� ���� �� ������ �����Ǵµ� �ʿ��� Ÿ���� 3 �����մϴ�.\n
            // ���� ������ ������ ������ �������� 50% �����մϴ�.

            item.itemDescription = "Skills have a 5% chance to inflict Freeze.\n"
                                           + "Increases <color=#1787D8>Magic Attack</color> by 50%.\n"
                                           + "Attacking frozen enemies increases the number of hits to remove Freeze by 3.\n"
                                           + "Amplifies damage dealt to frozen enemies by 50%.";

            // EN: When a battlefield turns into a permafrost, the weapon formely wielded by the ice beast Vaalfen appears. 
            // KR: ���忡 ������ �ָ���ĥ ��, ���� ���� ������ â�� ��Ÿ������
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
