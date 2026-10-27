using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentChenqibubeiRewardId = "tactic-chenqibubei-current-2239";
    public const string CurrentChenqibubei2RewardId = "tactic-chenqibubei-current-2240";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentDismantlementStockRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentChenqibubeiRewardId, "战法·趁其不备")
        { TacticSkillId = StandardShanheDismantlementStockPackage.SkillId, Rarity = 3,
            Description = "你使用【过河拆桥】时，至多弃置目标2张牌。" },
        new ShanheRewardDefinition(CurrentChenqibubei2RewardId, "战法·趁其不备Ⅱ")
        { TacticSkillId = StandardShanheDismantlementStockPackage.UpgradedSkillId, Rarity = 4,
            Description = "你使用【过河拆桥】时，至多弃置目标3张牌。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentDismantlementStockShopMetadata =>
        [new(CurrentChenqibubeiRewardId, new(2239, 500, 3, "战法")), new(CurrentChenqibubei2RewardId, new(2240, 700, 4, "战法"))];
    private static IEnumerable<string> CurrentDismantlementStockDefaultUnlockedRewardIds => [CurrentChenqibubeiRewardId];
    private static IEnumerable<KeyValuePair<int, string>> CurrentDismantlementStockCodexBindings => [new(135, CurrentChenqibubei2RewardId)];

    private static ShanheCampaignDefinition WithCurrentDismantlementStockTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentDismantlementStockRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentDismantlementStockRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, ShopMetadataByRewardId[reward.Id].Price,
            RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentDismantlementStockRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentDismantlementStockRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheDismantlementStockPackage.SkillId or StandardShanheDismantlementStockPackage.UpgradedSkillId);
}
