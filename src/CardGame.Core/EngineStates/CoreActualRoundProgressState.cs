#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualRoundProgressState : Contracts.ICoreActualRoundProgressRoundNumberAccess, Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess, Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess
{
    private int _roundNumber = default !;
    internal ref int ReferenceToRoundNumber => ref _roundNumber;

    private HashSet<int> _roundTurnSeats = [];
    internal ref readonly HashSet<int> ReferenceToRoundTurnSeats => ref _roundTurnSeats;

    ref int Contracts.ICoreActualRoundProgressRoundNumberAccess._roundNumber => ref _roundNumber;

    ref readonly HashSet<int> Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess._roundTurnSeats => ref _roundTurnSeats;

    ref int Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber => ref _roundNumber;

    ref readonly HashSet<int> Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats => ref _roundTurnSeats;
}
