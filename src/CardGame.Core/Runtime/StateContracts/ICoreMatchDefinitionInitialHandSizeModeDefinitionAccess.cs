#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess : IProgramRuntimeOperations
{
    ref readonly ContentModeDefinition _modeDefinition { get; }

    ref int _initialHandSize { get; }
}
