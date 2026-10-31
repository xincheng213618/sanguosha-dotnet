#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionEntries
{
    internal global::CardGame.Core.CardMovementRecord[] CaptureResponseCompletionCosts(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation, long parentFrameId) => _resolve().CaptureResponseCompletionCosts(a, continuation, parentFrameId);
    internal void ContinueCardResponseAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ContinueCardResponseAfterCompletion(attack, window);
    internal bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasCardResponseCompletedObserver(action, continuation);
    internal bool IsJueshengPaidDuelResponse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().IsJueshengPaidDuelResponse(action, continuation);
    internal bool IsResponseCompletionCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost) => _resolve().IsResponseCompletionCostFinished(window, cost);
    internal bool IsResponseCompletionEntry(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord moved) => _resolve().IsResponseCompletionEntry(action, continuation, parentFrameId, cost, moved);
    internal bool IsResponseCompletionProgramDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().IsResponseCompletionProgramDuelParent(frame, continuation);
    internal bool ResponseCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => _resolve().ResponseCompletionFirstChild(window, child);
    internal bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().ResponseCompletionStructuralEdge(parent, child);
    internal bool ReturnCardResponseCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnCardResponseCompletionMovement(child);
    internal bool TryBeginCardResponseCompleted(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation originalContinuation) => _resolve().TryBeginCardResponseCompleted(attack, action, originalContinuation);
    internal bool TryDrainCardResponseCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainCardResponseCompletionCosts(supplied);
    internal bool ValidResponseCompletionParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ValidResponseCompletionParent(window);
    internal bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ValidResponseCompletionWindow(window);
}
