using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentYongzhanRewardId = "tactic-yongzhan-current-2174";
    public const string CurrentYongzhan2RewardId = "tactic-yongzhan-current-2175";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentYongzhanRewards =>
    [
        new(CurrentYongzhanRewardId, "战法·勇战Ⅰ")
        { TacticSkillId = StandardShanheYongzhanPackage.SkillId, Rarity = 2, Description = "你脱离濒死时，对所有敌方造成1点伤害。" },
        new(CurrentYongzhan2RewardId, "战法·勇战Ⅱ")
        { TacticSkillId = StandardShanheYongzhanPackage.SecondSkillId, Rarity = 4, Description = "你脱离濒死时，对所有敌方造成2点伤害。" }
    ];
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentYongzhanShopMetadata =>
    [new(CurrentYongzhanRewardId, new(2174, 300, 2, "战法")), new(CurrentYongzhan2RewardId, new(2175, 700, 4, "战法"))];

    // Only source 2175 has a pinned S5 research node (144).
    private static IEnumerable<string> CurrentYongzhanDefaultUnlockedRewardIds => [CurrentYongzhanRewardId];

    private static ShanheCampaignDefinition WithCurrentYongzhanTactics(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Concat(CurrentYongzhanRewards).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Concat(CurrentYongzhanRewards.Select(reward => reward.Id)).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(CurrentYongzhanShopMetadata.Select(pair =>
                new ShanheNodeChoiceDefinition("buy-" + pair.Key, CurrentYongzhanRewards.Single(reward => reward.Id == pair.Key).Name,
                    pair.Value.Price, RewardId: pair.Key, LeavesNode: false)
                    { Description = CurrentYongzhanRewards.Single(reward => reward.Id == pair.Key).Description })).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentYongzhanRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheYongzhanPackage.SkillId or StandardShanheYongzhanPackage.SecondSkillId);
}
