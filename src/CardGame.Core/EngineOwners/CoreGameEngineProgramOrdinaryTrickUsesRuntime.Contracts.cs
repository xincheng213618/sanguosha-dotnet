#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramOrdinaryTrickUsesCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICoreProgramOrdinaryTrickUsesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
