#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreEquipmentDonationProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
