#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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

        var cards = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card)).Where(card => CanUseCardAsResponse(responder, card, requiredCardKind)).Where(card => !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(responder, requiredCardKind, [card]));
        cards = cards.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(responder).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(responder)).Where(card => _coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(responder, [card], requiredCardKind) && !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(responder, requiredCardKind, [card]) && _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, requiredCardKind, forResponse: true).Count != 0));
        if (requiredCardKind == CardKind.Slash)
            cards = cards.Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(responder, forResponse: true));
        if (requiredCardKind == CardKind.Dodge)
            cards = cards.Concat(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, requiredCardKind, true).Select(selection => selection.Cards[0]));
        cards = cards.Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileResponseCards(responder, requiredCardKind).Where(card => CanUseCardAsResponse(responder, card, requiredCardKind) && !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(responder, requiredCardKind, [card])));
        cards = cards.Concat(_characterMaChaoAnniversaryGodOperations.ShouliFieldResponseCards(responder, requiredCardKind).Where(card => _coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(responder, [card], requiredCardKind) && !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(responder, requiredCardKind, [card])));
        cards = cards.Where(card => !_coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(responder.Seat, card.Id));
        return cards.Where(card => !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(responder.Seat, requiredCardKind, [card.Id], true) && _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(responder, requiredCardKind, [card.Id])).DistinctBy(card => card.Id).ToArray();
    }

    internal IReadOnlyList<IReadOnlyList<Card>> GetZhangbaSlashPairs(CharacterState responder)
    {
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(responder.Seat, CardKind.Slash))
            return[];
        if (!_coreNativeTargetLegalityOperations.CanUseZhangbaSerpentSpear(responder))
        {
            return[];
        }

        var hand = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card) && !_characterZhugeLiangWumiaoOperations.IsWuZglCopyBlocked(responder.Seat, card.Id)).OrderBy(card => card.Id).ToArray();
        var pairs = new List<IReadOnlyList<Card>>();
        for (var first = 0; first < hand.Length - 1; first++)
        {
            for (var second = first + 1; second < hand.Length; second++)
            {
                pairs.Add(Array.AsReadOnly(new[] { hand[first], hand[second] }));
            }
        }

        return pairs;
    }

    internal IReadOnlyList<Card>? FindZhangbaSlashPair(CharacterState responder, IReadOnlyList<int> requestedCardIds)
    {
        if (requestedCardIds.Count != 2 || requestedCardIds.Distinct().Count() != 2)
        {
            return null;
        }

        var requested = requestedCardIds.Order().ToArray();
        return GetZhangbaSlashPairs(responder).FirstOrDefault(pair => pair.Select(card => card.Id).Order().SequenceEqual(requested));
    }

    internal bool CanConvertResponse(CharacterState responder, Card card, CardKind requiredCardKind)
    {
        var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(responder, card);
        if (identities.Count != 0)
            return identities.Any(match => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind));
        return _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, requiredCardKind, forResponse: true).Count != 0;
    }

    internal CardKind GetEffectiveResponseKind(CharacterState responder, Card responseCard, CardKind requiredCardKind)
    {
        var identities = _coreActionQueriesOperations.GetProgramCardIdentityMatches(responder, responseCard);
        if (identities.Count != 0)
        {
            return identities.Any(match => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind)) ? requiredCardKind : throw new InvalidOperationException($"The mandatory identity of response card {(responseCard.Id)} cannot satisfy {(requiredCardKind)}.");
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(responseCard.Kind, requiredCardKind))
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
        return identities.Count != 0 ? identities.Any(match => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(match.Identity.OutputKind, requiredCardKind)) : global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(card.Kind, requiredCardKind) || CanConvertResponse(responder, card, requiredCardKind);
    }
}
