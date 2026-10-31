#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseTurnNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref global::CardGame.Core.TurnPhase _phase { get; }
}
