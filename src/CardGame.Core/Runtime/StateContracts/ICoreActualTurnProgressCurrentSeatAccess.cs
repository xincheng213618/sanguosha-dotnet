#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _currentSeat { get; }
}
