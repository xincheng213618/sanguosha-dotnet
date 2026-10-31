#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.TurnPhase _phase { get; }
}
