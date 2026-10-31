#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICharacterZhaoYanOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
