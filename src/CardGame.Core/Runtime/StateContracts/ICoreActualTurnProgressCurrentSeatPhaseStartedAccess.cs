#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressCurrentSeatPhaseStartedAccess : IProgramRuntimeOperations
{
    ref bool _started { get; }

    ref int _currentSeat { get; }

    ref TurnPhase _phase { get; }
}
