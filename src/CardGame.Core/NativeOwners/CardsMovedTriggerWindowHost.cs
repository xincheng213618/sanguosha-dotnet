#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedTriggerWindowHost(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime) : ProgramTriggerWindowHost<CardsMovedTriggerWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
{
    public override bool IsComplete(CardsMovedTriggerWindowFrame frame) => frame.CandidateIndex >= frame.Candidates.Count;
    public override ProgramTriggerWindowFlow Close(CardsMovedTriggerWindowFrame frame)
    {
        getCoreGameEngineCardMovementProgramsRuntime().CompleteCardsMovedProgramWindow(frame);
        return ProgramTriggerWindowFlow.Yield;
    }

    protected override ProgramTriggerCandidate Candidate(CardsMovedTriggerWindowFrame frame) => frame.Candidates[frame.CandidateIndex];
    public override ProgramTriggerWindowEntry Read(CardsMovedTriggerWindowFrame frame) => new(Candidate(frame), getCoreGameEngineCardMovementProgramsRuntime().CreateCardsMovedProgramContext(frame, Candidate(frame)));
    public override void Skip(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => getCoreGameEngineCardMovementProgramsRuntime().AdvanceCardsMovedProgramCandidate(frame, candidate, activated: false, completed: false);
    public override void BeforePrompt(CardsMovedTriggerWindowFrame frame) => getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
}
