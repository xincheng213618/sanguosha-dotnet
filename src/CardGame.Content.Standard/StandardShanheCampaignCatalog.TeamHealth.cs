using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentTeamHealthRewardId = "tactic-duiti-current-2199";
    private static ShanheRewardDefinition CurrentTeamHealthReward => new(CurrentTeamHealthRewardId, "战法·队·体")
    {
        TacticSkillId = ShanheTeamRankPrograms.TeamHealthSkillId, Rarity = 3,
        Description = "你的所有队友体力+3。"
    };
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentTeamHealthShopMetadata =>
        [new(CurrentTeamHealthRewardId, new(2199, 500, 3, "战法"))];

    private static ShanheCampaignDefinition WithCurrentTeamHealthTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentTeamHealthReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentTeamHealthRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentTeamHealthRewardId, "战法·队·体 · 500钱", 500,
                    RewardId: CurrentTeamHealthRewardId, LeavesNode: false) { Description = CurrentTeamHealthReward.Description }).ToArray()
                : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentTeamHealthRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(ShanheTeamRankPrograms.TeamHealthSkillId, StringComparer.Ordinal);
}
