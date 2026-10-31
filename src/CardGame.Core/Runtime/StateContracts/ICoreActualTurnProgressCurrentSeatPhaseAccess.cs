#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseAccess : IProgramRuntimeOperations
{
    ref TurnPhase _phase { get; }

    ref int _currentSeat { get; }
}
