#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionPlayerCountAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly int _playerCount { get; }
}
