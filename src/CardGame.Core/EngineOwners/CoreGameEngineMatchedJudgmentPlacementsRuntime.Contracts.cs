#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreMatchedJudgmentPlacementsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
