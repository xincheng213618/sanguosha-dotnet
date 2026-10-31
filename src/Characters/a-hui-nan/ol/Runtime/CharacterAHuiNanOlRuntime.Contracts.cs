#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterAHuiNanOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
