#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSlashTargetBenefitPaymentsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
