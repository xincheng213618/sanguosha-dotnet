#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnQueries(EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IProgramTurnQueries
{
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int TurnOwnerSeat => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public int TurnOwnerTurn => actualTurnProgressState.ReferenceToTurnProgression.TurnNumber;
}
