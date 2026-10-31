#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterGuoTuOlCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterGuoTuOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
