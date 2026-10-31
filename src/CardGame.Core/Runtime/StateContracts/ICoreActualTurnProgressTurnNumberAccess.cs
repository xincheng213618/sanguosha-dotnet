#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualTurnProgressTurnNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _turnNumber { get; }
}
