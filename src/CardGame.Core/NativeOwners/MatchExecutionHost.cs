#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchExecutionHost(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.MatchStepDispatcher> getMatchStepDispatcher, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.NativeOwners.KernelCompleteGameCall completeGame, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime) : IMatchExecutionHost
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
