#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionPlayerCountAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentModeDefinition _modeDefinition { get; }

    ref readonly int _playerCount { get; }
}
