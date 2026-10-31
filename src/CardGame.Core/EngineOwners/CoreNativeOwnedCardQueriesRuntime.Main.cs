#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeOwnedCardQueriesRuntime
{
    internal Card? FindOwnedPlayableCard(CharacterState actor, int? cardId)
    {
        if (cardId is not { } requestedCardId)
        {
            return null;
        }

        return _coreCardDeclarationsOperations.AvailableDeclarationCard(actor)is { } declared && declared.Id == requestedCardId ? declared : _corePhysicalOwnedCardQueriesOperations.GetHand(actor).SingleOrDefault(card => card.Id == requestedCardId) ?? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(actor).SingleOrDefault(card => card.Id == requestedCardId) ?? _corePhysicalOwnedCardQueriesOperations.GetEquipment(actor).SingleOrDefault(card => card.Id == requestedCardId) ?? _corePhysicalOwnedCardQueriesOperations.GetAuthority(actor).SingleOrDefault(card => card.Id == requestedCardId) ?? _coreOwnPublicPileConversionsOperations.FindOwnPublicPileConversionCard(actor, requestedCardId) ?? _coreNativePublicPileOriginalCardsOperations.FindNativePublicPileOriginalCard(actor, requestedCardId) ?? _coreLastZoneConversionsOperations.FindLastZoneJudgmentConversionCard(actor, requestedCardId) ?? _characterCaoAngMouOperations.MouCaQueryOriginalCard(actor, requestedCardId) ?? _characterZhangHuanAnniversaryOperations.AnniversaryZhangHuanQueryOriginalCard(actor, requestedCardId) ?? _characterWeiYanHappyGodOperations.HappyWeiYanBoneReturnCard(actor, requestedCardId) ?? _characterFuRongOlOperations.FindFuRongDiscardCard(actor, requestedCardId) ?? _characterYaDanOlOperations.FindQingyaDiscardCard(actor, requestedCardId) ?? _characterDuanJiongOlOperations.FindDuanJiongSaoguDiscardCard(actor, requestedCardId) ?? _characterMaChaoAnniversaryGodOperations.FindShouliFieldCard(actor, requestedCardId) ?? _characterDuKuiAnniversaryOperations.FindDuKuiFanyinRevealedCard(actor, requestedCardId) ?? _characterBaiLingYunAnniversaryOperations.FindAnniversaryBaiLingYunViewedCard(actor, requestedCardId) ?? _characterLuSuMobileShiOperations.FindShiLsPlayableCard(actor, requestedCardId) ?? _characterZhouFeiBoundaryMobileOperations.FindBoundaryZhouFeiUseCard(actor, requestedCardId);
    }

    internal CardLocation FindOwnedCardLocation(CharacterState actor, Card card)
    {
        if (_characterZhouFeiBoundaryMobileOperations.IsBoundaryZhouFeiUseQuery(actor, card.Id))
            return _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (_characterLuSuMobileShiOperations.TryShiLsHandLocation(actor, card, out var sharedHand))
            return sharedHand;
        if (_characterMaChaoAnniversaryGodOperations.TryShouliFieldLocation(actor, card, out var fieldMount))
            return fieldMount;
        if (_coreNativePublicPileOriginalCardsOperations.TryNativePublicPilePaymentLocation(actor, card, out var originalPile))
            return originalPile;
        if (_characterGongsunZanMouMobileOperations.IsMouGszHuCard(actor, card.Id))
            return _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (_characterWeiYanHappyGodOperations.IsHappyWeiYanBoneReturnQuery(actor, card.Id) || _characterCaoAngMouOperations.IsMouCaExactCardQuery(actor, card.Id) || _characterZhangHuanAnniversaryOperations.IsAnniversaryZhangHuanExactCardQuery(actor, card.Id) || _characterFuRongOlOperations.IsFuRongSelectedDiscard(actor.Seat, card.Id) || _characterYaDanOlOperations.IsQingyaSelectedDiscard(actor.Seat, card.Id) || _characterDuanJiongOlOperations.IsDuanJiongSaoguSelectedDiscard(actor.Seat, card.Id))
            return CardLocation.DiscardPile;
        if (_coreRequestedDeckBasicsOperations.IsSelectedRequestedDeckBasicMaterial(actor.Seat, card.Id))
            return CardLocation.DrawPile;
        if (_coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, card.Id)is { } paid)
            return paid.Cost.From;
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(candidate => candidate.Id == card.Id))
        {
            return CardLocation.Hand(actor.Seat);
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor).Any(candidate => candidate.Id == card.Id))
        {
            return CardLocation.Equipment(actor.Seat);
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(actor).Any(candidate => candidate.Id == card.Id))
        {
            return CardLocation.WoodenOxGrain(actor.Seat);
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetAuthority(actor).Any(candidate => candidate.Id == card.Id))
        {
            return CardLocation.Authority(actor.Seat);
        }

        if (_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(actor, card) && _coreOwnPublicPileConversionsOperations.FindOwnPublicPileConversionCard(actor, card.Id)is not null)
            return _cardZonesOperations._cardZones.GetLocation(card.Id);
        if (_coreLastZoneConversionsOperations.TryGetLastZoneJudgmentConversionLocation(actor, card, out var lastJudgmentLocation))
            return lastJudgmentLocation;
        throw new InvalidOperationException($"The chosen card is no longer in the actor's playable zones. " + $"Actor={(actor.Seat)}; card={(card.Id)}; location={(_cardZonesOperations._cardZones.GetLocation(card.Id))}.");
    }

    internal static CardMoveReason GetTargetCardMoveReason(LegalActionKind actionKind, CardZoneKind sourceZone) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetTargetCardMoveReason(actionKind, sourceZone);
    internal static CardMoveReason GetTargetCardFinishedMoveReason(LegalActionKind actionKind, CardZoneKind sourceZone) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetTargetCardFinishedMoveReason(actionKind, sourceZone);
    internal static LegalAction? SelectPlayAction(IReadOnlyList<LegalAction> legalActions, Card card, IReadOnlyList<int> targets, CardKind? playedCardKind, int? targetCardId, CardConversionSource? conversionSource, IReadOnlyList<CardConversionSource>? additionalConversionSources) => RuntimeSkills.RuntimeRules_Core_CommonRules.SelectPlayAction(legalActions, card, targets, playedCardKind, targetCardId, conversionSource, additionalConversionSources);
    internal void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, CardKind effectiveCardKind)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && actorSeat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveCardKind))
        {
            _coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess._usedOrPlayedSlashDuringPlayPhase = true;
        }
    }
}
