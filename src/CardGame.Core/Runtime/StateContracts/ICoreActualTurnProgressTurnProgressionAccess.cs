#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnProgressionAccess : IProgramRuntimeOperations
{
    ref ActualTurnProgression _turnProgression { get; }
}
