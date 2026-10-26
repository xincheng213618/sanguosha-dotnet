namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public IReadOnlyList<string> AllyRankRebirthUsedKeys { get; init; } = Array.Empty<string>();
    }

    // Display metadata only; the mode and native school program apply all actual rank bonuses.
    private int GetSharedAllyRank(RunState state) => IsSchoolComplete(state, 6) ? Math.Max(3, _options.PlayerRank ?? 1) : 0;

    private string AllyRankRebirthKey(RunState state, string encounterId, ShanheCombatantDefinition member, int index) =>
        state.StartingCompanions.Concat(state.RecruitedCompanions).Any(companion => companion.GeneralId == member.GeneralId)
            ? "companion:" + member.GeneralId
            : "encounter:" + encounterId + ":" + index.ToString(System.Globalization.CultureInfo.InvariantCulture) + ":" + member.GeneralId;

    private IReadOnlyList<int> GetAllyRankRebirthUsedSeats(RunState state, ShanheEncounterDefinition encounter) =>
        ShanheCampaignContent.Freeze((encounter.Allies ?? []).Select((member, index) => (member, index))
            .Where(entry => state.AllyRankRebirthUsedKeys.Contains(
                AllyRankRebirthKey(state, encounter.Id, entry.member, entry.index), StringComparer.Ordinal))
            .Select(entry => entry.index + 1));

    private RunState ApplyTeamRebirthUsage(RunState state, GameEngine battle, ShanheBattleSetup setup)
    {
        if (setup.PlayerRank != 5 || state.EncounterId is not { } encounterId) return state;
        var used = battle.Events.Select(envelope => envelope.Payload).OfType<SkillUsageConsumedEvent>()
            .Where(usage => ShanheTeamRankPrograms.IsAllyExpeditionRebirthUsage(setup, usage))
            .Select(usage => AllyRankRebirthKey(state, encounterId, setup.AllyRoster[usage.SkillOwnerSeat - 1], usage.SkillOwnerSeat - 1));
        return state with { AllyRankRebirthUsedKeys = ShanheCampaignContent.Freeze(state.AllyRankRebirthUsedKeys.Concat(used).Distinct(StringComparer.Ordinal)) };
    }
}
