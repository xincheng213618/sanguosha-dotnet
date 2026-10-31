#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess : IProgramRuntimeOperations
{
    ref TurnPhase _phase { get; }

    ref ActualTurnProgression _turnProgression { get; }

    ref int _turnNumber { get; }
}
