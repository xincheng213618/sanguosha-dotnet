#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionOptionsAccess : IProgramRuntimeOperations
{
    ref readonly ContentModeDefinition _modeDefinition { get; }

    ref readonly GameOptions _options { get; }
}
