#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.TurnPhase _phase { get; }

    ref int _currentSeat { get; }
}
