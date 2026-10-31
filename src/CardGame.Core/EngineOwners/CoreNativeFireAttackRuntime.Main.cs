#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFireAttackRuntime
{
    internal void BeginFireAttackReveal(CardUseFrame pending)
    {
        if (_coreColorFireAttacksOperations.TryBeginColorFireAttackReveal(pending))
            return;
        if (!ReferenceEquals(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack, pending) || pending.FireAttackSelection?.RevealedCardId is not null)
        {
            throw new InvalidOperationException("The FireAttack reveal is not the current resolution.");
        }

        var target = _coreMatchParticipantsPlayersAccess._players[GetFireAttackTargetSeat(pending)];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
        if (hand.Count == 0)
        {
            throw new InvalidOperationException("FireAttack cannot request a reveal from an empty hand.");
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.FireAttackReveal, target.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].Name)} 对你使用了【火攻】，请选择一张手牌展示。", hand.Select(card => card.Id).ToArray(), [], pending.SourceSeat, pending.CardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = hand.Select(card => new PromptChoice(new ChoiceId($"fire-attack.reveal.card-{(card.Id)}"), $"展示【{(card.DisplayName)}】（{(card.RankText)}）", [card.Id], [], new Dictionary<string, string> { ["response"] = "fire-attack-reveal", ["action"] = "reveal-card" })).ToArray(),
            TargetSeat = target.Seat
        };
        _coreMatchOutcomeStatusAccess._status = target.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    internal void ResolveFireAttackReveal(CardUseFrame pending, int cardId)
    {
        if (!ReferenceEquals(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack, pending) || pending.FireAttackSelection?.RevealedCardId is not null)
        {
            throw new InvalidOperationException("The FireAttack reveal is not the current resolution.");
        }

        var target = _coreMatchParticipantsPlayersAccess._players[GetFireAttackTargetSeat(pending)];
        var revealed = _corePhysicalOwnedCardQueriesOperations.GetHand(target).SingleOrDefault(card => card.Id == cardId) ?? throw new InvalidOperationException("The selected FireAttack reveal card is not in the target hand.");
        pending = SetFireAttackSelection(pending.Id, new FireAttackSelectionState(revealed.Id));
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.Id, ResolutionFrameStep.AwaitingResponse);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackCardRevealedEvent(pending.Id, pending.SourceSeat, GetFireAttackTargetSeat(pending), revealed.Id, revealed.Kind, revealed.Suit));
        _coreMatchLogOperations.AddLog("CardRevealed", $"{(target.Name)} 展示了【{(revealed.DisplayName)}】（{(revealed.Suit)}），等待攻击者{((_coreRevealedFireAttackPaymentOperations.FireAttackPaymentIsReveal(pending.SourceSeat) ? "展示" : "弃置"))}同花色手牌。", target.Seat, pending.SourceSeat);
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(pending.Id, PublicHandRevealReturnKind.FireAttack))
            return;
        BeginFireAttackDiscard(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack ?? throw new InvalidOperationException("FireAttack lost its revealed selection."), revealed);
    }

    internal void BeginFireAttackDiscard(CardUseFrame pending, Card revealed)
    {
        if (_coreColorFireAttacksOperations.TryBeginColorFireAttackDiscard(pending, revealed))
            return;
        if (!ReferenceEquals(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack, pending) || pending.FireAttackSelection?.RevealedCardId != revealed.Id)
        {
            throw new InvalidOperationException("The FireAttack discard is not the current resolution.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var matchingCards = _corePhysicalOwnedCardQueriesOperations.GetHand(source).Where(card => card.Suit == revealed.Suit).Concat(_characterZhugeLiangMouOperations.HasMouOperation(source, SkillProgramEffectOp.MouZhitianView) ? _characterZhugeLiangMouOperations.MouZhitianTop(source.Seat).Where(card => card.Suit == revealed.Suit) : []).ToArray();
        if (matchingCards.Length == 0)
        {
            ResolveFireAttackDiscard(pending, selectedCardId: null);
            return;
        }

        var paymentVerb = _coreRevealedFireAttackPaymentOperations.FireAttackPaymentIsReveal(source.Seat) ? "展示" : "弃置";
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.FireAttackDiscard, source.Seat, $"目标展示了【{(revealed.DisplayName)}】（{(revealed.Suit)}），请选择一张同花色手牌{(paymentVerb)}，或放弃造成火焰伤害。", matchingCards.Select(card => card.Id).ToArray(), [], source.Seat, pending.CardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = matchingCards.Select(card => new PromptChoice(new ChoiceId($"fire-attack.discard.card-{(card.Id)}"), $"{(paymentVerb)}【{(card.DisplayName)}】（{(card.RankText)}），造成 1 点火焰伤害。", [card.Id], [], new Dictionary<string, string> { ["response"] = "fire-attack-discard", ["action"] = "discard-same-suit" })).Append(new PromptChoice(new ChoiceId("fire-attack.skip"), $"不{(paymentVerb)}同花色牌，火攻不造成伤害。", [], [], new Dictionary<string, string> { ["response"] = "fire-attack-skip", ["action"] = "skip-fire-attack" })).ToArray(),
            TargetSeat = GetFireAttackTargetSeat(pending)
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    internal void ResolveFireAttackDiscard(CardUseFrame pending, int? selectedCardId)
    {
        if (_characterZhugeLiangMouOperations.TryResolveMouZhitianFireDiscard(pending, selectedCardId))
            return;
        if (_coreColorFireAttacksOperations.TryResolveColorFireAttackDiscard(pending, selectedCardId))
            return;
        if (!ReferenceEquals(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack, pending) || pending.FireAttackSelection?.RevealedCardId is null)
        {
            throw new InvalidOperationException("The FireAttack discard is not the current resolution.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[GetFireAttackTargetSeat(pending)];
        var revealed = GetFireAttackRevealedCard(pending);
        var matchingDiscard = selectedCardId is { } cardId ? _corePhysicalOwnedCardQueriesOperations.GetHand(source).SingleOrDefault(card => card.Id == cardId) : null;
        if (selectedCardId is not null && (matchingDiscard is null || matchingDiscard.Suit != revealed.Suit))
        {
            throw new InvalidOperationException("FireAttack requires a same-suit source discard card.");
        }

        if (matchingDiscard is null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, source.Seat, target.Seat, revealed.Id, revealed.Suit, MatchingDiscardCardId: null, CausedDamage: false));
            _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} 未{((_coreRevealedFireAttackPaymentOperations.FireAttackPaymentIsReveal(source.Seat) ? "展示" : "弃置"))}同花色牌，{(target.Name)} 的【火攻】未造成伤害。", source.Seat, target.Seat);
            var effectCard = GetFireAttackEffectCard(pending);
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.Id, effectCard);
            SetFireAttackSelection(pending.Id, null);
            _coreNativeCardUseCompletionOperations.FinishCardUse(pending.Id, effectCard, pending.CardKind);
            return;
        }

        var revealInstead = _coreRevealedFireAttackPaymentOperations.FireAttackPaymentIsReveal(source.Seat);
        if (revealInstead)
        {
            pending = SetFireAttackSelection(pending.Id, pending.FireAttackSelection!with { RevealedPaymentCardId = matchingDiscard.Id });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(pending.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(matchingDiscard)]));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(pending.Id, "native-fire-attack-payment"))
                return;
            _coreRevealedFireAttackPaymentOperations.ContinueRevealedFireAttackPayment(pending);
            return;
        }

        var poisonPaymentSequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(source.Seat, source.Seat, [matchingDiscard.Id], CardMoveReasons.FireAttackDiscard, pending.Id);
        _corePhysicalMovementFacadesOperations.MoveCard(matchingDiscard, CardLocation.Hand(source.Seat), CardLocation.Processing, CardMoveReasons.FireAttackDiscard, disclosure: CardMovementDisclosure.FaceUp);
        _corePhysicalMovementFacadesOperations.MoveCard(matchingDiscard, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.FireAttackDiscardFinished);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (_coreWenhePoisonOperations.TryBeginWenhePoisonFireAttackPayment(pending, revealed, matchingDiscard, poisonPaymentSequenceBefore))
            return;
        _coreRevealedFireAttackPaymentOperations.CompleteNativeFireAttackPayment(pending, revealed, matchingDiscard, revealInstead: false);
    }

    internal Card GetFireAttackRevealedCard(CardUseFrame pending)
    {
        var revealedCardId = pending.FireAttackSelection?.RevealedCardId ?? throw new InvalidOperationException("FireAttack has no revealed card.");
        var location = CardLocation.Hand(GetFireAttackTargetSeat(pending));
        return _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == revealedCardId);
    }

    internal static int GetFireAttackTargetSeat(CardUseFrame pending) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.GetFireAttackTargetSeat(pending);
    internal Card GetFireAttackEffectCard(CardUseFrame pending) => _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(pending.Id, pending.CardId, requireProcessing: true);
    internal CardUseFrame SetFireAttackSelection(long frameId, FireAttackSelectionState? selection)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame cardUse || cardUse.Id != frameId || global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != CardKind.FireAttack || (selection is null && cardUse.FireAttackSelection is null) || (selection is not null && cardUse.TargetIndex >= cardUse.TargetSeats.Count))
        {
            throw new InvalidOperationException("FireAttack selection lost its current CardUse frame.");
        }

        var next = cardUse with
        {
            FireAttackSelection = selection
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(next);
        return next;
    }
}
