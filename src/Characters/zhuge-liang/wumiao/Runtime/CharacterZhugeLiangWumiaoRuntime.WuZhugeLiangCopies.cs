#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangWumiaoRuntime
{
    internal WuZhugeLiangCopyCreatedEvent? WuZglCopyFact(int id) => !HasWuZhugeLiangCapability ? null : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuZhugeLiangCopyCreatedEvent>().SingleOrDefault(e => e.CardId == id && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id) && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == e.Kind && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit == e.Suit && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank == e.Rank);
    internal bool IsWuZglCopyBlocked(int actor, int id) => HasWuZhugeLiangCapability && WuZglTurnOpen(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuZhugeLiangCopyReturnedEvent>().Any(e => e.CardId == id && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && WuZglCopyFact(id)?.Source.OwnerSeat == e.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.ReturnSequence && m.CardId == id && m.To == CardLocation.Hand(e.OwnerSeat) && m.Reason == e.Reason));
    internal bool IsWuZglUseMaterialForbidden(int actor, IReadOnlyList<int> ids, long? existingUseId = null)
    {
        var accepted = existingUseId is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } a } && a.ActorSeat == actor && a.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(ids.Order());
        return !accepted && ids.Any(id => IsWuZglCopyBlocked(actor, id));
    }

    internal static bool WuZglNativeInputReason(CardMoveReason r) => RuntimeSkills.RuntimeRules_zhuge_liang_wumiao_WuZhugeLiang_Copies.WuZglNativeInputReason(r);
    internal CardMovementRecord? WuZglCopyReturnEntry(Card card, CardLocation from, CardMoveReason reason)
    {
        if (WuZglCopyFact(card.Id)is not { } copy || !_coreMatchParticipantsPlayersAccess._players[copy.Source.OwnerSeat].IsAlive)
            return null;
        var owner = copy.Source.OwnerSeat;
        if (from == CardLocation.Processing)
        {
            if (!(reason == CardMoveReasons.UseFinished || reason == CardMoveReasons.ResponseFinished || reason == CardMoveReasons.NullificationFinished || reason == CardMoveReasons.IronChainFinished || reason == CardMoveReasons.FireAttackFinished || reason == CardMoveReasons.DismantlementFinished || reason == CardMoveReasons.SnatchFinished))
                return null;
            var entered = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To == CardLocation.Processing);
            if (entered is null || entered.From.OwnerSeat != owner || !WuZglNativeInputReason(entered.Reason))
                return null;
            if (entered.Reason.Value == "conversion.declaration.pay" && !_resolutionStackOperations._resolutionStack.Any(f => f.AcceptedDeclarationPayment is { Claimed: true } p && p.ActorSeat == owner && p.ProviderSeat == owner && p.Cost.CardId == card.Id))
                return null;
            return entered;
        }

        if (from.Zone == CardZoneKind.Judgment && reason == CardMoveReasons.DelayedCardFinish)
        {
            var placed = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To.Zone == CardZoneKind.Judgment);
            if (placed is null)
                return null;
            var entered = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.Sequence <= placed.Sequence && m.To == CardLocation.Processing);
            return entered is not null && entered.From.OwnerSeat == owner && WuZglNativeInputReason(entered.Reason) ? entered : null;
        }

        return null;
    }

    internal CardLocation NormalizeWuZglCopyDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => to == CardLocation.DiscardPile && WuZglCopyReturnEntry(card, from, reason)is not null && WuZglCopyFact(card.Id)is { } copy ? CardLocation.Hand(copy.Source.OwnerSeat) : to;
    internal void RecordWuZglCopyMovement(CardMovementRecord move)
    {
        if (move.To.Zone != CardZoneKind.Hand || WuZglCopyFact(move.CardId)is not { } copy || move.To.OwnerSeat != copy.Source.OwnerSeat || WuZglCopyReturnEntry(_coreAdvancedLifecycleOperations.GetAdvancedCard(move.CardId), move.From, move.Reason)is not { } entry)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangCopyReturnedEvent(copy.Source.OwnerSeat, move.CardId, entry.Sequence, move.Sequence, move.Reason, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat));
    }

    internal bool IsWuZglReturnedCopyCost(CardActionContext action, CardActionCost cost, CardMovementRecord? finish = null)
    {
        if (WuZglCopyFact(cost.CardId)is not { } copy || action.ProviderSeat != copy.Source.OwnerSeat || cost.From.OwnerSeat != copy.Source.OwnerSeat || !action.PhysicalCards.Contains(cost))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuZhugeLiangCopyReturnedEvent>().Any(e => e.OwnerSeat == copy.Source.OwnerSeat && e.CardId == cost.CardId && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.EntrySequence && m.CardId == cost.CardId && m.From == cost.From && m.To == CardLocation.Processing && WuZglNativeInputReason(m.Reason)) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.ReturnSequence && m.CardId == cost.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(copy.Source.OwnerSeat) && m.Reason == e.Reason && e.ReturnSequence > e.EntrySequence && (finish is null || m == finish)));
    }

    // Registration only; the query still belongs to legacy GameEngine.WuZhugeLiang.Copies.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 180)]
    internal void ProjectWuZhugeLiangCopyCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(WuZglCopyLabel(cardId));
    }

    internal string? WuZglCopyLabel(int id) => WuZglCopyFact(id)is not { } copy ? null : IsWuZglCopyBlocked(copy.Source.OwnerSeat, id) ? "智哲·本回合禁用" : "智哲";
}
