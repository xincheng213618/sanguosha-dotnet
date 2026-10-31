#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramActivationWindowRuntimePortsRuntime
{
    DamageTriggerWindowFrame? IProgramActivationWindowFacts.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    bool IProgramActivationWindowFacts.TryGetMobileZhitiContestTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _characterZhangLiaoMobileGodOperations.TryGetMobileZhitiContestTrigger(out candidate, out context);
    bool IProgramActivationWindowFacts.TryGetDrivenProgramTriggerCandidate(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _coreProgramTriggerWindowsOperations.TryGetDrivenProgramTriggerCandidate(out candidate, out context);
    bool IProgramActivationWindowFacts.TryGetPindianRevealTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context) => _corePindianRevealWindowsOperations.TryGetPindianRevealTrigger(out candidate, out context);
    ProgramTriggerCandidate IProgramActivationWindowFacts.AfterTurnEndedCandidate(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(frame);
    ProgramSkillWindowContext IProgramActivationWindowFacts.CreateAfterTurnEndedContext(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate) => _coreAfterTurnEndedProgramsOperations.CreateAfterTurnEndedContext(frame, candidate);
    ProgramSkillWindowContext IProgramActivationWindowFacts.CreateTurnEndingProgramContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts) => _coreProgramLifecycleOperations.CreateTurnEndingProgramContext(frame, candidate, candidateFacts);
    ProgramSkillWindowContext IProgramActivationWindowFacts.CreateOwnerDiedProgramContext(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreDeathProgramsOperations.CreateOwnerDiedProgramContext(frame, candidate);
    ProgramSkillWindowContext IProgramActivationWindowFacts.CreateBeforeDamageProgramContext(BeforeDamageProgramWindowFrame frame, BeforeDamageProgramCandidate item) => _coreDamagePreventionProgramsOperations.CreateBeforeDamageProgramContext(frame, item);
    ProgramSkillWindowContext IProgramActivationWindowFacts.CreateAfterDamageProgramContext(DamageTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.CreateAfterDamageProgramContext(frame, candidate);
    ProgramSkillWindowContext IProgramActivationWindowFacts.PindianRevealContext(PindianRevealWindowFrame frame, ProgramTriggerCandidate candidate) => _corePindianRevealWindowsOperations.PindianRevealContext(frame, candidate);
    bool IProgramActivationWindowReturns.TryResolveTurnEndingOrder(PromptChoice selected) => _coreTurnEndingOrderOperations.TryResolveTurnEndingOrder(selected);
    bool IProgramActivationWindowReturns.TryResolveDrivenProgramTriggerGroupChoice(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action) => _coreProgramTriggerWindowsOperations.TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);
    bool IProgramActivationWindowReturns.TrySkipMobileZhitiContestTrigger(ProgramTriggerCandidate candidate) => _characterZhangLiaoMobileGodOperations.TrySkipMobileZhitiContestTrigger(candidate);
    bool IProgramActivationWindowReturns.TrySkipDrivenProgramCandidate(ProgramTriggerCandidate candidate) => _coreProgramTriggerWindowsOperations.TrySkipDrivenProgramCandidate(candidate);
    void IProgramActivationWindowReturns.CompletePindianRevealBinding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePindianRevealWindowsOperations.CompletePindianRevealBinding2(candidate, context);
    void IProgramActivationWindowReturns.CompleteFirstLegacyBenefitCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreFirstSlashDuelLegacyWindowsOperations.CompleteFirstLegacyBenefitCandidate(candidate, context);
    void IProgramActivationWindowReturns.AdvanceForeignActualTurnStartCandidate(ForeignActualTurnStartWindowFrame frame, bool activated, bool completed) => _coreForeignTurnContestsOperations.AdvanceForeignActualTurnStartCandidate(frame, activated, completed);
    void IProgramActivationWindowReturns.AdvanceActualUseTargetCandidate(ActualUseTargetWindowFrame frame, bool activated, bool completed) => _coreActualUseTargetProgramsOperations.AdvanceActualUseTargetCandidate(frame, activated, completed);
    void IProgramActivationWindowReturns.CompleteCardEffectCandidate(ProgramSkillWindowContext context) => _coreCardEffectProgramsOperations.CompleteCardEffectCandidate(context);
    void IProgramActivationWindowReturns.AdvanceAfterTurnEndedCandidate(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreAfterTurnEndedProgramsOperations.AdvanceAfterTurnEndedCandidate(frame, candidate, activated, completed);
    void IProgramActivationWindowReturns.ContinueDeferredTurnEnd() => _coreDeferredHandAlignmentsOperations.ContinueDeferredTurnEnd();
    void IProgramActivationWindowReturns.AdvanceKillDiedProgramCandidate(ProgramKillTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreDeathProgramsOperations.AdvanceKillDiedProgramCandidate(frame, candidate, activated, completed);
    void IProgramActivationWindowReturns.AdvanceTurnEndingBoundaryCandidate(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreProgramLifecycleOperations.AdvanceTurnEndingBoundaryCandidate(frame, candidate, activated, completed);
    void IProgramActivationWindowReturns.AdvanceOwnerDiedProgramCandidate(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _coreDeathProgramsOperations.AdvanceOwnerDiedProgramCandidate(frame, candidate, activated, completed);
    void IProgramActivationWindowReturns.AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => _coreDamagePreventionProgramsOperations.AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
    void IProgramActivationWindowReturns.AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame) => _coreNativeDamageWindowsOperations.AdvanceDamageTriggerCandidate(frame);
}
