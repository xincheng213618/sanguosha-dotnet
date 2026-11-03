#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRevealedFireAttackPaymentRuntime
{
    internal bool FireAttackPaymentIsReveal(int seat) => _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, 2004) || _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.RevealFireAttackPayment, CardKind.FireAttack);
    [PublicHandRevealReturn("native-fire-attack-payment")]
    internal void ContinueRevealedFireAttackPayment(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame current || current.Id != use.Id || current.FireAttackSelection is not { RevealedCardId: { } targetCardId, RevealedPaymentCardId: { } paymentId } || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardsRevealedEvent>().Any(fact => fact.ResolutionId == use.Id && fact.Cards.Any(card => card.Id == paymentId)))
            throw new InvalidOperationException("The revealed Fire Attack payment lost its original card use and public entity.");
        CompleteNativeFireAttackPayment(current, _coreAdvancedLifecycleOperations.GetAdvancedCard(targetCardId), _coreAdvancedLifecycleOperations.GetAdvancedCard(paymentId), revealInstead: true);
    }

    internal void CompleteNativeFireAttackPayment(CardUseFrame pending, Card revealed, Card payment, bool revealInstead)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending)];
        var canDamage = _coreMatchOutcomeWinnerAccess._winner == Winner.None && source.IsAlive && target.IsAlive;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, source.Seat, target.Seat, revealed.Id, revealed.Suit, payment.Id, CausedDamage: canDamage));
        var attackCard = _coreNativeFireAttackOperations.GetFireAttackEffectCard(pending);
        _coreNativeFireAttackOperations.SetFireAttackSelection(pending.Id, null);
        if (!canDamage)
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.Id, attackCard);
            _coreNativeCardUseCompletionOperations.FinishCardUse(pending.Id, attackCard, pending.CardKind);
            return;
        }

        _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} {((revealInstead ? "展示" : "弃置"))}【{(payment.DisplayName)}】，{(target.Name)} 受到 1 点火焰伤害。", source.Seat, target.Seat);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.Id, source.Seat, target.Seat, (_characterDongChengMouOperations.IsMouDcZeroUse(pending.Id) || _characterZhouYuMouOperations.IsMouZyZeroFireUse(pending.Id) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(pending.Id) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroFireAttackUse(pending.Id) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(pending.Id) || _characterDiaoChanLeOperations.IsDiouZeroUse(pending.Id) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(pending.Id) || _characterQinLangOlOperations.IsQinLangXianyingUse(pending.Id)) ? null : attackCard, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal bool RefreshRevealedFireAttackPaymentPrompt()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 0 } || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not { SourceSeat: 0, FireAttackSelection: { RevealedCardId: { } revealed, RevealedPaymentCardId: null } } use)
            return false;
        _coreNativeFireAttackOperations.BeginFireAttackDiscard(use, _coreAdvancedLifecycleOperations.GetAdvancedCard(revealed));
        return true;
    }

    [PublicHandRevealReturn("fire-attack-retained-payment")]
    internal void ReturnFireAttackRetainedPublicHand(CardUseFrame pending)
    {
        var r = pending.PublicHandRevealSelection ?? throw new InvalidOperationException("FireAttack lost its retained payment reveal.");
        if (r.ReturnKey != "fire-attack-retained-payment" || r.ActionId != pending.Action?.ActionId || r.ActorSeat != pending.SourceSeat || pending.CardKind != CardKind.FireAttack)
            throw new InvalidOperationException("FireAttack changed its original accepted source and retained card.");
        _publicHandRevealFrames.ReplaceTop(pending = pending with { PublicHandRevealSelection = null });
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending)];
        var revealed = _coreAdvancedLifecycleOperations.GetAdvancedCard(pending.FireAttackSelection!.RevealedCardId!.Value);
        var matchingDiscard = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
        var revealInstead = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, source.Seat, target.Seat, revealed.Id, revealed.Suit, matchingDiscard.Id, CausedDamage: true));
        _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} {((revealInstead ? "展示" : "弃置"))}【{(matchingDiscard.DisplayName)}】，{(target.Name)} 受到 1 点火焰伤害。", source.Seat, target.Seat);
        var attackCard = _coreNativeFireAttackOperations.GetFireAttackEffectCard(pending);
        _coreNativeFireAttackOperations.SetFireAttackSelection(pending.Id, null);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.Id, source.Seat, target.Seat, (_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(pending.Id) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroFireAttackUse(pending.Id) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(pending.Id) || _characterDiaoChanLeOperations.IsDiouZeroUse(pending.Id) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(pending.Id) || _characterQinLangOlOperations.IsQinLangXianyingUse(pending.Id)) ? null : attackCard, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }

}
