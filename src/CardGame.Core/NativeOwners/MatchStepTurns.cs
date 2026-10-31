#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepTurns(EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CoreNativeAiDecisionsEntries> getCoreNativeAiDecisionsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineDiscardEntries> getCoreGameEngineDiscardRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseEntries> getCoreActualDiscardPhaseRuntime) : IMatchStepTurns
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
