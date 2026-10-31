#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool IsIssuedDiciChainTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice }];
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28111, SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain)]
    internal bool CanActivateDici(ProgramActivationLegalityContext c) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && c.Owner.IsAlive && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == c.OwnerSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciAcceptedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Phase == phase) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0);
    internal bool DiciLeaseSpent(long id) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseStartedEvent>().Any(e => e.LeaseId == id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseCancelledEvent>().Any(e => e.LeaseId == id);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28112)]
    internal void ObserveDiciEntry(IGameEvent payload)
    {
        if (!HasZhaoZhongCapability || payload is not CharacterStateChangedEvent { Change.Window: SkillProgramTriggerWindow.CharacterEnteredChain } entered)
            return;
        foreach (var lease in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseIssuedEvent>().Where(e => e.TargetSeat == entered.Change.TargetSeat && !DiciLeaseSpent(e.LeaseId) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseDueEvent>().Any(d => d.LeaseId == e.LeaseId)).OrderBy(e => e.Ordinal).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciLeaseDueEvent(lease.LeaseId, entered.Change.Id, lease.TargetSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28115)]
    internal void CancelDiciDeadTarget(PlayerDiedEvent died)
    {
        if (!HasZhaoZhongCapability)
            return;
        foreach (var lease in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseIssuedEvent>().Where(e => e.TargetSeat == died.VictimSeat && !DiciLeaseSpent(e.LeaseId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciLeaseCancelledEvent(lease.LeaseId, lease.TargetSeat));
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectIssuedDiciChainCandidates(CharacterStateChangeContext change)
    {
        if (change.Window != SkillProgramTriggerWindow.CharacterEnteredChain || !_coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive)
            return[];
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseIssuedEvent>().Where(e => e.TargetSeat == change.TargetSeat && !DiciLeaseSpent(e.LeaseId) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseDueEvent>().Any(d => d.LeaseId == e.LeaseId && d.ChangeId == change.Id)).OrderBy(e => e.Ordinal).Select(e => new ProgramTriggerCandidate(e.IssuerSeat, e.Source.SkillId, "issued-next-chain", e.Source.SkillInstanceId, e.GameplayHash, 0, e.Ordinal)).ToArray());
    }

    internal DiciLeaseIssuedEvent? DiciCandidateLease(ProgramTriggerCandidate c) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseIssuedEvent>().SingleOrDefault(e => e.Ordinal == c.OccurrenceIndex && e.IssuerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash);
    internal ProgramSkillWindowContext CreateIssuedDiciChainContext(ProgramLifecycleTriggerWindowFrame w, ProgramTriggerCandidate c, ProgramSkillWindowContext ordinary)
    {
        var lease = DiciCandidateLease(c) ?? throw new InvalidOperationException("Dici lost its original issued chain lease.");
        return new(SkillProgramTriggerWindow.CharacterEnteredChain, w.Id, c.OwnerSeat, SourceSeat: lease.IssuerSeat, TargetSeat: lease.TargetSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]));
    }

    [ProgramIssuedTrigger(160)]
    internal bool IsIssuedDiciChainCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.BindingId == "issued-next-chain" && IsIssuedDiciChainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && DiciCandidateLease(c)is { } lease && !DiciLeaseSpent(lease.LeaseId) && _coreMatchParticipantsPlayersAccess._players[lease.TargetSeat].IsAlive && context.Window == SkillProgramTriggerWindow.CharacterEnteredChain && context.OwnerSeat == c.OwnerSeat && context.TargetSeat == lease.TargetSeat && context.SourceSeat == lease.IssuerSeat && context.OccurrenceIndex == lease.Ordinal && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.CharacterEnteredChain } w && w.OwnerSeat == lease.TargetSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] == c && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiciLeaseDueEvent>().Count(e => e.LeaseId == lease.LeaseId && e.ChangeId == w.Id && e.TargetSeat == lease.TargetSeat) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.Id == w.Id && e.Change.Window == SkillProgramTriggerWindow.CharacterEnteredChain && e.Change.TargetSeat == lease.TargetSeat) == 1;
}
