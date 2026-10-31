#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaHappyGodRuntime
{
    internal void IssueHappyGuoJiaUse(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyGuoJia!;
        if (!ValidHappyGj(frame) || receipt is not { Stage: HappyGuoJiaStage.CostChildren, Operation: SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing, Plan: { } plan } || !HappyGjPlans(frame.OwnerSeat).Any(option => CharacterGuoJiaMobileGodRuntime.SameShenGuoJiaPlan(option, plan)))
        {
            FinishHappyGj(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyGuoJia = receipt with { Stage = HappyGuoJiaStage.IssuingUse } });
        if (plan.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = plan.Targets;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(frame.Id, frame.InstructionIndex, 0, frame.OwnerSeat, frame.OwnerSeat, frame.OwnerSeat, frame.SkillId, frame.ActivationId, frame.SkillInstanceId, frame.GameplayHash, receipt.Turn, plan.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, plan.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, frame.OwnerSeat, plan.Targets, plan.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        frame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(value => value.Id == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { HappyGuoJia = frame.HappyGuoJia!with { Stage = HappyGuoJiaStage.NativeUse, UseId = id, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaNativeIssuedEvent(frame.Id, id, use.Action!.ActionId, plan.Kind, receipt.Benefactor));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, frame.OwnerSeat, use.TargetSeats, plan.ActionKind, plan.TargetCardId, plan.RequiredKind, plan.Kind);
    }

    internal bool IsHappyGuoJiaVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: HappyGjZuoxing } origin || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ParentProgramFrameId)is not { HappyGuoJia: { Plan: { } plan } receipt } frame || !ValidHappyGj(frame) || receipt.Stage is not (HappyGuoJiaStage.IssuingUse or HappyGuoJiaStage.NativeUse) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } action || action.ProviderSeat != frame.OwnerSeat || use.SourceSeat != action.ActorSeat || action.ActionId != origin.CardActionId || action.EffectiveKind != plan.Kind || use.CardKind != plan.Kind || action.EffectiveSuit is not (null or Suit.None) || action.EffectiveRank is not (null or 0) || action.EffectiveIsRed == true || !action.TargetSeats.SequenceEqual(use.TargetSeats) || origin.OwnerSeat != frame.OwnerSeat || origin.InitialActorSeat != frame.OwnerSeat || origin.InitialTargetSeat != frame.OwnerSeat || origin.BindingId != frame.ActivationId || origin.SkillInstanceId != frame.SkillInstanceId || origin.GameplayHash != frame.GameplayHash || origin.InstructionIndex != frame.InstructionIndex || origin.LifecycleFrameId != 0 || origin.TurnNumber != receipt.Turn || receipt.UseId != 0 && (receipt.UseId != use.Id || receipt.ActionId != action.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Count(fact => fact.FrameId == frame.Id && fact.Purpose == "happy-zuoxing" && fact.Seat == receipt.Benefactor && fact.Before == receipt.MaximumBefore && fact.After == receipt.MaximumAfter && fact.After == HappyGjMaximumAfterPayment(fact.Seat, fact.Before, -1)) == 1 && (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(fact => fact.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, frame.OwnerSeat, frame.OwnerSeat) : action.ActorSeat == frame.OwnerSeat);
    }

    internal bool TryReturnHappyGuoJiaVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: HappyGjZuoxing } origin)
            return false;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ParentProgramFrameId);
        var receipt = frame.HappyGuoJia!;
        if (!ValidHappyGj(frame) || receipt.Stage != HappyGuoJiaStage.NativeUse || receipt.UseId != use.Id || receipt.ActionId != use.Action?.ActionId || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaNativeReturnedEvent>().Any(fact => fact.FrameId == frame.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.Id && fact.CardId == 0 && fact.CardKind == receipt.Plan!.Kind) != 1)
            throw new InvalidOperationException("Zuoxing requires the same complete native zero-material Use once.");
        var hadAttack = use.CardAttack is { Active: true };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyGuoJia = receipt with { Stage = HappyGuoJiaStage.ReturnedUse } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaNativeReturnedEvent(frame.Id, use.Id, receipt.ActionId, hadAttack));
        if (!hadAttack)
            FinishHappyGj(frame);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 40900)]
    internal bool ReturnHappyGuoJiaAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuoJia: { Stage: HappyGuoJiaStage.ReturnedUse } receipt } frame)
            return false;
        if (!ValidHappyGj(frame) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.UseId)is not null || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaNativeReturnedEvent>().Any(fact => fact.FrameId == frame.Id && fact.HadAttack))
            throw new InvalidOperationException("Zuoxing cannot resume before the native damage tail returns.");
        FinishHappyGj(frame);
        return true;
    }
}
