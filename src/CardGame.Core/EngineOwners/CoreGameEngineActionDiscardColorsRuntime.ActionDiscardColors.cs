#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActionDiscardColorsRuntime
{
    internal bool TracksActionDiscardColor => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewardDiscardedActionColor) == true;

    internal bool? CapturePhysicalCardColor(int seat, Card card) => _coreCardDeclarationsOperations.LiveDeclarationPayment(card.Id)is { } declared ? declared.Cost.EffectiveIsRed : _coreTurnRedSlashBenefitsOperations.CapturesActionColor || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PayAndDrawWushuangJinzhi) ? CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card)) : null;
    internal bool? CaptureActionColor(IReadOnlyList<CardActionCost> costs, Suit? effectiveSuit = null)
    {
        if (!_coreTurnRedSlashBenefitsOperations.CapturesActionColor || costs.Count == 0)
            return null;
        if (effectiveSuit == Suit.None)
            return null;
        if (CoreGameEnginePublicPileColorsRuntime.SuitColor(effectiveSuit)is { } declared)
            return declared;
        var colors = costs.Select(c => c.EffectiveIsRed).Distinct().ToArray();
        return colors.Length == 1 ? colors[0] : null;
    }

    internal void CaptureActionDiscardFact(long batchId, int turn, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksActionDiscardColor)
            return;
        var actual = movements.Where(m => m.To == CardLocation.DiscardPile && m.From == CardLocation.Processing && m.Reason.Value is "card.use-finished" or "card.response-finished" or "card.respond.nullification-finished" or "card.effect.iron-chain-finished").ToArray();
        if (actual.Length == 0)
            return;
        var activeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action is not null);
        var parentAction = activeUse?.Action;
        var response = actual.All(m => m.Reason == CardMoveReasons.ResponseFinished || m.Reason == CardMoveReasons.NullificationFinished);
        CardActionContext? action;
        if (!response)
            action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Select(f => f.Action).LastOrDefault(a => a is not null && a.Type == CardActionType.Use && actual.All(m => a.PhysicalCards.Any(c => c.CardId == m.CardId)));
        else
        {
            // Response identity must belong to this active card-use parent. Reused ids in unrelated turns/parents cannot match.
            action = parentAction is null ? null : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.Type == CardActionType.Response && a.ParentActionId == parentAction.ActionId && a.PhysicalCards.Count > 0 && actual.All(m => a.PhysicalCards.Any(c => c.CardId == m.CardId)));
        }

        if (action is null || action.PhysicalCards.Count == 0)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActionCardsDiscardedEvent(batchId, action.ActionId, action.ProviderSeat, action.EffectiveIsRed, turn));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 500)]
    internal void CaptureMovementBatchFactsCaptureActionDiscardFact(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureActionDiscardFact(batch.Id, batch.TurnNumber, movements);
}
