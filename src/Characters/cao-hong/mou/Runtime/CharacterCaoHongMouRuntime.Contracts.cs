#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoHongMouCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterCaoHongMouMouCaoHongOperations : IProgramRuntimeOperations
{
    MouCaoHongRuntime Get();
}
