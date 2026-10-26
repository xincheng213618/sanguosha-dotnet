using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentKuangbao3RewardId = "tactic-kuangbao-current-2164";
    public const string CurrentKuangbao4RewardId = "tactic-kuangbao-current-2165";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentLowHpDrawRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentKuangbao3RewardId, "战法·狂暴Ⅲ")
        { TacticSkillId = StandardShanheLowHpDrawPackage.Kuangbao3SkillId, Rarity = 1,
            Description = "体力值小于等于3时，你的摸牌数+1" },
        new ShanheRewardDefinition(CurrentKuangbao4RewardId, "战法·狂暴Ⅳ")
        { TacticSkillId = StandardShanheLowHpDrawPackage.Kuangbao4SkillId, Rarity = 2,
            Description = "体力值小于等于5时，你的摸牌数+1" }
    });

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentLowHpDrawShopMetadata =>
        [new(CurrentKuangbao3RewardId, new(2164, 200, 1, "战法")), new(CurrentKuangbao4RewardId, new(2165, 300, 2, "战法"))];
    private static IEnumerable<string> CurrentLowHpDrawDefaultUnlockedRewardIds => [CurrentKuangbao3RewardId];
    private static IEnumerable<KeyValuePair<int, string>> CurrentLowHpDrawCodexBindings => [new(12, CurrentKuangbao4RewardId)];

    private static ShanheCampaignDefinition WithCurrentLowHpDrawTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentLowHpDrawRewards.Select(reward => reward.Id).ToArray();
        var choices = CurrentLowHpDrawRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, reward.Name, ShopMetadataByRewardId[reward.Id].Price,
            RewardId: reward.Id, LeavesNode: false) { Description = reward.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentLowHpDrawRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }

    private static bool RequiresCurrentLowHpDrawRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheLowHpDrawPackage.Kuangbao3SkillId or StandardShanheLowHpDrawPackage.Kuangbao4SkillId);
}
