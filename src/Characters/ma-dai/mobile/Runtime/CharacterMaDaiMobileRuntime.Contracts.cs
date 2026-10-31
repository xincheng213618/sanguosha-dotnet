#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterMaDaiMobileCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
