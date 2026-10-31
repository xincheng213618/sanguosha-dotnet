#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCuiLieAnniversaryCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
