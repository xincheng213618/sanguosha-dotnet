#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionPlayerCountAccess : IProgramRuntimeOperations
{
    ref readonly ContentModeDefinition _modeDefinition { get; }

    ref readonly int _playerCount { get; }
}
