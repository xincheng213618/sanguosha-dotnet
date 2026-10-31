#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWuJingPaymentsRuntime
{
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.ProcessingAllowance, 20)]
    internal bool HasWuJingHejiTrickPaymentProcessing(IReadOnlyList<Card> processing)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^1] is not CardsMovedTriggerWindowFrame movement || _resolutionStackOperations._resolutionStack[^2] is not CardUseFrame { WuJingHeji: { }, RecoveryPaidContinuation: { } continuation, PaymentMovementReceipt: { } payment, Action: { } action } use || !_characterWuJingOlOperations.IsWuJingHejiUse(use) || !_characterWuJingOlOperations.HasWuJingHejiTrickPaymentTail(use, continuation) || !_coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, movement))
            return false;
        var allowed = payment.Batch.Movements.Where(move => move.To == CardLocation.Processing).Select(move => move.CardId).ToHashSet();
        var visited = new HashSet<long>();
        for (var parentId = action.ParentActionId; parentId is { } id;)
        {
            if (!visited.Add(id) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(parent => parent.Action?.ActionId == id)is not { Action: { } parentAction } parent || parent.Id >= use.Id)
                return false;
            allowed.UnionWith(parent.PhysicalCardIds ?? [parent.CardId]);
            parentId = parentAction.ParentActionId;
        }

        return processing.All(card => allowed.Contains(card.Id));
    }
}
