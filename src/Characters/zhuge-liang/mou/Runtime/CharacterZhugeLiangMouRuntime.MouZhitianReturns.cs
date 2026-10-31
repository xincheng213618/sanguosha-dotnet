#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal bool TryResolveMouZhitianFireDiscard(CardUseFrame pending, int? selectedCardId)
    {
        if (selectedCardId is not { } id || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DrawPile || !HasMouOperation(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat], SkillProgramEffectOp.MouZhitianView))
            return false;
        var revealed = _coreNativeFireAttackOperations.GetFireAttackRevealedCard(pending);
        var chosen = MouZhitianTop(pending.SourceSeat).SingleOrDefault(card => card.Id == id && card.Suit == revealed.Suit) ?? throw new InvalidOperationException("Zhitian requires one matching-suit card from the current visible top cards.");
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { MouZhitianPayment = new(id, revealed.Suit, _coreCardMovementJournalCardMovementsAccess._cardMovements.Count) });
        _corePhysicalMovementFacadesOperations.MoveCard(chosen, CardLocation.DrawPile, CardLocation.DiscardPile, CardMoveReasons.FireAttackDiscard);
        ContinueMouZhitianFirePayment(pending.Id);
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 130, typeof(NativeFrameRouteConditions), "MouZhitianPayment")]
    internal void ContinueMouZhitianFirePayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MouZhitianPayment: { } receipt } use || use.Id != id || use.FireAttackSelection?.RevealedCardId is not { } revealedId || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.Sequence > receipt.MovementSequence && move.CardId == receipt.CardId && move.From == CardLocation.DrawPile && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.FireAttackDiscard))
            throw new InvalidOperationException("Zhitian lost its once-paid deck replacement and owning FireAttack.");
        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        var target = global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(use);
        var allowed = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(id, use.SourceSeat, target, revealedId, receipt.RevealedSuit, receipt.CardId, allowed));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouZhitianPayment = null, FireAttackSelection = null });
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, true);
        if (!allowed)
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(id, card);
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, use.CardKind);
            return;
        }

        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, use.SourceSeat, target, IsMouZhijueZeroUse(id) || _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) ? null : card, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }
}
