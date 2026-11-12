namespace CardGame.Core;

// Boundary delivery changes future declarations only. Issued frame rules are
// independently owned by their original native use or counterspell node.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class AppliedResponseRelationStore(IProgramTurnQueries turn) : IAppliedResponseRelationBoundaries, IAppliedResponseRelationStore
{
    private readonly List<AppliedResponseRelation> _relations = [];
    private long _sequence;
    public IReadOnlyList<AppliedResponseRelation> Relations => _relations;
    public AppliedResponseRelation Add(AppliedResponseRelation relation)
    {
        var issued = relation with { Sequence = ++_sequence };
        _relations.Add(issued);
        return issued;
    }
    public void ProcessBoundary(SkillStateBoundary boundary) => _relations.RemoveAll(relation =>
        relation.Spec.RemainingExpiry == AppliedResponseRemainingExpiry.IssuedTurnNumberChanges && relation.IssuedTurnNumber != turn.TurnNumber ||
        relation.Lifetime?.Until.Any(end => ProgramRuleLifetimeBoundaries.Matches(end, boundary)) == true);
}
