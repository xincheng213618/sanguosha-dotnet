#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiYiOlRuntime
{
    internal static bool ChanshuangChildren(ChanshuangStage s) => RuntimeSkills.RuntimeRules_li_yi_ol_LiYiObservers.ChanshuangChildren(s);

    internal bool IsLiYiPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Chanshuang!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        var actor = ChanshuangItem(f).Seat;
        if (r.Stage == ChanshuangStage.DrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(actor) && m.Reason == CardMoveReasons.RecastDraw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ChanshuangMaterialPaidEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To))
            return true;
        return r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(actor)) && r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(actor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 180)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 170)]
    internal bool IsLiYiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Chanshuang is { Stage: ChanshuangStage.CostChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == ChanshuangItem(f).Seat && recovery.Attempt.TargetSeat == ChanshuangItem(f).Seat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(recovery.Attempt.TargetSeat)) && recovery.Attempt.Completion.MoveReason == (ChanshuangItem(f).Option == ChanshuangOption.Recast ? CardMoveReasons.RecastDiscard : new CardMoveReason(ChanshuangDiscardReason));
    internal bool LiYiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Chanshuang is not { } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is ProgramSkillFrame { HuanShuJun: not null } huanpei)
            return _characterHuanShuJunOlOperations.HuanpeiDraftRidesOn(huanpei, f);
        if (child is CardUseFrame use)
            return r.Stage == ChanshuangStage.NativeUse && use.ChanshuangSlash == r.Native && IsIssuedChanshuangUse(use);
        if (child is DrawFundedDistinctBasicFrame funded)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && funded.Payment.ParentFrameId == f.Id && funded.Payment.RequestFrameId == f.Id && funded.Payment.Cursor == f.InstructionIndex && funded.Payment.Source.OwnerSeat == actor && funded.Payment.TargetSeat is { } t && targets.Contains(t) && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(funded.Payment);
        if (child is ChainedStateBasicFrame chained)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && chained.Payment.ParentFrameId == f.Id && chained.Payment.RequestFrameId == f.Id && chained.Payment.Cursor == f.InstructionIndex && chained.Payment.Source.OwnerSeat == actor && chained.Payment.TargetSeat is { } t && targets.Contains(t) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chained.Payment);
        if (child is ProgramSkillFrame { Weijie: { } wj } draft)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && wj.Need.OwnerFrameId == f.Id && wj.Need.RequestFrameId == f.Id && wj.Need.ProviderSeat == actor && wj.Need.Targets.SequenceEqual(targets) && _characterGuoTuOlOperations.WeijieParent(draft);
        if (!ChanshuangChildren(r.Stage))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsLiYiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return r.Stage == ChanshuangStage.NativeChildren ? moved.ResumeProgramFrameId == f.Id && IsChanshuangNativeSettlementBatch(f, moved.Batch) : CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsLiYiPaidMovement(f, m));
    }

    internal ProgramSkillFrame? LiYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LiYiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27480)]
    internal bool AdvanceLiYiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
