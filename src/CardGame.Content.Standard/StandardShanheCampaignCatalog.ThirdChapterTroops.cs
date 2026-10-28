using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string ThirdChapterPatrolNodeId = "jingji-patrol";

    private static ShanheCampaignDefinition WithReturningThirdChapterTroops(
        ShanheCampaignDefinition definition, int difficulty, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var camp = definition.Nodes.Single(node => node.Id == "finalcamp");
        var growth = difficulty - 1;
        var tier = difficulty switch { 1 => 1, 2 => 11, 3 => 21, _ => throw new ArgumentOutOfRangeException(nameof(difficulty)) };
        var rewards = definition.Rewards.Where(reward => reward.Rarity >= 2 && reward.OpeningCompanionPool is null)
            .Select(reward => reward.Id).ToArray();
        var encounters = new[]
        {
            Patrol(2493, StandardShanheSourceWuqueAssassinPackage.GetGeneralId(tier), 3,
                1 + (tier >= 6 ? 1 : 0) + (tier >= 16 ? 1 : 0), []),
            Patrol(2495, StandardShanheSourceExecutionerPackage.GetGeneralId(tier), 4, 1, ExecutionerGear(tier)),
            ReturningMixedOrdinary(2465, difficulty, ThirdChapterPatrolNodeId + ":coalition-trio-2465-local", chapter: 3),
            ReturningMixedOrdinary(2464, difficulty, ThirdChapterPatrolNodeId + ":demon-trio-2464-local", chapter: 3),
            Patrol(2468, StandardShanheSourceZhangHePackage.GetGeneralId(tier), 4, 1 + (tier >= 21 ? 1 : 0), []),
            ReturningMixedOrdinary(2492, difficulty, ThirdChapterPatrolNodeId + ":jiangdong-trio-2492-local", chapter: 3)
        };
        var patrol = new ShanheNodeDefinition(ThirdChapterPatrolNodeId, "第三章 · 京畿巡军", ShanheNodeKind.Battle,
            camp.NextNodeIds, encounters, rewards)
        {
            ChapterNumber = 3, ChapterTitle = "第三章 · 京畿巡军", CategorizedRewards = true, AutomaticRewardCategory = true,
            PrimaryTacticRarity = 2, PrimarySkillRarity = 2, RewardSkipGold = 100,
            StoryText = "京畿巡军封住去路。可先清剿巡军取得补给，再选择最终路线或挑战东吴精锐。",
            Description = "可选前战，遭遇巡军或三将阵容。胜利后继续最终路线。"
        };
        return ShanheCampaignContent.FreezeAndValidate(definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id == camp.Id
                ? node with { NextNodeIds = camp.NextNodeIds.Append(patrol.Id).ToArray() } : node).Append(patrol).ToArray()
        });

        ShanheEncounterDefinition Patrol(int group, string general, int baseHp, int slashBonus,
            IReadOnlyList<ShanheStartingCard> cards) => new(ThirdChapterPatrolNodeId + ":troop-" + group,
                OrdinaryTroopName(group), [new(general, MaxHpBonus: 10 + 3 * growth - baseHp,
                    Hp: 10 + 3 * growth, InitialHandBonus: 2 + growth + Source2026EnemyDataByGroup[group].Single().TierAttributes
                        .Where(a => a.DifficultyGroupId <= (tier + 4) / 5).Sum(a => a.Attributes.StartingCards),
                    DrawBonus: 2, SlashBonus: slashBonus)
                    { StartingCards = cards }], [], VictoryGold: 200, HealAfterVictory: 1)
                { StoryText = OrdinaryTroopStory(group) };
    }

    private static IReadOnlyList<ShanheStartingCard> ExecutionerGear(int sourceDifficulty) => sourceDifficulty >= 21
        ? Array.AsReadOnly(new[] { new ShanheStartingCard(CardKind.HuxinMirror, Suit.Diamond, 2) }) : [];
}
