#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal bool IsKongShuPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.KongShu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage is KongShuStage.BenefitDrawChildren or KongShuStage.RoundDrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == (r.Stage == KongShuStage.BenefitDrawChildren ? LeiluanDrawReason : FuchaoDrawReason) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        var discard = r.Stage == KongShuStage.RoundDiscardChildren;
        if (r.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == (discard ? _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat)) && m.Reason.Value == (discard ? FuchaoDiscardReason : LeiluanGainReason) && _coreProgramEventHistoryOperations.ProgramEventHistory<KongShuMaterialMovedEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.CardId == m.CardId && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return discard && r.Grain.Contains(m.CardId) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(r.Payer) && m.From == CardLocation.WoodenOxGrain(r.Payer));
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 240)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 230)]
    internal bool IsKongShuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.KongShu is { Paid: true, Stage: KongShuStage.RoundDiscardChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == FuchaoDiscardReason && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(r.Payer));
    internal bool KongShuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.KongShu is not { Paid: true } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsKongShuRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == KongShuStage.RoundDiscardChildren && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && hp.Change.Amount == 1 && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(r.Payer));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window) && (r.Stage != KongShuStage.ChainChildren || state.OwnerSeat == r.TurnOwner);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsKongShuPaidMovement(f, m));
    }

    internal ProgramSkillFrame? KongShuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.KongShuFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27550)]
    internal bool AdvanceKongShuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KongShuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
