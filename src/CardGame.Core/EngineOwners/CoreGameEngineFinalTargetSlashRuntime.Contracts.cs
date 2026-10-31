#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreFinalTargetSlashCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreFinalTargetSlashCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
