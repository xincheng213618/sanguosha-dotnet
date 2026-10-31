#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _currentSeat { get; }

    ref int _turnNumber { get; }

    ref global::CardGame.Core.TurnPhase _phase { get; }
}
