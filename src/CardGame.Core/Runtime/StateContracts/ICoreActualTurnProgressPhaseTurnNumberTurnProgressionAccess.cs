#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.TurnPhase _phase { get; }

    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }

    ref int _turnNumber { get; }
}
