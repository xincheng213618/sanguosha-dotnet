#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramBindingRuntimePortsCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetActualPlayPhaseUseCount(int actorSeat);
}

internal interface ICoreProgramBindingRuntimePortsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramBindingRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ConsumeProgramTriggerUsage(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreProgramBindingRuntimePortsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDyingAlcoholPermission? CaptureProgramDyingAlcoholPermission(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramBindingRuntimePortsCoreEarnedActualEndingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeEarnedActualEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsCoreProgramMarkerPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId);
}

internal interface ICoreProgramBindingRuntimePortsCorePrepDiscardEndingPromisesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumePrepDiscardEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AttachAfterTurnEndedChild(global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsCorePublicPileColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordPublicPileColorOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsCoreFixedDistanceOneDebtOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeFixedDistanceDebtAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramBindingRuntimePortsCharacterHeQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramBindingRuntimePortsCoreHpConditionalSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureHpConditionalProgramSource(global::CardGame.Core.ProgramSkillFrame frame);
}
