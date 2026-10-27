using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Local three-tier recipes reuse the native source bodies, not the
    // source difficulty calculator. Apply after chapter growth and before
    // opening/terrain pool copies so every route keeps the same actual stages.
    private static ShanheCampaignDefinition WithReturningNativeBossStages(ShanheCampaignDefinition definition, int difficulty)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal) || definition.SourceDifficultyId is not null)
            return definition;
        if (difficulty is < 1 or > 3) throw new ArgumentOutOfRangeException(nameof(difficulty));
        var growth = difficulty - 1;
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            IntroText = definition.IntroText.Replace("张牛角保留无技能基础身体", "黄巾余匪采用张牛角与张燕两阶段", StringComparison.Ordinal),
            Nodes = definition.Nodes.Select(node => node.Encounters is null ? node : node with
            {
                StoryText = node.Id == "huanyuan" ? node.StoryText.Replace(
                    "采用主组2653张牛角3/3基础形态，无基础技能或固定牌，不预先授予中等及更高难度的技能。三档均无额外成长、固定牌或友方同行；位置、抽选及奖励为本地编排。",
                    "黄巾余匪由张牛角与张燕分两阶段迎战，按本地三档配置体力、技能与装备。", StringComparison.Ordinal) : node.StoryText,
                Encounters = node.Encounters.Select(encounter => encounter.Id switch
                {
                    "huanyuan:zhangniujiao-s5" => ReturningZhangYanStages(encounter, difficulty, growth),
                    "luoyang:gaichao-s5" => ReturningShenNuStage(encounter, growth),
                    _ => encounter
                }).ToArray()
            }).ToArray()
        });
    }

    private static ShanheEncounterDefinition ReturningZhangYanStages(ShanheEncounterDefinition encounter, int difficulty, int growth)
    {
        var firstGeneral = difficulty switch
        {
            1 => StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId,
            2 => StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId,
            _ => StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId
        };
        var secondGeneral = difficulty == 1 ? StandardShanheSourceZhangYanPackage.ZhangYanMediumId
            : StandardShanheSourceZhangYanPackage.ZhangYanHardId;
        var firstHp = 6 + 2 * growth;
        var secondHp = 12 + 2 * growth;
        var secondHand = 3 + growth;
        return encounter with
        {
            Name = "黄巾余匪 · 张牛角与张燕",
            // Preserve this encounter's original roster/resources/companions;
            // only its first source3-HP body receives the explicit local profile.
            Enemies = encounter.Enemies.Select((enemy, index) => index != 0 ? enemy : enemy with
            {
                GeneralId = firstGeneral, MaxHpBonus = firstHp - 3, Hp = firstHp,
                InitialHandBonus = 1 + growth, DrawBonus = 1, SlashBonus = 0,
                SourceDifficultyAttributes = null
            }).ToArray(),
            BossTransition = new(0, secondGeneral, secondHp, secondHp, secondHand)
            {
                Combatant = new(secondGeneral, MaxHpBonus: secondHp - 4, Hp: secondHp,
                    DrawBonus: 1 + growth, SlashBonus: 1),
                StartingEquipment = [new(CardKind.ShanheSanshou, Suit.Heart, 12)]
            },
            StoryText = $"张牛角以{firstHp}点体力迎战；击败后，张燕以{secondHp}点体力接战，补{secondHand}张牌并装备三首。" +
                "张燕拥有炎狱与蔽日：其他角色准备阶段可能遭受火焰伤害，张燕防止火焰伤害并回复体力、摸牌。"
        };
    }

    private static ShanheEncounterDefinition ReturningShenNuStage(ShanheEncounterDefinition encounter, int growth)
    {
        var general = StandardShanheSourceShenNuStagePackage.GeneralId;
        // Retain the existing local final-boss HP curve, replacing only the
        // former same-general second form. First form and allies are untouched.
        var hp = 20 + 4 * growth;
        var hand = 3 + growth;
        return encounter with
        {
            Name = "西凉霸主 · 改朝罪魁与篡天恶魂",
            BossTransition = new(0, general, hp, hp, hand)
            {
                Combatant = new(general, MaxHpBonus: hp - 20, Hp: hp,
                    DrawBonus: 1 + growth, SlashBonus: 1),
                StartingEquipment = [new(CardKind.ShuraInfernoHalberd, Suit.Heart, 12), new(CardKind.LionBelt, Suit.Club, 2)]
            },
            StoryText = $"击败改朝罪魁后，篡天恶魂以{hp}点体力接战，补{hand}张牌并装备修罗炼狱戟、玲珑狮蛮带。" +
                "神怒使其每次受伤积累一枚怒，并按伤害来源当前体力摸牌；每枚怒令其伤害增加一点，造成伤害后清空怒。"
        };
    }
}
