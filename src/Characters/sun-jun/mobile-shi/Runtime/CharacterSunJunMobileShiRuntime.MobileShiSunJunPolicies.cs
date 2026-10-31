#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJunMobileShiRuntime
{
    internal static bool ShiSjRealSuit(Suit? suit) => suit is Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond;
    internal bool ShiSjSuitSeen(int actor, Suit suit) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunHandUseEvent>().Any(e => e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Actor == actor && e.Suit == suit);
    internal bool ShiSjHandSource(int actor, CardLocation from) => from.Zone == CardZoneKind.Hand && from.OwnerSeat is { } owner && (owner == actor || _characterLuSuMobileShiOperations.HasShiLsHandView(actor, owner));
    internal bool ShiSjOwnTurn(int actor) => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == actor && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat == actor && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == actor);
    internal bool HasShiSunJunFirstSuitAllowance(CharacterState actor, Card? material = null, CardConversionSource? conversion = null)
    {
        if (!actor.IsAlive || !ShiSjOwnTurn(actor.Seat) || !_coreNationalSkillsOperations.HasRuntimeSkill(actor, ShiSjXianshuai))
            return false;
        if (material is null)
            return new[]
            {
                Suit.Spade,
                Suit.Heart,
                Suit.Club,
                Suit.Diamond
            }.Any(s => !ShiSjSuitSeen(actor.Seat, s));
        if (material.Id <= 0 || !ShiSjHandSource(actor.Seat, _cardZonesOperations._cardZones.GetLocation(material.Id)))
            return false;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, material, conversion));
        return ShiSjRealSuit(suit) && !ShiSjSuitSeen(actor.Seat, suit);
    }

    internal bool ShiSjMaterialAllowance(CharacterState actor, IReadOnlyList<int>? ids, Card? appearance = null, Suit? effectiveSuit = null)
    {
        if (ids is not { Count: > 0 } || ids.Distinct().Count() != ids.Count || ids.Any(id => id <= 0 || !ShiSjHandSource(actor.Seat, _cardZonesOperations._cardZones.GetLocation(id))))
            return false;
        var cards = ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        Suit[] suits = effectiveSuit is { } frozen ? [frozen] : appearance is not null && ids is [var id] && appearance.Id == id ? [_coreNativeCardAppearanceOperations.EffectiveSuit(actor, appearance)] : cards.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, c)).Distinct().ToArray();
        return suits is [var suit] && ShiSjRealSuit(suit) && ShiSjOwnTurn(actor.Seat) && _coreNationalSkillsOperations.HasRuntimeSkill(actor, ShiSjXianshuai) && !ShiSjSuitSeen(actor.Seat, suit);
    }

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 49200)]
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 49200)]
    internal bool ShiSjFirstSuitSlashQuota(ProgramSlashQuotaQuery q) => ShiSjMaterialAllowance(q.Actor, q.PhysicalCardIds ?? (q.PhysicalCard is { Id: > 0 } card ? [card.Id] : []), q.PhysicalCard, q.EffectiveSuit);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49200)]
    internal void CaptureShiSjActualHandUse(CardUseDeclaredEvent fact)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMobileShiXiongtu) || _coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use, EffectiveSuit: { } suit } a } || !ShiSjRealSuit(suit) || !ShiSjOwnTurn(a.ActorSeat) || a.PhysicalCards.Count == 0 || a.PhysicalCards.Any(c => !ShiSjHandSource(a.ActorSeat, c.From)) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunHandUseEvent>().Any(e => e.UseId == fact.ResolutionId))
            return;
        RecordShiSjHandAction(fact.ResolutionId, a, suit);
    }

    internal void RecordShiSjHandAction(long useId, CardActionContext a, Suit suit)
    {
        if (!ShiSjOwnTurn(a.ActorSeat) || !ShiSjRealSuit(suit) || a.PhysicalCards.Count == 0 || a.PhysicalCards.Any(c => !ShiSjHandSource(a.ActorSeat, c.From)) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunHandUseEvent>().Any(e => e.ActionId == a.ActionId))
            return;
        var first = !ShiSjSuitSeen(a.ActorSeat, suit);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunHandUseEvent(useId, a.ActionId, a.ActorSeat, a.ProviderSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, suit, first, first && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], ShiSjXianshuai)));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 49201)]
    internal void CaptureShiSjResponseHandUse(CardActionAcceptedEvent fact)
    {
        var a = fact.Action;
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMobileShiXiongtu) || a.Type != CardActionType.Response || a.EffectiveSuit is not { } suit)
            return;
        if (_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, ProgramCompletedResponseKind.Nullification) && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counterspell)
        {
            RecordShiSjHandAction(counterspell.Id, a, suit);
            return;
        }

        // A Dodge against an actual Slash is a Use; an Arrow Barrage Dodge and
        // a Duel/Barbarian Slash retain their ordinary played-response meaning.
        if (_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, ProgramCompletedResponseKind.Dodge) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == a.ParentActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(f.CardKind) && f.CardAttack?.TargetSeat == a.ActorSeat)is { } parent)
            RecordShiSjHandAction(parent.Id, a, suit);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 49200)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AfterSlashDebitIdentity, 49200)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 49200)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 49200)]
    internal bool ShiSjIssuedFirstSuitUse(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use } a } && a.ActorSeat == actor && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunHandUseEvent>().Any(e => e.UseId == id && e.ActionId == a.ActionId && e.Actor == actor && e.Provider == a.ProviderSeat && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Suit == a.EffectiveSuit && e.First && e.Exempt) && a.PhysicalCards.Count > 0 && a.PhysicalCards.All(c => c.From.Zone == CardZoneKind.Hand);
}
