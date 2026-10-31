#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess : IProgramRuntimeOperations
{
    ref ActualTurnProgression _turnProgression { get; }

    ref int _currentSeat { get; }

    ref int _turnNumber { get; }
}
