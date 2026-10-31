#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignActualTurnWindowReturns(global::CardGame.Core.NativeOwners.KernelCompleteGameCall completeGame, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignActualTurnWindowReturns
{
    public void CompleteGame() => completeGame();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void AfterForeignActualContests(int ownerSeat) => getCoreActualTurnsRuntime().ContinueTurnAfterForeignActualContests(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
}
