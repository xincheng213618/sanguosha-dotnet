#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangAuditARuntime
{
    internal const string WushuangAuditedTiaoxin = "wushuang:skill-3001";
    void IWushuangTiaoxinExpansionHost.Record(ProgramSkillFrame frame) => RecordWushuangTiaoxinExpansion(frame);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 63000)]
    internal bool AllowsWushuangTiaoxinPhase(ProgramActivationLegalityContext gate)
    {
        if (gate.SkillId != WushuangAuditedTiaoxin)
            return true;
        return _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((gate.OwnerSeat, gate.SkillId, gate.Activation.UsageGroup)) == 0 || _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is PhaseChangedEvent or TurnStartedEvent).OfType<WushuangTiaoxinPhaseExpandedEvent>().Any(e => e.OwnerSeat == gate.OwnerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.SkillInstanceId == _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(gate.Owner, gate.SkillId));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 63000)]
    internal bool AllowsWushuangBeigeCost(ProgramTriggerLegalityContext gate) => gate.SkillId != "wushuang:skill-3140" || gate.Trigger.Id != "slash-damage-prompt" || _coreGuanduGeneralsOperations.GuanduDiscardable(gate.Owner.Seat, gate.Owner.Seat).Length > 0;
    internal void RecordWushuangTiaoxinExpansion(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != WushuangAuditedTiaoxin || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.RecordWushuangTiaoxinPhaseExpansion || frame.ChoiceBindings.SingleOrDefault(b => b.Name == "tiaoxin-answer")is not { } answer)
            throw new InvalidOperationException("The official Tiaoxin expansion lost its exact completed requested Slash.");
        if (answer.OptionId == RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption && frame.RequestedSlashDamagedOwner == true)
            return;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangTiaoxinPhaseExpandedEvent>().Any(e => e.FrameId == frame.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangTiaoxinPhaseExpandedEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
    }
}
