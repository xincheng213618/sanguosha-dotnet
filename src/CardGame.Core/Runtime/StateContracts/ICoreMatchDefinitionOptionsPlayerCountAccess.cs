#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionOptionsPlayerCountAccess : IProgramRuntimeOperations
{
    ref readonly GameOptions _options { get; }

    ref readonly int _playerCount { get; }
}
