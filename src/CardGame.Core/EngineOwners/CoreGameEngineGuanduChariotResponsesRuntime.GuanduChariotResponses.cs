#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduChariotResponsesRuntime
{
    internal void CaptureGuanduChariotResponsePayments(CardActionContext action, long parentFrameId, ProgramCardContinuation continuation)
    {
        if (action.Type != CardActionType.Response || !global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime.IsResponseCompletionContinuation(continuation))
            return;
        foreach (var cost in action.PhysicalCards.Where(cost => cost.CardKind == CardKind.ThunderChariot && cost.From.Zone == CardZoneKind.Equipment))
        {
            if (cost.From != CardLocation.Equipment(action.ProviderSeat) || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == parentFrameId)is not { } parent || action.ParentActionId != (parent is NullificationWindowFrame counterspell ? _coreDelayedNullificationOperations.NullificationParentActionId(counterspell) : (parent as CardUseFrame)?.Action?.ActionId) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Count(fact => fact.Action.ActionId == action.ActionId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChariotResponsePaidEvent>().Any(fact => fact.ActionId == action.ActionId && fact.CardId == cost.CardId))
                throw new InvalidOperationException("A destroyed chariot response lost its exact accepted action and native parent.");
            var payments = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(batch => batch.ParentFrameId == parentFrameId && batch.ParentBatchId is null && batch.AwaitingProgramFrameId is null && batch.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber).SelectMany(batch => batch.Movements.Where(move => move.CardId == cost.CardId && move.CardKind == cost.CardKind && move.From == cost.From && move.To == CardLocation.OutsideGame && move.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime.IsResponseCompletionEntryReason(action, continuation, move.Reason)).Select(move => (Batch: batch, Move: move))).ToArray();
            if (payments is not [var payment] || !IsExactGuanduChariotDestruction(cost, payment.Move))
                throw new InvalidOperationException("A destroyed chariot response needs one actual equipped payment and its exact native batch.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduChariotResponsePaidEvent(action.ActionId, parentFrameId, cost.CardId, payment.Move.Sequence, payment.Batch.Id, continuation));
        }
    }

    internal bool IsExactGuanduChariotDestruction(CardActionCost cost, CardMovementRecord move) => cost.CardKind == CardKind.ThunderChariot && cost.From is { Zone: CardZoneKind.Equipment, OwnerSeat: { } owner } && move.CardId == cost.CardId && move.CardKind == cost.CardKind && move.From == cost.From && move.To == CardLocation.OutsideGame && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) && _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChariotDestroyedEvent>().Where(fact => fact.CardId == cost.CardId).ToArray()is [var destroyed] && destroyed.MovementSequence == move.Sequence && destroyed.FormerOwnerSeat == owner;
    internal CardMovementRecord? GuanduChariotResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation? continuation = null)
    {
        if (action.Type != CardActionType.Response || !action.PhysicalCards.Contains(cost) || cost.From != CardLocation.Equipment(action.ProviderSeat) || cost.CardKind != CardKind.ThunderChariot || _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChariotResponsePaidEvent>().Where(fact => fact.ActionId == action.ActionId && fact.ParentFrameId == parentFrameId && fact.CardId == cost.CardId).ToArray()is not [var paid] || (continuation is { } expected && paid.Continuation != expected) || !global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime.IsResponseCompletionContinuation(paid.Continuation) || paid.BatchId <= 0 || paid.BatchId > _coreResolutionIdentityBoundaryOperations._resolutionSequence)
            return null;
        var move = _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(move => move.Sequence == paid.MovementSequence);
        return move is not null && move.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && IsExactGuanduChariotDestruction(cost, move) && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime.IsResponseCompletionEntryReason(action, paid.Continuation, move.Reason) ? move : null;
    }

    internal bool HasGuanduChariotResponsePayment(CardActionContext action, ProgramCardContinuation continuation) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChariotResponsePaidEvent>().Where(fact => fact.ActionId == action.ActionId && fact.Continuation == continuation).Any(fact => action.PhysicalCards.Any(cost => cost.CardId == fact.CardId && GuanduChariotResponsePayment(action, fact.ParentFrameId, cost, continuation)is not null));
}
