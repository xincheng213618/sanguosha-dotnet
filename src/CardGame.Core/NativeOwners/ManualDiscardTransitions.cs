#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardTransitions(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IManualDiscardTransitions
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
