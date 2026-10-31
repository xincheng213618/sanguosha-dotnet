#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseGiftBasicRuntime
{
    internal const string PaidPhaseGiftUsage = "paid-phase-gift-count";
    internal void CapturePaidPhaseGift(long frameId, SkillProgramActivation activation, bool toDiscard)
    {
        if (toDiscard || !ProgramInstructionResolver.Default.Features(activation).HasOperation(SkillProgramEffectOp.AccumulatePaidPhaseGift))
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var payment = frame.SelectedCardPayment ?? throw new InvalidOperationException("A phase gift requires an owning physical payment.");
        if (!payment.MovementCommitted || payment.Operation != SkillProgramEffectOp.GiveSelected || frame.PhaseGiftReceipt is not null)
            throw new InvalidOperationException("A phase gift cannot be counted before payment or twice.");
        var actualIds = payment.CardIds;
        if (payment.GainRequestSettlement is { } settlement)
        {
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, frame.Id, payment.CardIds, CardLocation.Hand(payment.RecipientSeat), settlement.SequenceBefore);
            actualIds = settlement.Movements.Where(move => move.To == CardLocation.Hand(payment.RecipientSeat)).Select(move => move.CardId).ToArray();
        }

        var before = _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, PaidPhaseGiftUsage, SkillUsageScope.Phase);
        foreach (var cardId in actualIds)
        {
            if (_cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(payment.RecipientSeat))
                throw new InvalidOperationException("The committed gift entity lost its recipient before children.");
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, PaidPhaseGiftUsage, SkillUsageScope.Phase, int.MaxValue))
                throw new InvalidOperationException("The actual phase gift count could not advance.");
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PhaseGiftReceipt = new(payment.InstructionIndex, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, before, actualIds.Count) });
        if (actualIds.Count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(frame.OwnerSeat, frame.SkillId, PaidPhaseGiftUsage, SkillUsageScope.Phase, actualIds.Count));
    }

    internal void AccumulatePaidPhaseGift(ProgramSkillFrame frame, string resultBind, int threshold)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = active.PhaseGiftReceipt ?? throw new InvalidOperationException("An actual phase gift receipt is required.");
        if (receipt.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || active.SelectedCardPaymentResult is not { Completed: true } result || result.InstructionIndex != receipt.InstructionIndex || active.ChoiceBindings.Any(b => b.Name == resultBind))
            throw new InvalidOperationException("A phase gift lost its completed exact payment or result binding.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { ChoiceBindings = Array.AsReadOnly(active.ChoiceBindings.Append(new ProgramChoiceResultBinding(resultBind, receipt.PreviousCount < threshold && receipt.PreviousCount + receipt.ActualCount >= threshold ? "crossed" : "not-crossed", active.OwnerSeat)).ToArray()) });
    }

    void IPhaseGiftBasicProgramHost.AccumulatePaidPhaseGift(ProgramSkillFrame frame, string resultBind, int threshold) => AccumulatePaidPhaseGift(frame, resultBind, threshold);
    SkillProgramStepOutcome IPhaseGiftBasicProgramHost.OfferVirtualBasicCard(ProgramSkillFrame frame) => _coreVirtualBasicOffersOperations.OfferVirtualBasicCard(frame);
}
