using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentSteadyHandRewardId = "tactic-xinshou-nianlai-current-2166";
    private static ShanheRewardDefinition CurrentSteadyHandReward => new(CurrentSteadyHandRewardId, "战法·信手拈来")
    {
        TacticSkillId = StandardShanheSteadyHandPackage.SkillId, Rarity = 2,
        Description = "你的手牌上限不因体力值改变而改变。"
    };
    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentSteadyHandShopMetadata =>
        [new(CurrentSteadyHandRewardId, new(2166, 300, 2, "战法"))];
    private static IEnumerable<string> CurrentSteadyHandDefaultUnlockedRewardIds => [CurrentSteadyHandRewardId];

    private static ShanheCampaignDefinition WithCurrentSteadyHandTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentSteadyHandReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentSteadyHandRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentSteadyHandRewardId, "战法·信手拈来 · 300钱", 300,
                    RewardId: CurrentSteadyHandRewardId, LeavesNode: false)
                    { Description = CurrentSteadyHandReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };
    private static bool RequiresCurrentSteadyHandRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheSteadyHandPackage.SkillId, StringComparer.Ordinal);
}
