#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDengAiAnniversaryGodCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterDengAiAnniversaryGodCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICharacterDengAiAnniversaryGodCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
