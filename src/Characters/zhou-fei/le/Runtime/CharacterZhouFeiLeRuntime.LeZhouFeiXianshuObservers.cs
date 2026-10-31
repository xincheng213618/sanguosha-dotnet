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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28320)]
    internal void AssertLeZhouFeiXianshu(ProgramSkillFrame frame)
    {
        if (frame.LeZhouFeiXianshu is not { } receipt)
            return;
        if (receipt.Source != LeZhouFeiXianshuSource(frame) || receipt.GameplayHash != frame.GameplayHash || frame.SkillId != "le:xianshu" || frame.TriggerId is not null || frame.WindowContext is not null || frame.ActivationId != "marked-hand-gift" || frame.InstructionIndex != receipt.InstructionIndex || frame.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ResolveXianshuGiftDrawAndHealth || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(receipt.Phase) || receipt.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || receipt.Phase.ActorSeat != frame.OwnerSeat || receipt.Phase.TurnNumber != receipt.ActualTurn || receipt.Phase.ActualTurnOwnerSeat != receipt.TurnOwner || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.TargetSeat) || receipt.TargetSeat == frame.OwnerSeat || !Enum.IsDefined(receipt.Stage) || !Enum.IsDefined(receipt.GiftSuit) || receipt.Before < receipt.SequenceBefore || receipt.After < receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !frame.SelectedCardIds.SequenceEqual([receipt.CardId]) || !frame.SelectedTargetSeats.SequenceEqual([receipt.TargetSeat]) || receipt.GiftSequence > 0 && !HasPaidLeZhouFeiXianshuSelection(frame) || receipt.GiftSequence == 0 && (receipt.Stage != LeZhouFeiXianshuStage.GiftChildren || receipt.GiftBatchId != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuGiftPaidEvent>().Any(e => e.FrameId == frame.Id)))
            throw new InvalidOperationException("Xianshu lost its exact source, selected original Hand entity, native own Play or once-paid gift invoice.");
        if (receipt.RequestedDraw >= 0)
        {
            var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuDrawRequestedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
            if (!HasPaidLeZhouFeiXianshuSelection(frame) || receipt.RequestedDraw > 5 || facts is not [var draw] || draw.RequestedCount != receipt.RequestedDraw || draw.RequestedCount != (draw.OwnerHasHp && draw.TargetHasHp ? (int)Math.Min(5L, Math.Abs((long)draw.OwnerHp - draw.TargetHp)) : 0))
                throw new InvalidOperationException("Xianshu must sample one real capped HP difference after its gift children.");
        }
        else if (receipt.Stage != LeZhouFeiXianshuStage.GiftChildren || receipt.DrawOutcome is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuDrawRequestedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Xianshu cannot advance before its one native draw request is issued.");
        if (receipt.DrawOutcome is { } outcome)
        {
            if (receipt.Stage is LeZhouFeiXianshuStage.GiftChildren or LeZhouFeiXianshuStage.Drawing || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != receipt.RequestedDraw || outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || outcome.ReplacementGainedIds is not System.Collections.IList { IsReadOnly: true } || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.ActualDrawIds.Intersect(outcome.ReplacementGainedIds).Any() || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || outcome.ActualDrawIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == LeZhouFeiXianshuDrawReason) != 1) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == LeZhouFeiXianshuDrawReason) != outcome.ActualDrawIds.Count || outcome.SequenceBefore < receipt.GiftSequence || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuDrawReturnedEvent>().Count(e => e.FrameId == frame.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuDrawReturnedEvent>().Count(e => e.FrameId == frame.Id && e.RecipientSeat == outcome.RecipientSeat && e.RequestedCount == outcome.RequestedCount && e.ActualCount == outcome.ActualDrawIds.Count && e.ReplacementCount == outcome.ReplacementGainedIds.Count && e.Before == outcome.SequenceBefore && e.After == outcome.SequenceAfter && e.AttemptFrameId == outcome.AttemptFrameId) != 1)
                throw new InvalidOperationException("Xianshu's typed draw return changed its original request or actual DrawPile/replacement materials.");
        }
        else if (receipt.Stage is not (LeZhouFeiXianshuStage.GiftChildren or LeZhouFeiXianshuStage.Drawing))
            throw new InvalidOperationException("Xianshu cannot compare HP before its own draw returns.");
        if (receipt.HealthAction is { } action)
        {
            var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuHealthEvaluatedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
            if (receipt.DrawOutcome is null || receipt.Stage is not (LeZhouFeiXianshuStage.HealthIssuing or LeZhouFeiXianshuStage.HealthChildren) || facts is not [var health] || health.Action != action || health.GiftSuit != receipt.GiftSuit || health.Action != (!health.TargetAlive || !health.OwnerHasHp || !health.TargetHasHp ? XianshuHealthAction.None : global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(health.GiftSuit) && health.TargetHp <= health.OwnerHp ? XianshuHealthAction.Recover : (health.GiftSuit is Suit.Spade or Suit.Club) && health.TargetHp >= health.OwnerHp ? XianshuHealthAction.LoseHp : XianshuHealthAction.None))
                throw new InvalidOperationException("Xianshu lost its once-only colour and freshly evaluated post-draw HP comparison.");
            if (action == XianshuHealthAction.LoseHp && (receipt.Stage == LeZhouFeiXianshuStage.HealthChildren) != receipt.HpLossCaptured || action != XianshuHealthAction.LoseHp && (receipt.HpLossCaptured || receipt.AppearancePrevented))
                throw new InvalidOperationException("Xianshu cannot resume after an unperformed HP-loss instruction.");
            if (receipt.HpLossCaptured && (action != XianshuHealthAction.LoseHp || receipt.Stage != LeZhouFeiXianshuStage.HealthChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuHpLossAppliedEvent>().Count(e => e.FrameId == frame.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianshuHpLossAppliedEvent>().Count(e => e.FrameId == frame.Id && e.TargetSeat == receipt.TargetSeat && e.PreventedByAppearance == receipt.AppearancePrevented && e.Amount == (receipt.AppearancePrevented ? 0 : 1)) != 1))
                throw new InvalidOperationException("Xianshu repeated or lost its native HP deduction/identity-prevention return.");
            if (receipt.HpLossCaptured && !receipt.AppearancePrevented && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.TargetSeat == receipt.TargetSeat && e.Amount == 1) != 1)
                throw new InvalidOperationException("Xianshu lost its original native HP-loss result.");
        }
        else if (receipt.Stage is LeZhouFeiXianshuStage.HealthIssuing or LeZhouFeiXianshuStage.HealthChildren)
            throw new InvalidOperationException("Xianshu has no issued colour-health action for this stage.");
        if (frame.PendingMovementContinuation is { } pending && !IsLeZhouFeiXianshuMovement(frame, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame), pending))
            throw new InvalidOperationException("Xianshu's native child lost its exact typed parent return.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 210)]
    internal bool IsLeZhouFeiXianshuMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => frame.LeZhouFeiXianshu is { } receipt && effect?.Op == SkillProgramEffectOp.ResolveXianshuGiftDrawAndHealth && pending == frame.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && pending.SubjectSeat == (receipt.Stage is LeZhouFeiXianshuStage.GiftChildren or LeZhouFeiXianshuStage.DrawChildren ? frame.OwnerSeat : receipt.TargetSeat) && receipt.Stage != LeZhouFeiXianshuStage.Drawing;
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
        AssertLeZhouFeiXianshu(frame);
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
    internal ProgramSkillFrame? LeZhouFeiXianshuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => _characterZhouFeiLeOperations.LeZhouFeiXianshuFirstChild(frame, child), (index) => _coreObserverSubtreesOperations2.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _characterZhouFeiLeOperations.LeZhouFeiXianshuLifecycleEdge(next) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(next, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1140)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1250)]
    internal bool HasLeZhouFeiXianshuDamageObserver(long id) => LeZhouFeiXianshuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1280)]
    internal bool HasLeZhouFeiXianshuBeforeDamageObserver(long id) => LeZhouFeiXianshuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1110)]
    internal bool IsLeZhouFeiXianshuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && LeZhouFeiXianshuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28320)]
    internal bool AdvanceLeZhouFeiXianshuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LeZhouFeiXianshuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
