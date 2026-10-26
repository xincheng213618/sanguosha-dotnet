using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public const string CurrentHeartBladeRewardId = "tactic-guangongren-current-2045";
    private static ShanheRewardDefinition CurrentHeartBladeReward => new(CurrentHeartBladeRewardId, "战法·关公刃")
    {
        TacticSkillId = StandardShanheHeartBladePackage.SkillId, Rarity = 4,
        Description = "红桃【杀】伤害+2。"
    };

    private static IEnumerable<KeyValuePair<string, ShanheShopMetadata>> CurrentHeartBladeShopMetadata =>
        [new(CurrentHeartBladeRewardId, new(2045, 700, 4, "战法"))];

    // The pinned current S5 tree has no fixed source-2045 node.
    private static IEnumerable<string> CurrentHeartBladeDefaultUnlockedRewardIds => [CurrentHeartBladeRewardId];

    private static ShanheCampaignDefinition WithCurrentHeartBladeTactic(ShanheCampaignDefinition definition) => definition with
    {
        Rewards = definition.Rewards.Append(CurrentHeartBladeReward).ToArray(),
        Nodes = definition.Nodes.Select(node => node with
        {
            RewardIds = node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle
                ? (node.RewardIds ?? []).Append(CurrentHeartBladeRewardId).ToArray() : node.RewardIds,
            Choices = node.Kind == ShanheNodeKind.Shop
                ? (node.Choices ?? []).Append(new("buy-" + CurrentHeartBladeRewardId, "战法·关公刃 · 700钱", 700,
                    RewardId: CurrentHeartBladeRewardId, LeavesNode: false)
                    { Description = CurrentHeartBladeReward.Description }).ToArray() : node.Choices
        }).ToArray()
    };

    private static bool RequiresCurrentHeartBladeRegistry(ShanheBattleSetup setup) =>
        setup.TacticSkillIds.Concat(setup.ExtraSkillIds)
            .Concat(setup.AllyRoster.Concat(setup.EnemyRoster).SelectMany(member => member.ExtraSkillIds ?? []))
            .Contains(StandardShanheHeartBladePackage.SkillId, StringComparer.Ordinal);
}
