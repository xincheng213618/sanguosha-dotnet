#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualRoundProgressState : global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess
{
    private int _roundNumber = default !;
    internal ref int ReferenceToRoundNumber => ref _roundNumber;

    private global::System.Collections.Generic.HashSet<int> _roundTurnSeats = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToRoundTurnSeats => ref _roundTurnSeats;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess._roundNumber => ref _roundNumber;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess._roundTurnSeats => ref _roundTurnSeats;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber => ref _roundNumber;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats => ref _roundTurnSeats;
}
