#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangMouMobileRuntime
{
    internal IEnumerable<MouHanDangGongqiGrantedEvent> MouHanDangActionRestrictions(long action) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangUseRestrictedEvent>().Where(e => e.ActionId == action).SelectMany(e => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangGongqiGrantedEvent>().Where(g => g.FrameId == e.GrantFrameId));
    internal CardActionContext? CurrentMouHanDangRespondedAction()
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault();
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { ChainDepth: > 0 } n)
        {
            var original = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == n.ParentFrameId)?.Action;
            return _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Nullification && a.ParentActionId == original?.ActionId) ?? original;
        }

        return use?.Action;
    }

    internal bool IsMouHanDangNativeResponseAllowed(CharacterState provider, Card card, bool dyingUse = false)
    {
        if (!HasMouHanDangCapability || dyingUse || CurrentMouHanDangRespondedAction()is not { } original || original.ActorSeat == provider.Seat)
            return true;
        var grants = MouHanDangActionRestrictions(original.ActionId).ToArray();
        return grants.Length == 0 || _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(provider.Seat) && _coreActionDiscardColorsOperations.CapturePhysicalCardColor(provider.Seat, card)is { } red && grants.All(g => g.Suit is Suit.Heart or Suit.Diamond or Suit.Spade or Suit.Club && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(g.Suit) == red);
    }

    internal bool IsMouHanDangResponseAvailable(CharacterState provider, CardKind kind, IReadOnlyList<int> ids, bool dyingUse = false) => !HasMouHanDangCapability || dyingUse || ids.Count != 1 || IsMouHanDangNativeResponseAllowed(provider, _coreAdvancedLifecycleOperations.GetAdvancedCard(ids[0])) || _coreCardConversionsOperations.GetProgramCardIdentitySources(provider, _coreAdvancedLifecycleOperations.GetAdvancedCard(ids[0]), kind, true).Count != 0 || _coreActionQueriesOperations.GetProgramViewAsConversions(provider, _coreAdvancedLifecycleOperations.GetAdvancedCard(ids[0]), kind, true).Count != 0;
    internal bool IsMouHanDangPublishedResponseAllowed(CharacterState provider, Card card, CardConversionSource? source, bool forResponse, bool dyingUse = false) => !forResponse || dyingUse || source is not null || IsMouHanDangNativeResponseAllowed(provider, card);
    internal void SignMouHanDangUseRestrictions(CardActionContext a)
    {
        var isUse = a.Type == CardActionType.Use || a.Type == CardActionType.Response && a.ActorSeat == a.ProviderSeat && a.RequesterSeat is null && (a.EffectiveKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], a.EffectiveKind));
        if (!isUse || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != a.ActorSeat)
            return;
        foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangGongqiGrantedEvent>().Where(g => g.Source.OwnerSeat == a.ActorSeat && g.Phase == phase).ToArray())
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangUseRestrictedEvent>().Any(e => e.ActionId == a.ActionId && e.GrantFrameId == grant.FrameId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangUseRestrictedEvent(a.ActionId, a.ActorSeat, grant.FrameId));
    }
}
