#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardUseDebitsRuntime
{
    internal void ResetCardUseDebitPhase()
    {
        // An inserted Play can start inside the previous PlayEnding window.
        // Its skill sources must expire before a new phase identity is issued.
        _corePlayPhaseSkillGrantsOperations.CloseCurrentPlayPhaseSkillGrants("next-play-phase-started");
        _characterCaoChunOlOperations.ExpireCaoChunPlayDebt(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId);
        _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId = checked(_coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId + 1);
        _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits.Clear();
        _coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Clear();
        _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._refundedCardUseDebits.Clear();
    }

    internal void RecordSlashUseDebit(long cardUseFrameId, int actorSeat)
    {
        if (_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsSlashDebit(ProgramCardUsePaymentStage.BeforeSlashDebit, cardUseFrameId, actorSeat))
            return;
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == cardUseFrameId).Action ?? throw new InvalidOperationException("A counted Slash has no frozen card-action identity.");
        var identity = new CardUseDebitIdentity(action.ActionId, actorSeat, SkillRuleQuery.SlashLimit, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebitPhaseInstanceId);
        if (_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsSlashDebit(ProgramCardUsePaymentStage.AfterSlashDebitIdentity, cardUseFrameId, actorSeat))
            return;
        var suit = action.EffectiveSuit;
        if (suit is null && action.PhysicalCards.Count == 1)
        {
            var cost = action.PhysicalCards[0];
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cost.CardId)).Single(item => item.Id == cost.CardId);
            suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actorSeat], card);
        }

        if (_coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(actorSeat, suit) || _coreBoundDiscardSlashBenefitsOperations.HasTurnSlashSuitAllowance(actorSeat, suit))
            return;
        if (suit is { } usedSuit && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[actorSeat], SkillProgramCardPolicyKind.BypassSlashLimitBySuit, action.EffectiveKind).Any(item => item.Policy.InputSuit == usedSuit))
            return;
        if (!_coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits.TryAdd(action.ActionId, identity))
            throw new InvalidOperationException("A card action cannot debit its Slash quota twice.");
        _coreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDebitRecordedEvent(identity));
    }

    internal bool TryRefundCardUseDebit(CardUseDebitIdentity identity)
    {
        if (!_coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits.TryGetValue(identity.CardActionId, out var recorded) || recorded != identity)
            return false;
        if (!_coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._refundedCardUseDebits.Add(identity.CardActionId))
            return false;
        if (_coreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn <= 0)
            throw new InvalidOperationException("A recorded Slash debit cannot be refunded from an empty quota.");
        _coreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn--;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDebitRefundedEvent(identity));
        return true;
    }

    internal CardUseDebitIdentity? GetCardUseDebit(long actionId) => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits.GetValueOrDefault(actionId);
    internal bool IsCardUseDebitActive(long actionId) => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._cardUseDebits.ContainsKey(actionId) && !_coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess._refundedCardUseDebits.Contains(actionId);
    internal void RefundProgramCardUseDebit(ProgramSkillFrame frame)
    {
        var context = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).WindowContext?.CardUse ?? throw new InvalidOperationException("Card-use refund requires a frozen card-action context.");
        if (!context.WasUsageDebited || context.DebitIdentity is not { } identity)
            return;
        _ = TryRefundCardUseDebit(identity);
    }
}
