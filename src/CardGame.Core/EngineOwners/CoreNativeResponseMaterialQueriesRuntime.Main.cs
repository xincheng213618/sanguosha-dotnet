#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeResponseMaterialQueriesRuntime
{
    internal IReadOnlyList<Card> GetResponseCards(CharacterState responder, CardKind requiredCardKind)
    {
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(responder.Seat, requiredCardKind))
            return[];
        if (_characterCaoPiMouOperations.MouCaoPiResponseForbidden(responder.Seat, requiredCardKind, CardActionType.Response))
            return[];
        if (_characterXiahouHuiAnniversaryOperations.AnniversaryXiahouHuiResponseForbidden(responder.Seat, requiredCardKind))
            return[];
        if (_coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(responder.Seat) && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(responder, requiredCardKind) || requiredCardKind == CardKind.Slash && _coreCardUseModulesOperations.IsCardUseForbidden(responder.Seat, CardKind.Slash, CardActionType.Response))
        {
            return[];
        }

        var responseIsUse = requiredCardKind == CardKind.Nullification || requiredCardKind == CardKind.Dodge &&
            _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(responder, requiredCardKind);
        var cards = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card)).Where(card => CanUseCardAsResponse(responder, card, requiredCardKind)).Where(card => !(responseIsUse && _appliedCardActionRestrictions.IsMaterialUseForbidden(responder.Seat, [card.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(responder, card))));
        cards = cards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(responder).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(responder)).Where(card => _coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(responder, [card], requiredCardKind) && !(responseIsUse && _appliedCardActionRestrictions.IsMaterialUseForbidden(responder.Seat, [card.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(responder, card))) && _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, requiredCardKind, forResponse: true).Count != 0));
        if (requiredCardKind == CardKind.Slash)
            cards = cards.Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(responder, forResponse: true));
        if (requiredCardKind == CardKind.Dodge)
            cards = cards.Concat(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, requiredCardKind, true).Select(selection => selection.Cards[0]));
        cards = cards.Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileResponseCards(responder, requiredCardKind).Where(card => CanUseCardAsResponse(responder, card, requiredCardKind) && !(responseIsUse && _appliedCardActionRestrictions.IsMaterialUseForbidden(responder.Seat, [card.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(responder, card)))));
        cards = cards.Concat(_characterMaChaoAnniversaryGodOperations.ShouliFieldResponseCards(responder, requiredCardKind).Where(card => _coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(responder, [card], requiredCardKind) && !(responseIsUse && _appliedCardActionRestrictions.IsMaterialUseForbidden(responder.Seat, [card.Id], _coreNativeCardAppearanceOperations.EffectiveSuit(responder, card)))));
        cards = cards.Where(card => !_coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(responder.Seat, card.Id));
        return cards.Where(card => !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(responder.Seat, requiredCardKind, [card.Id], true) && _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(responder, requiredCardKind, [card.Id])).DistinctBy(card => card.Id).ToArray();
    }

    // Material availability is independent of weapon and card-use admission.
    // Both native admission and pair queries consume this one eligibility rule.
    private IEnumerable<Card> EligibleZhangbaMaterials(CharacterState responder) =>
        _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Where(card =>
            !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card) &&
            !_characterZhugeLiangWumiaoOperations.IsWuZglCopyBlocked(responder.Seat, card.Id));

    internal bool HasZhangbaSlashMaterials(CharacterState responder) =>
        EligibleZhangbaMaterials(responder).Take(2).Count() == 2;

    private Card[] CaptureZhangbaMaterials(CharacterState responder)
    {
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(responder.Seat, CardKind.Slash))
            return[];
        if (!_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(responder))
        {
            return[];
        }

        return EligibleZhangbaMaterials(responder).ToArray();
    }

    private Card[] CaptureOrderedZhangbaMaterials(CharacterState responder)
    {
        var materials = CaptureZhangbaMaterials(responder);
        Array.Sort(materials, static (left, right) => left.Id.CompareTo(right.Id));
        return materials;
    }

    internal IReadOnlyList<IReadOnlyList<Card>> GetZhangbaSlashPairs(CharacterState responder)
    {
        var hand = CaptureOrderedZhangbaMaterials(responder);
        var pairs = new List<IReadOnlyList<Card>>();
        for (var first = 0; first < hand.Length - 1; first++)
        for (var second = first + 1; second < hand.Length; second++)
            pairs.Add(Array.AsReadOnly(new[] { hand[first], hand[second] }));
        return pairs;
    }

    internal IEnumerable<IReadOnlyList<Card>> EnumerateZhangbaSlashPairs(CharacterState responder)
    {
        // Capture now, before returning the iterator. A pending enumeration owns
        // this query's materials even if the physical zones change before MoveNext.
        var materials = CaptureOrderedZhangbaMaterials(responder);
        return Pairs(materials);

        static IEnumerable<IReadOnlyList<Card>> Pairs(Card[] hand)
        {
            for (var first = 0; first < hand.Length - 1; first++)
            for (var second = first + 1; second < hand.Length; second++)
            {
                yield return Array.AsReadOnly(new[] { hand[first], hand[second] });
            }
        }
    }

    internal IReadOnlyList<Card>? FindZhangbaSlashPair(CharacterState responder, IReadOnlyList<int> requestedCardIds)
    {
        if (requestedCardIds.Count != 2)
        {
            return null;
        }
        var firstId = requestedCardIds[0];
        var secondId = requestedCardIds[1];
        if (firstId == secondId) return null;
        Card? first = null;
        Card? second = null;
        foreach (var card in CaptureZhangbaMaterials(responder))
        {
            if (card.Id == firstId) first ??= card;
            if (card.Id == secondId) second ??= card;
            if (first is not null && second is not null) break;
        }
        return first is null || second is null ? null : first.Id < second.Id
            ? Array.AsReadOnly(new[] { first, second }) : Array.AsReadOnly(new[] { second, first });
    }

    internal IReadOnlyList<Card>? FirstZhangbaSlashPair(CharacterState responder)
    {
        Card? first = null;
        Card? second = null;
        foreach (var card in CaptureZhangbaMaterials(responder))
        {
            if (first is null || card.Id < first.Id)
            {
                second = first;
                first = card;
            }
            else if (second is null || card.Id < second.Id)
                second = card;
        }
        return second is null ? null : Array.AsReadOnly(new[] { first!, second });
    }

    internal bool CanConvertResponse(CharacterState responder, Card card, CardKind requiredCardKind)
    {
        var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(responder, card);
        if (identities.Count != 0)
            return identities.Any(match => CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind));
        return _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, requiredCardKind, forResponse: true).Count != 0;
    }

    internal CardKind GetEffectiveResponseKind(CharacterState responder, Card responseCard, CardKind requiredCardKind)
    {
        var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(responder, responseCard);
        if (identities.Count != 0)
        {
            return identities.Any(match => CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind)) ? requiredCardKind : throw new InvalidOperationException($"The mandatory identity of response card {(responseCard.Id)} cannot satisfy {(requiredCardKind)}.");
        }

        if (CoreNativeCardClassificationsRuntime.MatchesRequiredCard(responseCard.Kind, requiredCardKind))
        {
            return requiredCardKind;
        }

        var supportsLegacyMultiCardResponse = _coreResponseEntityExchangeOperations.HasResponseEntityExchangeObservers() || _coreActionDiscardColorsOperations.TracksActionDiscardColor;
        if ((supportsLegacyMultiCardResponse || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundDistinctBasicUse || _characterKongShuOlOperations.HasLeiluanCapability || _characterGuanHaiOlOperations.HasGuanHaiCapability) && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, requiredCardKind, true).Any(selection => selection.Cards[0].Id == responseCard.Id && (supportsLegacyMultiCardResponse || _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)is { RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null })))
            return requiredCardKind;
        return CanConvertResponse(responder, responseCard, requiredCardKind) ? requiredCardKind : throw new InvalidOperationException($"The response card {(responseCard.Id)} cannot be used as {(requiredCardKind)}.");
    }

    internal bool CanUseCardAsResponse(CharacterState responder, Card card, CardKind requiredCardKind)
    {
        if (_characterZhugeLiangWumiaoOperations.IsWuZglCopyBlocked(responder.Seat, card.Id) || !_coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(responder, [card], requiredCardKind))
            return false;
        var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(responder, card);
        return identities.Count != 0 ? identities.Any(match => CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind)) : CoreNativeCardClassificationsRuntime.MatchesRequiredCard(card.Kind, requiredCardKind) || CanConvertResponse(responder, card, requiredCardKind);
    }
}
