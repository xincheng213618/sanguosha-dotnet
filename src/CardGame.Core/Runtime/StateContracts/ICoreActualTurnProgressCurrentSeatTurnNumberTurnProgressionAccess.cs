#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }

    ref int _currentSeat { get; }

    ref int _turnNumber { get; }
}
