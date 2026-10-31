#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZuoCiHappyGodCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICharacterZuoCiHappyGodProgramPhaseBoundaryOperations : IProgramRuntimeOperations
{
    ProgramPhaseBoundaryRuntime Get();
}
