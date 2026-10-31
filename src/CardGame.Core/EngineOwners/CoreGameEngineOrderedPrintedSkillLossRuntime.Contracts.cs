#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreOrderedPrintedSkillLossCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}
