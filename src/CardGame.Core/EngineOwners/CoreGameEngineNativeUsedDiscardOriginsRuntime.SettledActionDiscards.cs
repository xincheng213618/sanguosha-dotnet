#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 600)]
    internal void CaptureSettledActionDiscards(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements)
    {
        if (_contentRegistryOperations._contentRegistry?.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeSettledActionDiscard) != true)
            return;

        foreach (var movement in movements)
        {
            if (movement.To != CardLocation.DiscardPile || movement.From != CardLocation.Processing || movement.Reason.Value is not
                ("card.use-finished" or "card.response-finished" or "card.respond.nullification-finished"))
                continue;

            if (SettledActionContext(movement.CardId) is { } action)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(
                    new SettledActionCardDiscardEvent(batch.Id, movement.Sequence, movement.CardId, action.ActorSeat, batch.TurnNumber));
        }
    }

    internal CardActionContext? SettledActionContext(int cardId)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Select(frame => frame.Action)
            .LastOrDefault(action => action is { Type: CardActionType.Use } && action.PhysicalCards.Any(cost => cost.CardId == cardId));
        if (use is not null)
            return use;

        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Action is not null)?.Action;
        if (parent is null)
            return null;

        return _coreProgramEventHistoryOperations.Value.ProgramHistory.Complete().OfType<CardActionAcceptedEvent>()
            .Select(accepted => accepted.Action).LastOrDefault(action => action.Type == CardActionType.Response &&
                action.ParentActionId == parent.ActionId && action.PhysicalCards.Any(cost => cost.CardId == cardId));
    }
}
