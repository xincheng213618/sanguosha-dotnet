#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterYangFuOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterYangFuOlYangFuOperations : IProgramRuntimeOperations
{
    YangFuRuntime Get();
}

internal interface ICharacterYangFuOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
