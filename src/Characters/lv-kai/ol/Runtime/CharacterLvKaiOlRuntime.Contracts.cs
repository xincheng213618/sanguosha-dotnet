#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLvKaiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterLvKaiOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
