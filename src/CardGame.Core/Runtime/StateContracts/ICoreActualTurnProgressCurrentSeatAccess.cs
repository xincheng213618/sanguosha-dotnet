#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatAccess : IProgramRuntimeOperations
{
    ref int _currentSeat { get; }
}
