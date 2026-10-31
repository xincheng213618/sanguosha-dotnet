#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HpChangedTriggerWindowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime) : global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost<global::CardGame.Core.HpChangedTriggerWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
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
