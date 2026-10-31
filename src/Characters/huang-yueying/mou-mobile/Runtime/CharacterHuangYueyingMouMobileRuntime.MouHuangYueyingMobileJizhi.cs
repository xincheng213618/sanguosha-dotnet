#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingMouMobileRuntime
{
    internal bool HasMouHuangYueyingMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangYueyingJizhi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment);

    internal static CardConversionSource MouHySource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_huang_yueying_mou_mobile_MouHuangYueyingMobileJizhi.MouHySource(f);
    SkillProgramStepOutcome IMouHuangYueyingMobileHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouHuangYueyingJizhi => BeginMouHyJizhi(f),
        SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment => BeginMouHyQicai(f),
        _ => BeginMouHyQiTransfer(f)};
    bool IMouHuangYueyingMobileQuery.CanPlace(int owner, int target) => MouHyEquipmentChoices(owner, target).Any();
    internal SkillProgramStepOutcome BeginMouHyJizhi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouHuangYueyingMobile is not null)
        {
            ContinueMouHy(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouHuangYueyingMobileComposition.Jizhi || f.InstructionIndex != 1 || f.WindowContext is not { } c || !(f.TriggerId == "ordinary-trick-used" && c.Window == SkillProgramTriggerWindow.CardUseBeforeTargetEffects || f.TriggerId == "nullification-used" && c.Window == SkillProgramTriggerWindow.CardResponseAccepted))
            throw new InvalidOperationException("Mobile Jizhi requires the exact native ordinary-trick Use or Nullification response.");
        var source = MouHySource(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = new(source, f.GameplayHash, SkillProgramEffectOp.ResolveMouHuangYueyingJizhi, MouHuangYueyingMobileStage.Drawing, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, c.ParentFrameId, f.OwnerSeat, Paid: true) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingJizhiStartedEvent(f.Id, source, f.GameplayHash, c.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.mou-mobile:jizhi.draw"));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouHuangYueyingJizhi)]
    internal void ReturnMouHyJizhiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MouHuangYueyingMobile is not { Operation: SkillProgramEffectOp.ResolveMouHuangYueyingJizhi, Stage: MouHuangYueyingMobileStage.Drawing } r || outcome.RequestedCount != 1 || outcome.RecipientSeat != f.OwnerSeat)
            throw new InvalidOperationException("Jizhi lost its exact native Draw1 return.");
        foreach (var id in outcome.ActualDrawIds)
        {
            var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == id && m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.mou-mobile:jizhi.draw");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingJizhiExemptEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwnerSeat, id, movement.Sequence));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = r with { Stage = MouHuangYueyingMobileStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingJizhiDrawReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        ContinueMouHy(f.Id);
    }

    internal bool IsMouHyJizhiHandLimitExempt(CharacterState owner, int cardId) => HasMouHuangYueyingMobile && owner.IsAlive && cardId > 0 && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber == 0 || _coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingJizhiExemptEvent>().Any(e => e.Source.OwnerSeat == owner.Seat && e.CardId == cardId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal bool DrainMouHy(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouHy(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHuangYueyingMobile: { } r } f || f.Id != id)
            return;
        if (r.Stage is MouHuangYueyingMobileStage.Choosing or MouHuangYueyingMobileStage.Drawing)
            return;
        if (DrainMouHy(f))
            return;
        FinishMouHy(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishMouHy(ProgramSkillFrame f)
    {
        var r = f.MouHuangYueyingMobile!;
        if (r.Operation == SkillProgramEffectOp.TransferMouHuangYueyingQicaiGains)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQiClaimSettledEvent(f.Id, r.GrantId!.Value, r.ClaimSequence!.Value));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingMobileCompletedEvent(f.Id, r.Operation, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
