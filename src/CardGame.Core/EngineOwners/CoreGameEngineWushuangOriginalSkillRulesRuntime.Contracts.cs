#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreWushuangOriginalSkillRulesCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
