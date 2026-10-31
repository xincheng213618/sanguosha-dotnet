#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiFengOlRuntime
{
    internal bool CanContinueIssuedLiFeng(ProgramSkillFrame f)
    {
        if (f.LiFeng is { Paid: true })
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId == "completed-drawn-entity" && f.WindowContext is { } c && IsIssuedJiyunCompletedUseCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27591)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27591)]
    internal bool PermitIssuedLiFeng(ProgramSkillFrame f) => CanContinueIssuedLiFeng(f);
    internal bool IsLiFengPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.LiFeng is not { } r || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        return r.Stage switch
        {
            LiFengStage.InitialChildren => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == JiyunInitialReason && m.From == CardLocation.DrawPile && m.To == r.Pile.Location && r.Entities.Any(e => e.CardId == m.CardId && e.Kind == m.CardKind),
            LiFengStage.PreparationChildren => r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (r.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == JiyunDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle),
            LiFengStage.CollectChildren => m.Sequence > r.Before && m.Sequence <= r.After && (m.CardId == r.ActiveCardId && m.From == r.ActiveFrom && m.To == r.Pile.Location && m.Reason.Value == JiyunCollectReason || IsLiFengCollectedEquipment(f, CardKind.WoodenOx) && m.From == CardLocation.WoodenOxGrain(r.ActiveFrom!.Value.OwnerSeat!.Value) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard),
            LiFengStage.DistributionChildren => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.ActiveCardId && m.From == r.ActiveFrom && m.To == CardLocation.Hand(r.ActiveRecipient) && m.Reason.Value == ShuliangGiftReason,
            _ => false
        };
    }

    internal bool IsLiFengCollectedEquipment(ProgramSkillFrame f, CardKind kind) => f.LiFeng is { Paid: true, Stage: LiFengStage.CollectChildren, ActiveFrom: { Zone: CardZoneKind.Equipment } } r && r.CompletedEntities.Any(e => e.CardId == r.ActiveCardId && e.Kind == kind);
    internal bool LiFengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LiFeng is not { Paid: true } r)
            return false;
        if (r.Stage == LiFengStage.PreparationDraw && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsLiFengRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return IsLiFengCollectedEquipment(f, CardKind.SilverLion) && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.ActiveFrom!.Value.OwnerSeat && hp.Change.TargetSeat == r.ActiveFrom.Value.OwnerSeat && hp.Change.Amount == 1;
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsLiFengPaidMovement(f, m));
    }

    internal bool LiFengObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? LiFengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.LiFengFirstChild(f, child), (index) => this.LiFengObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 290)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 280)]
    internal bool IsLiFengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.LiFeng is not { Paid: true } r)
            return false;
        if (recovery.ParentFrameId == f.Id)
            return IsLiFengCollectedEquipment(f, CardKind.SilverLion) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.ActiveFrom!.Value.OwnerSeat && recovery.Attempt.TargetSeat == r.ActiveFrom.Value.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.CardId == r.ActiveCardId && recovery.Attempt.Completion.MoveReason?.Value == JiyunCollectReason;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && j > i + 1 && LiFengFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && LiFengObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27591)]
    internal bool AdvanceLiFengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiFengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
