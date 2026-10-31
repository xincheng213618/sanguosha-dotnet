#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterGongsunZanMouOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICharacterGongsunZanMouOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
