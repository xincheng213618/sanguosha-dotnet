#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhaoZhongOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
