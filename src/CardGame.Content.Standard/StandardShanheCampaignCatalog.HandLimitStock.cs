using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentHaoshenfaRewardId = "tactic-haoshenfa-current-2002";
    public const string CurrentCangtaohuRewardId = "tactic-cangtaohu-current-2003";
    public const string CurrentStableCapacityRewardId = "tactic-stable-capacity-current-2211";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentHandLimitStockRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentHaoshenfaRewardId, "战法·好身法")
        { TacticSkillId = StandardShanheHandLimitStockPackage.HaoshenfaSkillId, Rarity = 3, Description = "【闪】不计入手牌上限。" },
        new ShanheRewardDefinition(CurrentCangtaohuRewardId, "战法·藏桃户")
        { TacticSkillId = StandardShanheHandLimitStockPackage.CangtaohuSkillId, Rarity = 1, Description = "【桃】不计入手牌上限。" },
        new ShanheRewardDefinition(CurrentStableCapacityRewardId, "战法·稳定承载")
        { TacticSkillId = StandardShanheHandLimitStockPackage.StableCapacitySkillId, Rarity = 1, Description = "手牌上限等于你的体力上限。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentHandLimitStockShopMetadata =>
        [new(CurrentHaoshenfaRewardId, new(2002, 500, 3, "战法")), new(CurrentCangtaohuRewardId, new(2003, 200, 1, "战法")),
         new(CurrentStableCapacityRewardId, new(2211, 200, 1, "战法"))];
    // These three source IDs have no fixed node in the pinned S5 tree.
    private static IEnumerable<string> CurrentHandLimitStockDefaultUnlockedRewardIds => CurrentHandLimitStockRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentHandLimitStockTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentHandLimitStockRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentHandLimitStockRewards.Select(reward => new ShanheNodeChoiceDefinition("buy-" + reward.Id,
            reward.Name, ShopMetadataByRewardId[reward.Id].Price, RewardId: reward.Id, LeavesNode: false)
            { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentHandLimitStockRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentHandLimitStockRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheHandLimitStockPackage.HaoshenfaSkillId or StandardShanheHandLimitStockPackage.CangtaohuSkillId or
                StandardShanheHandLimitStockPackage.StableCapacitySkillId);
}
