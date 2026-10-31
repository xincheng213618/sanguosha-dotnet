#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPanOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 150)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 140)]
    internal bool IsLiuPanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Pijing is { Stage: PijingStage.GiftChildren, Gift: { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } g } r && recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.TargetSeat == g.DonorSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == PijingGiftReason && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingGiftPaidEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index && e.CardId == g.CardId);
    internal bool LiuPanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Pijing is not { } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuPanRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage is PijingStage.GiftChildren or PijingStage.DrawChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is not CardsMovedTriggerWindowFrame moved || f.PendingMovementContinuation is null || !CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
            return false;
        if (r.Stage == PijingStage.GiftChildren)
            return r.Gift is { } gift && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.CardId == gift.CardId && m.From == gift.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == PijingGiftReason || gift.Kind == CardKind.WoodenOx && gift.From.Zone == CardZoneKind.Equipment && m.From == CardLocation.WoodenOxGrain(gift.DonorSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
        return r.Stage == PijingStage.DrawChildren && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason.Value == PijingDrawReason));
    }

    internal ProgramSkillFrame? LiuPanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LiuPanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27410)]
    internal bool AdvanceLiuPanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuPanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
