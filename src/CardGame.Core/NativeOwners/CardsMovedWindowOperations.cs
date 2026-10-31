#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowOperations(Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, Func<RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries> getCoreGameEngineDrawFundedDistinctBasicPaymentRuntime, Func<RuntimeEntries.CoreGameEngineDiscardEntries> getCoreGameEngineDiscardRuntime, Func<RuntimeEntries.CoreGameEngineFactionRequestCostRecoveriesEntries> getCoreGameEngineFactionRequestCostRecoveriesRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime) : ICardsMovedWindowOperations
{
    public void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, from, to, reason);
    public void AddLog(string type, string message, int actor, int target) => getCoreMatchLogRuntime().AddLog(type, message, actor, target);
    public void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => getCoreGameEngineDrawFundedDistinctBasicPaymentRuntime().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    public void ContinueManualDiscard(long frameId) => getCoreGameEngineDiscardRuntime().ContinueManualDiscard(frameId);
    public void ContinuePaidFactionRequestCost(long producerId) => getCoreGameEngineFactionRequestCostRecoveriesRuntime().ContinuePaidFactionRequestCost(producerId);
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId, PostEventContinuation continuation) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(resumeFrameId, continuation);
}
