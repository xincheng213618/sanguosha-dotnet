#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLiangXingOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
