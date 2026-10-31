#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations : IProgramRuntimeOperations
{
    ProgramTargetResolverRegistry ProgramTargetResolvers { get; }
}

internal interface ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
