using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentQiaoquHaoduoRewardId = "tactic-qiaoqu-haoduo-current-2244";
    private static ShanheRewardDefinition CurrentQiaoquHaoduoReward => new(CurrentQiaoquHaoduoRewardId, "战法·巧取豪夺")
    {
        TacticSkillId = StandardShanheQiaoquHaoduoPackage.SkillId, Rarity = 2,
        Description = "因你的【借刀杀人】使用的【杀】伤害+1。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentQiaoquHaoduoShopMetadata =>
        [new(CurrentQiaoquHaoduoRewardId, new(2244, 300, 2, "战法"))];

    // The pinned current S5 tree has no fixed source-2244 node.
    private static IEnumerable<string> CurrentQiaoquHaoduoDefaultUnlockedRewardIds => [CurrentQiaoquHaoduoRewardId];

    private static ShanheCampaignDefinition WithCurrentQiaoquHaoduoTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentQiaoquHaoduoReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentQiaoquHaoduoRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentQiaoquHaoduoRewardId, "战法·巧取豪夺 · 300钱", 300,
                    RewardId: CurrentQiaoquHaoduoRewardId, LeavesNode: false)
                    { Description = CurrentQiaoquHaoduoReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentQiaoquHaoduoRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheQiaoquHaoduoPackage.SkillId, StringComparer.Ordinal);
}
