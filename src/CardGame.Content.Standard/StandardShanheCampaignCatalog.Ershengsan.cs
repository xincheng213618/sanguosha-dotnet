using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentErshengsanRewardId = "tactic-ershengsan-current-2030";
    private static ShanheRewardDefinition CurrentErshengsanReward => new(CurrentErshengsanRewardId, "战法·二生三")
    {
        TacticSkillId = StandardShanheErshengsanPackage.SkillId, Rarity = 2,
        Description = "【无中生有】额外摸1张牌。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentErshengsanShopMetadata =>
        [new(CurrentErshengsanRewardId, new(2030, 300, 2, "战法"))];

    // The pinned current S5 tree contains no fixed source-2030 node.
    private static IEnumerable<string> CurrentErshengsanDefaultUnlockedRewardIds => [CurrentErshengsanRewardId];

    private static ShanheCampaignDefinition WithCurrentErshengsanTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentErshengsanReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentErshengsanRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentErshengsanRewardId, "战法·二生三 · 300钱", 300,
                    RewardId: CurrentErshengsanRewardId, LeavesNode: false)
                    { Description = CurrentErshengsanReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentErshengsanRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheErshengsanPackage.SkillId, StringComparer.Ordinal);
}
