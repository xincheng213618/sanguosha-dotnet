#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionPlayerCountAccess : IProgramRuntimeOperations
{
    ref readonly int _playerCount { get; }
}
