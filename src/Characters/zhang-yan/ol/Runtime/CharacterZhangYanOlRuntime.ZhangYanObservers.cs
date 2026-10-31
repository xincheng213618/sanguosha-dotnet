#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYanOlRuntime
{
    internal bool IsZhangYanRewardMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ZhangYan!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        return m.CardId == r.CardId && m.From == r.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhangYanObtainReason || r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.PhaseOwner) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(ox => ox.Sequence > r.Before && ox.Sequence <= r.After && ox.CardId == r.CardId && ox.CardKind == CardKind.WoodenOx && ox.From == CardLocation.Equipment(r.PhaseOwner) && ox.To == CardLocation.Hand(f.OwnerSeat) && ox.Reason.Value == ZhangYanObtainReason);
    }

    internal bool IsZhangYanNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => f.ZhangYan is { Stage: ZhangYanStage.NativeChildren, NativeCompleted: true, NativeReturn: { } ret } r && batch.ParentFrameId == ret.UseId && batch.AwaitingProgramFrameId is null && batch.OriginOwnerSeat == f.OwnerSeat && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.Movements.Count > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanSujiReturnedEvent>().Count(e => e.Return == ret) == 1 && batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.After && m.CardId == ret.CardId && m.CardKind == ret.PrintedKind && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 770)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 710)]
    internal bool IsZhangYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.ZhangYan is { Stage: ZhangYanStage.RewardChildren } r && r.From == CardLocation.Equipment(r.PhaseOwner) && r.CardId is { } id && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => IsZhangYanRewardMovement(f, m) && m.CardId == id && m.CardKind == CardKind.SilverLion) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.PhaseOwner && recovery.Attempt.TargetSeat == r.PhaseOwner && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == ZhangYanObtainReason;
    internal bool ZhangYanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhangYan is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == ZhangYanStage.NativeUse && use.ZhangYanSuji == r.NativeReturn && IsZhangYanSujiUse(use);
        if (r.Stage is not (ZhangYanStage.NativeChildren or ZhangYanStage.RewardChildren) || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == ZhangYanStage.NativeChildren ? moved.ResumeProgramFrameId == f.Id && IsZhangYanNativeSettlementBatch(f, moved.Batch) : moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsZhangYanRewardMovement(f, m));
        if (child is RecoveryReplacementFrame recovery)
            return IsZhangYanRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == ZhangYanStage.RewardChildren && r.From == CardLocation.Equipment(r.PhaseOwner) && r.CardId is { } id && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.SourceSeat == r.PhaseOwner && hp.Change.TargetSeat == r.PhaseOwner && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? ZhangYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.ZhangYanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27000)]
    internal bool AdvanceZhangYanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 200)]
    internal bool WindowSettlement_IsZhangYanNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsZhangYanNativeSettlementBatch(frame, batch);
}
