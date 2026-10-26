using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string ThirdChapterDongwuEliteId = "dongwu-veterans-elite";

    // An optional third-chapter elite detour rejoins the existing final routes.
    // Applying this after chapter/boss composition avoids inferring a second form
    // or assigning the final-victory contract to an elite encounter.
    private static ShanheCampaignDefinition WithThirdChapterEliteEncounters(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var camp = definition.Nodes.Single(node => node.Id == "finalcamp");
        var rewards = definition.Rewards.Where(reward => reward.Rarity >= 2 && reward.OpeningCompanionPool is null)
            .Select(reward => reward.Id).ToArray();
        var encounter = new ShanheEncounterDefinition(ThirdChapterDongwuEliteId + ":encounter", "东吴精锐 · 黄盖、韩当、程普",
            [new(StandardShanheEliteDongwuPackage.HuangGaiId, Hp: 5, DrawBonus: 2, SlashBonus: 1),
             new(StandardShanheEliteHandangPackage.HandangId, Hp: 5, DrawBonus: 2, SlashBonus: 1),
             new(StandardShanheEliteDongwuPackage.ChengPuId, Hp: 5, DrawBonus: 2, SlashBonus: 1)],
            [], VictoryGold: 200);
        var elite = new ShanheNodeDefinition(ThirdChapterDongwuEliteId, "第三章精英 · 东吴精锐", ShanheNodeKind.EliteBattle,
            camp.NextNodeIds, [encounter], rewards)
        {
            ChapterNumber = 3, ChapterTitle = "第三章 · 东吴精锐", CategorizedRewards = true,
            PrimaryTacticRarity = 2, PrimarySkillRarity = 2, RewardSkipGold = 100,
            StoryText = "黄盖、韩当、程普率精锐部众拦路。击败三人，领取战利品后继续选择最终路线。",
            Description = "第三章可选精英战。三将各5体力，基础摸4张、出杀2次；黄盖英姿另加摸牌，韩当猛武使其用杀无次数和距离限制。"
        };
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id == camp.Id
                ? node with { NextNodeIds = camp.NextNodeIds.Append(elite.Id).ToArray() } : node).Append(elite).ToArray()
        });
    }
}
