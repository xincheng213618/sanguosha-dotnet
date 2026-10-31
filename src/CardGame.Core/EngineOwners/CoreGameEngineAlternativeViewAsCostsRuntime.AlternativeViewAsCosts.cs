#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlternativeViewAsCostsRuntime
{
    internal static CardKind ProgramBasicCardName(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(kind);
    internal void RecordProgramUsedBasicCard(int actorSeat, CardKind effectiveKind)
    {
        if (CardCatalog.Get(effectiveKind).CategoryName != "基本牌")
            return;
        if (!_coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess._usedProgramBasicCardNames.TryGetValue(actorSeat, out var row) || row.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber)
            _coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess._usedProgramBasicCardNames[actorSeat] = row = (_coreActualTurnProgressTurnNumberAccess._turnNumber, []);
        row.Kinds.Add(ProgramBasicCardName(effectiveKind));
    }

    internal bool HasProgramUsedBasicCardThisTurn(int actorSeat, CardKind kind) => _coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess._usedProgramBasicCardNames.TryGetValue(actorSeat, out var row) && row.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && row.Kinds.Contains(ProgramBasicCardName(kind));
    internal bool IsProgramAlternativeCost(CardActionContext? action, int cardId) => _coreWushuangJinzhiOperations.IsWushuangJinzhiPaidMaterial(action, cardId) || action is not null && (action.Type == CardActionType.Use || action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Dodge && (action.ActorSeat == action.ProviderSeat || _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action)) && action.RequesterSeat is null) && action.PhysicalCards.Any(cost => cost.CardId == cardId) && action.ConversionChain.Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.CostDestination is SkillProgramCardDestination.DrawPileTop or SkillProgramCardDestination.DrawPileBottom);
    internal bool IsProgramResponseCardUse(CharacterState responder, CardKind effectiveKind) => !_characterSimaYiMouOperations.IsMouSmPlayedActor(responder) && effectiveKind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { EffectiveCardKind: { } incomingKind } attack && attack.TargetSeat == responder.Seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind);
    internal bool IsProgramTopDeckDodgeSource(CardConversionSource? source) => source is not null && _corePlayCardHistoryOperations.ViewAsRule(source)is { UseOnly: true, OutputKind: CardKind.Dodge, CostDestination: SkillProgramCardDestination.DrawPileTop };
    internal CardMovementRecord? DestroyedAlternativeResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation)
    {
        if (continuation != ProgramCardContinuation.Dodge || action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Dodge || action.ActorSeat != action.ProviderSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null || action.PhysicalCards is not [var only] || only != cost || cost.From != CardLocation.Equipment(action.ProviderSeat) || action.ConversionChain is not [var source] || source.OwnerSeat != action.ProviderSeat || !IsProgramTopDeckDodgeSource(source) || _coreCardUseLifecycleOperations.LifecycleCardUse(parentFrameId)is not { Action: { } incoming, CardAttack: { Active: true } attack } || action.ParentActionId != incoming.ActionId || attack.TargetSeat != action.ActorSeat || attack.SourceSeat != action.OpponentSeat || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.OutsideGame || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(fact => fact.CardId == cost.CardId).ToArray()is not [var destroyed] || destroyed.ParentFrameId != parentFrameId || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(fact => fact.Action.ActionId == action.ActionId && fact.Action.Type == action.Type && fact.Action.ActorSeat == action.ActorSeat && fact.Action.ProviderSeat == action.ProviderSeat && fact.Action.ParentActionId == action.ParentActionId && fact.Action.PhysicalCards.SequenceEqual(action.PhysicalCards) && fact.Action.ConversionChain.SequenceEqual(action.ConversionChain)))
            return null;
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(move => move.Sequence == destroyed.MovementSequence);
        return movement is not null && movement.CardId == cost.CardId && movement.CardKind == cost.CardKind && movement.From == cost.From && movement.Reason == CardMoveReasons.Respond && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) ? movement : null;
    }

    internal void PaySingleCardResponse(Card card, CharacterState provider, CardKind effectiveKind, CardConversionSource? source)
    {
        if (source is not null && _corePlayCardHistoryOperations.ViewAsRule(source)is { } usageRule && (usageRule.TieredRoundConversion is not null || usageRule.UnusedOutputNameThisGame || usageRule.UsesPerActualTurn is not null || usageRule.ConversionStateId is not null && usageRule.UsesPerPhase is not null || source is { SkillId: "guandu:battle-event", BindingId: "hold-line" }))
            _corePlayCardHistoryOperations.ConsumeProgramViewAsUsage([source]);
        _characterZhangFeiWumiaoOperations.PrepareWuZhangFeiResponseRanks(provider, effectiveKind, [card], source);
        _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(provider, effectiveKind, [card]);
        _characterMiHengLeOperations.PrepareSiruiResponsePayment(provider, card, effectiveKind, source);
        var alternative = IsProgramTopDeckDodgeSource(source);
        if (alternative && (source!.OwnerSeat != provider.Seat || !IsProgramResponseCardUse(provider, effectiveKind)))
            throw new InvalidOperationException("A top-deck response cost requires its own enabled Slash-defense Dodge use.");
        _corePhysicalMovementFacadesOperations.MoveCard(card, _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(provider, card, source), alternative ? CardLocation.DrawPile : CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
    }

    internal void FinishSingleCardResponse(Card card, CardConversionSource? source)
    {
        if (IsProgramTopDeckDodgeSource(source))
        {
            if (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
                throw new InvalidOperationException("A top-deck Dodge cost was incorrectly retained in Processing.");
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
    }

    internal void FinishSingleBasicCardUseCost(CardUseFrame use, Card card)
    {
        if (_coreExtraCardEffectRoundsOperations.HasUnstartedExtraCardEffect(use.Id))
            return;
        if (card.Id == 0 && card.Kind == CardKind.Alcohol && (_characterDongZhuoMouOlOperations.IsMouDongZhuoOlWine(use.Id) || _coreShanheSourceMouDongZhuoWineOperations.IsShanheSourceMouWine(use.Id)))
            return;
        // A copied Zhijue card has no material; its ID zero must not move a real card.
        if (card.Id == 0 && card.Kind == use.CardKind && _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(use.Id))
            return;
        if (card.Id == 0 && use.CardKind == CardKind.Alcohol && _characterGuoTuOlOperations.IsIssuedWeijieUse(use))
            return;
        if (_characterKongShuOlOperations.TryFinishLeiluanAlcoholCost(use, card))
            return;
        if (_coreRoundDistinctBasicPlayOperations.TryFinishRoundDistinctBasicAlcoholCost(use, card))
            return;
        if (_characterZhaoTongZhaoGuangAnniversaryOperations.TryFinishAnniversaryYizanAlcoholCost(use, card))
            return;
        if (_characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuAlcoholCost(use, card))
            return;
        if (_characterJuShouMouOperations.FinishMouJsAlcoholCost(use, card) || _characterDaQiaoXiaoQiaoHappyGodOperations.FinishHappyQiaoAlcoholCost(use, card))
            return;
        if (_coreTieredRoundZeroReturnsOperations.SkipTieredRoundZeroFinishedMovement(use.Id, card))
            return;
        if (_coreChainedStateBasicReturnsOperations.SkipChainedStateBasicFinishedMovement(use.Id, card))
            return;
        if (_coreDrawFundedDistinctBasicReturnsOperations.SkipDrawFundedDistinctBasicFinishedMovement(use.Id, card))
            return;
        if (_coreGuanduEquipmentOperations.IsGuanduDestroyedChariotMaterial(use.Id, card.Id))
            return;
        // The original native use already paid this generated field material
        // through its producer's exact destruction receipt. It never reached
        // Processing and cannot be discarded or resurrected at completion.
        if (card.Id > 0 && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial3(use.Id, card.Id))
            return;
        if (_characterHuanShuJunOlOperations.SkipHuanpeiFinishedMovement(use.Id, card) || _characterDiaoChanLeOperations.SkipDiouFinishedMovement(use.Id, card) || _characterXiaoQiaoMouOlOperations.SkipMouXqJueyanFinishedMovement(use.Id, card) || _characterLiuChengMouMobileOperations.SkipMouLiuChengFinishedMovement(use.Id, card))
            return;
        if (IsProgramAlternativeCost(use.Action, card.Id))
        {
            if (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
                throw new InvalidOperationException("A top-deck basic-card cost was incorrectly retained in Processing.");
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
    }

    internal CardLocation NormalizeProgramViewAsCostDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
    {
        if (to != CardLocation.Processing || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain))
            return to;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Action?.PhysicalCards.Any(cost => cost.CardId == card.Id) == true);
        return IsProgramAlternativeCost(use?.Action, card.Id) && use!.Action!.PhysicalCards.Single(cost => cost.CardId == card.Id).From == from ? CardLocation.DrawPile : to;
    }

    internal bool TryValidateProgramAlternativeCostAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing, out bool valid)
    {
        valid = false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == attack.ResolutionId);
        if (!attack.PhysicalCards.Any(card => IsProgramAlternativeCost(use?.Action, card.Id)))
            return false;
        var costs = attack.PhysicalCards.Where(card => IsProgramAlternativeCost(use!.Action, card.Id)).Select(card => card.Id).ToHashSet();
        // The spent cost can already have been drawn by a nested skill. Its
        // declared provenance remains attached to the original use regardless.
        if (costs.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
            return true;
        var held = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(frame => frame.Id != attack.ResolutionId).SelectMany(frame => frame.Action?.PhysicalCards ?? []).Select(cost => cost.CardId).ToHashSet();
        var expected = attack.PhysicalCards.Where(card => !costs.Contains(card.Id)).Select(card => card.Id).ToHashSet();
        var actual = processing.Where(card => !held.Contains(card.Id)).Select(card => card.Id).ToHashSet();
        valid = actual.SetEquals(expected);
        return true;
    }
}
