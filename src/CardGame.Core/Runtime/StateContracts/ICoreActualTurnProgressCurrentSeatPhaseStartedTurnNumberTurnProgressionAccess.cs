#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }

    ref global::CardGame.Core.TurnPhase _phase { get; }

    ref int _currentSeat { get; }

    ref bool _started { get; }
}
