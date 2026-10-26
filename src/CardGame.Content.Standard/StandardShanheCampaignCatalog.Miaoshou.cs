using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentMiaoshouRewardId = "tactic-miaoshou-current-2238";
    private static ShanheRewardDefinition CurrentMiaoshouReward => new(CurrentMiaoshouRewardId, "战法·妙手空空")
    {
        TacticSkillId = StandardShanheMiaoshouPackage.SkillId,
        Description = "你使用【顺手牵羊】无距离限制。"
    };
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentMiaoshouShopMetadata =>
        [new(CurrentMiaoshouRewardId, new(2238, 300, 2, "战法"))];
    // The pinned S5 tree has no node for source 2238, so it enters ordinary stock.
    private static IEnumerable<string> CurrentMiaoshouDefaultUnlockedRewardIds => [CurrentMiaoshouRewardId];

    private static ShanheCampaignDefinition WithCurrentMiaoshouTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentMiaoshouReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentMiaoshouRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentMiaoshouRewardId, "战法·妙手空空 · 300钱", 300,
                    RewardId: CurrentMiaoshouRewardId, LeavesNode: false) { Description = CurrentMiaoshouReward.Description }).ToArray()
                : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentMiaoshouRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheMiaoshouPackage.SkillId, StringComparer.Ordinal);
}
