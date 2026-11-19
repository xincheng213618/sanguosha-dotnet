#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePindianPoliciesRuntime
{
    internal bool CanOfferPindianTopChoice(CharacterState owner) => (_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) > 0 || _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile) > 0) && _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianTopCardChoice).Any();
    internal int EffectivePindianRank(CharacterState owner, Card card)
    {
        var rank = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianRankBySuit).Where(binding => binding.Policy.InputSuit == _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)).Select(binding => binding.Policy.Value).DefaultIfEmpty(_coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(owner, card)).Max();
        // Several policy nodes on one enabled instance still describe one bonus.
        // Recompute from the real face/identity each time, never from a prior result.
        foreach (var instance in _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianRankBonus).GroupBy(binding => (binding.Source.SkillId, binding.Source.SkillInstanceId)))
            rank = (int)Math.Min(13L, (long)rank + instance.Max(binding => binding.Policy.Value));
        rank = (int)Math.Min(13L, (long)rank + _characterCuiLieAnniversaryOperations.AnniversaryCuiLiePindianBonus(owner.Seat));
        return rank;
    }

    internal bool HasCurrentPindianSourceCard(PindianFrame frame)
    {
        if (frame.WuLuKangContest is { SourcesReady: true } contest)
            return contest.Assignments[0].CardId > 0 &&
                _cardZonesOperations._cardZones.GetLocation(contest.Assignments[0].CardId) ==
                _characterLuKangWumiaoOperations.WuLuKangAssignedLocation(frame, frame.SourceSeat);
        var source = _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat];
        if (frame.SourceCardId == 0 && _characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(source))
            return true;
        if (frame.OwnedPileSource is not null)
            return _characterGuanHaiOlOperations.IsGuanHaiPindianSource(frame, false);
        return frame.SourceUsesDrawPileTop
            ? HoldsPindianSourceTop(frame)
            : _corePhysicalOwnedCardQueriesOperations.GetHand(source).Any(card => card.Id == frame.SourceCardId);
    }

    private bool HoldsPindianSourceTop(PindianFrame frame) => frame.SourceCardId is { } id &&
        frame.SourceTopReservationSequence is { } entry && _zoneEntries.CurrentEntry(CardLocation.Processing, id) == entry &&
        !(frame.ParentProcessingCardIds ?? []).Contains(id);

    internal (int CardId, int? ReservationSequence) ReserveOrReadPindianTop(CharacterState owner, bool reserve)
    {
        if (!CanOfferPindianTopChoice(owner) || !_corePhysicalDrawFacadesOperations.EnsureDrawPile())
            throw new InvalidOperationException("The optional Pindian top source is unavailable.");
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last();
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianTopCardChoice).First();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianTopSourceChosenEvent(owner.Seat, new(binding.Source.SkillId, binding.Policy.Id, owner.Seat, binding.Source.SkillInstanceId), card.Id));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(binding.Source.SkillId).Name)}】，使用牌堆顶的牌拼点。", owner.Seat);
        if (!reserve) return (card.Id, null);

        CardMovementRecord? reservation = null;
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing,
            new CardMoveReason("pindian.reserve-top"), movement => reservation = movement);
        if (reservation is not { From: var from, To: var to } || from != CardLocation.DrawPile || to != CardLocation.Processing)
            throw new InvalidOperationException("Pindian did not reserve its original draw-pile selection in Processing.");
        return (card.Id, reservation.Sequence);
    }

    internal int[] CurrentPindianProcessingIds(PindianFrame frame)
    {
        if (frame.WuLuKangContest is not null)
            return _characterLuKangWumiaoOperations.WuLuKangHeldProcessing(frame).ToArray();
        if (frame.Result is { } result)
            return _corePindianOperations.AvailablePindianCards(result);
        return frame.SourceUsesDrawPileTop && frame.SourceCardId is { } id && HoldsPindianSourceTop(frame) ? [id] : [];
    }
}
