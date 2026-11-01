namespace CardGame.Core;

internal interface IConditionalSkillGrantQueries : IProgramSelectionQuery
{
    ProgramSkillFrame CaptureSource(ProgramSkillFrame frame);
    bool CanContinue(ProgramSkillFrame frame);
}

internal interface IConditionalSkillGrantOperations : IProgramRuntimeOperations
{
    void Synchronize();
}

/// <summary>Materializes declared dependent grants; availability owns their read-only qualification.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ConditionalSkillGrantRuntime(IMatchParticipantStateAccess participants,
    IProgramContentQueries content, RuntimeEntries.CoreGameEngineNationalSkillsEntries skills,
    RuntimeEntries.CoreGameEngineHealthQueriesEntries health, IProgramRuntimeEvents events,
    ISkillGrantRevisionObservations observations,
    RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFrozenHandRosterRuntime> roster)
    : IConditionalSkillGrantQueries, IConditionalSkillGrantOperations
{
    private readonly Dictionary<(int OwnerSeat, ConditionalSkillGrantSource Source), string> _instances = [];
    private bool _synchronizing;

    public ProgramSkillFrame CaptureSource(ProgramSkillFrame frame)
    {
        var receipt = skills.GetSkillBindingShard(participants.Players[frame.OwnerSeat]).ActiveGrants
            .Where(grant => grant.SkillId == frame.SkillId && grant.SkillInstanceId == frame.SkillInstanceId && grant.ConditionalSource is not null)
            .Select(grant => new ConditionalSkillGrantProgramReceipt(grant.ConditionalSource!,
                content.Skill(grant.ConditionalSource!.CapabilitySkillId).Program!.SkillGrants
                    .Single(rule => rule.Id == grant.ConditionalSource.RuleId).OnSourceLoss))
            .OrderBy(receipt => receipt.OnSourceLoss == ConditionalSkillGrantContinuation.FinishInvocation ? 0 :
                receipt.OnSourceLoss == ConditionalSkillGrantContinuation.FinishPaidInstruction ? 1 : 2)
            .FirstOrDefault();
        return receipt is null ? frame : frame with { ConditionalSourceReceipt = receipt };
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 500)]
    public bool CanContinue(ProgramSkillFrame frame) => frame.ConditionalSourceReceipt?.OnSourceLoss switch
    {
        ConditionalSkillGrantContinuation.FinishInvocation => true,
        ConditionalSkillGrantContinuation.FinishPaidInstruction =>
            frame.FrozenHandRoster is { Stage: "movement" } or { Stage: "damage-issued", DamageIssued: true } &&
            roster.Value.ValidFrozenHandRoster(frame),
        _ => false
    };

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 50)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 30)]
    public void Synchronize()
    {
        if (_synchronizing) return;
        if (_instances.Count == 0 && !participants.Players.Any(owner =>
                owner.SkillGrants.Grants.Any(grant => content.Skill(grant.SkillId).Program?.SkillGrants.Count > 0))) return;
        _synchronizing = true;
        try
        {
            foreach (var key in _instances.Keys.Where(key => !participants.Players[key.OwnerSeat].SkillGrants.Grants.Any(grant =>
                         grant.GrantId == key.Source.CapabilityGrantId && grant.SkillId == key.Source.CapabilitySkillId &&
                         grant.SkillInstanceId == key.Source.CapabilitySkillInstanceId)).ToArray())
                _instances.Remove(key);

            foreach (var owner in participants.Players)
            {
                var desired = new Dictionary<string, SkillGrant>(StringComparer.Ordinal);
                foreach (var parent in owner.SkillGrants.Grants.Where(grant =>
                             content.Skill(grant.SkillId).Program?.SkillGrants.Count > 0 &&
                             skills.IsEffectiveSkillSource(owner, grant)).ToArray())
                foreach (var rule in content.Skill(parent.SkillId).Program!.SkillGrants.Where(rule => skills.ConditionalSkillRuleApplies(owner, rule)))
                {
                    var source = new ConditionalSkillGrantSource(parent.GrantId, parent.SkillId, parent.SkillInstanceId, rule.Id);
                    var id = GrantId(owner.Seat, source);
                    if (!_instances.TryGetValue((owner.Seat, source), out var instance))
                    {
                        instance = skills.GetSkillBindingShard(owner).ActiveGrants.Where(grant => grant.SkillId == rule.SkillId)
                            .OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId ??
                            desired.Values.FirstOrDefault(grant => grant.SkillId == rule.SkillId)?.SkillInstanceId ?? id;
                        _instances.Add((owner.Seat, source), instance);
                    }
                    desired.Add(id, new SkillGrant(id, rule.SkillId, instance, "conditional:" + parent.SourceId, ConditionalSource: source));
                }

                var stale = owner.SkillGrants.Grants.Where(grant => grant.ConditionalSource is not null && !desired.ContainsKey(grant.GrantId)).ToArray();
                var added = desired.Values.Where(grant => !owner.SkillGrants.Grants.Any(existing => existing.GrantId == grant.GrantId)).ToArray();
                if (stale.Length == 0 && added.Length == 0) continue;
                observations.Capture(participants.Players);
                foreach (var grant in stale)
                {
                    owner.SkillGrants.RemoveGrant(grant.GrantId);
                    events.AdvanceFact(new ConditionalSkillGrantChangedEvent(owner.Seat, grant.GrantId, grant.SkillId,
                        grant.SkillInstanceId, grant.ConditionalSource!, false, health.UnambiguousHp(owner)));
                }
                foreach (var grant in added)
                {
                    owner.SkillGrants.Grant(grant);
                    events.AdvanceFact(new ConditionalSkillGrantChangedEvent(owner.Seat, grant.GrantId, grant.SkillId,
                        grant.SkillInstanceId, grant.ConditionalSource!, true, health.UnambiguousHp(owner)));
                }
            }
        }
        finally { _synchronizing = false; }
    }

    private static string GrantId(int owner, ConditionalSkillGrantSource source) =>
        $"conditional:{owner}:{source.CapabilityGrantId.Length}:{source.CapabilityGrantId}:{source.CapabilitySkillInstanceId.Length}:{source.CapabilitySkillInstanceId}:{source.RuleId}";
}
