#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheBossCancelledMaterialsRuntime
{
    internal const string ShanheBossInterruptedProcessingReason = "campaign.shanhe.boss-interrupted-processing";
    internal void CaptureShanheBossCancelledMaterials()
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { StartingResourcesExpireOnUse: true, BossTransition: not null } || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheBossStageStartedEvent>().Count != 1)
            return;
        // Capture every paid native Use, including a virtual program child's
        // physical materials, before any frame is retired. No cost is refunded.
        foreach (var entry in _coreCardMovementJournalCardMovementsAccess._cardMovements.GroupBy(m => m.CardId).Select(g => g.Last()).Where(m => m.To == CardLocation.Processing).ToArray())
        {
            if (!_coreShanheStartingResourcesOperations.IsShanheStartingResource(entry.CardId) || _coreShanheStartingResourcesOperations.ShanheStartingResourceWasAcquired(entry.CardId))
                continue;
            bool Matches(CardActionContext action) => action.PhysicalCards.Any(c => c.CardId == entry.CardId && c.CardKind == entry.CardKind && c.From == entry.From);
            var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action is { Type: CardActionType.Use } a && Matches(a) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e => e.CardId == entry.CardId && e.EntrySequence == entry.Sequence && e.UseFrameId == f.Id && e.ActionId == a.ActionId));
            if (use?.Action is { } action)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheBossMaterialCancelledEvent(entry.CardId, use.Id, action.ActionId, entry.Sequence, CardActionType.Use));
                continue;
            }

            if (entry.Reason != CardMoveReasons.Respond && entry.Reason != CardMoveReasons.Nullification && entry.Reason.Value != "conversion.declaration.pay")
                continue;
            var response = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.Type == CardActionType.Response && Matches(a));
            ResolutionFrame? owner = response is null ? null : _resolutionStackOperations._resolutionStack.LastOrDefault(f => f switch
            {
                ProgramCardTriggerWindowFrame card => card.Action.ActionId == response.ActionId,
                CardUseFrame incoming => incoming.Action?.ActionId is { } id && response.ParentActionId == id,
                NullificationWindowFrame counter => response.ParentActionId == _coreDelayedNullificationOperations.Value.NullificationParentActionId(counter),
                ProgramSkillFrame program => _coreResponseCompletionOperations.IsResponseCompletionProgramDuelParent(program, ProgramCardContinuation.DuelSlash),
                _ => false
            });
            // A declared response must also retain its original claimed invoice.
            if (entry.Reason.Value == "conversion.declaration.pay" && !_resolutionStackOperations._resolutionStack.Any(f => f.AcceptedDeclarationPayment is { Claimed: true } paid && paid.OwnerFrameId == f.Id && paid.Cost.CardId == entry.CardId && paid.Cost.CardKind == entry.CardKind && paid.Cost.From == entry.From && response?.ConversionChain.Contains(paid.Source) == true))
                continue;
            if (owner is not null && response is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheBossMaterialCancelledEvent(entry.CardId, owner.Id, response.ActionId, entry.Sequence, CardActionType.Response));
        }
    }

    internal bool TryDescribeShanheBossCancelledMaterial(int cardId, CardMovementRecord entry, CardMoveReason reason, out ShanheBossMaterialCancelledEvent cancelled)
    {
        cancelled = null !;
        if (reason.Value != ShanheBossInterruptedProcessingReason || _coreMatchDefinitionOptionsAccess._options.ShanheBattle?.BossTransition is null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheBossStageStartedEvent>().Count != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheBossMaterialCancelledEvent>().Where(e => e.CardId == cardId && e.EntrySequence == entry.Sequence).ToArray()is not [var receipt] || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheBossResolutionCancelledEvent>().Any(e => e.FrameId == receipt.FrameId) || _resolutionStackOperations._resolutionStack.Any(f => f.Id == receipt.FrameId) || entry.CardId != cardId || entry.To != CardLocation.Processing)
            return false;
        if (receipt.ActionType == CardActionType.Use && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e => e.CardId == cardId && e.EntrySequence == entry.Sequence && e.UseFrameId == receipt.FrameId && e.ActionId == receipt.ActionId))
            return false;
        if (receipt.ActionType == CardActionType.Response && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == receipt.ActionId && e.Action.Type == CardActionType.Response && e.Action.PhysicalCards.Any(c => c.CardId == cardId && c.CardKind == entry.CardKind && c.From == entry.From)))
            return false;
        cancelled = receipt;
        return true;
    }
}
