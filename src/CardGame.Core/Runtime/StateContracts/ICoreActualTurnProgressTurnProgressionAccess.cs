#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnProgressionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ActualTurnProgression _turnProgression { get; }
}
