using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentCuixue2SkillId = "shanhe:current-cuixue-2049";
    public const string CurrentCuixue2RewardId = "tactic-cuixue-current-2049";
    public const string CurrentYuanzhu3SkillId = "shanhe:current-yuanzhu-2078";
    public const string CurrentYuanzhu3RewardId = "tactic-yuanzhu-current-2078";
    public const string CurrentMuniuLiumaSkillId = "shanhe:current-muniu-liuma-2088";
    public const string CurrentMuniuLiumaRewardId = "tactic-muniu-liuma-current-2088";
    public const string CurrentSougua3SkillId = "shanhe:current-sougua-2170";
    public const string CurrentSougua3RewardId = "tactic-sougua-current-2170";
    public const string CurrentFenjinSkillId = "shanhe:current-fenjin-2228";
    public const string CurrentFenjinRewardId = "tactic-fenjin-current-2228";
    public const string CurrentFenjin2SkillId = "shanhe:current-fenjin-2229";
    public const string CurrentFenjin2RewardId = "tactic-fenjin-current-2229";
    public const string CurrentQinmianSkillId = "shanhe:current-qinmian-2360";
    public const string CurrentQinmianRewardId = "tactic-qinmian-current-2360";

    private static IReadOnlyList<ShanheRewardDefinition> BuildTacticRewards { get; } = Array.AsReadOnly(new[]
    {
        new ShanheRewardDefinition(CurrentCuixue2RewardId, "战法·淬血Ⅱ")
        { TacticSkillId = CurrentCuixue2SkillId, Description = "每轮，你的【杀】首次造成伤害后摸2张牌。" },
        new ShanheRewardDefinition(CurrentYuanzhu3RewardId, "战法·援助Ⅲ")
        { TacticSkillId = CurrentYuanzhu3SkillId, Description = "你的回合结束时，摸3张牌。" },
        new ShanheRewardDefinition(CurrentMuniuLiumaRewardId, "战法·木牛流马")
        { TacticSkillId = CurrentMuniuLiumaSkillId, Description = "摸牌阶段，摸牌数+2；手牌上限-1。" },
        new ShanheRewardDefinition(CurrentSougua3RewardId, "战法·搜刮Ⅲ")
        { TacticSkillId = CurrentSougua3SkillId, Description = "击杀其他角色后，回复4点体力。" },
        new ShanheRewardDefinition(CurrentFenjinRewardId, "战法·奋进")
        { TacticSkillId = CurrentFenjinSkillId, Description = "你的回合开始时，若体力大于2，失去1点体力并摸2张牌。" },
        new ShanheRewardDefinition(CurrentFenjin2RewardId, "战法·奋进Ⅱ")
        { TacticSkillId = CurrentFenjin2SkillId, Rarity = 4, Description = "你的回合开始时，若体力大于4，失去1点体力并摸1张牌。" },
        new ShanheRewardDefinition(CurrentQinmianRewardId, "战法·勤勉")
        { TacticSkillId = CurrentQinmianSkillId, Rarity = 1, Description = "你始终跳过判定阶段。" }
    });

    private static ShanheCampaignDefinition WithBuildTactics(ShanheCampaignDefinition definition)
    {
        var rewardIds = BuildTacticRewards.Select(reward => reward.Id).ToArray();
        var choices = BuildTacticRewards.Select(reward => new ShanheNodeChoiceDefinition(
            "buy-" + reward.Id, $"{reward.Name} · {BuildTacticPrice(reward.Id)}钱",
            GoldCost: BuildTacticPrice(reward.Id), RewardId: reward.Id, LeavesNode: false)).ToArray();
        return WithChapterRewards(definition with
        {
            Rewards = definition.Rewards.Concat(BuildTacticRewards).ToArray(),
            Nodes = definition.Nodes.Select(node => node with
            {
                RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                    ? (node.RewardIds ?? []).Concat(rewardIds).ToArray() : node.RewardIds,
                Choices = node.Kind == ShanheNodeKind.Shop ? (node.Choices ?? []).Concat(choices).ToArray() : node.Choices
            }).ToArray()
        });
    }

    private static int BuildTacticPrice(string rewardId) => rewardId switch
    {
        CurrentCuixue2RewardId or CurrentMuniuLiumaRewardId or CurrentSougua3RewardId => 500,
        CurrentYuanzhu3RewardId or CurrentFenjin2RewardId => 700,
        CurrentFenjinRewardId or CurrentQinmianRewardId => 200,
        _ => throw new ArgumentOutOfRangeException(nameof(rewardId), rewardId, "Unknown verified build tactic.")
    };

    private static bool RequiresBuildTacticsRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Any(id => id is CurrentCuixue2SkillId or CurrentYuanzhu3SkillId or CurrentMuniuLiumaSkillId or CurrentSougua3SkillId or CurrentFenjinSkillId or CurrentFenjin2SkillId or CurrentQinmianSkillId);
}
