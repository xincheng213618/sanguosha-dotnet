#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedTriggerWindowHost(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime) : global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost<global::CardGame.Core.CardsMovedTriggerWindowFrame>(getFrameStore, getCoreGameEngineProgramLifecycleRuntime, advanceEventRulesAndQueueFact, getCoreGameEngineRuntimeRuntime, getCoreNativePromptBoundaryRuntime)
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
