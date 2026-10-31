#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreSameTypeActualUseAidCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
