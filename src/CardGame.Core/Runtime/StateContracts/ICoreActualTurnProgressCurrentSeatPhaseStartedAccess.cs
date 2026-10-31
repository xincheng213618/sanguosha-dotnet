#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseStartedAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _started { get; }

    ref int _currentSeat { get; }

    ref global::CardGame.Core.TurnPhase _phase { get; }
}
