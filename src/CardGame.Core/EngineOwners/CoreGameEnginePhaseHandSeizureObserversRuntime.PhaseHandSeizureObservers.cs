#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseHandSeizureObserversRuntime
{
    internal bool PhaseHandSeizureFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.PhaseHandSeizure is not null && !_corePhaseHandSeizureReceiptsOperations.ValidPhaseHandSeizureReceipt(root) || root.PhaseHandDebtReturn is not null && !_corePhaseHandSeizureReceiptsOperations.ValidPhaseHandDebtReturnReceipt(root))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame face)
            return root.PhaseHandSeizure is { Stage: PhaseHandSeizureStage.PaymentChildren or PhaseHandSeizureStage.FlipChildren } receipt && face.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && face.CharacterStateContinuation == CharacterStateContinuation.Program && face.ResumeProgramFrameId == root.Id && face.OwnerSeat == root.OwnerSeat && face.Window is SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp && (receipt.Stage != PhaseHandSeizureStage.FlipChildren || face.Window != SkillProgramTriggerWindow.CharacterTurnedOver || face.Facts.OwnerIsFaceDown == !receipt.WasFaceDown);
        if (root.PendingMovementContinuation is not { } pending || !_corePhaseHandSeizureReceiptsOperations.IsPhaseHandSeizureMovement(root, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(root), pending))
            return false;
        long before, after;
        string reason;
        IReadOnlyList<int> ids;
        if (root.PhaseHandSeizure is { Stage: PhaseHandSeizureStage.PaymentChildren } cost)
        {
            before = cost.CostBefore;
            after = cost.CostAfter;
            reason = global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandSeizureReason(root, "payment");
            ids = [cost.CostCardId];
        }
        else if (root.PhaseHandSeizure is { Stage: PhaseHandSeizureStage.TakeChildren } take && take.TargetSeat != root.OwnerSeat)
        {
            before = take.TakeBefore;
            after = take.TakeAfter;
            reason = global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandSeizureReason(root, "take");
            ids = take.TakenCardIds;
        }
        else if (root.PhaseHandDebtReturn is { Stage: PhaseHandDebtReturnStage.MovementChildren } gift)
        {
            before = gift.SequenceBefore;
            after = gift.SequenceAfter;
            reason = global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandDebtReturnReason(root);
            ids = Array.AsReadOnly(gift.CardIds.Where((id, index) => gift.Locations[index] != CardLocation.Hand(gift.TargetSeat)).ToArray());
            if (ids.Count == 0)
                return false;
        }
        else
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > before && m.Sequence <= after && ids.Contains(m.CardId) && m.Reason.Value == reason || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(root.OwnerSeat) && m.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(cost => cost.Sequence > before && cost.Sequence <= after && ids.Contains(cost.CardId) && cost.CardKind == CardKind.WoodenOx && cost.From == CardLocation.Equipment(root.OwnerSeat) && cost.Reason.Value == reason)));
        var lion = _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && ids.Contains(m.CardId) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(root.OwnerSeat) && m.Reason.Value == reason);
        if (!lion)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame replacement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replacement, root) && replacement.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && replacement.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && replacement.Attempt.Completion.MoveReason?.Value == reason && replacement.Attempt.SourceSeat == root.OwnerSeat && replacement.Attempt.TargetSeat == root.OwnerSeat && replacement.Attempt.Amount == 1;
    }

    internal ProgramSkillFrame? PhaseHandSeizureObserverRoot()
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !PhaseHandSeizureFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]))
                continue;
            var exact = true;
            for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
                if (!_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]))
                {
                    exact = false;
                    break;
                }

            if (exact)
                return root;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1900)]
    internal bool HasPhaseHandSeizureDamageObserver(long windowId) => PhaseHandSeizureObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == windowId && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
