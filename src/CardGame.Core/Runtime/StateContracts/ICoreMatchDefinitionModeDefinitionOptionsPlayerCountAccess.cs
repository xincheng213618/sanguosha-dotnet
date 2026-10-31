#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess : IProgramRuntimeOperations
{
    ref readonly ContentModeDefinition _modeDefinition { get; }

    ref readonly GameOptions _options { get; }

    ref readonly int _playerCount { get; }
}
