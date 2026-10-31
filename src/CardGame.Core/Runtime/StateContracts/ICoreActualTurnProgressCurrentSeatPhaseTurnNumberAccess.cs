#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess : IProgramRuntimeOperations
{
    ref int _currentSeat { get; }

    ref int _turnNumber { get; }

    ref TurnPhase _phase { get; }
}
