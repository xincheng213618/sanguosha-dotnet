#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchExecutionHost(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<MatchStepDispatcher> getMatchStepDispatcher, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelCompleteGameCall completeGame, Func<RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime) : IMatchExecutionHost
{
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public bool HasFrames => getFrameStore().Count != 0;
    public bool HasPendingDecision => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is not null;
    public bool HasHumanDecision => getCoreNativePromptBoundaryRuntime().IsHumanDecisionPending();
    public bool HasUnfinishedAutomaticDecision => getMatchStepDispatcher().HasUnfinishedAutomaticDecision();

    public void EnsureStarted() => getCoreNativePromptBoundaryRuntime().EnsureStarted();
    public void RunStep() => getCoreGameEngineRuntimeRuntime().RunOneEngineStep();
    public void CompleteGame() => completeGame();
    public void EndAsDraw(string reason) => getCoreMatchOutcomesRuntime().EndAsDraw(reason);
}
