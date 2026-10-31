#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentModeDefinition _modeDefinition { get; }

    ref int _initialHandSize { get; }
}
