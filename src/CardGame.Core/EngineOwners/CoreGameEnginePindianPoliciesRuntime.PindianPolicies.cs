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

    internal bool HasCurrentPindianSourceCard(PindianFrame frame) => frame.WuLuKangContest is { SourcesReady: true } w ? w.Assignments[0].CardId > 0 && _cardZonesOperations._cardZones.GetLocation(w.Assignments[0].CardId) == _characterLuKangWumiaoOperations.WuLuKangAssignedLocation(frame, frame.SourceSeat) : frame.SourceCardId == 0 && _characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat]) ? true : frame.OwnedPileSource is not null ? _characterGuanHaiOlOperations.IsGuanHaiPindianSource(frame, false) : frame.SourceUsesDrawPileTop ? frame.SourceCardId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !(frame.ParentProcessingCardIds ?? []).Contains(id) : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat]).Any(c => c.Id == frame.SourceCardId);
    internal int ReserveOrReadPindianTop(CharacterState owner, bool reserve)
    {
        if (!CanOfferPindianTopChoice(owner) || !_corePhysicalDrawFacadesOperations.EnsureDrawPile())
            throw new InvalidOperationException("The optional Pindian top source is unavailable.");
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last();
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianTopCardChoice).First();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianTopSourceChosenEvent(owner.Seat, new(binding.Source.SkillId, binding.Policy.Id, owner.Seat, binding.Source.SkillInstanceId), card.Id));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(binding.Source.SkillId).Name)}】，使用牌堆顶的牌拼点。", owner.Seat);
        if (reserve)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new CardMoveReason("pindian.reserve-top"));
        return card.Id;
    }

    internal int[] CurrentPindianProcessingIds(PindianFrame frame) => frame.WuLuKangContest is not null ? _characterLuKangWumiaoOperations.WuLuKangHeldProcessing(frame).ToArray() : frame.Result is { } result ? _corePindianOperations.AvailablePindianCards(result) : frame.SourceUsesDrawPileTop && frame.SourceCardId is { } id ? [id] : [];
}
