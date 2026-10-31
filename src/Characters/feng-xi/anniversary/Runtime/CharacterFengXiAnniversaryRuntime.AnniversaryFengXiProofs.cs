#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFengXiAnniversaryRuntime
{
    internal bool FengXiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.AnniversaryFengXi is not { Stage: AnniversaryFengXiStage.MovementChildren or AnniversaryFengXiStage.DrawChildren } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        return true;
    }

    internal bool FengXiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryFengXi is not { } r)
            return false;
        if (child is HappyQiaoHealthChoiceFrame qiao)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(qiao, f);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryFengXiStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == r.Target && draw.Context.RequestedCount == r.Amount && CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is IdentityAppearanceFrame a)
            return r.Stage is AnniversaryFengXiStage.HealthApplying or AnniversaryFengXiStage.HealthChildren && a.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && a.Receipt.ParentFrameId == f.Id && a.Receipt.OwnerSeat == r.Target && a.Receipt.OriginalInstructionIndex == f.InstructionIndex && a.Receipt.RequestedDeduction == r.Amount;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && FengXiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == (f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == AnniversaryFengXiStage.HealthChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == r.Target && hp.Change.Kind == HpChangeKind.Loss && hp.Continuation == PostEventContinuation.Program;
        if (child is DyingFrame dying)
            return r.Stage == AnniversaryFengXiStage.HealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Target && dying.Continuation == DyingContinuationKind.ProgramSkill && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.TargetSeat == r.Target && e.Amount == r.Amount && e.RemainingHp <= 0);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal ProgramSkillFrame? FengXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.FengXiFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44500)]
    internal bool FengXiDyingObserver() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && FengXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44500)]
    internal bool FengXiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryFengXi is null)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n && FengXiFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && (c == n + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(n)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44500)]
    internal bool AdvanceFengXiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FengXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
