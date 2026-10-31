namespace CardGame.Core;

internal interface IProgramActivationWindowFacts : IProgramSelectionQuery
{
    DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
    bool TryGetMobileZhitiContestTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context);
    bool TryGetDrivenProgramTriggerCandidate(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context);
    bool TryGetPindianRevealTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context);
    ProgramTriggerCandidate AfterTurnEndedCandidate(DeferredTurnEndFrame frame);
    ProgramSkillWindowContext CreateAfterTurnEndedContext(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate);
    ProgramSkillWindowContext CreateTurnEndingProgramContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts);
    ProgramSkillWindowContext CreateOwnerDiedProgramContext(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate);
    ProgramSkillWindowContext CreateBeforeDamageProgramContext(BeforeDamageProgramWindowFrame frame, BeforeDamageProgramCandidate item);
    ProgramSkillWindowContext CreateAfterDamageProgramContext(DamageTriggerWindowFrame frame, ProgramTriggerCandidate candidate);
    ProgramSkillWindowContext PindianRevealContext(PindianRevealWindowFrame frame, ProgramTriggerCandidate candidate);
}

// Each producer continues to validate its own typed return and cost receipts.
internal interface IProgramActivationWindowReturns : IProgramRuntimeOperations
{
    bool TryResolveTurnEndingOrder(PromptChoice selected);
    bool TryResolveDrivenProgramTriggerGroupChoice(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action);
    bool TrySkipMobileZhitiContestTrigger(ProgramTriggerCandidate candidate);
    bool TrySkipDrivenProgramCandidate(ProgramTriggerCandidate candidate);
    void CompletePindianRevealBinding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void CompleteFirstLegacyBenefitCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void AdvanceForeignActualTurnStartCandidate(ForeignActualTurnStartWindowFrame frame, bool activated, bool completed);
    void AdvanceActualUseTargetCandidate(ActualUseTargetWindowFrame frame, bool activated, bool completed);
    void CompleteCardEffectCandidate(ProgramSkillWindowContext context);
    void AdvanceAfterTurnEndedCandidate(DeferredTurnEndFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed);
    void ContinueDeferredTurnEnd();
    void AdvanceKillDiedProgramCandidate(ProgramKillTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed);
    void AdvanceTurnEndingBoundaryCandidate(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed);
    void AdvanceOwnerDiedProgramCandidate(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed);
    void AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed);
    void AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame);
}
