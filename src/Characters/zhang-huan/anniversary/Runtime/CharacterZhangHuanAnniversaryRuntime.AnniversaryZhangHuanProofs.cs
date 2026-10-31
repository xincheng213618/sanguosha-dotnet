#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHuanAnniversaryRuntime
{
    internal bool ZhangHuanParent(ProgramSkillFrame f)
    {
        if (f.SkillId != AnniversaryZhangHuanComposition.Shuguo || f.TriggerId != "actual-turn-discard-uses" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } w || w.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is DeferredTurnEndFrame parent && parent.Id == w.ParentFrameId && parent.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && parent.OwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.AfterTurnEnded?.CurrentChild?.FrameId == f.Id && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, w);
    }

    internal bool ValidZhangHuan(ProgramSkillFrame f) => f.AnniversaryZhangHuan is { } r && f.InstructionIndex == 1 && ZhangHuanParent(f) && r.Source == ZhangHuanSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && r.ParentId == f.WindowContext?.ParentFrameId && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.Used.All(id => id > 0) && r.Used.Distinct().Count() == r.Used.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHuanStartedEvent>().Count(e => e == new AnniversaryZhangHuanStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId)) == 1;
    internal bool ZhangHuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.AnniversaryZhangHuan is not { } r || effect?.Op != SkillProgramEffectOp.ResolveAnniversaryZhangHuanShuguo || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || !ValidZhangHuan(f))
            return false;
        if (r.Stage == AnniversaryZhangHuanStage.DrawChildren)
            return r.Draw is { } d && d.RecipientSeat == f.OwnerSeat && d.RequestedCount == r.RequestedDraw && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHuanDrawReturnedEvent>().Count(e => e == new AnniversaryZhangHuanDrawReturnedEvent(f.Id, d.RequestedCount, d.ActualDrawIds.Count, d.SequenceBefore, d.SequenceAfter)) == 1;
        return r.Stage == AnniversaryZhangHuanStage.NativeTail && r.Returned && _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => r.NativeBatches.Contains(b.Id)).All(b => IsAnniversaryZhangHuanSettlementBatch(f, b));
    }

    internal bool ZhangHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidZhangHuan(f) || f.AnniversaryZhangHuan is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryZhangHuanStage.NativeUse && IsZhangHuanNative(use);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryZhangHuanStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.RequestedDraw && CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ZhangHuanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal ProgramSkillFrame? ZhangHuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.ZhangHuanFirstChild(f, c), (i) => this.ZhangHuanObserverSuffix(i));
    internal bool ZhangHuanObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, dying));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46300)]
    internal bool ZhangHuanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ZhangHuanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46300)]
    internal bool RetainZhangHuan(ProgramSkillFrame f) => f.AnniversaryZhangHuan is not null && ValidZhangHuan(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46300)]
    internal bool AdvanceZhangHuanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangHuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46300)]
    internal bool ZhangHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryZhangHuan is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && ZhangHuanFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || ZhangHuanObserverSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
}
