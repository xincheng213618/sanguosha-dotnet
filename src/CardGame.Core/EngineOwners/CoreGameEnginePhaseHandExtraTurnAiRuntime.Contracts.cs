#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
