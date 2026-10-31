#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionOptionsPlayerCountAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.GameOptions _options { get; }

    ref readonly int _playerCount { get; }
}
