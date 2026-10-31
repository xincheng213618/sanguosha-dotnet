#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HpChangedTriggerWindowHost(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime) : ProgramTriggerWindowHost<HpChangedTriggerWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
{
    public override bool IsComplete(HpChangedTriggerWindowFrame frame) => frame.CandidateIndex >= frame.Candidates.Count;
    public override ProgramTriggerWindowFlow Close(HpChangedTriggerWindowFrame frame)
    {
        getCoreGameEngineHpChangeProgramsRuntime().CompleteHpChangedProgramWindow(frame);
        return ProgramTriggerWindowFlow.Yield;
    }

    protected override ProgramTriggerCandidate Candidate(HpChangedTriggerWindowFrame frame) => frame.Candidates[frame.CandidateIndex];
    public override ProgramTriggerWindowEntry Read(HpChangedTriggerWindowFrame frame) => new(Candidate(frame), frame.Contexts[frame.CandidateIndex]);
    public override void Skip(HpChangedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => getCoreGameEngineHpChangeProgramsRuntime().AdvanceHpChangedProgramCandidate(frame, activated: false, completed: false);
    public override void BeforePrompt(HpChangedTriggerWindowFrame frame) => getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
}
