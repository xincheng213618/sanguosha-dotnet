#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHeQiOlCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICharacterHeQiOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICharacterHeQiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
