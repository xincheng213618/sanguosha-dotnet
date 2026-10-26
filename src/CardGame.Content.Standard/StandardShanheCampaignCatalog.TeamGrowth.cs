using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Current public tactics.json schoolId=6; no same-name or rarity inference.
    private static readonly IReadOnlyDictionary<string, int> TeamGrowthSourceRewards = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        [OpeningCompanionRewardId] = 2219, [EliteOpeningCompanionRewardId] = 2220,
        ["tactic-duichu-2026"] = 2202, ["tactic-jiban-2026"] = 2204,
        ["tactic-duipai-2026"] = 2201, ["tactic-duijia-2026"] = 2200,
        ["tactic-duisha-2026"] = 2329, [ShanheTeamTactics.HealthRewardId] = 2199
    };

    private static ShanheCampaignDefinition WithTeamGrowth(ShanheCampaignDefinition definition, string editionId)
    {
        if (editionId != Returning2026EditionId) return definition;
        var rewards = definition.Rewards.Append(new(ShanheTeamTactics.HealthRewardId, "战法·队·体")
        {
            TacticSkillId = ShanheTeamTactics.HealthSkillId, Rarity = 3,
            Description = "史诗队友流战法：每场开战，其他队友体力上限+3并回复3点，先于先辅选择。原价500铜币，本地每征程限一份。"
        }).Append(new(ShanheTeamTactics.SetRewardId, "队友流套装")
        {
            Rarity = 4,
            Description = "持有至少2个普通、2个稀有和1个史诗队友流战法后自动激活。队友共享你的等阶效果，最低按三阶；五阶重生按每名队友本次征程各一次，与玩家独立。"
        }).Select(reward => TeamGrowthSourceRewards.ContainsKey(reward.Id)
            ? reward with { TacticSchoolId = ShanheTeamTactics.SchoolId,
                Rarity = reward.Id == OpeningCompanionRewardId ? 2 : reward.Rarity }
            : reward).ToArray();
        return definition with
        {
            TeamRankSetRewardId = ShanheTeamTactics.SetRewardId,
            Rewards = rewards,
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Append(ShanheTeamTactics.HealthRewardId).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Append(new("buy-team-health-2199", "战法·队·体 · 500钱", 500,
                    RewardId: ShanheTeamTactics.HealthRewardId, LeavesNode: false)).ToArray() : node.Choices
            }).ToArray()
        };
    }
}
