#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 34000)]
    internal IReadOnlyList<Card> PublicRevealInvariantProcessing(IReadOnlyList<Card> cards)
    {
        var suspended = _resolutionStackOperations._resolutionStack.Where(f => MouXiaoQiaoOl.PublicRevealSuspends(f.Id)).SelectMany(f => f switch
        {
            CardUseFrame u => u.Action?.PhysicalCards.Select(c => c.CardId) ?? u.PhysicalCardIds ?? [],
            ProgramSkillFrame p => p.CardAttack?.PhysicalCardIds ?? [],
            JudgmentFrame j => j.CardAttack?.PhysicalCardIds ?? [],
            _ => Enumerable.Empty<int>()}).ToHashSet();
        return suspended.Count == 0 ? cards : Array.AsReadOnly(cards.Where(c => !suspended.Contains(c.Id)).ToArray());
    }

    // These adapters return to the producer's existing native stage. They do not
    // repeat its reveal, accepted choice, payment, or first-suit claim.
    [PublicHandRevealReturn("qixi-guessed-hand")]
    internal void ReturnQixiPublicHand(ProgramSkillFrame f)
    {
        if (f.MouGanNing is not { Stage: MouGanNingStage.Discard })
            throw new InvalidOperationException("Qixi lost its accepted correct-guess reveal.");
        _characterGanNingMouMobileOperations.PublishMouGn(f);
    }

    [PublicHandRevealReturn("phase-name-hand")]
    internal void ReturnPhasePredictionPublicHand(ProgramSkillFrame f)
    {
        if (f.PhaseNamePrediction is not { Stage: PhaseNamePredictionStage.Guessing, RevealedCardId: not null })
            throw new InvalidOperationException("A name prediction lost its accepted blind reveal.");
        _corePhaseNamePredictionOperations.PublishPredictionChoice(f);
    }

    [PublicHandRevealReturn("xiashu-selected-hand")]
    internal void ReturnXiashuPublicHand(ProgramSkillFrame f)
    {
        if (f.XiashuPaid is not { Stage: XiashuStage.OwnerClaim })
            throw new InvalidOperationException("Xiashu lost its frozen recipient Hand reveal.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.XiashuPaid.Invoice.RecipientSeat].IsAlive)
            _characterKanZeOlOperations.FinishXiashu(f);
        else
            _characterKanZeOlOperations.PublishXiashuDecision(f);
    }

    [PublicHandRevealReturn("qingliang-hand")]
    internal void ReturnQingliangPublicHand(ProgramSkillFrame f)
    {
        if (f.RuiJiQingliang is not { Stage: "option" })
            throw new InvalidOperationException("Qingliang lost its issued whole-Hand reveal.");
        _characterRuiJiOlOperations.PresentQingliangOptionPrompt(f);
    }

    [PublicHandRevealReturn("diou-retained-hand")]
    internal void ReturnDiouPublicHand(ProgramSkillFrame f)
    {
        if (f.Diou is not { Paid: true, Stage: DiouStage.UseChoice })
            throw new InvalidOperationException("Diou lost its paid retained-Hand reveal.");
        _characterDiaoChanLeOperations.PublishDiou(f);
    }

    [PublicHandRevealReturn("hezhong-direction-hand")]
    internal void ReturnHezhongPublicHand(ProgramSkillFrame f)
    {
        if (f.FeiYi is not { Paid: true, Greater: not null, Stage: FeiYiStage.DirectionChoice })
            throw new InvalidOperationException("Hezhong lost its accepted direction and revealed card.");
        _characterFeiYiOlOperations.BeginFeiYiDraw(f, true);
    }

    [PublicHandRevealReturn("hand-comparison-source")]
    internal void ReturnHandComparisonSource(ProgramSkillFrame f)
    {
        if (f.HandComparisonDraft is not { Stage: "reveal" })
            throw new InvalidOperationException("A Hand comparison lost its frozen source and target roster.");
        _coreHandComparisonOperations.PublishHandComparison(f);
    }

    [PublicHandRevealReturn("fire-attack-retained-payment")]
    internal void ReturnFireAttackRetainedPublicHand(CardUseFrame pending)
    {
        var r = pending.PublicHandRevealSelection ?? throw new InvalidOperationException("FireAttack lost its retained payment reveal.");
        if (r.ReturnKey != "fire-attack-retained-payment" || r.ActionId != pending.Action?.ActionId || r.ActorSeat != pending.SourceSeat || pending.CardKind != CardKind.FireAttack)
            throw new InvalidOperationException("FireAttack changed its original accepted source and retained card.");
        _coreRuntimeOperations.ReplaceRuntimeTop(pending = pending with { PublicHandRevealSelection = null });
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending)];
        var revealed = _coreAdvancedLifecycleOperations.GetAdvancedCard(pending.FireAttackSelection!.RevealedCardId!.Value);
        var matchingDiscard = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
        var revealInstead = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(pending.Id, source.Seat, target.Seat, revealed.Id, revealed.Suit, matchingDiscard.Id, CausedDamage: true));
        _coreMatchLogOperations.AddLog("CardEffect", $"{(source.Name)} {((revealInstead ? "展示" : "弃置"))}【{(matchingDiscard.DisplayName)}】，{(target.Name)} 受到 1 点火焰伤害。", source.Seat, target.Seat);
        var attackCard = _coreNativeFireAttackOperations.GetFireAttackEffectCard(pending);
        _coreNativeFireAttackOperations.SetFireAttackSelection(pending.Id, null);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.Id, source.Seat, target.Seat, (_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(pending.Id) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroFireAttackUse(pending.Id) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(pending.Id) || _characterDiaoChanLeOperations.IsDiouZeroUse(pending.Id) || IsMouXqJueyanUse(pending.Id) || _characterQinLangOlOperations.IsQinLangXianyingUse(pending.Id)) ? null : attackCard, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }
}
