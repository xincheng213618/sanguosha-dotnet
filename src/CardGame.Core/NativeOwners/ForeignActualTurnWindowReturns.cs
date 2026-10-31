#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignActualTurnWindowReturns(KernelCompleteGameCall completeGame, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IForeignActualTurnWindowReturns
{
    public void CompleteGame() => completeGame();
    public void EndTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void AfterForeignActualContests(int ownerSeat) => getCoreActualTurnsRuntime().ContinueTurnAfterForeignActualContests(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
}
