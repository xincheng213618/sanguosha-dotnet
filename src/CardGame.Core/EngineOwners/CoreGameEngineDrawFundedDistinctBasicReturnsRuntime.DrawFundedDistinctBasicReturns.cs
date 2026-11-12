#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsRuntime
{
    internal bool SkipDrawFundedDistinctBasicFinishedMovement(long frameId, Card card)
    {
        if (card.Id != 0 || !_coreDrawFundedDistinctBasicUsesOperations.IsDrawFundedDistinctBasicUse(frameId))
            return false;
        if (card.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!.CardKind)
            throw new InvalidOperationException("A paid logical basic card changed its finished name.");
        return true;
    }

    [NativeUseValidation(20)]
    internal void AssertDrawFundedDistinctBasicUseCompletion(CardUseFrame use)
    {
        if (use.DrawFundedDistinctBasicUse is null && use.DrawFundedDistinctBasicResponse is null)
            return;
        if (use.DrawFundedDistinctBasicUse is not { } receipt || use.DrawFundedDistinctBasicResponse is not null || !_coreDrawFundedDistinctBasicUsesOperations.IsIssuedDrawFundedDistinctBasicUse(use) || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.ParentFrameId == use.Id))
            throw new InvalidOperationException("A draw-funded Use cannot return before its exact native completion children.");
        var p = receipt.Payment;
        if (p.Intent is DrawFundedDistinctBasicIntent.ProgramSlash or DrawFundedDistinctBasicIntent.ProgramNearestSlash or DrawFundedDistinctBasicIntent.AssistedSlash or DrawFundedDistinctBasicIntent.NearestLegalSlash)
        {
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.ParentFrameId)is not ProgramSkillFrame parent || !DrawFundedDistinctBasicProgramActor(parent, p) || use.CardAttack?.ProgramSkillCardUseFrameId != parent.Id)
                throw new InvalidOperationException("A paid program Slash lost its original actor/instruction return.");
        }
    }

    internal bool DrawFundedDistinctBasicProgramActor(ProgramSkillFrame parent, DrawFundedDistinctBasicPayment payment)
    {
        return parent.Id == payment.ParentFrameId && parent.Id == payment.RequestFrameId && parent.InstructionIndex == payment.Cursor &&
            payment.RequestAcceptance is { } acceptance && _programSlashRequests.OwnsAcceptance(parent, acceptance, payment.Source.OwnerSeat, payment.TargetSeat);
    }

    internal bool IsDrawFundedDistinctBasicProgramSelection(ProgramSkillFrame parent, SkillProgramEffect effect, DrawFundedDistinctBasicFrame paid)
    {
        if (!_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(paid.Payment) || !DrawFundedDistinctBasicProgramActor(parent, paid.Payment) || paid.OriginalDecision.PlayerSeat != paid.Payment.Source.OwnerSeat || paid.OriginalDecision.TargetSeat != paid.Payment.Source.OwnerSeat || paid.OriginalDecision.PromptId != paid.Payment.OriginalPromptId || paid.OriginalDecision.Revision != paid.Payment.OriginalRevision)
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect;
        if (paused != effect)
            return false;
        if (paid.Payment.Intent != DrawFundedDistinctBasicIntent.ProgramNearestSlash)
            return true;
        var native = CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(paid.OriginalDecision));
        return native.Kind == DecisionKind.ProgramTrigger && native.Choices.Count > 0 && native.Choices.All(choice => choice.Parameters.GetValueOrDefault("frame-id") == parent.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) && choice.Parameters.GetValueOrDefault("seat") == paid.Payment.Source.OwnerSeat.ToString(System.Globalization.CultureInfo.InvariantCulture)) && native.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "request-slash-nearest-decline");
    }

    [NativeUseReturn(90)]
    internal void ReturnDrawFundedDistinctBasicUse(CardUseFrame completed)
    {
        if (completed.DrawFundedDistinctBasicUse is not { } receipt)
            return;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == completed.Id) || !_coreDrawFundedDistinctBasicUsesOperations.IsDrawFundedDistinctBasicIssued(receipt))
            throw new InvalidOperationException("A draw-funded Use return must follow the exact owning-frame completion.");
        EmitDrawFundedDistinctBasicReturned(receipt);
    }

    internal void EmitDrawFundedDistinctBasicReturned(DrawFundedDistinctBasicUseReceipt receipt)
    {
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicReturnedEvent>().Any(e => e.PaymentFrameId == receipt.Payment.PaymentFrameId))
            throw new InvalidOperationException("A paid basic Use returned more than once.");
        var p = receipt.Payment;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawFundedDistinctBasicReturnedEvent(p.PaymentFrameId, receipt.OwnerFrameId, receipt.CardActionId, p.Source.OwnerSeat, p.Intent, p.EffectiveKind));
    }

    internal bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, DyingFrame dying)
    {
        if (dyingIndex < 0 || dyingIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[dyingIndex] is not DyingFrame exact || exact != dying || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.FrameId != dying.Id)
            return false;
        if (_resolutionStackOperations._resolutionStack[dyingIndex + 1] is DrawFundedDistinctBasicFrame paid)
            return paid.Payment.Intent == DrawFundedDistinctBasicIntent.Dying && paid.Payment.ParentFrameId == dying.Id && paid.Payment.RequestFrameId == dying.Id && paid.Payment.Source.OwnerSeat == dying.ResponderSeat && paid.Payment.Cursor == dying.ResponderIndex && paid.Payment.TargetSeat == dying.VictimSeat && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(paid.Payment) && _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicPaidSuffix(dyingIndex + 1);
        if (_resolutionStackOperations._resolutionStack[dyingIndex + 1] is not CardUseFrame rescue || !_coreDrawFundedDistinctBasicUsesOperations.IsIssuedDrawFundedDistinctBasicUse(rescue) || rescue.DrawFundedDistinctBasicUse!.Payment is not { Intent: DrawFundedDistinctBasicIntent.Dying } p || p.ParentFrameId != dying.Id || p.Cursor != dying.ResponderIndex || rescue.DyingResponse is not { } token || token.ResolutionId != dying.Id || token.ResponderSeat != dying.ResponderSeat || rescue.SourceSeat != dying.ResponderSeat || rescue.CardKind is not (CardKind.Peach or CardKind.Alcohol) || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || !rescue.Action!.EffectiveDesignatedTargetSeats.SequenceEqual([dying.VictimSeat]) || (rescue.CardKind == CardKind.Peach ? !token.UsedPeach || token.PeachCardId != 0 || token.UsedAlcohol || token.AlcoholCardId is not null : !token.UsedAlcohol || token.AlcoholCardId != 0 || token.UsedPeach || token.PeachCardId is not null || rescue.SourceSeat != dying.VictimSeat) || token.UsedPeachPhysicalCardKind is not null)
            return false;
        for (var index = dyingIndex + 2; index < _resolutionStackOperations._resolutionStack.Count; index++)
            if (!_coreParticipantHandDamageRidesOperations.ParticipantHandRescueObserverRide(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1], rescue))
                return false;
        return true;
    }
}
