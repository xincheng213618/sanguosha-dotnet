using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentGuandaoRewardId = "tactic-guandao-current-2010";
    private static ShanheRewardDefinition CurrentGuandaoReward => new(CurrentGuandaoRewardId, "战法·关刀之脊")
    {
        TacticSkillId = StandardShanheGuandaoPackage.SkillId, Rarity = 1,
        Description = "方片【杀】无距离限制。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentGuandaoShopMetadata =>
        [new(CurrentGuandaoRewardId, new(2010, 200, 1, "战法"))];

    // There is no fixed source-2010 node in the pinned S5 tree.
    private static IEnumerable<string> CurrentGuandaoDefaultUnlockedRewardIds => [CurrentGuandaoRewardId];

    private static ShanheCampaignDefinition WithCurrentGuandaoTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentGuandaoReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentGuandaoRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentGuandaoRewardId, "战法·关刀之脊 · 200钱", 200,
                    RewardId: CurrentGuandaoRewardId, LeavesNode: false)
                    { Description = CurrentGuandaoReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentGuandaoRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheGuandaoPackage.SkillId, StringComparer.Ordinal);
}
