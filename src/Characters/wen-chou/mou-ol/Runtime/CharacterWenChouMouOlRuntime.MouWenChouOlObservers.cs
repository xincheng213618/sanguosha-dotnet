#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenChouMouOlRuntime
{
    internal bool IsMouWenChouMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => f.MouWenChouOl is { Stage: MouWenChouOlStage.Children or MouWenChouOlStage.DiscardChildren } r && effect?.Op == r.Operation && movement == f.PendingMovementContinuation && movement.SubjectSeat == f.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null && ValidMouWenChouOl(f);
    internal bool MouWenChouLion(ProgramSkillFrame f, int seat, CardMoveReason? reason) => f.MouWenChouOl is { Stage: MouWenChouOlStage.DiscardChildren } r && r.TargetSeat == seat && reason?.Value == MouWenChouDiscardReason && _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouDiscardPaidEvent>().Any(e => e.FrameId == f.Id && e.OwnerSeat == seat && e.Before == r.Before && e.After == r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == e.CardId && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(seat) && m.Sequence > e.Before && m.Sequence <= e.After && m.Reason == reason));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33100)]
    internal bool IsMouWenChouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => ValidMouWenChouOl(f) && f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && MouWenChouLion(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    internal bool MouWenChouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouWenChouOl is not { } r || !ValidMouWenChouOl(f))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouWenChouOlStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return IsMouWenChouRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && MouWenChouLion(f, hp.Change.TargetSeat, new(MouWenChouDiscardReason));
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && IsMouWenChouMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
        {
            if (IsMouWenChouNativeSettlementBatch(f, moved.Batch))
                return moved.ResumeProgramFrameId == f.Id;
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }

        if (r.Stage == MouWenChouOlStage.NativeUse && child is CardUseFrame native)
            return native.Id == r.UseId && IsMouWenChouDuel(native.Id);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouWenChouNativeEdge(int n)
    {
        if (_resolutionStackOperations._resolutionStack[n - 1] is CardUseFrame use && IsMouWenChouDuel(use.Id) && _resolutionStackOperations._resolutionStack[n] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { MouWenChouLunzhan: not null } native && IsMouWenChouDuel(native.Id) && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return false;
    }

    internal bool MouWenChouObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || MouWenChouNativeEdge(n) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouWenChouFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouWenChouObserverSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => this.MouWenChouObserverEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouWenChouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MouWenChouFirstChild(f, c), (n) => this.MouWenChouObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33100)]
    internal bool IsMouWenChouDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouWenChouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33100)]
    internal bool AdvanceMouWenChouChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouWenChouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33100)]
    internal bool ContinuePaidMouWenChou(ProgramSkillFrame f) => f.MouWenChouOl is { } r && ValidMouWenChouOl(f) && (r.Operation != SkillProgramEffectOp.UseMouWenChouLunzhan || r.Stage != MouWenChouOlStage.Targets) || f.MouWenChouOl is null && f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && CanRunIssuedMouWenChou(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), w);
}
