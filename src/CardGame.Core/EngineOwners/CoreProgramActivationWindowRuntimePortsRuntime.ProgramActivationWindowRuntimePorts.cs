#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramActivationWindowRuntimePortsRuntime
{
    DamageTriggerWindowFrame? global::CardGame.Core.IProgramActivationWindowFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    bool global::CardGame.Core.IProgramActivationWindowFacts.TryGetMobileZhitiContestTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _characterZhangLiaoMobileGodOperations.TryGetMobileZhitiContestTrigger(out candidate, out context);
    bool global::CardGame.Core.IProgramActivationWindowFacts.TryGetDrivenProgramTriggerCandidate(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _coreProgramTriggerWindowsOperations.TryGetDrivenProgramTriggerCandidate(out candidate, out context);
    bool global::CardGame.Core.IProgramActivationWindowFacts.TryGetPindianRevealTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _corePindianRevealWindowsOperations.TryGetPindianRevealTrigger(out candidate, out context);
    ProgramTriggerCandidate global::CardGame.Core.IProgramActivationWindowFacts.AfterTurnEndedCandidate(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(frame);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.CreateAfterTurnEndedContext(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate) => _coreAfterTurnEndedProgramsOperations.CreateAfterTurnEndedContext(frame, candidate);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.CreateTurnEndingProgramContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts) => _coreProgramLifecycleOperations.CreateTurnEndingProgramContext(frame, candidate, candidateFacts);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.CreateOwnerDiedProgramContext(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreDeathProgramsOperations.CreateOwnerDiedProgramContext(frame, candidate);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.CreateBeforeDamageProgramContext(BeforeDamageProgramWindowFrame frame, BeforeDamageProgramCandidate item) => _coreDamagePreventionProgramsOperations.CreateBeforeDamageProgramContext(frame, item);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.CreateAfterDamageProgramContext(DamageTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.CreateAfterDamageProgramContext(frame, candidate);
    ProgramSkillWindowContext global::CardGame.Core.IProgramActivationWindowFacts.PindianRevealContext(PindianRevealWindowFrame frame, ProgramTriggerCandidate candidate) => _corePindianRevealWindowsOperations.PindianRevealContext(frame, candidate);
    bool global::CardGame.Core.IProgramActivationWindowReturns.TryResolveTurnEndingOrder(PromptChoice selected) => _coreTurnEndingOrderOperations.TryResolveTurnEndingOrder(selected);
    bool global::CardGame.Core.IProgramActivationWindowReturns.TryResolveDrivenProgramTriggerGroupChoice(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action) => _coreProgramTriggerWindowsOperations.TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);
    bool global::CardGame.Core.IProgramActivationWindowReturns.TrySkipMobileZhitiContestTrigger(ProgramTriggerCandidate candidate) => _characterZhangLiaoMobileGodOperations.TrySkipMobileZhitiContestTrigger(candidate);
    bool global::CardGame.Core.IProgramActivationWindowReturns.TrySkipDrivenProgramCandidate(ProgramTriggerCandidate candidate) => _coreProgramTriggerWindowsOperations.TrySkipDrivenProgramCandidate(candidate);
    void global::CardGame.Core.IProgramActivationWindowReturns.CompletePindianRevealBinding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePindianRevealWindowsOperations.CompletePindianRevealBinding(candidate, context);
    void global::CardGame.Core.IProgramActivationWindowReturns.CompleteFirstLegacyBenefitCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFirstSlashDuelLegacyWindowsOperations.CompleteFirstLegacyBenefitCandidate(candidate, context);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceForeignActualTurnStartCandidate(ForeignActualTurnStartWindowFrame frame, bool activated, bool completed) => _coreForeignTurnContestsOperations.AdvanceForeignActualTurnStartCandidate(frame, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceActualUseTargetCandidate(ActualUseTargetWindowFrame frame, bool activated, bool completed) => _coreActualUseTargetProgramsOperations.AdvanceActualUseTargetCandidate(frame, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.CompleteCardEffectCandidate(ProgramSkillWindowContext context) => _coreCardEffectProgramsOperations.CompleteCardEffectCandidate(context);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceAfterTurnEndedCandidate(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreAfterTurnEndedProgramsOperations.AdvanceAfterTurnEndedCandidate(frame, candidate, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.ContinueDeferredTurnEnd() => _coreDeferredHandAlignmentsOperations.ContinueDeferredTurnEnd();
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceKillDiedProgramCandidate(ProgramKillTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreDeathProgramsOperations.AdvanceKillDiedProgramCandidate(frame, candidate, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceTurnEndingBoundaryCandidate(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreProgramLifecycleOperations.AdvanceTurnEndingBoundaryCandidate(frame, candidate, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceOwnerDiedProgramCandidate(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreDeathProgramsOperations.AdvanceOwnerDiedProgramCandidate(frame, candidate, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => _coreDamagePreventionProgramsOperations.AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
    void global::CardGame.Core.IProgramActivationWindowReturns.AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame) => _coreNativeDamageWindowsOperations.AdvanceDamageTriggerCandidate(frame);
}
