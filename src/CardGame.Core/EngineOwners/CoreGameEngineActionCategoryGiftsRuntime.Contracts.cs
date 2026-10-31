#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreActionCategoryGiftsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
