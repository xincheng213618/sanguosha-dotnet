#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionEntries
{
    internal CardMovementRecord[] CaptureResponseCompletionCosts(CardActionContext a, ProgramCardContinuation continuation, long parentFrameId) => _resolve().CaptureResponseCompletionCosts(a, continuation, parentFrameId);
    internal void ContinueCardResponseAfterCompletion(NativeOwners.CardAttackHandle? attack, ProgramCardTriggerWindowFrame window) => _resolve().ContinueCardResponseAfterCompletion(attack, window);
    internal bool HasCardResponseCompletedObserver(CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasCardResponseCompletedObserver(action, continuation);
    internal bool IsJueshengPaidDuelResponse(CardActionContext action, ProgramCardContinuation continuation) => _resolve().IsJueshengPaidDuelResponse(action, continuation);
    internal bool IsResponseCompletionCostFinished(ProgramCardTriggerWindowFrame window, CardActionCost cost) => _resolve().IsResponseCompletionCostFinished(window, cost);
    internal bool IsResponseCompletionEntry(CardActionContext action, ProgramCardContinuation continuation, long parentFrameId, CardActionCost cost, CardMovementRecord moved) => _resolve().IsResponseCompletionEntry(action, continuation, parentFrameId, cost, moved);
    internal bool IsResponseCompletionProgramDuelParent(ProgramSkillFrame frame, ProgramCardContinuation continuation) => _resolve().IsResponseCompletionProgramDuelParent(frame, continuation);
    internal bool ResponseCompletionFirstChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _resolve().ResponseCompletionFirstChild(window, child);
    internal bool ResponseCompletionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().ResponseCompletionStructuralEdge(parent, child);
    internal bool ReturnCardResponseCompletionMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnCardResponseCompletionMovement(child);
    internal bool TryBeginCardResponseCompleted(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation originalContinuation) => _resolve().TryBeginCardResponseCompleted(attack, action, originalContinuation);
    internal bool TryDrainCardResponseCompletionCosts(ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardResponseCompletionCosts(supplied);
    internal bool ValidResponseCompletionParent(ProgramCardTriggerWindowFrame window) => _resolve().ValidResponseCompletionParent(window);
    internal bool ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame window) => _resolve().ValidResponseCompletionWindow(window);
}
