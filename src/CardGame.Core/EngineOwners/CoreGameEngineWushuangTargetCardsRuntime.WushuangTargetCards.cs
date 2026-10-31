#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangTargetCardsRuntime
{
    internal void BindWushuangTargetCards(NullificationWindowFrame pending, int targetSeat)
    {
        if (pending.ActionKind != LegalActionKind.Dismantlement || _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)is not { Action: { } action, WushuangTargetCards: null } use)
            return;
        var extra = Math.Max(_coreWushuangModeProjectionOperations.IsWushuangMode ? (_coreWushuangTacticRulesOperations.HasWushuangTactic(pending.SourceSeat, 2239) ? 1 : 0) + (_coreWushuangTacticRulesOperations.HasWushuangTactic(pending.SourceSeat, 2240) ? 2 : 0) : 0, _coreShanheDismantlementStockOperations.ShanheDismantlementSelectionLimit(pending.SourceSeat) - 1);
        if (extra > 0)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { WushuangTargetCards = new(action.ActionId, targetSeat, extra) });
    }

    internal bool IsWushuangExtraForeignSelection(long useId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.WushuangTargetCards is { ExtraSelection: true };
    internal bool TryContinueWushuangTargetCards(long id, Card effectCard, CardKind effectiveKind, int source, int target, LegalActionKind kind)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { WushuangTargetCards: { } receipt } use)
            return false;
        if (use.Action?.ActionId != receipt.ActionId || source != use.SourceSeat || target != receipt.TargetSeat || kind != LegalActionKind.Dismantlement)
            throw new InvalidOperationException("Additional Dismantlement lost its owning accepted effect.");
        if (receipt.Remaining <= 0 || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { WushuangTargetCards = null });
            return false;
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { WushuangTargetCards = receipt with { Remaining = receipt.Remaining - 1, ExtraSelection = true } });
        var continuation = new NullificationWindowFrame(id, id, source, [target], effectCard.Id, effectiveKind, kind, null, null, []);
        if (!_coreExtraEffectForeignSelectionOperations.TryBeginExtraForeignSelection(continuation, _coreMatchParticipantsPlayersAccess._players[target]))
            throw new InvalidOperationException("Additional Dismantlement did not create its typed selection.");
        return true;
    }

    internal void FinishWushuangTargetCards(CardUseFrame use)
    {
        if (use.WushuangTargetCards is not { ExtraSelection: true } receipt || use.Action?.ActionId != receipt.ActionId)
            throw new InvalidOperationException("Additional Dismantlement cannot decline another use's effect.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { WushuangTargetCards = null });
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, true);
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(use.Id, card);
        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, card, CardKind.Dismantlement);
    }

    internal void WushuangClaimNullifiedTrick(NullificationWindowFrame window, IReadOnlyList<Card> finishedPhysicalCards, int beforeFinishMovement)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || !window.EffectNullified)
            return;
        var response = _coreProgramEventHistoryOperations.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(e => e.ResolutionId == window.ParentFrameId && e.ChainDepth == window.ChainDepth && e.EffectNullified);
        if (response is null || !_coreWushuangTacticRulesOperations.HasWushuangTactic(response.ResponderSeat, 2008) || !_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat].IsAlive)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } action } use || action.ActorSeat != window.SourceSeat || !(use.PhysicalCardIds ?? [use.CardId]).SequenceEqual(finishedPhysicalCards.Select(card => card.Id)))
            throw new InvalidOperationException("A counterspell claim lost its exact native trick materials.");
        foreach (var card in finishedPhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.Sequence > beforeFinishMovement && move.CardId == c.Id && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished)))
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(response.ResponderSeat), new("wushuang.counterspell-claim"));
    }
}
