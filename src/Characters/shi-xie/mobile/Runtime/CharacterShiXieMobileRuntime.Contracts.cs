#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterShiXieMobileCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
