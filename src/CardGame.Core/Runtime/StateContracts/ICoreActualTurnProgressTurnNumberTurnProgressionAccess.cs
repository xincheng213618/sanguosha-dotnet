#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnNumberTurnProgressionAccess : IProgramRuntimeOperations
{
    ref int _turnNumber { get; }

    ref ActualTurnProgression _turnProgression { get; }
}
