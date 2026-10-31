#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreWushuangSkillsBAuditCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
