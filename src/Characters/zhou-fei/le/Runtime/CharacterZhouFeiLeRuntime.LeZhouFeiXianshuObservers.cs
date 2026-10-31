#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal bool HasPaidLeZhouFeiXianshuSelection(ProgramSkillFrame frame)
    {
        if (frame.LeZhouFeiXianshu is not { GiftSequence: > 0 } receipt || frame.SkillId != "le:xianshu" || frame.ActivationId != "marked-hand-gift" || frame.TriggerId is not null || frame.WindowContext is not null || receipt.Source != LeZhouFeiXianshuSource(frame) || receipt.GameplayHash != frame.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || frame.InstructionIndex != receipt.InstructionIndex || frame.InstructionIndex != 1 || !frame.SelectedCardIds.SequenceEqual([receipt.CardId]) || !frame.SelectedTargetSeats.SequenceEqual([receipt.TargetSeat]) || receipt.GiftSequence <= receipt.SequenceBefore || receipt.GiftSequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuGiftStartedEvent>().Count(e => e.FrameId == frame.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuGiftPaidEvent>().Count(e => e.FrameId == frame.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuGiftStartedEvent>().Count(e => e.FrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.Phase == receipt.Phase && e.ActualTurn == receipt.ActualTurn && e.TurnOwner == receipt.TurnOwner && e.CardId == receipt.CardId && e.PrintedKind == receipt.PrintedKind && e.GiftSuit == receipt.GiftSuit && e.TargetSeat == receipt.TargetSeat && e.SequenceBefore == receipt.SequenceBefore) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuGiftPaidEvent>().Count(e => e.FrameId == frame.Id && e.BatchId == receipt.GiftBatchId && e.CardId == receipt.CardId && e.From == CardLocation.Hand(frame.OwnerSeat) && e.To == CardLocation.Hand(receipt.TargetSeat) && e.MovementSequence == receipt.GiftSequence) != 1)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == receipt.GiftSequence && m.CardId == receipt.CardId && m.CardKind == receipt.PrintedKind && m.From == CardLocation.Hand(frame.OwnerSeat) && m.To == CardLocation.Hand(receipt.TargetSeat) && m.Reason.Value == LeZhouFeiXianshuGiftReason) == 1;
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 370)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 360)]
    internal bool IsLeZhouFeiXianshuRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => frame.LeZhouFeiXianshu is { Stage: LeZhouFeiXianshuStage.HealthChildren, HealthAction: XianshuHealthAction.Recover } receipt && HasPaidLeZhouFeiXianshuSelection(frame) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == frame.OwnerSeat && recovery.Attempt.TargetSeat == receipt.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == frame.InstructionIndex;
    internal bool LeZhouFeiXianshuPaidMovement(ProgramSkillFrame frame, CardMovementRecord movement)
    {
        var receipt = frame.LeZhouFeiXianshu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement))
            return false;
        if (receipt.Stage == LeZhouFeiXianshuStage.GiftChildren)
            return movement.Sequence == receipt.GiftSequence && movement.CardId == receipt.CardId && movement.From == CardLocation.Hand(frame.OwnerSeat) && movement.To == CardLocation.Hand(receipt.TargetSeat) && movement.Reason.Value == LeZhouFeiXianshuGiftReason;
        return receipt.Stage == LeZhouFeiXianshuStage.DrawChildren && receipt.DrawOutcome is { } draw && movement.Sequence > draw.SequenceBefore && movement.Sequence <= draw.SequenceAfter && (draw.ActualDrawIds.Contains(movement.CardId) && movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(frame.OwnerSeat) && movement.Reason.Value == LeZhouFeiXianshuDrawReason || draw.ReplacementGainedIds.Contains(movement.CardId) && movement.To == CardLocation.Hand(frame.OwnerSeat) || movement.From == CardLocation.DiscardPile && movement.To == CardLocation.DrawPile && movement.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool LeZhouFeiXianshuFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.LeZhouFeiXianshu is not { } receipt || !HasPaidLeZhouFeiXianshuSelection(frame))
            return false;
        if (receipt.Stage == LeZhouFeiXianshuStage.Drawing && frame.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, frame);
        if (child is IdentityAppearanceFrame appearance)
            return receipt.HealthAction == XianshuHealthAction.LoseHp && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == frame.Id && appearance.Receipt.OwnerSeat == receipt.TargetSeat && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == frame.InstructionIndex;
        if (child is DyingFrame dying)
            return receipt.Stage == LeZhouFeiXianshuStage.HealthChildren && receipt.HealthAction == XianshuHealthAction.LoseHp && receipt.HpLossCaptured && dying.ParentFrameId == frame.Id && dying.VictimSeat == receipt.TargetSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is RecoveryReplacementFrame recovery)
            return IsLeZhouFeiXianshuRecoveryParent(frame, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == frame.Id && hp.ResumeFrameId == frame.Id && receipt.Stage == LeZhouFeiXianshuStage.HealthChildren && hp.Change.TargetSeat == receipt.TargetSeat && (receipt.HealthAction == XianshuHealthAction.LoseHp && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.Amount == 1 || receipt.HealthAction == XianshuHealthAction.Recover && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == frame.OwnerSeat && hp.Change.Amount == 1);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == frame.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && frame.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.ParentFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(movement => LeZhouFeiXianshuPaidMovement(frame, movement));
    }

    internal bool LeZhouFeiXianshuLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame frame && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]) && frame.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal ProgramSkillFrame? LeZhouFeiXianshuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.LeZhouFeiXianshuFirstChild(frame, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || this.LeZhouFeiXianshuLifecycleEdge(next) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28320)]
    internal bool AdvanceLeZhouFeiXianshuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LeZhouFeiXianshuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
