using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentYuzhan2SkillId = "shanhe:current-yuzhan-2373";
    public const string CurrentYuzhan2RewardId = "tactic-yuzhan-current-2373";
    public const string CurrentYuzhan3SkillId = "shanhe:current-yuzhan-2374";
    public const string CurrentYuzhan3RewardId = "tactic-yuzhan-current-2374";
    public const string CurrentBattleLearning3SkillId = "shanhe:current-battle-learning-2380";
    public const string CurrentBattleLearning3RewardId = "tactic-battle-learning-current-2380";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentLateRoundTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentYuzhan2RewardId, "战法·愈战愈勇Ⅱ")
        { TacticSkillId = CurrentYuzhan2SkillId, Rarity = 2, Description = "从第2轮开始，你的杀造成的伤害+1。" },
        new ShanheRewardDefinition(CurrentYuzhan3RewardId, "战法·愈战愈勇Ⅲ")
        { TacticSkillId = CurrentYuzhan3SkillId, Rarity = 1, Description = "从第3轮开始，你的杀造成的伤害+1。" },
        new ShanheRewardDefinition(CurrentBattleLearning3RewardId, "战法·战斗学习Ⅲ")
        { TacticSkillId = CurrentBattleLearning3SkillId, Rarity = 1, Description = "从第3轮开始，你的出杀次数+2。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentLateRoundTacticShopMetadata =>
        [new(CurrentYuzhan2RewardId, new(2373, 300, 2, "战法")), new(CurrentYuzhan3RewardId, new(2374, 200, 1, "战法")),
         new(CurrentBattleLearning3RewardId, new(2380, 200, 1, "战法"))];
    private static IEnumerable<string> CurrentLateRoundTacticDefaultUnlockedRewardIds => CurrentLateRoundTacticRewards.Select(r => r.Id);

    private static ShanheCampaignDefinition WithCurrentLateRoundTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentLateRoundTacticRewards.Select(r => r.Id).ToArray();
        var choices = CurrentLateRoundTacticRewards.Select(r => new ShanheNodeChoiceDefinition(
            "buy-" + r.Id, r.Name, ShopMetadataByRewardId[r.Id].Price, RewardId: r.Id, LeavesNode: false) { Description = r.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentLateRoundTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }
    private static bool RequiresCurrentLateRoundTacticsRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(m => m.ExtraSkillIds ?? []))
            .Any(id => id is CurrentYuzhan2SkillId or CurrentYuzhan3SkillId or CurrentBattleLearning3SkillId);
}
