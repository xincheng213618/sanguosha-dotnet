namespace CardGame.Core;

internal interface ISkillGrantRevisionObservations : IProgramRuntimeOperations
{
    void Capture(IEnumerable<CharacterState> players);
    bool Observe(CharacterState owner);
}

// One owner for observed grant revisions. Re-capturing an in-flight payment
// must retain its earlier baseline until the change window consumes it.
internal sealed class SkillGrantRevisionStore : ISkillGrantRevisionObservations
{
    internal Dictionary<int, long> Revisions { get; } = [];

    public void Capture(IEnumerable<CharacterState> players)
    {
        foreach (var player in players) Revisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
    }

    public bool Observe(CharacterState owner)
    {
        var changed = Revisions.TryGetValue(owner.Seat, out var revision) && revision != owner.SkillGrants.GrantRevision;
        Revisions[owner.Seat] = owner.SkillGrants.GrantRevision;
        return changed;
    }
}
