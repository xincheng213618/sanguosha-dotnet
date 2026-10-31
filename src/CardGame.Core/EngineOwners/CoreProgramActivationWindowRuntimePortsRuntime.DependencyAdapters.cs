#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreTurnEndingOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreTurnEndingOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndingOrderRuntime> _targetFactory = targetFactory;
    public bool TryResolveTurnEndingOrder(global::CardGame.Core.PromptChoice choice) => this._targetFactory().TryResolveTurnEndingOrder(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryGetMobileZhitiContestTrigger(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().TryGetMobileZhitiContestTrigger(out candidate, out context);
    public bool TrySkipMobileZhitiContestTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().TrySkipMobileZhitiContestTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCorePindianRevealWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCorePindianRevealWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _targetFactory = targetFactory;
    public bool TryGetPindianRevealTrigger(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().TryGetPindianRevealTrigger(out candidate, out context);
    public global::CardGame.Core.ProgramSkillWindowContext PindianRevealContext(global::CardGame.Core.PindianRevealWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().PindianRevealContext(w, c);
    public void CompletePindianRevealBinding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompletePindianRevealBinding(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreProgramTriggerWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreProgramTriggerWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> _targetFactory = targetFactory;
    public bool TryGetDrivenProgramTriggerCandidate(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().TryGetDrivenProgramTriggerCandidate(out candidate, out context);
    public bool TryResolveDrivenProgramTriggerGroupChoice(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.PromptChoice selected, string? action) => this._targetFactory().TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);
    public bool TrySkipDrivenProgramCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().TrySkipDrivenProgramCandidate(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public void ContinueDeferredTurnEnd() => this._targetFactory().ContinueDeferredTurnEnd();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().AfterTurnEndedCandidate(parent);
    public global::CardGame.Core.ProgramSkillWindowContext CreateAfterTurnEndedContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateAfterTurnEndedContext(parent, candidate);
    public void AdvanceAfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => this._targetFactory().AdvanceAfterTurnEndedCandidate(parent, candidate, activated, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreDeathProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreDeathProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateOwnerDiedProgramContext(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateOwnerDiedProgramContext(frame, candidate);
    public void AdvanceKillDiedProgramCandidate(global::CardGame.Core.ProgramKillTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => this._targetFactory().AdvanceKillDiedProgramCandidate(frame, candidate, activated, completed);
    public void AdvanceOwnerDiedProgramCandidate(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => this._targetFactory().AdvanceOwnerDiedProgramCandidate(frame, candidate, activated, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreNativeDamageWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreNativeDamageWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _targetFactory = targetFactory;
    public void AdvanceDamageTriggerCandidate(global::CardGame.Core.DamageTriggerWindowFrame window) => this._targetFactory().AdvanceDamageTriggerCandidate(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreActualUseTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreActualUseTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _targetFactory = targetFactory;
    public void AdvanceActualUseTargetCandidate(global::CardGame.Core.ActualUseTargetWindowFrame parent, bool activated, bool completed) => this._targetFactory().AdvanceActualUseTargetCandidate(parent, activated, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreCardEffectProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreCardEffectProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteCardEffectCandidate(global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteCardEffectCandidate(context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateTurnEndingProgramContext(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTriggerFacts? candidateFacts = default) => this._targetFactory().CreateTurnEndingProgramContext(frame, candidate, candidateFacts);
    public global::CardGame.Core.ProgramSkillWindowContext CreateAfterDamageProgramContext(global::CardGame.Core.DamageTriggerWindowFrame damage, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateAfterDamageProgramContext(damage, candidate);
    public void AdvanceTurnEndingBoundaryCandidate(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => this._targetFactory().AdvanceTurnEndingBoundaryCandidate(frame, candidate, activated, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreFirstSlashDuelLegacyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreFirstSlashDuelLegacyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _targetFactory = targetFactory;
    public void CompleteFirstLegacyBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteFirstLegacyBenefitCandidate(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreDamagePreventionProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreDamagePreventionProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateBeforeDamageProgramContext(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, global::CardGame.Core.BeforeDamageProgramCandidate item) => this._targetFactory().CreateBeforeDamageProgramContext(frame, item);
    public void AdvanceBeforeDamageProgramCandidate(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => this._targetFactory().AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CoreProgramActivationWindowRuntimePortsCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramActivationWindowRuntimePortsCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public void AdvanceForeignActualTurnStartCandidate(global::CardGame.Core.ForeignActualTurnStartWindowFrame parent, bool activated, bool completed) => this._targetFactory().AdvanceForeignActualTurnStartCandidate(parent, activated, completed);
}
