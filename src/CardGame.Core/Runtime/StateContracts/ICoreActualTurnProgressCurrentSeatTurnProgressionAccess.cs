#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }

    ref int _currentSeat { get; }
}
