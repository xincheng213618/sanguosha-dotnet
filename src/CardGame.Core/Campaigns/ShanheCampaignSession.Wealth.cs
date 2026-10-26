namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private static IEnumerable<string> PotentialBattleTacticSkills(ShanheRewardDefinition reward) =>
        new[] { reward.TacticSkillId, reward.CampaignEconomy?.BattleSkillId }.OfType<string>();

    // Battle instances retain acquisition multiplicity; school recipes deduplicate their component sources separately.
    internal IReadOnlyList<string> GetBattleTacticSkills(int entryGold, bool bossBattle) =>
        BattleTacticSkills(_state, entryGold, bossBattle);

    private IReadOnlyList<string> BattleTacticSkills(RunState state, int entryGold, bool bossBattle) => ShanheCampaignContent.Freeze(
        state.TacticIds.SelectMany(id =>
        {
            var reward = _rewards[id];
            if (reward.BossBattleOnly && !bossBattle) return Array.Empty<string>();
            var economy = reward.CampaignEconomy;
            return economy?.BattleSkillId is { } skill && economy.GoldAbove is { } threshold && entryGold > threshold
                ? new[] { skill }
                : new[] { reward.TacticSkillId }.OfType<string>();
        }).Concat(SchoolProgress(state).Where(progress => progress.IsComplete)
            .Select(progress => ShanheSchoolEffects.SkillId(progress.School)).OfType<string>()));
}
