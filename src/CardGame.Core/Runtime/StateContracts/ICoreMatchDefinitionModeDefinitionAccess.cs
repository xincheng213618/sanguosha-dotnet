#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionAccess : IProgramRuntimeOperations
{
    ref readonly ContentModeDefinition _modeDefinition { get; }
}
