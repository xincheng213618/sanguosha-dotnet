#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardActionTriggerWindowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries> getCoreGameEngineCardActionsRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry) : global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost<global::CardGame.Core.ProgramCardTriggerWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
{
    public override ProgramTriggerWindowPreparation<ProgramCardTriggerWindowFrame> Prepare(ProgramCardTriggerWindowFrame frame) => getCoreGameEngineCardActionsRuntime().PrepareProgramCardWindow(frame);
    public override bool IsComplete(ProgramCardTriggerWindowFrame frame) => frame.CandidateIndex == frame.Candidates.Count;
    public override ProgramTriggerWindowFlow Close(ProgramCardTriggerWindowFrame frame) => getCoreGameEngineCardActionsRuntime().CompleteProgramCardWindow(frame);
    protected override ProgramTriggerCandidate Candidate(ProgramCardTriggerWindowFrame frame) => global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(frame.Candidates[frame.CandidateIndex]);
    protected override bool MatchesSkipCandidate(ProgramCardTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => frame.Candidates[frame.CandidateIndex] is { } current && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(current) == candidate;
    protected override bool CanReadPending(ProgramCardTriggerWindowFrame frame) => frame.Candidates[frame.CandidateIndex] is not null;
    public override ProgramTriggerWindowEntry Read(ProgramCardTriggerWindowFrame frame)
    {
        var candidate = frame.Candidates[frame.CandidateIndex];
        var program = getContentRegistry().Skills[candidate.SkillId].Program!;
        if (program.GameplayHash != candidate.GameplayHash)
            throw new InvalidOperationException("A running card trigger definition changed.");
        var trigger = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, candidate.TriggerId).Trigger!;
        return new(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate), getCoreGameEngineCardActionsRuntime().CreateCardActionProgramContext2(frame, candidate), trigger);
    }

    // Prompt restoration originally reads only the frozen action context; it does not resolve the program again.
    protected override ProgramTriggerWindowEntry ReadPending(ProgramCardTriggerWindowFrame frame) => new(Candidate(frame), getCoreGameEngineCardActionsRuntime().CreateCardActionProgramContext2(frame, frame.Candidates[frame.CandidateIndex]));
    public override void Skip(ProgramCardTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => getCoreGameEngineCardActionsRuntime().AdvanceProgramCardCandidate(frame);
}
