#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndNativeReturns(Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<ProgramPhaseSchedulingRuntime> getProgramPhaseSchedulingRuntime) : IActualPhaseEndNativeReturns
{
    public void BeginTurnStartAfterProgramLifecycle(int actorSeat) => getCoreActualTurnsRuntime().BeginTurnStartAfterProgramLifecycle(matchParticipantsState.ReferenceToPlayers[actorSeat]);
    public void CompleteTurnStart(int actorSeat, int delayedEffects) => getCoreActualTurnsRuntime().CompleteTurnStart(matchParticipantsState.ReferenceToPlayers[actorSeat], (GameEngine.DelayedTurnEffects)delayedEffects);
    public void CompleteTurnStartAfterDraw(int actorSeat, int delayedEffects, bool afterNormalDrawProgramsCompleted, bool drawPhaseEndedProgramsCompleted, bool actualDrawCompletionCompleted) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(matchParticipantsState.ReferenceToPlayers[actorSeat], (GameEngine.DelayedTurnEffects)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    public void CompletePlayPhaseAfterProgramWindow() => getCoreGameEngineProgramLifecycleRuntime().CompletePlayPhaseAfterProgramWindow();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void FinalizeEndTurn(int actorSeat) => getCoreActualTurnsRuntime().FinalizeEndTurn(matchParticipantsState.ReferenceToPlayers[actorSeat]);
    public void AdvanceRulesAndPublishState() => advanceRulesAndPublishState();
    public bool ResumeScheduledProgramPhase() => getProgramPhaseSchedulingRuntime().ResumeScheduledProgramPhase();
}
