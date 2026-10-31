#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterPangTongFriendCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterPangTongFriendCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
}
