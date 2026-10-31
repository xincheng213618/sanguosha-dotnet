#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDuKuiAnniversaryCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterDuKuiAnniversaryCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
