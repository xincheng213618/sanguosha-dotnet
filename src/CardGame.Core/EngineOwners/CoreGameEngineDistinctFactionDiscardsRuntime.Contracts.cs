#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDistinctFactionDiscardsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
