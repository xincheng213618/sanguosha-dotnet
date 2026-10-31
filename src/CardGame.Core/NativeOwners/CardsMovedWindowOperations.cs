#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries> getCoreGameEngineDrawFundedDistinctBasicPaymentRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardEntries> getCoreGameEngineDiscardRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFactionRequestCostRecoveriesEntries> getCoreGameEngineFactionRequestCostRecoveriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime) : ICardsMovedWindowOperations
{
    public void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, from, to, reason);
    public void AddLog(string type, string message, int actor, int target) => getCoreMatchLogRuntime().AddLog(type, message, actor, target);
    public void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => getCoreGameEngineDrawFundedDistinctBasicPaymentRuntime().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    public void ContinueManualDiscard(long frameId) => getCoreGameEngineDiscardRuntime().ContinueManualDiscard(frameId);
    public void ContinuePaidFactionRequestCost(long producerId) => getCoreGameEngineFactionRequestCostRecoveriesRuntime().ContinuePaidFactionRequestCost(producerId);
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId, PostEventContinuation continuation) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(resumeFrameId, continuation);
}
