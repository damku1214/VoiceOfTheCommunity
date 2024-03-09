using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities;
using Characters.Gear.Synergy.Inscriptions;
using VoiceOfTheCommunity.CustomAbilities;
using UnityEngine.AddressableAssets;
using static Characters.Damage;
using static Characters.CharacterStatus;

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

            item.stats = new Stat.Values(new Stat.Value[] {});

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

            item.abilities = new Ability[] {
                ability
            };

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
            // If the Succubus Quintessence is in your possession, this item turns into 'Lustful Heart'.

            // KR: <color=#1787D8>�������ݷ�</color>�� 20% �����մϴ�.\n
            // ���� ��ٿ� �ӵ��� 30% �����մϴ�.\n
            // ������ ������ ������ �������� 15% �����˴ϴ�.\n
            // ��ť���� ���� ���� �� �� �������� '������ ����'���� ���մϴ�.

            item.itemDescription = "Increases <color=#1787D8>Magic Attack</color> by 20%.\n"
                                           + "Increases Quintessence cooldown speed by 30%.\n"
                                           + "Amplifies Quintessence damage by 15%.\n"
                                           + "If the Succubus Quintessence is in your possession, this item turns into 'Lustful Heart'.";

            // EN: Some poor being must have their heart torn both metaphorically and literally.
            // KR: ���� ��, ������ ���������ε� ���������ε� �������ٴ�.
            item.itemLore = "Some poor being must have their heart torn both metaphorically and literally.";

            item.prefabKeyword1 = Inscription.Key.Heritage;
            item.prefabKeyword2 = Inscription.Key.Wisdom;

            item.stats = new Stat.Values(new Stat.Value[]
            {
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.2),
                new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.EssenceCooldownSpeed, 0.3),
            });

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
            item.obtainable = false;
            item.rarity = Rarity.Legendary;

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
            // All effects double and Amplifies damage dealt to enemies by up to 20% when "Skul" or "Hero Little Bone" is your current active skull.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // ��ų ��ٿ� �ӵ� �� ��ų ���� �ӵ��� 30% �����մϴ�.\n
            // ġ��Ÿ Ȯ�� �� ġ��Ÿ ���ذ� 10% �����մϴ�.\n
            // "����" Ȥ�� "��� ��Ʋ��" ������ ��� ���� �� �� �������� ��� ���� ����ġ�� �ι谡 �Ǹ� ������ ������ �������� 20% �����˴ϴ�.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                           + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                           + "Increases Crit Rate and Crit Damage by 10%.\n"
                                           + "All effects double and Amplifies damage dealt to enemies by up to 20% when \"Skul\" or \"Hero Little Bone\" is your current active skull.";

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
            item.obtainable = false;
            item.rarity = Rarity.Legendary;

            // EN: Small Twig
            // KR: ���� ��������
            item.itemName = "Small Twig";

            // EN: Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n
            // Increases skill cooldown speed and skill casting speed by 30%.\n
            // Increases Crit Rate and Crit Damage by 10%.\n
            // All effects double and Amplifies damage dealt to enemies by up to 20% when "Skul" or "Hero Little Bone" is your current active skull.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // ��ų ��ٿ� �ӵ� �� ��ų ���� �ӵ��� 30% �����մϴ�.\n
            // ġ��Ÿ Ȯ�� �� ġ��Ÿ ���ذ� 10% �����մϴ�.\n
            // "����" Ȥ�� "��� ��Ʋ��" ������ ��� ���� �� �� �������� ��� ���� ����ġ�� �ι谡 �Ǹ� ������ ������ �������� 20% �����˴ϴ�.

            item.itemDescription = "Amplifies <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15%.\n"
                                           + "Increases skill cooldown speed and skill casting speed by 30%.\n"
                                           + "Increases Crit Rate and Crit Damage by 10%.\n"
                                           + "All effects double and Amplifies damage dealt to enemies by up to 20% when \"Skul\" or \"Hero Little Bone\" is your current active skull.";

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

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "VolcanicShard";
            item.rarity = Rarity.Legendary;

            // EN: Volcanic Shard
            // KR: ȭ���� �ϰ�
            item.itemName = "Volcanic Shard";

            // EN: Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 150%.\n
            // Normal attacks and skills have a 20% chance to inflict Burn.\n
            // Amplifies Damage to Burning enemies by 25%.\n
            // Burn duration increases by 20% for each Arson inscription in possession.

            // KR: <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����˴ϴ�.\n
            // �� ���� �� 20% Ȯ���� ȭ���� �����ϴ�.\n
            // ������ ȭ������ ������ �������� 25% �����˴ϴ�.\n
            // ������ �ִ� ��ȭ ���ο� ����Ͽ� ȭ���� ���ӽð��� 20%�� �����մϴ�.

            item.itemDescription = "Increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 150%.\n"
                                           + "Normal attacks and skills have a 20% chance to inflict Burn.\n"
                                           + "Amplifies Damage to Burning enemies by 25%.\n"
                                           + "Burn duration increases by 20% for each Arson inscription in possession.";

            // EN: Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.
            // KR: ������ ��伮 ȭ���� ���߿��� ��������ٰ� ������, ���󿡼� ���� �߰ſ� Į��
            item.itemLore = "Rumored to be created from the Black Rock Volcano when erupting, this giant blade is the hottest flaming sword.";

            item.prefabKeyword1 = Inscription.Key.Arms;
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

            applyBurnWhenAttacked._status = new CharacterStatus.ApplyInfo(status);

            item.abilities = [
                applyBurnWhenAttacked,
                new VolcanicShardAbility(),
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
                new RustyChaliceAbility(),
            ];

            items.Add(item);
        }
        {
            var item = new CustomItemReference();
            item.name = "RustyChalice_2";
            item.obtainable = false;
            item.rarity = Rarity.Legendary;

            // EN: Goddess's Chalice
            // KR: ������ ����
            item.itemName = "Goddess's Chalice";

            // EN: Increases swap cooldown speed by 40%.\n
            // Damage dealt to enemies through a swap skill is amplified by 35%.\n
            // Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15% for 5 seconds (maximum 60%).\n
            // At maximum stacks, swap cooldown speed is increased by 25% and damage dealt to enemies through a swap skill is amplified by 10%.

            // KR: ���� ��ٿ� �ӵ��� 40% �����d�ϴ�.\n
            // ������ ���뽺ų�� ������ �������� 35% �����˴ϴ�.\n
            // ���� �� 5�� ���� <color=#F25D1C>�������ݷ�</color> �� <color=#1787D8>�������ݷ�</color>�� 15% �����մϴ� (�ִ� 60%).\n
            // ���ݷ� ����ġ�� �ִ��� ��, ���� ��ٿ� �ӵ��� 25% �����ϰ� ������ ���뽺ų�� ������ �������� 10% �����˴ϴ�.

            item.itemDescription = "Increases swap cooldown speed by 40%.\n"
                                           + "Damage dealt to enemies through a swap skill is amplified by 35%.\n"
                                           + "Swapping increases <color=#F25D1C>Physical Attack</color> and <color=#1787D8>Magic Attack</color> by 15% for 5 seconds (maximum 60%).\n"
                                           + "At maximum stacks, swap cooldown speed is increased by 25% and damage dealt to enemies through a swap skill is amplified by 10%.";

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
                _timeout = 5.0f,
                _maxStack = 4,
                _statPerStack = new Stat.Values(
                [
                    new(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.15),
                    new(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.15),
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
