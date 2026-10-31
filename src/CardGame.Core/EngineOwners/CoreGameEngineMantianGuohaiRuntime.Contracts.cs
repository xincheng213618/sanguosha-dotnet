#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreMantianGuohaiCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICoreMantianGuohaiCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
