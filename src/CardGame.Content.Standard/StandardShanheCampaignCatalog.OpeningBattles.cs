using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // These two locations stay available together. The run seed selects a concrete
    // first-chapter story at each location; later chapters retain their own pools.
    private static ShanheCampaignDefinition WithRandomOpeningBattles(ShanheCampaignDefinition definition, int difficulty)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal)) return definition;
        // The non-codex entry has only ungraded Peach/Duel resources. Add the
        // same authored rare physical cards used by the catalogue for elites.
        var rareCards = definition.Rewards.Any(reward => reward.Rarity == 2 && reward.StartingCards?.Count > 0)
            ? Array.Empty<ShanheRewardDefinition>() : CurrentShopCardSources
                .Where(source => source.Rarity == 2 && !EquipmentCatalog.IsEquipment(source.Kind))
                .Select(source => CodexBasicCard(source.SourceId) with
                    { Id = "route-rare-card-" + source.SourceId.ToString(System.Globalization.CultureInfo.InvariantCulture), Rarity = 2 }).ToArray();
        var sources = definition.Nodes.Where(node => node.ChapterNumber == 1 && node.Id is "hulao" or "guangcheng")
            .ToDictionary(node => node.Id, StringComparer.Ordinal);
        var skills = definition.Rewards.Where(reward => reward.SkillId is not null && reward.Rarity == 2).Select(reward => reward.Id).ToArray();
        var ordinaryRewards = definition.Rewards.Where(reward => reward.Rarity is 1 or 2)
            .Select(reward => reward.Id).ToHashSet(StringComparer.Ordinal);
        var scenes = new[]
        {
            new ShanheEncounterDefinition("opening:yibing", "疑兵之计",
                [new(StandardShanheOpeningEncounterPackage.ZhangheId, MaxHpBonus: difficulty, Hp: 4 + difficulty,
                    DrawBonus: difficulty)], [], VictoryGold: 200)
            {
                StoryText = "军营四周忽然竖起多面袁绍军旗，军士叫嚣已经包围了你，要你速速投降。\n" +
                    "你心下稍作计算，料定袁绍不可能在短时间内包围你的部队，四周必为疑兵。你假装投降引出敌方主将，原来是张郃。"
            },
            new ShanheEncounterDefinition("opening:qinwang", "擒贼擒王",
                [new(StandardShanheOpeningEncounterPackage.TadunId, MaxHpBonus: difficulty, Hp: 4 + difficulty,
                    DrawBonus: difficulty)], [], VictoryGold: 200)
            {
                StoryText = "乌桓单于站在山坡上，指挥着乌桓军变换阵型，试图包围我军。你看出了他阵型中的弱点，单于自身周围缺乏护卫。\n" +
                    "双方军士鏖战之时，你单骑冲出战线，直奔单于本人。单于匆忙迎战，这是你正面击溃他的好机会。"
            }
        };
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Rewards = definition.Rewards.Concat(rareCards).ToArray(),
            RandomStartingNode = false,
            AllowChapterDetours = true,
            IntroText = definition.IntroText + "\n开局两处战斗地点都可挑战，先后顺序自选；本章已开启的未完成地点可返回补打，进入下一章后关闭旧章地点。每处按本局种子确定剧情和敌军，预览与挑战一致。仅装备传承时先选择初始技能。",
            Nodes = definition.Nodes.Select(node =>
            {
                // Older standalone exclusive forms were labelled bosses. Only
                // actual chapter-ending battles own the map's boss marker and
                // chapter reward contract; other strong encounters are elite.
                var kind = node.Kind == ShanheNodeKind.BossBattle && !ChapterBossIds.Contains(node.Id)
                    ? ShanheNodeKind.EliteBattle : node.Kind;
                var elite = kind == ShanheNodeKind.EliteBattle;
                return node with
                {
                    Kind = kind,
                    CategorizedRewards = node.CategorizedRewards || elite,
                    AutomaticRewardCategory = node.AutomaticRewardCategory || elite,
                    RewardSkipGold = elite && node.RewardSkipGold == 0 ? 100 : node.RewardSkipGold,
                    PrimaryTacticRarity = elite ? 2 : kind == ShanheNodeKind.Battle ? 1 : 0,
                    PrimarySkillRarity = elite ? node.ChapterNumber >= 2 ? 3 : 2 : kind == ShanheNodeKind.Battle
                        ? node.ChapterNumber >= 2 ? 2 : 1 : 0,
                    PrimaryCardRarity = elite ? 2 : kind == ShanheNodeKind.Battle ? 1 : 0,
                    Encounters = node.Encounters?.Select(encounter => encounter with
                        { VictoryGold = kind == ShanheNodeKind.BossBattle ? 500 : 200 }).ToArray(),
                    // The base sampler applies the quality independently to
                    // each category. Retain elite upgrades for promotion tactics.
                    RewardIds = !elite && node.BossRewards is null && node.ChapterNumber < 3 && node.RewardIds is { Count: > 0 }
                        ? node.RewardIds.Where(ordinaryRewards.Contains).ToArray()
                        : elite ? (node.RewardIds ?? []).Concat(rareCards.Select(reward => reward.Id)).ToArray() : node.RewardIds
                };
            }).Select(node => node.Id is not ("hulao-start" or "guangcheng-start") ? node : node with
            {
                Name = node.Id == "hulao-start" ? "村落" : "戍寨",
                Kind = ShanheNodeKind.Battle,
                Choices = [],
                // Keep complete authored first-chapter rosters. Do not import
                // second-chapter troops or final boss recipes into this pool.
                Encounters = scenes.Concat(sources[node.Id == "hulao-start" ? "hulao" : "guangcheng"].Encounters!
                    .Where(encounter => encounter.BossTransition is null)
                    .Select(encounter => encounter with
                    {
                        VictoryGold = 200, HealAfterVictory = 0,
                        // The route's later paragraphs document implementation
                        // details. Keep its authored story introduction in play.
                        StoryText = encounter.StoryText ?? sources[node.Id == "hulao-start" ? "hulao" : "guangcheng"].StoryText
                            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).First()
                    }))
                    .Select(encounter => encounter with { Id = node.Id + ":" + encounter.Id }).ToArray(),
                RewardIds = skills,
                RewardChoiceCount = 3,
                RewardSkipGold = 100,
                CategorizedRewards = false,
                AutomaticRewardCategory = false,
                PrimaryTacticRarity = 1,
                PrimarySkillRarity = 2,
                StoryText = "前方驻军已现。选择此处，查看本次征程确定的剧情、敌军和过关奖励，再决定是否挑战。",
                Description = "第一章随机预设战斗；胜利获得200铜币并选择一项技能，战后不自动回复体力。"
            }).ToArray()
        });
    }
}
