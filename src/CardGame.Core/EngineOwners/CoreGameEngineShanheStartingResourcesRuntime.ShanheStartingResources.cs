#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheStartingResourcesRuntime
{
    internal void PublishShanheStartingResourceOrigins()
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { StartingResourcesExpireOnUse: true } setup || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceCreatedEvent>().Count != 0)
            return;
        // Noninteractive initial dealing runs in the constructor, before the
        // event journal is open. Bind those exact generated initial-deal moves
        // when Start opens it, rather than keeping an uncommitted side ledger.
        var entries = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.From == CardLocation.OutsideGame && (m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.InitialDeal || m.To == CardLocation.Equipment(0) && m.Reason == CardMoveReasons.EquipmentEnter) && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(m.CardId)).OrderBy(m => m.Sequence).ToArray();
        var recipe = setup.StartingCards.Concat(setup.StartingEquipment).ToArray();
        for (var index = 0; index < entries.Length; index++)
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(entries[index].CardId);
            var expected = recipe[index];
            if (card.Kind != expected.Kind || card.Suit != expected.Suit || card.Rank != expected.Rank)
                throw new InvalidOperationException("The starting resource initial-deal order no longer matches its original recipe.");
            var equipment = index >= setup.StartingCards.Count;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheStartingResourceCreatedEvent(card.Id, equipment, equipment ? index - setup.StartingCards.Count : index));
        }
    }

    internal bool IsShanheStartingResource(int id)
    {
        if (_coreShanheMarketApplicationOperations.IsShanheMarketResource(id))
            return true;
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { StartingResourcesExpireOnUse: true } setup || !_coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceCreatedEvent>().Where(e => e.CardId == id).ToArray()is not [var origin])
            return false;
        var recipe = origin.EquipmentResource ? setup.StartingEquipment : setup.StartingCards;
        if (origin.RecipeIndex < 0 || origin.RecipeIndex >= recipe.Count)
            return false;
        var item = recipe[origin.RecipeIndex];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return card.Kind == item.Kind && card.Suit == item.Suit && card.Rank == item.Rank;
    }

    internal bool ShanheStartingResourceWasAcquired(int id, int before = int.MaxValue) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == id && m.Sequence < before && m.To.Zone == CardZoneKind.Hand && m.From != m.To && m.Reason != CardMoveReasons.InitialDeal && m.Reason != CardMoveReasons.Draw && m.Reason != CardMoveReasons.RecastDraw && m.Reason != CoreGameEngineShanheMarketDefinitions.ShanheMarketCardGain);
    internal void CaptureShanheClaimedDeclarationUse(DeclaredCardPayment paid)
    {
        if (!IsShanheStartingResource(paid.Cost.CardId) || ShanheStartingResourceWasAcquired(paid.Cost.CardId) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Action: { Type: CardActionType.Use } action } use || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.OwnerFrameId)?.AcceptedDeclarationPayment is not { Claimed: true } invoice || invoice != paid with
        {
            Claimed = true
        }

        || !action.PhysicalCards.Contains(paid.Cost) || !action.ConversionChain.Contains(paid.Source) || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == paid.Cost.CardId)is not { To.Zone: CardZoneKind.Processing } entry || entry.From != paid.Cost.From || entry.Reason.Value != "conversion.declaration.pay")
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheStartingResourceUseEnteredEvent(entry.CardId, use.Id, action.ActionId, entry.Sequence));
    }

    internal void CaptureShanheStartingResourceUseEntries(CardMovementBatchContext batch)
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { StartingResourcesExpireOnUse: true } || batch.ParentFrameId is not { } parent || _coreCardUseLifecycleOperations.LifecycleCardUse(parent)is not { Action: { Type: CardActionType.Use } action } use)
            return;
        foreach (var move in batch.Movements)
        {
            if (!IsShanheStartingResource(move.CardId) || ShanheStartingResourceWasAcquired(move.CardId, move.Sequence) || move.To != CardLocation.Processing || move.Reason != CardMoveReasons.Use && move.Reason != CardMoveReasons.IronChainUse && move.Reason != CardMoveReasons.EquipmentUse || !action.PhysicalCards.Any(c => c.CardId == move.CardId && c.CardKind == move.CardKind && c.From == move.From))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheStartingResourceUseEnteredEvent(move.CardId, use.Id, action.ActionId, move.Sequence));
        }
    }

    internal bool TryDescribeShanheResourceExit(int cardId, CardLocation from, CardMoveReason reason, int before, out int entrySequence, out long parentFrame, out long actionId)
    {
        entrySequence = 0;
        parentFrame = 0;
        actionId = 0;
        if (!IsShanheStartingResource(cardId) || ShanheStartingResourceWasAcquired(cardId, before))
            return false;
        var used = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().LastOrDefault(e => e.CardId == cardId && e.EntrySequence < before);
        if (from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && used is not null)
        {
            entrySequence = used.EntrySequence;
            parentFrame = used.UseFrameId;
            actionId = used.ActionId;
            return true;
        }

        if (from != CardLocation.Processing || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.Sequence < before)is not { To.Zone: CardZoneKind.Processing } entry)
            return false;
        if (_coreShanheBossCancelledMaterialsOperations.TryDescribeShanheBossCancelledMaterial(cardId, entry, reason, out var cancelled))
        {
            entrySequence = entry.Sequence;
            parentFrame = cancelled.FrameId;
            actionId = cancelled.ActionId;
            return true;
        }

        if (used is not null && used.EntrySequence == entry.Sequence && (CoreGameEngineNativeUsedDiscardOriginsRuntime.IsNativeUseFinishReason(reason) || reason.Value == "area.abolished.equipment") && _coreCardUseLifecycleOperations.LifecycleCardUse(used.UseFrameId)is { Action: { } action } && action.ActionId == used.ActionId && action.PhysicalCards.Any(c => c.CardId == cardId && c.From == entry.From && c.CardKind == entry.CardKind))
        {
            entrySequence = entry.Sequence;
            parentFrame = used.UseFrameId;
            actionId = used.ActionId;
            return true;
        }

        if (used is not null && entry.From.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && (reason == CardMoveReasons.DismantlementFinished || reason == CardMoveReasons.DismantlementJudgmentFinished))
        {
            entrySequence = used.EntrySequence;
            parentFrame = used.UseFrameId;
            actionId = used.ActionId;
            return true;
        }

        if (reason != CardMoveReasons.ResponseFinished && reason != CardMoveReasons.NullificationFinished)
            return false;
        var current = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var response = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.Type == CardActionType.Response && a.PhysicalCards.Any(c => c.CardId == cardId && c.From == entry.From && c.CardKind == entry.CardKind) && (current is CardUseFrame incoming && a.ParentActionId == incoming.Action?.ActionId || current is ProgramSkillFrame program && _coreResponseCompletionOperations.IsResponseCompletionProgramDuelParent(program, ProgramCardContinuation.DuelSlash) || current is NullificationWindowFrame counter && a.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(counter)));
        var claimedDeclaration = entry.Reason.Value == "conversion.declaration.pay" && current?.AcceptedDeclarationPayment is { Claimed: true } paid && paid.OwnerFrameId == current.Id && paid.Cost.CardId == cardId && paid.Cost.CardKind == entry.CardKind && paid.Cost.From == entry.From;
        if (response is not null && (entry.Reason == CardMoveReasons.Respond || entry.Reason == CardMoveReasons.Nullification || claimedDeclaration && current!.AcceptedDeclarationPayment is { } responsePaid && response.ConversionChain.Contains(responsePaid.Source)))
        {
            entrySequence = entry.Sequence;
            parentFrame = current!.Id;
            actionId = response.ActionId;
            return true;
        }

        // The single native counterspell records its accepted response after its
        // physical finish. Its exact current cursor owns both movements already.
        if (current is NullificationWindowFrame n && (entry.Reason == CardMoveReasons.Nullification || claimedDeclaration && n.AcceptedDeclarationPayment!.DeclaredKind == CardKind.Nullification) && reason == CardMoveReasons.NullificationFinished && entry.From.OwnerSeat is { } provider && n.CandidateIndex >= 0 && n.CandidateIndex < n.CandidateSeats.Count && n.CandidateSeats[n.CandidateIndex] == provider && _coreDelayedNullificationOperations.HasNullificationEffectOwner(n))
        {
            entrySequence = entry.Sequence;
            parentFrame = n.Id;
            return true;
        }

        return false;
    }

    internal CardLocation NormalizeShanheStartingResourceDestination(Card card, CardLocation from, CardLocation to, CardMoveReason? reason)
    {
        if (!IsShanheStartingResource(card.Id))
            return to;
        if (from == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Any(e => e.CardId == card.Id))
            return from;
        if (to != CardLocation.DiscardPile)
            return to;
        // Pure proof callers omit a reason only when forecasting a continuing
        // field departure. Processing exits always require the actual finish.
        return (reason is { } actual && TryDescribeShanheResourceExit(card.Id, from, actual, int.MaxValue, out _, out _, out _) || reason is null && from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && !ShanheStartingResourceWasAcquired(card.Id) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e => e.CardId == card.Id)) ? CardLocation.OutsideGame : to;
    }

    internal void RecordShanheStartingResourceDestruction(CardMovementRecord move)
    {
        if (move.To != CardLocation.OutsideGame || !IsShanheStartingResource(move.CardId))
            return;
        if (!TryDescribeShanheResourceExit(move.CardId, move.From, move.Reason, move.Sequence, out var entry, out var parent, out var action))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Any(e => e.CardId == move.CardId))
            throw new InvalidOperationException("A starting resource exit needs its exact native use/response or continuing field provenance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheStartingResourceDestroyedEvent(move.CardId, move.Sequence, entry, parent, action));
    }

    internal bool IsShanheStartingResourceDestroyedMaterial(CardMovementRecord move) => move.To == CardLocation.OutsideGame && IsShanheStartingResource(move.CardId) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Where(e => e.CardId == move.CardId).ToArray()is [var fact] && fact.MovementSequence == move.Sequence && !ShanheStartingResourceWasAcquired(move.CardId, move.Sequence) && _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == fact.EntrySequence)is { } entry && entry.CardId == move.CardId && entry.To == CardLocation.Processing && (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e => e.CardId == move.CardId && e.EntrySequence == entry.Sequence && e.UseFrameId == fact.ParentFrameId && e.ActionId == fact.ActionId) || move.From == CardLocation.Processing && entry.Sequence < move.Sequence && (entry.Reason == CardMoveReasons.Respond || entry.Reason == CardMoveReasons.Nullification || entry.Reason.Value == "conversion.declaration.pay") && (move.Reason == CardMoveReasons.ResponseFinished || move.Reason == CardMoveReasons.NullificationFinished) || _coreShanheBossCancelledMaterialsOperations.TryDescribeShanheBossCancelledMaterial(move.CardId, entry, move.Reason, out var cancelled) && cancelled.FrameId == fact.ParentFrameId && cancelled.ActionId == fact.ActionId);
    internal bool IsShanheStartingResourceResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => action.Type == CardActionType.Response && action.PhysicalCards.Contains(cost) && finish.CardId == cost.CardId && finish.CardKind == cost.CardKind && IsShanheStartingResourceDestroyedMaterial(finish) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Single(e => e.CardId == cost.CardId)is { } fact && (fact.ActionId == 0 || fact.ActionId == action.ActionId) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence == fact.EntrySequence)is { } entry && entry.From == cost.From && entry.Sequence < finish.Sequence;
    internal bool IsShanheStartingResourceDestroyedUseMaterial(long useId, int cardId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheStartingResourceDestroyedEvent>().Any(e => e.CardId == cardId && e.ParentFrameId == useId && _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == e.MovementSequence)is { } m && IsShanheStartingResourceDestroyedMaterial(m));
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4200)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheStartingResourceUseEntries(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureShanheStartingResourceUseEntries(completed);
        return completed;
    }
}
