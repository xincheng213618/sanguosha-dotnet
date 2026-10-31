#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeYanAnniversaryRuntime
{
    internal bool HeYanMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.AnniversaryHeYan is not { Stage: AnniversaryHeYanStage.MovementChildren or AnniversaryHeYanStage.DrawChildren } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        return true;
    }

    internal bool HeYanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryHeYan is not { } r)
            return false;
        if (child is PublicHandRevealWindowFrame reveal)
            return r.Stage == AnniversaryHeYanStage.Revealing && reveal.ParentFrameId == f.Id && reveal.ReturnKind == PublicHandRevealReturnKind.Registered && reveal.ReturnKey == HeYanRevealReturn && _characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, HeYanRevealReturn);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryHeYanStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.DrawTarget && draw.Context.RequestedCount == r.DrawAmount && CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (r.Stage is not (AnniversaryHeYanStage.MovementChildren or AnniversaryHeYanStage.DrawChildren))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            var before = r.Stage == AnniversaryHeYanStage.MovementChildren ? r.Payment!.Before : r.Draws[^1].SequenceBefore;
            var after = r.Stage == AnniversaryHeYanStage.MovementChildren ? r.Payment!.After : r.Draws[^1].SequenceAfter;
            return f.PendingMovementContinuation is { } p && HeYanMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }

        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal ProgramSkillFrame? HeYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.HeYanFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44200)]
    internal bool HeYanDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HeYanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44200)]
    internal bool HeYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryHeYan is null)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n && HeYanFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && (c == n + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(n)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44200)]
    internal bool AdvanceHeYanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HeYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
