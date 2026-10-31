#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengGongYingOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27100)]
    internal void AssertChengGongYing(ProgramSkillFrame f)
    {
        if (f.ChengGongYing is not { } r)
            return;
        if (f.TriggerId is null || f.InstructionIndex != 1 || r.Source != ChengGongYingSource(f) || r.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.AdjustKuangxiangHandToFour || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } c || c.ParentFrameId != r.WindowId || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || !ChengGongYingParent(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c) || _coreProgramEventHistoryOperations.ProgramEventHistory<ChengGongYingStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.WindowId == r.WindowId && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner) != 1 || r.Count < 0 || r.InitialHandCount < 0 || r.Selected.Count > r.Count || r.Selected.Distinct().Count() != r.Selected.Count || r.Candidates.Distinct().Count() != r.Candidates.Count || r.Selected.Any(id => !r.Candidates.Contains(id)) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualCount < 0)
            throw new InvalidOperationException("Kuangxiang lost its exact current native candidate, owning instruction, frozen participants or physical selection.");
        if (r.Stage == ChengGongYingStage.TargetChoice)
        {
            if (r.Recipient is not null || r.InitialHandCount != 0 || r.Count != 0 || r.Candidates.Count != 0 || r.Selected.Count != 0 || r.Before != 0 || r.After != 0 || r.ActualCount != 0)
                throw new InvalidOperationException("An unpaid Kuangxiang target selection cannot own an adjustment invoice.");
        }
        else if (r.Recipient is not { } payer || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(payer) || _coreProgramEventHistoryOperations.ProgramEventHistory<KuangxiangTargetChosenEvent>().Count(e => e.FrameId == f.Id && e.Recipient == payer && e.InitialHandCount == r.InitialHandCount && e.RequestedCount == Math.Abs(r.InitialHandCount - 4) && e.Draw == (r.InitialHandCount < 4)) != 1 || (r.Stage == ChengGongYingStage.DrawChildren ? r.InitialHandCount >= 4 || r.Count != 4 - r.InitialHandCount || r.Candidates.Count != 0 || r.Selected.Count != 0 : r.InitialHandCount < 4 || r.Count > r.InitialHandCount - 4 || r.Count > r.Candidates.Count))
            throw new InvalidOperationException("Kuangxiang lost its original hand-count adjustment and exact target.");
        if (f.PendingMovementContinuation is not null)
        {
            var draw = r.Stage == ChengGongYingStage.DrawChildren;
            if (r.Stage is not (ChengGongYingStage.DrawChildren or ChengGongYingStage.DiscardChildren) || r.Recipient is not { } recipient || _coreProgramEventHistoryOperations.ProgramEventHistory<KuangxiangAdjustmentIssuedEvent>().Count(e => e.FrameId == f.Id && e.Recipient == recipient && e.Draw == draw && e.Before == r.Before && e.After == r.After && e.ActualCount == r.ActualCount) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && (!draw || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason.Value == KuangxiangDrawReason)) != r.ActualCount || r.Reshuffles.Select(s => s.BatchId).Distinct().Count() != r.Reshuffles.Count || r.Reshuffles.Any(s => s.ParentFrameId != f.Id || s.Movements.Count == 0 || s.Before < r.Before || s.After > r.After || s.Before != s.Movements[0].Sequence - 1 || s.After != s.Movements[^1].Sequence || s.Movements.Any(m => m.From != CardLocation.DiscardPile || m.To != CardLocation.DrawPile || m.Reason != CardMoveReasons.Reshuffle || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) || _coreProgramEventHistoryOperations.ProgramEventHistory<KuangxiangReshuffleCapturedEvent>().Count(e => e.FrameId == f.Id && e.BatchId == s.BatchId && e.Before == s.Before && e.After == s.After && e.Count == s.Movements.Count) != 1) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => !IsChengGongYingIssuedMovement(f, m)))
                throw new InvalidOperationException("Kuangxiang lost its once-issued real native movement interval and exact reshuffle proof.");
            if (draw && _coreProgramEventHistoryOperations.ProgramEventHistory<KuangxiangCardObtainedEvent>().Where(e => e.FrameId == f.Id).Any(e => e.Source != r.Source || e.Recipient != recipient || e.MovementSequence <= r.Before || e.MovementSequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason.Value == KuangxiangDrawReason)))
                throw new InvalidOperationException("Kuangxiang cannot mark a card not really obtained by its exact draw.");
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage is ChengGongYingStage.TargetChoice or ChengGongYingStage.DiscardChoice)
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != ChengGongYingChooser(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, ChengGongYingChoices(f)))
                throw new InvalidOperationException("Kuangxiang lost its exact source/recipient-private selection.");
    }

    internal bool IsChengGongYingIssuedMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ChengGongYing!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || r.Recipient is not { } payer)
            return false;
        return r.Stage switch
        {
            ChengGongYingStage.DrawChildren => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(payer) && m.Reason.Value == KuangxiangDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && r.Reshuffles.Any(s => s.ParentFrameId == f.Id && s.Movements.Contains(m)),
            ChengGongYingStage.DiscardChildren => r.Selected.Contains(m.CardId) && m.From == CardLocation.Hand(payer) && m.To == CardLocation.DiscardPile && m.Reason.Value == KuangxiangDiscardReason,
            _ => false
        };
    }

    internal bool ChengGongYingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ChengGongYing is not { } r || f.PendingMovementContinuation is null)
            return false;
        AssertChengGongYing(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsChengGongYingIssuedMovement(f, m)) && (moved.Batch.Movements.All(m => m.Reason != CardMoveReasons.Reshuffle) || moved.Batch.ParentBatchId is null && r.Reshuffles.Any(s => s.BatchId == moved.Batch.Id && s.ParentFrameId == moved.Batch.ParentFrameId && s.Movements.SequenceEqual(moved.Batch.Movements)));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? ChengGongYingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterChengGongYingOlOperations.ChengGongYingFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 660)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 770)]
    internal bool HasChengGongYingDamageObserver(long id) => ChengGongYingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 800)]
    internal bool HasChengGongYingBeforeDamageObserver(long id) => HasChengGongYingDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 620)]
    internal bool IsChengGongYingDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ChengGongYingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27100)]
    internal bool AdvanceChengGongYingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ChengGongYingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
