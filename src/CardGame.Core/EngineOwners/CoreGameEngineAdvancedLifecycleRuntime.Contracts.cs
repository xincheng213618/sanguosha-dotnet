#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAdvancedLifecycleCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreAdvancedLifecycleCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
