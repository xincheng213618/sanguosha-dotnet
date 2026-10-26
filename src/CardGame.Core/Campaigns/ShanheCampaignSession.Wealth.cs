namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private static IEnumerable<string> PotentialBattleTacticSkills(ShanheRewardDefinition reward) =>
        new[] { reward.TacticSkillId, reward.CampaignEconomy?.BattleSkillId }.OfType<string>();

    private IReadOnlyList<string> BattleTacticSkills(RunState state, ShanheNodeKind battleKind) => ShanheCampaignContent.Freeze(
        state.TacticIds.SelectMany(id =>
        {
            var reward = _rewards[id];
            if (reward.BossBattleOnly && battleKind != ShanheNodeKind.BossBattle) return Array.Empty<string>();
            var economy = reward.CampaignEconomy;
            return economy?.BattleSkillId is { } skill && economy.GoldAbove is { } threshold && state.Gold > threshold
                ? new[] { skill }
                : new[] { reward.TacticSkillId }.OfType<string>();
        }).Concat(SchoolProgress(state).Where(progress => progress.IsComplete)
            .Select(progress => ShanheSchoolEffects.SkillId(progress.School)).OfType<string>())
        .Distinct(StringComparer.Ordinal));
}
