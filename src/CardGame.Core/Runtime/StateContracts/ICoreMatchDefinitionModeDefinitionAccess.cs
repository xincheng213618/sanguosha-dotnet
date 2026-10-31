#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionModeDefinitionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentModeDefinition _modeDefinition { get; }
}
