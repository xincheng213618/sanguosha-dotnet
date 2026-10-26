namespace CardGame.Core.Campaigns;

public static class ShanheTeamRankPrograms
{
    public const string SharedRebirthSkillId = "shanhe:school-team-rebirth-2235";
    public const string TeamHealthSkillId = "shanhe:current-duiti-2199";

    public static int EffectiveRank(ShanheBattleSetup setup) => Math.Max(3, setup.PlayerRank ?? 1);

    public static ShanheTacticBonuses EntryBonuses(ShanheBattleSetup setup) =>
        setup.ActiveSchoolIds.Contains(6) && setup.TacticSkillIds.Contains(ShanheSchoolEffects.TeamRankSkillId, StringComparer.Ordinal)
            ? ShanheGeneralRanks.GetBonuses(EffectiveRank(setup)) : new();

    // Both uses consume one expedition allowance. A teammate cannot refresh the
    // player's spent privilege merely by entering the next encounter.
    public static bool IsExpeditionRebirthUsage(ShanheBattleSetup setup, SkillUsageConsumedEvent usage) =>
        usage.Scope == SkillUsageScope.Game && usage.Count > 0 &&
        (usage.SkillId == ShanheGeneralRanks.RebirthSkillId && usage.SkillOwnerSeat == 0 ||
         usage.SkillId == SharedRebirthSkillId && usage.SkillOwnerSeat > 0 && usage.SkillOwnerSeat <= setup.AllyRoster.Count);
}
