namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool FireAttackPaymentIsReveal(int seat) => HasWushuangTactic(seat, 2004) ||
        HasCardPolicy(_players[seat], SkillProgramCardPolicyKind.RevealFireAttackPayment, CardKind.FireAttack);

    [PublicHandRevealReturn("native-fire-attack-payment")]
    private void ContinueRevealedFireAttackPayment(CardUseFrame use)
    {
        if (_resolutionStack.LastOrDefault() is not CardUseFrame current || current.Id != use.Id ||
            current.FireAttackSelection is not { RevealedCardId: { } targetCardId, RevealedPaymentCardId: { } paymentId } ||
            !ProgramEventHistory<CardsRevealedEvent>().Any(fact => fact.ResolutionId == use.Id &&
                fact.Cards.Any(card => card.Id == paymentId)))
            throw new InvalidOperationException("The revealed Fire Attack payment lost its original card use and public entity.");
        CompleteNativeFireAttackPayment(current, GetAdvancedCard(targetCardId), GetAdvancedCard(paymentId), revealInstead: true);
    }

    private void CompleteNativeFireAttackPayment(CardUseFrame pending, Card revealed, Card payment, bool revealInstead)
    {
        var source = _players[pending.SourceSeat];
        var target = _players[GetFireAttackTargetSeat(pending)];
        var canDamage = _winner == Winner.None && source.IsAlive && target.IsAlive;
        AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, source.Seat, target.Seat,
            revealed.Id, revealed.Suit, payment.Id, CausedDamage: canDamage));
        var attackCard = GetFireAttackEffectCard(pending);
        SetFireAttackSelection(pending.Id, null);
        if (!canDamage)
        {
            MoveFinishedTrickCard(pending.Id, attackCard);
            FinishCardUse(pending.Id, attackCard, pending.CardKind);
            return;
        }
        AddLog("CardEffect", $"{source.Name} {(revealInstead ? "展示" : "弃置")}【{payment.DisplayName}】，{target.Name} 受到 1 点火焰伤害。",
            source.Seat, target.Seat);
        var attack = new CardAttackHandle(this, pending.Id, source.Seat, target.Seat,
            (IsMouDcZeroUse(pending.Id) || IsMouZyZeroFireUse(pending.Id) || IsProgramVirtualOrdinaryTrickUse(pending.Id) ||
             IsTieredRoundZeroFireAttackUse(pending.Id) || IsMouZhijueZeroUse(pending.Id) || IsDiouZeroUse(pending.Id) ||
             IsMouXqJueyanUse(pending.Id) || IsQinLangXianyingUse(pending.Id)) ? null : attackCard,
            damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: GetCardUsePhysicalCards(pending.Id));
        ActiveCardAttack = attack;
        if (!ApplyAttackDamage(attack)) CompleteAttack(attack);
    }

    private bool RefreshRevealedFireAttackPaymentPrompt()
    {
        if (_pendingDecision is not { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 0 } ||
            ActiveFireAttack is not { SourceSeat: 0, FireAttackSelection: { RevealedCardId: { } revealed, RevealedPaymentCardId: null } } use)
            return false;
        BeginFireAttackDiscard(use, GetAdvancedCard(revealed));
        return true;
    }
}
