#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramBindingRuntimePortsRuntime
{
    ContentSkillDefinition global::CardGame.Core.IProgramBindingFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    SkillProgramTrigger global::CardGame.Core.IProgramBindingFacts.Trigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
    bool global::CardGame.Core.IProgramBindingFacts.CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context);
    bool global::CardGame.Core.IProgramBindingFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IProgramBindingFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int global::CardGame.Core.IProgramBindingFacts.ActualPlayPhaseUseCount(int seat) => _coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(seat);
    void global::CardGame.Core.IProgramBindingProtocols.ConsumeEarnedActualEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreEarnedActualEndingBenefitsOperations.ConsumeEarnedActualEndingAtBegin(candidate, context);
    void global::CardGame.Core.IProgramBindingProtocols.ConsumePrepDiscardEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePrepDiscardEndingPromisesOperations.ConsumePrepDiscardEndingAtBegin(candidate, context);
    void global::CardGame.Core.IProgramBindingProtocols.ConsumeFixedDistanceDebt(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFixedDistanceOneDebtOperations.ConsumeFixedDistanceDebtAtBegin(candidate, context);
    void global::CardGame.Core.IProgramBindingProtocols.ConsumeUsage(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(candidate);
    void global::CardGame.Core.IProgramBindingProtocols.PayMarkerCost(int seat, SkillProgramMarkerCost? cost, string skillId, string bindingId) => _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(_coreMatchParticipantsPlayersAccess._players[seat], cost, skillId, bindingId);
    ProgramDyingAlcoholPermission? global::CardGame.Core.IProgramBindingProtocols.CaptureDyingAlcoholPermission(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreAlcoholIdentityPoliciesOperations.CaptureProgramDyingAlcoholPermission(candidate, context);
    ProgramSkillFrame global::CardGame.Core.IProgramBindingProtocols.CaptureHpConditionalSource(ProgramSkillFrame frame) => _coreHpConditionalSkillGrantsOperations.CaptureHpConditionalProgramSource(frame);
    ProgramSkillFrame global::CardGame.Core.IProgramBindingProtocols.CaptureEquipmentSuitSource(ProgramSkillFrame frame) => _characterHeQiOlOperations.CaptureEquipmentSuitProgramSource(frame);
    void global::CardGame.Core.IProgramBindingProtocols.AttachAfterTurnEndedChild(ProgramSkillFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreAfterTurnEndedProgramsOperations.AttachAfterTurnEndedChild(frame, candidate, context);
    void global::CardGame.Core.IProgramBindingProtocols.RecordPublicPileColorOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePublicPileColorsOperations.RecordPublicPileColorOffer(candidate, context);
    void global::CardGame.Core.IProgramBindingProtocols.LogStarted(int seat, string skillName) => _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 发动【{(skillName)}】。", seat);
}
