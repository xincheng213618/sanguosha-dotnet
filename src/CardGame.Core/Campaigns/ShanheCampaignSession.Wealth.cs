namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private static IEnumerable<string> PotentialBattleTacticSkills(ShanheRewardDefinition reward) =>
        new[] { reward.TacticSkillId, reward.CampaignEconomy?.BattleSkillId }.OfType<string>();

    private IReadOnlyList<string> BattleTacticSkills(RunState state) => ShanheCampaignContent.Freeze(
        state.TacticIds.SelectMany(id =>
        {
            var reward = _rewards[id];
            var economy = reward.CampaignEconomy;
            return economy?.BattleSkillId is { } skill && economy.GoldAbove is { } threshold && state.Gold > threshold
                ? new[] { skill }
                : new[] { reward.TacticSkillId }.OfType<string>();
        }).Distinct(StringComparer.Ordinal));
}
