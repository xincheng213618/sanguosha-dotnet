#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengOlRuntime
{

    internal bool IsLuYuShengIssuedMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.LuYuSheng!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        return r.Stage switch
        {
            LuYuShengStage.DrawChildren => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == LuYuShengDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && r.Reshuffles.Any(s => s.ParentFrameId == f.Id && s.Movements.Contains(m)),
            LuYuShengStage.BottomDiscardChildren => r.Selected.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.DiscardPile && m.Reason.Value == LuYuShengBottomDiscardReason,
            LuYuShengStage.RunweiDiscardChildren => m.CardId == r.CardId && m.From == r.From && m.To == (m.CardKind == CardKind.GeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) && m.Reason.Value == LuYuShengRunweiDiscardReason || r.WoodenOxGrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Payer) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(ox => ox.Sequence > r.Before && ox.Sequence <= r.After && ox.CardId == r.CardId && ox.CardKind == CardKind.WoodenOx && ox.From == CardLocation.Equipment(r.Payer) && ox.Reason.Value == LuYuShengRunweiDiscardReason),
            _ => false
        };
    }

    internal bool LuYuShengSilverLion(ProgramSkillFrame f) => f.LuYuSheng is { Stage: LuYuShengStage.RunweiDiscardChildren } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => IsLuYuShengIssuedMovement(f, m) && m.CardId == r.CardId && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Payer));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 730)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 670)]
    internal bool IsLuYuShengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.LuYuSheng is { } r && LuYuShengSilverLion(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == LuYuShengRunweiDiscardReason;
    internal bool LuYuShengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LuYuSheng is not { } r || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsLuYuShengIssuedMovement(f, m)) && (moved.Batch.Movements.All(m => m.Reason != CardMoveReasons.Reshuffle) || moved.Batch.ParentBatchId is null && r.Reshuffles.Any(s => s.BatchId == moved.Batch.Id && s.ParentFrameId == moved.Batch.ParentFrameId && s.Movements.SequenceEqual(moved.Batch.Movements)));
        if (child is RecoveryReplacementFrame recovery)
            return IsLuYuShengRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && LuYuShengSilverLion(f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? LuYuShengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LuYuShengFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26900)]
    internal bool AdvanceLuYuShengSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LuYuShengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
