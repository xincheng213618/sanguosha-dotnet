namespace CardGame.Core;

// History, including the pending tail, remains owned by the command session.
// This runtime derives the directed grants anew and retains no policy map.
[ProgramRuntimeModule]
internal sealed class ZhuitaoDistanceRuntime(IDistanceRuleRuntimeQueries queries, IProgramEventJournalQueries journal)
{
    internal IReadOnlyList<ProgramDirectedDistanceGrantedEvent> ActiveGrants(int ownerSeat, int targetSeat)
    {
        var active = new List<ProgramDirectedDistanceGrantedEvent>();
        foreach (var fact in journal.Complete())
        {
            switch (fact)
            {
                case ProgramDirectedDistanceGrantedEvent grant when grant.OwnerSeat == ownerSeat && grant.TargetSeat == targetSeat:
                    active.Add(grant);
                    break;
                case ProgramDirectedDistanceRevokedEvent revoked when revoked.OwnerSeat == ownerSeat && revoked.TargetSeat == targetSeat:
                    active.Clear();
                    break;
            }
        }
        return active;
    }

    internal RuleQueryEvaluation ApplyDistance(int sourceSeat, int targetSeat, RuleQueryEvaluation original)
    {
        if (sourceSeat == targetSeat ||
            !queries.HasTriggerOperation(SkillProgramEffectOp.ZhuitaoMarkTarget))
            return original;
        var grants = ActiveGrants(sourceSeat, targetSeat);
        if (grants.Count == 0) return original;
        var value = original.Value is UnlimitedRuleQueryValue ? int.MaxValue : ((FiniteRuleQueryValue)original.Value).Value;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue),
            [new RuleQueryBaseTerm("distance:before-program-directed", value)],
            grants.Select((grant, index) => (RuleQueryContribution)new FiniteRuleQueryContribution(
                $"program-directed:{grant.FrameId}:{index}", SkillRuleOperation.Add, grant.Amount)).ToArray());
    }
}
