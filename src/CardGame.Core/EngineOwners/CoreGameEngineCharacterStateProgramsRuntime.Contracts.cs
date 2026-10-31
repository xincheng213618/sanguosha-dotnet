#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCharacterStateProgramsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
