#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSuitDamagePreventionBenefitsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
