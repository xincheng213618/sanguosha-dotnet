#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramBindingRuntimePortsRuntime
{
    ContentSkillDefinition IProgramBindingFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    SkillProgramTrigger IProgramBindingFacts.Trigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
    bool IProgramBindingFacts.CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context);
    bool IProgramBindingFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IProgramBindingFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int IProgramBindingFacts.ActualPlayPhaseUseCount(int seat) => _coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(seat);
    void IProgramBindingProtocols.ConsumeEarnedActualEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreEarnedActualEndingBenefitsOperations.Value.ConsumeEarnedActualEndingAtBegin(candidate, context);
    void IProgramBindingProtocols.ConsumePrepDiscardEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePrepDiscardEndingPromisesOperations.ConsumePrepDiscardEndingAtBegin(candidate, context);
    void IProgramBindingProtocols.ConsumeFixedDistanceDebt(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFixedDistanceOneDebtOperations.Value.ConsumeFixedDistanceDebtAtBegin(candidate, context);
    void IProgramBindingProtocols.ConsumeUsage(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(candidate);
    void IProgramBindingProtocols.PayMarkerCost(int seat, SkillProgramMarkerCost? cost, string skillId, string bindingId) => _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[seat], cost, skillId, bindingId);
    ProgramDyingAlcoholPermission? IProgramBindingProtocols.CaptureDyingAlcoholPermission(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreAlcoholIdentityPoliciesOperations.CaptureProgramDyingAlcoholPermission(candidate, context);
    ProgramSkillFrame IProgramBindingProtocols.CaptureHpConditionalSource(ProgramSkillFrame frame) => _coreHpConditionalSkillGrantsOperations.Value.CaptureHpConditionalProgramSource(frame);
    ProgramSkillFrame IProgramBindingProtocols.CaptureEquipmentSuitSource(ProgramSkillFrame frame) => _characterHeQiOlOperations.CaptureEquipmentSuitProgramSource(frame);
    void IProgramBindingProtocols.AttachAfterTurnEndedChild(ProgramSkillFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreAfterTurnEndedProgramsOperations.AttachAfterTurnEndedChild(frame, candidate, context);
    void IProgramBindingProtocols.RecordPublicPileColorOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePublicPileColorsOperations.RecordPublicPileColorOffer(candidate, context);
    void IProgramBindingProtocols.LogStarted(int seat, string skillName) => _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 发动【{(skillName)}】。", seat);
}
