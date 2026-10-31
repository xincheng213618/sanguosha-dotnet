#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionObserversRuntime
{
    internal bool DamageAcquisitionFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.DamageZoneAcquisition is not { Stage: DamageZoneAcquisitionStage.MovementChildren } r)
            return false;
        _coreDamageZoneAcquisitionOperations.Value.RequireDamageAcquisitionReceipt(root);
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (root.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == "skill-program.damage-zone-acquisition.obtain" && r.Cards.Contains(m.CardId));
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (!r.Cards.Select((id, i) => (id, from: r.From[i])).Any(p => p.from == CardLocation.Equipment(r.VictimSeat) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == p.id && m.CardKind == CardKind.SilverLion && m.From == p.from)))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.VictimSeat && hp.Change.TargetSeat == r.VictimSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.VictimSeat && recovery.Attempt.TargetSeat == r.VictimSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == "skill-program.damage-zone-acquisition.obtain";
    }

    internal ProgramSkillFrame? DamageAcquisitionObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.DamageAcquisitionFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 480)]
    internal bool TryAdvanceDamageAcquisitionSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DamageAcquisitionObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 10)]
    internal bool IsDamageAcquisitionDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && DamageAcquisitionObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
