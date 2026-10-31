#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27130)]
    internal void AssertWuJing(ProgramSkillFrame f)
    {
        if (f.WuJing is not { } r)
            return;
        if (f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || !WuJingComposition.Handles(r.Operation) || r.Source != WuJingSource(f) || r.GameplayHash != f.GameplayHash || r.WindowId != context.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Entities.Select(c => c.CardId).Distinct().Count() != r.Entities.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == r.Operation && e.WindowId == r.WindowId && e.ActualTurn == r.ActualTurn && e.UseId == r.UseId && e.ActionId == r.ActionId && e.TargetSeat == r.TargetSeat) != 1)
            throw new InvalidOperationException("Wu Jing lost his exact owning instruction, immutable entities or native start invoice.");
        var at = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (at < 1)
            throw new InvalidOperationException("Wu Jing lost his retained native trigger parent.");
        if (r.Operation == SkillProgramEffectOp.ClaimLiubingUndamagingSlash)
        {
            if (r.Stage != WuJingStage.ClaimChildren || context.Window != SkillProgramTriggerWindow.DiscardPileReceived || context.MovementBatch is not { } batch || _resolutionStackOperations._resolutionStack[at - 1] is not CardsMovedTriggerWindowFrame parent || parent.Id != r.WindowId || parent.Id != batch.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || r.Entities.Count == 0 || r.Entities.Any(entity => entity.From != CardLocation.DiscardPile || !_coreProgramEventHistoryOperations.ProgramEventHistory<WuJingLiubingSettledEvent>().Any(e => e.UseId == r.UseId && e.ActionId == r.ActionId && e.BatchId == batch.Id && e.FinalActor == r.TargetSeat && e.FinalActor != f.OwnerSeat && e.CardId == entity.CardId && e.MovementSequence == entity.Sequence)) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingClaimIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId && e.BatchId == batch.Id && e.Count == r.Entities.Count && e.Before == r.Before && e.After == r.After) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => m.From != CardLocation.DiscardPile || m.To != CardLocation.Hand(f.OwnerSeat) || m.Reason.Value != WuJingClaimReason || !r.Entities.Any(e => e.CardId == m.CardId)) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == WuJingClaimReason) != r.Entities.Count)
                throw new InvalidOperationException("Liubing lost its exact complete zero-damage Use, other actor's actual Play or once-claimed discard entities.");
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramCardTriggerWindowFrame parent || parent.Id != r.WindowId || parent.ParentFrameId != r.UseId || parent.Action.ActionId != r.ActionId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])))
                throw new InvalidOperationException("Heji lost the exact completed native Use and current trigger cursor.");
            if (r.Operation == SkillProgramEffectOp.RewardHejiUnconvertedUse)
            {
                if (r.Stage != WuJingStage.RewardChildren || r.NativeOrigin is not { RewardEligible: true } origin || origin.UseId != r.UseId || origin.ActionId != r.ActionId || origin.Source.OwnerSeat != f.OwnerSeat || origin.Source.SkillId != f.SkillId || origin.Source.SkillInstanceId != f.SkillInstanceId || origin.GameplayHash != f.GameplayHash || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { WuJingHeji: { Rewarded: true } issued } || issued.Origin != origin || _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingRewardIssuedEvent>().Count(e => e.FrameId == f.Id && e.ProgramFrameId == origin.ProgramFrameId && e.UseId == r.UseId && e.CardId == r.RewardCardId && e.Before == r.Before && e.After == r.After) != 1 || (r.RewardCardId is { } card ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray()is not [var moved] || moved.CardId != card || moved.From != CardLocation.DrawPile || moved.To != CardLocation.Hand(f.OwnerSeat) || moved.Reason.Value != WuJingRewardReason || _coreAdvancedLifecycleOperations.GetAdvancedCard(card).Suit is not (Suit.Heart or Suit.Diamond) : r.Before != r.After))
                    throw new InvalidOperationException("Heji's reward lost its one genuinely unconverted native Use and seeded real red deck entity.");
            }
            else if (r.Stage is WuJingStage.NativeUse or WuJingStage.NativeChildren)
            {
                if (r.NativeOrigin is not { } origin || origin.ProgramFrameId != f.Id || origin.Source != r.Source || origin.GameplayHash != r.GameplayHash || origin.IncomingUseId != r.UseId || origin.IncomingActionId != r.ActionId || origin.ActualTurn != r.ActualTurn || origin.ActorSeat != f.OwnerSeat || origin.TargetSeat != r.TargetSeat || r.Selection is not { } selected || selected.Kind != origin.Kind || !selected.CardIds.SequenceEqual(origin.Materials.Select(m => m.CardId)) || origin.Materials.Count == 0 && !WuJingZeroPaymentProof(f, r) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == origin.UseId && e.Before == r.Before && e.After == r.After) != 1 || (r.Stage == WuJingStage.NativeUse ? r.NativeCompleted && _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null : !r.NativeCompleted || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == origin.UseId) != 1))
                    throw new InvalidOperationException("Heji lost its exact once-paid native physical attack and complete typed return.");
            }
            else if (r.Stage is not (WuJingStage.UseChoice or WuJingStage.IssuingUse or WuJingStage.FactionRequest))
                throw new InvalidOperationException("Heji entered another instruction's paid stage.");
        }

        var nativePrompt = _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } currentPrompt ? _characterMaXiuMaTieOlOperations.KenshangProgramNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime.WeijieNativeDecision(currentPrompt)))) : null;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage == WuJingStage.UseChoice && (nativePrompt is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, WuJingChoices(f))))
            throw new InvalidOperationException("Heji lost its real private chosen-card boundary.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 600)]
    internal bool IsWuJingMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.WuJing is { Stage: WuJingStage.NativeChildren or WuJingStage.RewardChildren or WuJingStage.ClaimChildren } r && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && (e is null || e.Op == r.Operation);
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

    internal ProgramSkillFrame? WuJingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterWuJingOlOperations.WuJingFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1460)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1570)]
    internal bool HasWuJingDamageObserver(long id) => WuJingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1590)]
    internal bool HasWuJingBeforeDamageObserver(long id) => HasWuJingDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1420)]
    internal bool IsWuJingDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && WuJingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27130)]
    internal bool AdvanceWuJingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuJingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 300)]
    internal bool WindowSettlement_IsWuJingNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsWuJingNativeSettlementBatch(frame, batch);
}
