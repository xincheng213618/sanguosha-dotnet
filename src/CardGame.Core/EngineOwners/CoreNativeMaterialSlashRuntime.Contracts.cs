#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeMaterialSlashCoreTrustedMovementDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardMovementRecord> CardMovements { get; }
}

internal interface ICoreNativeMaterialSlashCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
