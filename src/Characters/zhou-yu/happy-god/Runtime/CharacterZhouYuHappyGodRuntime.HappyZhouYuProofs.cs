#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuHappyGodRuntime
{
    internal bool HappyZyMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.HappyZhouYu is not { Stage: HappyZyStage.Children, Mode: "draw", Draw: not null } || e?.Op != SkillProgramEffectOp.ResolveHappyZhouYuQinyin || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        return true;
    }

    internal bool HappyZyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HappyZhouYu is not { } r)
            return false;
        if (child is IdentityAppearanceFrame appearance)
            return r.Mode == "lose" && r.Stage is HappyZyStage.HpLoss or HappyZyStage.Children && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == r.Target && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex;
        if (r.Stage != HappyZyStage.Children)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Mode == "draw" && r.Draw is { } draw && f.PendingMovementContinuation is { } p && HappyZyMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return r.Mode == "heal" && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Target && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Mode is "heal" or "lose" && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == r.Target && hp.Continuation is PostEventContinuation.Program or PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == (r.Mode == "lose" ? HpChangeKind.Loss : HpChangeKind.Recovery);
        if (child is DyingFrame dying)
            return r.Mode == "lose" && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Target && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == r.Target && e.Amount == 1 && e.RemainingHp <= 0);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? HappyZyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HappyZyFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 41440)]
    internal bool HappyZyDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && HappyZyObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 41440)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 41440)]
    internal bool HappyZyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.HappyZhouYu is null)
            return false;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return root >= 0 && child > root && HappyZyFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && (child == root + 1 || Enumerable.Range(root + 2, child - root - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i))) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 41440)]
    internal bool AdvanceHappyZyChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappyZyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
