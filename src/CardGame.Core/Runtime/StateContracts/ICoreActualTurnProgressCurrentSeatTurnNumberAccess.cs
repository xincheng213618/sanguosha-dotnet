#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatTurnNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref int _currentSeat { get; }
}
