#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess : IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref ActualTurnProgression _turnProgression { get; }

    ref TurnPhase _phase { get; }

    ref int _currentSeat { get; }

    ref bool _started { get; }
}
