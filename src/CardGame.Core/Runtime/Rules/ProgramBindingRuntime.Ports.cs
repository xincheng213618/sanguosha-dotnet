namespace CardGame.Core;

internal interface IProgramBindingFacts : IProgramSelectionQuery
{
    ContentSkillDefinition Skill(string skillId);
    SkillProgramTrigger Trigger(ProgramTriggerCandidate candidate);
    bool CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    bool IsHuman(int seat);
    string Name(int seat);
    int ActualPlayPhaseUseCount(int seat);
}

// Native producers retain their receipts and rule-specific payment semantics.
// The binding runtime owns the order in which those producers are invoked.
internal interface IProgramBindingProtocols : IProgramRuntimeOperations
{
    void ConsumeEarnedActualEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void ConsumePrepDiscardEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void ConsumeFixedDistanceDebt(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void ConsumeUsage(ProgramTriggerCandidate candidate);
    void PayMarkerCost(int seat, SkillProgramMarkerCost? cost, string skillId, string bindingId);
    ProgramDyingAlcoholPermission? CaptureDyingAlcoholPermission(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillFrame CaptureHpConditionalSource(ProgramSkillFrame frame);
    ProgramSkillFrame CaptureEquipmentSuitSource(ProgramSkillFrame frame);
    void AttachAfterTurnEndedChild(ProgramSkillFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void RecordPublicPileColorOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void LogStarted(int seat, string skillName);
}

// Other native windows still own their exact parent-selection and return proofs.
internal interface IProgramActivationWindows : IProgramRuntimeOperations
{
    bool TryResolveTurnEndingOrder(PromptChoice selected);
    (ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context) PendingCandidate();
    bool TryResolveGroup(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action);
    void CompleteSkipped(ProgramTriggerCandidate candidate);
}
