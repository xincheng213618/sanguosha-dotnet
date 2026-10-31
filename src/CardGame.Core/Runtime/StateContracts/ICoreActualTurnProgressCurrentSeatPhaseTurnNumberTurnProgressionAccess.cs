#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.TurnPhase _phase { get; }

    ref int _currentSeat { get; }

    ref int _turnNumber { get; }

    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }
}
