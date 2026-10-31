#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDamageJudgmentSuitPaymentObserversCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
