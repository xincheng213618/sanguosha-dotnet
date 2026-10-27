using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentBadaoRewardId = "tactic-badao-current-2188";
    public const string CurrentBadao2RewardId = "tactic-badao-current-2189";
    private static IReadOnlyList<ShanheRewardDefinition> CurrentBadaoRewards =>
    [
        new(CurrentBadaoRewardId, "战法·拔刀术")
        { TacticSkillId = StandardShanheBadaoPackage.SkillId, Rarity = 3,
            Description = "若上一轮你未造成任何伤害，则你本轮造成的所有伤害+1。" },
        new(CurrentBadao2RewardId, "战法·拔刀术Ⅱ")
        { TacticSkillId = StandardShanheBadaoPackage.SecondSkillId, Rarity = 2,
            Description = "若上一轮造成的伤害小于3，则你本轮造成的所有伤害+1。" }
    ];
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentBadaoShopMetadata =>
        [new(CurrentBadaoRewardId, new(2188, 500, 3, "战法")), new(CurrentBadao2RewardId, new(2189, 300, 2, "战法"))];
    private static IEnumerable<string> CurrentBadaoDefaultUnlockedRewardIds => CurrentBadaoRewards.Select(reward => reward.Id);

    private static ShanheCampaignDefinition WithCurrentBadaoTactics(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Concat(CurrentBadaoRewards).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Concat(CurrentBadaoRewards.Select(reward => reward.Id)).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(CurrentBadaoShopMetadata.Select(pair =>
                new ShanheNodeChoiceDefinition("buy-" + pair.Key, CurrentBadaoRewards.Single(reward => reward.Id == pair.Key).Name,
                    pair.Value.Price, RewardId: pair.Key, LeavesNode: false)
                    { Description = CurrentBadaoRewards.Single(reward => reward.Id == pair.Key).Description })).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentBadaoRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is StandardShanheBadaoPackage.SkillId or StandardShanheBadaoPackage.SecondSkillId);
}
