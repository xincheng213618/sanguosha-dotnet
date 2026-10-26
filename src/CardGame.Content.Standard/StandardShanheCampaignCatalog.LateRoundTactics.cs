using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentYuzhanSkillId = "shanhe:current-yuzhan-2372";
    public const string CurrentYuzhanRewardId = "tactic-yuzhan-current-2372";
    public const string CurrentYuzhan2SkillId = "shanhe:current-yuzhan-2373";
    public const string CurrentYuzhan2RewardId = "tactic-yuzhan-current-2373";
    public const string CurrentYuzhan3SkillId = "shanhe:current-yuzhan-2374";
    public const string CurrentYuzhan3RewardId = "tactic-yuzhan-current-2374";
    public const string CurrentBuzhenSkillId = "shanhe:current-buzhen-2375";
    public const string CurrentBuzhenRewardId = "tactic-buzhen-current-2375";
    public const string CurrentBuzhen2SkillId = "shanhe:current-buzhen-2376";
    public const string CurrentBuzhen2RewardId = "tactic-buzhen-current-2376";
    public const string CurrentBuzhen3SkillId = "shanhe:current-buzhen-2377";
    public const string CurrentBuzhen3RewardId = "tactic-buzhen-current-2377";
    public const string CurrentBattleLearningSkillId = "shanhe:current-battle-learning-2378";
    public const string CurrentBattleLearningRewardId = "tactic-battle-learning-current-2378";
    public const string CurrentBattleLearning2SkillId = "shanhe:current-battle-learning-2379";
    public const string CurrentBattleLearning2RewardId = "tactic-battle-learning-current-2379";
    public const string CurrentBattleLearning3SkillId = "shanhe:current-battle-learning-2380";
    public const string CurrentBattleLearning3RewardId = "tactic-battle-learning-current-2380";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentLateRoundTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentYuzhanRewardId, "战法·愈战愈勇")
        { TacticSkillId = CurrentYuzhanSkillId, Rarity = 3, Description = "从第2轮开始，你的杀造成的伤害+2。" },
        new ShanheRewardDefinition(CurrentYuzhan2RewardId, "战法·愈战愈勇Ⅱ")
        { TacticSkillId = CurrentYuzhan2SkillId, Rarity = 2, Description = "从第2轮开始，你的杀造成的伤害+1。" },
        new ShanheRewardDefinition(CurrentYuzhan3RewardId, "战法·愈战愈勇Ⅲ")
        { TacticSkillId = CurrentYuzhan3SkillId, Rarity = 1, Description = "从第3轮开始，你的杀造成的伤害+1。" },
        new ShanheRewardDefinition(CurrentBuzhenRewardId, "战法·布阵")
        { TacticSkillId = CurrentBuzhenSkillId, Rarity = 3, Description = "从第2轮开始，你的摸牌数+4。" },
        new ShanheRewardDefinition(CurrentBuzhen2RewardId, "战法·布阵Ⅱ")
        { TacticSkillId = CurrentBuzhen2SkillId, Rarity = 2, Description = "从第2轮开始，你的摸牌数+3。" },
        new ShanheRewardDefinition(CurrentBuzhen3RewardId, "战法·布阵Ⅲ")
        { TacticSkillId = CurrentBuzhen3SkillId, Rarity = 1, Description = "从第3轮开始，你的摸牌数+2。" },
        new ShanheRewardDefinition(CurrentBattleLearningRewardId, "战法·战斗学习")
        { TacticSkillId = CurrentBattleLearningSkillId, Rarity = 3, Description = "从第2轮开始，你的出杀次数+4。" },
        new ShanheRewardDefinition(CurrentBattleLearning2RewardId, "战法·战斗学习Ⅱ")
        { TacticSkillId = CurrentBattleLearning2SkillId, Rarity = 2, Description = "从第2轮开始，你的出杀次数+2。" },
        new ShanheRewardDefinition(CurrentBattleLearning3RewardId, "战法·战斗学习Ⅲ")
        { TacticSkillId = CurrentBattleLearning3SkillId, Rarity = 1, Description = "从第3轮开始，你的出杀次数+2。" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentLateRoundTacticShopMetadata =>
        [new(CurrentYuzhanRewardId, new(2372, 500, 3, "战法")),
         new(CurrentYuzhan2RewardId, new(2373, 300, 2, "战法")), new(CurrentYuzhan3RewardId, new(2374, 200, 1, "战法")),
         new(CurrentBuzhenRewardId, new(2375, 500, 3, "战法")), new(CurrentBuzhen2RewardId, new(2376, 300, 2, "战法")),
         new(CurrentBuzhen3RewardId, new(2377, 200, 1, "战法")),
         new(CurrentBattleLearningRewardId, new(2378, 500, 3, "战法")), new(CurrentBattleLearning2RewardId, new(2379, 300, 2, "战法")),
         new(CurrentBattleLearning3RewardId, new(2380, 200, 1, "战法"))];
    private static IEnumerable<string> CurrentLateRoundTacticDefaultUnlockedRewardIds =>
        CurrentLateRoundTacticRewards.Where(r => r.Id != CurrentYuzhanRewardId).Select(r => r.Id);
    private static IEnumerable<KeyValuePair<int, string>> CurrentLateRoundTacticCodexBindings => [new(68, CurrentYuzhanRewardId)];

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
            .Any(id => CurrentLateRoundTacticRewards.Any(reward => reward.TacticSkillId == id));
}
