#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterTaiShiCiMobileShiShiTaiShiCiOperations : IProgramRuntimeOperations
{
    ShiTaiShiCiRuntime Get();
}

internal interface ICharacterTaiShiCiMobileShiCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}
