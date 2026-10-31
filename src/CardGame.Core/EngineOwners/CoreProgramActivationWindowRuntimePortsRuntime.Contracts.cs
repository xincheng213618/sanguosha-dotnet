#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramActivationWindowRuntimePortsCoreTurnEndingOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveTurnEndingOrder(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramActivationWindowRuntimePortsCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetMobileZhitiContestTrigger(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context);
    bool TrySkipMobileZhitiContestTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreProgramActivationWindowRuntimePortsCorePindianRevealWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetPindianRevealTrigger(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.ProgramSkillWindowContext PindianRevealContext(global::CardGame.Core.PindianRevealWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c);
    void CompletePindianRevealBinding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreProgramTriggerWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetDrivenProgramTriggerCandidate(out global::CardGame.Core.ProgramTriggerCandidate candidate, out global::CardGame.Core.ProgramSkillWindowContext context);
    bool TryResolveDrivenProgramTriggerGroupChoice(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.PromptChoice selected, string? action);
    bool TrySkipDrivenProgramCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDeferredTurnEnd();
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent);
    global::CardGame.Core.ProgramSkillWindowContext CreateAfterTurnEndedContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate);
    void AdvanceAfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateOwnerDiedProgramContext(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate);
    void AdvanceKillDiedProgramCandidate(global::CardGame.Core.ProgramKillTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed);
    void AdvanceOwnerDiedProgramCandidate(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceDamageTriggerCandidate(global::CardGame.Core.DamageTriggerWindowFrame window);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceActualUseTargetCandidate(global::CardGame.Core.ActualUseTargetWindowFrame parent, bool activated, bool completed);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreCardEffectProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteCardEffectCandidate(global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateTurnEndingProgramContext(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTriggerFacts? candidateFacts = default);
    global::CardGame.Core.ProgramSkillWindowContext CreateAfterDamageProgramContext(global::CardGame.Core.DamageTriggerWindowFrame damage, global::CardGame.Core.ProgramTriggerCandidate candidate);
    void AdvanceTurnEndingBoundaryCandidate(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteFirstLegacyBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreDamagePreventionProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateBeforeDamageProgramContext(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, global::CardGame.Core.BeforeDamageProgramCandidate item);
    void AdvanceBeforeDamageProgramCandidate(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, bool activated, bool completed);
}

internal interface ICoreProgramActivationWindowRuntimePortsCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceForeignActualTurnStartCandidate(global::CardGame.Core.ForeignActualTurnStartWindowFrame parent, bool activated, bool completed);
}
