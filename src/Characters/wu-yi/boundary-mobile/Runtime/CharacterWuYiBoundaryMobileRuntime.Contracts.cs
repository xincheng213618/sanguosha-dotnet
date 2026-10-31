#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterWuYiBoundaryMobileCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
