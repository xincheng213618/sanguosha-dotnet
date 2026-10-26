using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentHugujiuSkillId = "shanhe:current-hugujiu-2108";
    public const string CurrentHugujiuRewardId = "tactic-hugujiu-current-2108";
    public const string CurrentHugujiu2SkillId = "shanhe:current-hugujiu-2109";
    public const string CurrentHugujiu2RewardId = "tactic-hugujiu-current-2109";

    private static IReadOnlyList<ShanheRewardDefinition> CurrentWineTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentHugujiuRewardId, "战法·虎骨酒")
        { TacticSkillId = CurrentHugujiuSkillId, Rarity = 2, Description = "每回合，你可以额外使用1次【酒】" },
        new ShanheRewardDefinition(CurrentHugujiu2RewardId, "战法·虎骨酒Ⅱ")
        { TacticSkillId = CurrentHugujiu2SkillId, Rarity = 4, Description = "每回合，你可以额外使用2次【酒】" }
    });
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentWineTacticShopMetadata =>
        [new(CurrentHugujiuRewardId, new(2108, 300, 2, "战法")), new(CurrentHugujiu2RewardId, new(2109, 700, 4, "战法"))];
    // Neither pinned S5 source has a fixed codex node; both join ordinary stock.
    private static IEnumerable<string> CurrentWineTacticDefaultUnlockedRewardIds => CurrentWineTacticRewards.Select(r => r.Id);

    private static ShanheCampaignDefinition WithCurrentWineTactics(ShanheCampaignDefinition definition)
    {
        var ids = CurrentWineTacticRewards.Select(r => r.Id).ToArray();
        var choices = CurrentWineTacticRewards.Select(r => new ShanheNodeChoiceDefinition(
            "buy-" + r.Id, r.Name, ShopMetadataByRewardId[r.Id].Price, RewardId: r.Id, LeavesNode: false)
            { Description = r.Description }).ToArray();
        return definition with
        {
            Rewards = definition.Rewards.Concat(CurrentWineTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(ids).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        };
    }

    private static bool RequiresCurrentWineTacticsRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentHugujiuSkillId or CurrentHugujiu2SkillId);
}
