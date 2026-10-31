#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess : IProgramRuntimeOperations
{
    ref TurnPhase _phase { get; }

    ref int _currentSeat { get; }

    ref int _turnNumber { get; }

    ref ActualTurnProgression _turnProgression { get; }
}
