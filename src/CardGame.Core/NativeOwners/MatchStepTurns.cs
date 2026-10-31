#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepTurns(global::CardGame.Core.EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries> getCoreNativeAiDecisionsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardEntries> getCoreGameEngineDiscardRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseEntries> getCoreActualDiscardPhaseRuntime) : IMatchStepTurns
{
    public void ClearStrategicEndPlay() => gameEngineStrategicProgramsState.ReferenceToStrategicEndPlayTurn = -1;
    public void CompletePlayPhase() => getCoreGameEngineProgramLifecycleRuntime().CompleteCurrentPlayPhase();
    public void BeginTurn() => getCoreActualTurnsRuntime().BeginTurn();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void RequestHumanPlay() => getCoreNativePromptBoundaryRuntime().RequestHumanPlay();
    public void RunAiPlayDecision(int seat) => getCoreNativeAiDecisionsRuntime().RunOneAiPlayDecision(matchParticipantsState.ReferenceToPlayers[seat]);
    public int RequiredDiscard(int seat) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().RequiredSelfHandLimitDiscard(matchParticipantsState.ReferenceToPlayers[seat]);
    public void RequestHumanDiscard(int seat) => getCoreGameEngineDiscardRuntime().RequestHumanDiscard(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool AutoDiscardAndTryBeginEndedWindow(int seat)
    {
        // A native encounter may replace a seat while discard effects run.
        // The ended window still belongs to this exact original participant.
        var current = matchParticipantsState.ReferenceToPlayers[seat];
        getCoreActualDiscardPhaseRuntime().AutoDiscard(current);
        return getCoreGameEngineProgramLifecycleRuntime().TryBeginDiscardPhaseEndedProgramWindow(current);
    }
}
