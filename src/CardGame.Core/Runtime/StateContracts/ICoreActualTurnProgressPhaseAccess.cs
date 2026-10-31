#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseAccess : IProgramRuntimeOperations
{
    ref TurnPhase _phase { get; }
}
