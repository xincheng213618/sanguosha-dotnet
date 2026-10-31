#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterTianChouOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
