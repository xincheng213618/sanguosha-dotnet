namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    public IReadOnlyList<ShanheSchoolProgress> GetSchoolProgress() => SchoolProgress(_state);

    private IReadOnlyList<ShanheSchoolProgress> SchoolProgress(RunState state)
    {
        var schools = Definition.Schools ?? [];
        if (schools.Count == 0) return [];
        var counts = schools.ToDictionary(s => s.Id, _ => new int[4]);
        var identities = new HashSet<string>(StringComparer.Ordinal);
        var sources = new HashSet<int>();
        // Actual carried or acquired rewards only. Research access is not ownership.
        foreach (var rewardId in state.TacticIds)
        {
            var reward = _rewards[rewardId];
            if (reward.SchoolMembership is not { } member ||
                !counts.ContainsKey(member.SchoolId)) continue;
            var identity = RewardAcquisitionIdentity(reward);
            if (identities.Contains(identity) || sources.Contains(member.SourceId)) continue;
            identities.Add(identity); sources.Add(member.SourceId);
            counts[member.SchoolId][member.Rarity - 1]++;
        }
        return ShanheCampaignContent.Freeze(schools.Select(school => new ShanheSchoolProgress(school,
            counts[school.Id], school.Requirements.Select((required, tier) => counts[school.Id][tier] >= required).All(met => met))));
    }

    private bool IsSchoolComplete(RunState state, int schoolId) =>
        SchoolProgress(state).Any(progress => progress.School.Id == schoolId && progress.IsComplete);
}
