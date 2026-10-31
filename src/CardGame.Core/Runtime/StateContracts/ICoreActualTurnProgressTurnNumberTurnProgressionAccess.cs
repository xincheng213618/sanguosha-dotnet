#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnNumberTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }
}
