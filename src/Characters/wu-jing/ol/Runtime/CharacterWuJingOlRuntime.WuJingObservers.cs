#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal bool IsWuJingNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => f.WuJing is { Stage: WuJingStage.NativeChildren, NativeCompleted: true, NativeOrigin: { } origin } r && batch.ParentFrameId == origin.UseId && batch.AwaitingProgramFrameId is null && batch.Movements.Count > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == origin.UseId) == 1 && batch.Movements.All(m => m.Sequence > r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished && origin.Materials.Any(e => e.CardId == m.CardId && e.PrintedKind == m.CardKind));
    internal bool WuJingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuJing is not { } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is DrawFundedDistinctBasicFrame draw)
            return IsWuJingHejiRequest(f) && draw.Payment.ParentFrameId == f.Id && draw.Payment.RequestFrameId == f.Id && draw.Payment.Cursor == f.InstructionIndex && draw.Payment.Source.OwnerSeat == f.OwnerSeat && draw.Payment.TargetSeat == r.TargetSeat && draw.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && draw.OriginalDecision.PlayerSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<DrawFundedDistinctBasicStartedEvent>().Any(e => e.PaymentFrameId == draw.Id && e.ParentFrameId == f.Id && e.RequestFrameId == f.Id && e.OriginalPromptId == draw.OriginalDecision.PromptId);
        if (child is ChainedStateBasicFrame chain)
            return IsWuJingHejiRequest(f) && chain.Payment.ParentFrameId == f.Id && chain.Payment.RequestFrameId == f.Id && chain.Payment.Cursor == f.InstructionIndex && chain.Payment.Source.OwnerSeat == f.OwnerSeat && chain.Payment.TargetSeat == r.TargetSeat && chain.OriginalDecision.SkillPrompt?.SkillId == f.SkillId && chain.OriginalDecision.PlayerSeat == f.OwnerSeat && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chain.Payment);
        if (child is CardUseFrame use)
            return r.Stage == WuJingStage.NativeUse && use.WuJingHeji?.Origin == r.NativeOrigin && IsWuJingHejiUse(use);
        if (f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == WuJingStage.NativeChildren ? moved.ResumeProgramFrameId == f.Id && IsWuJingNativeSettlementBatch(f, moved.Batch) : r.Stage is WuJingStage.RewardChildren or WuJingStage.ClaimChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(f.OwnerSeat) && (r.Stage == WuJingStage.RewardChildren ? m.From == CardLocation.DrawPile && m.CardId == r.RewardCardId && m.Reason.Value == WuJingRewardReason : m.From == CardLocation.DiscardPile && r.Entities.Any(e => e.CardId == m.CardId) && m.Reason.Value == WuJingClaimReason));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? WuJingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.WuJingFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27130)]
    internal bool AdvanceWuJingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuJingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 300)]
    internal bool WindowSettlement_IsWuJingNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsWuJingNativeSettlementBatch(frame, batch);
}
