#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentModeDefinition _modeDefinition { get; }

    ref readonly global::CardGame.Core.GameOptions _options { get; }

    ref readonly int _playerCount { get; }
}
