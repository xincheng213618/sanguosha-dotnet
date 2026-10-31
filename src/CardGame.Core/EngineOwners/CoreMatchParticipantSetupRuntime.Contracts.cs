#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreMatchParticipantSetupCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
