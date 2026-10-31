#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDuanJiongOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
