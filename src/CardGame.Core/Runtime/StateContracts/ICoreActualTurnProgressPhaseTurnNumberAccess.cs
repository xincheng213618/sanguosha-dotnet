#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseTurnNumberAccess : IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref TurnPhase _phase { get; }
}
