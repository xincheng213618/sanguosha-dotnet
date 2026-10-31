#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public int GetActualPlayPhaseUseCount(int actorSeat) => this._targetFactory().GetActualPlayPhaseUseCount(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public void ConsumeProgramTriggerUsage(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().ConsumeProgramTriggerUsage(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDyingAlcoholPermission? CaptureProgramDyingAlcoholPermission(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CaptureProgramDyingAlcoholPermission(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreEarnedActualEndingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreEarnedActualEndingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> _targetFactory = targetFactory;
    public void ConsumeEarnedActualEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ConsumeEarnedActualEndingAtBegin(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreProgramMarkerPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreProgramMarkerPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> _targetFactory = targetFactory;
    public void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId) => this._targetFactory().PayProgramMarkerCost(owner, cost, skillId, bindingId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCorePrepDiscardEndingPromisesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCorePrepDiscardEndingPromisesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _targetFactory = targetFactory;
    public void ConsumePrepDiscardEndingAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ConsumePrepDiscardEndingAtBegin(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public void AttachAfterTurnEndedChild(global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().AttachAfterTurnEndedChild(child, candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCorePublicPileColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCorePublicPileColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime> _targetFactory = targetFactory;
    public void RecordPublicPileColorOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().RecordPublicPileColorOffer(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreFixedDistanceOneDebtAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreFixedDistanceOneDebtOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime> _targetFactory = targetFactory;
    public void ConsumeFixedDistanceDebtAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ConsumeFixedDistanceDebtAtBegin(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCharacterHeQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCharacterHeQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CaptureEquipmentSuitProgramSource(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime))]
internal sealed class CoreProgramBindingRuntimePortsCoreHpConditionalSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBindingRuntimePortsCoreHpConditionalSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureHpConditionalProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CaptureHpConditionalProgramSource(frame);
}
