#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnNumberAccess : IProgramRuntimeOperations
{
    ref int _turnNumber { get; }
}
