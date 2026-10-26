using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentZuiquanRewardId = "tactic-zuiquan-current-2001";
    private static ShanheRewardDefinition CurrentZuiquanReward => new(CurrentZuiquanRewardId, "战法·醉拳")
    {
        TacticSkillId = StandardShanheZuiquanPackage.SkillId, Rarity = 3,
        Description = "【酒】【杀】不能被抵消。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentZuiquanShopMetadata =>
        [new(CurrentZuiquanRewardId, new(2001, 500, 3, "战法"))];

    // S5 node 125 is a random epic tactic node, not a fixed source-2001 unlock.
    private static IEnumerable<string> CurrentZuiquanDefaultUnlockedRewardIds => [CurrentZuiquanRewardId];

    private static ShanheCampaignDefinition WithCurrentZuiquanTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentZuiquanReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentZuiquanRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentZuiquanRewardId, "战法·醉拳 · 500钱", 500,
                    RewardId: CurrentZuiquanRewardId, LeavesNode: false)
                    { Description = CurrentZuiquanReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentZuiquanRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheZuiquanPackage.SkillId, StringComparer.Ordinal);
}
