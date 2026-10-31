#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardTransitions(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IManualDiscardTransitions
{
    public void SetPrompt(PendingDecision prompt)
    {
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = prompt;
        matchOutcomeState.ReferenceToStatus = EngineStatus.AwaitingHumanDiscard;
    }

    public void PublishState() => advanceRulesAndPublishState();
    public bool BeginDiscardPhaseEnded(CharacterState owner) => getCoreGameEngineProgramLifecycleRuntime().TryBeginDiscardPhaseEndedProgramWindow(owner);
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void Log(string type, string message, int actorSeat) => getCoreMatchLogRuntime().AddLog(type, message, actorSeat);
}
