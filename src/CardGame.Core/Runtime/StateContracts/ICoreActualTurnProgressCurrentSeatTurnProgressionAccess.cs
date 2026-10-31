#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatTurnProgressionAccess : IProgramRuntimeOperations
{
    ref ActualTurnProgression _turnProgression { get; }

    ref int _currentSeat { get; }
}
