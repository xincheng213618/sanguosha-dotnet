#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseEndNativeReturns(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.ProgramPhaseSchedulingRuntime> getProgramPhaseSchedulingRuntime) : IActualPhaseEndNativeReturns
{
    public void BeginTurnStartAfterProgramLifecycle(int actorSeat) => getCoreActualTurnsRuntime().BeginTurnStartAfterProgramLifecycle(matchParticipantsState.ReferenceToPlayers[actorSeat]);
    public void CompleteTurnStart(int actorSeat, int delayedEffects) => getCoreActualTurnsRuntime().CompleteTurnStart(matchParticipantsState.ReferenceToPlayers[actorSeat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects);
    public void CompleteTurnStartAfterDraw(int actorSeat, int delayedEffects, bool afterNormalDrawProgramsCompleted, bool drawPhaseEndedProgramsCompleted, bool actualDrawCompletionCompleted) => getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(matchParticipantsState.ReferenceToPlayers[actorSeat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    public void CompletePlayPhaseAfterProgramWindow() => getCoreGameEngineProgramLifecycleRuntime().CompletePlayPhaseAfterProgramWindow();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void FinalizeEndTurn(int actorSeat) => getCoreActualTurnsRuntime().FinalizeEndTurn(matchParticipantsState.ReferenceToPlayers[actorSeat]);
    public void AdvanceRulesAndPublishState() => advanceRulesAndPublishState();
    public bool ResumeScheduledProgramPhase() => getProgramPhaseSchedulingRuntime().ResumeScheduledProgramPhase();
}
