#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUnexpectedAssaultRuntime
{
    internal void ResolveUnexpectedAssault(CharacterState source, CharacterState target, Card card, CardKind? playedCardKind = null)
    {
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: card.Id).Any(action => action.Kind == LegalActionKind.UnexpectedAssault && action.CardId == card.Id && action.TargetSeat == target.Seat && action.PlayedCardKind == playedCardKind))
            throw new InvalidOperationException("Unexpected Assault became illegal before its real use.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [target.Seat], playedCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, source.Seat, [target.Seat], LegalActionKind.UnexpectedAssault, playedCardKind: playedCardKind);
    }

    internal void ResolveUnexpectedAssaultEffect(NullificationWindowFrame pending)
    {
        var targetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Unexpected Assault lost its current target.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || use.CardId != pending.EffectCardId || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.UnexpectedAssault || pending.EffectCardKind != global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || pending.ActionKind != LegalActionKind.UnexpectedAssault || use.SourceSeat != pending.SourceSeat || use.TargetIndex >= global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != targetSeat || use.Action is not { Type: CardActionType.Use, EffectiveSuit: not null })
            throw new InvalidOperationException("Unexpected Assault lost its exact owning native use and suit.");
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0)
        {
            _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.TargetHandEmpty);
            return;
        }

        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(use.CardId, global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), use.SourceSeat, targetSeat));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].Name)} 对 {(target.Name)} 使用【出其不意】，选择展示其一张手牌。", use.SourceSeat, targetSeat);
        _coreNativeTrickEffectsOperations.BeginTargetCardSelection(pending, target, _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count);
    }

    internal bool TryApplyUnexpectedAssaultTargetCardEffect(long resolutionId, Card effectCard, CardKind effectiveCardKind, int sourceSeat, int targetSeat, LegalActionKind actionKind, Card targetCard, CardLocation targetCardFromZone)
    {
        if (effectiveCardKind != CardKind.UnexpectedAssault && actionKind != LegalActionKind.UnexpectedAssault)
            return false;
        if (effectiveCardKind != CardKind.UnexpectedAssault || actionKind != LegalActionKind.UnexpectedAssault || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != resolutionId || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != effectiveCardKind || use.CardId != effectCard.Id || use.SourceSeat != sourceSeat || use.Step != ResolutionFrameStep.ResolvingEffect || use.TargetIndex >= global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != targetSeat || targetCardFromZone != CardLocation.Hand(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Any(card => card.Id == targetCard.Id) || use.UnexpectedAssaultReveal?.TargetIndex == use.TargetIndex || use.Action is not { Type: CardActionType.Use, EffectiveSuit: { } useSuit } action || action.ActorSeat != sourceSeat)
            throw new InvalidOperationException("Unexpected Assault reveal lost its exact current hand, action or target cursor.");
        var receipt = new UnexpectedAssaultRevealReceipt(use.Id, action.ActionId, use.TargetIndex, sourceSeat, targetSeat, targetCard.Id, targetCard.Kind, targetCard.Suit, targetCard.Rank, useSuit, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[targetSeat], targetCard), _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(use.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { UnexpectedAssaultReveal = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(use.Id, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(targetCard) })));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnexpectedAssaultRevealedEvent(receipt));
        _coreMatchLogOperations.AddLog("CardRevealed", $"{(_coreMatchParticipantsPlayersAccess._players[sourceSeat].Name)} 展示了 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 的【{(targetCard.DisplayName)}】（{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(receipt.EffectiveRevealedSuit))}）。", sourceSeat, targetSeat);
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(use.Id, "unexpected-assault-hand"))
            return true;
        ContinueUnexpectedAssaultPublicReveal(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!);
        return true;
    }

    internal void AssertUnexpectedAssaultReceipts()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(frame => frame.UnexpectedAssaultReveal is not null))
        {
            var receipt = use.UnexpectedAssaultReveal!;
            if (global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.UnexpectedAssault || receipt.CardUseFrameId != use.Id || use.Action is not { Type: CardActionType.Use } action || action.ActionId != receipt.ActionId || action.ActorSeat != use.SourceSeat || action.EffectiveSuit != receipt.EffectiveUseSuit || receipt.SourceSeat != use.SourceSeat || receipt.TargetIndex < 0 || receipt.TargetIndex >= global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || receipt.TargetIndex > use.TargetIndex || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[receipt.TargetIndex] != receipt.TargetSeat || receipt.RevealedCardId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.TargetSeat) || !Enum.IsDefined(receipt.EffectiveUseSuit) || !Enum.IsDefined(receipt.EffectiveRevealedSuit) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnexpectedAssaultRevealedEvent>().Count(e => e.Receipt == receipt) != 1 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardsRevealedEvent>().Any(e => e.ResolutionId == use.Id && e.Cards is [var card] && card.Id == receipt.RevealedCardId && card.Kind == receipt.RevealedCardKind && card.Suit == receipt.RevealedPrintedSuit && card.Rank == receipt.RevealedRank))
                throw new InvalidOperationException("Unexpected Assault lost its one actual public reveal and frozen scalar comparison.");
        }
    }

    [PublicHandRevealReturn("unexpected-assault-hand")]
    internal void ContinueUnexpectedAssaultPublicReveal(CardUseFrame use)
    {
        var receipt = use.UnexpectedAssaultReveal ?? throw new InvalidOperationException("Unexpected Assault lost its selected Hand reveal.");
        if (receipt.CardUseFrameId != use.Id || receipt.ActionId != use.Action?.ActionId || receipt.TargetIndex != use.TargetIndex)
            throw new InvalidOperationException("Unexpected Assault changed its original native cursor.");
        var sourceSeat = receipt.SourceSeat;
        var targetSeat = receipt.TargetSeat;
        var effectiveCardKind = CardKind.UnexpectedAssault;
        var effectCard = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true);
        if (!receipt.CanCauseDamage)
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(use.Id, effectCard);
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, effectCard, effectiveCardKind);
            return;
        }

        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, sourceSeat, targetSeat, effectCard, playedCardKind: effectiveCardKind, damageNatureOverride: DamageNature.Normal, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(use.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return;
    }
}
